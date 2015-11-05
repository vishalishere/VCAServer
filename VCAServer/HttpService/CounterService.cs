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

namespace VCAServer
{
    public class CounterService : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpClient _http;
        public CounterService()
        {
            _http = new HttpClient();
            _http.BaseAddress =new Uri(AppConfig.Server);
            _http.Timeout = TimeSpan.FromSeconds(15);
        }

        public async Task<JsonMessage> SaveEventToServerAsync(Counter input)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(input), new UTF8Encoding(), "application/json");
                var response = await _http.PostAsync(AppConfig.CounterApi, content);

                response.EnsureSuccessStatusCode();
                var resultJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonMessage>(resultJson);
            }
            catch (HttpRequestException hre)
            {
                log.Warn(new Exception("HttpRequestException", hre));
            }
            catch (Exception tce)
            {
                log.Warn(tce.StackTrace);
            }
            return null;
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
