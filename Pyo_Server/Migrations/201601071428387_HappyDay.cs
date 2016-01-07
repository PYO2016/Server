namespace Pyo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HappyDay : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.CapturedImages", "fk_ParsedTables", "pk");
            AddColumn("dbo.ParsedTables", "fk_CapturedImages", c => c.Int(nullable: false));
            RenameColumn("dbo.ParsedTables", "filename", "result");

            //DropPrimaryKey("dbo.CapturedImages");
            //AddColumn("dbo.CapturedImages", "pk", c => c.Int(nullable: false, identity: true));
            //AddColumn("dbo.ParsedTables", "fk_CapturedImages", c => c.Int(nullable: false));
           // AddPrimaryKey("dbo.CapturedImages", "pk");
           // DropColumn("dbo.CapturedImages", "fk_ParsedTable");
        }

        public override void Down()
        {
            RenameColumn("dbo.ParsedTables", "result", "filename");
            DropColumn("dbo.ParsedTables", "fk_CapturedImages");
            RenameColumn("dbo.CapturedImages", "pk", "fk_ParsedTables");

            //AddColumn("dbo.CapturedImages", "fk_ParsedTable", c => c.Int(nullable: false, identity: true));
            //DropPrimaryKey("dbo.CapturedImages");
            //DropColumn("dbo.ParsedTables", "fk_CapturedImages");
            //DropColumn("dbo.CapturedImages", "pk");
            //AddPrimaryKey("dbo.CapturedImages", "fk_ParsedTable");

        }
    }
}
