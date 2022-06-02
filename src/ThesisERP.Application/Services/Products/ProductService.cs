using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Products;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Product> _productRepo;
    public ProductService(IMapper mapper, IRepositoryBase<Product> productRepo)
    {
        _mapper = mapper;
        _productRepo = productRepo;
    }

    public async Task<ProductDTO> CreateAsync(CreateProductDTO productDTO)
    {
        var productToAdd = _mapper.Map<Product>(productDTO);
        var result = _productRepo.Add(productToAdd);

        await _productRepo.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(result);
    }

    public async Task<ProductDTO> UpdateAsync(int id, UpdateProductDTO productDTO)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null) { return null; }

        _mapper.Map(productDTO, product);

        _productRepo.Update(product);

        await _productRepo.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<List<ProductDTO>> GetAllAsync()
    {
        var products = await _productRepo
                           .GetAllAsync
                            (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<ProductDTO>>(products);

        return results;
    }

    public async Task<ProductDTO> GetByIdAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null) { return null; }

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null) { return; }

        product.IsDeleted = true;

        _productRepo.Update(product);
        await _productRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null) { return; }

        product.IsDeleted = false;

        _productRepo.Update(product);
        await _productRepo.SaveChangesAsync();
    }
}
