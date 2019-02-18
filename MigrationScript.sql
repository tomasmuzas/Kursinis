USE master;  
GO
--Indexes
EXEC sp_rename N'dbo.DbInvestmentAgreements.IX_DbProfile_Id',N'IX_InvestmentAgreements_DbProfileId', N'INDEX';
EXEC sp_rename N'dbo.DbInvestmentAgreements.IX_DbProfile_Id',N'IX_InvestmentAgreements_DbProfileId', N'INDEX';

--Primary Keys
EXEC sp_rename N'[dbo].[DbInvestmentAgreements].[PK_dbo.DbInvestmentAgreements]',N'PK_InvestmentAgreements';
EXEC sp_rename N'[dbo].[DbProducts].[PK_dbo.DbProducts]',N'PK_DbProducts';
EXEC sp_rename N'[dbo].[DbProfiles].[PK_dbo.DbProfiles]',N'PK_DbProfiles';

--Foreign Keys
EXEC sp_rename N'[dbo].[FK_dbo.DbInvestmentAgreements_dbo.Profiles_Profile_Id]',N'FK_InvestmentAgreements_Profiles_DbProfileId';

-- Columns
EXEC sp_rename N'[dbo].[DbInvestmentAgreements].[DbProfile_Id]',N'DbProfileId', N'COLUMN';

GO  
