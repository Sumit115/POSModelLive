Create  PROCEDURE [dbo].[uspGetValueOfId]
 @PkId bigint,
 @TableName NVarchar(50),--='tblAccount_mas',
 @ColumnName NVarchar(50),--='Account' ,
@PkColumnName NVarchar(50),--='PkAccountId' 
@ColumnValue  NVarchar(max) output

AS
BEGIN
	 Declare @ParamDef NVarchar(2000)= '@ColumnValue NVarchar(Max) output'
	Declare @StrQry NVarchar(2000)

	if IsNull(@PkId,0)>0
	Begin
		Set @StrQry ='Select @ColumnValue='+@ColumnName+'  From '+@TableName+'   Where '+@PkColumnName+' = '+Convert(NVarchar(Max),@PkId)
	End
	 

	Exec sp_executesql @StrQry,@ParamDef,@ColumnValue Output

	 
END