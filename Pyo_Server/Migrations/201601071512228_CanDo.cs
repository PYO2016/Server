namespace Pyo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CanDo : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ParsedTables", "filename", "result");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ParsedTables", "result", "filename");
        }
    }
}
