namespace BetsTheBest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstatefiledtoSportEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SportEvents", "state", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SportEvents", "state");
        }
    }
}
