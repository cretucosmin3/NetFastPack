using MessagePack;

[MessagePackObject]
public class Document
{
    [Key(0)]
    public int Id { get; set; }

    [Key(1)]
    public string Path { get; set; }

    [Key(2)]
    public string SharedWith { get; set; }

    [Key(3)]
    public int Views { get; set; }

    [Key(4)]
    public DateTime CreatedOn { get; set; }

    [Key(5)]
    public DateTime LastUpdated { get; set; }
}