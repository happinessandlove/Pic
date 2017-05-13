namespace Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PictureId = c.Guid(nullable: false),
                        Name = c.String(),
                        FinishTime = c.DateTime(nullable: false),
                        Effective = c.Boolean(nullable: false),
                        Volunteer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId)
                .ForeignKey("dbo.Volunteers", t => t.Volunteer_Id)
                .Index(t => t.PictureId)
                .Index(t => t.Volunteer_Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Url = c.String(),
                        Type_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        MenuId = c.String(nullable: false),
                        Remarks = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        LoginName = c.String(nullable: false, maxLength: 10),
                        Password = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 20),
                        LivingAddress = c.String(maxLength: 200),
                        TelephoneNumber = c.String(maxLength: 20),
                        MobileNumber = c.String(maxLength: 20),
                        IDCardNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        Introduction = c.String(maxLength: 1000),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VolunteerId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Uuid = c.String(maxLength: 100),
                        LoginName = c.String(nullable: false, maxLength: 10),
                        Password = c.String(nullable: false, maxLength: 50),
                        SmsCaptcha = c.String(maxLength: 10),
                        SmsCaptchaUsedFor = c.Int(),
                        SmsCaptchaExpiredTime = c.DateTime(),
                        AccessFailedCount = c.Int(),
                        LockoutStartTime = c.DateTime(),
                        LockoutDuration = c.Int(nullable: false),
                        OperateToken = c.String(maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 20),
                        MobileNumber = c.String(maxLength: 20),
                        TelephoneNumber = c.String(maxLength: 20),
                        IDCardNumber = c.String(maxLength: 50),
                        RegisterTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VolunteerTokens",
                c => new
                    {
                        VolunteerId = c.Guid(nullable: false),
                        Token = c.String(nullable: false, maxLength: 900),
                        CreateTime = c.DateTime(nullable: false),
                        ActiveTime = c.DateTime(nullable: false),
                        Validity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VolunteerId)
                .Index(t => t.Token);
            
            CreateTable(
                "dbo.VolunteerTypes",
                c => new
                    {
                        Volunteer_Id = c.Guid(nullable: false),
                        Type_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Volunteer_Id, t.Type_Id })
                .ForeignKey("dbo.Volunteers", t => t.Volunteer_Id)
                .ForeignKey("dbo.Types", t => t.Type_Id)
                .Index(t => t.Volunteer_Id)
                .Index(t => t.Type_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolunteerTypes", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.VolunteerTypes", "Volunteer_Id", "dbo.Volunteers");
            DropForeignKey("dbo.Labels", "Volunteer_Id", "dbo.Volunteers");
            DropForeignKey("dbo.Pictures", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Labels", "PictureId", "dbo.Pictures");
            DropIndex("dbo.VolunteerTypes", new[] { "Type_Id" });
            DropIndex("dbo.VolunteerTypes", new[] { "Volunteer_Id" });
            DropIndex("dbo.VolunteerTokens", new[] { "Token" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Pictures", new[] { "Type_Id" });
            DropIndex("dbo.Labels", new[] { "Volunteer_Id" });
            DropIndex("dbo.Labels", new[] { "PictureId" });
            DropTable("dbo.VolunteerTypes");
            DropTable("dbo.VolunteerTokens");
            DropTable("dbo.Volunteers");
            DropTable("dbo.Types");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Pictures");
            DropTable("dbo.Labels");
        }
    }
}
