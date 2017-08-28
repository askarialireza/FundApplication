
namespace DAL
{
    public interface IFundRepository : IRepository<Models.Fund>
    {
        System.Linq.IQueryable<Models.Fund> GetFundsByUser(Models.User user);
        System.Linq.IQueryable<Models.Fund> GetFund(System.Guid id, string password);
        int MembersCountByFund(Models.Fund fund);
    }
}
