
namespace DAL
{
    public interface ITransactionRepository : IRepository<Models.Transaction>
    {
        System.Linq.IQueryable<Models.Transaction> GetTransactions(System.DateTime fromDate, System.DateTime toDate);
    }
}
