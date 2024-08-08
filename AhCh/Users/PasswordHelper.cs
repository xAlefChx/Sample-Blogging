using System.Security.Cryptography;
using System.Text;

namespace AhCh.Users
{
    public class PasswordHelper
    {
        public static string HashPassword(string pass)
        {
            var originalBytes = Encoding.Default.GetBytes(pass);
            var encodedBytes = MD5.HashData(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
    }
}