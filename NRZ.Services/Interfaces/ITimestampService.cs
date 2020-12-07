using NRZ.Ts.Client.Models;

namespace NRZ.Services.Interfaces
{
    public interface ITimestampService
    {
        TimestampResult GetTimeStamp(string inpit, string filePath, string authority);
        TimestampResult GetTimeStamp(object objectToStamp, string filePath, string authority);
        string Validate(byte[] tsr);
    }
}
