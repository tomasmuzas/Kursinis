namespace MigrationDemo.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddProfiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DbInvestmentAgreements", "Profile_Id", c => c.Int());
            CreateIndex("dbo.DbInvestmentAgreements", "Profile_Id");
            AddForeignKey("dbo.DbInvestmentAgreements", "Profile_Id", "dbo.Profiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DbInvestmentAgreements", "Profile_Id", "dbo.Profiles");
            DropIndex("dbo.DbInvestmentAgreements", new[] { "Profile_Id" });
            DropColumn("dbo.DbInvestmentAgreements", "Profile_Id");
            DropTable("dbo.Profiles");
        }
    }
}
