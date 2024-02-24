using MessagePack;

namespace Example.Base.Types;

[MessagePackObject]
public class Game
{
    [Key(0)]
    public int Id { get; set; } = 1;

    [Key(1)]
    public int Score { get; set; } = 1;

    [Key(2)]
    public int Time { get; set; } = 1;
}