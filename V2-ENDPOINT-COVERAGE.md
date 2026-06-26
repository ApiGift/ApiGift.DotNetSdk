# ApiGift Shop Gateway v2 SDK Coverage

Repository: [ApiGift.DotNetSdk](https://github.com/ApiGift/ApiGift.DotNetSdk)

Discovery source:
`ApiGift.WebAPI/Controllers/ShopGateway/v2`

## Summary

- Controllers discovered: 6
- HTTP actions discovered: 18
- Swagger-visible public endpoints: 17
- Public endpoints implemented: 17
- Public endpoints not implemented: 0
- Swagger-hidden actions excluded: 1
- SDK contract types generated: 32

## Controllers and endpoints

| Controller | Endpoint | SDK client method |
|---|---|---|
| `ProductCategoryController` | `GET v2/ProductCategory` | `Categories.GetCategoriesAsync` |
| `ProductsController` | `GET v2/Products` | `Products.GetProductsAsync` |
| `ProductsController` | `GET v2/Products/{productId}/InventoryStatus` | `Products.GetInventoryStatusAsync` |
| `ProductsController` | `GET v2/Products/{productId}/MarketStatus` | `Products.GetMarketStatusAsync` |
| `TakeoutInventoryController` | `POST v2/TakeoutInventory` | `Inventory.CreateTakeoutAsync` |
| `TakeoutInventoryController` | `DELETE v2/TakeoutInventory/{takeoutInventoryId}` | `Inventory.CancelTakeoutAsync` |
| `TakeoutInventoryController` | `GET v2/TakeoutInventory?deliveryKey=...` | `Inventory.GetTakeoutStatusAsync` |
| `ReturnInventoryController` | `POST v2/ReturnInventory` | `Inventory.CreateReturnAsync` |
| `ReturnInventoryController` | `POST v2/ReturnInventory/Warning` | `Inventory.CreateReturnWarningAsync` |
| `ReturnInventoryController` | `POST v2/ReturnInventory/Reply` | `Inventory.ReplyToReturnAsync` |
| `ReturnInventoryController` | `PUT v2/ReturnInventory/StartMediation/{inventoryId}` | `Inventory.StartReturnMediationAsync` |
| `ReturnInventoryController` | `PUT v2/ReturnInventory/Close/{inventoryId}` | `Inventory.CloseReturnAsync` |
| `ReturnInventoryController` | `GET v2/ReturnInventory/{inventoryId}` | `Inventory.GetReturnDetailsAsync` |
| `SubscriptionsController` | `POST v2/Subscriptions` | `Orders.CreateSubscriptionActivationAsync` |
| `SubscriptionsController` | `POST v2/Subscriptions/validate-identifier` | `Orders.ValidateSubscriptionIdentifierAsync` |
| `SubscriptionsController` | `GET v2/Subscriptions?referenceId=...` | `Orders.GetSubscriptionStatusAsync` |
| `OrderCreditController` | `GET v2/OrderCredit` | `Wallet.GetOrderCreditAsync` |

## Contracts generated

- Common: `ProductVariantType`, `SupportTicketStatus`
- Categories: `ProductCategory`, `CategoryRegion`, `ProductFamily`
- Products: `GetProductsRequest`, `ProductSummary`,
  `ProductFulfillmentMethod`, `ProductVariant`, `ProductInventoryStatus`,
  `ProductMarketStatus`, `ProductAvailabilityStatus`, `ProductAutoSupplyInfo`,
  `ProductAutoSupplyBlockReason`
- Inventory: `InventoryReturnReasonCodes`, `CreateTakeoutRequest`,
  `CreateTakeoutResponse`, `TakeoutStatus`, `CreateReturnRequest`,
  `CreateReturnWarningRequest`, `CreateReturnResponse`, `ReplyToReturnRequest`,
  `ReplyToReturnResponse`, `ReturnDetails`, `ReturnReply`
- Orders: `CreateSubscriptionActivationRequest`,
  `CreateSubscriptionActivationResponse`,
  `ValidateSubscriptionIdentifierRequest`,
  `ValidateSubscriptionIdentifierResponse`, `SubscriptionStatus`,
  `SubscriptionOrderStatus`
- Wallet: `OrderCredit`
- Merchant: no public v2 contracts were discovered

## Excluded action

`PUT v2/Subscriptions/{orderItemId}/Pay` is marked `[SwaggerIgnore]`.
It is treated as an internal action and is intentionally absent from the public
SDK surface.

## Suggested client grouping

- Categories: product category and catalog navigation endpoints
- Products: product discovery, inventory availability, and market status
- Inventory: takeout inventory and return workflow endpoints
- Orders: subscription activation and subscription order status
- Wallet: order-credit balance
- Merchant: reserved root-client group; no Swagger-visible v2 endpoint currently
  maps to it
