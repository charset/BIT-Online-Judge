namespace BITOJ.Data.Migrations
{
    using BITOJ.Common.Cache;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System.Data.Entity.Migrations;
    using System.Text;
    using System.Security.Cryptography;

    internal sealed class Configuration : DbMigrationsConfiguration<UserDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BITOJ.Data.UserDataContext";
        }

        protected override void Seed(UserDataContext context)
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

            UserProfileEntity profile = new UserProfileEntity()
            {
                Username = "Lancern",
                Organization = "BIT",
                UserGroup = UserGroup.Administrators,
                ProfileFileName = ApplicationDirectory.GetAppSubDirectory("Users") + "\\Lancern",
            };
            using (SHA512 hash = SHA512.Create())
            {
                profile.PasswordHash = hash.ComputeHash(Encoding.Unicode.GetBytes("Lancern"));
            }

            context.UserProfiles.AddOrUpdate(profile);
        }
    }
}
