IF Not EXISTS(Select * from tblForm_Mas where FormName  ='Dashboard')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (0, Null, 0, N'Dashboard', N'Dashboard', NULL, NULL, N'<i class="bi bi-house-door nav-icon"></i>', NULL, N'/Dashboard', 1)
 
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FkRoleID)
Select PKFormID,1,0,0,0,0,(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where (FKMasterFormID>0 OR PKFormID=0) and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)