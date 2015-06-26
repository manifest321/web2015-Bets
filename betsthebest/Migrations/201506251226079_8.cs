namespace BetsTheBest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "src", c => c.String());
            AddColumn("dbo.Messages", "dest", c => c.String());
            DropColumn("dbo.Messages", "srcId");
            DropColumn("dbo.Messages", "destId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "destId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "srcId", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "dest");
            DropColumn("dbo.Messages", "src");
        }
    }
}
