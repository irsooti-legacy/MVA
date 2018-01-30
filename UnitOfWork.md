# Unit of Work

Gli step necessari per la realizzazione di uno unit of work generico sono 3:

- Creare l'application db context
- Creare un repository generico
- Creare la classe (che per convenzione chiameremo UnitOfWork.cs) unit of work con le repository necessarie.

 ```cs
 // Creazione dell'application context
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<FlickrPhoto> FlickrPhotos { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new AdminMap(modelBuilder.Entity<Admin>());
            new FlickrPhotoMap(modelBuilder.Entity<FlickrPhoto>());
            //new BookMap(modelBuilder.Entity<Book>());
        }
    }
```

```cs
    // Creazione del repository generico
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext context;
        private DbSet<T> entities;
        public IQueryable<T> Entities()
        {
            return entities;
        }
        string errorMessage = string.Empty;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
   
        public T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //context.SaveChanges();


        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }
    }
```

```cs

    // Creazione della classe UnitOfWork
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _dbContext;

        #region Repositories
        public IRepository<Admin> AdminRepository => new Repository<Admin>(_dbContext);
        public IRepository<FlickrPhoto> FlickrPhotoRepository => new Repository<FlickrPhoto>(_dbContext);
        #endregion

        public UnitOfWork(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
```

### Inject del servizio nella classe startup

Per injectare il dbcontext ed il pattern unit of work sulla web app occorre richiamare i metodi opportuni sul metodo `ConfigureServices` in `Startup.cs`.

In primo luogo va aggiunto il dbcontext e la relativa connessione al db, dopodichè va aggiunto lo unit of work.

```cs
services.AddDbContext<ApplicationContext>(
    options => options.UseSqlServer(Configuration["DefaultConnection"]));
services.AddTransient<IUnitOfWork, UnitOfWork>();
```

## A che serve questo pattern?
Il pattern corrente permette di eseguire i test con più facilità, sostituendo la base dati che sia di un database o una serie di dati fake.

![unit of work](./img/uow.png)    

## Link utili

- Unit of work pattern msdocs: https://docs.microsoft.com/it-it/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
- Unit of work pattern (step-by-step): https://medium.com/@utterbbq/c-unitofwork-and-repository-pattern-305cd8ecfa7a
- Inject unit of work in .NET webapp: https://stackoverflow.com/questions/45535311/how-to-pass-connection-string-to-unitofwork-project-from-startup-cs-asp-net-core