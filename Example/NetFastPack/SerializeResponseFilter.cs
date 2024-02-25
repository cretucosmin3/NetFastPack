using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using MessagePack;
using System.Threading.Tasks;

namespace NetFastPack;

public class BytesPackedResponseFilter : IAsyncResultFilter
{
    private static MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Check if the action is marked with the BytesPackedResponse attribute
        var attribute = context.ActionDescriptor.EndpointMetadata.OfType<ByteResponse>().FirstOrDefault();

        if (attribute != null)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value != null)
                {
                    var httpContext = context.HttpContext;
                    var response = httpContext.Response;

                    response.ContentType = "application/x-msgpack";
                    
                    await MessagePackSerializer.SerializeAsync(response.Body, objectResult.Value, Options);

                    // Skip the next result execution delegate to prevent further processing
                    return;
                }
            }
        }

        // Continue with the pipeline if the attribute is not present or the result is not an ObjectResult
        await next();
    }
}