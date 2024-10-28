using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using Frontend.HttpRequests;
using System.Reflection;

namespace Frontend.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly HttpClient _httpClient;
        GenericRequests<Vehicles> genericRequestsVehicles = new GenericRequests<Vehicles>();

        public VehiclesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
              var vehicles=  await genericRequestsVehicles.GetListHttpRequest(":5191/api/Vehicles/get-vehicles");
                //string apiUrl = "http://localhost:5191/api/Vehicles/get-vehicles";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<Vehicles>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    return View(model);
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<Vehicles>());
                //}
                return View(vehicles);

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Vehicles brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://localhost:5191/api/Vehicles/create-vehicle", content);
               await  genericRequestsVehicles.PostRequestGeneric(":5191/api/Vehicles/create-vehicle" , brand);
            }


        }
        [HttpPost]
        public async Task Update([FromBody] Vehicles brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://localhost:5191/api/Vehicles/update-vehicle", content);
                 await genericRequestsVehicles.UpdateRequestGeneric(":5191/api/Vehicles/update-vehicle", brand);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
              //  await _httpClient.DeleteAsync("http://localhost:5191/api/Vehicles/delete-vehicle/" + id);
               await genericRequestsVehicles.DeleteRequestGeneric(":5191/api/Vehicles/delete-vehicle/" , id);
            }
        }
    }
}
