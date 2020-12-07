using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NRZ.Shared
{
    public static class Helper
    {
        public static bool CheckEgn(string egn)
        {
            //TODO Checksum is not enough for egn check
            if (!String.IsNullOrWhiteSpace(egn) && (egn.Equals("7777777777") ||
                                                    egn.Equals("1909090909")))
                return true;

            Regex regex = new Regex(@"^\d{10}$");
            if (!regex.Match(egn).Success)
                return false;

            int sum = 0;
            byte[] checkDigits = { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            byte[] egnDigits = Encoding.ASCII.GetBytes(egn);
            for (int k = 0; k < 9; k++)
            {
                sum += (int)(checkDigits[k] * (egnDigits[k] - 48));
            }
            byte checkDigit = (byte)((sum % 11) % 10);
            return (checkDigit == (egnDigits[9] - 48));
        }

        public static bool CheckLNCH(string lnch)
        {
            byte[] checkDigits = { 21, 19, 17, 13, 11, 9, 7, 3, 1 };
            Regex regex = new Regex(@"^\d{10}$");
            if (!regex.Match(lnch).Success)
                return false;

            int sum = 0;

            byte[] egnDigits = Encoding.ASCII.GetBytes(lnch);
            for (int k = 0; k < 9; k++)
            {
                sum += (int)(checkDigits[k] * (egnDigits[k] - 48));
            }
            byte checkDigit = (byte)(sum % 10);
            return (checkDigit == (egnDigits[9] - 48));
        }
    }
}
