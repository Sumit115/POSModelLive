Drop TABLE [dbo].[tblRecipe_dtl]

CREATE TABLE [dbo].[tblRecipe_dtl](
	[PkId] [bigint] IDENTITY(1,1) NOT NULL,
	[FkRecipeId] [bigint] NOT NULL,
	[SrNo] [bigint] NOT NULL,
	[TranType] [char](1) NOT NULL,
	[FkProductId] [bigint] NOT NULL,
	[Batch] [nvarchar](max) NULL,
	[Color] [nvarchar](max) NULL,
	[Qty] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tblRecipe_Dtl] PRIMARY KEY CLUSTERED 
(
	[PkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[tblRecipe_dtl]  WITH CHECK ADD  CONSTRAINT [FK_tblRecipe_Dtl_tblProduct_mas] FOREIGN KEY([FkProductId])
REFERENCES [dbo].[tblProduct_mas] ([PkProductId])


ALTER TABLE [dbo].[tblRecipe_dtl] CHECK CONSTRAINT [FK_tblRecipe_Dtl_tblProduct_mas]



