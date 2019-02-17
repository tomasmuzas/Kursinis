namespace MigrationDemo.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RenameProfileTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Profiles", newName: "DbProfiles");
            RenameColumn(table: "dbo.DbInvestmentAgreements", name: "Profile_Id", newName: "DbProfile_Id");
            RenameIndex(table: "dbo.DbInvestmentAgreements", name: "IX_Profile_Id", newName: "IX_DbProfile_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DbInvestmentAgreements", name: "IX_DbProfile_Id", newName: "IX_Profile_Id");
            RenameColumn(table: "dbo.DbInvestmentAgreements", name: "DbProfile_Id", newName: "Profile_Id");
            RenameTable(name: "dbo.DbProfiles", newName: "Profiles");
        }
    }
}
