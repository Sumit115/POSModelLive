IF NOT EXISTS (
    SELECT 1 
    FROM sys.indexes 
    WHERE name = 'U_CouponCode' 
      AND object_id = OBJECT_ID('tblCouponCode_lnk')
)
BEGIN
    ALTER TABLE tblCouponCode_lnk
    ADD CONSTRAINT U_CouponCode UNIQUE(CouponCode);
END
