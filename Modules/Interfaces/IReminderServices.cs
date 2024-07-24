using Modules.Models;

namespace Modules.Interfaces;

public interface IReminderServices
{

	Task<bool> AddReminder(Reminder model, ILogger logger);
}
