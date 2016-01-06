namespace Pyo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyFuck : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ParsedTables", "fk_User", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ParsedTables", "fk_User", c => c.Int(nullable: false));
        }
    }
}
