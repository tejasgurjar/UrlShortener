using System.Security.Cryptography;
using System.Text;
using UrlShortener.Models;

namespace UrlShortener.Data;

public class Md5BasedUrlShorteningStrategy: IApplyShorteningStrategy
{
    public const string FixedUrlSuffix = "_<#>";
    public const int ShortUrlLength = 8;
    public string CreateShortUrlKey(string originalLongUrl, Func<string, ShortUrl?> checkIfShortUrlExists)
    {
        string longUrl = originalLongUrl;
        bool uniqueShortUrlFound = false;
        string shortUrl = string.Empty;
        
        while (!uniqueShortUrlFound)
        {
            shortUrl = ShortenGivenLongUrl(longUrl);
            ShortUrl? existingShortUrl = checkIfShortUrlExists(shortUrl);
            if (existingShortUrl != null)
            {
                if (string.Compare(existingShortUrl.LongUrl, longUrl, StringComparison.Ordinal) != 0)
                {
                    // Same short Url mapping to two different long URLs
                    StringBuilder sb = new();
                    sb.Append(longUrl);
                    sb.Append(FixedUrlSuffix);
                    longUrl = sb.ToString();
                }
                else
                {
                    shortUrl = existingShortUrl.UrlKey;
                    uniqueShortUrlFound = true;
                }
            }
            else
            {
                uniqueShortUrlFound = true;
            }
        }
        
        return shortUrl;
    }

    private string ShortenGivenLongUrl(string longUrl)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        // AI generated code
        using MD5 md5Hash = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(longUrl));

        // Create a new StringBuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString()[..ShortUrlLength];
    }
}