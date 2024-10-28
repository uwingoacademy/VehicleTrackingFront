using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using Frontend.HttpRequests;
using System.Reflection;

namespace Frontend.Controllers
{
    public class PacketController : Controller
    {
        private readonly HttpClient _httpClient;
        GenericRequests<Packets> genericRequestsPacket = new GenericRequests<Packets>();
        public PacketController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
            var packets =  await genericRequestsPacket.GetListHttpRequest(":5191/api/Packets/get-packet");
                //string apiUrl = "http://localhost:5191/api/Packets/get-packet";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<Packets>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    return View(model);
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<Packets>());
                //}
                return View(packets);
            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Packets brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://localhost:5191/api/Packets/create-packet", content);
                await genericRequestsPacket.PostRequestGeneric(":5191/api/Packets/create-packet", brand);

            }


        }
        [HttpPost]
        public async Task Update([FromBody] Packets brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://localhost:5191/api/Packets/update-packet", content);
               await genericRequestsPacket.UpdateRequestGeneric(":5191/api/Packets/update-packet" , brand);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
              //  await _httpClient.DeleteAsync("http://localhost:5191/api/Packets/delete-driver/" + id);
               await genericRequestsPacket.DeleteRequestGeneric(":5191/api/Packets/delete-driver/" , id);
            }
        }
    }
}
