using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace NRZ.Shared.Localization
{
    public class Resources
    {
        private static readonly ResourceManager _resManager = new ResourceManager(typeof(SharedResources));

        public static string Get(string key)
        {
            return _resManager.GetString(key);
        }
    }
}
