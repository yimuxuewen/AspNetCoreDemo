using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService
{
    public static class ConsulManager
    {
        public static void UseConsul(this IApplicationBuilder applicationBuilder,IConfiguration configuration,IConsulClient consulClient)
        {
            RegServer(configuration,consulClient);
        }

        private static void RegServer(IConfiguration configuration, IConsulClient consulClient)
        {
            //服务组名
            string consulGroup=configuration["consulGroup"];
            //服务IP
            string ip = configuration["ip"];
            //服务端口
            int port = int.Parse(configuration["port"]);
            //健康检查端口 docker 映射时使用
            int healthport = 0;
            try
            {
                healthport = int.Parse(configuration["healthport"]);
            }
            catch (Exception)
            {
                healthport = port;
            }
            //服务ID
            string id = $"{consulGroup}_{ip}_{healthport}";
            //服务健康检查
            var check = new AgentCheckRegistration()
            {
                HTTP = $"http://{ip}:{healthport}/Health",
                Interval = TimeSpan.FromSeconds(10),
                Timeout= TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter= TimeSpan.FromSeconds(60)
            };
            //服务注册类
            var regist = new AgentServiceRegistration()
            {
                Name= consulGroup,
                Address=ip,
                Port= healthport,
                ID=id,
                Check= check
            };

            //注册服务
            consulClient.Agent.ServiceRegister(regist);
        }
    }
}
