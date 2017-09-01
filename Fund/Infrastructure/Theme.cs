using System.Linq;

namespace Infrastructure
{
    public static class Theme
    {
        static Theme()
        {
            ThemesList = new System.Collections.Generic.List<ViewModels.ThemeViewModel>();

            if (ThemesList.Count == 0)
            {
                ViewModels.ThemeViewModel oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.SevenName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.SevenFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.VS2010Name;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.VS2010FullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2016ColorfulName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2016ColorfulFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2016WhiteName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2016WhiteFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2013LightGrayName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2013LightGrayFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2010BlueName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2010BlueFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2010SilverName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2010SilverFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2007BlueName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2007BlueFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2007SilverName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2007SilverFullName;

                ThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.DXStyleName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.DXStyleFullName;

                ThemesList.Add(oViewModel);
            }

            ThemesList = ThemesList
                .OrderBy(current => current.DisplayName)
                .ToList();
        }

        private static System.Collections.Generic.List<ViewModels.ThemeViewModel> _themesList;

        public static System.Collections.Generic.List<ViewModels.ThemeViewModel> ThemesList
        {
            get
            {
                return _themesList;
            }
            set
            {
                _themesList = value;
            }
        }
    }
}
