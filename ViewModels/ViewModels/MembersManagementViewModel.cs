
namespace ViewModels
{
    public class MembersManagementViewModel
    {
        public MembersManagementViewModel()
        {
        }

        public System.Guid Id { get; set; }

        public Models.ComplexTypes.FullName FullName { get; set; }

        public string FatherName { get; set; }

        public string NationalCode { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

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

    }
}
