﻿namespace BankingSolution.Dtos.Deposit;

public class TransferDto
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
}