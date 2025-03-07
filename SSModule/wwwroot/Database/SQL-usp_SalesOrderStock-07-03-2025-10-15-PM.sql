 
ALTER procedure [dbo].[usp_SalesOrderStock]
(
	@ReportType  nvarchar(50)='L',
	@GroupByColumn  nvarchar(max)='',--'CategoryGroupName,CategoryName,Product,Batch'
	@StateFilter  dbo.[FilterString] ReadOnly,	 
	@TrnStatusFilter  dbo.[FilterString] ReadOnly	 
)
as 
begin
	if @ReportType='L'
		Begin
			 
			 declare @tbl Table  (FkProductId bigint,Batch nvarchar(50) ,InStock decimal(18,2),OutStock decimal(18,2),CurStock decimal(18,2))
		
				if CHARINDEX('Batch',@GroupByColumn) = 0 And CHARINDEX('Product',@GroupByColumn) > 0
				Begin
					Insert Into @tbl(FkProductId,Batch,InStock,OutStock,CurStock)
					Select  a.FkProductId, '' ,sum(a.InStock) as InStock,sum(a.OutStock) as OutStock,sum(a.CurStock) as CurStock  from tblProdStock_Dtl as a inner join tblProdLot_dtl as d on a.FKLotID = d.PkLotId and a.FKProductId = d.FkProductId	  
					group by a.FkProductId   
				End
				Else 
				Begin
					Insert Into @tbl(FkProductId,Batch,InStock,OutStock,CurStock)
					Select  a.FkProductId, d.Batch ,sum(a.InStock) as InStock,sum(a.OutStock) as OutStock,sum(a.CurStock) as CurStock  from tblProdStock_Dtl as a inner join tblProdLot_dtl as d on a.FKLotID = d.PkLotId and a.FKProductId = d.FkProductId	  
					group by a.FkProductId,  d.Batch   
				End

			Select ROW_NUMBER() over(order by dtl.FkProductId) as sno,
			 cat.CategoryGroupName,subcat.CategoryName,Product.Product,dtl.Batch
			,dtl.Qty  as OrderQty  
			,dtl.DueQty  as DueQty  
			,ISNULL(stock.InStock,0) as InQty
			,ISNULL(stock.OutStock,0) as OutQty
			,ISNULL(stock.CurStock,0) as StockQty
			,cust.Name as PartyName
			into #temptbl		
			from tblSalesOrder_dtl dtl
			inner join tblSalesOrder_trn as trn on dtl.FkId=trn.PkId
			inner join tblCustomer_mas as cust on trn.FkPartyId=cust.PkCustomerId
			inner join tblProduct_mas as Product on dtl.FkProductId=Product.PkProductId
			inner join tblCategory_mas as subcat on Product.FKProdCatgId=subcat.PkCategoryId
			inner join tblCategoryGroup_mas as cat on subcat.FkCategoryGroupId=cat.PkCategoryGroupId
			 left join  @tbl as stock on stock.FKProductId=dtl.FkProductId and  stock.Batch=(case when CHARINDEX('Batch',@GroupByColumn) = 0 then '' else dtl.Batch end) 
			--group by cat.CategoryGroupName,subcat.CategoryName,Product.Product,dtl.FkProductId,dtl.Batch
			where(case when (Select Count(*) From @StateFilter where [Text]=cust.StateName)>0 then cust.StateName else '' end)=(case when (Select Count(*) From @StateFilter where IsNull([Text],'')!='')>0 then cust.StateName else '' end)
			And (case when (Select Count(*) From @TrnStatusFilter where [Text]=(ISNULL(trn.TrnStatus,'P')))>0 then (ISNULL(trn.TrnStatus,'P')) else '' end)=(case when (Select Count(*) From @TrnStatusFilter where IsNull([Text],'')!='')>0 then (ISNULL(trn.TrnStatus,'P')) else '' end)
			order by Product

			 Declare @qry nvarchar(max);
		 if ISNULL(@GroupByColumn,'')!=''
			Begin
				set @qry  ='select ROW_NUMBER() over( order by '+@GroupByColumn+' asc) as sno,'+@GroupByColumn+' , Sum(OrderQty) as OrderQty, Sum(DueQty) as DueQty,Max(InQty) as InQty,Max(OutQty) as OutQty,Max(StockQty) as StockQty from #temptbl group by '+ @GroupByColumn
			End
		Else
			Begin
				set @qry  ='select ROW_NUMBER() over( order by OrderQty asc) as sno,OrderQty,DueQty, InQty, OutQty, StockQty  from #temptbl '
			End

		  EXEC (@qry)
		End

--where(case when (Select Count(PKID) From @ProductFilter where PKID=b.PkProductId)>0 then b.PkProductId else 0 end)=(case when (Select Count(PKID) From @ProductFilter where PKID>0)>0 then b.PkProductId else 0 end)
--	And (case when (Select Count(PKID) From @LocationFilter where PKID=c.PKLocationID)>0 then c.PKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then c.PKLocationID else 0 end)
		 	
end

