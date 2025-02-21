using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<IResult> CreateProductAsync(ProductRegistrationForm productform);
        Task<IResult> DeleteProductAsync(int id);
        Task<IResult> GetAllProductsAsync();
        Task<IResult> GetProductAsync(Expression<Func<ProductEntity, bool>> expression);
        Task<IResult> UpdateProductAsync(int id, ProductUpdateForm updateForm);
    }
}