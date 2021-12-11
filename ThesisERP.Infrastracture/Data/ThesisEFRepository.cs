namespace ThesisERP.Infrastracture.Data;

public class ThesisEFRepository<T> : BaseRepository<T> where T : class
{
    public ThesisEFRepository(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
