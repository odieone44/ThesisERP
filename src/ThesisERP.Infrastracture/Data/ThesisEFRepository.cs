namespace ThesisERP.Infrastracture.Data;

public class ThesisEFRepository<T> : RepositoryBase<T> where T : class
{
    public ThesisEFRepository(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
