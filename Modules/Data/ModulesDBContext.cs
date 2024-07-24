using Microsoft.EntityFrameworkCore;
using Modules.Models;

namespace Modules.Data;

public class ModulesDBContext : DbContext
{

	public ModulesDBContext(DbContextOptions<ModulesDBContext> options)
		  : base(options)
	{
	}
	public DbSet<Department> Departments { get; set; }
	public DbSet<Reminder> Reminders { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Department>()
			.HasOne(e => e.Parent)
			.WithMany(e => e.SubDepartments)
			.HasForeignKey(e => e.ParentId).IsRequired(false);

		modelBuilder.Seed();
	}
}

