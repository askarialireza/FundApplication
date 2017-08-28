using System;
using System.Linq;

namespace DAL
{
    public class UserRepository : Repository<Models.User>, IUserRepository
    {
        public UserRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Models.User> GetAdminUser()
        {

            var varResult = Get()
                .Where(current => current.IsAdmin == true)
                .Where(current => current.CanBeDeleted == false)
                ;

            return varResult;
        }


        public IQueryable<object> UsersToReport()
        {
            var varResult = Get()
                .OrderBy(current => current.Username)
                .Select(current => new
                {
                    current.Username,
                    current.PersianRegisterationDate,
                    IsAdmin = (current.IsAdmin == true) ? "کاربر مدیر" : "کاربر عادی",
                    current.PersianLastLoginTime
                })
                ;

            return varResult;
        }

        public IQueryable<Models.User> FindUser(string username, string password)
        {
            var varResult = Get()
                .Where(current => string.Compare(current.Username, username, true) == 0)
                .Where(current => current.Password == password)
                ;

            return varResult;
        }

        public IQueryable<Models.User> FindUsername(string username)
        {
            var varResult = Get()
                .Where(current => string.Compare(current.Username, username, true) == 0)
                ;

            return varResult;
        }

        public int FundsCountByUser(Models.User user)
        {
            int count = 0;

            count = Get()
                .Where(current => current.Id == user.Id)
                .Select(current => current.Funds.Count)
                .DefaultIfEmpty(0)
                .Sum();

            return count;
        }
    }
}
