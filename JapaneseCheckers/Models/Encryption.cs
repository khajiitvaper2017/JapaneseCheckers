using System;
using System.Security.Cryptography;
using System.Text;

namespace JapaneseCheckers.Models;

internal static class Encryption
{
    public static string RandomDataBase64Url(uint length)
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Base64UrlencodeNoPadding(bytes);
    }

    public static byte[] Sha256Bytes(string inputStirng)
    {
        var bytes = Encoding.ASCII.GetBytes(inputStirng);

        return SHA256.Create().ComputeHash(bytes);
    }

    public static string Sha256String(string inputStirng)
    {
        return Convert.ToHexString(Sha256Bytes(inputStirng));
    }

    public static string Base64UrlencodeNoPadding(byte[] buffer)
    {
        var base64 = Convert.ToBase64String(buffer);

        base64 = base64.Replace("+", "-");
        base64 = base64.Replace("/", "_");
        base64 = base64.Replace("=", "");

        return base64;
    }
}