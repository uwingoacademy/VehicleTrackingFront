using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;

namespace Frontend.Controllers
{
    public class PeriodicMaintenanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public PeriodicMaintenanceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                string apiUrl = "https://localhost:7299/api/PeriodicMaintenance/get-periodicmaintenance";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiUrlVehicle = "https://localhost:7299/api/Vehicles/get-vehicles";
                var responseVehicle = await _httpClient.GetAsync(apiUrlVehicle);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<List<PeriodicMaintenance>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    var jsonResponseVehicle = await responseVehicle.Content.ReadAsStringAsync();
                    var modelPacket = System.Text.Json.JsonSerializer.Deserialize<List<Vehicles>>(jsonResponseVehicle, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                  var newModel = modelPacket.Where(x => !model.Any(packet => packet.VehicleId == x.VehicleId)).ToList();
                    return View(Tuple.Create(model, modelPacket , newModel));
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                    return View(new List<Devices>());
                }

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] PeriodicMaintenance brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7299/api/PeriodicMaintenance/create-periodicmaintenance", content);

            }


        }
        [HttpPost]
        public async Task Update([FromBody] PeriodicMaintenance brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PutAsync("https://localhost:7299/api/PeriodicMaintenance/update-periodicmaintenance", content);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("https://localhost:7299/api/PeriodicMaintenance/delete-periodicmaintenance/" + id);
            }
        }
    }
}
