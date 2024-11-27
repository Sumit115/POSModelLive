  
IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Master/Location/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (24, 1025, 2, N'Location', N'Location', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Master/Location/List', 1)

IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Master/Unit/List')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (31, 1021, 1, N'Unit', N'Unit', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Master/Unit/List', 1)
   