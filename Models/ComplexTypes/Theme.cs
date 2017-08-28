
namespace Models.ComplexTypes
{
    [System.ComponentModel.DataAnnotations.Schema.ComplexType]
    public class Theme : object
    {
        public Theme() : base()
        {
            ApplicationTheme = "Office2010Blue";
            FontFamily = "B Yagut";
        }

        public string ApplicationTheme { get; set; }

        public string FontFamily { get; set; }
    }
}
