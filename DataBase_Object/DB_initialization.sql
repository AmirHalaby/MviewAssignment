
--Step 1 Create New DB

Create database MViewTest;

 
-- Step 2 

-- Use the DB created now 
-- Run the bellow Types

CREATE TYPE [dbo].[udt_License_Plates_Errors_Fix_Suggest_table] AS TABLE(
	[ID] [int] NOT NULL,
	[Date] DateTime NOT NULL,
	[ErorrLicensePlate] [nvarchar](1000) NOT NULL,
	[SuggestionToFixed] [nvarchar](1000) NOT NULL
)


drop table if exists LicensePlatesErrorsAndFixSuggest;
create table LicensePlatesErrorsAndFixSuggest
(
	ID [int] NOT NULL,
	Date DateTime NOT NULL,
	ErorrLicensePlate nvarchar(1000) NOT NULL,
	SuggestionToFixed nvarchar(1000) NOT NULL
);