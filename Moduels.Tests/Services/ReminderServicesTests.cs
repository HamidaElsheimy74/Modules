using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moduels.Tests.TestUtilities;
using Modules.Controllers;
using Modules.Data;
using Modules.Interfaces;
using Moq;

namespace Modules.Services.Tests;

[TestClass()]
public class ReminderServicesTests
{
    private readonly ModulesDBContext _dbContext;
    private readonly Mock<IEmailNotificationServices> _emailNotificationServices = new();
    private readonly Mock<ILogger<HomeController>> _logger = new();
    private readonly Mock<IBackgroundJobClient> _jobClient = new();
    private ReminderServices _service;
    public ReminderServicesTests()
    {
        var options = new DbContextOptionsBuilder<ModulesDBContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;
        _dbContext = new ModulesDBContext(options);
        _service = new(_dbContext, _emailNotificationServices.Object, _jobClient.Object);
    }

    [TestMethod()]
    public void AddReminder_whenSuccess_ReturnTrue()
    {
        //Arrange
        var model = ReminderTestInitializer.AddReminder();
        _emailNotificationServices.Setup(x => x.SendMail(It.IsAny<string>(), It.IsAny<string>()));

        //Act
        var result = _service.AddReminder(model, _logger.Object).Result;

        //Assert
        Assert.IsTrue(result);
    }


    [TestMethod()]
    public void AddReminder_whenException_throwException()
    {
        //Arrange
        var model = ReminderTestInitializer.AddReminder();
        model.Title = null!;
        var exception = new DbUpdateException();
        _emailNotificationServices.Setup(x => x.SendMail(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(exception);


        var result = Assert.ThrowsExceptionAsync<DbUpdateException>(() => _service.AddReminder(model, _logger.Object)).Result;

        //Assert
        Assert.AreEqual(exception.GetType(), result.GetType());
        StringAssert.Contains(result.Message, "Required properties '{'Title'}' are missing");
    }
}