update tblForm_Mas set FormName='Walking Credit',ShortName='Walking Credit',WebURL='/Report/WalkingCreditAmt/List' where PKFormID  =213

IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Report/WalkingCreditAmt/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (213, 1004, 13, N'Walking Credit', N'Walking Credit', NULL, NULL, N'<i class="fas fa-clipboard-list nav-icon"></i>', NULL, N'/Report/WalkingCreditAmt/List', 1)
 
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FkRoleID)
Select PKFormID,1,1,1,1,1,(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where FKMasterFormID>0 and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)

