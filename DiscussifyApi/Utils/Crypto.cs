using System;
using System.Globalization;
using static BCrypt.Net.BCrypt;

namespace DiscussifyApi.Utils
{
    public class Crypto
    {
        public static string Hash(string password)
        {
            return HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return Verify(password, hashedPassword);
        }
    }
}
