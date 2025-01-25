CREATE TABLE [dbo].[tblJobWork_dtl](
	[FkId] [bigint] NOT NULL,
	[FKSeriesId] [bigint] NOT NULL,
	[SrNo] [bigint] NOT NULL,
	[FkProductId] [bigint] NOT NULL,
	[FkLotId] [bigint] NOT NULL,
	[FKLocationID] [bigint] NOT NULL,
	[Batch] [nvarchar](50) NULL,
	[Color] [nvarchar](50) NULL,
	[MfgDate] [date] NULL,
	[ExpiryDate] [date] NULL,
	[MRP] [decimal](18, 2) NULL,
	[SaleRate] [decimal](18, 2) NULL,
	[Rate] [decimal](18, 2) NULL,
	[RateUnit] [nchar](1) NULL,
	[Qty] [decimal](18, 2) NULL,
	[FreeQty] [decimal](18, 2) NULL,
	[GrossAmt] [decimal](18, 2) NULL,
	[SchemeDisc] [decimal](18, 2) NULL,
	[SchemeDiscType] [nchar](1) NULL,
	[SchemeDiscAmt] [decimal](18, 2) NULL,
	[TradeDisc] [decimal](18, 2) NULL,
	[TradeDiscType] [nchar](1) NULL,
	[TradeDiscAmt] [decimal](18, 2) NULL,
	[LotDisc] [decimal](18, 2) NULL,
	[TaxableAmt] [decimal](18, 2) NULL,
	[ICRate] [decimal](18, 2) NULL,
	[ICAmt] [decimal](18, 2) NULL,
	[SCRate] [decimal](18, 2) NULL,
	[SCAmt] [decimal](18, 2) NULL,
	[NetAmt] [decimal](18, 2) NULL,
	[Remark] [nvarchar](max) NULL,
	[TaxCalcMethod] [nchar](1) NULL,
	[CessAmt] [decimal](18, 4) NULL,
	[FKOrderID] [bigint] NULL,
	[OrderSrNo] [bigint] NULL,
	[FKOrderSrID] [bigint] NULL,
	[FKChallanID] [bigint] NULL,
	[ChallanSrNo] [bigint] NULL,
	[FKChallanSrID] [bigint] NULL,
	[FKUserID] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[FKCreatedByID] [bigint] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[Barcode] [nvarchar](max) NULL,
	[DueQty] [decimal](18, 2) NOT NULL,
	[LinkSrNo] [bigint] NULL,
	[PromotionType] [char](4) NULL,
 	[TranType] [char](1) NULL,
 CONSTRAINT [PK_tblJobWork_dtl] PRIMARY KEY CLUSTERED 
(
	[FKSeriesId] ASC,
	[FkId] ASC,
	[SrNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
CREATE TABLE [dbo].[tblJobWork_trn](
	[PkId] [bigint] IDENTITY(1,1) NOT NULL,
	[FKSeriesId] [bigint] NOT NULL,
	[EntryNo] [bigint] NOT NULL,
	[EntryDate] [date] NOT NULL,
	[EntryTime] [time](7) NOT NULL,
	[FkPartyId] [bigint] NOT NULL,
	[GRNo] [nvarchar](100) NULL,
	[GRDate] [datetime] NOT NULL,
	[GrossAmt] [decimal](18, 2) NULL,
	[SgstAmt] [decimal](18, 2) NULL,
	[TaxAmt] [decimal](18, 2) NULL,
	[CashDiscount] [decimal](18, 2) NULL,
	[CashDiscType] [nchar](1) NULL,
	[CashDiscountAmt] [decimal](18, 2) NULL,
	[TotalDiscount] [decimal](18, 2) NULL,
	[RoundOfDiff] [decimal](18, 2) NULL,
	[Shipping] [decimal](18, 2) NULL,
	[OtherCharge] [decimal](18, 2) NULL,
	[NetAmt] [decimal](18, 2) NULL,
	[Cash] [bit] NOT NULL,
	[CashAmt] [decimal](18, 4) NULL,
	[Credit] [bit] NOT NULL,
	[CreditAmt] [decimal](18, 4) NULL,
	[CreditDate] [date] NULL,
	[FKPostAccID] [bigint] NULL,
	[Cheque] [bit] NOT NULL,
	[ChequeAmt] [decimal](18, 4) NULL,
	[ChequeNo] [nvarchar](30) NULL,
	[ChequeDate] [date] NULL,
	[FKBankChequeID] [bigint] NULL,
	[CreditCard] [bit] NOT NULL,
	[CreditCardAmt] [decimal](18, 4) NULL,
	[CreditCardNo] [nvarchar](30) NULL,
	[CreditCardDate] [date] NULL,
	[FKBankCreditCardID] [bigint] NULL,
	[Remark] [nvarchar](max) NULL,
	[InvStatus] [nchar](1) NULL,
	[DraftMode] [bit] NOT NULL,
	[TrnStatus] [nchar](10) NULL,
	[FKUserId] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[FKCreatedByID] [bigint] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[PartyName] [nvarchar](50) NULL,
	[PartyMobile] [nvarchar](50) NULL,
	[PartyAddress] [nvarchar](max) NULL,
	[PartyDob] [nvarchar](50) NULL,
	[PartyMarriageDate] [nvarchar](50) NULL,
	[TradeDiscAmt] [decimal](18, 2) NULL,
	[BiltyNo] [nvarchar](50) NULL,
	[FreePoint] [decimal](18, 2) NOT NULL,
	[FkHoldLocationId] [bigint] NULL,
 CONSTRAINT [PK_tblJobWork_trn] PRIMARY KEY CLUSTERED 
(
	[FKSeriesId] ASC,
	[PkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [MRP]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [SaleRate]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [Rate]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [Qty]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [FreeQty]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [GrossAmt]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [SchemeDisc]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [SchemeDiscAmt]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [TradeDisc]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [TradeDiscAmt]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [LotDisc]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [ICRate]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [ICAmt]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [SCRate]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [SCAmt]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [NetAmt]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ('T') FOR [TaxCalcMethod]

ALTER TABLE [dbo].[tblJobWork_dtl] ADD  DEFAULT ((0)) FOR [DueQty]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Gross__28D80438]  DEFAULT ((0)) FOR [GrossAmt]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_SgstA__29CC2871]  DEFAULT ((0)) FOR [SgstAmt]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_TaxAm__2AC04CAA]  DEFAULT ((0)) FOR [TaxAmt]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_CashD__2BB470E3]  DEFAULT ((0)) FOR [CashDiscount]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_CashD__2CA8951C]  DEFAULT ((0)) FOR [CashDiscountAmt]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Total__2D9CB955]  DEFAULT ((0)) FOR [TotalDiscount]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Round__2E90DD8E]  DEFAULT ((0)) FOR [RoundOfDiff]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Shipp__2F8501C7]  DEFAULT ((0)) FOR [Shipping]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Other__30792600]  DEFAULT ((0)) FOR [OtherCharge]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_NetAm__316D4A39]  DEFAULT ((0)) FOR [NetAmt]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Cash]  DEFAULT ((0)) FOR [Cash]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Credit]  DEFAULT ((0)) FOR [Credit]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_Cheque]  DEFAULT ((0)) FOR [Cheque]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_CreditCard]  DEFAULT ((0)) FOR [CreditCard]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_InvSt__353DDB1D]  DEFAULT ('O') FOR [InvStatus]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  CONSTRAINT [DF_tblJobWork_trn_TrnSt__3631FF56]  DEFAULT ('A') FOR [TrnStatus]

ALTER TABLE [dbo].[tblJobWork_trn] ADD  DEFAULT ((0)) FOR [FreePoint]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblJobWork_dtl_tblUser_mas] FOREIGN KEY([FKCreatedByID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_CreatedBy_tblJobWork_dtl_tblUser_mas]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblLocation_mas] FOREIGN KEY([FKLocationID])
REFERENCES [dbo].[tblLocation_mas] ([PKLocationID])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblLocation_mas]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblProdLot_dtl] FOREIGN KEY([FkProductId], [FkLotId])
REFERENCES [dbo].[tblProdLot_dtl] ([FKProductId], [PkLotId])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblProdLot_dtl]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblProduct_mas] FOREIGN KEY([FkProductId])
REFERENCES [dbo].[tblProduct_mas] ([PkProductId])
ON UPDATE CASCADE

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblProduct_mas]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblJobWork_trn] FOREIGN KEY([FKSeriesId], [FkId])
REFERENCES [dbo].[tblJobWork_trn] ([FKSeriesId], [PkId])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblJobWork_trn]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblJobWork_trn1] FOREIGN KEY([FKChallanSrID], [FKChallanID])
REFERENCES [dbo].[tblJobWork_trn] ([FKSeriesId], [PkId])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblJobWork_trn1]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblSeries_Mas] FOREIGN KEY([FKSeriesId])
REFERENCES [dbo].[tblSeries_mas] ([PkSeriesId])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblSeries_Mas]

ALTER TABLE [dbo].[tblJobWork_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_dtl_tblUser_mas] FOREIGN KEY([FKUserID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])

ALTER TABLE [dbo].[tblJobWork_dtl] CHECK CONSTRAINT [FK_tblJobWork_dtl_tblUser_mas]

ALTER TABLE [dbo].[tblJobWork_trn]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblJobWork_trn_tblUser_mas] FOREIGN KEY([FKCreatedByID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])

ALTER TABLE [dbo].[tblJobWork_trn] CHECK CONSTRAINT [FK_CreatedBy_tblJobWork_trn_tblUser_mas]

ALTER TABLE [dbo].[tblJobWork_trn]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_trn_tblCustomer_mas] FOREIGN KEY([FkPartyId])
REFERENCES [dbo].[tblCustomer_mas] ([PkCustomerId])

ALTER TABLE [dbo].[tblJobWork_trn] CHECK CONSTRAINT [FK_tblJobWork_trn_tblCustomer_mas]

ALTER TABLE [dbo].[tblJobWork_trn]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_trn_tblSeries_Mas] FOREIGN KEY([FKSeriesId])
REFERENCES [dbo].[tblSeries_mas] ([PkSeriesId])

ALTER TABLE [dbo].[tblJobWork_trn] CHECK CONSTRAINT [FK_tblJobWork_trn_tblSeries_Mas]

ALTER TABLE [dbo].[tblJobWork_trn]  WITH CHECK ADD  CONSTRAINT [FK_tblJobWork_trn_tblUser_mas] FOREIGN KEY([FKUserId])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])

ALTER TABLE [dbo].[tblJobWork_trn] CHECK CONSTRAINT [FK_tblJobWork_trn_tblUser_mas]

