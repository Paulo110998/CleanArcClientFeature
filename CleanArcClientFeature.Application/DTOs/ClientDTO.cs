using System.ComponentModel.DataAnnotations;

namespace CleanArcClientFeature.Application.DTOs;

public class ClientDTO
{
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string? NomeFantasia { get; set; }

    [Required]
    [MaxLength(14)]
    public string? Cnpj { get; set; } // Mantém como string

    public bool Ativo { get; set; }
}