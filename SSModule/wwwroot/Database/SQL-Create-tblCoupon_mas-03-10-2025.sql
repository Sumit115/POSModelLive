IF Not  EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'tblCoupon_mas')
BEGIN

			/****** Object:  Table [dbo].[tblCoupon_mas]    Script Date: 03-10-2025 10:05:20 PM ******/
			SET ANSI_NULLS ON
			
			SET QUOTED_IDENTIFIER ON
			
			CREATE TABLE [dbo].[tblCoupon_mas](
				[PkCouponId] [bigint] NOT NULL,
				[NoOfCoupon] [bigint]  NOT NULL,
				[Amount] [Decimal](18,2) NOT NULL,
				[FKUserID] [bigint] NOT NULL,
				[ModifiedDate] [datetime] NOT NULL,
				[FKCreatedByID] [bigint] NOT NULL,
				[CreationDate] [datetime] NOT NULL,
				[SKUDefinition] [nvarchar](200) NULL,
			 CONSTRAINT [PK_tblCoupon_mas] PRIMARY KEY CLUSTERED 
			(
				[PkCouponId] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
			
			/****** Object:  Table [dbo].[tblCouponCode_lnk]    Script Date: 03-10-2025 10:05:21 PM ******/
			SET ANSI_NULLS ON
			
			SET QUOTED_IDENTIFIER ON
			
			CREATE TABLE [dbo].[tblCouponCode_lnk](
				[PkId] [bigint] IDENTITY(1,1) NOT NULL,
				[FkCouponId] [bigint] NOT NULL,
				[CouponCode] [nvarchar](12) NOT NULL,
				[FkId] [bigint]   NULL,
				[FkSeriesId] [bigint]   NULL,
				[FKUserID] [bigint] NOT NULL,
				[ModifiedDate] [datetime] NOT NULL,
				[FKCreatedByID] [bigint] NOT NULL,
				[CreationDate] [datetime] NOT NULL,
			 CONSTRAINT [PK_tblCouponCode_lnk] PRIMARY KEY CLUSTERED 
			(
				[PkId] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
			
			ALTER TABLE [dbo].[tblCoupon_mas]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblCoupon_mas_tblUser_mas] FOREIGN KEY([FKCreatedByID])
			REFERENCES [dbo].[tblUser_mas] ([PkUserId])
			
			ALTER TABLE [dbo].[tblCoupon_mas] CHECK CONSTRAINT [FK_CreatedBy_tblCoupon_mas_tblUser_mas]
			
			ALTER TABLE [dbo].[tblCoupon_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblCoupon_mas_tblUser_mas] FOREIGN KEY([FKUserID])
			REFERENCES [dbo].[tblUser_mas] ([PkUserId])
			
			ALTER TABLE [dbo].[tblCoupon_mas] CHECK CONSTRAINT [FK_tblCoupon_mas_tblUser_mas]
			
			ALTER TABLE [dbo].[tblCouponCode_lnk]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblCouponCode_lnk_tblUser_mas] FOREIGN KEY([FKCreatedByID])
			REFERENCES [dbo].[tblUser_mas] ([PkUserId])
			
			ALTER TABLE [dbo].[tblCouponCode_lnk] CHECK CONSTRAINT [FK_CreatedBy_tblCouponCode_lnk_tblUser_mas]
			
			ALTER TABLE [dbo].[tblCouponCode_lnk]  WITH CHECK ADD  CONSTRAINT [FK_tblCouponCode_lnk_tblUser_mas] FOREIGN KEY([FKUserID])
			REFERENCES [dbo].[tblUser_mas] ([PkUserId])
			
			ALTER TABLE [dbo].[tblCouponCode_lnk] CHECK CONSTRAINT [FK_tblCouponCode_lnk_tblUser_mas]
			
End
