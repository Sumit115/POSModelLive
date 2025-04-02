IF Not  EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'tblEWayDtl_Lnk')
BEGIN
   
 CREATE TABLE [dbo].[tblEWayDtl_Lnk](
	[FKID] [bigint] NOT NULL,
	[FkSeriesId] [bigint] NOT NULL,
	[EWayNo] [nvarchar](50) NULL,
	[EWayDate] [date] NULL,
	[VehicleNo] [nvarchar](20) NULL,
	[TransDocNo] [nvarchar](20) NULL,
	[TransDocDate] [date] NULL,
	[TransMode] [nvarchar](20) NULL,
	[SupplyType] [nvarchar](50) NULL,
	[Distance] [decimal](9, 2) NULL,
	[VehicleType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblEWayDtl_Lnk] PRIMARY KEY CLUSTERED 
(
	[FKID] ASC,
	[FkSeriesId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

END