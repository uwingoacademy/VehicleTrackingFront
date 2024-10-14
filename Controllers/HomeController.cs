using Frontend.Helper;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            Cound cound = new Cound();
            string apiUrl = "http://78.111.111.81:5191/api/Cound/get-cound";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                cound = System.Text.Json.JsonSerializer.Deserialize<Cound>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return View(cound);
        }
    }
}
