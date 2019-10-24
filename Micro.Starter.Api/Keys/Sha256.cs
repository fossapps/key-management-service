using System.Security.Cryptography;
using System.Text;

namespace Micro.Starter.Api.Keys
{
    public static class Sha256
    {
        public static string Compute(string text)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            var builder = new StringBuilder();  
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }  
            return builder.ToString();
        }
    }
}