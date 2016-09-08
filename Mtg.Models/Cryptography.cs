using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mtg.Models.DTO;

namespace Mtg.Models
{
    public static class Cryptography
    {
        private const string Salt = "WizardsOfTheCoast";
        private const string Format = "{0}{1}{2}{3}";

        public static string GetSha1Hash(GameRequest request)
        {
            var str = string.Format(Format, request.GameId, request.PlayerId, request.RequestTime.ToString("O"), Salt);
            var bytes = Encoding.UTF8.GetBytes(str);
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(bytes);
            return HexStringFromBytes(hash);
        }

        private static string HexStringFromBytes(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach(byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public static bool ValidateRequest(GameRequest request)
        {
            var hash = GetSha1Hash(request);
            return string.Equals(hash, request.Token);
        }
    }
}