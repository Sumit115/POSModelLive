IF Not EXISTS(Select * from tblsysdefaults where SysDefKey='BarcodePrint_FontSize')
 INSERT [dbo].[tblSysDefaults] ([PKSysDefID], [SysDefKey], [SysDefValue], [FKTableName], [FKColumnName], [FKUserID], [DATE_MODIFIED]) VALUES ((Select Isnull((Select max(PKSysDefID) from tblsysdefaults),0)+1), N'BarcodePrint_FontSize', N'18', NULL, NULL, 1, GetDate())

 