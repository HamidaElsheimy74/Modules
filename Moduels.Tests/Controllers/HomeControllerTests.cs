using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moduels.Tests.TestUtilities;
using Modules.Helpers;
using Modules.Interfaces;
using Modules.Models;
using Moq;
using System.Net;

namespace Modules.Controllers.Tests;

[TestClass()]
public class HomeControllerTests
{

    private readonly Mock<ILogger<HomeController>> _logger = new();
    private readonly Mock<IReminderServices> _reminderServices = new();
    private readonly Mock<IDepartmentServices> _departmentServices = new();
    HomeController _controller;
    public HomeControllerTests()
    {
        _controller = new(_reminderServices.Object, _departmentServices.Object, _logger.Object);
    }

    [TestMethod()]
    public void Index_WhenNoDepartmentExists_ReturnEmptyList()
    {
        //Arrange
        _departmentServices.Setup(x => x.GetDepartments(It.IsAny<ILogger>())).
            ReturnsAsync(DepartmentTestInitializer.GetEmptyDepartmentList());

        //Act
        var result = _controller.Index().Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var model = result.Model as List<Department>;
        Assert.IsNotNull(model);
        Assert.AreEqual(0, model.Count);
        _departmentServices.Verify(dept => dept.GetDepartments(It.IsAny<ILogger>()), Times.Once);
    }

    [TestMethod()]
    public void Index_WhenDepartmentsExist_ReturnDepartmentsList()
    {
        //Arrange
        var departments = DepartmentTestInitializer.GetDepartmentList();

        _departmentServices.Setup(x => x.GetDepartments(It.IsAny<ILogger>())).
            ReturnsAsync(departments);

        //Act
        var result = _controller.Index().Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var model = result.Model as List<Department>;
        Assert.IsNotNull(model);
        Assert.AreEqual(departments.Count, model.Count);
        _departmentServices.Verify(dept => dept.GetDepartments(It.IsAny<ILogger>()), Times.Once);
    }

    [TestMethod()]
    public void Index_WhenException_ReturnError()
    {

        //Arrange
        var exception = new Exception(ModulesConstants.Exception);

        _departmentServices.Setup(x => x.GetDepartments(It.IsAny<ILogger>())).
            ThrowsAsync(exception);

        //Act
        var result = _controller.Index().Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var model = result.Model as ErrorViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(ModulesConstants.InternalServer, model.Message);
        Assert.AreEqual(HttpStatusCode.InternalServerError, model.StatusCode);
        _departmentServices.Verify(dept => dept.GetDepartments(It.IsAny<ILogger>()), Times.Once);

    }

    [TestMethod()]
    public void DepartmentDetails_WhenNoDepartmentExists_ReturnError()
    {
        //Arrange
        var id = 3;
        DepartmentDetailsModel department = null!;
        _departmentServices.Setup(x => x.GetDepartmentDetails(It.IsAny<long>(), It.IsAny<ILogger>())).
            ReturnsAsync(department);

        //Act
        var result = _controller.DepartmentDetails(id).Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var model = result.Model as ErrorViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(ModulesConstants.NotFoundDept, model.Message);
        Assert.AreEqual(HttpStatusCode.NotFound, model.StatusCode);
        _departmentServices.Verify(dept => dept.GetDepartmentDetails(It.IsAny<long>(), It.IsAny<ILogger>()), Times.Once);

    }


    [TestMethod()]
    public void DepartmentDetails_WhenDepartmentExists_ReturnDepartment()
    {
        //Arrange
        var id = 1;
        var department = DepartmentTestInitializer.GetDepartment();
        _departmentServices.Setup(x => x.GetDepartmentDetails(It.IsAny<long>(), It.IsAny<ILogger>())).
            ReturnsAsync(department);

        //Act
        var result = _controller.DepartmentDetails(id).Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var model = result.Model as DepartmentDetailsModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(id, model.Department.Id);
        _departmentServices.Verify(dept => dept.GetDepartmentDetails(It.IsAny<long>(), It.IsAny<ILogger>()), Times.Once);
    }

    [TestMethod()]
    public void DepartmentDetails_WhenException_ReturnError()
    {

        //Arrange
        var exception = new Exception(ModulesConstants.Exception);
        var id = 1;
        _departmentServices.Setup(x => x.GetDepartmentDetails(It.IsAny<long>(), It.IsAny<ILogger>())).
            ThrowsAsync(exception);

        //Act
        var result = _controller.DepartmentDetails(id).Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var model = result.Model as ErrorViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(ModulesConstants.InternalServer, model.Message);
        Assert.AreEqual(HttpStatusCode.InternalServerError, model.StatusCode);
        _departmentServices.Verify(dept => dept.GetDepartmentDetails(It.IsAny<long>(), It.IsAny<ILogger>()), Times.Once);

    }

    [TestMethod()]
    public void CreateReminder_WhenValidModel_CreateReminderAndGoToHome()
    {
        //Arrange
        var expectedResult = true;
        var departments = DepartmentTestInitializer.GetDepartmentList();
        var model = ReminderTestInitializer.AddReminder();

        _reminderServices.Setup(x => x.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>())).
            ReturnsAsync(expectedResult);
        //Act
        var result = _controller.CreateReminder(model).Result as RedirectToActionResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.ActionName);
        Assert.AreEqual("Home", result.ControllerName);
        _reminderServices.Verify(dept => dept.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>()), Times.Once);

    }

    [TestMethod()]
    public void CreateReminder_WhenInValidModel_RetunBadRequest()
    {
        //Arrange
        Reminder model = ReminderTestInitializer.AddReminder();
        model
            .Title = null!;
        _controller.ModelState.AddModelError("Title", "Title is required");

        //Act
        var result = _controller.CreateReminder(model).Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var viewModel = result.Model as ErrorViewModel;
        Assert.IsNotNull(viewModel);
        Assert.AreEqual(ModulesConstants.InvalidModel, viewModel.Message);
        Assert.AreEqual(HttpStatusCode.BadRequest, viewModel.StatusCode);
        _reminderServices.Verify(dept => dept.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>()), Times.Never);

    }

    [TestMethod()]
    public void CreateReminder_WhenFailedCreation_RetunServerError()
    {
        //Arrange
        var expectedResult = false;
        var model = ReminderTestInitializer.AddReminder();
        _reminderServices.Setup(x => x.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>())).
            ReturnsAsync(expectedResult);

        //Act
        var result = _controller.CreateReminder(model).Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var viewModel = result.Model as ErrorViewModel;
        Assert.IsNotNull(viewModel);
        Assert.AreEqual(ModulesConstants.InternalServer, viewModel.Message);
        Assert.AreEqual(HttpStatusCode.InternalServerError, viewModel.StatusCode);
        _reminderServices.Verify(dept => dept.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>()), Times.Once);

    }


    [TestMethod()]
    public void CreateReminder_WhenException_RetunServerError()
    {
        //Arrange
        var exception = new Exception(ModulesConstants.Exception);
        var model = ReminderTestInitializer.AddReminder();
        _reminderServices.Setup(x => x.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>())).
            ThrowsAsync(exception);

        //Act
        var result = _controller.CreateReminder(model).Result as ViewResult;

        //Assert
        Assert.IsNotNull(result);
        var viewModel = result.Model as ErrorViewModel;
        Assert.IsNotNull(viewModel);
        Assert.AreEqual(ModulesConstants.InternalServer, viewModel.Message);
        Assert.AreEqual(HttpStatusCode.InternalServerError, viewModel.StatusCode);
        _reminderServices.Verify(dept => dept.AddReminder(It.IsAny<Reminder>(), It.IsAny<ILogger>()), Times.Once);

    }
}