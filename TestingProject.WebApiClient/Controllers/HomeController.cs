﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TestingProject.DTOs;
using TestingProject.WebApiClient.Models;

namespace TestingProject.WebApiClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Get()
        {
            IEnumerable<CarDTO> cars = Enumerable.Empty<CarDTO>();

            HttpResponseMessage dataTask = await _httpClient.GetAsync($"car");
            if (dataTask.IsSuccessStatusCode)
                cars = dataTask.Content.ReadAsAsync<IEnumerable<CarDTO>>().Result;

            return View(cars);
        }

        public async Task<IActionResult> GetSingle(int id)
        {
            CarDTO car = null;

            HttpResponseMessage dataTask = await _httpClient.GetAsync($"car/{id}");
            if (dataTask.IsSuccessStatusCode)
                car = dataTask.Content.ReadAsAsync<CarDTO>().Result;

            return View(car);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarDTO DTO)
        {
            HttpResponseMessage dataTask = await _httpClient.PostAsJsonAsync<CarDTO>($"car", DTO);

            return RedirectToAction("GetSingle");//, new { id },);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
