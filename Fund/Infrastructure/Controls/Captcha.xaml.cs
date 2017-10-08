
namespace Infrastructure.Controls
{
    public partial class Captcha : System.Windows.Controls.UserControl
    {

        public static readonly System.Windows.DependencyProperty CaptchaValueProperty = System.Windows.DependencyProperty.Register("CaptchaValue", typeof(string), typeof(Captcha));

        public static readonly System.Windows.RoutedEvent CaptchaRefreshedEvent = System.Windows.EventManager.RegisterRoutedEvent("CaptchaRefreshed", System.Windows.RoutingStrategy.Bubble, typeof(System.Windows.RoutedEventHandler), typeof(Captcha));


        public string CaptchaValue
        {
            get
            {
                return (string)base.GetValue(CaptchaValueProperty);
            }
            set
            {
                base.SetValue(CaptchaValueProperty, value);
            }
        }

        public event System.Windows.RoutedEventHandler CaptchaRefreshed
        {
            add { AddHandler(routedEvent: CaptchaRefreshedEvent, handler: value); }

            remove { RemoveHandler(routedEvent: CaptchaRefreshedEvent, handler: value); }
        }

        private void RaiseCaptchaRefreshedEvent()
        {
            System.Windows.RoutedEventArgs oRoutedEventArgs = new System.Windows.RoutedEventArgs(Captcha.CaptchaRefreshedEvent);

            RaiseEvent(oRoutedEventArgs);
        }

        private void OnCaptchaRefreshed()
        {
            RaiseCaptchaRefreshedEvent();
        }

        public Captcha()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateCaptcha();
        }

        private void SpeechCaptchaButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Speech.Synthesis.SpeechSynthesizer oSpeechSynthesizer = new System.Speech.Synthesis.SpeechSynthesizer();

            oSpeechSynthesizer.Volume = 100;

            foreach (char objchar in CaptchaText.Text)
            {
                oSpeechSynthesizer.Speak(objchar.ToString());
            }
        }

        private void ResetCaptchaButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateCaptcha();
        }

        private void CreateCaptcha()
        {
            string allowchar = string.Empty;
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            string[] ar = allowchar.Split(a);
            string pwd = string.Empty;
            string temp = string.Empty;
            System.Random r = new System.Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];

                pwd += temp;
            }

            CaptchaText.Text = pwd;

            CaptchaValue = CaptchaText.Text;

            OnCaptchaRefreshed();
        }
    }
}
