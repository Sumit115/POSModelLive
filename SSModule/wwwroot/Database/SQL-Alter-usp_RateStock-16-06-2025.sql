 
ALTER procedure [dbo].[usp_RateStock]
(
    @ReportType  nvarchar(50)='S',--S=Summary | L=Detail
  @GroupByColumn  nvarchar(max)='',--'CategoryName,NameToDisplay,Location,Batch,MRP,StockDays,Barcode '
	@ProductFilter  dbo.[Filter] ReadOnly
)
as 
begin 
if @ReportType='S' 
	 Begin
	 SET DATEFORMAT dmy; 
	 insert into spParam
	 Select PKID from @ProductFilter
	 Select ROW_NUMBER() over( order by e.CategoryName asc) as sno,
		e.CategoryName,b.NameToDisplay,c.Location,d.Batch,d.MRP,DATEDIFF(day,d.StockDate,getDate()) as StockDays, f.Barcode,
		a.OpStock as purchaseQTY,
	   a.CurStock as SaleQTY  
		from tblProdStock_Dtl as a
		inner join
		tblProdLot_dtl as d on a.FKLotID = d.PkLotId  
		inner join
		tblProduct_mas as b on d.FKProductId = b.PkProductId	
		INNER JOIN
		tblCategory_mas as e on b.FKProdCatgId = e.PkCategoryId
		Left Outer join
		tblLocation_mas as c on a.FKLocationId = c.PKLocationID
		Left Outer join
		tblSalesInvoice_dtl as f on a.FKProductId = f.FkProductId
		Where (Select Count(PKID) From @ProductFilter) = 0 Or d.FKProductId in (Select PKID From @ProductFilter)
		
		End	
 if @ReportType='L' 
	 Begin
		SET DATEFORMAT dmy; 
	 insert into spParam
	 Select PKID from @ProductFilter
	 
	
		
Select ROW_NUMBER() over( order by e.CategoryName asc) as sno,
		e.CategoryName,b.NameToDisplay,c.Location,d.Batch,d.MRP,DATEDIFF(day,d.StockDate,getDate()) as StockDays, a.Barcode,
			 1 as purchaseQTY,
			 Case when a.TranOutId is null then 0 else 1 end  as SaleQTY ,
			 Case when a.TranOutId is null then 1 else 0 end  as RemainQTY  
			into #temptbl			
		from tblProdQTYBarcode as a
		inner join
		tblProdLot_dtl as d on a.FKLotID = d.PkLotId and a.FKProductId = d.FkProductId	
		inner join
		tblProduct_mas as b on d.FKProductId = b.PkProductId	
		INNER JOIN
		tblCategory_mas as e on b.FKProdCatgId = e.PkCategoryId
		Left Outer join
		tblLocation_mas as c on a.FKLocationId = c.PKLocationID
		Where (Select Count(PKID) From @ProductFilter) = 0 Or d.FKProductId in (Select PKID From @ProductFilter)
		--group by e.CategoryName,b.NameToDisplay,c.Location, d.Batch, d.MRP, d.StockDate, f.Barcode 
       order by b.Product asc

	   Declare @qry nvarchar(max);
	   if ISNULL(@GroupByColumn,'')!=''
		Begin
			set @qry  ='select ROW_NUMBER() over( order by '+@GroupByColumn+' asc) as sno , Sum(purchaseQTY) as purchaseQTY,Sum(SaleQTY) as SaleQTY,Sum(RemainQTY) as RemainQTY,'+@GroupByColumn+' from #temptbl group by '+ @GroupByColumn
		End
	Else
		Begin
			set @qry  ='select ROW_NUMBER() over( order by purchaseQTY asc) as sno,purchaseQTY, SaleQTY,RemainQTY  from #temptbl '
		End

	   EXEC (@qry)
	   End	
end






