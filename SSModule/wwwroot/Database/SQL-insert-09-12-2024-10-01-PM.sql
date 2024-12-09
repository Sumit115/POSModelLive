IF Not EXISTS(Select * from tblSysDefaults where SysDefKey  ='CodingScheme')
	INSERT [dbo].[tblSysDefaults] ([PKSysDefID], [SysDefKey], [SysDefValue], [FKTableName], [FKColumnName], [FKUserID], [DATE_MODIFIED]) VALUES (451, N'CodingScheme', N'Unique', NULL, NULL, 1, GetDate())
 