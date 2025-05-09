 
Create procedure [dbo].[usp_SalesInvoiceShippingDetailAddUpd]
( 
	@JsonData nvarchar(max)  
)
as 
begin
	Declare @PkId bigint,   @FKUserId bigint = 0,@FKSeriesId bigint=0;

	SELECT @PkId=PkId,@FKSeriesId=FKSeriesId ,@FKUserId=FKUserId 
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint] ,[FKUserId] [bigint] ) as jsondata;				 
	
	 		UPDATE [dbo].[tblSalesInvoice_trn]  
						SET  
						 BiltyNo =Trn.BiltyNo  
					 	,BiltyDate=Trn.BiltyDate 
					 	,TransportName=Trn.TransportName 
					 	,NoOfCases=Trn.NoOfCases 
					 	,FreightType=Trn.FreightType 
					 	,FreightAmt=Trn.FreightAmt 
					 	,ShipingAddress=Trn.ShipingAddress 
					 	,PaymentMode=Trn.PaymentMode 
					 	,FKBankThroughBankID=Trn.FKBankThroughBankID 
					 	,DeliveryDate=Trn.DeliveryDate 
					 	,ShippingMode=Trn.ShippingMode 
					 	,PaymentDays=Trn.PaymentDays  
					 	FROM OPENJSON(@JsonData)
						WITH (  
						[PkId] [bigint] ,
						[FKSeriesId] [bigint] , 
						[BiltyNo] [nvarchar](50) , 
						[BiltyDate] [date] ,
						[TransportName] [nvarchar](50) ,
						[NoOfCases] [bigint] ,
						[FreightType] [nvarchar](20) ,
						[FreightAmt] [decimal](18, 4) ,
						[ShipingAddress] [nvarchar](500) ,
						[PaymentMode] [nchar](1) ,
						[FKBankThroughBankID] [bigint] ,
						[DeliveryDate] [date] ,
						[ShippingMode] [nchar](1) ,
						[PaymentDays] [int] 
						) AS Trn  
						Where tblSalesInvoice_trn.PkId=Trn.PkId  
						And tblSalesInvoice_trn.FKSeriesId=Trn.FKSeriesId 
	
		if Exists (Select * from tblEWayDtl_Lnk Where FKID=@PkId and FkSeriesId=@FKSeriesId)	 	 	
			Begin
					UPDATE [dbo].[tblEWayDtl_Lnk]  
						SET  EWayNo=Trn.EWayNo
							,EWayDate=Trn.EWayDate
							,VehicleNo=Trn.VehicleNo
							,TransDocNo=Trn.TransDocNo
							,TransDocDate=Trn.TransDocDate
							,TransMode=Trn.TransMode
							,SupplyType=Trn.SupplyType
							,Distance=Trn.Distance
							,VehicleType=Trn.VehicleType 
						FROM OPENJSON(@JsonData, '$.EWayDetail')
						WITH (  
						 [EWayNo] [nvarchar](50) ,
						[EWayDate] [date] ,
						[VehicleNo] [nvarchar](20) ,
						[TransDocNo] [nvarchar](20) ,
						[TransDocDate] [date] ,
						[TransMode] [nvarchar](20) ,
						[SupplyType] [nvarchar](50) ,
						[Distance] [decimal](9, 2) ,
						[VehicleType] [nvarchar](50) 
						) AS Trn  
						Where tblEWayDtl_Lnk.FKID=@PkId
						And tblEWayDtl_Lnk.FKSeriesId=@FKSeriesId 
			End
		Else
			Begin
				  Insert into tblEWayDtl_Lnk(FKID,FkSeriesId,EWayNo,EWayDate,VehicleNo,TransDocNo,TransDocDate,TransMode,SupplyType,Distance,VehicleType)
				  SELECT @PkId,@FKSeriesId,EWayNo,EWayDate,VehicleNo,TransDocNo,TransDocDate,TransMode,SupplyType,Distance,VehicleType
					FROM OPENJSON(@JsonData, '$.EWayDetail')
					WITH ( 
						[EWayNo] [nvarchar](50) ,
						[EWayDate] [date] ,
						[VehicleNo] [nvarchar](20) ,
						[TransDocNo] [nvarchar](20) ,
						[TransDocDate] [date] ,
						[TransMode] [nvarchar](20) ,
						[SupplyType] [nvarchar](50) ,
						[Distance] [decimal](9, 2) ,
						[VehicleType] [nvarchar](50) 
					) as jsondata; 
			End
	 			
	print 'A'
	   
	 
end