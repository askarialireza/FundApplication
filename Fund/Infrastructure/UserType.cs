
namespace Infrastructure
{
    public static class UserType
    {
        static UserType()
        {
            UserTypesList =
                new System.Collections.Generic.List<ViewModels.UserTypeViewModel>();

            ViewModels.UserTypeViewModel oViewModel = new ViewModels.UserTypeViewModel();

            System.Uri oUri = new System.Uri(@"/Fund;component/Resources/Icons/Admin30.png", System.UriKind.Relative);

            oViewModel.ComboBoxItemLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);
            oViewModel.IsAdmin = true;
            oViewModel.Description = "کاربر مدیر";

            UserTypesList.Add(oViewModel);

            oViewModel = new ViewModels.UserTypeViewModel();

            oUri = new System.Uri(@"/Fund;component/Resources/Icons/Person30.png", System.UriKind.Relative);

            oViewModel.ComboBoxItemLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);
            oViewModel.IsAdmin = false;
            oViewModel.Description = "کاربر عادی";

            UserTypesList.Add(oViewModel);
        }

        private static System.Collections.Generic.List<ViewModels.UserTypeViewModel> _genderList;

        public static System.Collections.Generic.List<ViewModels.UserTypeViewModel> UserTypesList
        {
            get
            {
                return _genderList;
            }
            set
            {
                _genderList = value;
            }
        }
    }
}
