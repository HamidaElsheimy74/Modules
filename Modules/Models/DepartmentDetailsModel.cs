namespace Modules.Models;

public class DepartmentDetailsModel
{
    public Department Department { get; set; }
    public List<Department> ParentDepartments { get; set; }
}
