using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace Frontend.Controllers
{
    public class TarckingDataAccController : Controller
    {
        private readonly HttpClient _httpClient;
        private static List<DeviceVehicles>? last;
      //  List<DeviceVehicles> last = new List<DeviceVehicles>();
        public TarckingDataAccController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<IActionResult> GetById(int? id) 
        
        {
            List<TrackingDataForACC> r = new List<TrackingDataForACC> { new TrackingDataForACC
        {
            AccelerometerDataId = 1,
            DeviceId = 1001,
            Timestamp = DateTime.Now,
            AccX = 0.12m,
            SerialNumber = 'A', // Tek bir karakterli değer
            AccY = 0.34m,
            AccZ = 0.56m,
            DateTime = DateTime.Now,
            Latitude = 37.7749m,
            NS = 'N',
            Longitude = -122.4194m,
            EW = 'W',
            Altitude = 30.5m,
            Speed = 15.7m,
            COG = 90.0m,
            IGN = false
        } };
            return Ok(r);
            
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TrackingDataViewModel> trackingDataViewModels = new List<TrackingDataViewModel>();
               last=   await  GetActiveDevice();
               // List<DeviceVehicles> last = new List<DeviceVehicles>();
                //string apiUrl = "http://localhost:5191/api/DeviceVehicles/get-activedevice";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response != null)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    last = System.Text.Json.JsonSerializer.Deserialize<List<DeviceVehicles>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //}
                foreach (var item in last) {
                    
                    string apiUrlDevice = $"http://localhost:5191/api/Devices/getby-device/{item.DeviceId}";
                    var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                    var jsonResponseDevice = await responseDevice.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<Devices>(jsonResponseDevice, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                //getbysrnmongo
                
                    TrackingDataViewModel trackingDataViewModel = new TrackingDataViewModel();
                    string apiUrlTracking = $"http://localhost:5007/WeatherForecast/getby-srnlast?serialNumber={model.SerialNumber}";
                    var responseTracking = await _httpClient.GetAsync(apiUrlTracking);
                    var jsonResponseTracking = await responseTracking.Content.ReadAsStringAsync();
                    var modelTracking = System.Text.Json.JsonSerializer.Deserialize<TrackingDataForStd>(jsonResponseTracking, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    trackingDataViewModel.trackingDataForStd = modelTracking;

                    //getbyplate
                    string apiUrlVehicle = $"http://localhost:5191/api/Vehicles/getby-vehicle/{item.VehicleId}";
                    var responseVehicle = await _httpClient.GetAsync(apiUrlVehicle);
                    if (responseVehicle.IsSuccessStatusCode)
                    {
                        var jsonResponseVehicle = await responseVehicle.Content.ReadAsStringAsync();
                        var modelVehicle = System.Text.Json.JsonSerializer.Deserialize<Vehicles>(jsonResponseVehicle, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        trackingDataViewModel.vehicles = modelVehicle;
                    }
                    trackingDataViewModels.Add(trackingDataViewModel);
                }
                return Ok(trackingDataViewModels);
                //string apiUrl = "https://localhost:7215/api/TrackingDataForAcc/GetAllTrackingDataForAcc";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<TrackingDataForACC>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    return View(model);
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<TrackingDataForACC>());
                //}

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        public async Task<IActionResult> TrackingIndex() {
            List<Devices> devices =  new List<Devices>();
            List<Vehicles> vehicleActive = new List<Vehicles>();
            List<TrackingDataForStd> trackingdata = new List<TrackingDataForStd>();
            last = await GetActiveDevice();
            foreach (var item in last) {
                //Aktif araçları Vehicle getir
                string apiUrlVehicle = $"http://localhost:5191/api/Vehicles/getby-vehicle/{item.VehicleId}";
                    var responseVehicle = await _httpClient.GetAsync(apiUrlVehicle);
                    if (responseVehicle.IsSuccessStatusCode)
                    {
                        var jsonResponseVehicle = await responseVehicle.Content.ReadAsStringAsync();
                         var modelVehicle = System.Text.Json.JsonSerializer.Deserialize<Vehicles>(jsonResponseVehicle, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        if (modelVehicle is not null)
                            vehicleActive.Add(modelVehicle);
                    }
                    //Aktif Deviceleri getir
                    string apiUrlDevice = $"http://localhost:5191/api/Devices/getby-device/{item.DeviceId}";
                    var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                    var jsonResponseDevice = await responseDevice.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<Devices>(jsonResponseDevice, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if(model is not null)
                    devices.Add(model);
                    //model.SerialNumber seri no firstDate last Date göre verileri getir ekrana bas
            }
            return View("TrackingDataIndex",Tuple.Create(vehicleActive, trackingdata));
        }

        [HttpPost]
        public async Task<IActionResult> FiltreIndex(int? id, DateTime? firstDate, DateTime? lastDate)
        {
            List<TrackingDataForStd> dataList = new List<TrackingDataForStd>
            {
                new TrackingDataForStd
                {
                    TrackingDataId = 1,
                    SerialNumber = "A1234567",
                    Timestamp = DateTime.Now,
                    Latitude = 40.7128m,
                    Longitude = -74.0060m,
                    Speed = 80.5m,
                    Altitude = 15.0m,
                    Odometer = 1200.5m,
                    IsOpenIgnition = true,
                    FuelLevel = 50.2m,
                    NS = 'N',
                    EW = 'W',
                    Satellites = 5,
                    COG = 180.5m,
                    DIN1 = true,
                    DIN2 = false,
                    DOUT = true,
                    TotalSpentFuel = 300.0m,
                    GSM_RSSI = 4
                },
                new TrackingDataForStd
                {
                    TrackingDataId = 2,
                    SerialNumber = "B7654321",
                    Timestamp = DateTime.Now.AddMinutes(-30),
                    Latitude = 34.0522m,
                    Longitude = -118.2437m,
                    Speed = 65.3m,
                    Altitude = 30.2m,
                    Odometer = 1800.7m,
                    IsOpenIgnition = false,
                    FuelLevel = 75.6m,
                    NS = 'S',
                    EW = 'E',
                    Satellites = 8,
                    COG = 200.1m,
                    DIN1 = false,
                    DIN2 = true,
                    DOUT = false,
                    TotalSpentFuel = 500.0m,
                    GSM_RSSI = 3
                },
                new TrackingDataForStd
                {
                    TrackingDataId = 3,
                    SerialNumber = "C2468135",
                    Timestamp = DateTime.Now.AddHours(-1),
                    Latitude = 51.5074m,
                    Longitude = -0.1278m,
                    Speed = 55.2m,
                    Altitude = 25.0m,
                    Odometer = 900.4m,
                    IsOpenIgnition = true,
                    FuelLevel = 20.1m,
                    NS = 'N',
                    EW = 'E',
                    Satellites = 6,
                    COG = 150.3m,
                    DIN1 = true,
                    DIN2 = false,
                    DOUT = true,
                    TotalSpentFuel = 200.0m,
                    GSM_RSSI = 5
                }
            };




            List<TrackingDataForStd> trackingdata = new List<TrackingDataForStd>();
            List<Vehicles> vehicleActive = new List<Vehicles>();
            foreach (var item in last)
            {
                if (item.VehicleId == id) {
                    string apiUrlDevice = $"http://localhost:5191/api/Devices/getby-device/{item.DeviceId}";
                    var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                    var jsonResponseDevice = await responseDevice.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<Devices>(jsonResponseDevice, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    //model.SerialNumber seri no firstDate last Date göre verileri getir ekrana bas
                }
            }
            return Json(new { vehicleActives = vehicleActive, trackingdata = dataList });
           // return View("TrackingDataIndex",Tuple.Create(vehicleActive, trackingdata));
        }
        public async Task<List<DeviceVehicles>> GetActiveDevice() {
            List<DeviceVehicles> last = new List<DeviceVehicles>();
            string apiUrl = "http://localhost:5191/api/DeviceVehicles/get-activedevice";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response != null)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                last = System.Text.Json.JsonSerializer.Deserialize<List<DeviceVehicles>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return last;
        }


        [HttpPost]
        public async Task Create([FromBody] TrackingDataForACC brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7215/api/TrackingDataForAcc/CreateTrackingDataForAcc", content);

            }


        }
        [HttpPost]
        public async Task Update([FromBody] TrackingDataForACC brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7215/api/TrackingDataForAcc/UpdateTrackingDataForAcc", content);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("https://localhost:7215/api/TrackingDataForAcc/DeleteTrackingDataForAcc/" + id);
            }
        }
    }
}
