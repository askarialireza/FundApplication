using System.Linq;

namespace DAL
{
    public class TransactionRepository : Repository<Models.Transaction>, ITransactionRepository
    {
        public TransactionRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public System.Linq.IQueryable<Models.Transaction> GetTransactions(System.DateTime fromDate, System.DateTime toDate)
        {
            System.DateTime oFromDate = new System.DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);

            System.DateTime oToDate = new System.DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

            var varResult = Get()
                .Where(current => current.Date >= oFromDate)
                .Where(current => current.Date <= oToDate)
                .OrderBy(current => current.Date)
                ;

            return varResult;
        }
    }
}
