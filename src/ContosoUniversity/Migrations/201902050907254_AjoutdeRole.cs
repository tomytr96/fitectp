namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutdeRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccount", "Role", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccount", "Role");
        }
    }
}
