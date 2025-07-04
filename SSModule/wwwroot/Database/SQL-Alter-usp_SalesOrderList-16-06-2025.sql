 
ALTER procedure [dbo].[usp_SalesOrderList]
(
	
	@FromDate date ,
	@ToDate date ,
	@SeriesFilter  nvarchar(50)='',
	@DocumentType  nvarchar(50)='',
	@StatusFilter  nvarchar(50)='',
	@LocationFilter  dbo.[Filter] ReadOnly,
	@StateFilter  dbo.[FilterString] ReadOnly

)
as 
begin 
	 
		 SET DATEFORMAT dmy; Select ROW_NUMBER() over(order by a.EntryDate) as sno,
			CONVERT(varchar,a.EntryDate,105)  as Entrydt  
			,c.Series+''+Cast(a.EntryNo as nvarchar(20)) as Inum
			,a.*
			,CONVERT(varchar,a.OrderScheduleDate,105)  as OrderScheduleDt  
			--,(case when c.TranAlias='LORD' then l.Location else b.Name end) as PartyName
			--,(case when c.TranAlias='LORD' then l.Phone1 else b.Mobile end)  as PartyMobile
			--,(case when c.TranAlias='LORD' then l.Address else b.Address end)  as PartyAddress
			,(case when a.TrnStatus ='I' then 'Invoice'
				  When a.TrnStatus ='C' then 'Close'
				  Else 'Pending'
			end) as TranStatus
			from tblSalesOrder_trn as a
			left outer join  tblCustomer_mas as b on a.FkPartyId=b.PkCustomerId
			left outer join  tblLocation_mas as l on a.FkPartyId=l.PKLocationID
			left outer join  tblSeries_mas as c on a.FKSeriesId=c.PkSeriesId 
			where  cast(a.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
			and c.TranAlias=@SeriesFilter and c.DocumentType=@DocumentType
			And (ISNULL(a.TrnStatus,'P')=@StatusFilter Or ISNULL(@StatusFilter,'')='' Or  @StatusFilter ='All')
			And (case when (Select Count(PKID) From @LocationFilter where PKID=c.FKLocationID)>0 then c.FKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then c.FKLocationID else 0 end)
			And (case when (Select Count(*) From @StateFilter where [Text]=b.StateName)>0 then b.StateName else '' end)=(case when (Select Count(*) From @StateFilter where IsNull([Text],'')!='')>0 then b.StateName else '' end)
			Order by a.EntryDate
			
	 
end

