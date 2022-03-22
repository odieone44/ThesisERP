using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Application.DTOs.Entities;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Services.Entities;
using ThesisERP.Core.Entities;
using ThesisERP.UnitTests.Helpers;
using ThesisERP.UnitTests.Helpers.Builders;
using Xunit;

namespace ThesisERP.UnitTests.Application.Services.ClientService_Tests;

public class ClientService_UpdateAsync
{
    private Mock<IRepositoryBase<Entity>> _mockRepo = new();

    private ClientService _clientService;
    private IMapper _mapper;

    public ClientService_UpdateAsync()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperInitializer>();
        });

        _mapper = mockMapper.CreateMapper();

        _clientService = new ClientService(_mapper, _mockRepo.Object);
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

        _mockRepo.SetupGetAll(new List<Entity>() { testClient });
        _mockRepo.Setup(x => x.Update(It.IsAny<Entity>())).Verifiable();
        _mockRepo.Setup(x => x.SaveChangesAsync()).Verifiable();

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

        _mockRepo.SetupGetAll(new List<Entity>());
        _mockRepo.Setup(x => x.Update(It.IsAny<Entity>())).Verifiable();
        _mockRepo.Setup(x => x.SaveChangesAsync()).Verifiable();

        var result = await _clientService.UpdateAsync(testClient.Id, testDto);

        Assert.Null(result);        
    }

}
