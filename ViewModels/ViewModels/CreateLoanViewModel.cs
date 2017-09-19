
namespace ViewModels
{
    public class CreateLoanViewModel
    {
        public CreateLoanViewModel()
        {
            Installments = new System.Collections.Generic.List<long>();
        }

        public long LoanAmount { get; set; }

        public long RefundAmount { get; set; }

        public long CommissionAmount { get; set; }

        public int InstallmentsCount { get; set; }

        public System.Collections.Generic.List<long> Installments { get; set; }
    }
}
