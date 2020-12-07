using System;

namespace NRZ.Shared
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the innermost Exception for an object
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetInnerMostException(this Exception ex)
        {
            Exception currentEx = ex;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
            }

            return currentEx;
        }
    }
}
