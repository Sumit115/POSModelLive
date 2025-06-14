 
 
ALTER procedure [dbo].[usp_DashboardSummary] 
@Month int=1
as 
BEGIN 
					declare @FromDate datetime, @ToDate datetime
					declare @FinYear int = (ISNULL((Select top 1 SysDefValue from tblSysDefaults where SysDefKey  ='FinYear'),2025))
					declare @Year int=(Case When @Month>=4 And @Month<=12  then @FinYear else @FinYear+1 end)
					
					Select @FromDate = (select DATEFROMPARTS(@Year,@Month,1) ),@ToDate = (select EOMONTH(DATEFROMPARTS(@Year,@Month,1)))  
					--if @ForType='Month'
					--Begin
					--	set @FromDate = (select CONVERT(DATE,dateadd(dd,-(day(getdate())-1),getdate())) )
					--	set @ToDate = (select dateadd(s,-1,dateadd(mm,datediff(m,0,getdate())+1,0)))  
					--End
					--Else
					--Begin
					--	set @FromDate = (select CAST(DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) as Date) )
					--	set @ToDate = (select CAST(DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1) as Date))  
					--End
					declare @TotalCount_PurchaseInvoice bigint,@TotalAmount_PurchaseInvoice decimal(18,2),@TotalCount_SalesInvoice bigint,@TotalAmount_SalesInvoice decimal(18,2)
					declare @TotalCount_SalesOrder bigint,@TotalAmount_SalesOrder decimal(18,2),@TotalCount_SalesChallan bigint,@TotalAmount_SalesChallan decimal(18,2)

					Select  @TotalCount_PurchaseInvoice=count(*),@TotalAmount_PurchaseInvoice=sum(trn.NetAmt) from tblPurchaseInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where ( cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and cast(@ToDate as datetime) )
					and c.TranAlias='PINV'  

					Select  @TotalCount_SalesOrder=count(*),@TotalAmount_SalesOrder=sum(trn.NetAmt) from tblSalesOrder_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where ( cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and cast(@ToDate as datetime) )
					and c.TranAlias='SORD'  

					Select  @TotalCount_SalesInvoice=count(*),@TotalAmount_SalesInvoice=sum(trn.NetAmt) from tblSalesInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where ( cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and cast(@ToDate as datetime) )
					and c.TranAlias='SINV'  

					Select  @TotalCount_SalesChallan=count(*),@TotalAmount_SalesChallan=sum(trn.NetAmt) from tblSalesInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where ( cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and cast(@ToDate as datetime) )
					and c.TranAlias='SPSL'  
					 
					 
					declare @tblGraphData table ([Date] datetime, PurchaseInvoiceAmount decimal(18,2),PurchaseInvoiceCount int,SalesInvoiceAmount decimal(18,2),SalesInvoiceCount int )
					insert Into @tblGraphData([Date],PurchaseInvoiceCount,PurchaseInvoiceAmount,SalesInvoiceCount,SalesInvoiceAmount)
					Select  cast(trn.EntryDate as Date),count(*),sum(trn.NetAmt),0,0 from tblPurchaseInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where ( cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and cast(@ToDate as datetime) )
					and c.TranAlias='PINV' 
					group by cast(trn.EntryDate as Date)

					insert Into @tblGraphData([Date],PurchaseInvoiceCount,PurchaseInvoiceAmount,SalesInvoiceCount,SalesInvoiceAmount)
					Select   cast(trn.EntryDate as Date),0,0,count(*),sum(trn.NetAmt) from tblSalesInvoice_trn as trn
					Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId  
					where ( cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and cast(@ToDate as datetime) )
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
					,(Select cast([Date] as Date) as [Date]
					,Day(cast([Date] as Date)) as [Day]
					,Sum(PurchaseInvoiceAmount) as PurchaseInvoiceAmount
					,Sum(PurchaseInvoiceCount) as PurchaseInvoiceCount
					,Sum(SalesInvoiceAmount) as SalesInvoiceAmount
					,Sum(SalesInvoiceCount) as SalesInvoiceCount
					from @tblGraphData group by cast([Date] as Date) FOR JSON PATH) as  GraphDataList 
			FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
	 SELECT * FROM @TempJsonData;
End