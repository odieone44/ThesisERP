namespace ThesisERP.Core.Extensions;

public static class Enumerables
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
    {
        return self.Select((item, index) => (item, index));
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
    {
        return self is null || !self.Any();
    }
}
