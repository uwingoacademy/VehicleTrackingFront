using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace Frontend.Controllers
{
    public class DeviceVehiclesController : Controller
    {
        private readonly HttpClient _httpClient;

        public DeviceVehiclesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            //boşta vehicle ve boşta deviceler 

            try
            {
                List<Devices> devices = new List<Devices>();
                List<Vehicles> vehicles = new List<Vehicles>();
                List<DeviceVehicles> deviceVehicles = new List<DeviceVehicles>();
                //vehicle
                string apiUrl = "http://78.111.111.81:5191/api/Vehicles/get-vehicles";
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    vehicles = System.Text.Json.JsonSerializer.Deserialize<List<Vehicles>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                //Device
                string apiUrlDevice = "http://78.111.111.81:5191/api/Devices/get-device";
                var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await responseDevice.Content.ReadAsStringAsync();
                    devices = System.Text.Json.JsonSerializer.Deserialize<List<Devices>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                string apiUrlDeviceVehicle = "http://78.111.111.81:5191/api/DeviceVehicles/get-devicevehicle";
                var responseDeviceVehicle = await _httpClient.GetAsync(apiUrlDeviceVehicle);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await responseDeviceVehicle.Content.ReadAsStringAsync();
                    deviceVehicles = System.Text.Json.JsonSerializer.Deserialize<List<DeviceVehicles>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                }

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
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("http://78.111.111.81:5191/api/DeviceVehicles/create-devicevehicle", content);

            }
        }
        [HttpPost]
        public async Task Update([FromBody] DeviceVehicles brand)
        { 
            //update durumunda bağlı cihaz değişti ise cihazın bağlı durumunu update yap 
            //update durumunda yeni bir veri gibi bu veriyi create yap (id yi 0a çekerek) buradan gelen idli veriyi ise güncelle ve remove date now ekle 
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PutAsync("http://78.111.111.81:5191/api/DeviceVehicles/update-devicevehicle", content);
            }
        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        { //delete durumunda bu idye sahip veride bulunan cihazın bağlı durumunu false çek update yap ardından bu id veriyi sil
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("http://78.111.111.81:5191/api/DeviceVehicles/delete-devicevehicle/" + id);
            }
        }

        public IActionResult GetVehicles()
        {
            var vehicles = new List<Vehicles>
        {
            new Vehicles { VehicleId = 1, Plate = "35AG9685" },
            new Vehicles { VehicleId = 2, Plate = "34ABC123" },
            new Vehicles { VehicleId = 3, Plate = "06DEF456" }
        };

            return Json(vehicles);
        }
    }
}
