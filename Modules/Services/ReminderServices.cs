using Hangfire;
using Modules.Data;
using Modules.Helpers;
using Modules.Interfaces;
using Modules.Models;

namespace Modules.Services;

public class ReminderServices : IReminderServices
{
    private readonly ModulesDBContext _dbContext;
    private readonly IEmailNotificationServices _emailNotificationServices;
    private readonly IBackgroundJobClient _jobClient;
    public ReminderServices(ModulesDBContext dbContext, IEmailNotificationServices emailNotificationServices, IBackgroundJobClient jobClient)
    {
        _dbContext = dbContext;
        _emailNotificationServices = emailNotificationServices;
        _jobClient = jobClient;
    }

    public async Task<bool> AddReminder(Reminder model, ILogger logger)
    {
        try
        {
            logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(AddReminder)}"));

            model.creationDate = DateTime.UtcNow;

            model.Date_Time = TimeZoneInfo.ConvertTimeToUtc(model.Date_Time);

            var result = await _dbContext.Reminders.AddAsync(model);

            _dbContext.SaveChanges();

            var reminderId = result.Entity.Id;

            var isAdded = reminderId != 0 ? true : false;

            if (isAdded)
            {
                var runAt = new DateTimeOffset(model.Date_Time.Year, model.Date_Time.Month, model.Date_Time.Day, model.Date_Time.Hour, model.Date_Time.Minute, model.Date_Time.Second, TimeSpan.Zero);

                _jobClient.Schedule(() => SendMAilAndUpdateDB(model.Email, model.Title, reminderId, model.Date_Time), runAt);

            }

            return isAdded;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
        finally
        {
            logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(AddReminder)}"));

        }
    }

    public async Task SendMAilAndUpdateDB(string email, string title, long reminderId, DateTime sendDate)
    {
        try
        {
            await _emailNotificationServices.SendMail(email, title);
            var reminder = new Reminder() { Id = reminderId, Send = true, SentDate = sendDate };
            _dbContext.Reminders.Attach(reminder);
            _dbContext.Entry(reminder).Property(e => e.Send).IsModified = true;
            _dbContext.Entry(reminder).Property(e => e.SentDate).IsModified = true;
            _dbContext.SaveChanges();

        }
        catch (Exception)
        {
            throw;
        }
    }
}
