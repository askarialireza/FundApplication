namespace DAL
{
    public interface IUserRepository : IRepository<Models.User>
    {
        System.Linq.IQueryable<Models.User> GetAdminUser();
        System.Linq.IQueryable<Models.User> FindUser(string username, string password);
        System.Linq.IQueryable<Models.User> FindUsername(string username);
        int FundsCountByUser(Models.User user);
    }
}
