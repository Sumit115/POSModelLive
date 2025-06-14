IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PaymentMode' AND Object_ID = Object_ID(N'dbo.tblSeries_mas'))
Begin
	Alter Table [tblSeries_mas] Add [PaymentMode] [nvarchar](50)
End 