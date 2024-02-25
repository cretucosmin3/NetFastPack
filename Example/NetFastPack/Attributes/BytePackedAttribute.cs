using MessagePack;

namespace NetFastPack;

/// <summary>
/// Used to mark classes or structs for serialization.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public class BytePacked : MessagePackObjectAttribute
{
    public BytePacked() : base(false) { }
}