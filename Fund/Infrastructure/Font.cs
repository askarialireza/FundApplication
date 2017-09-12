using System.Linq;

namespace Infrastructure
{
    public static class Font
    {
        static Font()
        {
            FontsList = new System.Collections.Generic.List<ViewModels.FontViewModel>();

            if (FontsList.Count == 0)
            {
                ViewModels.FontViewModel oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "یاقوت";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(Infrastructure.FontFamily.BYagut);

                FontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "ترافیک";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(Infrastructure.FontFamily.BTraffic);

                FontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "نازنین";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(Infrastructure.FontFamily.BNazanin);

                FontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "یکان";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(Infrastructure.FontFamily.BYekan);

                FontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "آریـال";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(Infrastructure.FontFamily.Arial);

                FontsList.Add(oViewModel);
            }

            FontsList = FontsList
                .OrderBy(current => current.FontName)
                .ToList();
        }

        private static System.Collections.Generic.List<ViewModels.FontViewModel> _fontsList;

        public static System.Collections.Generic.List<ViewModels.FontViewModel> FontsList
        {
            get
            {
                return _fontsList;
            }
            set
            {
                _fontsList = value;
            }
        }
    }
}
