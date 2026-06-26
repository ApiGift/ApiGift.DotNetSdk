namespace ApiGift.Sdk.Contracts.V2.Common;

/// <summary>Identifies how a product variant is expressed.</summary>
public enum ProductVariantType : byte
{
    Amount = 1,
    Duration = 2,
    Credit = 3,
    AccountType = 4,
    Plan = 5,
    Other = 255
}

/// <summary>Identifies the current status of a return support ticket.</summary>
public enum SupportTicketStatus
{
    Pending,
    UserPending,
    TargetPending,
    Closed
}
