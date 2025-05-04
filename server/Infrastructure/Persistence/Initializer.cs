namespace Infrastructure.Persistence
{
    using Domain.Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    internal class DatabaseInitializer : IInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly IEnumerable<IInitialData> _initialDataProviders;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseInitializer(
            ApplicationDbContext db,
            IEnumerable<IInitialData> initialDataProviders,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _initialDataProviders = initialDataProviders;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            _db.Database.Migrate();

            SeedRoles();

            foreach (var initialDataProvider in _initialDataProviders)
            {
                if (this.DataSetIsEmpty(initialDataProvider.EntityType))
                {
                    var data = initialDataProvider.GetData();

                    foreach (var entity in data)
                    {
                        _db.Add(entity);
                    }
                }
            }

            _db.SaveChanges();
        }

        private bool DataSetIsEmpty(Type type)
        {
            var setMethod = this.GetType()
                .GetMethod(nameof(this.GetSet), BindingFlags.Instance | BindingFlags.NonPublic)!
                .MakeGenericMethod(type);

            var set = setMethod.Invoke(this, Array.Empty<object>());

            var countMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == nameof(Queryable.Count) && m.GetParameters().Length == 1)
                .MakeGenericMethod(type);

            var result = (int)countMethod.Invoke(null, new[] { set })!;

            return result == 0;
        }

        private void SeedRoles()
        {
            string[] roles = new[] { "Host", "Buyer" };

            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }
        }

        private DbSet<TEntity> GetSet<TEntity>()
            where TEntity : class
            => _db.Set<TEntity>();
    }
}
