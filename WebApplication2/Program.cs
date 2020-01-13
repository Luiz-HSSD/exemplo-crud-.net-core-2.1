using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;
using System.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionstring = "Data Source=DESKTOP-VK3OR7N;Initial Catalog=projeto1;Integrated Security=True";

            //var optionsBuilder = new DbContextOptionsBuilder<WebApplication2Context>();
            //optionsBuilder.UseSqlServer(connectionstring);
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            //WebApplication2Context _context = new WebApplication2Context(connectionstring);
/*
            for (int i = 0; i < 1000000; i++)
            {
                _context.Fruit.Add(new Fruit() { Nome = "Teste" });
                _context.SaveChanges();
            }
            _context.SaveChanges();
*/
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
