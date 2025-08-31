using UrlShortener.Models;

namespace UrlShortener.Data;

public interface IUrlShortener
{
    void CreateShortenedUrl(ShortUrl shortUrl);
    
    ShortUrl? GetShortenedUrlByShortUrlKey(string shortUrlKey);
    
    IEnumerable<ShortUrl> GetShortenedUrls();
}