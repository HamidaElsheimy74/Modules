using Microsoft.EntityFrameworkCore;
using Modules.Helpers;
using Modules.Models;

namespace Modules.Data;

public static class DepartmentInitializer
{
    public static void Seed(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, DepartmentName = "Web Development and Design", ParentId = null, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 2, DepartmentName = "Front-End Development", ParentId = 1, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\Frontend.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 3, DepartmentName = "Back-End Development", ParentId = 1, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\BackEnd.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 4, DepartmentName = "Open-Source", ParentId = 3, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\BackEnd.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 5, DepartmentName = "Microsoft", ParentId = 3, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\BackEnd.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 6, DepartmentName = "Web Design", ParentId = 1, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 7, DepartmentName = "Data and Analytics", ParentId = null, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 8, DepartmentName = "Data Analysist", ParentId = 5, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 9, DepartmentName = "Reporting", ParentId = 5, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 10, DepartmentName = "Data Management", ParentId = 5, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 11, DepartmentName = "Video Marketing", ParentId = null, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 12, DepartmentName = "Video Production", ParentId = 9, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 13, DepartmentName = "Video Editing", ParentId = 9, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow },
            new Department { Id = 14, DepartmentName = "Video Strategy", ParentId = 9, DepartmentLogo = ImageHelper.ImageToByteArray(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\WebDevDes.png")), creationDate = DateTime.UtcNow }




            );
    }
}
