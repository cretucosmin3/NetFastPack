namespace NetFastPack;

/// <summary>
/// Ensures the response of this endpoint is optimally compressed to a byte array.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ByteResponse : Attribute { }