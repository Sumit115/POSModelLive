IF Not  EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'tblReceipt_trn')
BEGIN 

			CREATE TABLE [dbo].[tblReceipt_trn](
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
				[BiltyDate] [date] NULL,
				[TransportName] [nvarchar](50) NULL,
				[NoOfCases] [bigint] NULL,
				[FreightType] [nvarchar](20) NULL,
				[FreightAmt] [decimal](18, 4) NULL,
				[ShipingAddress] [nvarchar](500) NULL,
				[PaymentMode] [nchar](1) NULL,
				[FKBankThroughBankID] [bigint] NULL,
				[DeliveryDate] [date] NULL,
				[ShippingMode] [nchar](1) NULL,
				[PaymentDays] [int] NULL,
				[FKReferById] [bigint] NULL,
				[FKSalesPerId] [bigint] NULL,
				[PayMode] [nvarchar](20) NULL,
				[FkRebateAccId] [bigint] NULL,
				[FkInterestAccId] [bigint] NULL,
				[FKBankPostID] [bigint] NULL,
				[Rebate] [decimal](18, 4) NULL,
				[Interest] [decimal](18, 4) NULL,
				 CONSTRAINT [PK_tblReceipt_trn] PRIMARY KEY CLUSTERED 
			(
				[FKSeriesId] ASC,
				[PkId] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_Gross__28D80438]  DEFAULT ((0)) FOR [GrossAmt]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_SgstA__29CC2871]  DEFAULT ((0)) FOR [SgstAmt]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_TaxAm__2AC04CAA]  DEFAULT ((0)) FOR [TaxAmt]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_CashD__2BB470E3]  DEFAULT ((0)) FOR [CashDiscount]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_CashD__2CA8951C]  DEFAULT ((0)) FOR [CashDiscountAmt]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_Total__2D9CB955]  DEFAULT ((0)) FOR [TotalDiscount]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_Round__2E90DD8E]  DEFAULT ((0)) FOR [RoundOfDiff]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_Shipp__2F8501C7]  DEFAULT ((0)) FOR [Shipping]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_Other__30792600]  DEFAULT ((0)) FOR [OtherCharge]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_NetAm__316D4A39]  DEFAULT ((0)) FOR [NetAmt]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_tblReceipt_trn_Cash]  DEFAULT ((0)) FOR [Cash]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_tblReceipt_trn_Credit]  DEFAULT ((0)) FOR [Credit]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_tblReceipt_trn_Cheque]  DEFAULT ((0)) FOR [Cheque]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_tblReceipt_trn_CreditCard]  DEFAULT ((0)) FOR [CreditCard]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_InvSt__353DDB1D]  DEFAULT ('O') FOR [InvStatus]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  CONSTRAINT [DF_TblReceipt_TrnSt__3631FF56]  DEFAULT ('A') FOR [TrnStatus]


			ALTER TABLE [dbo].[tblReceipt_trn] ADD  DEFAULT ((0)) FOR [FreePoint]


			ALTER TABLE [dbo].[tblReceipt_trn]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblReceipt_trn_tblUser_mas] FOREIGN KEY([FKCreatedByID])
			REFERENCES [dbo].[tblUser_mas] ([PkUserId])


			ALTER TABLE [dbo].[tblReceipt_trn] CHECK CONSTRAINT [FK_CreatedBy_tblReceipt_trn_tblUser_mas]


			ALTER TABLE [dbo].[tblReceipt_trn]  WITH CHECK ADD  CONSTRAINT [FK_tblReceipt_trn_tblCustomer_mas] FOREIGN KEY([FkPartyId])
			REFERENCES [dbo].[tblCustomer_mas] ([PkCustomerId])


			ALTER TABLE [dbo].[tblReceipt_trn] CHECK CONSTRAINT [FK_tblReceipt_trn_tblCustomer_mas]


			ALTER TABLE [dbo].[tblReceipt_trn]  WITH CHECK ADD  CONSTRAINT [FK_tblReceipt_trn_tblSeries_Mas] FOREIGN KEY([FKSeriesId])
			REFERENCES [dbo].[tblSeries_mas] ([PkSeriesId])


			ALTER TABLE [dbo].[tblReceipt_trn] CHECK CONSTRAINT [FK_tblReceipt_trn_tblSeries_Mas]


			ALTER TABLE [dbo].[tblReceipt_trn]  WITH CHECK ADD  CONSTRAINT [FK_tblReceipt_trn_tblUser_mas] FOREIGN KEY([FKUserId])
			REFERENCES [dbo].[tblUser_mas] ([PkUserId])


			ALTER TABLE [dbo].[tblReceipt_trn] CHECK CONSTRAINT [FK_tblReceipt_trn_tblUser_mas]

End

