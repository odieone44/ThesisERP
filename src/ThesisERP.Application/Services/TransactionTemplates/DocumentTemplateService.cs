using ThesisERP.Core.Entities;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.Interfaces.TransactionTemplates;

using AutoMapper;

namespace ThesisERP.Application.Services.TransactionTemplates;

public class DocumentTemplateService : IDocumentTemplateService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<DocumentTemplate> _templateRepo;
    public DocumentTemplateService(IMapper mapper, IRepositoryBase<DocumentTemplate> templateRepo)
    {
        _mapper = mapper;
        _templateRepo = templateRepo;
    }

    public async Task<DocumentTemplateDTO> CreateAsync(CreateDocumentTemplateDTO templateDTO)
    {
        var templateToAdd = _mapper.Map<DocumentTemplate>(templateDTO);
        var result = _templateRepo.Add(templateToAdd);

        await _templateRepo.SaveChangesAsync();

        return _mapper.Map<DocumentTemplateDTO>(result);
    }

    public async Task<DocumentTemplateDTO> UpdateAsync(int id, UpdateDocumentTemplateDTO templateDTO)
    {
        var template = await _templateRepo.GetByIdAsync(id);

        if (template == null) { return null; }

        _mapper.Map(templateDTO, template);

        _templateRepo.Update(template);

        await _templateRepo.SaveChangesAsync();

        return _mapper.Map<DocumentTemplateDTO>(template);
    }

    public async Task<List<DocumentTemplateDTO>> GetAllAsync()
    {
        var templates = await _templateRepo
                           .GetAllAsync
                            (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<DocumentTemplateDTO>>(templates);

        return results;
    }

    public async Task<DocumentTemplateDTO> GetByIdAsync(int id)
    {
        var template = await _templateRepo.GetByIdAsync(id);

        if (template == null) { return null; }

        return _mapper.Map<DocumentTemplateDTO>(template);
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
