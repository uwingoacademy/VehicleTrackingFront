using Frontend.Helper;
using Frontend.HttpRequests;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        GenericRequests<Cound> genericRequests = new GenericRequests<Cound>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var responseApi =   await genericRequests.GetHttpRequest(":5191/api/Cound/get-cound");
            return View(responseApi);
        }
    }
}
