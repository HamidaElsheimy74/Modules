using Modules.Data;
using Modules.Models;

namespace Moduels.Tests.TestUtilities;
public static class DBContextTestIntializer
{
    public static void GetInMemoryDepartment(ModulesDBContext _context)
    {
        _context.Departments.AddAsync(new Department
        {
            Id = 3,
            DepartmentName = "Test",
            DepartmentLogo = new byte[] { },
            creationDate = DateTime.Now,
        });
        _context.SaveChanges();
    }

    public static void GetInMemoryDepartmentList(ModulesDBContext _context)
    {
        _context.Departments.AddRange(DepartmentTestInitializer.GetDepartmentList());
        _context.SaveChanges();
    }

}
