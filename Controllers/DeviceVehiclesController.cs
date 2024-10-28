using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Frontend.HttpRequests;

namespace Frontend.Controllers
{
    public class DeviceVehiclesController : Controller
    {
        private readonly HttpClient _httpClient;
        GenericRequests<DeviceVehicles> genericRequestsDeviceVehicles = new GenericRequests<DeviceVehicles>();

        public DeviceVehiclesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            //boşta vehicle ve boşta deviceler 

            try
            {

                GenericRequests<Vehicles> genericRequestsVehicle = new GenericRequests<Vehicles>();
                var vehicles = await  genericRequestsVehicle.GetListHttpRequest(":5191/api/Vehicles/get-vehicles");
                GenericRequests<Devices> genericRequestsDevice = new GenericRequests<Devices>();
                var devices= await genericRequestsDevice.GetListHttpRequest(":5191/api/Devices/get-device");
                var deviceVehicles =   await genericRequestsDeviceVehicles.GetListHttpRequest(":5191/api/DeviceVehicles/get-devicevehicle");
               // List<Devices> devices = new List<Devices>();
                //List<Vehicles> vehicles = new List<Vehicles>();
                //List<DeviceVehicles> deviceVehicles = new List<DeviceVehicles>();
                ////vehicle
                //string apiUrl = "http://78.111.111.81:5191/api/Vehicles/get-vehicles";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    vehicles = System.Text.Json.JsonSerializer.Deserialize<List<Vehicles>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //}
                ////Device
                //string apiUrlDevice = "http://78.111.111.81:5191/api/Devices/get-device";
                //var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await responseDevice.Content.ReadAsStringAsync();
                //    devices = System.Text.Json.JsonSerializer.Deserialize<List<Devices>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //}
                //string apiUrlDeviceVehicle = "http://78.111.111.81:5191/api/DeviceVehicles/get-devicevehicle";
                //var responseDeviceVehicle = await _httpClient.GetAsync(apiUrlDeviceVehicle);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await responseDeviceVehicle.Content.ReadAsStringAsync();
                //    deviceVehicles = System.Text.Json.JsonSerializer.Deserialize<List<DeviceVehicles>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });

                //}

                var newDevice = devices.Where(x => x.IsConnectedVehicles == false).ToList();
                var deviceTermin = deviceVehicles.Where(x => x.RemoveDate == null).ToList();
                var unmatchedVehicles = vehicles.Where(vehicle => !deviceTermin.Any(deviceVehicle => deviceVehicle.VehicleId == vehicle.VehicleId)).ToList();
                return View(Tuple.Create(deviceVehicles, devices, vehicles , newDevice, unmatchedVehicles));
            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] DeviceVehicles brand)
        {// seçilen cihazın bağlı durumunu true olarak ata
            if (ModelState.IsValid)
            {
                brand.InstallDate = DateTime.Now;
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync("http://78.111.111.81:5191/api/DeviceVehicles/create-devicevehicle", content);
                await  genericRequestsDeviceVehicles.PostRequestGeneric(":5191/api/DeviceVehicles/create-devicevehicle", brand);
            }
        }
        [HttpPost]
        public async Task Update([FromBody] DeviceVehicles brand)
        { 
           
            if (ModelState.IsValid)
            {
                //var jsonData = JsonConvert.SerializeObject(brand);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                //await _httpClient.PutAsync("http://78.111.111.81:5191/api/DeviceVehicles/update-devicevehicle", content);
               await genericRequestsDeviceVehicles.UpdateRequestGeneric(":5191/api/DeviceVehicles/update-devicevehicle", brand);
            }
        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
               // await _httpClient.DeleteAsync("http://78.111.111.81:5191/api/DeviceVehicles/delete-devicevehicle/" + id);
                await genericRequestsDeviceVehicles.DeleteRequestGeneric(":5191/api/DeviceVehicles/delete-devicevehicle/", id);
            }
        }
    }
}
