using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetFastPack;

/// <summary>
/// Get request data from tightly packed serialized bytes.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class FromPackedBytesAttribute : ModelBinderAttribute
{
    public FromPackedBytesAttribute() : base()
    {
        BinderType = typeof(FromPackedBytesModelBinder);
        BindingSource = BindingSource.Custom;

        // Set a unique ID for your custom binding source
        this.BindingSource = new BindingSource("FromPackedBytes", "From Packed Bytes", isGreedy: true, isFromRequest: true);
    }
}