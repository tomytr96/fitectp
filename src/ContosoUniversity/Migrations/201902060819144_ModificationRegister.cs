namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "Role", c => c.String(nullable: false));
            AddColumn("dbo.Person", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "Email");
            DropColumn("dbo.Person", "Role");
        }
    }
}
