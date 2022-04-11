namespace ThesisERP.UnitTests.Application.Services.ClientService_Tests;

using Xunit;
using AutoMapper;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;
using System.Collections.Generic;
using ThesisERP.UnitTests.Helpers;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.DTOs.Entities;
using ThesisERP.UnitTests.Helpers.Builders;
using ThesisERP.Application.Services.Entities;

public class ClientService_UpdateAsync
{
    private IMockRepositoryBase<Entity> _mockRepo = new MockRepositoryBase<Entity>();

    private ClientService _clientService;
    private IMapper _mapper;

    public ClientService_UpdateAsync()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperInitializer>();
        });

        _mapper = mockMapper.CreateMapper();

        _clientService = new ClientService(_mapper, _mockRepo.MockInstance);
    }

    [Fact]
    public async Task ShouldUpdateEntityValues()
    {
        var testClient = new EntityBuilder()
                            .WithDefaultClientValues()
                            .Build();

        var testDto = _mapper.Map<UpdateClientDTO>(testClient);
        testDto.FirstName = "Changed Name";
        testDto.LastName = "Changed Last Name";

        _mockRepo.MockGetAll(new List<Entity>() { testClient })
                 .MockUpdate()
                 .MockSaveChanges();

        var result = await _clientService.UpdateAsync(testClient.Id, testDto);

        Assert.NotNull(result);
        Assert.NotNull(result!.DateUpdated);
        Assert.Equal(testDto.FirstName, result!.FirstName);
        Assert.Equal(testDto.LastName, result!.LastName);
    }

    [Fact]
    public async Task ShouldReturnNullIfClientDoesNotExist()
    {
        var testClient = new EntityBuilder()
                            .WithDefaultClientValues()
                            .Build();

        var testDto = _mapper.Map<UpdateClientDTO>(testClient);
        testDto.FirstName = "Changed Name";
        testDto.LastName = "Changed Last Name";

        _mockRepo.MockGetAll(new List<Entity>())
                 .MockUpdate()
                 .MockSaveChanges();

        var result = await _clientService.UpdateAsync(testClient.Id, testDto);

        Assert.Null(result);
    }

}
