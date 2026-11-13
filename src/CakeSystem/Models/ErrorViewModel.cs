using System.Net;

namespace CakeSystem.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

    public string? ErrorMessage { get; set; }
}
