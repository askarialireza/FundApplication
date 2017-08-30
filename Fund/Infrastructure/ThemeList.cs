using System.Linq;

namespace Infrastructure
{
    public static class ThemeList
    {
        static ThemeList()
        {
            ApplicationThemesList = new System.Collections.Generic.List<ViewModels.ThemeViewModel>();

            if (ApplicationThemesList.Count == 0)
            {
                ViewModels.ThemeViewModel oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.SevenName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.SevenFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.VS2010Name;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.VS2010FullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2016ColorfulName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2016ColorfulFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2016WhiteName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2016WhiteFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2013LightGrayName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2013LightGrayFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2010BlueName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2010BlueFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2010SilverName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2010SilverFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2007BlueName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2007BlueFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2007SilverName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2007SilverFullName;

                ApplicationThemesList.Add(oViewModel);

                oViewModel = new ViewModels.ThemeViewModel();

                oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.DXStyleName;
                oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.DXStyleFullName;

                ApplicationThemesList.Add(oViewModel);
            }

            ApplicationThemesList = ApplicationThemesList
                .OrderBy(current => current.DisplayName)
                .ToList();
        }

        private static System.Collections.Generic.List<ViewModels.ThemeViewModel> _themesList;

        public static System.Collections.Generic.List<ViewModels.ThemeViewModel> ApplicationThemesList
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
