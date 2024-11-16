USE [MViewTest118]
GO
/****** Object:  StoredProcedure [dbo].[usp_MV_GetLicensePlates]    Script Date: 13/11/2024 12:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
Created by AmirH, 13-Nov-2024. 

	exec usp_MV_GetLicensePlates 

	select * from LicensePlatesOrder
*/

drop Procedure if exists usp_MV_GetLicensePlates;
Go
Create Procedure [dbo].[usp_MV_GetLicensePlates]    

As
	Begin
		select ID, Date, LicensePlate 
		from LicensePlatesOrder
		where Date is not null 
		Order By Date
	End;  

