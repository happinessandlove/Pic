namespace Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class g : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Volunteers", "LoginName", c => c.String(maxLength: 10));
            AlterColumn("dbo.Volunteers", "Password", c => c.String(maxLength: 50));
            AlterColumn("dbo.Volunteers", "Name", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Volunteers", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Volunteers", "Password", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Volunteers", "LoginName", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
