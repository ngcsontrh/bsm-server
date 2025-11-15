namespace BSM.Domain.Modules.ContentModule.ValueObjects;

public record BlogStatusObject
{
    public int Value { get; private set; }

    public string Name => Value switch
    {
        0 => "Draft",
        1 => "Published",
        2 => "Archived",
        _ => "Unknown"
    };
    
    private BlogStatusObject() { }
    
    private BlogStatusObject(int value)
    {
        Value = value;
    }

    public static BlogStatusObject Published => new(0);
    public static BlogStatusObject Draft => new(1);
    public static BlogStatusObject Archived => new(2);

    public override string ToString() => Name;
}