using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;

namespace Frontend.Controllers
{
    public class PacketContentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PacketContentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // paketleri de çek tupple yap
                string apiUrl = "https://localhost:7299/api/PacketContent/get-packetcontent";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiUrl2 = "https://localhost:7299/api/Packets/get-packet";
                var response2 = await _httpClient.GetAsync(apiUrl2);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<List<PacketContent>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    var jsonResponse2 = await response2.Content.ReadAsStringAsync();
                    var model2 = System.Text.Json.JsonSerializer.Deserialize<List<Packets>>(jsonResponse2, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(Tuple.Create(model,model2));
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                    return View(new List<PacketContent>());
                }

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] PacketContent brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7299/api/PacketContent/create-packetcontent", content);

            }


        }
        [HttpPost]
        public async Task Update([FromBody] PacketContent brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PutAsync("https://localhost:7299/api/PacketContent/update-packetcontent", content);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("https://localhost:7299/api/PacketContent/delete-packetcontent/" + id);
            }
        }
    }
}
