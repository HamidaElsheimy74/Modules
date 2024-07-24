using System.ComponentModel.DataAnnotations;

namespace Modules.Models;

public class Reminder : BaseEntity
{
	[Required]
	public string Title { get; set; }
	[Required]
	public DateTime Date_Time { get; set; }
	[Required]
	public string Email { set; get; }
	[Required]
	public bool Send { set; get; }

	public DateTime? SentDate { get; set; }
}
