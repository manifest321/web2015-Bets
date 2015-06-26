namespace BetsTheBest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SportEvents", "time", c => c.DateTime(nullable: false));
            DropColumn("dbo.SportEvents", "duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SportEvents", "duration", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.SportEvents", "time");
        }
    }
}
