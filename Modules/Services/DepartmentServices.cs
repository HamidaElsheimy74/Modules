using Microsoft.EntityFrameworkCore;
using Modules.Data;
using Modules.Helpers;
using Modules.Interfaces;
using Modules.Models;

namespace Modules.Services;

public class DepartmentServices : IDepartmentServices
{
    private readonly ModulesDBContext _dbContext;
    public DepartmentServices(ModulesDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<DepartmentDetailsModel> GetDepartmentDetails(long deptId, ILogger logger)
    {
        try
        {
            logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(GetDepartmentDetails)}"));

            var department = await _dbContext.Departments.Include(dept => dept.Parent).ThenInclude(dep => dep.Parent).Include(dept => dept.SubDepartments).ThenInclude(dept => dept.SubDepartments).FirstOrDefaultAsync(dept => dept.Id == deptId);

            if (department == null)
            {
                return null!;
            }

            var parentDepartments = GetParentDepartments(department);

            var departmentDetails = new DepartmentDetailsModel()
            {
                Department = department,
                ParentDepartments = parentDepartments
            };

            return departmentDetails!;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(GetDepartmentDetails)}"));

        }
    }

    public async Task<List<Department>> GetDepartments(ILogger logger)
    {
        try
        {
            logger.LogTrace(string.Format(ModulesConstants.MethodStart, $"{GetType().Name}.{nameof(GetDepartments)}"));

            List<Department> departments = await _dbContext.Departments.ToListAsync();

            return departments;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            logger.LogTrace(string.Format(ModulesConstants.MethodEnd, $"{GetType().Name}.{nameof(GetDepartments)}"));
        }
    }


    private List<Department> GetParentDepartments(Department department)
    {
        var parents = new List<Department>();
        var current = department.Parent;
        while (current != null)
        {
            parents.Add(current);
            current = current.Parent;
        }
        return parents;
    }
}
