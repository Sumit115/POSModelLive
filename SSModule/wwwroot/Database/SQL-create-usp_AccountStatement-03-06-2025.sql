 
Create procedure [dbo].[usp_AccountStatement]
( 
	@FKAccountID bigint
)
as 
begin 
	 
		SET DATEFORMAT dmy; Select ROW_NUMBER() over(order by trn.EntryDate) as sno, 
		CONVERT(varchar,trn.EntryDate,105)  as Entrydt  
		,dtl.VoucherNarration
		,(case when dtl.VoucherAmt>0 then ABS(dtl.VoucherAmt) else  0 end) as CreditAmt
		,(case when dtl.VoucherAmt<0 then ABS(dtl.VoucherAmt) else  0 end) as DebitAmt
		,acc.Account  as AccountName_Text 
		from tblVoucher_Dtl as dtl
		inner join tblVoucher_trn as trn on dtl.FKVoucherID=trn.PKVoucherID  and dtl.FKSeriesID=trn.FKSeriesID
		inner join tblAccount_mas as acc on dtl.FKAccountID=acc.PkAccountId
		Where dtl.FKAccountID=@FKAccountID
		Order by trn.EntryDate
	 
end

 