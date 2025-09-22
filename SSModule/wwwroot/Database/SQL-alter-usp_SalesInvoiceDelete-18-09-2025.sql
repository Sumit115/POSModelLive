 
Alter PROCEDURE [dbo].[usp_SalesInvoiceDelete]
( 
    @PkId BIGINT,
    @FkSeriesId BIGINT,
    @FKUserId BIGINT,
    @Flag CHAR(1), -- 'D' for Delete, 'C' for Cancel
    @ErrMsg NVARCHAR(MAX) = NULL OUTPUT
)
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- Initialize variables
    SET @ErrMsg = '';
    DECLARE @InvoiceExists INT = 0;
    DECLARE @ReturnInvoiceExists INT = 0;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validation 1: Check if invoice exists and is not already processed/cancelled
        SELECT @InvoiceExists = COUNT(*) 
        FROM [dbo].[tblSalesInvoice_trn] 
        WHERE PkId = @PkId 
          AND FKSeriesId = @FkSeriesId 
          AND TrnStatus <> 'P'; -- Exclude Cancelled
        
        IF @InvoiceExists > 0
        BEGIN
            SET @ErrMsg = 'Invoice not found or already cancelled';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Validation 2: Check if return invoice/credit note exists
        SELECT @ReturnInvoiceExists = COUNT(*) 
        FROM [dbo].[tblSalesCrNote_dtl] 
        WHERE FKInvoiceID = @PkId 
          AND FKInvoiceSrID = @FkSeriesId;
        
        IF @ReturnInvoiceExists > 0
        BEGIN
            SET @ErrMsg = 'Cannot delete/cancel invoice - Return/Credit note already generated';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
		IF @ErrMsg = ''
        BEGIN
            -- Step 1: Reverse stock movements
            UPDATE stk
            SET stk.OutStock = stk.OutStock - (dtl.Qty + ISNULL(dtl.FreeQty, 0))
			FROM tblProdStock_Dtl stk
            INNER JOIN tblSalesInvoice_dtl dtl 
                ON stk.FKProductId = dtl.FkProductId 
               AND stk.FKLotID = dtl.FKLotID 
               AND stk.FKLocationId = dtl.FKLocationID
            WHERE dtl.FkId = @PkId 
              AND dtl.FkSeriesId = @FkSeriesId;
            
			   

            -- Step 2: Update barcode tracking
            UPDATE tblProdQTYBarcode 
            SET TranOutId = NULL,
                TranOutSeriesId = NULL,
                TranOutSrNo = NULL
            WHERE TranOutId = @PkId 
              AND TranOutSeriesId = @FkSeriesId;
            
            -- Step 3: Clean up voucher entries
            DELETE FROM tblVoucher_dtl 
            WHERE FKVoucherID = @PkId 
              AND FKSeriesId = @FkSeriesId;
            
            DELETE FROM tblVoucher_trn 
            WHERE PKVoucherID = @PkId 
              AND FKSeriesId = @FkSeriesId;
            
            -- Step 4: Handle based on flag
            IF @Flag = 'D' -- Delete
            BEGIN 
               
                
                -- Delete detail records first (foreign key constraint)
                DELETE FROM tblSalesInvoice_dtl 
                WHERE FkId = @PkId 
                  AND FKSeriesId = @FkSeriesId;
                
                -- Delete master record
                DELETE FROM tblSalesInvoice_trn 
                WHERE PkId = @PkId 
                  AND FKSeriesId = @FkSeriesId;

            END
            ELSE IF @Flag = 'C' -- Cancel
            BEGIN
                -- Update invoice status to cancelled
                UPDATE tblSalesInvoice_trn 
                SET TrnStatus = 'C', 
                  --  EntryNo = 0,
                    FKUserId = @FKUserId,
                    ModifiedDate = GETDATE()
                WHERE PkId = @PkId 
                  AND FKSeriesId = @FkSeriesId;
                                
            END
            ELSE
            BEGIN
                SET @ErrMsg = 'Invalid Action';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;        
        SET @ErrMsg = 'Error: ' + ERROR_MESSAGE();        
		Insert Into tblError_Log Select 0,ERROR_Message(),GetDate(),GetDate(),0,@FKUserId 
        
        
    END CATCH
    
END