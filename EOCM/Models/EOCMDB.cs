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

            modelBuilder.Entity<Governorate >().Property(x => x.Govt_Lat).HasPrecision(10, 6);
            modelBuilder.Entity<Governorate>().Property(x => x.Govt_Long).HasPrecision(10, 6);

            modelBuilder.Entity<Cluster>().Property(x => x.Cluster_Lat).HasPrecision(10, 6);
            modelBuilder.Entity<Cluster>().Property(x => x.Cluster_Long).HasPrecision(10, 6);

            modelBuilder.Entity<Cluster>().Property(x => x.Cluster_EmpFemale).HasPrecision(5, 2);
            modelBuilder.Entity<Cluster>().Property(x => x.OfficalProjects).HasPrecision(5, 2);
            modelBuilder.Entity<Cluster>().Property(x => x.NonOfficalProjects).HasPrecision(5, 2);
            modelBuilder.Entity<Cluster>().Property(x => x.CompanyPercent1).HasPrecision(5, 2);
            modelBuilder.Entity<Cluster>().Property(x => x.CompanyPercent2).HasPrecision(5, 2);
            modelBuilder.Entity<Cluster>().Property(x => x.CompanyPercent3).HasPrecision(5, 2);
            modelBuilder.Entity<Cluster>().Property(x => x.CompanyPercent4).HasPrecision(5, 2);



        }

        public System.Data.Entity.DbSet<EOCM.Models.Cluster> Clusters { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Governorate> Governorates { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.District> Districts { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Village> Villages { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Field> Fields { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.Sector> Sectors { get; set; }

        public System.Data.Entity.DbSet<EOCM.Models.ClusterNature> ClusterNature { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.ClusterType> ClusterType { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.MarketType> MarketType { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.ProductSeason> ProductSeason { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.ExportFlag> ExportFlag { get; set; }
        public System.Data.Entity.DbSet<EOCM.Models.IncomeLevel> IncomeLevel { get; set; }
      

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}