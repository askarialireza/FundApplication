
using System.Linq;

namespace DAL
{
    public class MemberRepository : Repository<Models.Member> ,IMemberRepository
    {
        public MemberRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {
                
        }

        public System.Linq.IQueryable<object> MembersToReport()
        {

            var varResult = Get()
                .OrderBy(current => current.FullName.LastName)
                .ThenBy(current => current.FullName.FirstName)
                .Select(current => new
                {
                    FullName = current.FullName.FirstName + " " + current.FullName.LastName,
                    current.FatherName,
                    Gender = current.GenderToString,
                    current.NationalCode,
                    current.EmailAddress,
                    current.PhoneNumber
                })
                ;

            return varResult;
        }
    }
}
