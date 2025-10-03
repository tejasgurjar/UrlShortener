using UrlShortener.Models;

namespace UrlShortener.Data;

public interface IUrlShortener
{
    ShortUrl CreateShortenedUrl(string longUrl);
    ShortUrl? GetShortenedUrlByShortUrlKey(string shortUrlKey);
    IEnumerable<ShortUrl> GetShortenedUrls();
    bool DeleteShortenedUrlByShortUrlKey(string shortUrlKey);
}