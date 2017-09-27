
namespace ViewModels
{
    public class EmailViewModel : object
    {
        public EmailViewModel() : base()
        {
        }

        public string SenderEmail { get; set; }

        public string SenderPassword { get; set; }

        public string DisplayName { get; set; }

        public string ReceiverEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public System.IO.Stream Attachment { get; set; }

        public string AttachmentFileName { get; set; }
    }
}
