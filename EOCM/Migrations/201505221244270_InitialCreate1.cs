namespace EOCM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clusters", "Address", c => c.String(maxLength: 255));
            AddColumn("dbo.Clusters", "Products", c => c.String(maxLength: 1024));
            AddColumn("dbo.Clusters", "OfficalProjects", c => c.Int(nullable: false));
            AddColumn("dbo.Clusters", "NonOfficalProjects", c => c.Int(nullable: false));
            AddColumn("dbo.Clusters", "ClusterNature", c => c.String(maxLength: 1024));
            AddColumn("dbo.Clusters", "ClusterType", c => c.String(maxLength: 1024));
            AddColumn("dbo.Clusters", "SupportingOrg", c => c.String(maxLength: 1024));
            AddColumn("dbo.Clusters", "Challenges", c => c.String(maxLength: 1024));
            AlterColumn("dbo.Clusters", "Cluster_EmpNum", c => c.Int());
            AlterColumn("dbo.Clusters", "Cluster_ShopNum", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clusters", "Cluster_ShopNum", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Clusters", "Cluster_EmpNum", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Clusters", "Challenges");
            DropColumn("dbo.Clusters", "SupportingOrg");
            DropColumn("dbo.Clusters", "ClusterType");
            DropColumn("dbo.Clusters", "ClusterNature");
            DropColumn("dbo.Clusters", "NonOfficalProjects");
            DropColumn("dbo.Clusters", "OfficalProjects");
            DropColumn("dbo.Clusters", "Products");
            DropColumn("dbo.Clusters", "Address");
        }
    }
}
