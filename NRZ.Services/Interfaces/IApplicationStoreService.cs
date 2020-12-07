using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Services.Interfaces
{
    public interface IApplicationStoreService
    {
        void SetApplicationBaseUrl(string requestId, string url);
        string GetApplicationBaseUrl(string requestId);
        void SetLang(string requestId,string lang);
        string GetLang(string requestId);
        void SetUserType(string requestId, string userType);
        string GetUserType(string requestId);
        void SetChsiNumber(string requestId, string chsiNumber);
        string GetChsiNumber(string requestId);
        void SetEmail(string requestId, string email);
        string GetEmail(string requestId);
        void Clear(string requestId);
    }
}
