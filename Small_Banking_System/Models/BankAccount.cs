namespace Banking.Api.Models;

public sealed class BankAccount
{
    public Guid Id { get; private set; }

    public string OwnerId { get; private set; } = string.Empty;

    public string Currency { get; private set; } = "VND";

    public decimal Balance { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    // EF Core uses this when reconstructing stored accounts.
    private BankAccount()
    {
    }

    public BankAccount(string ownerId)
    {
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            throw new ArgumentException(
                "An account must have an owner.",
                nameof(ownerId));
        }

        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Balance = 0m;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Credit(decimal amount)
    {
        ValidateAmount(amount);

        Balance += amount;
    }

    public void Debit(decimal amount)
    {
        ValidateAmount(amount);

        if (amount > Balance)
        {
            throw new InvalidOperationException(
                "Insufficient funds.");
        }

        Balance -= amount;
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero.");
        }

        if (decimal.Truncate(amount) != amount)
        {
            throw new ArgumentException(
                "VND amounts must be whole numbers.",
                nameof(amount));
        }
    }
}
