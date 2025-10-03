using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortener _urlShortener;
    private readonly IApplyShorteningStrategy _applyShorteningStrategy;

    public UrlShortenerController(IUrlShortener urlShortener, IApplyShorteningStrategy urlShorteningStrategy)
    {
        _urlShortener = urlShortener;
        _applyShorteningStrategy = urlShorteningStrategy;
    }

    [HttpGet]
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
    public ActionResult<ShortUrl> CreateShortenedUrl(Url url)
    {
       ShortUrl shortUrl = _urlShortener.CreateShortenedUrl(url.LongUrl);
       return CreatedAtRoute(nameof(GetShortenedUrlByShortUrlKey),
           new 
           { 
               UrlKey = shortUrl.UrlKey, 
               LongUrl = shortUrl.LongUrl, 
               ShortUrl = shortUrl.ShortenedUrl 
           }, 
           shortUrl);
    }
    
    [HttpDelete("{UrlKey}", Name = "DeleteShortenedUrlByShortUrlKey")]
    public ActionResult<HttpResponseMessage> DeleteShortenedUrlByShortUrlKey(string urlKey)
    {
        return _urlShortener.DeleteShortenedUrlByShortUrlKey(urlKey) ? Ok(): NotFound();
    }
}