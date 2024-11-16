USE [MViewTest118]
GO
/****** Object:  StoredProcedure [dbo].[usp_MV_InsertLicenseErrorsAndFix]    Script Date: 16/11/2024 12:01:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
Created by AmirH, 16-Nov-2024.    

declare @tTable as  udt_License_Plates_Errors_Fix_Suggest_table
insert into @tTable
 select 1,
'2024/10/15 15:10:15',
'51644641',
'1465165' Union all
 Select 2,
'2024/10/15 15:10:15',
'51644641',
'1465165' 
exec usp_MV_InsertLicenseErrorsAndFix @tTable


, @pInsertRowCount OUTPUT, @pErrCode OUTPUT, @pErrMsg OUTPUT
declare @pErrCode tinyint, @pErrMsg nvarchar(1000),@pInsertRowCount int

delete from LicensePlatesErrorsAndFixSuggest
select * from LicensePlatesErrorsAndFixSuggest
*/

drop Procedure if exists usp_MV_InsertLicenseErrorsAndFix;
Go 

Create Procedure [dbo].[usp_MV_InsertLicenseErrorsAndFix]    
( 
	@pFields as udt_License_Plates_Errors_Fix_Suggest_table READONLY
)
As
Begin
	Begin Try
		Begin Tran Tran2

			--select @pErrCode= 0 ,@pErrMsg= '', @pInsertRowCount =0
			INSERT INTO LicensePlatesErrorsAndFixSuggest(ID, Date, ErorrLicensePlate, SuggestionToFixed)
			select ID, Date, ErorrLicensePlate, SuggestionToFixed from @pFields
			--select @pInsertRowCount = @@ROWCOUNT
		Commit Tran Tran2;
	End Try
		Begin Catch
		if  @@TRANCOUNT > 0
				Rollback Transaction Tran2;
		--select 	@pErrCode	= 1, @pErrMsg 	= 'Procedure usp_TP_InsertAttractionsDistance failed.'
	End Catch; 
End;  