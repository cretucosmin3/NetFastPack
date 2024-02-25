using NetFastPack;

namespace Example.Base.Types;

[BytePacked]
public class Document
{
    [Ix(0)]
    public int Id { get; set; }

    [Ix(1)]
    public string Path { get; set; }

    [Ix(2)]
    public string SharedWith { get; set; }

    [Ix(3)]
    public int Views { get; set; }

    [Ix(4)]
    public DateTime CreatedOn { get; set; }

    [Ix(5)]
    public DateTime LastUpdated { get; set; }
}