using Frontend.Helper;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;


namespace Frontend.Controllers
{
    public class BrandController : Controller
    {

        private readonly HttpClient _httpClient;
        public BrandController(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Brand> brands = new List<Brand> { 
                    new Brand { BrandName = "Apple", BrandId = 1 }, 
                    new Brand { BrandName = "Smsung", BrandId = 2 } 
                };
                return View(brands);
                //string apiUrl = "https://localhost:7215/api/Brand/get-brands";
                //var response = await _httpClient.GetAsync(apiUrl);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonResponse = await response.Content.ReadAsStringAsync();
                //    var model = System.Text.Json.JsonSerializer.Deserialize<List<Brand>>(jsonResponse, new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true
                //    });
                //    List<Brand> brands = new List<Brand> { new Brand { BrandName="Apple" , BrandId=1 }, new Brand { BrandName="Smsung" , BrandId=2 } };
                //    return View(model);
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                //    return View(new List<Brand>());
                //}

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Brand brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7215/api/Brand/create-brand", content);

            }


        }
        [HttpPost]
        public async  Task Update([FromBody] Brand brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7215/api/Brand/update-brand", content);
              
            }

            
        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("https://localhost:7215/api/Brand/delete-brand/"+id);
            }
        }
    }
}
