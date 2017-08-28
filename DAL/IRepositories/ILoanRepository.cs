
namespace DAL
{
    public interface ILoanRepository : IRepository<Models.Loan>
    {
        System.Linq.IQueryable<Models.Loan> GetActiveLoans();
        System.Linq.IQueryable<Models.Loan> GetNotActiveLoans();
        System.Linq.IQueryable<Models.Loan> GetPayedLoans();
        bool IsPayedLoan(System.Guid LoanId);
    }
}
