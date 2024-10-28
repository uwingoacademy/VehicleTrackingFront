using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System.Reflection;
using Frontend.HttpRequests;

namespace Frontend.Controllers
{
    public class DriversController : Controller
    {
        private readonly HttpClient _httpClient;
        GenericRequests<Drivers> genericRequestsDrivers = new GenericRequests<Drivers>();

        public DriversController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var drivers = await genericRequestsDrivers.GetListHttpRequest(":5191/api/Drivers/get-driver");
                //string apiUrl = "http://localhost:5191/api/Drivers/get-driver";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<Drivers>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    return View(model);
                //}
                return View(drivers);
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<Drivers>());
                //}

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Drivers brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://localhost:5191/api/Drivers/create-driver", content);
               await genericRequestsDrivers.PostRequestGeneric(":5191/api/Drivers/create-driver", brand);
            }


        }
        [HttpPost]
        public async Task Update([FromBody] Drivers brand)
        {
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://localhost:5191/api/Drivers/update-driver", content);
               await genericRequestsDrivers.UpdateRequestGeneric(":5191/api/Drivers/update-driver", brand);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("http://localhost:5191/api/Drivers/delete-driver/" + id);
             // await genericRequestsDrivers.DeleteRequestGeneric(":5191/api/Drivers/delete-driver/" , id);
            }
        }
    }
}
