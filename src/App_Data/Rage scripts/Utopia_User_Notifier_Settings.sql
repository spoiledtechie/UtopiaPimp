/*
   den 7 januari 201100:53:54
   User: 
   Server: .\SQLEXPRESS2010
   Database: 
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.Utopia_User_Notifier_Settings.User_Id', N'Tmp_User_ID_6', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Utopia_User_Notifier_Settings.Tmp_User_ID_6', N'User_ID', 'COLUMN' 
GO
ALTER TABLE dbo.Utopia_User_Notifier_Settings SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Utopia_User_Notifier_Settings', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Utopia_User_Notifier_Settings', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Utopia_User_Notifier_Settings', 'Object', 'CONTROL') as Contr_Per 