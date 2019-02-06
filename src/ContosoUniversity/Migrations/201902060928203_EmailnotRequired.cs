namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailnotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Email", c => c.String(nullable: false));
        }
    }
}
