using Modules.Interfaces;
using Modules.Services;

namespace Modules.Helpers;

public static class DI
{
	public static IServiceCollection ConfigureServices(IServiceCollection services)
	{
		services.AddScoped<IEmailNotificationServices, EmailNotificationServices>();
		services.AddScoped<IDepartmentServices, DepartmentServices>();
		services.AddScoped<IReminderServices, ReminderServices>();

		return services;
	}
}
