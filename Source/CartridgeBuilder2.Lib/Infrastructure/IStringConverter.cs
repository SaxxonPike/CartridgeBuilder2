namespace CartridgeBuilder2.Lib.Infrastructure
{
    public interface IStringConverter
    {
        byte[] ConvertToBytes(string value);
        byte[] ConvertToBytes(string value, int length);
        byte[] ConvertToBytes(string value, int length, byte padding);
        string ConvertToString(byte[] value);
        string ConvertToString(byte[] value, byte padding);
    }
}