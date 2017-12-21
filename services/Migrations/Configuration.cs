namespace services.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<services.Models.ServicesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            /*  In case an update takes a long time to run, such and building an index on a large table (WaterTemp),
             *  and the Package Manager times out, we can set the CommandTimeout to unlimited, so that it runs
             *  and completes, via the following command.
             */
            CommandTimeout = 0; // Number of seconds.
        }

        protected override void Seed(services.Models.ServicesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        /*
         * Returns migration base path so that we can always get to our migration sql files.
         * from: https://stackoverflow.com/questions/28165146/dbmigration-sqlfile-difference-in-base-directory
         */
        public static string GetMigrationBasePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\Migrations");
        }
    }
}
