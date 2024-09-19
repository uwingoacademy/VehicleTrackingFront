using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;

namespace Frontend.Controllers
{
    public class TarckingDataAccController : Controller
    {
        private readonly HttpClient _httpClient;

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
        },
        new TrackingDataForACC
        {
            AccelerometerDataId = 2,
            DeviceId = 1002,
            Timestamp = DateTime.Now.AddMinutes(-5),
            AccX = 0.22m,
            SerialNumber = 'B', // Tek bir karakterli değer
            AccY = 0.44m,
            AccZ = 0.66m,
            DateTime = DateTime.Now.AddMinutes(-5),
            Latitude = 34.0522m,
            NS = 'N',
            Longitude = -118.2437m,
            EW = 'W',
            Altitude = 50.0m,
            Speed = 20.5m,
            COG = 180.0m,
            IGN = true
        },
         new TrackingDataForACC
        {
            AccelerometerDataId = 2,
            DeviceId = 1002,
            Timestamp = DateTime.Now.AddMinutes(-5),
            AccX = 0.22m,
            SerialNumber = 'C', // Tek bir karakterli değer
            AccY = 0.44m,
            AccZ = 0.66m,
            DateTime = DateTime.Now.AddMinutes(-5),
            Latitude = 39.0522m,
            NS = 'N',
            Longitude = -108.2437m,
            EW = 'W',
            Altitude = 50.0m,
            Speed = 20.5m,
            COG = 180.0m,
            IGN = true
        }
            };
                return Ok(r);
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
