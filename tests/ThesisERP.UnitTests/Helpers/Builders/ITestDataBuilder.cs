using System;
using System.Collections.Generic;

namespace ThesisERP.UnitTests.Helpers.Builders;

public interface ITestDataBuilder<T>
    where T : class
{
    ITestDataBuilder<T> WithDefaultValues();

    T Build();

    IEnumerable<T> BuildDefaultList(int length, bool withIds);
}
