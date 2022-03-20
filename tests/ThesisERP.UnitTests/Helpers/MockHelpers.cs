using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ThesisERP.Application.Interfaces;

namespace ThesisERP.UnitTests.Helpers;

public static class MockHelpers
{
    public static void SetupGetAll<T>(this Mock<IRepositoryBase<T>> mockRepo, List<T> returnList)
        where T : class
    {
        mockRepo.Setup(r =>
                r.GetAllAsync(
                    It.IsAny<Expression<Func<T, bool>>>(),
                    It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>?>(),
                    It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object?>>?>()))
                .ReturnsAsync(returnList);
    }
}
