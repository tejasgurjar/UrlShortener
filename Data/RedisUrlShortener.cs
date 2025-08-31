using System.Text.Json;
using StackExchange.Redis;
using UrlShortener.Models;

namespace UrlShortener.Data;

public class RedisUrlShortener : IUrlShortener
{
    private readonly IConnectionMultiplexer _redis;

    public RedisUrlShortener(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public void CreateShortenedUrl(ShortUrl shortUrl)
    {
        if (shortUrl == null)
        {
            throw new ArgumentNullException(nameof(shortUrl));
        }
        
        var db = _redis.GetDatabase(0);
        var serialShortUrl = JsonSerializer.Serialize(shortUrl);
        db.StringSet(shortUrl.UrlKey, serialShortUrl);
    }

    public ShortUrl? GetShortenedUrlByShortUrlKey(string shortUrlKey)
    {
        var db = _redis.GetDatabase(0);
        string? serialShortUrl = db.StringGet(shortUrlKey);
        if (!string.IsNullOrEmpty(serialShortUrl))
        {
            return JsonSerializer.Deserialize<ShortUrl>(serialShortUrl);
        }

        return null;
    }

    public IEnumerable<ShortUrl> GetShortenedUrls()
    {
        throw new NotImplementedException();
    }
}