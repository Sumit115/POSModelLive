IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Master/Recipe/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (29, 1021, 6, N'Recipe', N'Recipe', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Master/Recipe/List', 1)
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FKUserID,ModifiedDate,FKCreatedByID,CreationDate,FkRoleID)
Select PKFormID,1,1,1,1,1,1,Getdate(),1,Getdate(),(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where FKMasterFormID>0 and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)

Delete from tblGridStructer where FkFormId=29