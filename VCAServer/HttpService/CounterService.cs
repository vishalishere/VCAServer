using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using VCA.Client.HttpService;
using VCACommon;
using System.Collections.Concurrent;

namespace VCAServer
{
    public class CounterService : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private BlockingCollection<Counter> _queue = new BlockingCollection<Counter>(new ConcurrentBag<Counter>());
        private Dictionary<string, int> _counterMap = new Dictionary<string, int>();
        private HttpClient _http;
        private object queuelock = new object();
        public CounterService()
        {
            _http = new HttpClient();
            _http.BaseAddress =new Uri(AppConfig.Server);
            _http.Timeout = TimeSpan.FromSeconds(15);
            Task.Factory.StartNew(() => 
            {
                PostDataLoop();
            }, 
            TaskCreationOptions.LongRunning); 
        }


        public void Add(vca vca)
        {
            if (vca.counts == null || vca.counts.Length == 0)
                return;
          
            foreach (var counter in vca.counts)
            {
                
                bool valueChanged = true;
                if(!_counterMap.ContainsKey(counter.name))
                {
                    _counterMap.Add(counter.name, counter.val);
                }
                else if (_counterMap[counter.name] == counter.val)
                {
                    valueChanged = false;
                }
                else
                {
                    _counterMap[counter.name] = counter.val;
                }
              
                if(valueChanged)
                {
                    log.Debug("Add Counter: " + counter.name + " from: " + vca.cam_ip);
                    _queue.Add(new Counter
                    {
                        Name = counter.name,
                        Value = counter.val,
                        DateTime = DateTime.Now
                    });
                }
            }
        }

        private void PostDataLoop()
        {
            while (true)
            {
                try
                {
                    Counter counter = _queue.Take();
                    SaveEventToServerAsync(counter).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }


        public async Task SaveEventToServerAsync(Counter input)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(input), new UTF8Encoding(), "application/json");
                var response = await _http.PostAsync(AppConfig.CounterApi, content);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception tce)
            {
                Console.WriteLine(tce.Message);
            }
        }

        public void Dispose()
        {
            _queue.CompleteAdding();
            _http.Dispose();

        }
    }
}
