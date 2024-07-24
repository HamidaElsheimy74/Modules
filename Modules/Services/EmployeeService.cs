using Microsoft.EntityFrameworkCore;
using Modules.Data;
using Modules.Models;

namespace Modules.Services;


public class DepartmentService
{
    private readonly ModulesDBContext _context;

    public DepartmentService(ModulesDBContext context)
    {
        _context = context;
    }

    public Department GetManagerWithHierarchy(int managerId)
    {
        return _context.Departments
            .Include(e => e.SubDepartments)
            .ThenInclude(e => e.SubDepartments) // Recursively include subordinates
            .FirstOrDefault(e => e.Id == managerId);
    }

    public List<Department> GetAllManagersAndDepartments(int managerId)
    {
        var manager = GetManagerWithHierarchy(managerId);
        var allDepartments = new List<Department>();
        GetSubordinates(manager, allDepartments);
        return allDepartments;
    }

    private void GetSubordinates(Department manager, List<Department> allDepartments)
    {
        if (manager == null) return;

        allDepartments.Add(manager);
        foreach (var subordinate in manager.SubDepartments)
        {
            GetSubordinates(subordinate, allDepartments);
        }
    }
}


