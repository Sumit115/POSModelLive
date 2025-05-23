 
ALTER  PROCEDURE [dbo].[uspProdBarCode_Handler]
(@OutParam bigint Output,@StockDate date)ASBEGIN
	Declare @ProdBarcode bigint, @InitBarcode varchar(10), @DefBarcodeLen int, @DefBarcode bigint, @BranchNo bigint, @MaxDefBarcode bigint
	
	Set @BranchNo=0
	Set @InitBarcode = FORMAT(@StockDate,'yyMM')
	Set @DefBarcodeLen=Len(@InitBarcode)
	print @InitBarcode
	print @DefBarcodeLen

	Declare @OutParam1 bigint= Isnull((Select max(cast(STUFF(Isnull(BarCode,'XXXXXXXXX'), 1, @DefBarcodeLen +2, '') as Bigint)) From tblProdLot_Dtl 
					Where Substring(Isnull(BarCode,'XXXXXXXXX'),3,@DefBarcodeLen)=@InitBarcode And ISNUMERIC(Barcode)=1 ),0)

		
	Declare @OutParam2 bigint = isnull((Select max(cast(STUFF(Isnull(BarCode,'XXXXXXXXX'), 1, @DefBarcodeLen, '') as Bigint)) From tblProdQTYBarcode 
					Where Substring(Isnull(BarCode,'XXXXXXXXX'),0,@DefBarcodeLen+1)=@InitBarcode And ISNUMERIC(Barcode)=1),0)
	
	if(@OutParam1 > @OutParam2)
		Set @OutParam=@OutParam1
	else	
		Set @OutParam=@OutParam2
 
	
End

