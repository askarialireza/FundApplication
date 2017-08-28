using System.Linq;

namespace DAL
{
    public class FundRepository : Repository<Models.Fund>, IFundRepository
    {
        public FundRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Models.Fund> GetFund(System.Guid id, string password)
        {
            var varResult = Get()
                .Where(current => current.Id == id)
                .Where(current => current.ManagerPassword == password)
                ;

            return varResult;
        }

        public IQueryable<Models.Fund> GetFundsByUser(Models.User user)
        {
            var varResult = Get()
                .Where(current => current.UserId == user.Id)
                ;

            return varResult;
        }

        public int MembersCountByFund(Models.Fund fund)
        {
            int count = Get()
                .Where(current => current.Id == fund.Id)
                .Select(current => current.Members.Count)
                .DefaultIfEmpty(0)
                .Sum();

            return count;
        }
    }
}
