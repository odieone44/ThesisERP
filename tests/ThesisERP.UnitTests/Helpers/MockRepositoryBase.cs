namespace ThesisERP.UnitTests.Helpers;

using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using ThesisERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

public class MockRepositoryBase<T> : Mock<IRepositoryBase<T>>, IMockRepositoryBase<T> 
    where T : class
{
    public IRepositoryBase<T> MockInstance { get; }

    public MockRepositoryBase() : base()
    {
        MockInstance = this.Object;
    }

    public MockRepositoryBase<T> MockGetById(T? returnObject)
    {
        Setup(x => x.GetByIdAsync(It.IsAny<It.IsAnyType>()))
                    .ReturnsAsync(returnObject);

        return this;
    }

    public MockRepositoryBase<T> MockGetAll(List<T> returnList)
    {
        Setup(r =>
                r.GetAllAsync(
                    It.IsAny<Expression<Func<T, bool>>>(),
                    It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>?>(),
                    It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object?>>?>()))
                .ReturnsAsync(returnList);

        return this;
    }

    public MockRepositoryBase<T> MockUpdate()
    {
        Setup(x => x.Update(It.IsAny<T>())).Verifiable();
        return this;
    }

    public MockRepositoryBase<T> MockAdd(Func<T, T> valueFunction)
    {

        Setup(x => x.Add(It.IsAny<T>()))
                    .Returns(valueFunction);

        return this;
    }

    public MockRepositoryBase<T> MockDelete()
    {
        Setup(x => x.Delete(It.IsAny<T>()))
                    .Verifiable();

        return this;
    }

    public MockRepositoryBase<T> MockSaveChanges()
    {
        Setup(x => x.SaveChangesAsync())
                    .Verifiable();

        return this;
    }

    public MockRepositoryBase<T> MockCount(int result)
    {
        Setup(x => x.CountAsync(It.IsAny<Expression<Func<T, bool>>?>()))
                    .ReturnsAsync(result);

        return this;
    }

}