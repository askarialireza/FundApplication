
namespace ViewModels
{
    public class GenderViewModel
    {
        public GenderViewModel()
        {
        }

        public System.Windows.Media.ImageSource ComboBoxItemLogo { get; set; }

        public Models.Gender Gender { get; set; }

        public string Description { get; set; }
    }
}
