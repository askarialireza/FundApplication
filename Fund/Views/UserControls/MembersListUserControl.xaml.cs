using System.Linq;

namespace Fund
{
    public partial class MembersListUserControl : System.Windows.Controls.UserControl
    {
        public MembersListUserControl()
        {
            InitializeComponent();

            GenderComboBox.SelectedIndex = 0;
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = (MembersGridControl.ItemsSource as System.Collections.Generic.List<Models.Member>)
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .OrderBy(current => current.FullName.LastName)
                    .ThenBy(current => current.FullName.FirstName)
                    .Select(current => new ViewModels.MemberViewModel()
                    {
                        Id = current.Id,
                        EmailAddress = current.EmailAddress,
                        FatherName = current.FatherName,
                        FullName = current.FullName,
                        Gender = current.Gender,
                        MembershipDate = current.MembershipDate,
                        NationalCode = current.NationalCode,
                        PhoneNumber = current.PhoneNumber,
                        Picture = current.Picture,
                    })
                    .ToList();

                var varMembersToReport = varList
                    .Select(current => new
                    {
                        FullName = current.MemberName,
                        Gender = current.GenderDescription,
                        current.FatherName,
                        MembershipDate =  current.PersianMembershipDate,
                        current.NationalCode,
                        current.PhoneNumber,
                        current.EmailAddress,
                        current.Image,
                    })
                    .ToList();

                Stimulsoft.Report.StiReport oReport = new Stimulsoft.Report.StiReport();

                string genderTitle = (GenderComboBox.SelectedIndex == 2) ? "اعضای خانم صندوق" : (GenderComboBox.SelectedIndex == 1) ? "اعضای آقا عضو صندوق" : "همه اعضای صندوق";

                oReport.Load(Properties.Resources.MembersListReport);
                oReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oReport.Dictionary.Variables.Add("Gender", genderTitle);
                oReport.RegBusinessObject("Members", varMembersToReport);
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

        private void LoadGridControl(int selectedIndex)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varData = oUnitOfWork.MemberRepository
                                    .Get()
                                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                                    .AsQueryable()
                                    ;

                switch (selectedIndex)
                {
                    case 0:
                        {
                            varData = varData
                                .OrderBy(current => current.FullName.LastName)
                                .ThenBy(current => current.FullName.FirstName)
                                ;

                            break;
                        }
                    case 1:
                        {
                            varData = varData
                                .Where(current => current.Gender == Models.Gender.Male)
                                .OrderBy(current => current.FullName.LastName)
                                .ThenBy(current => current.FullName.FirstName)
                                ;

                            break;
                        }
                    case 2:
                        {
                            varData = varData
                                .Where(current => current.Gender == Models.Gender.Female)
                                .OrderBy(current => current.FullName.LastName)
                                .ThenBy(current => current.FullName.FirstName)
                                ;

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                var varList = varData.ToList();

                MembersGridControl.ItemsSource = varList;

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message); ;
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

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadGridControl(GenderComboBox.SelectedIndex);
        }
    }
}
