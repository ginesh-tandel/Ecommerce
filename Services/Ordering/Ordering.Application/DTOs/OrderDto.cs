namespace Ordering.Application.DTOs
{
    public record OrderDto(
        int? Id,
        string? UserName,
        decimal TotalPrice,
        string? FirstName,
        string? LastName,
        string? Email,
        string? AddressLine,
        string? Country,
        string? State,
        string? ZipCode,
        string? CardName,
        string? CardNumber,
        string? Expiration,
        string? Cvv,
        int? PaymentMethod
    );
    public record CreateOrderDto(
        int? Id,
        string? UserName,
        decimal TotalPrice,
        string? FirstName,
        string? LastName,
        string? Email,
        string? AddressLine,
        string? Country,
        string? State,
        string? ZipCode,
        string? CardName,
        string? CardNumber,
        string? Expiration,
        string? Cvv,
        int? PaymentMethod
    );
}
