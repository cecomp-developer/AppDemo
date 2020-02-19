using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AppDemo.Security
{
    public class Hash
    {
        public static string GenerarHash(string password, string sal)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(sal),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public static byte[] GenerarSalByte()
        {
            byte[] sal = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }
            return sal;
        }

        public static string GenerarSalString()
        {
            byte[] sal = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }
            return Convert.ToBase64String(sal);
        }

        public static bool VerificarHashPassword(string password, string sal, string hash)
        {
            if (GenerarHash(password, sal) == hash)
                return true;
            else
                return false;
        }
    }
}
