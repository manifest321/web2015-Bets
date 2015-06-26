namespace BetsTheBest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "eId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "eId");
        }
    }
}
