using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemo.Controllers
{
    [ApiController]
    [Route("/user/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        static int _iseed = 0;
        private static int iSeed
        {
            get
            {
                return _iseed;
            }
            set
            {
                if (value > 999999)
                {
                    _iseed = 0;
                }
                else
                {
                    _iseed = value;
                }
            }
        }

        private IConfiguration _configuration;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //Console.WriteLine($"WeatherForecast调用的服务端口：{ _configuration["port"]}");
            #region Consul
            string url = "http://productservice/product/WeatherForecast";
            Uri uri = new Uri(url);
            string groupName = uri.Host;
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://192.168.19.130:11500/");
                c.Datacenter = "dc1";
            });
            var response = client.Agent.Services().Result.Response;
            ///找到服务名为groupName的所有服务
            var serviceDict = response.Values.Where(m => m.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();
            #region 负载均衡
            AgentService agentService = null;
            //平均分配法
            {
                agentService = serviceDict[new Random(iSeed++).Next(0, serviceDict.Length)];
            }
            //轮询分配法
            {
                //agentService = serviceDict[iSeed++ % serviceDict.Length];
            }
            //权重分配法
            {
                //List<AgentService> pairsList = new List<AgentService>();
                //foreach (var pair in serviceDict)
                //{
                //    int count = int.Parse(pair.Tags?[0]);
                //    for (int i = 0; i < count; i++)
                //    {
                //        pairsList.Add(pair);
                //    }
                //}
                //agentService = pairsList[new Random(iSeed++).Next(0, pairsList.Count)];
            }
            #endregion

            string baseurl = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
            Console.WriteLine(baseurl);
            string content = WebApiHelper.InvokeApi(baseurl);
            Console.WriteLine(content);
            #endregion

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Port=_configuration["port"]
            })
            .ToArray();
        }
    }
}
