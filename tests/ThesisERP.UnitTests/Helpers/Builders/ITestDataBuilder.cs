using System;
using System.Collections.Generic;

namespace ThesisERP.UnitTests.Helpers.Builders;

internal interface ITestDataBuilder<T>
{
    ITestDataBuilder<T> WithDefaultValues();

    T Build();

    IEnumerable<T> BuildDefaultList(int length, bool withIds);
}
