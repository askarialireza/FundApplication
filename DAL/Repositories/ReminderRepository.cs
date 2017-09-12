
using System;
using System.Linq;
using Models;

namespace DAL
{
    public class ReminderRepository : Repository<Models.Reminder>, IReminderRepository
    {
        public ReminderRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public IQueryable<Reminder> GetByDateTime(DateTime dateTime)
        {
            var varResult = Get()
                            .Where(current => current.DateTime == dateTime)
                            ;
            return varResult;
        }

        public IQueryable<Reminder> GetByPersianDate(int year, int month, int day)
        {
            var varResult = Get()
                            .Where(current => current.PersianDate.Year == year)
                            .Where(current => current.PersianDate.Month == month)
                            .Where(current => current.PersianDate.Day == day)
                            ;
            return varResult;
        }
    }
}
