IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Report/AccountStatement/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (210, 1004, 10, N'Account Statement', N'AccountStatement', NULL, NULL, N'<i class="fas fa-clipboard-list nav-icon"></i>', NULL, N'/Report/AccountStatement/List', 1)
 
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FkRoleID)
Select PKFormID,1,1,1,1,1,(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where FKMasterFormID>0 and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)