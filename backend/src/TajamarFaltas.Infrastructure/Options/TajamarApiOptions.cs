using System.ComponentModel.DataAnnotations;

namespace TajamarFaltas.Infrastructure.Options;

public sealed class TajamarApiOptions
{
    public const string SectionName = "TajamarApi";

    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    [Required]
    public string AdminUser { get; set; } = string.Empty;

    [Required]
    public string AdminPassword { get; set; } = string.Empty;
}