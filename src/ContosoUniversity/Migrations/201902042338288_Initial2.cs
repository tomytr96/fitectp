namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAccount",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            AddColumn("dbo.Person", "Username", c => c.String(nullable: false));
            DropColumn("dbo.Person", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "Login", c => c.String(nullable: false));
            DropColumn("dbo.Person", "Username");
            DropTable("dbo.UserAccount");
        }
    }
}
