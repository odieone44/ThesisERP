using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Services.Entities;
using ThesisERP.Core.Entities;
using Xunit;

namespace ThesisERP.UnitTests.Application.Services;

public class ClientService_GetClient
{
    private Mock<IRepositoryBase<Entity>> _mockRepo = new();

    private ClientService _clientService;
    private IMapper _mapper;

    public ClientService_GetClient()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperInitializer>();
        });

        _mapper = mockMapper.CreateMapper();

        _clientService = new ClientService(_mapper, _mockRepo.Object);
    }

    [Fact]
    public async Task ShouldReturnNullIfNoEntitiesExist()
    {
        _mockRepo.Setup(r =>
                r.GetAllAsync(
                    It.IsAny<Expression<Func<Entity, bool>>>(),
                    It.IsAny<Func<IQueryable<Entity>, IOrderedQueryable<Entity>>?>(),
                    It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object?>>?>()))
                .ReturnsAsync(new List<Entity>());

        var result = await _clientService.GetAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ShouldReturnClientIfIdExists()
    {

        var testClient = new EntityBuilder()
                           .WithClientTestValues()
                           .Build();

        _mockRepo.Setup(r =>
                r.GetAllAsync(
                    It.IsAny<Expression<Func<Entity, bool>>>(),
                    It.IsAny<Func<IQueryable<Entity>, IOrderedQueryable<Entity>>?>(),
                    It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object?>>?>()))
                .ReturnsAsync(new List<Entity>() { testClient });

        var result = await _clientService.GetAsync(1);

        Assert.Equal(testClient.Id, result?.Id);
    }

    //[Fact]
    //public async Task ShouldReturnNullIfOnlySuppliersExist()
    //{

    //    var testSupplier = new EntityBuilder()
    //                        .WithSupplierTestValues()
    //                        .Build();

    //    _mockRepo.Setup(r =>
    //            r.GetAllAsync(
    //                x => x.EntityType == EntityType.client && x.Id == It.IsAny<int>(),
    //                It.IsAny<Func<IQueryable<Entity>, IOrderedQueryable<Entity>>?>(),
    //                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object?>>?>()))
    //            .ReturnsAsync(new List<Entity>() { testSupplier }.Where(x=>x.EntityType == EntityType.client).ToList());

    //    var result = await _clientService.GetAsync(1);

    //    Assert.Null(result);
    //}
}
