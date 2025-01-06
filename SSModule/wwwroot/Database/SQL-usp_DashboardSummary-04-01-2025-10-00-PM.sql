Alter procedure [dbo].[usp_DashboardSummary] 
as 
BEGIN 
					declare @FromDate datetime, @ToDate datetime

					set @FromDate = (select CONVERT(DATE,dateadd(dd,-(day(getdate())-1),getdate())) )
					set @ToDate = (select dateadd(s,-1,dateadd(mm,datediff(m,0,getdate())+1,0)))  
					declare @TotalCount_PurchaseInvoice bigint,@TotalAmount_PurchaseInvoice decimal(18,2),@TotalCount_SalesInvoice bigint,@TotalAmount_SalesInvoice decimal(18,2)
					declare @TotalCount_SalesOrder bigint,@TotalAmount_SalesOrder decimal(18,2),@TotalCount_SalesChallan bigint,@TotalAmount_SalesChallan decimal(18,2)

					Select  @TotalCount_PurchaseInvoice=count(*),@TotalAmount_PurchaseInvoice=sum(trn.NetAmt) from tblPurchaseInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where c.TranAlias='PINV'  

					Select  @TotalCount_SalesOrder=count(*),@TotalAmount_SalesOrder=sum(trn.NetAmt) from tblSalesOrder_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where  c.TranAlias='SORD'  

					Select  @TotalCount_SalesInvoice=count(*),@TotalAmount_SalesInvoice=sum(trn.NetAmt) from tblSalesInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where  c.TranAlias='SINV'  

					Select  @TotalCount_SalesChallan=count(*),@TotalAmount_SalesChallan=sum(trn.NetAmt) from tblSalesInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where   c.TranAlias='SPSL'  
					 
					 
					declare @tblCurrentMonthData table ([Date] datetime, PurchaseInvoiceAmount decimal(18,2),PurchaseInvoiceCount int,SalesInvoiceAmount decimal(18,2),SalesInvoiceCount int )
					insert Into @tblCurrentMonthData([Date],PurchaseInvoiceCount,PurchaseInvoiceAmount,SalesInvoiceCount,SalesInvoiceAmount)
					Select  cast(trn.EntryDate as Date),count(*),sum(trn.NetAmt),0,0 from tblPurchaseInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where  cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
					and c.TranAlias='PINV' 
					group by cast(trn.EntryDate as Date)

					insert Into @tblCurrentMonthData([Date],PurchaseInvoiceCount,PurchaseInvoiceAmount,SalesInvoiceCount,SalesInvoiceAmount)
					Select   cast(trn.EntryDate as Date),0,0,count(*),sum(trn.NetAmt) from tblSalesInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where  cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
					and c.TranAlias='SINV'  
					group by cast(trn.EntryDate as Date)
					
					DECLARE @TempJsonData TABLE (JsonData NVARCHAR(MAX));


	INSERT INTO @TempJsonData (JsonData)
    Select 
        (
					Select 
					ISNULL(@TotalCount_PurchaseInvoice,0) as TotalCount_PurchaseInvoice
					,ISNULL(@TotalAmount_PurchaseInvoice,0) as TotalAmount_PurchaseInvoice
					,ISNULL(@TotalCount_SalesOrder,0) as TotalCount_SalesOrder
					,ISNULL(@TotalAmount_SalesOrder,0) as TotalAmount_SalesOrder
					,ISNULL(@TotalCount_SalesInvoice,0) as TotalCount_SalesInvoice
					,ISNULL(@TotalAmount_SalesInvoice,0) as TotalAmount_SalesInvoice
					,ISNULL(@TotalCount_SalesChallan,0) as TotalCount_SalesChallan
					,ISNULL(@TotalAmount_SalesChallan,0) as TotalAmount_SalesChallan
					,(Select cast([Date] as Date) as [Date],Sum(PurchaseInvoiceAmount) as PurchaseInvoiceAmount
					,Sum(PurchaseInvoiceCount) as PurchaseInvoiceCount
					,Sum(SalesInvoiceAmount) as SalesInvoiceAmount
					,Sum(SalesInvoiceCount) as SalesInvoiceCount
					from @tblCurrentMonthData group by cast([Date] as Date) FOR JSON PATH) as  CurrentMonthData 
			FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
	 SELECT * FROM @TempJsonData;
End