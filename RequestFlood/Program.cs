using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RequestFlood
{
    class Program
    {
        public static void Main(string[] args)
        {
            var requestInfo = new RequestInfo()
            {
                Url = "http://localhost:23641/",
                Parameters = new Dictionary<string, string>
                {
                    {"Param1","1" },
                    { "Param2","laksjdflksdfj"}
                }
            };

            var userCount = 100;
            int iterationCount = 10;
            var sessionResults = Enumerable
                .Range(1, iterationCount)
                .Select(i => Task.WhenAll( Enumerable
                    .Range(1, userCount)
                    .Select(i1 => new HttpClient()
                        .PostAsync(requestInfo.Url, new StringContent(JsonConvert.SerializeObject(requestInfo.Parameters))))
                    .ToArray()))
                .ToArray();
            var materializedResults = Task.WhenAll(sessionResults);
            materializedResults.Wait();


        }
    }

    public class RequestInfo
    {
        public string Url { get; set; }
        public string Verb { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
