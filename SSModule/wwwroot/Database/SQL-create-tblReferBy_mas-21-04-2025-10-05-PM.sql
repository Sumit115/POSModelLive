IF Not EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'tblReferBy_mas') 
BEGIN
     
CREATE TABLE [dbo].[tblReferBy_mas](
	[PkReferById] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NULL,
	[Name] [nvarchar](125) NOT NULL,
	[FatherName] [nvarchar](max) NULL,
	[MotherName] [nvarchar](max) NULL,
	[Marital] [nvarchar](max) NULL,
	[Gender] [nvarchar](max) NULL,
	[Dob] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Aadhar] [nvarchar](max) NULL,
	[Panno] [nvarchar](max) NULL,
	[Gstno] [nvarchar](max) NULL,
	[Passport] [nvarchar](max) NULL,
	[AadharCardFront] [nvarchar](max) NULL,
	[AadharCardBack] [nvarchar](max) NULL,
	[PanCard] [nvarchar](max) NULL,
	[Signature] [nvarchar](max) NULL,
	[IsAadharVerify] [int] NULL,
	[IsPanVerify] [int] NULL,
	[Status] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[StateName] [nvarchar](50) NULL,
	[FkCityId] [bigint] NULL,
	[Salary] [decimal](18, 2) NULL,
	[Post] [nvarchar](100) NULL,
	[Location] [nvarchar](max) NULL,
	[Pin] [nvarchar](50) NULL,
	[FKUserID] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[FKCreatedByID] [bigint] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[Disc] [decimal](18, 0) NOT NULL,
	[FkAccountID] [bigint] NULL,
 CONSTRAINT [PK_tblReferBy_mas] PRIMARY KEY CLUSTERED 
(
	[PkReferById] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[tblReferBy_mas] ADD  DEFAULT ((0)) FOR [Disc]


ALTER TABLE [dbo].[tblReferBy_mas]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblReferBy_mas_tblUser_mas] FOREIGN KEY([FKCreatedByID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])


ALTER TABLE [dbo].[tblReferBy_mas] CHECK CONSTRAINT [FK_CreatedBy_tblReferBy_mas_tblUser_mas]


ALTER TABLE [dbo].[tblReferBy_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblReferBy_mas_tblUser_mas] FOREIGN KEY([FKUserID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])


ALTER TABLE [dbo].[tblReferBy_mas] CHECK CONSTRAINT [FK_tblReferBy_mas_tblUser_mas]

ALTER TABLE [dbo].[tblReferBy_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblReferBy_mas_tblCity_mas] FOREIGN KEY([FkCityId])
REFERENCES [dbo].[tblCity_mas] ([PkCityId])


ALTER TABLE [dbo].[tblReferBy_mas] CHECK CONSTRAINT [FK_tblReferBy_mas_tblCity_mas]



END