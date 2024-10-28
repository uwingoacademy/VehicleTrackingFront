using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using Frontend.HttpRequests;

namespace Frontend.Controllers
{
    public class DriverVehicleController : Controller
    {
        private readonly HttpClient _httpClient; 
        GenericRequests<DriverVehicle> genericRequestsDriverVehicle = new GenericRequests<DriverVehicle>();

        public DriverVehicleController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            GenericRequests<Drivers> genericRequestsDrivers = new GenericRequests<Drivers>();
           var drivers = await genericRequestsDrivers.GetListHttpRequest(":5191/api/Drivers/get-driver");
            var driverVehicles = await  genericRequestsDriverVehicle.GetListHttpRequest(":5191/api/DriverVehicle/get-drivervehicle");
            GenericRequests<Vehicles> genericRequestsVehicles = new GenericRequests<Vehicles>();
           var vehicles = await genericRequestsVehicles.GetListHttpRequest(":5191/api/Vehicles/get-vehicles");
            // List<Drivers> drivers = new List<Drivers>();
          //  List<Vehicles> vehicles = new List<Vehicles>();
           // List<DriverVehicle> driverVehicles = new List<DriverVehicle>();
            try
            {
                //string apiUrlDriver = "http://localhost:5191/api/Drivers/get-driver";
                //var responseDriver = await _httpClient.GetAsync(apiUrlDriver);
                //string apiUrlVehicle = "http://localhost:5191/api/Vehicles/get-vehicles";
                //var responseVehicle = await _httpClient.GetAsync(apiUrlVehicle);
                ////sürücüleri çek
                ////araçları çek
                ////aracabağlı sürücüleri çek
                //string apiUrl = "http://localhost:5191/api/DriverVehicle/get-drivervehicle";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    driverVehicles = System.Text.Json.JsonSerializer.Deserialize<List<DriverVehicle>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //}


                ////Vehicle 

                //if (responseVehicle.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await responseVehicle.Content.ReadAsStringAsync();
                //    vehicles = System.Text.Json.JsonSerializer.Deserialize<List<Vehicles>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //}

                ////driver
                //if (responseDriver.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await responseDriver.Content.ReadAsStringAsync();
                //    drivers = System.Text.Json.JsonSerializer.Deserialize<List<Drivers>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<DriverVehicle>());
                //}
                //araçlarda IsThereDriver false olanları bul yeni bir obje olarak bas
                //sürücülerde gelenleri araca bağlı sürücüler içerisinde ara eşleşmeyenleri çek yeni bir obje olarak bas
                var newVehicle = vehicles.Where(x => x.IsThereDriver == false).ToList();
                var driversTermin= driverVehicles.Where(x => x.TerminationDate == null).ToList();
                var unmatchedVehicles = drivers.Where(drivers => !driversTermin.Any(driverVehicle => driverVehicle.DriversId==drivers.DriverId)).ToList();
                return View(Tuple.Create(driverVehicles, drivers, vehicles, unmatchedVehicles, newVehicle));
            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] DriverVehicle brand)
        { //Seçilen aracın sürücü kısmını true olarak güncelle
            if (ModelState.IsValid)
            {
                brand.IdentificationDate = DateTime.Now;
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://localhost:5191/api/DriverVehicle/create-drivervehicle", content);
               await genericRequestsDriverVehicle.PostRequestGeneric(":5191/api/DriverVehicle/create-drivervehicle", brand);
            }


        }
        [HttpPost]
        public async Task Update([FromBody] DriverVehicle brand)
        {
           if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://localhost:5191/api/DriverVehicle/update-drivervehicle", content);
                await genericRequestsDriverVehicle.UpdateRequestGeneric(":5191/api/DriverVehicle/update-drivervehicle", brand);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
              //  await _httpClient.DeleteAsync("http://localhost:5191/api/DriverVehicle/delete-drivervehicle/" + id);
               await genericRequestsDriverVehicle.DeleteRequestGeneric(":5191/api/DriverVehicle/delete-drivervehicle/" , id);
            }
        }
    }
}
