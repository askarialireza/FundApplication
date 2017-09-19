using System.Linq;

namespace Fund
{
    public partial class MembersListUserControl : System.Windows.Controls.UserControl
    {
        public MembersListUserControl()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.ReportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.ReportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.ReportType reportType)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.MemberRepository
                    .MembersToReport()
                    .ToList();

                Stimulsoft.Report.StiReport oReport = new Stimulsoft.Report.StiReport();

                oReport.Load(Properties.Resources.MembersViewReport);
                oReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oReport.RegBusinessObject("Members", varList);
                oReport.Compile();
                oReport.RenderWithWpf();
                oReport.DoAction(reportType, string.Format("گزارش اعضا ({0}) ", Utility.CurrentFund.Name));

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }

            }
        }
    }
}
