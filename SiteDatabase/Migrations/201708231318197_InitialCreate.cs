namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Organization = c.String(),
                        Sex = c.Int(nullable: false),
                        UserGroup = c.Int(nullable: false),
                        PasswordHash = c.Binary(),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.UserProfileEntityTeamProfileEntities",
                c => new
                    {
                        UserProfileEntity_Username = c.String(nullable: false, maxLength: 128),
                        TeamProfileEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserProfileEntity_Username, t.TeamProfileEntity_Id })
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileEntity_Username, cascadeDelete: true)
                .ForeignKey("dbo.TeamProfiles", t => t.TeamProfileEntity_Id, cascadeDelete: true)
                .Index(t => t.UserProfileEntity_Username)
                .Index(t => t.TeamProfileEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfileEntityTeamProfileEntities", "TeamProfileEntity_Id", "dbo.TeamProfiles");
            DropForeignKey("dbo.UserProfileEntityTeamProfileEntities", "UserProfileEntity_Username", "dbo.UserProfiles");
            DropIndex("dbo.UserProfileEntityTeamProfileEntities", new[] { "TeamProfileEntity_Id" });
            DropIndex("dbo.UserProfileEntityTeamProfileEntities", new[] { "UserProfileEntity_Username" });
            DropTable("dbo.UserProfileEntityTeamProfileEntities");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.TeamProfiles");
        }
    }
}
