using Modules.Models;

namespace Modules.Interfaces;

public interface IDepartmentServices
{
    Task<List<Department>> GetDepartments(ILogger logger);
    Task<DepartmentDetailsModel> GetDepartmentDetails(long deptId, ILogger logger);
}
