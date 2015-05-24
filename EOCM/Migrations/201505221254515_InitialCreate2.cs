namespace EOCM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Governorates", "Govt_Lat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Governorates", "Govt_Long", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Governorates", "Govt_Long");
            DropColumn("dbo.Governorates", "Govt_Lat");
        }
    }
}
