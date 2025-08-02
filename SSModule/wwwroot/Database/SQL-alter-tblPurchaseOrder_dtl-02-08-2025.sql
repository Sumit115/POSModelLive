IF COL_LENGTH('tblPurchaseOrder_dtl', 'PromotionType') IS NOT NULL
BEGIN
    -- Column exists, so ALTER (example: change datatype)
    ALTER TABLE tblPurchaseOrder_dtl ALTER COLUMN PromotionType char(5); 
END
ELSE
BEGIN
    -- Column does not exist, so ADD
    ALTER TABLE tblPurchaseOrder_dtl ADD PromotionType char(5);
END
