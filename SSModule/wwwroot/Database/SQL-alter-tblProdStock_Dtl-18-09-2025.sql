DECLARE @sql NVARCHAR(MAX);

-- 🔹 Drop DEFAULT constraint if exists
SELECT @sql = 'ALTER TABLE tblProdStock_Dtl DROP CONSTRAINT [' + dc.name + ']'
FROM sys.default_constraints dc
JOIN sys.columns c ON c.column_id = dc.parent_column_id AND c.object_id = dc.parent_object_id
WHERE dc.parent_object_id = OBJECT_ID('tblProdStock_Dtl')
  AND c.name = 'CurStock';

IF @sql IS NOT NULL
    EXEC(@sql);

-- 🔹 Drop CHECK constraint if exists
SELECT @sql = 'ALTER TABLE tblProdStock_Dtl DROP CONSTRAINT [' + cc.name + ']'
FROM sys.check_constraints cc
JOIN sys.columns c ON c.object_id = cc.parent_object_id
WHERE cc.parent_object_id = OBJECT_ID('tblProdStock_Dtl')
  AND c.name = 'CurStock';

IF @sql IS NOT NULL
    EXEC(@sql);

-- 🔹 Drop the column if exists
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('tblProdStock_Dtl') AND name = 'CurStock'
)
BEGIN
    ALTER TABLE tblProdStock_Dtl DROP COLUMN CurStock;
END

-- 🔹 Add it back as computed persisted not-null
ALTER TABLE tblProdStock_Dtl
ADD CurStock AS (
    ISNULL(OpStock,0) 
  + ISNULL(InStock,0) 
  - ISNULL(OutStock,0) 
  - ISNULL(AdjStock,0)
) PERSISTED;
