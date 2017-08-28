namespace DAL
{
    public interface IUnitOfWork : System.IDisposable
    {
        IUserRepository UserRepository { get; }
        ILoanRepository LoanRepository { get; }
        IFundRepository FundRepository { get; }
        IReminderRepository RemainderRepository { get; }
        IMemberRepository MemberRepository { get; }
        INoteRepository NoteRepository { get; }
        void Save();
    }
}
