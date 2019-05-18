namespace EFConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbInvestmentAgreements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Product_Id = c.Int(),
                        DbProfile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbProducts", t => t.Product_Id)
                .ForeignKey("dbo.DbProfiles", t => t.DbProfile_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.DbProfile_Id);
            
            CreateTable(
                "dbo.DbProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Isin = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DbInvestmentAgreements", "DbProfile_Id", "dbo.DbProfiles");
            DropForeignKey("dbo.DbInvestmentAgreements", "Product_Id", "dbo.DbProducts");
            DropIndex("dbo.DbInvestmentAgreements", new[] { "DbProfile_Id" });
            DropIndex("dbo.DbInvestmentAgreements", new[] { "Product_Id" });
            DropTable("dbo.DbProfiles");
            DropTable("dbo.DbProducts");
            DropTable("dbo.DbInvestmentAgreements");
        }
    }
}
