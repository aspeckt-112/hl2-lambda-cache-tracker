namespace LambdaCacheTracker.Web.Parsers;

public class ULongToBinaryParser
{
    private readonly ILogger<ULongToBinaryParser> _logger;

    public ULongToBinaryParser(ILogger<ULongToBinaryParser> logger)
    {
        _logger = logger;
    }

    public (bool ParsedSuccessfully, int[]? BinaryValue) Parse(ulong hexValue)
    {
        if (hexValue == 0)
        {
            _logger.LogError("Hex value is zero");
            return (false, null);
        }

        int bitCount = (int)Math.Log(hexValue, 2) + 1;
        int[] binaryValue = new int[bitCount];

        for (int i = 0; i < bitCount; i++)
        {
            binaryValue[bitCount - 1 - i] = (hexValue & (1UL << i)) != 0 ? 1 : 0;
        }

        return (true, binaryValue);
    }
}