namespace BankingSolution.Dtos;

public class CreateAccountDto
{
    public string Owner { get; set; }
    public decimal InitialBalance { get; set; }
}