namespace Modules.Models;

public class Department : BaseEntity
{
	public long? ParentId { get; set; }
	public string DepartmentName { get; set; }
	public byte[] DepartmentLogo { set; get; }

	public Department Parent { get; set; }

	public List<Department> SubDepartments { get; set; }
}
