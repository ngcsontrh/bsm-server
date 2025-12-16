using System.Security.Cryptography;

namespace BSM.Framework.NanoID;

public static class NanoIDGenerator
{
    private const string DefaultAlphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string DefaultPrefix = "";
    private const int DefaultSize = 8;

    public static string Generate(string prefix = DefaultPrefix, int size = DefaultSize)
    {
        if (size < 0) throw new ArgumentException("Size cannot be less than 0");
        var buffers = new char[size];
        var data = new byte[size];

        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        for (int i = 0; i < size; i++)
        {
            var index = data[i] % DefaultAlphabet.Length;
            buffers[i] = DefaultAlphabet[index];
        }
        var randomPart = new string(buffers);

        var result = prefix switch
        {
            "" => randomPart,
            _ => $"{prefix}-{randomPart}"
        };
        return result;
    }
}
