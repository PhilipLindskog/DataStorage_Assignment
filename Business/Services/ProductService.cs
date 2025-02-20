using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ProductService(ProductRepository productRepository) : IProductService
{
    private readonly ProductRepository _productRepository = productRepository;

    public async Task<IResult> CreateProductAsync(ProductRegistrationForm productform)
    {
        if (productform == null)
            return Result.BadRequest("Invalid product registration");

        await _productRepository.BeginTransactionAsync();

        try
        {
            var productEntity = ProductFactory.Create(productform);

            if (await _productRepository.AlreadyExistsAsync(x => x.ProductName == productform.ProductName))
            {
                await _productRepository.RollbackTransactionAsync();
                return Result.AlreadyExists("A product with this name already exists");
            }

            var result = await _productRepository.CreateAsync(productEntity);

            if (result != null)
            {
                await _productRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _productRepository.RollbackTransactionAsync();
                return Result.Error("Product could not be created");
            }
        }
        catch (Exception ex)
        {
            await _productRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> GetAllCustomersAsync()
    {
        var productEntities = await _productRepository.GetAllAsync();

        if (productEntities == null)
            return Result.NotFound("No products found.");

        var products = productEntities.Select(ProductFactory.Create);
        return Result<IEnumerable<Product>>.Ok(products);
    }

    public async Task<IResult> GetProductAsync(Expression<Func<ProductEntity, bool>> expression)
    {
        var productEntity = await _productRepository.GetAsync(expression);

        if (productEntity == null)
            return Result.NotFound("Product not found.");

        var product = ProductFactory.Create(productEntity);
        return Result<Product>.Ok(product);
    }

    public async Task<IResult> UpdateProductAsync(Expression<Func<ProductEntity, bool>> expression)
    {
        await _productRepository.BeginTransactionAsync();

        try
        {
            var oldProductEntity = await _productRepository.GetAsync(expression);
            if (oldProductEntity == null)
            {
                await _productRepository.RollbackTransactionAsync();
                return Result.NotFound("Product not found.");
            }

            var productEntity = ProductFactory.Create(oldProductEntity);

            if (productEntity != null)
            {
                await _productRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _productRepository.RollbackTransactionAsync();
                return Result.Error("Product could not be updated.");
            }
        }
        catch (Exception ex)
        {
            await _productRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> DeleteProductAsync(int id)
    {
        var productEntity = await _productRepository.GetAsync(x => x.Id == id);

        if (productEntity == null)
            return Result.NotFound("Product not found.");

        try
        {
            var result = await _productRepository.DeleteAsync(x => x.Id == id);
            return result ? Result.Ok() : Result.Error("Unable to delete product");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }
}
