using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using Frontend.HttpRequests;

namespace Frontend.Controllers
{
    public class PeriodicMaintenanceController : Controller
    {
        private readonly HttpClient _httpClient;
        GenericRequests<PeriodicMaintenance> genericRequestsPeriod = new GenericRequests<PeriodicMaintenance>();

        public PeriodicMaintenanceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
             var period =  await genericRequestsPeriod.GetListHttpRequest(":5191/api/PeriodicMaintenance/get-periodicmaintenance");
                GenericRequests<Vehicles> genericRequestsVehicles = new GenericRequests<Vehicles>();
               var vehicle = await genericRequestsVehicles.GetListHttpRequest(":5191/api/Vehicles/get-vehicles");
                //string apiUrl = "http://localhost:5191/api/PeriodicMaintenance/get-periodicmaintenance";
                //var response = await _httpClient.GetAsync(apiUrl);
                //string apiUrlVehicle = "http://localhost:5191/api/Vehicles/get-vehicles";
                //var responseVehicle = await _httpClient.GetAsync(apiUrlVehicle);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<PeriodicMaintenance>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    var jsonResponseVehicle = await responseVehicle.Content.ReadAsStringAsync();
                //    var modelPacket = System.Text.Json.JsonSerializer.Deserialize<List<Vehicles>>(jsonResponseVehicle, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //  var newModel = vehicle.Where(x => !period.Any(packet => packet.VehicleId == x.VehicleId)).ToList();
                //    return View(Tuple.Create(period, vehicle, newModel));
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<Devices>());
                //}
                var newModel = vehicle.Where(x => !period.Any(packet => packet.VehicleId == x.VehicleId)).ToList();
                return View(Tuple.Create(period, vehicle, newModel));
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
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://localhost:5191/api/PeriodicMaintenance/create-periodicmaintenance", content);
                await genericRequestsPeriod.PostRequestGeneric(":5191/api/PeriodicMaintenance/create-periodicmaintenance", brand);
            }


        }
        [HttpPost]
        public async Task Update([FromBody] PeriodicMaintenance brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://localhost:5191/api/PeriodicMaintenance/update-periodicmaintenance", content);
               await genericRequestsPeriod.UpdateRequestGeneric(":5191/api/PeriodicMaintenance/update-periodicmaintenance", brand);
            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
              //  await _httpClient.DeleteAsync("http://localhost:5191/api/PeriodicMaintenance/delete-periodicmaintenance/" + id);
                await genericRequestsPeriod.DeleteRequestGeneric(":5191/api/PeriodicMaintenance/delete-periodicmaintenance/" , id);
            }
        }
    }
}
