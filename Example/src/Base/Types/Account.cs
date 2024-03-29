using NetFastPack;

namespace Example.Base.Types;

[BytePacked]
public class Account
{
    [Ix(0)]
    public int Age { get; set; }

    [Ix(1)]
    public string FirstName { get; set; }

    [Ix(2)]
    public string LastName { get; set; }

    [Ix(3)]
    public bool CanLogin { get; set; }

    [Ix(4)]
    public bool IsDeleted { get; set; }

    [Ix(5)]
    public int LoginAttemptsLeft { get; set; }

    [Ix(6)]
    public List<Document> Documents { get; set; } = new List<Document>();
}