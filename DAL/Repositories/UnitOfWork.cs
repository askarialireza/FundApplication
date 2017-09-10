namespace DAL
{
    public class UnitOfWork : System.Object , IUnitOfWork
    {
        public UnitOfWork()
        {
            IsDisposed = false;
        }

        protected bool IsDisposed { get; set; }

        private Models.DatabaseContext _databaseContext;
        //*********************************************************
        //*********************************************************
        protected virtual Models.DatabaseContext DatabaseContext
        {
            get
            {
                if (_databaseContext == null)
                {
                    _databaseContext =
                        new Models.DatabaseContext();
                }

                return (_databaseContext);
            }
        }
        //*********************************************************
        //*********************************************************
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository =
                        new UserRepository(DatabaseContext);
                }

                return (_userRepository);
            }
        }
        //*********************************************************
        //*********************************************************
        private ILoanRepository _loanRepository;
        public ILoanRepository LoanRepository
        {
            get
            {
                if (_loanRepository == null)
                {
                    _loanRepository =
                        new LoanRepository(DatabaseContext);
                }

                return (_loanRepository);
            }
        }
        //*********************************************************
        //*********************************************************
        private IFundRepository _fundRepository;
        public IFundRepository FundRepository
        {
            get
            {
                if (_fundRepository == null)
                {
                    _fundRepository =
                        new FundRepository(DatabaseContext);
                }

                return (_fundRepository);
            }
        }
        //*********************************************************
        //*********************************************************
        private IReminderRepository _remainderRepository;
        public IReminderRepository RemainderRepository
        {
            get
            {
                if (_remainderRepository == null)
                {
                    _remainderRepository =
                        new ReminderRepository(DatabaseContext);
                }

                return (_remainderRepository);
            }
        }
        //*********************************************************
        //*********************************************************
        private IMemberRepository _memberRepository;
        public IMemberRepository MemberRepository
        {
            get
            {
                if (_memberRepository == null)
                {
                    _memberRepository =
                        new MemberRepository(DatabaseContext);
                }

                return (_memberRepository);
            }
        }
        //*********************************************************
        //*********************************************************
        private IInstallmentRepository _installmentRepository;
        public IInstallmentRepository InstallmentRepository
        {
            get
            {
                if (_installmentRepository == null)
                {
                    _installmentRepository =
                        new InstallmentRepository(DatabaseContext);
                }

                return (_installmentRepository);
            }
        }
        //*********************************************************
        //*********************************************************
        private ITransactionRepository _transactionRepository;
        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository =
                        new TransactionRepository(DatabaseContext);
                }

                return (_transactionRepository);
            }
        }
        //*********************************************************
        //*********************************************************

        public void Save()
        {
            DatabaseContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed == false)
            {
                if (disposing)
                {
                    _databaseContext.Dispose();
                    _databaseContext = null;
                }
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            System.GC.SuppressFinalize(this);
        }
    }
}
