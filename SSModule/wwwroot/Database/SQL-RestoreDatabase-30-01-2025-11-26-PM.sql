ALTER proc [dbo].[RestoreDatabase]
 @flag bit
 as
 begin

truncate table tblProdQTYBarcode

truncate table tblVoucher_Dtl
truncate table tblVoucher_Trn
truncate table tblSalesOrder_dtl
Delete from tblSalesOrder_trn
truncate table tblSalesInvoice_dtl
Delete from tblSalesInvoice_trn
truncate table tblPurchaseOrder_dtl
Delete from tblPurchaseOrder_trn
truncate table tblPurchaseInvoice_dtl
Delete from tblPurchaseInvoice_trn
truncate table tblSalesCrNote_dtl
Delete from tblSalesCrNote_trn

Delete from tblCustomer_mas Where pkCustomerId>1
Delete from tblVendor_mas Where pkVendorId>1
Delete from tblEmployee_mas Where pkEmployeeId>1
Delete from tblUser_mas Where pkUserId>1
Delete from tblLocation_mas
Truncate table tblWalkingCustomer_mas
Truncate table tblProdQTYBarcode
Truncate table tblProdUniqueIdBarCode
Truncate table tblProdStock_Dtl
Delete from tblProdLot_dtl
Delete from tblProduct_mas
Truncate table tblProdColor_Mas
 
truncate table tblAccount_dtl
truncate table tblAccountLic_dtl
truncate table tblAccountLoc_lnk
Delete from tblAccount_mas
truncate table tblAccountGroup_mas
truncate table tblBranch_mas
Delete from tblBrand_mas
Delete from tblCatery_mas
Delete from tblCateryGroup_mas
truncate table tblCaterySize_lnk
truncate table tblCity_mas
truncate table tblColor_Mas
truncate table tblCompany
truncate table tblCountry_mas
truncate table tblDistrict_mas

truncate table tblError_Log
truncate table tblGridStructer
truncate table tblImgRemark_mas
truncate table tblLocality_mas
truncate table tblLog_mas
truncate table tblPromotion_mas
truncate table tblRecipe_dtl
Delete from tblRecipe_Mas
truncate table tblRegion_mas
truncate table tblReturnType_Mas
			   
truncate table tblSize_Mas
truncate table tblStation_mas
truncate table tblWallet_mas
truncate table tblZone_mas
 
delete from  tblSeries_mas 

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (1, N'PURCHASE TAXABLE ODS', N'', 3, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-05-04T16:39:31.220' AS DateTime), 1, CAST(N'2024-05-04T16:39:31.227' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (2, N'SALES TAXABLE ODS', N'', 4, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', CAST(N'2024-04-01T00:00:00.000' AS DateTime), 1, CAST(N'2024-05-04T16:39:31.220' AS DateTime), 1, CAST(N'2024-05-04T16:39:31.227' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (3, N'SGST INPUT', N'', 7, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', CAST(N'2024-04-09T00:00:00.000' AS DateTime), 1, CAST(N'2024-05-04T16:39:31.220' AS DateTime), 1, CAST(N'2024-05-04T16:39:31.227' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (4, N'SGST OUTPUT', N'', 7, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-05-23T22:15:21.330' AS DateTime), 1, CAST(N'2024-05-23T22:15:21.507' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (5, N'CGST INPUT', N'', 7, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-06-05T23:08:16.603' AS DateTime), 1, CAST(N'2024-06-05T23:08:16.683' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (6, N'CGST OUTPUT', N'', 7, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-06-08T14:51:37.627' AS DateTime), 1, CAST(N'2024-06-08T14:51:38.147' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (7, N'IGST INPUT', N'', 7, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-06-08T15:33:31.940' AS DateTime), 1, CAST(N'2024-06-08T15:33:32.030' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (8, N'IGST OUTPUT', N'', 7, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-06-08T16:47:25.280' AS DateTime), 1, CAST(N'2024-06-08T16:47:25.363' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (9, N'CASH IN HAND', N'', 5, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-07-06T22:57:41.823' AS DateTime), 1, CAST(N'2024-06-08T16:55:42.863' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (10, N'BANK ACCOUNTS', N'', 8, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-07-06T23:31:20.880' AS DateTime), 1, CAST(N'2024-07-06T23:31:22.247' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (11, N'ROUND OFF A/C', N'', 6, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-07-06T23:32:00.783' AS DateTime), 1, CAST(N'2024-07-06T23:32:01.653' AS DateTime))

INSERT [dbo].[tblAccount_mas] ([PkAccountId], [Account], [Alias], [FkAccountGroupId], [Address], [Station], [Locality], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [Dob], [Dow], [ApplyCostCenter], [ApplyTCS], [ApplyTDS], [PrintBrDtl], [FKBankID], [AccountNo], [Status], [DiscDate], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (12, N'Walking Customer', N'', 2, N'', N'', N'', N'', N'', N'', NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'Continue', NULL, 1, CAST(N'2024-07-06T23:32:00.783' AS DateTime), 1, CAST(N'2024-07-06T23:32:01.653' AS DateTime))
 
SET IDENTITY_INSERT [dbo].[tblAccountGroup_mas] ON 

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (1, 0, N'B', N'Sundry Creditors', N'10', N'L', 0, 0, 1, CAST(N'2024-07-08T23:06:10.973' AS DateTime), 1, CAST(N'2024-05-04T17:27:47.340' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (2, 0, N'T', N'Sundry Debtors', N'4', N'I', 0, 1, 1, CAST(N'2024-07-08T23:05:48.287' AS DateTime), 1, CAST(N'2024-05-04T17:27:47.340' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (3, 0, N'T', N'Purchase Accounts', N'12', NULL, 0, 0, 1, CAST(N'2024-07-08T23:05:09.320' AS DateTime), 1, CAST(N'2024-07-08T23:04:39.767' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (4, 0, NULL, N'Sales Accounts', NULL, NULL, 0, 0, 1, CAST(N'2024-07-08T23:09:06.930' AS DateTime), 1, CAST(N'2024-07-08T23:09:06.930' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (5, 0, NULL, N'Cash-In-Hand', NULL, NULL, 0, 0, 1, CAST(N'2024-07-08T23:09:55.690' AS DateTime), 1, CAST(N'2024-07-08T23:09:55.690' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (6, 0, NULL, N'InDirect Expenses', NULL, NULL, 0, 0, 1, CAST(N'2024-07-08T23:10:21.130' AS DateTime), 1, CAST(N'2024-07-08T23:10:21.130' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (7, 0, NULL, N'Duties & Taxes', NULL, NULL, 0, 0, 1, CAST(N'2024-07-08T23:11:14.050' AS DateTime), 1, CAST(N'2024-07-08T23:11:14.050' AS DateTime))

INSERT [dbo].[tblAccountGroup_mas] ([PkAccountGroupId], [FkAccountGroupId], [GroupType], [AccountGroupName], [GroupAlias], [NatureOfGroup], [PrintDtl], [NetCrDrBalanceForRpt], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate]) VALUES (8, 0, NULL, N'Bank Accounts', NULL, NULL, 0, 0, 1, CAST(N'2024-07-08T23:13:56.150' AS DateTime), 1, CAST(N'2024-07-08T23:13:56.150' AS DateTime))

SET IDENTITY_INSERT [dbo].[tblAccountGroup_mas] OFF

--
SET IDENTITY_INSERT [dbo].[tblBranch_mas] ON  

INSERT [dbo].[tblBranch_mas] ([PkBranchId], [BranchName], [ContactPerson], [Email], [Mobile], [Address], [State], [Pin], [Country], [FkRegId], [BranchCode], [FkCityId], [Location], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [Image1]) VALUES (1, N'Jaipur Soft', N'A', N'jaipursoft@gmail.com', N'0000000000', N'Jaipur', N'Rajasthan', N'000000', N'India', 1, N'B0001', 1, N'Location Jaipur', 1, GetDate(), 1, GetDate(), N'')
 
SET IDENTITY_INSERT [dbo].[tblBranch_mas] OFF
 
SET IDENTITY_INSERT [dbo].[tblLocation_mas] ON  

INSERT [dbo].[tblLocation_mas] ([PKLocationID], [Location], [Alias], [IsBillingLocation], [IsAllProduct], [IsAllCustomer], [IsAllVendor], [IsAllAccount], [Address], [FKStationID], [FKLocalityID], [Pincode], [Phone1], [Phone2], [Fax], [Email], [Website], [IsDifferentTax], [FKAccountID], [FKBranchID], [IsAllCostCenter], [FKDefaultUserID], [FKCreatedByID], [CreationDate], [FKUserID], [ModifiedDate], [State], [FkCityId]) VALUES (1, N'Jaipur', N'Jaipur', 1, 0, 0, 0, 0, NULL, 1, 1, NULL, N'0000000000', NULL, N'00', N'jaipursoft@gmail.com', N'ss', 0, 2, 1, 0, NULL, 1, Getdate(), 1, Getdate(), NULL, 1)
 
SET IDENTITY_INSERT [dbo].[tblLocation_mas] OFF
 
--
SET IDENTITY_INSERT [dbo].[tblSeries_mas] ON 

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (1, N'A', 0, 1, N'MRP', N'SORD', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-08-16T22:10:10.117' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (2, N'A', 0, 1, N'MRP', N'SINV', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (3, N'A', 0, 1, N'MRP', N'SINV', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'C')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (4, N'A', 0, 1, N'MRP', N'SPSL', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-08-10T00:22:18.660' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (5, N'A', 0, 1, N'MRP', N'PORD', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (6, N'A', 0, 1, N'MRP', N'PINV', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (7, N'A', 0, 1, N'MRP', N'SCRN', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (8, N'A', 0, 1, N'MRP', N'SRTN', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (9, N'A', 0, 1, N'MRP', N'V_JR', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (10, N'A', 0, 1, N'MRP', N'V_RC', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (11, N'A', 0, 1, N'MRP', N'V_PY', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (12, N'A', 0, 1, N'MRP', N'V_CT', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (13, N'A', 0, 1, N'MRP', N'LORD', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (14, N'A', 0, 1, N'MRP', N'LINV', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (15, N'A', 0, 1, N'MRP', N'PJ_O', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (16, N'A', 0, 1, N'MRP', N'PJ_I', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

INSERT [dbo].[tblSeries_mas] ([PkSeriesId], [Series], [SeriesNo], [FkBranchId], [BillingRate], [TranAlias], [FormatName], [ResetNoFor], [AllowWalkIn], [AutoApplyPromo], [RoundOff], [DefaultQty], [AllowZeroRate], [AllowFreeQty], [FKLocationID], [FKUserID], [ModifiedDate], [FKCreatedByID], [CreationDate], [DocumentType]) VALUES (17, N'A', 0, 1, N'MRP', N'PJ_R', NULL, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-05-04T17:09:28.517' AS DateTime), 1, CAST(N'2024-05-04T17:09:29.197' AS DateTime), N'B')

SET IDENTITY_INSERT [dbo].[tblSeries_mas] OFF

 
end