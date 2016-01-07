namespace Pyo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeFree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParsedTables", "isProccessed", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParsedTables", "filename", c => c.String());
            AlterColumn("dbo.ParsedTables", "time", c => c.DateTime(nullable: false));
            DropColumn("dbo.ParsedTables", "fk_CapturedImages");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParsedTables", "fk_CapturedImages", c => c.Int(nullable: false));
            AlterColumn("dbo.ParsedTables", "time", c => c.Int(nullable: false));
            DropColumn("dbo.ParsedTables", "filename");
            DropColumn("dbo.ParsedTables", "isProccessed");
        }
    }
}
