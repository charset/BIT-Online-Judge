namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProblemCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProblemEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 32),
                        Title = c.String(),
                        Author = c.String(),
                        Source = c.String(),
                        Origin = c.Int(nullable: false),
                        AuthorizationGroup = c.Int(nullable: false),
                        TotalSubmissions = c.Int(nullable: false),
                        AcceptedSubmissions = c.Int(nullable: false),
                        ProblemDirectory = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProblemEntityProblemCategoryEntities",
                c => new
                    {
                        ProblemEntity_Id = c.String(nullable: false, maxLength: 32),
                        ProblemCategoryEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProblemEntity_Id, t.ProblemCategoryEntity_Id })
                .ForeignKey("dbo.ProblemEntities", t => t.ProblemEntity_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProblemCategories", t => t.ProblemCategoryEntity_Id, cascadeDelete: true)
                .Index(t => t.ProblemEntity_Id)
                .Index(t => t.ProblemCategoryEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProblemEntityProblemCategoryEntities", "ProblemCategoryEntity_Id", "dbo.ProblemCategories");
            DropForeignKey("dbo.ProblemEntityProblemCategoryEntities", "ProblemEntity_Id", "dbo.ProblemEntities");
            DropIndex("dbo.ProblemEntityProblemCategoryEntities", new[] { "ProblemCategoryEntity_Id" });
            DropIndex("dbo.ProblemEntityProblemCategoryEntities", new[] { "ProblemEntity_Id" });
            DropTable("dbo.ProblemEntityProblemCategoryEntities");
            DropTable("dbo.ProblemEntities");
            DropTable("dbo.ProblemCategories");
        }
    }
}
