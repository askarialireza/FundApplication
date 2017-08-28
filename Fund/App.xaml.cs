using System.Linq;

namespace Fund
{
    public partial class App : System.Windows.Application 
    {
        public App()
        {
            InitializeComponent();
        }

        [System.STAThread]

        static void Main()
        {
            App app = new App();

            Utility.FarsiLocalization();
            Utility.SetStimulsoftLicense();

            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int UsersCount = oUnitOfWork.UserRepository
                .Get()
                .Count();

            if (UsersCount == 0)
            {
                CreateAdminWindow oCreateAdminWindow = new CreateAdminWindow();
                app.Run(oCreateAdminWindow);
            }
            else
            {
                UserLoginWindow oUserLoginWindow = new UserLoginWindow();
                app.Run(oUserLoginWindow);
            }
        }
    }

}
