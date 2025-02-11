IF Not EXISTS(Select * from tblSysDefaults where SysDefKey  ='FinYear')
	INSERT [dbo].[tblSysDefaults] ([PKSysDefID], [SysDefKey], [SysDefValue], [FKTableName], [FKColumnName], [FKUserID], [DATE_MODIFIED]) VALUES ((Select max(PKSysDefID) from tblSysDefaults)+1, N'FinYear', N'2024', NULL, NULL, 1, GetDate())
 