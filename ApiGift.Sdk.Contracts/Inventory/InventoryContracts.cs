using ApiGift.Sdk.Contracts.V2.Common;

namespace ApiGift.Sdk.Contracts.V2.Inventory;

/// <summary>Provides supported inventory return reason codes.</summary>
public static class InventoryReturnReasonCodes
{
    /// <summary>Indicates that the supplied code is invalid.</summary>
    public const string InvalidCode = "INVALID_CODE";
    /// <summary>Indicates that the supplied code was already redeemed.</summary>
    public const string AlreadyRedeemed = "ALREADY_REDEEMED";
    /// <summary>Indicates that the supplied value differs from the product.</summary>
    public const string DifferentValue = "DIFFERENT_VALUE";
    /// <summary>Indicates that the supplied region differs from the product.</summary>
    public const string DifferentRegion = "DIFFERENT_REGION";
}

/// <summary>Represents a request to take one product into inventory.</summary>
public sealed class CreateTakeoutRequest
{
    /// <summary>Gets or sets the product identifier.</summary>
    public Guid ProductId { get; set; }
    /// <summary>Gets or sets the caller-provided unique delivery key.</summary>
    public string DeliveryKey { get; set; } = string.Empty;
}

/// <summary>Represents a newly created takeout request.</summary>
public sealed class CreateTakeoutResponse
{
    /// <summary>Gets or sets the takeout request identifier.</summary>
    public Guid TakeoutInventoryId { get; set; }
}

/// <summary>Represents current takeout and delivered inventory status.</summary>
public sealed class TakeoutStatus
{
    /// <summary>Gets or sets the takeout request identifier.</summary>
    public Guid TakeoutInventoryId { get; set; }
    /// <summary>Gets or sets the delivered inventory identifier.</summary>
    public Guid? InventoryId { get; set; }
    /// <summary>Gets or sets the final entry price.</summary>
    public double? EntryPrice { get; set; }
    /// <summary>Gets or sets the product identifier.</summary>
    public Guid ProductId { get; set; }
    /// <summary>Gets or sets encrypted virtual collection data.</summary>
    public string? EncryptedVirtualCollections { get; set; }
    /// <summary>Gets or sets the latest webhook delivery error.</summary>
    public string? WebhookDeliveredError { get; set; }
    /// <summary>Gets or sets whether the takeout request was canceled.</summary>
    public bool IsCanceled { get; set; }
}

/// <summary>Represents a request to open or update an inventory return.</summary>
public sealed class CreateReturnRequest
{
    /// <summary>Gets or sets the inventory identifier.</summary>
    public Guid InventoryId { get; set; }
    /// <summary>Gets or sets the return reason code.</summary>
    public string ReturnReasonCode { get; set; } = string.Empty;
    /// <summary>Gets or sets the return description.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Gets or sets optional attachment URLs.</summary>
    public List<string>? AttachmentUrls { get; set; }
}

/// <summary>Represents a warning return request.</summary>
public sealed class CreateReturnWarningRequest
{
    /// <summary>Gets or sets the inventory identifier.</summary>
    public Guid InventoryId { get; set; }
    /// <summary>Gets or sets the return reason code.</summary>
    public string ReturnReasonCode { get; set; } = string.Empty;
}

/// <summary>Represents a created return support ticket.</summary>
public sealed class CreateReturnResponse
{
    /// <summary>Gets or sets the support ticket identifier.</summary>
    public Guid SupportTicketId { get; set; }
}

/// <summary>Represents a reply to an inventory return request.</summary>
public sealed class ReplyToReturnRequest
{
    /// <summary>Gets or sets the inventory identifier.</summary>
    public Guid InventoryId { get; set; }
    /// <summary>Gets or sets the reply text.</summary>
    public string Text { get; set; } = string.Empty;
    /// <summary>Gets or sets optional attachment URLs.</summary>
    public List<string>? AttachmentUrls { get; set; }
}

/// <summary>Represents a created return reply.</summary>
public sealed class ReplyToReturnResponse
{
    /// <summary>Gets or sets the reply identifier.</summary>
    public Guid ReplyId { get; set; }
}

/// <summary>Represents details of an inventory return request.</summary>
public sealed class ReturnDetails
{
    /// <summary>Gets or sets the return reason code.</summary>
    public string ReturnReasonCode { get; set; } = string.Empty;
    /// <summary>Gets or sets the support ticket number.</summary>
    public string TicketNumber { get; set; } = string.Empty;
    /// <summary>Gets or sets the creation timestamp.</summary>
    public DateTime CreateAt { get; set; }
    /// <summary>Gets or sets whether mediation has started.</summary>
    public bool UnderMediation { get; set; }
    /// <summary>Gets or sets the support ticket status.</summary>
    public SupportTicketStatus Status { get; set; }
    /// <summary>Gets or sets visible return replies and events.</summary>
    public List<ReturnReply> Replies { get; set; } = [];
    /// <summary>Gets or sets the support ticket identifier.</summary>
    public Guid SupportTicketId { get; set; }
}

/// <summary>Represents a reply or event in a return request.</summary>
public sealed class ReturnReply
{
    /// <summary>Gets or sets the creation timestamp.</summary>
    public DateTime CreateAt { get; set; }
    /// <summary>Gets or sets the reply text, when available.</summary>
    public string? Text { get; set; }
    /// <summary>Gets or sets the associated shop identifier.</summary>
    public Guid? ShopId { get; set; }
    /// <summary>Gets or sets whether the item is a system event.</summary>
    public bool IsEvent { get; set; }
    /// <summary>Gets or sets optional attachment URLs.</summary>
    public List<string>? AttachmentUrls { get; set; }
}
