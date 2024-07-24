using Microsoft.AspNetCore.Mvc;
using Modules.Helpers;
using Modules.Interfaces;
using Modules.Models;
using System.Diagnostics;
using System.Net;

namespace Modules.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IReminderServices _reminderServices;
    private readonly IDepartmentServices _departmentServices;
    public HomeController(IReminderServices reminderServices, IDepartmentServices departmentServices, ILogger<HomeController> logger)
    {
        _logger = logger;
        _reminderServices = reminderServices;
        _departmentServices = departmentServices;
    }

    public async Task<IActionResult> Index()
    {
        var departments = await _departmentServices.GetDepartments(_logger);
        return View(departments);
    }


    public async Task<IActionResult> DepartmentDetails(long Id)
    {
        if (Id <= 0)
            return View("Error", new ErrorViewModel
            {

                StatusCode = HttpStatusCode.NotFound,
                Message = ModulesConstants.NotFoundDept
            });

        var departments = await _departmentServices.GetDepartmentDetails(Id, _logger);

        if (departments == null)
        {
            return View("Error", new ErrorViewModel
            {

                StatusCode = HttpStatusCode.NotFound,
                Message = ModulesConstants.NotFoundDept
            });
        }

        return View(departments);
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> CreateReminder()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> CreateReminder(Reminder model)
    {

        try
        {
            if (ModelState.IsValid)
            {

                var result = await _reminderServices.AddReminder(model, _logger);
                if (result)
                    return View("Index");
                else
                    return View("Error", new ErrorViewModel
                    {

                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = ModulesConstants.InternalServer
                    });
            }
            else
            {
                return View("Error", new ErrorViewModel
                {

                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ModulesConstants.InvalidModel
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel
            {

                StatusCode = HttpStatusCode.InternalServerError,
                Message = ModulesConstants.InternalServer
            });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
