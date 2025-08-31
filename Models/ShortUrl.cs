using System.ComponentModel.DataAnnotations;
using Redis.OM;
namespace UrlShortener.Models;

public class ShortUrl
{
    [Required]
    public required string UrlKey { get; set; }
    [Required]
    public required string OriginalUrl { get; set; }
    [Required]
    public required string ShortenedUrl { get; set; }
}