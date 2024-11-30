using System.ComponentModel.DataAnnotations;

namespace BankingSolution.Dtos;

public class CreateAccountDto
{
    [Required(ErrorMessage = "Owner is required")]
    [StringLength(100, ErrorMessage = "Owner name must be less than 100 characters")]
    public string Owner { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Initial balance must be a positive value")]
    public decimal InitialBalance { get; set; }
}