namespace BSM.Domain.Modules.ContentModule.ValueObjects;

public record EventStatusObject
{
    public int Value { get; private set; }

    public string Name => Value switch
    {
        0 => "Draft",
        1 => "Upcoming",
        2 => "OnGoing",
        3 => "End",
        _ => "Unknown"
    };
    
    private EventStatusObject() { }

    private EventStatusObject(int value)
    {
        Value = value;
    }
    public static EventStatusObject Draft => new(0);
    public static EventStatusObject Upcoming => new (1);
    public static EventStatusObject OnGoing => new(2);
    public static EventStatusObject End => new (3);
    
    public override string ToString() => Name;
}