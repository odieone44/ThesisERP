namespace ThesisERP.Application.Services.Pricing;

using ThesisERP.Core.Entities;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Pricing;

using AutoMapper;

internal class TaxService : ITaxService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Tax> _taxRepo;
    public TaxService(IMapper mapper, IRepositoryBase<Tax> taxRepo)
    {
        _mapper = mapper;
        _taxRepo = taxRepo;
    }

    public async Task<TaxDTO> CreateAsync(CreateTaxDTO taxDTO)
    {
        var taxToAdd = _mapper.Map<Tax>(taxDTO);
        var result = _taxRepo.Add(taxToAdd);

        await _taxRepo.SaveChangesAsync();

        return _mapper.Map<TaxDTO>(result);
    }

    public async Task<TaxDTO> UpdateAsync(int id, UpdateTaxDTO taxDTO)
    {
        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return null; }

        _mapper.Map(taxDTO, tax);

        _taxRepo.Update(tax);

        await _taxRepo.SaveChangesAsync();

        return _mapper.Map<TaxDTO>(tax);
    }

    public async Task<List<TaxDTO>> GetAllAsync()
    {
        var taxes = await _taxRepo
                           .GetAllAsync
                            (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<TaxDTO>>(taxes);

        return results;
    }

    public async Task<TaxDTO> GetByIdAsync(int id)
    {
        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return null; }

        return _mapper.Map<TaxDTO>(tax);
    }

    public async Task DeleteAsync(int id)
    {
        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return; }

        tax.IsDeleted = true;

        _taxRepo.Update(tax);
        await _taxRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return; }

        tax.IsDeleted = false;

        _taxRepo.Update(tax);
        await _taxRepo.SaveChangesAsync();
    }


}
