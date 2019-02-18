# Controllers

* Base classes
    * `Controller: ApiController` -> `Controller: ControllerBase`
    * `using System.Web.Http;` -> `using Microsoft.AspNetCore.Mvc;`
* Generic return types   
    * `IHttpActionResult` -> `IActionResult` or `ActionResult<T>`
    * `Ok()` stays the same
* HTTP Attributes
    * `FromUri` -> `FromQuery`
    * `FromBody` stays the same

# Database Code

* Database context constructor:
    * There is no empty constructor anymore.
        * Was
            ```csharp
            public class DatabaseContextName : DbContext{
                public DatabaseContextName()
                    : base("ConnectionString")
                {

                }
            }

            ```
        * Now
            ```csharp
            public class DatabaseContextName : DbContext{
                public DatabaseContextName(DbContextOptions<DatabaseContext> options)
                    : base(options)
                {

                }
            }
            ```
            Thus dependency Injection is used `services.AddDbContext<DatabaseContext>(options => options.UseSqlServer("ConnectionString"))`
    * Namespace `using Microsoft.EntityFrameworkCore;` is required to use LINQ's `IQueryable` and `DbContext`

# Database Schema

* Primary Key Naming `PK_dbo.DbInvestmentAgreements` -> `PK_InvestmentAgreements`
* Foreign Key Naming `FK_dbo.DbInvestmentAgreements_dbo.Profiles_Profile_Id` -> `FK_InvestmentAgreements_Profiles_DbProfileId`
* Index Naming `IX_Product_Id` -> `IX_InvestmentAgreements_ProductId`
* Id Column Naming `DbProfile_Id` -> `DbProfileId`

Migration script:

```sql
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
```

# Migrations

* Migration blob is not saved anymore. Only migration ID is saved (which is migration name)
