using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class Url
{
    [Required]
    public required string LongUrl { get; set; }
}