using System.Net;

namespace Modules.Models;

public class ErrorViewModel
{
	public string? RequestId { get; set; }

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	public HttpStatusCode StatusCode { get; set; }

	public string Message { get; set; }
}
