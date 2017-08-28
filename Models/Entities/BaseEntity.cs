namespace Models
{
    public class BaseEntity : object
    {

        #region Constructor

        public BaseEntity() : base()
        {
            Id = System.Guid.NewGuid();
        }

        #endregion /Constructor

        #region Properties

        public System.Guid Id { get; set; }

        #endregion /Properties        

    }
}
