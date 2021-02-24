using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Untility
{
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        static Dictionary<string, List<DateTime>> dict = new Dictionary<string, List<DateTime>>();
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string key = $"{context.HttpContext.Connection.RemoteIpAddress.ToString()}_{context.HttpContext.Connection.RemotePort}";
            if (dict.ContainsKey(key))
            {
                var list = dict[key].FindAll(m => m > m.AddMinutes(-1));
                Console.WriteLine($"list.Count:{list.Count}");
                if (list.Count>=5) 
                {
                    context.Result = new JsonResult("访问过于频繁，请稍后再试") { StatusCode = 403 };
                }
                else
                {
                    dict[key].Add(DateTime.Now);
                    int count = dict[key].Count - list.Count;
                    Console.WriteLine($"count:{count}");
                    dict[key].RemoveRange(0, count-1);
                }
            }
            else
            {
                List<DateTime> dt = new List<DateTime>();
                dt.Add(DateTime.Now);
                dict.Add(key,dt);
            }
        }
    }
}
