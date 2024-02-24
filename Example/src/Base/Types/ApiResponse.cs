using MessagePack;

namespace Example.Base.Types;

[MessagePackObject]
public class ApiResponse<T>
{
    [Key(0)]
    public T Data { get; set; } = default!;

    [Key(1)]
    public string Messages { get; set; } = default!;

    [Key(2)]
    public bool Success { get; set; } = false;
}