using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortener _urlShortener;

    public UrlShortenerController(IUrlShortener urlShortener)
    {
        _urlShortener = urlShortener;
    }

    public string Index()
    {
        return "My Url Shortener";
    }
    
    [HttpGet("{UrlKey}", Name = "GetShortenedUrlByShortUrlKey")]
    public ActionResult<ShortUrl> GetShortenedUrlByShortUrlKey(string urlKey)
    {
        var shortUrl = _urlShortener.GetShortenedUrlByShortUrlKey(urlKey);
        if (shortUrl != null)
        {
            return Ok(shortUrl);
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult<ShortUrl> CreateShortenedUrl(ShortUrl shortUrl)
    {
        _urlShortener.CreateShortenedUrl(shortUrl);
        return CreatedAtRoute(nameof(GetShortenedUrlByShortUrlKey),
            new
            {
                UrlKey = shortUrl.UrlKey,
                OriginalUrl = shortUrl.OriginalUrl,
                ShortenedUrl = shortUrl.ShortenedUrl
            },
            shortUrl);
    }
}