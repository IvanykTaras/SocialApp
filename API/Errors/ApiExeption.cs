using System;

namespace API.Errors;

public class ApiExeption
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }

    public ApiExeption(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }
}
