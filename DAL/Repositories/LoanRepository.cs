using System.Linq;

namespace DAL
{
    public class LoanRepository : Repository<Models.Loan>, ILoanRepository
    {
        public LoanRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public IQueryable<Models.Loan> GetActiveLoans()
        {
            var varResult = Get()
                .Where(current => current.IsActive == true)
                ;
            return varResult;
        }

        public IQueryable<Models.Loan> GetNotActiveLoans()
        {
            var varResult = Get()
                .Where(current => current.IsActive == false)
                ;
            return varResult;
        }

        public IQueryable<Models.Loan> GetPayedLoans()
        {
            var varResult = Get()
                .Where(current => current.Installments.All(payment => payment.IsPayed == true))
                ;

            return varResult;
        }

        public bool IsPayedLoan(System.Guid LoanId)
        {
            var varResult = GetPayedLoans()
                .Where(current => current.Id == LoanId)
                .FirstOrDefault();

            return ((varResult == null) ? false : true);
        }
    }
}
