using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace Frontend.Controllers
{
    public class TarckingDataAccController : Controller
    {
        private readonly HttpClient _httpClient;
        private static List<DeviceVehicles>? last;
        //  List<DeviceVehicles> last = new List<DeviceVehicles>();
        private static List<DeviceVehicleWithDetails> deviceVehicleWithDetails = new List<DeviceVehicleWithDetails>();
        public TarckingDataAccController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TrackingDataViewModel> trackingDataViewModels = new List<TrackingDataViewModel>();
                List<TrackingDataForStd> datas = new List<TrackingDataForStd>();
                deviceVehicleWithDetails =   await  GetActiveDevice();
                List<Devices> devices = new List<Devices>();
                List<string> series = new List<string>();
                foreach (var item2 in deviceVehicleWithDetails)
                {
                    series.Add(item2.Device.SerialNumber);  
                }
                datas = await GetLastData(series);
                //foreach (var item2 in last)
                //{
                //    string apiUrlDevice = $"http://78.111.111.81:5191/api/Devices/getby-device/{item2.DeviceId}";
                //    var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                //    var jsonResponseDevice = await responseDevice.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<Devices>(jsonResponseDevice, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    devices.Add(model);
                //}
                foreach (var item4 in datas)
                {
                    foreach (var item5 in deviceVehicleWithDetails) {
                        if (item5.Device.SerialNumber == item4.SerialNumber)
                        {
                            TrackingDataViewModel trackingDataViewModel = new TrackingDataViewModel();
                            trackingDataViewModel.trackingDataForStd = item4;
                            trackingDataViewModel.vehicles = item5.Vehicle;
                            trackingDataViewModels.Add(trackingDataViewModel);
                        }
                    }
                }
               
                return Ok(trackingDataViewModels);

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
            deviceVehicleWithDetails = await GetActiveDevice();
            foreach (var item in deviceVehicleWithDetails) {
                //Aktif araçları Vehicle getir
                string apiUrlVehicle = $"http://78.111.111.81:5191/api/Vehicles/getby-vehicle/{item.DeviceVehicle.VehicleId}";
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
                    string apiUrlDevice = $"http://78.111.111.81:5191/api/Devices/getby-device/{item.DeviceVehicle.DeviceId}";
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
        public async Task<IActionResult> FiltreIndex(int? id, DateTime firstDate, DateTime lastDate, int? pageNumber = 1, int? pageSize = 10)
        {
            List<TrackingDataForStd> trackingdatas = new List<TrackingDataForStd>();
            List<Vehicles> vehicleActive = new List<Vehicles>();
            lastDate = lastDate.Date.AddDays(1).AddTicks(-1);
            string us = lastDate.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string usFrist = firstDate.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));

            foreach (var item in deviceVehicleWithDetails)
            {
                if (item.DeviceVehicle.VehicleId == id)
                {
                    string apiUrlDevice = $"http://78.111.111.81:5191/api/Devices/getby-device/{item.DeviceVehicle.DeviceId}";
                    var responseDevice = await _httpClient.GetAsync(apiUrlDevice);
                    var jsonResponseDevice = await responseDevice.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<Devices>(jsonResponseDevice, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    string apiUrlDeviceMongo = $"http://78.111.111.81:5007/WeatherForecast/getby-filtre?serialNumber={model.SerialNumber}&firstDate={usFrist}&lastDate={us}&pageNumber={pageNumber}&pageSize={pageSize}";
                    var responseDeviceMongo = await _httpClient.GetAsync(apiUrlDeviceMongo);
                    var jsonResponseDeviceMongo = await responseDeviceMongo.Content.ReadAsStringAsync();

                    var trackingDataResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<TrackingDataForStd>>(jsonResponseDeviceMongo, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // trackingDataResponse objesinde hem sayfa bilgileri hem de veri var
                    trackingdatas = trackingDataResponse.Data;
                    int totalRecords = trackingDataResponse.TotalRecords;
                    int pageNumbers = trackingDataResponse.PageNumber;
                    int pageSizes = trackingDataResponse.PageSize;
                    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize.Value);
                    ViewBag.PageNumber = pageNumbers;
                    ViewBag.PageSize = pageSizes;
                    ViewBag.TotalRecords = totalRecords;
                    ViewBag.TotalPages = totalPages;
                }
            }
           

            var modelData = new Tuple<List<Vehicles>, List<TrackingDataForStd>>(vehicleActive, trackingdatas);

            // PartialView'i döndür
            return PartialView("Index", trackingdatas);
        }
        public class ApiResponse<T>
        {
            public int TotalRecords { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public List<T> Data { get; set; }
        }
        public async Task<List<TrackingDataForStd>> GetLastData(List<string> series) {
            var apiUrlLast = "http://78.111.111.81:5007/WeatherForecast/getby-srnlastlist";
            var responseLast = await _httpClient.PostAsJsonAsync(apiUrlLast, series);
            var jsonResponseLast = await responseLast.Content.ReadAsStringAsync();
            var models = System.Text.Json.JsonSerializer.Deserialize<List<TrackingDataForStd>>(jsonResponseLast, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return models.ToList();
        }
        public async Task<List<DeviceVehicleWithDetails>> GetActiveDevice() {
            List<DeviceVehicles> last = new List<DeviceVehicles>();
            List<DeviceVehicleWithDetails> deviceVehiclesActive = new List<DeviceVehicleWithDetails>();
            string apiUrl = "http://78.111.111.81:5191/api/DeviceVehicles/get-activedevice";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response != null)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                deviceVehiclesActive = System.Text.Json.JsonSerializer.Deserialize<List<DeviceVehicleWithDetails>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return deviceVehiclesActive.ToList();
        }
    }
}
