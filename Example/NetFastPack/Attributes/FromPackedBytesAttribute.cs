using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetFastPack;

/// <summary>
/// Get request data from tightly packed bytes.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class FromBytesAttribute : ModelBinderAttribute
{
    public FromBytesAttribute() : base()
    {
        BinderType = typeof(FromBytesModelBinder);
        BindingSource = BindingSource.Custom;

        // Set a unique ID for your custom binding source
        this.BindingSource = new BindingSource("FromBytes", "From Bytes", isGreedy: true, isFromRequest: true);
    }
}