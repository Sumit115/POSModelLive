 
Alter procedure [dbo].[usp_WalkingCreditAmt]
(
	@ReportType  nvarchar(50)='S',--S=Summary | D=Detail 
	@PartyMobile  nvarchar(50)='',
	@LocationFilter  dbo.[Filter] ReadOnly

)
as 
begin 
	 SET DATEFORMAT dmy;
	 if @ReportType='S'
		BEGIN
			SELECT 
				ROW_NUMBER() OVER (ORDER BY a.PartyName) AS sno,
				a.PartyName,
				a.PartyMobile, 
				SUM(a.CreditAmt) AS TotalCreditAmt,
				'View' as [View]
			FROM tblSalesInvoice_trn AS a WITH (NOLOCK)
			LEFT JOIN tblSeries_mas AS d WITH (NOLOCK)  ON a.FKSeriesId = d.PkSeriesId 
			WHERE 
				d.TranAlias = 'SINV' 
				AND d.DocumentType = 'C'  and a.CreditAmt>0
				--And cast(a.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
				 And (case when (Select Count(PKID) From @LocationFilter where PKID=d.FKLocationID)>0 then d.FKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then d.FKLocationID else 0 end)
					GROUP BY 
				a.PartyMobile,
				a.PartyName
			ORDER BY 
				a.PartyName ASC;

 		END
	
	Else  if @ReportType='D'
		BEGIN
			SELECT 
				ROW_NUMBER() OVER (ORDER BY a.EntryDate) AS sno,
				a.PartyName,
				a.PartyMobile,
				d.Series+''+Cast(a.EntryNo as nvarchar(20)) as Inum,
				CONVERT(varchar,a.EntryDate,105)  as Entrydt ,
				 a.CreditAmt 
			FROM tblSalesInvoice_trn AS a WITH (NOLOCK)
			LEFT JOIN tblSeries_mas AS d WITH (NOLOCK)  ON a.FKSeriesId = d.PkSeriesId 
			WHERE 
				d.TranAlias = 'SINV' 
				AND d.DocumentType = 'C'  and a.CreditAmt>0 and a.PartyMobile=@PartyMobile
				--And cast(a.EntryDate as datetime) between cast(@FromDate as datetime) and DATEADD(DAY, 1, cast(@ToDate as datetime))
				 And (case when (Select Count(PKID) From @LocationFilter where PKID=d.FKLocationID)>0 then d.FKLocationID else 0 end)=(case when (Select Count(PKID) From @LocationFilter where PKID>0)>0 then d.FKLocationID else 0 end)
			 ORDER BY 
				a.EntryDate ASC;

 		END
end

GO


