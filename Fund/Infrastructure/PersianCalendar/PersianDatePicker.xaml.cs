using System.Linq;

namespace Fund
{
    public partial class PersianDatePicker
    {
        private string SelectedRectangleBaseName { get; set; }
        private System.Windows.Shapes.Rectangle PreviousRectangle { get; set; }
        private System.Collections.Generic.List<System.Windows.Shapes.Rectangle> Rectangles { get; set; }
        private System.Collections.Generic.List<System.Windows.Controls.TextBlock> TextBlocks { get; set; }
        private System.Collections.Generic.List<int> IntEventedIndex { get; set; }

        private FarsiLibrary.Utils.PersianDate _selectedDateTime;
        public FarsiLibrary.Utils.PersianDate SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
            }
        }
        public event System.EventHandler SelectedDateTimeChanged;


        #region Fields

        System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();

        //اطلاعات تاریخ امروز 
        readonly int currentYear = 1387;
        readonly int currentMonth = 10;
        readonly int currentDay = 1;

        //برای حرکت بین ماه ها
        //به شمسی
        int yearForNavigating = 1387;
        int monthForNavigating = 10;

        //Persian
        int yearPersian = 1387;
        int monthPersian = 01;
        int dayPersian = 01;

        #endregion

        #region Constructor

        public PersianDatePicker()
        {
            this.InitializeComponent();

            GetRectangles();
            GetTextBoxes();

            PreviousRectangle = Rectangles
                .Where(current => current.Style == (System.Windows.Style)FindResource("RectangleStyleToday"))
                .FirstOrDefault();

            PreviousRectangle.Style = (System.Windows.Style)FindResource("RectangleStyleNone");

            this.currentYear = persianCalendar.GetYear(System.DateTime.Now);
            this.currentMonth = persianCalendar.GetMonth(System.DateTime.Now);
            this.currentDay = persianCalendar.GetDayOfMonth(System.DateTime.Now);

            CalculateMonth(currentYear, currentMonth);
        }

        #endregion

        #region Calculating and Showing the Calendar


        void CalculateMonth(int thisYear, int thisMonth)
        {
            IntEventedIndex = new System.Collections.Generic.List<int>();

            try
            {
                yearForNavigating = thisYear;
                monthForNavigating = thisMonth;

                System.DateTime tempDateTime = persianCalendar.ToDateTime(yearForNavigating, monthForNavigating, 15, 01, 01, 01, 01);

                int thisDay = 1;
                TextBlockThisMonth.Text = string.Empty;
                TextBlockThisMonth.Text =
                    monthForNavigating.ConvertToPersianMonth() + " " +
                    yearForNavigating.ConvertToPersianNumber();

                //Different between first place of calendar and first place of this month
                //اختلاف بین خانه شروع ماه و اولین خانه تقویم            
                string DayOfWeek = persianCalendar.GetDayOfWeek(persianCalendar.ToDateTime(thisYear, thisMonth, 01, 01, 01, 01, 01)).ToString();

                int span = CalculatePersianSpan(DayOfWeek.ConvertToPersianDay());

                DecreasePersianDay(ref thisYear, ref thisMonth, ref thisDay, span);

                string persianDate;//حاوی تاریخ روزهای شمسی Contains the date of Persian

                ////////////////////////////////////

                for (int i = 0; i < 6 * 7; i++)
                {
                    tempDateTime = persianCalendar.ToDateTime(thisYear, thisMonth, thisDay, 01, 01, 01, 01);

                    persianDate = thisDay.ConvertToPersianNumber();

                    DayOfWeek = persianCalendar.GetDayOfWeek(tempDateTime).ToString();
                    //RectangleStyleEventedDay
                    if (thisMonth == monthForNavigating) // ماه کنونی 
                    {
                        if (thisDay == currentDay && thisMonth == currentMonth && thisYear == currentYear) // امروز
                        {
                            if (GetEventCountOfDay(thisYear, thisMonth, thisDay) != 0)
                            {
                                IntEventedIndex.Add(i);
                            }

                            ChangeProperties(i, persianDate, "RectangleStyleToday", "TextBlockStyle1");

                            SelectedDateTime = new FarsiLibrary.Utils.PersianDate(thisYear, thisMonth, thisDay);

                            SelectedDateTimeChanged?.Invoke(this, System.EventArgs.Empty);
                        }

                        else // سایر روزهای ماه کنونی
                        {
                            if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0)
                            {
                                ChangeProperties(i, persianDate, "RectangleStyleNone", "TextBlockStyle1");
                            }
                            else
                            {
                                ChangeProperties(i, persianDate, "RectangleStyleEventedDay", "TextBlockStyle1");
                                IntEventedIndex.Add(i);
                            }
                        }
                    }

                    else // ماه های دیگر
                    {
                        ChangeProperties(i, persianDate, "RectangleStyleNone", "TextBlockStyleForOtherMonths");
                    }

                    IncreasePersianDay(ref thisYear, ref thisMonth, ref thisDay, 1);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        void IncreasePersianMonth(ref int year, ref int month, int number)
        {
            month += number;
            if (month > 12)
            {
                month = 1;
                year++;
            }
        }

        void DecreasePersianMonth(ref int year, ref int month, int number)
        {
            month -= number;
            if (month < 1)
            {
                month = 12;
                year--;
            }
        }

        void IncreasePersianDay(ref int year, ref int month, ref int day, int number)
        {
            int tempDay = day;
            tempDay += number;
            //شش ماه اول سال
            if (month <= 6 && tempDay > 31)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            //5 ماه دوم سال 
            else if (month > 6 && month < 12 && tempDay > 30)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            //اسفند در سال کبیسه
            else if (month == 12 && persianCalendar.IsLeapYear(year) && tempDay > 30)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            //اسفند در سال غیر کبیسه
            else if (month == 12 && !persianCalendar.IsLeapYear(year) && tempDay > 29)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            else
                day += number;
        }

        void DecreasePersianDay(ref int year, ref int month, ref int day, int number)
        {
            int tempDay = day;
            tempDay -= number;
            //شش ماه اول سال
            if (month == 1 && tempDay < 1)
            {
                if (persianCalendar.IsLeapYear(year - 1))
                    day = 30 - number + 1;//+1 رو باید اضافه کرد در غیر این صورت محاسبات اشتباه میشوند ، تجربی
                else
                    day = 29 - number + 1;
                DecreasePersianMonth(ref year, ref month, 1);
            }
            else if (month <= 7 && month > 1 && tempDay < 1)
            {
                day = 31 - number + 1;
                month--;
            }
            //6 ماه دوم سال 
            else if (month > 7 && month <= 12 && tempDay < 1)
            {
                day = 30 - number + 1;
                DecreasePersianMonth(ref year, ref month, 1);
            }
            else
                day -= number;

        }

        int CalculatePersianSpan(string weekday)
        {
            switch (weekday)
            {
                case "شنبه":
                    return 0;

                case "یک شنبه":
                    return 1;

                case "دو شنبه":
                    return 2;

                case "سه شنبه":
                    return 3;

                case "چهار شنبه":
                    return 4;

                case "پنج شنبه":
                    return 5;

                case "جمعه":
                    return 6;

                default:
                    return 0;
            }
        }

        void ChangeProperties(int which, string persianDate, string rectangleResourceName, string persianTextBlockResourceName)
        {
            Rectangles.ElementAt(which)
                .Style = (System.Windows.Style)FindResource(rectangleResourceName);

            TextBlocks.ElementAt(which)
                .Text = persianDate;

            TextBlocks.ElementAt(which)
                .Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
        }

        #endregion

        #region Events

        void nextMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IncreasePersianMonth(ref yearForNavigating, ref monthForNavigating, 1);
            CalculateMonth(yearForNavigating, monthForNavigating);
        }

        void previousMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DecreasePersianMonth(ref yearForNavigating, ref monthForNavigating, 1);
            CalculateMonth(yearForNavigating, monthForNavigating);
        }

        void previousYear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.yearForNavigating--;
            CalculateMonth(yearForNavigating, monthForNavigating);
        }

        void nextYear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.yearForNavigating++;
            CalculateMonth(yearForNavigating, monthForNavigating);
        }

        void goToToday_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CalculateMonth(this.currentYear, this.currentMonth);
        }

        public void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            int todayIndex;

            foreach (System.Windows.Shapes.Rectangle item in Rectangles)
            {
                item.Style = (System.Windows.Style)FindResource("RectangleStyleNone");

                if (item.Style == (System.Windows.Style)FindResource("RectangleStyleToday"))
                {
                    todayIndex = Rectangles.IndexOf(item);
                }
            }

            SelectedRectangleBaseName = System.Text.RegularExpressions.Regex.Replace((sender as System.Windows.Controls.Grid).Name, @"Grid", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Windows.Shapes.Rectangle SelectedRectangle = FindName("Rectangle" + SelectedRectangleBaseName) as System.Windows.Shapes.Rectangle;

            int Index = Rectangles.IndexOf(SelectedRectangle);

            System.Windows.Controls.TextBlock SelectedTextBlock = TextBlocks.ElementAt(Index);

            if ((SelectedRectangle.Style == FindResource("RectangleStyleNone") && SelectedTextBlock.Style == FindResource("TextBlockStyleForOtherMonths")))
                return;

            foreach (int index in IntEventedIndex)
            {

                (Rectangles.ElementAt(index)).Style = (System.Windows.Style)FindResource("RectangleStyleEventedDay");

            }

            SelectedRectangle.Style = (System.Windows.Style)FindResource("RectangleStyleToday");

            PreviousRectangle = SelectedRectangle;

            ////بدست آوردن اطلاعات امروز
            this.yearPersian = this.yearForNavigating;
            this.monthPersian = this.monthForNavigating;
            this.dayPersian = (FindName("TextBlock" + SelectedRectangleBaseName) as System.Windows.Controls.TextBlock).Text.ConvertToInteger();

            SelectedDateTime = new FarsiLibrary.Utils.PersianDate(yearPersian, monthPersian, dayPersian);

            SelectedDateTimeChanged?.Invoke(this, System.EventArgs.Empty);
        }

        int previousIndex = -1;
        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            string BaseName = System.Text.RegularExpressions.Regex.Replace((sender as System.Windows.Controls.Grid).Name, @"Grid", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Windows.Shapes.Rectangle EnteredRectangle = FindName("Rectangle" + BaseName) as System.Windows.Shapes.Rectangle;

            int Index = Rectangles.IndexOf(EnteredRectangle);

            if (Rectangles.ElementAt(Index).Style == FindResource("RectangleStyleEventedDay"))
            {
                previousIndex = Index;
            }

            System.Windows.Controls.TextBlock EnteredTextBlock = TextBlocks.ElementAt(Index);


            if ((EnteredRectangle.Style == FindResource("RectangleStyleNone") && EnteredTextBlock.Style == FindResource("TextBlockStyleForOtherMonths")) ||
              EnteredRectangle.Style == FindResource("RectangleStyleToday"))
                return;


            EnteredRectangle.Style = (System.Windows.Style)FindResource("RectangleStyleMouseEntered");

        }

        private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

            string BaseName = System.Text.RegularExpressions.Regex.Replace((sender as System.Windows.Controls.Grid).Name, @"Grid", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Windows.Shapes.Rectangle LeavedRectangle = FindName("Rectangle" + BaseName) as System.Windows.Shapes.Rectangle;

            int Index = Rectangles.IndexOf(LeavedRectangle);
            if (LeavedRectangle.Style != FindResource("RectangleStyleToday"))
            {
                if (previousIndex == Index)
                {
                    LeavedRectangle.Style = (System.Windows.Style)FindResource("RectangleStyleEventedDay");
                    previousIndex = -1;
                    return;
                }
            }

            System.Windows.Controls.TextBlock LeavedTextBlock = TextBlocks.ElementAt(Index);

            if ((LeavedRectangle.Style == FindResource("RectangleStyleNone") && LeavedTextBlock.Style == FindResource("TextBlockStyleForOtherMonths")) ||
                LeavedRectangle.Style == FindResource("RectangleStyleToday") ||
                LeavedRectangle.Style == FindResource("RectangleStyleEventedDay"))
                return;

            LeavedRectangle.Style = (System.Windows.Style)FindResource("RectangleStyleNone");

        }

        #endregion

        private void GetRectangles()
        {
            Rectangles =
                new System.Collections.Generic.List<System.Windows.Shapes.Rectangle>();

            Rectangles.Add(RectangleShanbe0);
            Rectangles.Add(Rectangle1Shanbe0);
            Rectangles.Add(Rectangle2Shanbe0);
            Rectangles.Add(Rectangle3Shanbe0);
            Rectangles.Add(Rectangle4Shanbe0);
            Rectangles.Add(Rectangle5Shanbe0);
            Rectangles.Add(RectangleJome0);

            Rectangles.Add(RectangleShanbe1);
            Rectangles.Add(Rectangle1Shanbe1);
            Rectangles.Add(Rectangle2Shanbe1);
            Rectangles.Add(Rectangle3Shanbe1);
            Rectangles.Add(Rectangle4Shanbe1);
            Rectangles.Add(Rectangle5Shanbe1);
            Rectangles.Add(RectangleJome1);

            Rectangles.Add(RectangleShanbe2);
            Rectangles.Add(Rectangle1Shanbe2);
            Rectangles.Add(Rectangle2Shanbe2);
            Rectangles.Add(Rectangle3Shanbe2);
            Rectangles.Add(Rectangle4Shanbe2);
            Rectangles.Add(Rectangle5Shanbe2);
            Rectangles.Add(RectangleJome2);

            Rectangles.Add(RectangleShanbe3);
            Rectangles.Add(Rectangle1Shanbe3);
            Rectangles.Add(Rectangle2Shanbe3);
            Rectangles.Add(Rectangle3Shanbe3);
            Rectangles.Add(Rectangle4Shanbe3);
            Rectangles.Add(Rectangle5Shanbe3);
            Rectangles.Add(RectangleJome3);

            Rectangles.Add(RectangleShanbe4);
            Rectangles.Add(Rectangle1Shanbe4);
            Rectangles.Add(Rectangle2Shanbe4);
            Rectangles.Add(Rectangle3Shanbe4);
            Rectangles.Add(Rectangle4Shanbe4);
            Rectangles.Add(Rectangle5Shanbe4);
            Rectangles.Add(RectangleJome4);

            Rectangles.Add(RectangleShanbe5);
            Rectangles.Add(Rectangle1Shanbe5);
            Rectangles.Add(Rectangle2Shanbe5);
            Rectangles.Add(Rectangle3Shanbe5);
            Rectangles.Add(Rectangle4Shanbe5);
            Rectangles.Add(Rectangle5Shanbe5);
            Rectangles.Add(RectangleJome5);
        }
        private void GetTextBoxes()
        {
            TextBlocks =
                new System.Collections.Generic.List<System.Windows.Controls.TextBlock>();

            TextBlocks.Add(TextBlockShanbe0);
            TextBlocks.Add(TextBlock1Shanbe0);
            TextBlocks.Add(TextBlock2Shanbe0);
            TextBlocks.Add(TextBlock3Shanbe0);
            TextBlocks.Add(TextBlock4Shanbe0);
            TextBlocks.Add(TextBlock5Shanbe0);
            TextBlocks.Add(TextBlockJome0);

            TextBlocks.Add(TextBlockShanbe1);
            TextBlocks.Add(TextBlock1Shanbe1);
            TextBlocks.Add(TextBlock2Shanbe1);
            TextBlocks.Add(TextBlock3Shanbe1);
            TextBlocks.Add(TextBlock4Shanbe1);
            TextBlocks.Add(TextBlock5Shanbe1);
            TextBlocks.Add(TextBlockJome1);

            TextBlocks.Add(TextBlockShanbe2);
            TextBlocks.Add(TextBlock1Shanbe2);
            TextBlocks.Add(TextBlock2Shanbe2);
            TextBlocks.Add(TextBlock3Shanbe2);
            TextBlocks.Add(TextBlock4Shanbe2);
            TextBlocks.Add(TextBlock5Shanbe2);
            TextBlocks.Add(TextBlockJome2);

            TextBlocks.Add(TextBlockShanbe3);
            TextBlocks.Add(TextBlock1Shanbe3);
            TextBlocks.Add(TextBlock2Shanbe3);
            TextBlocks.Add(TextBlock3Shanbe3);
            TextBlocks.Add(TextBlock4Shanbe3);
            TextBlocks.Add(TextBlock5Shanbe3);
            TextBlocks.Add(TextBlockJome3);

            TextBlocks.Add(TextBlockShanbe4);
            TextBlocks.Add(TextBlock1Shanbe4);
            TextBlocks.Add(TextBlock2Shanbe4);
            TextBlocks.Add(TextBlock3Shanbe4);
            TextBlocks.Add(TextBlock4Shanbe4);
            TextBlocks.Add(TextBlock5Shanbe4);
            TextBlocks.Add(TextBlockJome4);

            TextBlocks.Add(TextBlockShanbe5);
            TextBlocks.Add(TextBlock1Shanbe5);
            TextBlocks.Add(TextBlock2Shanbe5);
            TextBlocks.Add(TextBlock3Shanbe5);
            TextBlocks.Add(TextBlock4Shanbe5);
            TextBlocks.Add(TextBlock5Shanbe5);
            TextBlocks.Add(TextBlockJome5);
        }

        int GetEventCountOfDay(int year, int month, int day)
        {

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                int count = oUnitOfWork.RemainderRepository
                    .GetByPersianDate(year, month, day)
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .Count();

                return count;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }
            }
        }
        string GetTextOfMemo(int year, int month, int day)
        {

            int count = GetEventCountOfDay(year, month, day);


            switch (count)
            {
                case 0:
                    return "هیچ رویدادی برای این روز ثبت نشده است";
                default:
                    return (string.Format("{0} رویداد ثبت شده است", FarsiLibrary.Utils.ToWords.ToString(count)));
            }
        }

    }


}
