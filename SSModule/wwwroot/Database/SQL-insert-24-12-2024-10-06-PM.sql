IF Not EXISTS(Select * from tblForm_Mas where FormName  ='Location Transfer')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (1007, Null, 5, N'Location Transfer', N'Location Transfer', NULL, NULL, N'<i class="fa-spell-check fas nav-icon"></i>', 'P', NULL, 1)

IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Transactions/LocationTransferRequest/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (114, 1007, 1, N'Order', N'Order', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Transactions/LocationTransferRequest/List', 1)

IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Transactions/LocationTransferInvoice/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (115, 1007, 1, N'Invoice', N'Invoice', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Transactions/LocationTransferInvoice/List', 1)
 
IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Transactions/LocationRequest/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (116, 1007, 1, N'Location Request', N'Location Request', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Transactions/LocationRequest/List', 1)
 
IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Transactions/LocationReceive/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (117, 1007, 1, N'Location Receive', N'Location Receive', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Transactions/LocationReceive/List', 1)
 
IF Not EXISTS(Select * from tblRole_mas where RoleName='All')
 INSERT [dbo].[tblRole_mas] ([PkRoleId], [RoleName], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES ((Select Isnull((Select max(PkRoleId) from tblRole_mas),0)+1), N'All', 1, Getdate(), 1,Getdate())
 
Insert Into tblRole_Dtl(FKFormID,IsAccess,IsEdit,IsCreate,IsPrint,IsBrowse,FKUserID,ModifiedDate,FKCreatedByID,CreationDate,FkRoleID)
Select PKFormID,1,1,1,1,1,1,Getdate(),1,Getdate(),(Select PkRoleId from tblRole_mas where RoleName='All') from tblForm_Mas where FKMasterFormID>0 and PKFormID Not in (Select FKFormID from tblRole_Dtl where FkRoleID=(Select PkRoleId from tblRole_mas where RoleName='All'))
and PKFormID not in (1021,1022,1023,1024,1025,1026)