IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Transactions/JobworkIssue/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (119, 1001, 8, N'Issue Job Order', N'Issue Job Order', NULL, NULL, N'<i class="fas fa-clipboard-list nav-icon"></i>', NULL, N'/Transactions/JobworkIssue/List', 1)
 
IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Transactions/JobworkReceive/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (120, 1001, 8, N'Receive Job Order', N'Receive Job Order', NULL, NULL, N'<i class="fas fa-clipboard-list nav-icon"></i>', NULL, N'/Transactions/JobworkReceive/List', 1)
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FKUserID,ModifiedDate,FKCreatedByID,CreationDate,FkRoleID)
Select PKFormID,1,1,1,1,1,1,Getdate(),1,Getdate(),(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where FKMasterFormID>0 and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)
 