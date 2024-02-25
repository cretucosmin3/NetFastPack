using MessagePack;

[MessagePackObject]
public class Account
{
    [Key(0)]
    public int Age { get; set; }

    [Key(1)]
    public string FirstName { get; set; }

    [Key(2)]
    public string LastName { get; set; }

    [Key(3)]
    public bool CanLogin { get; set; }

    [Key(4)]
    public bool IsDeleted { get; set; }

    [Key(5)]
    public int LoginAttemptsLeft { get; set; }

    [Key(6)]
    public List<Document> Documents { get; set; } = new List<Document>();
}