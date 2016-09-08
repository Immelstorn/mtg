namespace Mtg.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Created", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Created");
        }
    }
}
