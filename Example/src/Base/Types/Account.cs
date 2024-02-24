using MessagePack;

namespace Example.Base.Types;

[MessagePackObject]
public class Person
{
    [Key(0)]
    public int Age { get; set; } = 25;

    [Key(1)]
    public string FirstName { get; set; } = "Bob";

    [Key(2)]
    public string LastName { get; set; } = "Marley";

    [Key(3)]
    public string Address { get; set; } = "123";

    [Key(4)]
    public bool CanLogin { get; set; } = true;

    [Key(5)]
    public bool IsDeleted { get; set; } = true;

    [Key(6)]
    public int LoginAttemptsLeft { get; set; }

    [Key(7)]
    public int TopScore { get; set; }

    [Key(8)]
    public List<Game> Games { get; set; } = new List<Game>();
}