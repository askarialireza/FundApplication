using System.Linq;

namespace Infrastructure
{
    public static class FontsList
    {
        static FontsList()
        {
            PersianFontsList = new System.Collections.Generic.List<ViewModels.FontViewModel>();

            if (PersianFontsList.Count == 0)
            {
                ViewModels.FontViewModel oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "یاقوت";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(FontFamily.BYagut);

                PersianFontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "ترافیک";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(FontFamily.BTraffic);

                PersianFontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "نازنین";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(FontFamily.BNazanin);

                PersianFontsList.Add(oViewModel);

                oViewModel = new ViewModels.FontViewModel();

                oViewModel.FontName = "یکان";
                oViewModel.FontFamily = new System.Windows.Media.FontFamily(FontFamily.BYekan);

                PersianFontsList.Add(oViewModel);
            }

            PersianFontsList = PersianFontsList
                .OrderBy(current => current.FontName)
                .ToList();
        }

        private static System.Collections.Generic.List<ViewModels.FontViewModel> _fontsList;

        public static System.Collections.Generic.List<ViewModels.FontViewModel> PersianFontsList
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
