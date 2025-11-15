using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.BookModule.ValueObjects;

public record ImageObject
{
    public string Name { get; init; }
    public string Url { get; init; }
    public string MimeType { get; init; }
    public string? AltText { get; init; }
    
    private ImageObject() { }

    private ImageObject(string name, string url, string mimeType, string? altText)
    {
        Name = name;
        Url = url;
        AltText = altText;
        MimeType = mimeType;
    }

    public static ImageObject Create(string name, string url, string mimeType, string? altText)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name cannot be empty");
        if (string.IsNullOrWhiteSpace(url)) throw new DomainException("Url cannot be empty");
        if (string.IsNullOrWhiteSpace(mimeType)) throw new DomainException("MimeType cannot be empty");
        return new ImageObject(name, url, mimeType, altText);
    }
}