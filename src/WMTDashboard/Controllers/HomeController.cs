using System.Diagnostics;
using System.Linq;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WMTDashboard.Models;
using WMTServer;

namespace WMTDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly WindmillFarm.WindmillFarmClient client;

        public HomeController(ILogger<HomeController> logger, WindmillFarm.WindmillFarmClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public IActionResult Index()
        {
            var authHeader = new Metadata
            {
                {
                    "Authorization",
                    "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJHYWxpY2lhIE5ldENvbmYiLCJpYXQiOjE1Njk1ODM3ODIsImV4cCI6MTYwMTExOTc4MiwiYXVkIjoiZ1JQQ2VycyIsInN1YiI6Im1hbnVlbC52aWxhY2hhbkBleHBlcnRzY29kaW5nLmVzIiwiUm9sZSI6IkFkbWluaXN0cmF0b3IifQ.lIBqaGckTF9moSeMdVepyyc4KL71ubfKzuDI0CQhfJo"
                }
            };

            //var response = client.RequestList(new WindmillListRequest(), authHeader);
            var response = client.RequestList(new WindmillListRequest());
            return View(response.Windmills.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
