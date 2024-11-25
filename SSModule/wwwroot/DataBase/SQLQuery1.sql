IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Demo]') AND type IN (N'P', N'PC'))
BEGIN    
	CREATE PROCEDURE [dbo].[usp_Demo]
	(
		@JsonData NVARCHAR(MAX),
		@OutParam BIGINT OUTPUT,
		@ErrMsg NVARCHAR(MAX) = NULL OUTPUT,
		@SeriesNo BIGINT = 0 OUTPUT
	)
	AS
	BEGIN
		-- Procedure logic starts here
		DECLARE @rc INT;
		DECLARE @PkId BIGINT, @src INT = 0, @FKUserId BIGINT = 0, @PKSeriesId BIGINT = 0, @TranAlias NVARCHAR(MAX);
		DECLARE @DateModified DATETIME2(7), @DateCreated DATETIME2(7), @FDate DATETIME, @TDate DATETIME;
		DECLARE @FkPartyId BIGINT = 0;

		ADDRETRY:
		BEGIN TRY
			BEGIN TRAN
			-- Your procedure logic here...

			COMMIT TRAN;
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN;
			DECLARE @LogID BIGINT;
			SET @ErrMsg = ERROR_MESSAGE();

			IF ERROR_NUMBER() = 1205
			BEGIN
				WAITFOR DELAY '00:00:00.10';
				GOTO ADDRETRY;
			END
			ELSE
			BEGIN
				SET @LogID = @PkId;
				INSERT INTO tblError_Log
				SELECT ISNULL(@LogID, 0), ERROR_MESSAGE(), GETDATE(), GETDATE(), @src, @FKUserId;
			END

			SET @OutParam = 0;
		END CATCH;
	END;
END
else
begin
	Alter PROCEDURE [dbo].[usp_Demo]
	(
		@JsonData NVARCHAR(MAX),
		@OutParam BIGINT OUTPUT,
		@ErrMsg NVARCHAR(MAX) = NULL OUTPUT,
		@SeriesNo BIGINT = 0 OUTPUT
	)
	AS
	BEGIN
		-- Procedure logic starts here
		DECLARE @rc INT;
		DECLARE @PkId BIGINT, @src INT = 0, @FKUserId BIGINT = 0, @PKSeriesId BIGINT = 0, @TranAlias NVARCHAR(MAX);
		DECLARE @DateModified DATETIME2(7), @DateCreated DATETIME2(7), @FDate DATETIME, @TDate DATETIME;
		DECLARE @FkPartyId BIGINT = 0;

		ADDRETRY:
		BEGIN TRY
			BEGIN TRAN
			-- Your procedure logic here...

			COMMIT TRAN;
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN;
			DECLARE @LogID BIGINT;
			SET @ErrMsg = ERROR_MESSAGE();

			IF ERROR_NUMBER() = 1205
			BEGIN
				WAITFOR DELAY '00:00:00.10';
				GOTO ADDRETRY;
			END
			ELSE
			BEGIN
				SET @LogID = @PkId;
				INSERT INTO tblError_Log
				SELECT ISNULL(@LogID, 0), ERROR_MESSAGE(), GETDATE(), GETDATE(), @src, @FKUserId;
			END

			SET @OutParam = 0;
		END CATCH;
	END;
end