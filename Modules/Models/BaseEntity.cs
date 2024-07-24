using System.ComponentModel.DataAnnotations;

namespace Modules.Models;

public class BaseEntity
{
	[Key]
	public long Id { get; set; }

	public DateTime creationDate { get; set; }
}
