IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Report/GSTReport/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (209, 1004, 9, N'GST Report', N'GSTReport', NULL, NULL, N'<i class="fas fa-clipboard-list nav-icon"></i>', NULL, N'/Report/GSTReport/List', 1)
 
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FkRoleID)
Select PKFormID,1,1,1,1,1,(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where FKMasterFormID>0 and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)