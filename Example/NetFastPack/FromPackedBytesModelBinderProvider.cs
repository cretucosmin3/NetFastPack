using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace NetFastPack;

public class FromBytesModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.IsComplexType && context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.Id == "FromBytes")
        {
            return new FromBytesModelBinder();
        }

        return null;
    }
}