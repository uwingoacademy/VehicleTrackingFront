using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using Frontend.HttpRequests;
using System.Reflection;

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
                deviceVehicleWithDetails = await GetActiveDevice();
                var deviceVehicleDict = deviceVehicleWithDetails.ToDictionary(d => d.Device.SerialNumber, d => d.Vehicle);
                var series = deviceVehicleDict.Keys.ToList();
                var datas = await GetLastData(series);
                foreach (var item in datas)
                {
                    if (item != null && deviceVehicleDict.TryGetValue(item.SerialNumber, out var vehicle) && vehicle != null)
                    {
                        TrackingDataViewModel trackingDataViewModel = new TrackingDataViewModel
                        {
                            trackingDataForStd = item,
                            vehicles = vehicle
                        };
                        trackingDataViewModels.Add(trackingDataViewModel);
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
                GenericRequests<Vehicles> genericRequestsVehicles = new GenericRequests<Vehicles>();
               var vehicles=  await  genericRequestsVehicles.GetHttpRequest($":5191/api/Vehicles/getby-vehicle/{item.DeviceVehicle.VehicleId}");
                if (vehicles is not null)
                    vehicleActive.Add(vehicles);
                GenericRequests<Devices> genericRequestsDevices = new GenericRequests<Devices>();
               var device = await genericRequestsDevices.GetHttpRequest($":5191/api/Devices/getby-device/{item.DeviceVehicle.DeviceId}");
                if (device is not null)
                    devices.Add(device);
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
                    GenericRequests<Devices> genericRequestsDevices = new GenericRequests<Devices>();
                 var model =  await genericRequestsDevices.GetHttpRequest($":5191/api/Devices/getby-device/{item.DeviceVehicle.DeviceId}");
                    string apiUrlDeviceMongo = $"http://78.111.111.81:5007/WeatherForecast/getby-filtre?serialNumber={model.SerialNumber}&firstDate={usFrist}&lastDate={us}&pageNumber={pageNumber}&pageSize={pageSize}";
                    var responseDeviceMongo = await _httpClient.GetAsync(apiUrlDeviceMongo);
                    var jsonResponseDeviceMongo = await responseDeviceMongo.Content.ReadAsStringAsync();

                    var trackingDataResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<TrackingDataForStd>>(jsonResponseDeviceMongo, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
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
         GenericRequests<DeviceVehicleWithDetails> genericRequestsDeviceVehicleWithDetails = new GenericRequests<DeviceVehicleWithDetails>();
          var deviceVehiclesActive = await  genericRequestsDeviceVehicleWithDetails.GetListHttpRequest(":5191/api/DeviceVehicles/get-activedevice");
            return deviceVehiclesActive.ToList();
        }
    }
}
