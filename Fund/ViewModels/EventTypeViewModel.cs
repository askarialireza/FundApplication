
namespace ViewModels
{
    public class EventTypeViewModel
    {
        public System.Guid Id { get; set; }

        public System.Windows.Media.ImageSource ListBoxItemLogo { get; set; }

        private Models.Event _eventType;

        public Models.Event EventType
        {
            get
            {
                return _eventType;
            }
            set
            {
                _eventType = value;

                PropertyChanged(_eventType);
            }
        }

        public string Description { get; set; }


        private void PropertyChanged(Models.Event eventType)
        {
            System.Uri oUri;

            switch (eventType)
            {
                case Models.Event.Text:
                    {
                        oUri = new System.Uri(@"/Fund;component/Resources/Icons/EditProperty_30px.png", System.UriKind.Relative);
                        ListBoxItemLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);

                        break;
                    }

                case Models.Event.Installment:
                    {
                        oUri = new System.Uri(@"/Fund;component/Resources/Icons/Schedule_30px.png", System.UriKind.Relative);
                        ListBoxItemLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);

                        break;
                    }
            }
        }
    }
}
