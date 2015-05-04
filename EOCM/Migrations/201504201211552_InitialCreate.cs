namespace EOCM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clusters",
                c => new
                    {
                        Cluster_ID = c.String(nullable: false, maxLength: 255),
                        Cluster_Name = c.String(),
                        Govt_ID = c.String(maxLength: 255),
                        District_ID = c.String(maxLength: 255),
                        Village_ID = c.String(maxLength: 255),
                        Sector_ID = c.String(maxLength: 255),
                        Field_ID = c.String(maxLength: 255),
                        Product_ID = c.String(maxLength: 255),
                        Cluster_Lat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cluster_Long = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cluster_EmpNum = c.Decimal(precision: 18, scale: 2),
                        Cluster_ShopNum = c.Decimal(precision: 18, scale: 2),
                        Cluster_ProductImage = c.String(),
                        Cluster_ProcessImage = c.String(),
                        Cluster_DetailPage = c.String(),
                    })
                .PrimaryKey(t => t.Cluster_ID)
                .ForeignKey("dbo.Districts", t => t.District_ID)
                .ForeignKey("dbo.Governorates", t => t.Govt_ID)
                .ForeignKey("dbo.Villages", t => t.Village_ID)
                .ForeignKey("dbo.Fields", t => t.Field_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .ForeignKey("dbo.Sectors", t => t.Sector_ID)
                .Index(t => t.Govt_ID)
                .Index(t => t.District_ID)
                .Index(t => t.Village_ID)
                .Index(t => t.Sector_ID)
                .Index(t => t.Field_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        District_ID = c.String(nullable: false, maxLength: 255),
                        District_Name = c.String(),
                        Govt_ID = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.District_ID)
                .ForeignKey("dbo.Governorates", t => t.Govt_ID)
                .Index(t => t.Govt_ID);
            
            CreateTable(
                "dbo.Governorates",
                c => new
                    {
                        Govt_ID = c.String(nullable: false, maxLength: 255),
                        Govt_Name = c.String(),
                    })
                .PrimaryKey(t => t.Govt_ID);
            
            CreateTable(
                "dbo.Villages",
                c => new
                    {
                        Village_ID = c.String(nullable: false, maxLength: 255),
                        Village_Name = c.String(),
                        Govt_ID = c.String(maxLength: 255),
                        District_ID = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Village_ID)
                .ForeignKey("dbo.Districts", t => t.District_ID)
                .ForeignKey("dbo.Governorates", t => t.Govt_ID)
                .Index(t => t.Govt_ID)
                .Index(t => t.District_ID);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Field_ID = c.String(nullable: false, maxLength: 255),
                        Field_Name = c.String(),
                        Sector_ID = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Field_ID)
                .ForeignKey("dbo.Sectors", t => t.Sector_ID)
                .Index(t => t.Sector_ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Product_ID = c.String(nullable: false, maxLength: 255),
                        Product_Name = c.String(),
                        Sector_ID = c.String(maxLength: 255),
                        Field_ID = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Product_ID)
                .ForeignKey("dbo.Fields", t => t.Field_ID)
                .ForeignKey("dbo.Sectors", t => t.Sector_ID)
                .Index(t => t.Sector_ID)
                .Index(t => t.Field_ID);
            
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        Sector_ID = c.String(nullable: false, maxLength: 255),
                        Sector_Name = c.String(),
                    })
                .PrimaryKey(t => t.Sector_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Sector_ID", "dbo.Sectors");
            DropForeignKey("dbo.Fields", "Sector_ID", "dbo.Sectors");
            DropForeignKey("dbo.Clusters", "Sector_ID", "dbo.Sectors");
            DropForeignKey("dbo.Products", "Field_ID", "dbo.Fields");
            DropForeignKey("dbo.Clusters", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Clusters", "Field_ID", "dbo.Fields");
            DropForeignKey("dbo.Villages", "Govt_ID", "dbo.Governorates");
            DropForeignKey("dbo.Villages", "District_ID", "dbo.Districts");
            DropForeignKey("dbo.Clusters", "Village_ID", "dbo.Villages");
            DropForeignKey("dbo.Districts", "Govt_ID", "dbo.Governorates");
            DropForeignKey("dbo.Clusters", "Govt_ID", "dbo.Governorates");
            DropForeignKey("dbo.Clusters", "District_ID", "dbo.Districts");
            DropIndex("dbo.Products", new[] { "Field_ID" });
            DropIndex("dbo.Products", new[] { "Sector_ID" });
            DropIndex("dbo.Fields", new[] { "Sector_ID" });
            DropIndex("dbo.Villages", new[] { "District_ID" });
            DropIndex("dbo.Villages", new[] { "Govt_ID" });
            DropIndex("dbo.Districts", new[] { "Govt_ID" });
            DropIndex("dbo.Clusters", new[] { "Product_ID" });
            DropIndex("dbo.Clusters", new[] { "Field_ID" });
            DropIndex("dbo.Clusters", new[] { "Sector_ID" });
            DropIndex("dbo.Clusters", new[] { "Village_ID" });
            DropIndex("dbo.Clusters", new[] { "District_ID" });
            DropIndex("dbo.Clusters", new[] { "Govt_ID" });
            DropTable("dbo.Sectors");
            DropTable("dbo.Products");
            DropTable("dbo.Fields");
            DropTable("dbo.Villages");
            DropTable("dbo.Governorates");
            DropTable("dbo.Districts");
            DropTable("dbo.Clusters");
        }
    }
}
