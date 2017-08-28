
namespace DAL
{
    public interface IReminderRepository : IRepository<Models.Reminder>
    {
        System.Linq.IQueryable<Models.Reminder> GetByPersianDate(int year,int month,int day);
        System.Linq.IQueryable<Models.Reminder> GetByDateTime(System.DateTime dateTime);
    }
}
