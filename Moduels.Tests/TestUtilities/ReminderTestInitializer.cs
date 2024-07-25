using Modules.Models;

namespace Moduels.Tests.TestUtilities;
public static class ReminderTestInitializer
{

    public static Reminder AddReminder()
    {
        return new Reminder()
        {
            Title = "Test",
            Date_Time = DateTime.Now,
            Email = "test@test.com"

        };
    }

}
