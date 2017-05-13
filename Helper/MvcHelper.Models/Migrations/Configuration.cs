namespace Models
{
    using ImageClassification.Enums;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DbEntity>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DbEntity context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Roles.AddOrUpdate(
                p => p.Name,
                new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "admin", MenuId = "{\"SystemManage\":true,\"PersonalUser\":true,\"Details_PersonalUser\":true,\"Edit_PersonalUser\":true,\"ModifyPwd_PersonalUser\":true,\"User\":true,\"Index_User\":true,\"Create_User\":true,\"Edit_User\":true,\"Delete_User\":true,\"Details_User\":true,\"Role\":true,\"Index_Role\":true,\"Create_Role\":true,\"Edit_Role\":true,\"Delete_Role\":true}" }
                );
            context.Users.AddOrUpdate(
                p => p.LoginName,
                new User { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), LoginName = "admin", Password = "DD4B21E9EF71E1291183A46B913AE6F2",  Name = "admin", CreateTime = DateTime.Now, Status=UserStatus.Normal, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001") }
                );
            context.Types.AddOrUpdate(
                p=>p.Name,
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),Name="ATM" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "����" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "��վ" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "��̲" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = "������" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = "����" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Name = "�ֵ�" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Name = "�̳�" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Name = "����֮��" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Name = "̩ɽ" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), Name = "ѧУ" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000012"), Name = "ҽԺ" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000013"), Name = "����" },
                new Type { Id = Guid.Parse("00000000-0000-0000-0000-000000000014"), Name = "�㳡" }
                );

            context.Volunteers.AddOrUpdate(
                p=>p.Name,
                new Volunteer { Id=Guid.Parse("00000000-0001-0000-0000-000000000000"), Uuid ="0000000000000000000", LoginName ="����", Password = "DD4B21E9EF71E1291183A46B913AE6F2", SmsCaptcha="666666", SmsCaptchaUsedFor= EnumCaptchaUsedFor.־Ը��ע��, Name="����", MobileNumber="13275261392", RegisterTime=DateTime.Now, Status=EnumUserStatus.���� }              
                );

        }
    }
}
