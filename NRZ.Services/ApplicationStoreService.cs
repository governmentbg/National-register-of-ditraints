using NRZ.Services.Interfaces;
using System.Collections.Generic;

namespace NRZ.Services
{
    public class ApplicationStoreService : IApplicationStoreService
    {
        private Dictionary<string, string> _applicationBaseUrlDict = new Dictionary<string, string>();
        private Dictionary<string, string> _langDict = new Dictionary<string, string>();
        private Dictionary<string, string> _userTypeDict = new Dictionary<string, string>();
        private Dictionary<string, string> _emailDict = new Dictionary<string, string>();
        private Dictionary<string, string> _chsiNumberTypeDict = new Dictionary<string, string>();

        public string GetLang(string requestId)
        {
            if (_langDict.TryGetValue(requestId, out string lang))
            {
                return lang ?? "bg";
            }

            return null;
        }

        public string GetApplicationBaseUrl(string requestId)
        {
            if (_applicationBaseUrlDict.TryGetValue(requestId, out string url))
            {
                return url;
            }

            return null;
        }

        public string GetUserType(string requestId)
        {
            if (_userTypeDict.TryGetValue(requestId, out string userType))
            {
                return userType;
            }

            return null;
        }

        public string GetChsiNumber(string requestId)
        {
            if (_chsiNumberTypeDict.TryGetValue(requestId, out string chsiNumber))
            {
                return chsiNumber;
            }

            return default;
        }

        public string GetEmail(string requestId)
        {
            if (_emailDict.TryGetValue(requestId, out string email))
            {
                return email;
            }

            return null;
        }

        public void SetApplicationBaseUrl(string requestId, string url)
        {
            _applicationBaseUrlDict[requestId] = url;
        }

        public void SetUserType(string requestId, string userType)
        {
            _userTypeDict[requestId] = userType;
        }

        public void SetChsiNumber(string requestId, string chsiNumber)
        {
            _chsiNumberTypeDict[requestId] = chsiNumber;
        }

        public void SetLang(string requestId, string lang)
        {
            _langDict[requestId] = lang;
        }

        public void SetEmail(string requestId, string email)
        {
            _emailDict[requestId] = email;
        }

        public void Clear(string requestId)
        {
            if (_applicationBaseUrlDict.ContainsKey(requestId))
            {
                _applicationBaseUrlDict.Remove(requestId);
            }

            if (_langDict.ContainsKey(requestId))
            {
                _langDict.Remove(requestId);
            }

            if (_userTypeDict.ContainsKey(requestId))
            {
                _userTypeDict.Remove(requestId);
            }

            if (_emailDict.ContainsKey(requestId))
            {
                _emailDict.Remove(requestId);
            }

            if (_chsiNumberTypeDict.ContainsKey(requestId))
            {
                _chsiNumberTypeDict.Remove(requestId);
            }
        }
    }
}
