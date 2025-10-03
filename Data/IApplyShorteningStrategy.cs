using UrlShortener.Models;

namespace UrlShortener.Data;

public interface IApplyShorteningStrategy
{
    string CreateShortUrlKey(string longUrl, Func<string, ShortUrl?> checkForExistingShortUrl) ;
}