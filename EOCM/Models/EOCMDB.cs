namespace EOCM.Models
{
    using EOCM.Migrations;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class EOCMDB : DbContext
    {
        // Your context has been configured to use a 'EOCMDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EOCM.Models.EOCMDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'EOCMDB' 
        // connection string in the application configuration file.
        public EOCMDB()
            : base("name=EOCMDB")
        {
            //Database.Log = s => Debug.WriteLine(s);
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EOCMDB>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EOCMDB, Configuration>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        

        }

        public System.Data.Entity.DbSet<EOCM.Models.Cluster> Clusters { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Governorate> Governorates { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.District> Districts { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Village> Villages { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Field> Fields { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Sector> Sectors { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}