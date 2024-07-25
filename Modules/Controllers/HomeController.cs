using Microsoft.AspNetCore.Mvc;
using Modules.Helpers;
using Modules.Interfaces;
using Modules.Models;
using System.Net;
using System.Text.Json;

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
        _logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(Index)}"));
        try
        {

            var departments = await _departmentServices.GetDepartments(_logger);
            return View(departments);
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
        finally
        {
            _logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(Index)}"));

        }
    }


    public async Task<IActionResult> DepartmentDetails(long Id)
    {
        _logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(DepartmentDetails)}"));
        _logger.LogInformation($"Recived department id is {Id}");
        try
        {
            if (Id <= 0)
                return View("Error", new ErrorViewModel
                {

                    StatusCode = HttpStatusCode.NotFound,
                    Message = ModulesConstants.NotFoundDept
                });

            var department = await _departmentServices.GetDepartmentDetails(Id, _logger);

            if (department == null)
            {
                return View("Error", new ErrorViewModel
                {

                    StatusCode = HttpStatusCode.NotFound,
                    Message = ModulesConstants.NotFoundDept
                });
            }

            return View(department);
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
        finally
        {
            _logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(DepartmentDetails)}"));

        }
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> CreateReminder()
    {
        try
        {
            _logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(CreateReminder)}"));


            return View();
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
        finally
        {
            _logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(CreateReminder)}"));

        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateReminder(Reminder model)
    {
        _logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(CreateReminder)}"));

        _logger.LogInformation($"Recived reminder Model is {JsonSerializer.Serialize(model)}");

        try
        {

            if (ModelState.IsValid)
            {

                var result = await _reminderServices.AddReminder(model, _logger);
                if (result)
                    return RedirectToAction("Index", "Home");
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
        finally
        {
            _logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(CreateReminder)}"));

        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { Message = ModulesConstants.InternalServer, StatusCode = HttpStatusCode.InternalServerError });
    }
}
