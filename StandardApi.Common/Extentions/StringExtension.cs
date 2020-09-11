﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StandardApi.Common.Securities;
using StandardApi.Constants;

namespace StandardApi.Common.Extentions
{
    public static class StringExtension
    {
        public static string ReplaceDomain(this string str, string value) => str.Replace(CustomKey.Domain, value);

        public static bool IsEmpty(this string s)
        {
            if ((s == null) || (s.Trim().Length == 0))
                return true;
            return false;
        }

        public static string HandleEmpty(this string s, string replace)
        {
            if ((s == null) || (s.Trim().Length == 0))
                return replace;
            return s;
        }

        public static List<int> ParseToToListNumber(this string s, char separator)
        {
            List<int> result = new List<int>();
            foreach (var day in s.Split(separator).ToList())
            {
                int parseDay;
                if (int.TryParse(day.Trim(), out parseDay))
                {
                    result.Add(parseDay);
                }
            }
            return result;
        }

        public static int? TryConvertToInt(this string input, int? returnCatchValue = null)
        {
            try
            {
                return int.Parse(input);
            }
            catch
            {
                return returnCatchValue;
            }
        }

        public static DateTime? TryConvertToDateTime(this string inputDate, DateTime? returnCatchValue = null)
        {
            try
            {
                return DateTime.ParseExact(inputDate, DateFormat.DateFormatFromURL, CultureInfo.InvariantCulture);
            }
            catch
            {
                return returnCatchValue;
            }
        }

        public static string EnCrypt(this string text) => AesCrypto.Encrypt(text);

        public static string DeCrypt(this string text) => AesCrypto.Decrypt(text);

        public static string ToMD5(this string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString(StringFormat.HexadecimalCharacters));
                }
                return sb.ToString();
            }
        }
    }
}
