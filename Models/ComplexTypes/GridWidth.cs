
namespace Models.ComplexTypes
{
    [System.ComponentModel.DataAnnotations.Schema.ComplexType]
    public class GridWidth : object
    {
        #region Constructor
        public GridWidth() : base()
        {
            FundPanelWidth = 0;
            MainPanelWidth = 0;
        }
        #endregion /Constructor

        public double FundPanelWidth { get; set; }

        public double MainPanelWidth { get; set; }
    }
}
