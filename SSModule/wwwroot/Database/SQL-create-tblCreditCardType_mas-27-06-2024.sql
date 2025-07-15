IF Not  EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'tblCreditCardType_mas')
 BEGIN
		CREATE TABLE [dbo].[tblCreditCardType_mas](
			[PkCreditCardTypeId] [bigint] IDENTITY(1,1) NOT NULL,
			[CreditCardType] [nvarchar](125) NOT NULL,
			[FkAccountID] [bigint] NULL,
			[Assembly] [nvarchar](200) NULL,
			[Class] [nvarchar](200) NULL,
			[Marital] [nvarchar](200) NULL,
			[Method] [nvarchar](200) NULL,
			[Parameter] [nvarchar](200) NULL,
			[FKUserID] [bigint] NOT NULL,
			[ModifiedDate] [datetime] NOT NULL,
			[FKCreatedByID] [bigint] NOT NULL,
			[CreationDate] [datetime] NOT NULL, 
		 CONSTRAINT [PK_tblCreditCardType_mas] PRIMARY KEY CLUSTERED 
		(
			[PkCreditCardTypeId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] 
		 
 
		ALTER TABLE [dbo].[tblCreditCardType_mas]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblCreditCardType_mas_tblUser_mas] FOREIGN KEY([FKCreatedByID])
		REFERENCES [dbo].[tblUser_mas] ([PkUserId])
		 

		ALTER TABLE [dbo].[tblCreditCardType_mas] CHECK CONSTRAINT [FK_CreatedBy_tblCreditCardType_mas_tblUser_mas]
		 

		ALTER TABLE [dbo].[tblCreditCardType_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblCreditCardType_mas_tblAccount_mas] FOREIGN KEY([FkAccountID])
		REFERENCES [dbo].[tblAccount_mas] ([PkAccountId])
		 

		ALTER TABLE [dbo].[tblCreditCardType_mas] CHECK CONSTRAINT [FK_tblCreditCardType_mas_tblAccount_mas]
		 
 
		ALTER TABLE [dbo].[tblCreditCardType_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblCreditCardType_mas_tblUser_mas] FOREIGN KEY([FKUserID])
		REFERENCES [dbo].[tblUser_mas] ([PkUserId])
		 

		ALTER TABLE [dbo].[tblCreditCardType_mas] CHECK CONSTRAINT [FK_tblCreditCardType_mas_tblUser_mas]
		 

END
