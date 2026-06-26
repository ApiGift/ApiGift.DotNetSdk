namespace ApiGift.Sdk.Contracts.V2.Orders;

/// <summary>Represents a subscription activation request.</summary>
public sealed class CreateSubscriptionActivationRequest
{
    /// <summary>Gets or sets the subscription product identifier.</summary>
    public Guid ProductId { get; set; }
    /// <summary>Gets or sets the caller-provided idempotency reference.</summary>
    public string ReferenceId { get; set; } = string.Empty;
    /// <summary>Gets or sets product-specific identity values.</summary>
    public Dictionary<string, string> Identities { get; set; } = [];
}

/// <summary>Represents a subscription activation result.</summary>
public sealed class CreateSubscriptionActivationResponse
{
    /// <summary>Gets or sets the result message.</summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>Gets or sets the API result status.</summary>
    public int Status { get; set; }
    /// <summary>Gets or sets the created or existing order identifier.</summary>
    public Guid? OrderId { get; set; }
    /// <summary>Gets or sets the created or existing order number.</summary>
    public string? OrderNumber { get; set; }
}

/// <summary>Represents a subscription identifier validation request.</summary>
public sealed class ValidateSubscriptionIdentifierRequest
{
    /// <summary>Gets or sets the subscription product identifier.</summary>
    public Guid ProductId { get; set; }
    /// <summary>Gets or sets product-specific identity values.</summary>
    public Dictionary<string, string> Identities { get; set; } = [];
}

/// <summary>Represents a subscription identifier validation result.</summary>
public sealed class ValidateSubscriptionIdentifierResponse
{
    /// <summary>Gets or sets whether the identity is valid.</summary>
    public bool IsValid { get; set; }
    /// <summary>Gets or sets the validation message.</summary>
    public string Message { get; set; } = string.Empty;
}

/// <summary>Represents the current status of a subscription order.</summary>
public sealed class SubscriptionStatus
{
    /// <summary>Gets or sets the order identifier.</summary>
    public Guid OrderId { get; set; }
    /// <summary>Gets or sets the order status.</summary>
    public SubscriptionOrderStatus OrderStatus { get; set; }
    /// <summary>Gets or sets the order price.</summary>
    public double Price { get; set; }
}

/// <summary>Identifies a subscription order processing state.</summary>
public enum SubscriptionOrderStatus
{
    Pending,
    Done,
    Payment,
    Canceled,
    Refund,
    Pricing,
    PayToSeller,
    Supply
}
