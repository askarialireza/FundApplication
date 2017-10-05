
namespace Infrastructure.Text
{
    public static class RegularExpressions
    {
        public const string IranMobilePhoneNumber = @"(^(09|9)[1][1-9]\d{7}$)|(^(09|9)[3][0-9]\d{7}$)|(^(09|9)[0][1-3]\d{7}$)|(^(09|9)[2][0-3]\d{7}$)";

        public const string PersianDate = @"[1-4]\d{3}\/((0?[1-6]\/((3[0-1])|([1-2][0-9])|(0?[1-9])))|((1[0-2]|(0?[7-9]))\/(30|([1-2][0-9])|(0?[1-9]))))";

        public const string Username = "[a-zA-Z0-9_]";

        public const string EmailAddress = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        public const string NumbersOnly = "^[0-9]*$";

        public const string PercentValue = "^[1-9][0-9]?$|^100|^0$";
    }
}
