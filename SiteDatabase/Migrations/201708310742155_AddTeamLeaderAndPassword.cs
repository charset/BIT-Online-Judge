namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamLeaderAndPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamProfiles", "Leader", c => c.String());
            AddColumn("dbo.TeamProfiles", "PasswordHash", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamProfiles", "PasswordHash");
            DropColumn("dbo.TeamProfiles", "Leader");
        }
    }
}
