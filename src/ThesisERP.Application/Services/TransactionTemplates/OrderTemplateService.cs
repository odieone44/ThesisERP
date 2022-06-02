using AutoMapper;

using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.TransactionTemplates;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.TransactionTemplates;

public class OrderTemplateService : IOrderTemplateService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<OrderTemplate> _templateRepo;
    public OrderTemplateService(IMapper mapper, IRepositoryBase<OrderTemplate> templateRepo)
    {
        _mapper = mapper;
        _templateRepo = templateRepo;
    }

    public async Task<OrderTemplateDTO> CreateAsync(CreateOrderTemplateDTO templateDTO)
    {
        var templateToAdd = _mapper.Map<OrderTemplate>(templateDTO);
        var result = _templateRepo.Add(templateToAdd);

        await _templateRepo.SaveChangesAsync();

        return _mapper.Map<OrderTemplateDTO>(result);
    }

    public async Task<OrderTemplateDTO> UpdateAsync(int id, UpdateOrderTemplateDTO templateDTO)
    {
        var template = await _templateRepo.GetByIdAsync(id);

        if (template == null) { return null; }

        _mapper.Map(templateDTO, template);

        _templateRepo.Update(template);

        await _templateRepo.SaveChangesAsync();

        return _mapper.Map<OrderTemplateDTO>(template);
    }

    public async Task<List<OrderTemplateDTO>> GetAllAsync()
    {
        var templates = await _templateRepo
                           .GetAllAsync
                            (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<OrderTemplateDTO>>(templates);

        return results;
    }

    public async Task<OrderTemplateDTO> GetByIdAsync(int id)
    {
        var template = await _templateRepo.GetByIdAsync(id);

        if (template == null) { return null; }

        return _mapper.Map<OrderTemplateDTO>(template);
    }

    public async Task DeleteAsync(int id)
    {
        var template = await _templateRepo.GetByIdAsync(id);

        if (template == null) { return; }

        template.IsDeleted = true;

        _templateRepo.Update(template);
        await _templateRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var template = await _templateRepo.GetByIdAsync(id);

        if (template == null) { return; }

        template.IsDeleted = false;

        _templateRepo.Update(template);
        await _templateRepo.SaveChangesAsync();
    }
}
