using System.Linq;

namespace Fund
{
    public partial class PersianCalendar
    {
        public event System.EventHandler DayEventsChanged;
        public int HijriAdjustment { get; set; }
        #region Fields

        System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
        System.Globalization.HijriCalendar hijriCalendar = new System.Globalization.HijriCalendar();

        //اطلاعات تاریخ امروز 
        readonly int currentYear = 1387;
        readonly int currentMonth = 10;
        readonly int currentDay = 1;

        //برای حرکت بین ماه ها
        //به شمسی
        int yearForNavigating = 1387;
        int monthForNavigating = 10;

        //اطلاعات روزی که کاربر روی آن کلیک کرده
        //Christian
        int yearChristian = 2009;
        int monthChristian = 01;
        int dayChristian = 01;

        //Persian
        int yearPersian = 1387;
        int monthPersian = 01;
        int dayPersian = 01;

        //Hijri
        int yearHijri = 1387;
        int monthHijri = 01;
        int dayHijri = 01;

        #endregion

        #region Constructor

        public PersianCalendar()
        {
            HijriAdjustment = ReadHijriAdjustmentValue();

            this.InitializeComponent();

            this.currentYear = persianCalendar.GetYear(System.DateTime.Now);
            this.currentMonth = persianCalendar.GetMonth(System.DateTime.Now);
            this.currentDay = persianCalendar.GetDayOfMonth(System.DateTime.Now);

            hijriCalendar.HijriAdjustment = HijriAdjustment;
            TextBoxHijriAdjustment.Text = hijriCalendar.HijriAdjustment.ToString();

            CalculateMonth(currentYear, currentMonth);
        }

        #endregion

        #region Calculating and Showing the Calendar

        void CalculateMonth(int thisYear, int thisMonth)
        {
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
                string christianDate;//حاوی تاریخ روزهای میلادی Contains the date of Christian
                string hijriDate;//حاوی تاریخ روزهای قمری Contains the date of Hijri

                string TooltipContext = string.Empty;//Contains the text of tooltip

                ////////////////////////////////////

                for (int i = 0; i < 6 * 7; i++)
                {
                    tempDateTime = persianCalendar.ToDateTime(thisYear, thisMonth, thisDay, 01, 01, 01, 01);

                    christianDate = tempDateTime.Day.ToString() + " " + EnglishMonthName(tempDateTime.Month) + " " + tempDateTime.Year.ToString();
                    hijriDate = hijriCalendar.GetDayOfMonth(tempDateTime).ConvertToPersianNumber() + " " + hijriCalendar.GetMonth(tempDateTime).ConvertToHigriMonth() + " " + hijriCalendar.GetYear(tempDateTime).ConvertToPersianNumber();
                    persianDate = thisDay.ConvertToPersianNumber();

                    DayOfWeek = persianCalendar.GetDayOfWeek(tempDateTime).ToString();

                    TooltipContext = string.Empty;

                    if (thisMonth == monthForNavigating) // ماه کنونی 
                    {

                        if (thisDay == currentDay && thisMonth == currentMonth && thisYear == currentYear) // امروز
                        {
                            TooltipContext = GetTextOfMemo(thisYear, thisMonth, thisDay);

                            if (DayOfWeek.ConvertToPersianDay() == "جمعه") // امروز جمعه است
                            {
                                if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0) // امروز جمعه رویدادی ندارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleForFridayToday", "TextBlockStyle3", "TextBlockStyle12", "TextBlockStyle8", TooltipContext);
                                }
                                else // امروز جمعه رویداد دارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleForEventedFridayToday", "TextBlockStyle3", "TextBlockStyle12", "TextBlockStyle8", TooltipContext);
                                }
                            }
                            else // امروز جمعه نیست
                            {
                                if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0) // امروز رویدادی ندارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleToday", "TextBlockStyle5", "TextBlockStyle10", "TextBlockStyle6", TooltipContext);
                                }
                                else // امروز رویداد دارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleEventedToday", "TextBlockStyle5", "TextBlockStyle10", "TextBlockStyle6", TooltipContext);
                                }

                            }
                        }

                        else // سایر روزهای ماه کنونی
                        {
                            TooltipContext = GetTextOfMemo(thisYear, thisMonth, thisDay);

                            if (DayOfWeek.ConvertToPersianDay() == "جمعه") // روز جمعه است
                            {
                                if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0) // روز جمعه رویداد ندارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleForHolydays", "TextBlockStyle3", "TextBlockStyle12", "TextBlockStyle8", TooltipContext);
                                }
                                else // روز جمعه رویداد دارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleForEventedFriday", "TextBlockStyle3", "TextBlockStyle12", "TextBlockStyle8", TooltipContext);
                                }
                            }
                            else // روز جمعه نیست
                            {
                                if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0) // روز عادی رویداد ندارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyle2", "TextBlockStyle5", "TextBlockStyle10", "TextBlockStyle6", TooltipContext);
                                }
                                else // روز عادی رویداد دارد
                                {
                                    ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleEventedDay", "TextBlockStyle5", "TextBlockStyle10", "TextBlockStyle6", TooltipContext);
                                }
                            }
                        }
                    }

                    else // ماه های دیگر
                    {
                        TooltipContext = GetTextOfMemo(thisYear, thisMonth, thisDay);

                        if (DayOfWeek.ConvertToPersianDay() == "جمعه")// روز جمعه در ماه های دیگر
                        {
                            if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0) // رویداد نداشتن روز جمعه در ماه های دیگر
                            {
                                ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleForOtherHolydays", "TextBlockStyle4", "TextBlockStyle13", "TextBlockStyle9", TooltipContext);
                            }
                            else // رویداد داشتن روز جمعه در ماه های دیگر
                            {
                                ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleForOtherEventedFriday", "TextBlockStyle4", "TextBlockStyle13", "TextBlockStyle9", TooltipContext);
                            }
                        }
                        else // سایر روز های هفته در ماه های دیگر
                        {
                            if (GetEventCountOfDay(thisYear, thisMonth, thisDay) == 0) // رویداد نداشتن روزهای هفته در ماه های دیگر
                            {
                                ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleOtherMonths", "TextBlockStyle2", "TextBlockStyle11", "TextBlockStyle7", TooltipContext);
                            }
                            else // رویداد داشتن سایر روزهای هفته در ماه های دیگر
                            {
                                ChangeProperties(i, persianDate, hijriDate, christianDate, "RectangleStyleEventedOtherMonths", "TextBlockStyle2", "TextBlockStyle11", "TextBlockStyle7", TooltipContext);
                            }
                        }
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

        string EnglishMonthName(int monthNumber)
        {
            switch (monthNumber)
            {
                case 01:
                    return "Jan";

                case 02:
                    return "Feb";

                case 03:
                    return "Mar";

                case 04:
                    return "Apr";

                case 05:
                    return "May";

                case 06:
                    return "Jun";

                case 07:
                    return "Jul";

                case 08:
                    return "Aug";

                case 09:
                    return "Sep";

                case 10:
                    return "Oct";

                case 11:
                    return "Nov";

                case 12:
                    return "Dec";

                default:
                    return string.Empty;
            }
        }

        void ChangeProperties(int which, string persianDate, string hijriDate, string miladiDate, string rectangleResourceName, string persianTextBlockResourceName, string hijriTextBlockResourceName, string miladiTextBlockResourceName, string tooltip_context)
        {
            switch (which)
            {
                case 0:
                    TextBlockShanbe0.Text = persianDate;
                    TextBlockShanbe0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockShanbe0Miladi.Text = miladiDate;
                    TextBlockShanbe0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockShanbe0Hijri.Text = hijriDate;
                    TextBlockShanbe0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleShanbe0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridShanbe0.ToolTip = tooltip_context;
                    else GridShanbe0.ToolTip = null;
                    break;

                case 1:
                    TextBlock1Shanbe0.Text = persianDate;
                    TextBlock1Shanbe0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock1Shanbe0Miladi.Text = miladiDate;
                    TextBlock1Shanbe0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock1Shanbe0Hijri.Text = hijriDate;
                    TextBlock1Shanbe0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle1Shanbe0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid1Shanbe0.ToolTip = tooltip_context;
                    else Grid1Shanbe0.ToolTip = null;
                    break;

                case 2:
                    TextBlock2Shanbe0.Text = persianDate;
                    TextBlock2Shanbe0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock2Shanbe0Miladi.Text = miladiDate;
                    TextBlock2Shanbe0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock2Shanbe0Hijri.Text = hijriDate;
                    TextBlock2Shanbe0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle2Shanbe0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid2Shanbe0.ToolTip = tooltip_context;
                    else Grid2Shanbe0.ToolTip = null;
                    break;

                case 3:
                    TextBlock3Shanbe0.Text = persianDate;
                    TextBlock3Shanbe0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock3Shanbe0Miladi.Text = miladiDate;
                    TextBlock3Shanbe0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock3Shanbe0Hijri.Text = hijriDate;
                    TextBlock3Shanbe0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle3Shanbe0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid3Shanbe0.ToolTip = tooltip_context;
                    else Grid3Shanbe0.ToolTip = null;
                    break;

                case 4:
                    TextBlock4Shanbe0.Text = persianDate;
                    TextBlock4Shanbe0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock4Shanbe0Miladi.Text = miladiDate;
                    TextBlock4Shanbe0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock4Shanbe0Hijri.Text = hijriDate;
                    TextBlock4Shanbe0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle4Shanbe0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid4Shanbe0.ToolTip = tooltip_context;
                    else Grid4Shanbe0.ToolTip = null;
                    break;

                case 5:
                    TextBlock5Shanbe0.Text = persianDate;
                    TextBlock5Shanbe0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock5Shanbe0Miladi.Text = miladiDate;
                    TextBlock5Shanbe0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock5Shanbe0Hijri.Text = hijriDate;
                    TextBlock5Shanbe0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle5Shanbe0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid5Shanbe0.ToolTip = tooltip_context;
                    else Grid5Shanbe0.ToolTip = null;
                    break;

                case 6:
                    TextBlockJome0.Text = persianDate;
                    TextBlockJome0.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockJome0Miladi.Text = miladiDate;
                    TextBlockJome0Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockJome0Hijri.Text = hijriDate;
                    TextBlockJome0Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleJome0.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridJome0.ToolTip = tooltip_context;
                    else GridJome0.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 7:
                    TextBlockShanbe1.Text = persianDate;
                    TextBlockShanbe1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockShanbe1Miladi.Text = miladiDate;
                    TextBlockShanbe1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockShanbe1Hijri.Text = hijriDate;
                    TextBlockShanbe1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleShanbe1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridShanbe1.ToolTip = tooltip_context;
                    else GridShanbe1.ToolTip = null;
                    break;

                case 8:
                    TextBlock1Shanbe1.Text = persianDate;
                    TextBlock1Shanbe1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock1Shanbe1Miladi.Text = miladiDate;
                    TextBlock1Shanbe1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock1Shanbe1Hijri.Text = hijriDate;
                    TextBlock1Shanbe1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle1Shanbe1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid1Shanbe1.ToolTip = tooltip_context;
                    else Grid1Shanbe1.ToolTip = null;
                    break;

                case 9:
                    TextBlock2Shanbe1.Text = persianDate;
                    TextBlock2Shanbe1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock2Shanbe1Miladi.Text = miladiDate;
                    TextBlock2Shanbe1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock2Shanbe1Hijri.Text = hijriDate;
                    TextBlock2Shanbe1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle2Shanbe1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid2Shanbe1.ToolTip = tooltip_context;
                    else Grid2Shanbe1.ToolTip = null;
                    break;

                case 10:
                    TextBlock3Shanbe1.Text = persianDate;
                    TextBlock3Shanbe1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock3Shanbe1Miladi.Text = miladiDate;
                    TextBlock3Shanbe1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock3Shanbe1Hijri.Text = hijriDate;
                    TextBlock3Shanbe1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle3Shanbe1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid3Shanbe1.ToolTip = tooltip_context;
                    else Grid3Shanbe1.ToolTip = null;
                    break;

                case 11:
                    TextBlock4Shanbe1.Text = persianDate;
                    TextBlock4Shanbe1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock4Shanbe1Miladi.Text = miladiDate;
                    TextBlock4Shanbe1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock4Shanbe1Hijri.Text = hijriDate;
                    TextBlock4Shanbe1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle4Shanbe1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid4Shanbe1.ToolTip = tooltip_context;
                    else Grid4Shanbe1.ToolTip = null;
                    break;

                case 12:
                    TextBlock5Shanbe1.Text = persianDate;
                    TextBlock5Shanbe1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock5Shanbe1Miladi.Text = miladiDate;
                    TextBlock5Shanbe1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock5Shanbe1Hijri.Text = hijriDate;
                    TextBlock5Shanbe1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle5Shanbe1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid5Shanbe1.ToolTip = tooltip_context;
                    else Grid5Shanbe1.ToolTip = null;
                    break;

                case 13:
                    TextBlockJome1.Text = persianDate;
                    TextBlockJome1.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockJome1Miladi.Text = miladiDate;
                    TextBlockJome1Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockJome1Hijri.Text = hijriDate;
                    TextBlockJome1Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleJome1.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridJome1.ToolTip = tooltip_context;
                    else GridJome1.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 14:
                    TextBlockShanbe2.Text = persianDate;
                    TextBlockShanbe2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockShanbe2Miladi.Text = miladiDate;
                    TextBlockShanbe2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockShanbe2Hijri.Text = hijriDate;
                    TextBlockShanbe2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleShanbe2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridShanbe2.ToolTip = tooltip_context;
                    else GridShanbe2.ToolTip = null;
                    break;

                case 15:
                    TextBlock1Shanbe2.Text = persianDate;
                    TextBlock1Shanbe2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock1Shanbe2Miladi.Text = miladiDate;
                    TextBlock1Shanbe2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock1Shanbe2Hijri.Text = hijriDate;
                    TextBlock1Shanbe2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle1Shanbe2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid1Shanbe2.ToolTip = tooltip_context;
                    else Grid1Shanbe2.ToolTip = null;
                    break;

                case 16:
                    TextBlock2Shanbe2.Text = persianDate;
                    TextBlock2Shanbe2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock2Shanbe2Miladi.Text = miladiDate;
                    TextBlock2Shanbe2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock2Shanbe2Hijri.Text = hijriDate;
                    TextBlock2Shanbe2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle2Shanbe2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid2Shanbe2.ToolTip = tooltip_context;
                    else Grid2Shanbe2.ToolTip = null;
                    break;

                case 17:
                    TextBlock3Shanbe2.Text = persianDate;
                    TextBlock3Shanbe2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock3Shanbe2Miladi.Text = miladiDate;
                    TextBlock3Shanbe2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock3Shanbe2Hijri.Text = hijriDate;
                    TextBlock3Shanbe2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle3Shanbe2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid3Shanbe2.ToolTip = tooltip_context;
                    else Grid3Shanbe2.ToolTip = null;
                    break;

                case 18:
                    TextBlock4Shanbe2.Text = persianDate;
                    TextBlock4Shanbe2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock4Shanbe2Miladi.Text = miladiDate;
                    TextBlock4Shanbe2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock4Shanbe2Hijri.Text = hijriDate;
                    TextBlock4Shanbe2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle4Shanbe2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid4Shanbe2.ToolTip = tooltip_context;
                    else Grid4Shanbe2.ToolTip = null;
                    break;

                case 19:
                    TextBlock5Shanbe2.Text = persianDate;
                    TextBlock5Shanbe2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock5Shanbe2Miladi.Text = miladiDate;
                    TextBlock5Shanbe2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock5Shanbe2Hijri.Text = hijriDate;
                    TextBlock5Shanbe2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle5Shanbe2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid5Shanbe2.ToolTip = tooltip_context;
                    else Grid5Shanbe2.ToolTip = null;
                    break;

                case 20:
                    TextBlockJome2.Text = persianDate;
                    TextBlockJome2.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockJome2Miladi.Text = miladiDate;
                    TextBlockJome2Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockJome2Hijri.Text = hijriDate;
                    TextBlockJome2Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleJome2.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridJome2.ToolTip = tooltip_context;
                    else GridJome2.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 21:
                    TextBlockShanbe3.Text = persianDate;
                    TextBlockShanbe3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockShanbe3Miladi.Text = miladiDate;
                    TextBlockShanbe3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockShanbe3Hijri.Text = hijriDate;
                    TextBlockShanbe3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleShanbe3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridShanbe3.ToolTip = tooltip_context;
                    else GridShanbe3.ToolTip = null;
                    break;

                case 22:
                    TextBlock1Shanbe3.Text = persianDate;
                    TextBlock1Shanbe3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock1Shanbe3Miladi.Text = miladiDate;
                    TextBlock1Shanbe3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock1Shanbe3Hijri.Text = hijriDate;
                    TextBlock1Shanbe3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle1Shanbe3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid1Shanbe3.ToolTip = tooltip_context;
                    else Grid1Shanbe3.ToolTip = null;
                    break;

                case 23:
                    TextBlock2Shanbe3.Text = persianDate;
                    TextBlock2Shanbe3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock2Shanbe3Miladi.Text = miladiDate;
                    TextBlock2Shanbe3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock2Shanbe3Hijri.Text = hijriDate;
                    TextBlock2Shanbe3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle2Shanbe3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid2Shanbe3.ToolTip = tooltip_context;
                    else Grid2Shanbe3.ToolTip = null;
                    break;

                case 24:
                    TextBlock3Shanbe3.Text = persianDate;
                    TextBlock3Shanbe3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock3Shanbe3Miladi.Text = miladiDate;
                    TextBlock3Shanbe3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock3Shanbe3Hijri.Text = hijriDate;
                    TextBlock3Shanbe3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle3Shanbe3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid3Shanbe3.ToolTip = tooltip_context;
                    else Grid3Shanbe3.ToolTip = null;
                    break;

                case 25:
                    TextBlock4Shanbe3.Text = persianDate;
                    TextBlock4Shanbe3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock4Shanbe3Miladi.Text = miladiDate;
                    TextBlock4Shanbe3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock4Shanbe3Hijri.Text = hijriDate;
                    TextBlock4Shanbe3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle4Shanbe3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid4Shanbe3.ToolTip = tooltip_context;
                    else Grid4Shanbe3.ToolTip = null;
                    break;

                case 26:
                    TextBlock5Shanbe3.Text = persianDate;
                    TextBlock5Shanbe3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock5Shanbe3Miladi.Text = miladiDate;
                    TextBlock5Shanbe3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock5Shanbe3Hijri.Text = hijriDate;
                    TextBlock5Shanbe3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle5Shanbe3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid5Shanbe3.ToolTip = tooltip_context;
                    else Grid5Shanbe3.ToolTip = null;
                    break;

                case 27:
                    TextBlockJome3.Text = persianDate;
                    TextBlockJome3.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockJome3Miladi.Text = miladiDate;
                    TextBlockJome3Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockJome3Hijri.Text = hijriDate;
                    TextBlockJome3Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleJome3.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridJome3.ToolTip = tooltip_context;
                    else GridJome3.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 28:
                    TextBlockShanbe4.Text = persianDate;
                    TextBlockShanbe4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockShanbe4Miladi.Text = miladiDate;
                    TextBlockShanbe4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockShanbe4Hijri.Text = hijriDate;
                    TextBlockShanbe4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleShanbe4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridShanbe4.ToolTip = tooltip_context;
                    else GridShanbe4.ToolTip = null;
                    break;

                case 29:
                    TextBlock1Shanbe4.Text = persianDate;
                    TextBlock1Shanbe4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock1Shanbe4Miladi.Text = miladiDate;
                    TextBlock1Shanbe4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock1Shanbe4Hijri.Text = hijriDate;
                    TextBlock1Shanbe4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle1Shanbe4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid1Shanbe4.ToolTip = tooltip_context;
                    else Grid1Shanbe4.ToolTip = null;
                    break;

                case 30:
                    TextBlock2Shanbe4.Text = persianDate;
                    TextBlock2Shanbe4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock2Shanbe4Miladi.Text = miladiDate;
                    TextBlock2Shanbe4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock2Shanbe4Hijri.Text = hijriDate;
                    TextBlock2Shanbe4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle2Shanbe4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid2Shanbe4.ToolTip = tooltip_context;
                    else Grid2Shanbe4.ToolTip = null;
                    break;

                case 31:
                    TextBlock3Shanbe4.Text = persianDate;
                    TextBlock3Shanbe4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock3Shanbe4Miladi.Text = miladiDate;
                    TextBlock3Shanbe4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock3Shanbe4Hijri.Text = hijriDate;
                    TextBlock3Shanbe4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle3Shanbe4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid3Shanbe4.ToolTip = tooltip_context;
                    else Grid3Shanbe4.ToolTip = null;
                    break;

                case 32:
                    TextBlock4Shanbe4.Text = persianDate;
                    TextBlock4Shanbe4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock4Shanbe4Miladi.Text = miladiDate;
                    TextBlock4Shanbe4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock4Shanbe4Hijri.Text = hijriDate;
                    TextBlock4Shanbe4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle4Shanbe4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid4Shanbe4.ToolTip = tooltip_context;
                    else Grid4Shanbe4.ToolTip = null;
                    break;

                case 33:
                    TextBlock5Shanbe4.Text = persianDate;
                    TextBlock5Shanbe4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock5Shanbe4Miladi.Text = miladiDate;
                    TextBlock5Shanbe4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock5Shanbe4Hijri.Text = hijriDate;
                    TextBlock5Shanbe4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle5Shanbe4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid5Shanbe4.ToolTip = tooltip_context;
                    else Grid5Shanbe4.ToolTip = null;
                    break;

                case 34:
                    TextBlockJome4.Text = persianDate;
                    TextBlockJome4.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockJome4Miladi.Text = miladiDate;
                    TextBlockJome4Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockJome4Hijri.Text = hijriDate;
                    TextBlockJome4Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleJome4.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridJome4.ToolTip = tooltip_context;
                    else GridJome4.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 35:
                    TextBlockShanbe5.Text = persianDate;
                    TextBlockShanbe5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockShanbe5Miladi.Text = miladiDate;
                    TextBlockShanbe5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockShanbe5Hijri.Text = hijriDate;
                    TextBlockShanbe5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleShanbe5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridShanbe5.ToolTip = tooltip_context;
                    else GridShanbe5.ToolTip = null;
                    break;

                case 36:
                    TextBlock1Shanbe5.Text = persianDate;
                    TextBlock1Shanbe5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock1Shanbe5Miladi.Text = miladiDate;
                    TextBlock1Shanbe5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock1Shanbe5Hijri.Text = hijriDate;
                    TextBlock1Shanbe5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle1Shanbe5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid1Shanbe5.ToolTip = tooltip_context;
                    else Grid1Shanbe5.ToolTip = null;
                    break;

                case 37:
                    TextBlock2Shanbe5.Text = persianDate;
                    TextBlock2Shanbe5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock2Shanbe5Miladi.Text = miladiDate;
                    TextBlock2Shanbe5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock2Shanbe5Hijri.Text = hijriDate;
                    TextBlock2Shanbe5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle2Shanbe5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid2Shanbe5.ToolTip = tooltip_context;
                    else Grid2Shanbe5.ToolTip = null;
                    break;

                case 38:
                    TextBlock3Shanbe5.Text = persianDate;
                    TextBlock3Shanbe5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock3Shanbe5Miladi.Text = miladiDate;
                    TextBlock3Shanbe5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock3Shanbe5Hijri.Text = hijriDate;
                    TextBlock3Shanbe5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle3Shanbe5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid3Shanbe5.ToolTip = tooltip_context;
                    else Grid3Shanbe5.ToolTip = null;
                    break;

                case 39:
                    TextBlock4Shanbe5.Text = persianDate;
                    TextBlock4Shanbe5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock4Shanbe5Miladi.Text = miladiDate;
                    TextBlock4Shanbe5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock4Shanbe5Hijri.Text = hijriDate;
                    TextBlock4Shanbe5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle4Shanbe5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid4Shanbe5.ToolTip = tooltip_context;
                    else Grid4Shanbe5.ToolTip = null;
                    break;

                case 40:
                    TextBlock5Shanbe5.Text = persianDate;
                    TextBlock5Shanbe5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlock5Shanbe5Miladi.Text = miladiDate;
                    TextBlock5Shanbe5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlock5Shanbe5Hijri.Text = hijriDate;
                    TextBlock5Shanbe5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    Rectangle5Shanbe5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) Grid5Shanbe5.ToolTip = tooltip_context;
                    else Grid5Shanbe5.ToolTip = null;
                    break;

                case 41:
                    TextBlockJome5.Text = persianDate;
                    TextBlockJome5.Style = (System.Windows.Style)FindResource(persianTextBlockResourceName);
                    TextBlockJome5Miladi.Text = miladiDate;
                    TextBlockJome5Miladi.Style = (System.Windows.Style)FindResource(miladiTextBlockResourceName);
                    TextBlockJome5Hijri.Text = hijriDate;
                    TextBlockJome5Hijri.Style = (System.Windows.Style)FindResource(hijriTextBlockResourceName);
                    RectangleJome5.Style = (System.Windows.Style)FindResource(rectangleResourceName);
                    if (tooltip_context != string.Empty) GridJome5.ToolTip = tooltip_context;
                    else GridJome5.ToolTip = null;
                    break;
            }
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

        void goToDate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                CalculateMonth(int.Parse(textBoxYear.Text), comboBoxMonths.SelectedIndex + 1);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "An exception", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        void ButttonHijriAdjustment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                hijriCalendar.HijriAdjustment = int.Parse(TextBoxHijriAdjustment.Text);

                DAL.UnitOfWork oUnitOfWork = null;

                try
                {
                    oUnitOfWork = new DAL.UnitOfWork();

                    Models.User oUser = oUnitOfWork.UserRepository
                        .GetById(Utility.CurrentUser.Id);

                    oUser.UserSetting.PersianCalendarHijriAdjustment = hijriCalendar.HijriAdjustment;

                    oUnitOfWork.UserRepository.Update(oUser);

                    oUnitOfWork.Save();
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (oUnitOfWork != null)
                    {
                        oUnitOfWork.Dispose();
                        oUnitOfWork = null;
                    }

                }

                CalculateMonth(this.currentYear, this.currentMonth);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "An exception", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Media.Animation.Storyboard hideEventGrid = (System.Windows.Media.Animation.Storyboard)TryFindResource("hideEventGrid");
            hideEventGrid.Begin();
        }

        string baseName;
        void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.baseName = System.Text.RegularExpressions.Regex.Replace((sender as System.Windows.Controls.Grid).Name, @"Grid", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Windows.Shapes.Rectangle rect = FindName("Rectangle" + this.baseName) as System.Windows.Shapes.Rectangle;

            if (rect.Style == FindResource("RectangleStyleForOtherHolydays") ||
                rect.Style == FindResource("RectangleStyleOtherMonths") ||
                rect.Style == FindResource("RectangleStyleEventedOtherMonths") ||
                rect.Style == FindResource("RectangleStyleForOtherEventedFriday"))
                return;

            TextBoxEventText.Text = string.Empty;


            //بدست آوردن اطلاعات امروز
            this.yearPersian = this.yearForNavigating;
            this.monthPersian = this.monthForNavigating;
            this.dayPersian = (FindName("TextBlock" + this.baseName) as System.Windows.Controls.TextBlock).Text.ConvertToInteger();

            RefreshListBox();

            System.DateTime tempDateTime = persianCalendar.ToDateTime(this.yearPersian, this.monthPersian, this.dayPersian, 01, 01, 01, 01);

            this.dayChristian = tempDateTime.Day;
            this.monthChristian = tempDateTime.Month;
            this.yearChristian = tempDateTime.Year;

            this.yearHijri = hijriCalendar.GetYear(tempDateTime);
            this.monthHijri = hijriCalendar.GetMonth(tempDateTime);
            this.dayHijri = hijriCalendar.GetDayOfMonth(tempDateTime);

            TextBlockSelectedDateShamsi.Text = (FindName("TextBlock" + this.baseName) as System.Windows.Controls.TextBlock).Text + "  " + TextBlockThisMonth.Text;
            TextBlockSelectedDateMiladi.Text = (FindName("TextBlock" + this.baseName + "Miladi") as System.Windows.Controls.TextBlock).Text;
            TextBlockSelectedDateHijri.Text = (FindName("TextBlock" + this.baseName + "Hijri") as System.Windows.Controls.TextBlock).Text;

            EventsListBox.SelectedIndex = 0;

            System.Windows.Media.Animation.Storyboard hideEventGrid = (System.Windows.Media.Animation.Storyboard)TryFindResource("showEventGrid");
            hideEventGrid.Begin();
        }
        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        #endregion

        #region Transcations
        private void ButtonAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(TextBoxEventText.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Warning,
                        messageBoxText: "لطفا در کادر یادداشتی وارد نمایید",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Warning,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );
                return;
            }

            #endregion

            FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(this.yearPersian, this.monthPersian, this.dayPersian);

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Reminder oReminder = new Models.Reminder();
                oReminder.Year = oPersianDate.Year;
                oReminder.Month = oPersianDate.Month;
                oReminder.Day = oPersianDate.Day;
                oReminder.DateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(oPersianDate);
                oReminder.PersianDateTime = oPersianDate.ToString("d");
                oReminder.Description = TextBoxEventText.Text.Trim();
                oReminder.FundId = Utility.CurrentFund.Id;
                oUnitOfWork.RemainderRepository.Insert(oReminder);

                oUnitOfWork.Save();

                RefreshListBox();


            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }
            }

            try
            {

                CalculateMonth(oPersianDate.Year, oPersianDate.Month);

            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
            }
        }
        void saveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(TextBoxEventText.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Warning,
                        messageBoxText: "لطفا در کادر یادداشتی وارد نمایید",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Warning,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );
                return;
            }

            #endregion

            FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(this.yearPersian, this.monthPersian, this.dayPersian);

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Reminder currentRemainder = EventsListBox.SelectedItem as Models.Reminder;

                if (currentRemainder != null)
                {
                    currentRemainder.Description = TextBoxEventText.Text.Trim();
                    oUnitOfWork.RemainderRepository.Update(currentRemainder);
                }

                oUnitOfWork.Save();

                RefreshListBox();

            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }
            }

            try
            {

                CalculateMonth(oPersianDate.Year, oPersianDate.Month);

            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
            }
        }
        void ButtonDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(this.yearPersian, this.monthPersian, this.dayPersian);

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Reminder currentRemainder = EventsListBox.SelectedItem as Models.Reminder;

                if (currentRemainder != null)
                {
                    System.Windows.MessageBoxResult oResult =
                        DevExpress.Xpf.Core.DXMessageBox.Show
                            (
                                caption: Infrastructure.MessageBoxCaption.Question,
                                messageBoxText: "آیا مطمئن به حذف رویداد می‌باشید؟.",
                                button: System.Windows.MessageBoxButton.YesNo,
                                icon: System.Windows.MessageBoxImage.Question,
                                defaultResult: System.Windows.MessageBoxResult.No,
                                options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                            );
                    if (oResult == System.Windows.MessageBoxResult.Yes)
                    {
                        oUnitOfWork.RemainderRepository.Delete(currentRemainder);
                        oUnitOfWork.Save();

                        RefreshListBox();
                        TextBoxEventText.Clear();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            caption: Infrastructure.MessageBoxCaption.Error,
                            messageBoxText: "رویدادی برای حذف شدن وجود ندارد.",
                            button: System.Windows.MessageBoxButton.OK,
                            icon: System.Windows.MessageBoxImage.Error,
                            defaultResult: System.Windows.MessageBoxResult.OK,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                        );
                    return;
                }
            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }
            }

            CalculateMonth(oPersianDate.Year, oPersianDate.Month);
        }
        #endregion

        #region Helper

        int GetEventCountOfDay(int year, int month, int day)
        {

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                int count = oUnitOfWork.RemainderRepository
                    .GetByPersianDate(year, month, day)
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
        private void RefreshListBox()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                int Year = this.yearPersian;
                int Month = this.monthPersian;
                int Day = this.dayPersian;

                EventsListBox.ItemsSource = oUnitOfWork.RemainderRepository
                    .GetByPersianDate(Year, Month, Day)
                    .ToList();

                oUnitOfWork.Save();

                EventsListBox.DisplayMemberPath = "Description";
                EventsListBox.SelectedValuePath = "Id";

                DayEventsChanged?.Invoke(this, System.EventArgs.Empty);
                TextBoxEventTextChanged(null, null);

                (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).MiniPersianSchedulerReminder.RefreshMonth();
                (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).RefreshSchedulerListBox();
            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
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
        private int ReadHijriAdjustmentValue()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(Utility.CurrentUser.Id);

                return (oUser.UserSetting.PersianCalendarHijriAdjustment);

            }
            catch (System.Exception)
            {
                return -1;
            }
        }

        #endregion

        private void TextBoxEventTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxEventText.Text) == true)
            {
                ButtonAdd.IsEnabled = false;
                ButtonSave.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
            }
            else
            {
                if (EventsListBox.HasItems == true)
                {
                    if (EventsListBox.SelectedItem != null)
                    {
                        ButtonAdd.IsEnabled = true;
                        ButtonSave.IsEnabled = true;
                        ButtonDelete.IsEnabled = true;
                    }
                    else
                    {
                        ButtonAdd.IsEnabled = true;
                        ButtonSave.IsEnabled = false;
                        ButtonDelete.IsEnabled = false;
                    }

                }

                if (EventsListBox.HasItems == false)
                {
                    ButtonAdd.IsEnabled = true;
                    ButtonSave.IsEnabled = false;
                    ButtonDelete.IsEnabled = false;
                }

            }
        }
        private void EventsListBoxSelectedChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Models.Reminder selectedRemainder = EventsListBox.SelectedItem as Models.Reminder;

            if (selectedRemainder != null)
            {
                TextBoxEventText.Text = selectedRemainder.Description;
            }
            else
            {
                TextBoxEventText.Clear();
            }
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// متدی برای تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        public static string ConvertToPersianNumber(this int input)
        {
            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            string temp = input.ToString();

            temp = System.Text.RegularExpressions.Regex.Replace(temp, "0", "۰");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "1", "۱");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "2", "۲");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "3", "۳");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "4", "۴");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "5", "۵");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "6", "۶");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "7", "۷");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "8", "۸");
            temp = System.Text.RegularExpressions.Regex.Replace(temp, "9", "۹");

            return temp;
        }

        /// <summary>
        /// تبدیل اعداد فارسی به معادلش به صورت عدد integer
        /// </summary>
        public static int ConvertToInteger(this string input)
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, "۰", "0");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۱", "1");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۲", "2");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۳", "3");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۴", "4");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۵", "5");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۶", "6");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۷", "7");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۸", "8");
            input = System.Text.RegularExpressions.Regex.Replace(input, "۹", "9");

            input = System.Text.RegularExpressions.Regex.Replace(input, @"\D*", string.Empty);

            return int.Parse(input);
        }

        /// <summary>
        /// تبدیل نام روزهای هفته میلادی به شمسی
        /// </summary>
        public static string ConvertToPersianDay(this string input)
        {
            switch (input)
            {
                case "Saturday":
                    return "شنبه";

                case "Sunday":
                    return "یک شنبه";

                case "Monday":
                    return "دو شنبه";

                case "Tuesday":
                    return "سه شنبه";

                case "Wednesday":
                    return "چهار شنبه";

                case "Thursday":
                    return "پنج شنبه";

                case "Friday":
                    return "جمعه";
            }
            return Infrastructure.MessageBoxCaption.Error;
        }

        /// <summary>
        /// تبدیل عدد ماه به معادل نام ماه شمسی
        /// </summary>
        public static string ConvertToPersianMonth(this int input)
        {
            string FarsiMonthName = "هیچ کدام";
            //تعیین نام ماه شمسی  
            switch (input)
            {
                //فروردین
                case 01:
                    FarsiMonthName = "فروردین";
                    break;

                //اردیبهشت
                case 02:
                    FarsiMonthName = "اردیبهشت";
                    break;

                //خرداد
                case 03:
                    FarsiMonthName = "خرداد";
                    break;

                //تیر
                case 04:
                    FarsiMonthName = "تیر";
                    break;

                //مرداد
                case 05:
                    FarsiMonthName = "مرداد";
                    break;

                //شهریور
                case 06:
                    FarsiMonthName = "شهریور";
                    break;

                //مهر
                case 07:
                    FarsiMonthName = "مهر";
                    break;

                //آبان
                case 08:
                    FarsiMonthName = "آبان";
                    break;

                //آذر
                case 09:
                    FarsiMonthName = "آذر";
                    break;

                //دی
                case 10:
                    FarsiMonthName = "دی";
                    break;

                //بهمن
                case 11:
                    FarsiMonthName = "بهمن";
                    break;

                //اسفند
                case 12:
                    FarsiMonthName = "اسفند";
                    break;
            }

            return FarsiMonthName;
        }

        /// <summary>
        /// تبدیل عدد ماه به معادل نام ماه قمری
        /// </summary>
        public static string ConvertToHigriMonth(this int input)
        {
            string higriMonthName = "هیچ کدام";
            //تعیین نام ماه هجری قمری  
            switch (input)
            {
                case 01:
                    higriMonthName = "محرم";
                    break;

                case 02:
                    higriMonthName = "صفر";
                    break;

                case 03:
                    higriMonthName = "ربیع الاول";
                    break;

                case 04:
                    higriMonthName = "ربیع الثانی";
                    break;

                case 05:
                    higriMonthName = "جمادی الاول";
                    break;

                case 06:
                    higriMonthName = "جمادی الثانی";
                    break;

                case 07:
                    higriMonthName = "رجب";
                    break;

                case 08:
                    higriMonthName = "شعبان";
                    break;

                case 09:
                    higriMonthName = "رمضان";
                    break;

                case 10:
                    higriMonthName = "شوال";
                    break;

                case 11:
                    higriMonthName = "ذیقعده";
                    break;

                case 12:
                    higriMonthName = "ذیحجه";
                    break;
            }

            return higriMonthName;
        }
    }

}
