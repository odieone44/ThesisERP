namespace ThesisERP.UnitTests.Helpers;

using System;
using System.Collections.Generic;
using ThesisERP.Application.Interfaces;


public interface IMockRepositoryBase<T> where T : class
{
    IRepositoryBase<T> MockInstance { get; }

    MockRepositoryBase<T> MockUpdate();
    MockRepositoryBase<T> MockDelete();
    MockRepositoryBase<T> MockSaveChanges();
    MockRepositoryBase<T> MockCount(int result);
    MockRepositoryBase<T> MockGetById(T? returnObject);
    MockRepositoryBase<T> MockGetAll(List<T> returnList);
    MockRepositoryBase<T> MockAdd(Func<T, T> valueFunction);
}