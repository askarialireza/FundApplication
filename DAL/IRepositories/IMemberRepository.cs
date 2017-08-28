namespace DAL
{
    public interface IMemberRepository : IRepository<Models.Member>
    {
        System.Linq.IQueryable<object> MembersToReport();
    }
}
