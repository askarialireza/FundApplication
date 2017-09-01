using System.Linq;

namespace Infrastructure
{
    public static class Gender
    {
        static Gender()
        {
            GendersList =
                new System.Collections.Generic.List<ViewModels.GenderViewModel>();

            ViewModels.GenderViewModel oViewModel
                 = new ViewModels.GenderViewModel();


            System.Uri oUri = new System.Uri(@"/Fund;component/Resources/Icons/male32.png", System.UriKind.Relative);

            oViewModel.ComboBoxItemLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);
            oViewModel.Gender = Models.Gender.Male;
            oViewModel.Description = "آقا";

            GendersList.Add(oViewModel);

            oViewModel = new ViewModels.GenderViewModel();

            oUri = new System.Uri(@"/Fund;component/Resources/Icons/female32.png", System.UriKind.Relative);

            oViewModel.ComboBoxItemLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);
            oViewModel.Gender = Models.Gender.Female;
            oViewModel.Description = "خانم";

            GendersList.Add(oViewModel);

            GendersList = GendersList
                .OrderBy(current => current.Description)
                .ToList();
        }

        private static System.Collections.Generic.List<ViewModels.GenderViewModel> _gendersList;

        public static System.Collections.Generic.List<ViewModels.GenderViewModel> GendersList
        {
            get
            {
                return _gendersList;
            }
            set
            {
                _gendersList = value;
            }
        }
    }
}
