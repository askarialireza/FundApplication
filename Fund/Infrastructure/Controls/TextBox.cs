
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Infrastructure.Controls
{
    public class TextBox : System.Windows.Controls.TextBox
    {
        public TextBox() : base()
        {
            IsSpaceAllowed = false;
            IsMoneyValue = false;
            Mask = string.Empty;
            RegEx = RegexType.None;
            MaskType = MaskTypeEnum.None;
            InvalidMaskError = "Error! Mask Is Invalid.";
        }

        public enum MaskTypeEnum
        {
            None = 0,
            Numeric = 1,
            Regex = 2,
        }

        public enum CurrencyExtensionType
        {
            Rial = 0,
            Toman = 1
        }

        public enum RegexType
        {
            None = 0,
            EmailAddress = 1,
            IranMobileNumber = 2,
            ShamsiDate =3,
            Username = 4,
            PercentValue = 5,
            NationalCode = 6,
            Custom = 7,
        }

        public static readonly System.Windows.DependencyProperty RegexTypeProperty =
            System.Windows.DependencyProperty.Register("RegexType", typeof(RegexType), typeof(TextBox), new System.Windows.PropertyMetadata(RegexType.None));

        public static readonly System.Windows.DependencyProperty MaskProperty =
            System.Windows.DependencyProperty.Register("Mask", typeof(string), typeof(TextBox), new System.Windows.FrameworkPropertyMetadata(null, new System.Windows.PropertyChangedCallback(OnMaskChanged)));

        public static readonly System.Windows.DependencyProperty MaskTypeEnumProperty =
            System.Windows.DependencyProperty.Register("MaskTypeEnum", typeof(MaskTypeEnum), typeof(TextBox), new System.Windows.PropertyMetadata(MaskTypeEnum.None));

        public static readonly System.Windows.DependencyProperty CurrencyExtensionTypeProperty =
            System.Windows.DependencyProperty.Register("CurrencyExtensionType", typeof(CurrencyExtensionType), typeof(TextBox), new System.Windows.PropertyMetadata(CurrencyExtensionType.Rial));

        public static readonly System.Windows.DependencyProperty IsSpaceAllowedProperty =
            System.Windows.DependencyProperty.RegisterAttached("IsSpaceAllowed", typeof(bool), typeof(TextBox), new System.Windows.UIPropertyMetadata(OnIsSpaceAllowedChanged));

        public static readonly System.Windows.DependencyProperty IsMoneyValueProperty =
            System.Windows.DependencyProperty.RegisterAttached("IsMoneyValue", typeof(bool), typeof(TextBox), new System.Windows.UIPropertyMetadata(OnIsMoneyValueChanged));

        public static readonly System.Windows.DependencyProperty InvalidMaskErrorProperty =
            System.Windows.DependencyProperty.RegisterAttached("InvalidMaskError", typeof(string), typeof(TextBox));

        public string Mask
        {
            get
            {
                return (string)base.GetValue(MaskProperty);
            }
            set
            {
                base.SetValue(MaskProperty, value);
            }
        }

        public string InvalidMaskError
        {
            get
            {
                return (string)base.GetValue(InvalidMaskErrorProperty);
            }
            set
            {
                base.SetValue(InvalidMaskErrorProperty, value);
            }
        }

        public bool IsSpaceAllowed
        {
            get
            {
                return (bool)base.GetValue(IsSpaceAllowedProperty);
            }
            set
            {
                base.SetValue(IsSpaceAllowedProperty, value);
            }
        }

        public bool IsMoneyValue
        {
            get
            {
                return (bool)base.GetValue(IsMoneyValueProperty);
            }
            set
            {
                base.SetValue(IsManipulationEnabledProperty, value);
            }
        }

        public CurrencyExtensionType CurrencyExtension
        {
            get
            {
                return (CurrencyExtensionType)base.GetValue(CurrencyExtensionTypeProperty);
            }
            set
            {
                base.SetValue(CurrencyExtensionTypeProperty, value);
            }
        }

        public RegexType RegEx
        {
            get
            {
                return (RegexType)base.GetValue(RegexTypeProperty);
            }
            set
            {
                base.SetValue(RegexTypeProperty, value);
            }
        }

        public MaskTypeEnum MaskType
        {
            get
            {
                return (MaskTypeEnum)base.GetValue(MaskTypeEnumProperty);
            }
            set
            {
                base.SetValue(MaskTypeEnumProperty, value);
            }
        }

        private static void OnIsMoneyValueChanged(System.Windows.DependencyObject o, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            TextBox oTextBox = o as TextBox;

            if (oTextBox.IsMoneyValue == true && oTextBox.IsSpaceAllowed == true)
            {
                throw new System.InvalidOperationException("Both Properties 'IsMoneyValue' and 'IsSpaceAllowed' can't be set to 'True'");
            }
        }

        private static void OnIsSpaceAllowedChanged(System.Windows.DependencyObject o, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            TextBox oTextBox = o as TextBox;

            if (oTextBox.IsMoneyValue == true && oTextBox.IsSpaceAllowed == true)
            {
                throw new System.InvalidOperationException("Both Properties 'IsMoneyValue' and 'IsSpaceAllowed' can't be set to 'True'");
            }
        }

        private static void OnMaskChanged(System.Windows.DependencyObject o, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            TextBox oTextBox = o as TextBox;

            if (oTextBox.IsMoneyValue == true && oTextBox.IsSpaceAllowed == true)
            {
                throw new System.InvalidOperationException("Both Properties 'IsMoneyValue' and 'IsSpaceAllowed' can't be set to 'True'");
            }
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (MaskType == MaskTypeEnum.Numeric)
            {
                if (IsSpaceAllowed && (e.Key == System.Windows.Input.Key.Space))
                {
                    e.Handled = false;

                    return;
                }
                if (e.Key == System.Windows.Input.Key.Back | e.Key == System.Windows.Input.Key.Tab | e.Key == System.Windows.Input.Key.Delete)
                {
                    e.Handled = false;

                    return;
                }
                else if (e.Key >= System.Windows.Input.Key.NumPad0 && e.Key <= System.Windows.Input.Key.NumPad9)
                {
                    e.Handled = false;

                    return;
                }
                else if (System.Char.IsDigit((char)System.Windows.Input.KeyInterop.VirtualKeyFromKey(e.Key)) == true)
                {
                    e.Handled = false;

                    return;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (MaskType == MaskTypeEnum.Numeric && IsMoneyValue == true && IsSpaceAllowed == false)
            {

                switch (CurrencyExtension)
                {
                    case CurrencyExtensionType.Rial:
                        {
                            this.Text = this.Text.Replace(" ریال", string.Empty).Replace(",", string.Empty);

                            if(Text == "0")
                            {
                                Text = string.Empty;
                            }

                            break;
                        }

                    case CurrencyExtensionType.Toman:
                        {
                            this.Text = this.Text.Replace(" تومان", string.Empty).Replace(",", string.Empty);

                            if (Text == "0")
                            {
                                Text = string.Empty;
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
        }

        protected override void OnLostFocus(System.Windows.RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (MaskType == MaskTypeEnum.Numeric && IsMoneyValue == true && IsSpaceAllowed == false)
            {
                switch (CurrencyExtension)
                {
                    case CurrencyExtensionType.Rial:
                        {
                            if (string.IsNullOrWhiteSpace(this.Text) == false)
                            {
                                long value = System.Convert.ToInt64(this.Text.Replace(" ریال", string.Empty).Replace(",", string.Empty));

                                this.Text = value.ToString("#,##0 ریال");
                            }
                            else
                            {
                                long zero = 0;

                                this.Text = zero.ToString("#,##0 ریال");
                            }

                            break;
                        }

                    case CurrencyExtensionType.Toman:
                        {
                            if (string.IsNullOrWhiteSpace(this.Text) == false)
                            {
                                long value = System.Convert.ToInt64(this.Text.Replace(" تومان", string.Empty).Replace(",", string.Empty));

                                this.Text = value.ToString("#,##0 تومان");
                            }
                            else
                            {
                                long zero = 0;

                                this.Text = zero.ToString("#,##0 تومان");
                            }
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }


            }
        }

        protected override void OnPreviewLostKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);

            if (MaskType == MaskTypeEnum.Regex)
            {
                if(string.IsNullOrWhiteSpace(Text)==false)
                {
                    switch (RegEx)
                    {

                        case RegexType.EmailAddress:
                            {
                                System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.EmailAddress);

                                if (oRegex.IsMatch(Text) == false)
                                {
                                    InvalidMaskError = "پست الکترونیکی نامعتبر می‌باشد.";

                                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                    e.Handled = true;
                                }

                                break;
                            }

                        case RegexType.IranMobileNumber:
                            {
                                System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.IranMobilePhoneNumber);

                                if (oRegex.IsMatch(Text) == false)
                                {
                                    InvalidMaskError = "شماره تلفن همراه نامعتبر می‌باشد.";

                                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                    e.Handled = true;
                                }

                                break;
                            }

                        case RegexType.Username:
                            {
                                System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.Username);

                                if (oRegex.IsMatch(Text) == false)
                                {
                                    InvalidMaskError = "نام کاربری باید شامل حداقل 6 کاراکتر و حداکثر 20 کاراکتر و فقط می‌تواند شامل حروف لاتین، اعداد و _ باشد.";

                                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                    e.Handled = true;
                                }

                                break;
                            }

                        case RegexType.ShamsiDate:
                            {
                                System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.PersianDate);

                                if (oRegex.IsMatch(Text) == false)
                                {
                                    InvalidMaskError = "تاریخ درج شده نامعتبر می‌باشد.";

                                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                    e.Handled = true;
                                }
                                break;
                            }


                        case RegexType.PercentValue:
                            {
                                System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.PercentValue);

                                if (oRegex.IsMatch(Text) == false)
                                {
                                    InvalidMaskError = "مقدار درصد باید عددی بین 0 تا 100 باشد.";

                                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                    e.Handled = true;
                                }

                                break;
                            }

                        case RegexType.NationalCode:
                            {
                                System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.NationalCode);

                                if (oRegex.IsMatch(Text) == false)
                                {
                                    InvalidMaskError = "کد ملی بایست عددی 10 رقمی باشد.";

                                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                    e.Handled = true;
                                }

                                break;
                            }

                        case RegexType.Custom:
                            {
                                if ((string.IsNullOrWhiteSpace(Mask) == false))
                                {
                                    System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(Mask);

                                    if (oRegex.IsMatch(Text) == false)
                                    {
                                        Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: InvalidMaskError);

                                        e.Handled = true;
                                    }
                                }
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
        }


    }
}
