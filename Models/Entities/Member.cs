namespace Models
{
    public class Member : BaseEntity
    {

        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Member>
        {
            public Configuration() : base()
            {
                ToTable("Members");

                HasRequired(current => current.Fund)
                    .WithMany(fund => fund.Members)
                    .HasForeignKey(current => current.FundId)
                    .WillCascadeOnDelete(true);

                Property(current => current.Picture)
                    .IsMaxLength();
            }
        }

        #endregion /Configuration

        #region Constructor

        public Member() : base()
        {
            FullName = new ComplexTypes.FullName();

            MembershipDateTime = System.DateTime.Now;

            PersianMembershipDateTime = FarsiLibrary.Utils.PersianDateConverter
                .ToPersianDate(System.DateTime.Now).ToString("d");
        }

        #endregion /Constructor

        #region Properties

        public ComplexTypes.FullName FullName { get; set; }

        [System.ComponentModel.DataAnnotations.MinLength(2)]
        public string FatherName { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(10)]
        public string NationalCode { get; set; }

        public Gender GenderType { get; set; }

        public string GenderToString { get; set; }

        [System.ComponentModel.DataAnnotations.RegularExpression
            (pattern: Dtx.Text.RegularExpressions.Patterns.Email)]
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public System.DateTime MembershipDateTime { get; set; }

        public string PersianMembershipDateTime { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        public System.Guid FundId { get; set; }
        public virtual Fund Fund { get; set; }

        public virtual System.Collections.Generic.IList<Loan> Loans { get; set; }
        #endregion /Properties        

        #region Methods

        #endregion /Methods

    }
}
