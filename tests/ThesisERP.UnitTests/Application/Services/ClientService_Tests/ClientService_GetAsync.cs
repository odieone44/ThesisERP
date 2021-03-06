using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Services.Entities;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.UnitTests.Helpers;
using ThesisERP.UnitTests.Helpers.Builders;
using Xunit;

namespace ThesisERP.UnitTests.Application.Services.ClientService_Tests;

public class ClientService_GetAsync
{
    private MockRepositoryBase<Entity> _mockRepo = new();

    private ClientService _clientService;
    private IMapper _mapper;

    public ClientService_GetAsync()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperInitializer>();
        });

        _mapper = mockMapper.CreateMapper();

        _clientService = new ClientService(_mapper, _mockRepo.MockInstance);
    }

    [Fact]
    public async Task ShouldReturnClientIfIdExists()
    {
        var testEntities = EntityBuilder
                        .BuildDefaultList(length: 10, withIds: false)
                        .ToList();

        var testClient = new EntityBuilder()
                           .WithDefaultClientValues()
                           .WithId(1)
                           .Build();

        testEntities.Add(testClient);

        _mockRepo.MockGetAll(
                    testEntities
                    .Where(x => x.EntityType == EntityType.client && x.Id == testClient.Id)
                    .ToList());

        var result = await _clientService.GetAsync(testClient.Id);

        Assert.NotNull(result);
        Assert.Equal(testClient.Id, result!.Id);
    }

    [Fact]
    public async Task ShouldReturnNullIfNoEntitiesExist()
    {
        _mockRepo.MockGetAll(new List<Entity>());

        var result = await _clientService.GetAsync(1);

        Assert.Null(result);
    }


    [Fact]
    public async Task ShouldReturnNullIfIdBelongsToSupplier()
    {
        var testSupplier = new EntityBuilder()
                            .WithDefaultSupplierValues()
                            .Build();

        _mockRepo.MockGetAll(new List<Entity>());

        var result = await _clientService.GetAsync(testSupplier.Id);

        Assert.Null(result);
    }
}
