 
Create procedure [dbo].[usp_StockAndSalesAnalysis]
(
	@FromDate date,
	@ToDate date,
	@GroupByColumn  nvarchar(max)='',--'CategoryGroupName,CategoryName,Product,Batch'
	@ProductFilter  dbo.[Filter] ReadOnly,	  
	@LocationFilter  dbo.[Filter] ReadOnly	 
)
as 
begin
	 
				-- Step 1: Get all sales data up to the end date
				SELECT 
					trn.EntryDate,
					dtl.FkProductId,
					dtl.FkLotId,
					dtl.FKLocationID,
					(dtl.Qty + dtl.FreeQty) AS SaleQTY
				INTO #tmpSale
				FROM tblSalesInvoice_dtl AS dtl
					INNER JOIN tblSalesInvoice_trn AS trn 
						ON dtl.FKID = trn.pkId 
						AND dtl.FKSeriesId = trn.FKSeriesId  
						WHERE trn.EntryDate < @ToDate;

				-- Step 2: Aggregate sales for the specified month period
				SELECT 
					FkProductId,
					FkLotId,
					FKLocationID,
					SUM(SaleQTY) AS SaleQTY
				INTO #tmpMonthSale
				FROM #tmpSale
				WHERE EntryDate >= @FromDate 
					AND EntryDate <= @ToDate
				GROUP BY FkProductId, FkLotId, FKLocationID;

				-- Step 3: Get stock data as of the end date
				SELECT 
					FKProductId,
					FKLocationId,
					FKLotID,
					(OpStock + InStock) AS InStock
				INTO #tmpStock
				FROM tblProdStock_Dtl 
				WHERE StockDate <= @ToDate;

				-- Step 4: Calculate current stock and monthly sales
				WITH TotalSales AS (
					SELECT 
						FkProductId,
						FkLotId,
						FKLocationID,
						SUM(SaleQTY) AS SaleQTY
					FROM #tmpSale
					GROUP BY FkProductId, FkLotId, FKLocationID
				)
				SELECT 
					stk.FkProductId,
					stk.FkLotId,
					stk.FKLocationID,
					(stk.InStock - ISNULL(sale.SaleQTY, 0)) AS Stock,
					ISNULL(monSale.SaleQTY, 0) AS Sale
				INTO #dyStock
				FROM #tmpStock AS stk
					LEFT JOIN TotalSales AS sale
						ON stk.FkProductId = sale.FkProductId 
						AND stk.FkLotId = sale.FkLotId 
						AND stk.FKLocationID = sale.FKLocationID
					LEFT JOIN #tmpMonthSale AS monSale
						ON stk.FkProductId = monSale.FkProductId
						AND stk.FkLotId = monSale.FkLotId
						AND stk.FKLocationID = monSale.FKLocationID;

				-- Step 5: Get detailed information with product, category, and location details
				SELECT 
					prod.NameToDisplay,
					lot.Batch,
					lot.Color,
					cat.CategoryName,
					catgrp.CategoryGroupName,
					loc.Location, 
					dy.Stock,
					dy.Sale
				into #temptbl		
				FROM #dyStock AS dy
					INNER JOIN tblProdLot_dtl AS lot 
						ON dy.FKLotID = lot.PkLotId 
						AND dy.FKProductId = lot.FKProductId
					INNER JOIN tblProduct_mas AS prod 
						ON dy.FKProductId = prod.PkProductId
					INNER JOIN tblCategory_mas AS cat 
						ON prod.FKProdCatgId = cat.PkCategoryId
					INNER JOIN tblCategoryGroup_mas AS catgrp 
						ON cat.FkCategoryGroupId = catgrp.PkCategoryGroupId
					INNER JOIN tblLocation_mas AS loc 
						ON dy.FKLocationID = loc.PKLocationID
						--Apply Location,Product Filter
					Where (case when (Select Count(PKID) From @LocationFilter where PKID=dy.FKLocationID)>0 then dy.FKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then dy.FKLocationID else 0 end)
					And (case when (Select Count(PKID) From @ProductFilter where PKID=dy.FkProductId)>0 then dy.FkProductId else 0 end)=(case when (Select Count(PKID) From @ProductFilter where PKID>0)>0 then dy.FkProductId else 0 end)
				
		 Declare @qry nvarchar(max);
		 if ISNULL(@GroupByColumn,'')!=''
			Begin
				set @qry  ='select ROW_NUMBER() over( order by '+@GroupByColumn+' asc) as sno,'+@GroupByColumn+' , Sum(Stock) as Stock  , Sum(Sale) as Sale  from #temptbl group by '+ @GroupByColumn
			End
		Else
			Begin
				set @qry  ='select 1 as sno,  Sum(Stock) as Stock  , Sum(Sale) as Sale   from #temptbl '
			End

		  EXEC (@qry)

				-- Cleanup temporary tables
				DROP TABLE IF EXISTS #tmpSale;
				DROP TABLE IF EXISTS #tmpMonthSale;
				DROP TABLE IF EXISTS #tmpStock;
				DROP TABLE IF EXISTS #dyStock;
				DROP TABLE IF EXISTS #tmp;
			  	 	
end

GO


