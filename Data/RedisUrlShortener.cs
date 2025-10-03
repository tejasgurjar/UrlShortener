using System.Text.Json;
using StackExchange.Redis;
using UrlShortener.Models;

namespace UrlShortener.Data;

public class RedisUrlShortener : IUrlShortener
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IApplyShorteningStrategy _urlShorteningStrategy;

    public RedisUrlShortener(IConnectionMultiplexer redis, IApplyShorteningStrategy urlShorteningStrategy)
    {
        _redis = redis;
        _urlShorteningStrategy = urlShorteningStrategy;
    }

    public ShortUrl CreateShortenedUrl(string? originalLongUrl)
    {
        ArgumentNullException.ThrowIfNull(originalLongUrl);

        string shortenedUrlKey =
            _urlShorteningStrategy.CreateShortUrlKey(originalLongUrl, GetShortenedUrlByShortUrlKey);
        ShortUrl shortUrl = new()
        {
            UrlKey = shortenedUrlKey,
            LongUrl = originalLongUrl,
            ShortenedUrl = $"http://localhost:5068/{shortenedUrlKey}"
        };

        StoreShortenedUrl(shortUrl);
        return shortUrl;
    }

    private void StoreShortenedUrl(ShortUrl shortUrl)
    {
        if (shortUrl == null)
        {
            throw new ArgumentNullException(nameof(shortUrl));
        }

        var db = _redis.GetDatabase(0);
        var serialShortUrl = JsonSerializer.Serialize(shortUrl);

        string? shortUrlKey = db.StringGet(shortUrl.UrlKey);
        if (shortUrlKey == null)
        {
            db.StringSet(shortUrl.UrlKey, serialShortUrl);
        }
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

    public bool DeleteShortenedUrlByShortUrlKey(string shortUrlKey)
    {
        var db = _redis.GetDatabase(0);
        if (db.KeyExists(shortUrlKey))
        {
            return db.KeyDelete(shortUrlKey);
        }

        return false;
    }
}