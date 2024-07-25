using Modules.Models;

namespace Moduels.Tests.TestUtilities;
public static class DepartmentTestInitializer
{
    public static List<Department> GetEmptyDepartmentList()
    {
        return new();
    }

    public static List<Department> GetDepartmentList()
    {
        return new()
        {
         new Department { Id = 1, DepartmentName = "Web Development and Design", ParentId = null, DepartmentLogo = new byte[]{ }, creationDate = DateTime.UtcNow },
         new Department { Id = 2, DepartmentName = "Front-End Development", ParentId = 1, DepartmentLogo = new byte[]{ }, creationDate = DateTime.UtcNow }

        };
    }

    public static DepartmentDetailsModel GetDepartment()
    {
        return new()
        {
            Department = new Department()
            {
                Id = 1,
                DepartmentName = "Web Development and Design",
                ParentId = null,
                DepartmentLogo = new byte[] { },
                creationDate = DateTime.UtcNow
            }
        };

    }

}
