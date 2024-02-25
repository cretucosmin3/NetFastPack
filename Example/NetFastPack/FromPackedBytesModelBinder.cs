using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MessagePack;

namespace NetFastPack;

public class FromBytesModelBinder : IModelBinder
{
    private static MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        // Ensure there's content to read
        if (bindingContext.HttpContext.Request.ContentLength == null || bindingContext.HttpContext.Request.ContentLength.Value == 0)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        var inputStream = bindingContext.HttpContext.Request.Body;

        try
        {
            // Deserialize the input stream using MessagePack, considering the expected model type
            var deserializedObject = await MessagePackSerializer.DeserializeAsync(
                bindingContext.ModelType,
                inputStream,
                Options,
                bindingContext.HttpContext.RequestAborted
            );

            bindingContext.HttpContext.Response.ContentType = "application/x-msgpack";
            bindingContext.Result = ModelBindingResult.Success(deserializedObject);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}