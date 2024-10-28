using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Reflection;
using Frontend.HttpRequests;

namespace Frontend.Controllers
{
    public class DevicesController : Controller
    {
        private readonly HttpClient _httpClient;
        GenericRequests<Devices> genericRequestsDevices = new GenericRequests<Devices>();

        public DevicesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        { 
            try
            {
              var devicesList  = await genericRequestsDevices.GetListHttpRequest(":5191/api/Devices/get-device");
                GenericRequests<Packets> genericRequestsPackets = new GenericRequests<Packets>();
              var pakets=  await genericRequestsPackets.GetListHttpRequest(":5191/api/Packets/get-packet");
                //string apiUrl = "http://78.111.111.81:5191/api/Devices/get-device";
                //var response = await _httpClient.GetAsync(apiUrl);
                //string apiUrlPacket = "http://78.111.111.81:5191/api/Packets/get-packet";
                //var responsePacket = await _httpClient.GetAsync(apiUrlPacket);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<Devices>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    var jsonResponsePacket = await responsePacket.Content.ReadAsStringAsync();
                //    var modelPacket = System.Text.Json.JsonSerializer.Deserialize<List<Packets>>(jsonResponsePacket, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    return View(Tuple.Create(devicesList, pakets));
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<Devices>());
                //}
                return View(Tuple.Create(devicesList, pakets));
            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Devices brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://78.111.111.81:5191/api/Devices/create-device", content);

               await genericRequestsDevices.PostRequestGeneric(":5191/api/Devices/create-device",brand);
            }


        }
        [HttpPost]
        public async Task Update([FromBody] Devices brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://78.111.111.81:5191/api/Devices/update-device", content);
                 await genericRequestsDevices.UpdateRequestGeneric(":5191/api/Devices/update-device", brand);
            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
               // await _httpClient.DeleteAsync("http://78.111.111.81:5191/api/Devices/delete-device/" + id);
                await genericRequestsDevices.DeleteRequestGeneric(":5191/api/Devices/delete-device/",id);
            }
        }
    }
}
