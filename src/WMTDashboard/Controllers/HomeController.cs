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

                    //Role Other
                    //"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJFeHBlcnRzQ29kaW5nIERlbW9zIiwiaWF0IjoxNTc0ODE1MTg3LCJleHAiOjE2MDY0Mzc4MDgsImF1ZCI6ImdSUENlcnMiLCJzdWIiOiJtYW51ZWwudmlsYWNoYW5AZXhwZXJ0c2NvZGluZy5lcyIsIlJvbGUiOiJPdGhlciJ9.Wm79O1SMqsPHI-QpWbOdgxZP_-Abe2U1SpqXEwwVab0"

                    //Role User
                    "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJFeHBlcnRzQ29kaW5nIERlbW9zIiwiaWF0IjoxNTc0ODE1MTg3LCJleHAiOjE2MDY0Mzc4MDgsImF1ZCI6ImdSUENlcnMiLCJzdWIiOiJtYW51ZWwudmlsYWNoYW5AZXhwZXJ0c2NvZGluZy5lcyIsIlJvbGUiOiJVc2VyIn0.EFYPLRt9STgu46ZkdQ9yGq5seTAZOsf9N71fMtPpYhk"

                    //Role Administrator
                    //"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJFeHBlcnRzQ29kaW5nIERlbW9zIiwiaWF0IjoxNTc0ODE1MTg3LCJleHAiOjE2MDY0Mzc4MDgsImF1ZCI6ImdSUENlcnMiLCJzdWIiOiJtYW51ZWwudmlsYWNoYW5AZXhwZXJ0c2NvZGluZy5lcyIsIlJvbGUiOiJBZG1pbmlzdHJhdG9yIn0.pJOfeoAjypIel1EHBHxzSVMBgilQbqBt4gZ6mSXlao8"
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
