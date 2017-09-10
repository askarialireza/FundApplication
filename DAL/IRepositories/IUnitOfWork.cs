namespace DAL
{
    public interface IUnitOfWork : System.IDisposable
    {
        IUserRepository UserRepository { get; }
        ILoanRepository LoanRepository { get; }
        IFundRepository FundRepository { get; }
        IReminderRepository RemainderRepository { get; }
        IMemberRepository MemberRepository { get; }
        IInstallmentRepository InstallmentRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        void Save();
    }
}
