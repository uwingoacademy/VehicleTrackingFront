﻿using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace Frontend.Controllers
{
    public class DevicesController : Controller
    {
        private readonly HttpClient _httpClient;

        public DevicesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        { 
            try
            {
                string apiUrl = "http://78.111.111.81:5191/api/Devices/get-device";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiUrlPacket = "http://78.111.111.81:5191/api/Packets/get-packet";
                var responsePacket = await _httpClient.GetAsync(apiUrlPacket);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<List<Devices>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    var jsonResponsePacket = await responsePacket.Content.ReadAsStringAsync();
                    var modelPacket = System.Text.Json.JsonSerializer.Deserialize<List<Packets>>(jsonResponsePacket, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(Tuple.Create(model, modelPacket));
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                    return View(new List<Devices>());
                }

            }
            catch (Exception ex)
            {

                return View("Erorr");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Devices brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("http://78.111.111.81:5191/api/Devices/create-device", content);

            }


        }
        [HttpPost]
        public async Task Update([FromBody] Devices brand)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(brand);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PutAsync("http://78.111.111.81:5191/api/Devices/update-device", content);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("http://78.111.111.81:5191/api/Devices/delete-device/" + id);
            }
        }
    }
}
