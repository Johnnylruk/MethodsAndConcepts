﻿using System.Diagnostics;
using Lealthy_Hospital_Application_System.Filters;
using Microsoft.AspNetCore.Mvc;
using Lealthy_Hospital_Application_System.Models;

namespace Lealthy_Hospital_Application_System.Controllers;

[UserLoggedPage]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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