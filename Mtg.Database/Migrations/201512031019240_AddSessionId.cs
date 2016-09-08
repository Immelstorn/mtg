namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSessionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "SessionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "SessionId");
        }
    }
}
