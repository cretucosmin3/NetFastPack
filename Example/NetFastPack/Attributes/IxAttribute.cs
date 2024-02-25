using MessagePack;

/// <summary>
/// Indexes properties in your object for serialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class Ix : KeyAttribute
{
    public Ix(int x) : base(x) { }
    public Ix(string x) : base(x) { }
}
