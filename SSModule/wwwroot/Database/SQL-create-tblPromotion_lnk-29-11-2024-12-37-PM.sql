IF Not  EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'tblPromotion_lnk')
BEGIN
   
 

CREATE TABLE [dbo].[tblPromotion_lnk](
	[PkId] [bigint] IDENTITY(1,1) NOT NULL,
	[FkPromotionId] [bigint] NOT NULL,
	[FkLinkId] [bigint] NOT NULL,
	[FKUserID] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[FKCreatedByID] [bigint] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblPromotion_lnk] PRIMARY KEY CLUSTERED 
(
	[PkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
 

ALTER TABLE [dbo].[tblPromotion_lnk]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblPromotion_lnk_tblUser_mas] FOREIGN KEY([FKCreatedByID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])
 

ALTER TABLE [dbo].[tblPromotion_lnk] CHECK CONSTRAINT [FK_CreatedBy_tblPromotion_lnk_tblUser_mas]
 

ALTER TABLE [dbo].[tblPromotion_lnk]  WITH CHECK ADD  CONSTRAINT [FK_tblPromotion_lnk_tblUser_mas] FOREIGN KEY([FKUserID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])
 

ALTER TABLE [dbo].[tblPromotion_lnk] CHECK CONSTRAINT [FK_tblPromotion_lnk_tblUser_mas]
 




END