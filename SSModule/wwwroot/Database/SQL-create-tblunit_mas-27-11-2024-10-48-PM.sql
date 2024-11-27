IF Not  EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'tblUnit_mas')
BEGIN
   CREATE TABLE [dbo].[tblUnit_mas](
	[PkUnitId] [bigint] IDENTITY(1,1) NOT NULL,
	[UnitName] [nvarchar](50) NOT NULL,
	[FKUserID] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[FKCreatedByID] [bigint] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblUnit_mas] PRIMARY KEY CLUSTERED 
(
	[PkUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[tblUnit_mas]  WITH CHECK ADD  CONSTRAINT [FK_CreatedBy_tblUnit_mas_tblUser_mas] FOREIGN KEY([FKCreatedByID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])


ALTER TABLE [dbo].[tblUnit_mas] CHECK CONSTRAINT [FK_CreatedBy_tblUnit_mas_tblUser_mas]


ALTER TABLE [dbo].[tblUnit_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblUnit_mas_tblUser_mas] FOREIGN KEY([FKUserID])
REFERENCES [dbo].[tblUser_mas] ([PkUserId])


ALTER TABLE [dbo].[tblUnit_mas] CHECK CONSTRAINT [FK_tblUnit_mas_tblUser_mas]

END