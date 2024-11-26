
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Image1' AND Object_ID = Object_ID(N'dbo.tblbranch_mas'))
 Alter Table tblbranch_mas Add Image1 nvarchar(500) 
  