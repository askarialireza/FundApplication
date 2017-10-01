
namespace ViewModels
{
    public class MemberBalanceViewModel : object
    {
        public MemberBalanceViewModel() : base()
        {
        }

        public int LoansCount { get; set; }

        public int InstallmentsCount { get; set; }

        public int PayedInstallmentsCount { get; set; }

        public int PayedLoansCount { get; set; }

        public long LoansAmount { get; set; }

        public long InstallmentsAmount { get; set; }

        public long PayedInstallmentsAmount { get; set; }

        public long PayedLoansAmount { get; set; }
    }
}
