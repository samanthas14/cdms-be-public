using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System;
using System.Linq;
using System.Collections;
using NLog;
using System.Diagnostics;

namespace services.Models
{
    public class ServicesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<services.Models.ServicesContext>());

        public ServicesContext() : base("name=ServicesContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /*    
             * There are three types of entities in CDMS:
             * 1) CDMS System Entities - system-level classes like:
             *      - projects
             *      - datasets
             *      - locations
             *      - activities
             *      
             *      These are defined in this file statically and are in the namespace: services.Models
             *      
             *  2) CDMS "Core" Datasets - datasets that are dynamically loaded but
             *     are shared with everyone. These are the fisheries datasets like:
             *      - Adult Weir
             *      - Screw Trap
             *      - StreamNet_NOSA
             *      
             *  3) CDMS "Private" Datasets - datasets that are dynamically loaded but the code for these 
             *     are not shared with everyone. These are datasets that tribes create
             *     that they want to have only within their organization.
             *     
             *     Both type 2 and type 3 are loaded dynamically from the namespace: services.Models.Data
             *     These are loaded at runtime during the OnModelCreating method below.
             *     
             *  
         */

        public DbSet<Project> Projects { get; set; }
        public DbSet<MetadataValue> MetadataValue { get; set; }
        public DbSet<AuditJournal> AuditJournal { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<LocationType> LocationType { get; set; }
        public DbSet<MetadataEntity> MetadataEntity { get; set; }
        public DbSet<MetadataProperty> MetadataProperty { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<SdeFeatureClass> SdeFeatureClass { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<Datastore> Datastores { get; set; }
        public DbSet<DatasetField> DatasetFields { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldCategory> FieldCategories { get; set; }
        public DbSet<FieldRole> FieldRoles { get; set; }
        public DbSet<Instrument> Instruments { get; set; }

        public DbSet<InstrumentType> InstrumentType { get; set; }
        public DbSet<InstrumentAccuracyCheck> AccuracyChecks { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<QAStatus> QAStatuses { get; set; }
        public DbSet<WaterBody> WaterBodies { get; set; }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityQA> ActivityQAs { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<Fisherman> Fishermen { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Funding> Funding { get; set; }

        //get the dbset by name rather than by type
        public DbSet GetDbSet(string entityName)
        {
            return this.Set(GetTypeFor(entityName));
        }

        public System.Type GetTypeFor(string entityName)
        {
            var datasource = "services.Models.Data." + entityName;
            var obj = System.Activator.CreateInstance("services", datasource).Unwrap();
            return obj.GetType();
        }

        public dynamic GetObjectFor(string entityName)
        {
            var datasource = "services.Models.Data." + entityName;
            var obj = System.Activator.CreateInstance("services", datasource).Unwrap();
            return obj;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Debug.WriteLine("OnModelCreating");

            //Load all "services.Models.Data" entities
            IEnumerable typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "services.Models.Data");
            foreach (Type type in typelist)
            {
                Debug.WriteLine("Found something: " + type.Name);
                if (type.IsSubclassOf(typeof(Data.DataDetail)) ||
                    type.IsSubclassOf(typeof(Data.DataHeader)) ||
                    type.IsSubclassOf(typeof(Data.DatasetStandalone)) )
                {
                    Debug.WriteLine(" It is a dataset... attaching entity: " + type.Name);

                    //dynamic configurationInstance = Activator.CreateInstance(type);
                    //modelBuilder.Configurations.Add(configurationInstance);

                    MethodInfo method = modelBuilder.GetType().GetMethod("Entity");
                    method = method.MakeGenericMethod(new Type[] { type });
                    method.Invoke(modelBuilder, null);
                }
            }

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //in EFF 5 this is the only way to specify decimal precision
            modelBuilder.Entity<Location>().Property(p => p.GPSEasting).HasPrecision(18, 8);
            modelBuilder.Entity<Location>().Property(p => p.GPSNorthing).HasPrecision(18, 8);
            modelBuilder.Entity<Location>().Property(p => p.Latitude).HasPrecision(18, 13);
            modelBuilder.Entity<Location>().Property(p => p.Longitude).HasPrecision(18, 13);
            modelBuilder.Entity<Location>().Property(p => p.RiverMile).HasPrecision(5, 2);
        }

        public static ServicesContext Current
        {
            get
            {
                if (System.Web.HttpContext.Current != null) //hey because sometimes it is! TODO
                    return System.Web.HttpContext.Current.Items["_EntityContext"] as ServicesContext;
                else
                    return new ServicesContext();
            }

        }

        public static ServicesContext RestartCurrent
        {
            get
            {
                //dispose of the existing one if it exists.
                var entityContext = System.Web.HttpContext.Current.Items["_EntityContext"] as ServicesContext;
                if (entityContext != null)
                    entityContext.Dispose();

                //start a new one.
                System.Web.HttpContext.Current.Items["_EntityContext"] = new ServicesContext(); //create a whole new one...
                return System.Web.HttpContext.Current.Items["_EntityContext"] as ServicesContext;
            }

        }

        private IEnumerable GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes()
                .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }

    }
}
