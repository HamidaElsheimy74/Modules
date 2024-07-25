using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moduels.Tests.TestUtilities;
using Modules.Controllers;
using Modules.Data;
using Moq;

namespace Modules.Services.Tests;

[TestClass()]
public class DepartmentServicesTests
{
    private readonly ModulesDBContext _dbContext;
    private readonly Mock<ILogger<HomeController>> _logger = new();

    private DepartmentServices _service;
    public DepartmentServicesTests()
    {
        var options = new DbContextOptionsBuilder<ModulesDBContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
                  .Options;
        _dbContext = new ModulesDBContext(options);
        _service = new(_dbContext);

    }


    [TestMethod()]
    public void GetDepartmentDetails_WhenNoDepartmentExist_ReturnNull()
    {
        //Arrange
        var id = 111;

        //Act
        var result = _service.GetDepartmentDetails(id, _logger.Object).Result;

        //Assert
        Assert.IsNull(result);
    }

    [TestMethod()]
    public void GetDepartmentDetails_WhenDepartmentExists_ReturnDepartment()
    {
        //Arrange
        var id = 3;
        DBContextTestIntializer.GetInMemoryDepartment(_dbContext);

        //Act
        var result = _service.GetDepartmentDetails(id, _logger.Object).Result;

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(id, result.Department.Id);

    }

    [TestMethod()]
    public void GetDepartments_WhenDepartmentsExist_ReturnDepartments()
    {
        //Arrange
        DBContextTestIntializer.GetInMemoryDepartmentList(_dbContext);

        //Act
        var result = _service.GetDepartments(_logger.Object).Result;

        //Assert
        Assert.IsNotNull(result);
        Assert.AreNotEqual(0, result.Count);

    }


    [TestMethod()]
    public void GetDepartments_WhenNoDepartmentsExist_ReturnEmptyListOfDepartments()
    {
        //Arrange
        _dbContext.Database.EnsureDeleted();
        _dbContext.Departments.RemoveRange();
        _dbContext.SaveChanges();

        //Act
        var result = _service.GetDepartments(_logger.Object).Result;

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}