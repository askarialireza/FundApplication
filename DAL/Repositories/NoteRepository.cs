
namespace DAL
{
    public class NoteRepository : Repository<Models.Note>, INoteRepository
    {
        public NoteRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {

        }
    }
}
