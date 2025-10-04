using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers;

[Route("")]
[ApiController]
public class UrlRedirectController : ControllerBase
{

    private readonly IUrlShortener _urlShortener;
    public UrlRedirectController(IUrlShortener urlShortener)
    {
        _urlShortener = urlShortener; 
    }

    [HttpGet]
    public string Index()
    {
        return "My Url Redirect";
    }
    
    [HttpGet("{UrlKey}", Name = "GetLongUrlByShortUrlKeyAndRedirect")]
    public ActionResult<ShortUrl> GetLongUrlByShortUrlKeyAndRedirect(string urlKey)
    {
        ShortUrl? shortUrl = _urlShortener.GetShortenedUrlByShortUrlKey(urlKey);
        if (shortUrl != null)
        {
            return Redirect(shortUrl.LongUrl);
        }

        return NotFound();
    }
}