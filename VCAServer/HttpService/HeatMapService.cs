using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VCACommon;

namespace VCAServer.HttpService
{
    public class HeatMapService : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private BlockingCollection<HeatPoint> _queue = new BlockingCollection<HeatPoint>(new ConcurrentBag<HeatPoint>());
        private Dictionary<string, int> _counterMap = new Dictionary<string, int>();
        private HttpClient _http;

        private DateTime lastAdd = DateTime.Now;
        public HeatMapService()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri(AppConfig.Server);
            _http.Timeout = TimeSpan.FromSeconds(15);
            Task.Factory.StartNew(() =>
            {
                PostDataLoop();
            },
            TaskCreationOptions.LongRunning);
        }


        public void Add(vca vca)
        {
            if (vca.objects == null || vca.objects.Length == 0)
                return;
            if (DateTime.Now.Subtract(lastAdd).TotalMinutes < 10)
                return;

            foreach (var box in vca.objects)
            {
                HeatPoint heatPoint = new HeatPoint();
                heatPoint.CreateTime = DateTime.Now;
                heatPoint.From = vca.cam_ip;
                var bb = box.bb;
                heatPoint.X = (bb.x + bb.w / 2) / 65535.0f;
                heatPoint.Y = (bb.y + bb.h / 2) / 65535.0f;
                _queue.Add(heatPoint);
                
            }
            lastAdd = DateTime.Now;
        }

        private void PostDataLoop()
        {
            while (true)
            {
                try
                {
                    HeatPoint heatPoint = _queue.Take();
                    SaveHeatPointServerAsync(heatPoint).GetAwaiter().GetResult();

                }
                catch (Exception)
                {
                    break;
                }
            }
        }


        public async Task SaveHeatPointServerAsync(HeatPoint input)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(input), new UTF8Encoding(), "application/json");
                var response = await _http.PostAsync(AppConfig.HeatPointApi, content);

                response.EnsureSuccessStatusCode();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine(ae.InnerException.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            _queue.CompleteAdding();
            _http.Dispose();

        }
    }
}
