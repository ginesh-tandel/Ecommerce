using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponse ToResponse(this Product product)
        {
            if (product == null) return null;
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Summary = product.Summary,
                ImageFile = product.ImageFile,
                Brand = product.Brand,
                Type = product.Type,
                CreatedDate = product.CreatedDate
            };
        }
        public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination) =>
            new(
                pagination.PageIndex,
                pagination.PageSize,
                pagination.Count,
                pagination.Data.Select(p => p.ToResponse()).ToList()
                );
        public static List<ProductResponse> ToResponseList(this IEnumerable<Product> products) =>
            products.Select(p => p.ToResponse()).ToList();

        public static Product ToEntity(this CreateProductCommand command, ProductBrand brand, ProductType type) =>
            new()
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Summary = command.Summary,
                ImageFile = command.ImageFile,
                Brand = brand,
                Type = type,
                CreatedDate = DateTimeOffset.UtcNow
            };
        public static Product ToUpdateEntity(this UpdateProductCommand command, Product existing, ProductBrand brand, ProductType type)
        {
            return new Product
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Summary = command.Summary,
                ImageFile = command.ImageFile,
                Brand = brand,
                Type = type,
                CreatedDate = existing.CreatedDate
            };
        }
        public static ProductDto ToDto(this ProductResponse product)
        {
            if (product == null) return null;
            return new ProductDto
            (
                Id: product.Id,
                Name: product.Name,
                Description: product.Description,
                Price: product.Price,
                Summary: product.Summary,
                ImageFile: product.ImageFile,
                Brand: new BrandDto(product.Brand.Id, product.Brand.Name),
                Type: new TypeDto(product.Type.Id, product.Type.Name),
                CreatedDate: DateTimeOffset.UtcNow
                );
        }
        public static UpdateProductCommand ToCommand(this UpdateProductDto dto,string id)
        {
            if (dto == null) return null;
            return new UpdateProductCommand
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Summary = dto.Summary,
                ImageFile = dto.ImageFile,
                BrandId = dto.BrandId,
                TypeId = dto.TypeId
            };
        }
    }
}
