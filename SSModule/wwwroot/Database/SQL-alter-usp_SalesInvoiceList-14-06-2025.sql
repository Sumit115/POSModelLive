 
ALTER procedure [dbo].[usp_SalesInvoiceList]
(
	
	@FromDate date,
	@ToDate date,
	@SeriesFilter  nvarchar(50)='',
	@DocumentType  nvarchar(50)='',
	@LocationFilter  dbo.[Filter] ReadOnly,
	@StateFilter  dbo.[FilterString] ReadOnly

)
as 
begin 
	 
		SET DATEFORMAT dmy; Select ROW_NUMBER() over(order by a.EntryDate) as sno,
			CONVERT(varchar,a.EntryDate,105)  as Entrydt  
			,c.Series+''+Cast(a.EntryNo as nvarchar(20)) as Inum
			,a.*
			--,(case when Series.TranAlias='LORD' then l.Location else vender.Name end) as PartyName
			--,(case when Series.TranAlias='LORD' then l.Phone1 else vender.Mobile end) as PartyMobile
			--,(case when Series.TranAlias='LORD' then '' else vender.Gstno end) as PartyGSTN
			--,(case when Series.TranAlias='LORD' then l.Address else vender.Address end) as PartyAddress
			,(case when c.TranAlias='LORD' then l.State else b.StateName end) as PartyStateName
			,(Select Sum(Qty+FreeQty) from tblSalesInvoice_dtl where FkId=a.PkId and FKSeriesId=a.FKSeriesId) as ProductCount
			,salesPer.Name as SalesPersonName,salesPer.Mobile as SalesPersonMobile
			,referby.Name as ReferByName,referby.Mobile as ReferByMobile
			from tblSalesInvoice_trn as a
			Inner join  tblSeries_mas as c on a.FKSeriesId=c.PkSeriesId 
			left outer join  tblCustomer_mas as b on a.FkPartyId=b.PkCustomerId
			left outer join  tblLocation_mas as l on a.FkPartyId=l.PKLocationID
			left outer join  tblEmployee_mas as salesPer on a.FKSalesPerId=salesPer.PkEmployeeId
			left outer join  tblReferBy_mas as referby on a.FKReferById=referby.PkReferById
			where  cast(a.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
			and c.TranAlias=@SeriesFilter and c.DocumentType=@DocumentType
			And (case when (Select Count(PKID) From @LocationFilter where PKID=c.FKLocationID)>0 then c.FKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then c.FKLocationID else 0 end)
			And (case when (Select Count(*) From @StateFilter where [Text]=b.StateName)>0 then b.StateName else '' end)=(case when (Select Count(*) From @StateFilter where IsNull([Text],'')!='')>0 then b.StateName else '' end)
		    Order by a.EntryDate 
	 
end

 