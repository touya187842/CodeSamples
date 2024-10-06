using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TestServerAndTUnit;

public class MirrorMiddleware
{
    private readonly RequestDelegate Next;

    public MirrorMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var stringBuilder = new StringBuilder();
        var b = new UriBuilder();

        // Append Method and Url
        stringBuilder.AppendLine($"{request.Method} {request.Path}{request.QueryString}");

        // Append Headers
        foreach (var header in request.Headers)
        {
            stringBuilder.AppendLine($"{header.Key}: {header.Value}");
        }

        // Append Body
        if (request is { ContentLength: > 0 })
        {
            stringBuilder.AppendLine();
            using var memory = new MemoryStream();
            await request.Body.CopyToAsync(memory);
            stringBuilder.AppendLine(Convert.ToBase64String(memory.ToArray(), Base64FormattingOptions.InsertLineBreaks));
        }

        // Push to Response
        var response = context.Response;
        response.Headers.Append("Content-Type", "text/plain");
        await response.WriteAsync(stringBuilder.ToString());

        // Call next
        await Next.Invoke(context);
    }
}
