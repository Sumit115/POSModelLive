 
Create procedure [dbo].[usp_GetSysDefaults]
as 
begin
 
	DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX);

SET @cols = STUFF((SELECT distinct ',' + QUOTENAME([SysDefKey]) 
                   FROM tblSysDefaults c
                   FOR XML PATH(''), TYPE
                  ).value('.', 'NVARCHAR(MAX)') 
                 ,1,1,'')

--SELECT @cols as Columns;  -- just for debug

			set @query = 'SELECT [ID], ' + @cols + ' from 
						(
							select 1 as ID
								, [SysDefValue]
								, [SysDefKey]
							from tblSysDefaults
					   ) x
						pivot 
						(
							 max([SysDefValue])
							for [SysDefKey] in (' + @cols + ')
						) p '

			--SELECT @query as Query;  -- just for debug    

			execute(@query);

end

