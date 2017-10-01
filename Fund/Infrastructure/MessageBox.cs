
namespace Infrastructure
{
    public static class MessageBox
    {
        internal static class Caption
        {
            public const string Error = "خطا";

            public const string Question = "سوال";

            public const string Warning = "اخطار";

            public const string Information = "پیام";
        }

        static MessageBox()
        {
        }

        public static System.Windows.MessageBoxResult Show(string text)
        {
            return DevExpress.Xpf.Core.DXMessageBox.Show(text);
        }

        public static System.Windows.MessageBoxResult Show(string caption, string text)
        {
            System.Windows.MessageBoxButton oButton = System.Windows.MessageBoxButton.OK;

            System.Windows.MessageBoxImage oImage = System.Windows.MessageBoxImage.Error;

            System.Windows.MessageBoxResult oResult = System.Windows.MessageBoxResult.OK;

            switch (caption)
            {

                case Infrastructure.MessageBox.Caption.Error:
                    {
                        oButton = System.Windows.MessageBoxButton.OK;
                        oImage = System.Windows.MessageBoxImage.Error;
                        oResult = System.Windows.MessageBoxResult.OK;
                        break;
                    }
                case Infrastructure.MessageBox.Caption.Information:
                    {
                        oButton = System.Windows.MessageBoxButton.OK;
                        oImage = System.Windows.MessageBoxImage.Information;
                        oResult = System.Windows.MessageBoxResult.OK;
                        break;
                    }
                case Infrastructure.MessageBox.Caption.Question:
                    {
                        oButton = System.Windows.MessageBoxButton.YesNo;
                        oImage = System.Windows.MessageBoxImage.Question;
                        oResult = System.Windows.MessageBoxResult.No;
                        break;
                    }
                case Infrastructure.MessageBox.Caption.Warning:
                    {
                        oButton = System.Windows.MessageBoxButton.YesNoCancel;
                        oImage = System.Windows.MessageBoxImage.Warning;
                        oResult = System.Windows.MessageBoxResult.Cancel;
                        break;
                    }
                default:
                    break;
            }

            return DevExpress.Xpf.Core.DXMessageBox.Show
                 (
                    caption: caption,
                    messageBoxText: text,
                    button: oButton,
                    icon: oImage,
                    defaultResult: oResult,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                 );
        }
    }
}
