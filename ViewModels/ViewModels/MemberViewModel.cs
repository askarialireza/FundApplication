
namespace ViewModels
{
    public class MemberViewModel
    {
        public MemberViewModel()
        {
        }

        public System.Guid Id { get; set; }

        private Models.ComplexTypes.FullName _fullName;
        public Models.ComplexTypes.FullName FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value;

                MemberName = _fullName.ToString();
            }
        }

        public string MemberName { get; set; }

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

        private byte[] _picture;
        public byte[] Picture
        {
            get
            {
                return _picture;
            }
            set
            {
                _picture = value;

                if (_picture != null)
                {
                    System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(_picture);
                    System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                    Image = oImage;
                }
                else
                {
                    System.Drawing.Image oImage = null;
                    Image = oImage;
                }
            }
        }

        public System.Drawing.Image Image { get; set; }

    }
}
