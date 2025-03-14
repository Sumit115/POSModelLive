Create procedure [dbo].[usp_LocationRequestList]
(
	
	@FromDate date ,
	@ToDate date ,
	@SeriesFilter  nvarchar(50)='',
	@DocumentType  nvarchar(50)='',
	@LocationFilter  dbo.[Filter] ReadOnly

)
as 
begin 
	 
		SET DATEFORMAT dmy; Select ROW_NUMBER() over(order by a.EntryDate) as sno,
			CONVERT(varchar,a.EntryDate,105)  as Entrydt  
			,c.Series+''+Cast(a.EntryNo as nvarchar(20)) as Inum
			,l.PKLocationID as FkPartyId
			,a.* 
			,CONVERT(varchar,a.OrderScheduleDate,105)  as OrderScheduleDt  
			,l.Location  as PartyName
			,l.Phone1  as PartyMobile
			,l.Address   as PartyAddress
			,(case when a.TrnStatus ='I' then 'Invoice'
				  When a.TrnStatus ='C' then 'Close'
				  Else 'Pending'
			end) as TranStatus
			from tblSalesOrder_trn as a
			left outer join  tblSeries_mas as c on a.FKSeriesId=c.PkSeriesId 
			left outer join  tblLocation_mas as l on c.FKLocationID=l.PKLocationID
			where  cast(a.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
			and c.TranAlias=@SeriesFilter and c.DocumentType=@DocumentType
			And (case when (Select Count(PKID) From @LocationFilter where PKID=a.FkPartyId)>0 then a.FkPartyId else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then a.FkPartyId else 0 end)
			Order by a.EntryDate
			
	 
end

