namespace Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;

    internal class DatabaseInitializer : IInitializer
    {
        private readonly ApplicationDbContext db;

        public DatabaseInitializer(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Initialize()
        {
            this.db.Database.Migrate();
        }
    }
}
