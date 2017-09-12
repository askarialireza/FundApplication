
namespace ViewModels
{
    public class MembershipCardViewModel
    {
        public MembershipCardViewModel() : base()
        {
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string NationalCode { get; set; }

        public string FundName { get; set; }

        private Models.Gender _gender;
        public Models.Gender Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;

                GenderDescription = (_gender == Models.Gender.Male) ? "آقا" : "خانم";
            }
        }

        public string GenderDescription { get; set; }

        private System.DateTime _membershipDate;
        public System.DateTime MembershipDate
        {
            get
            {
                return _membershipDate;
            }
            set
            {
                _membershipDate = value;

                PersianMembershipDate = new FarsiLibrary.Utils.PersianDate(_membershipDate).ToString("d");
            }
        }

        public string PersianMembershipDate { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public System.Drawing.Image Picture { get; set; }
    }
}
