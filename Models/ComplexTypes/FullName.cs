namespace Models.ComplexTypes
{
    [System.ComponentModel.DataAnnotations.Schema.ComplexType]
    public class FullName : object
    {
        #region Constructor
        public FullName() : base()
        {
        }

        #endregion /Constructor

        #region Properties
        public string FirstName { get; set; }

        public string LastName { get; set; }

        #endregion /Properties

        #region Methods
        public override string ToString()
        {
            string strResult = string.Empty;

            if (string.IsNullOrWhiteSpace(FirstName) == false)
            {
                strResult = FirstName.Trim();
            }

            if (string.IsNullOrWhiteSpace(LastName) == false)
            {
                strResult =
                    string.Format("{0} {1}",
                    strResult, LastName.Trim()).Trim();
            }

            return (strResult);
        }

        #endregion /Methods

    }
}
