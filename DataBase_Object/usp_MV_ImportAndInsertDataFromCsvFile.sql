USE [MViewTest118]
GO
/****** Object:  StoredProcedure [dbo].[usp_MV_ImportAndInsertDataFromCsvFile]    Script Date: 13/11/2024 9:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
Created by AmirH, 13-Nov-2024.

--declare @pCsvPathFile nvarchar(1000) = 'C:\TestLicence\Book1.Csv'

declare @pCsvPathFile nvarchar(1000) = 'C:\Users\amirh\source\repos\MviewTask\MviewAssignment\RandomlicensePlates.Csv'

exec usp_MV_ImportAndInsertDataFromCsvFile 
	@pCsvPathFile 

select * from LicensePlatesOrder
*/

drop Procedure if exists usp_MV_ImportAndInsertDataFromCsvFile;
Go
Create Procedure [dbo].[usp_MV_ImportAndInsertDataFromCsvFile]
(
	@pCsvPathFile nvarchar(1000)
)
As
	Begin
		Begin Try

				--Get Data from Csv file 

				drop table if exists LicensePlates;

				create table LicensePlates
				(
				DateTimeStr Varchar(20),
				LicensePlateNumber NVarchar(50)
				);

				declare @vsql nvarchar(max) = '';
				set @vsql = 'BULK INSERT LicensePlates
							 FROM ' + '''' + @pCsvPathFile + '''' +
							 ' WITH (fieldterminator='','', firstrow = 2);';
	
				exec(@vsql);
			
				--Save Data to Csv LicensePlatesOrder table 

				drop table if exists LicensePlatesOrder;

				create table LicensePlatesOrder
				(
				ID INT IDENTITY(1, 1),
				--DateTimeStr Varchar(20),
				Date DateTime,
				--DateTimeMin DateTime,
				LicensePlate Varchar(50)
				);


				insert into LicensePlatesOrder( Date, LicensePlate) 
				Select CONVERT(DateTime, DateTimeStrNew , 120) as DateTime,
					   Ltrim(Rtrim(LicensePlateNumber)) as LicensePlate
				From
				(
				select DateTimeStr,
					Substring(DateTimeStr,7,4)+'-'
				   +Left(DateTimeStr,2)+'-'
				   +Substring(DateTimeStr,4,2)				   
				   +Substring(DateTimeStr,11,9) DateTimeStrNew, 
					LicensePlateNumber 
				from LicensePlates
				) Q
				Order By 2 /* Cast(DateTimeStr As DateTime) */;

		End Try

		Begin Catch
			--select 	@pErrCode	= 1, @pErrMsg 	= 'Procedure usp_MV_ImportAndInsertDataFromCsvFile failed.'
		End Catch; 
	End;  
