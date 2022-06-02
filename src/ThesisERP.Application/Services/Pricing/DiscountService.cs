namespace ThesisERP.Application.Services.Pricing;

using ThesisERP.Core.Entities;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Pricing;

using AutoMapper;

public class DiscountService : IDiscountService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Discount> _discountRepo;

    public DiscountService(IMapper mapper, IRepositoryBase<Discount> discountRepo)
    {
        _mapper = mapper;
        _discountRepo = discountRepo;
    }

    public async Task<DiscountDTO> CreateAsync(CreateDiscountDTO discountDTO)
    {
        var discountToAdd = _mapper.Map<Discount>(discountDTO);
        var result = _discountRepo.Add(discountToAdd);

        await _discountRepo.SaveChangesAsync();

        return _mapper.Map<DiscountDTO>(result);
    }

    public async Task<DiscountDTO> UpdateAsync(int id, UpdateDiscountDTO discountDTO)
    {
        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return null; }

        _mapper.Map(discountDTO, discount);

        _discountRepo.Update(discount);

        await _discountRepo.SaveChangesAsync();

        return _mapper.Map<DiscountDTO>(discount);
    }

    public async Task<List<DiscountDTO>> GetAllAsync()
    {
        var discountes = await _discountRepo
                           .GetAllAsync
                            (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<DiscountDTO>>(discountes);

        return results;
    }

    public async Task<DiscountDTO> GetByIdAsync(int id)
    {
        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return null; }

        return _mapper.Map<DiscountDTO>(discount);
    }

    public async Task DeleteAsync(int id)
    {
        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return; }

        discount.IsDeleted = true;

        _discountRepo.Update(discount);
        await _discountRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return; }

        discount.IsDeleted = false;

        _discountRepo.Update(discount);
        await _discountRepo.SaveChangesAsync();
    }

}
