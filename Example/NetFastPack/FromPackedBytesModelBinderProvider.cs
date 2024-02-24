using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace NetFastPack;

public class FromPackedBytesModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.IsComplexType && context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.Id == "FromPackedBytes")
        {
            return new FromPackedBytesModelBinder();
        }

        return null;
    }
}