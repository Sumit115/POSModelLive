  
IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Option/SysDefaults/Create')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (1006, Null, 8, N'Option', N'Option', NULL, NULL, N'<i class="fa-spell-check fas nav-icon"></i>', 'P', NULL, 1)

IF Not EXISTS(Select * from tblForm_Mas where WebURL  ='/Option/SysDefaults/Create')
 INSERT [dbo].[tblForm_Mas] ([PKFormID], [FKMasterFormID], [SeqNo], [FormName], [ShortName], [ShortCut], [ToolTip], [Image], [FormType], [WebURL], [IsActive]) VALUES (350, 1006, 1, N'System Defaults', N'System Defaults', NULL, NULL, N'<i class="far fa-dot-circle nav-icon"></i>', NULL, N'/Option/SysDefaults/Create', 1)
  
IF Not EXISTS(Select * from tblLocation_mas where Location  ='Hold')
  INSERT [dbo].[tblLocation_mas] ( [Location], [Alias], [IsBillingLocation], [IsAllProduct], [IsAllCustomer], [IsAllVendor], [IsAllAccount], [Address], [FKStationID], [FKLocalityID], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [IsDifferentTax], [FKAccountID], [FKBranchID], [IsAllCostCenter], [FKDefaultUserID], [FKCreatedByID], [CreationDate], [FKUserID], [ModifiedDate], [State], [FkCityId]) VALUES ( N'Hold', N'Hold', 1, 0, 0, 0, 0, NULL, 1, 1, NULL, N'0000000000', NULL, N'00', N'jaipursoft@gmail.com', N'ss', 0, 2, 1, 0, NULL, 1, Getdate(), 1, Getdate(), NULL, 1)
     
IF Not EXISTS(Select * from tblSeries_mas where TranAlias  ='LORD')
	INSERT [dbo].[tblSeries_mas] ( [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES ( N'A', 0, 1, N'MRP', N'LORD', NULL, NULL, 1, 1, 1, 1, 1, 1, (Select top 1 PKLocationID from tblLocation_mas), 1, Getdate(), 1, Getdate(), N'B')

IF Not EXISTS(Select * from tblSeries_mas where TranAlias  ='LINV')
	INSERT [dbo].[tblSeries_mas] ( [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES ( N'A', 0, 1, N'MRP', N'LINV', NULL, NULL, 1, 1, 1, 1, 1, 1, (Select top 1 PKLocationID from tblLocation_mas), 1, Getdate(), 1, Getdate(), N'B')
