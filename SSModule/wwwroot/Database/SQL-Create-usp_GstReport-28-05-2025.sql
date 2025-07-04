 
Create procedure [dbo].[usp_GSTReport]
( 
	@FromDate date,
	@ToDate date,
	@TranAlias  nvarchar(50)='',
	@ReportType  nvarchar(50)='',
	@LocationFilter  dbo.[Filter] ReadOnly

)
as 
begin 
	 
		SET DATEFORMAT dmy; Select ROW_NUMBER() over(order by trn.EntryDate) as sno,
			trn.PartyName as BuyerName,party.Gstno as BuyerGSTNo,party.StateName as BuyerState
			,branch.BranchName as SellerName,'' as SellerGSTNo,branch.State as SellerState
			,trn.NetAmt as BillAmount,series.Series+''+Cast(trn.EntryNo as nvarchar(20)) as Inum
			,dtl.* from
			(Select a.FkId, max(b.HSNCode) as HSNCode,(sum(a.Qty)+sum(a.FreeQty)) as Qty,sum(a.TaxableAmt) as TaxableAmt,a.ICRate as IGSTRate,sum(ICAmt) as IGSTAmt,a.SCRate as CGSTRate,sum(SCAmt) as CGSTAmt,a.SCRate as SGSTRate,sum(SCAmt) as SGSTAmt,sum(a.NetAmt) as NetAmt from tblSalesInvoice_dtl a inner join tblProduct_mas b on a.FkProductId=b.PkProductId group by a.ICRate,a.SCRate,a.FkId	) as dtl 
			Inner join tblSalesInvoice_trn trn on dtl.FkId=trn.PkId
			Inner join  tblSeries_mas as series on trn.FKSeriesId=series.PkSeriesId 
			Inner join  tblLocation_mas as loc on series.FKLocationID=loc.PKLocationID 
			Inner join  tblBranch_mas as branch on loc.FKBranchID=branch.PkBranchId 
			Left Outer join tblCustomer_mas party on trn.FkPartyId=party.PkCustomerId	
			Where   series.TranAlias=@TranAlias and series.DocumentType=@ReportType
			And  cast(trn.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
			 And (case when (Select Count(PKID) From @LocationFilter where PKID=series.FKLocationID)>0 then series.FKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then series.FKLocationID else 0 end)
			Order by trn.EntryDate 
	 
end

 