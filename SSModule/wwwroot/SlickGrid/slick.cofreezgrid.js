var coGridFreeze = function(grd){
        
        this.outGridFreezName= grd+"_freez";
        this.outGridMainName = grd+"_main";
        this.outGridName = grd;
        this.defaultHeight;
        
        this.isTotalRow = false;
        this.thisObjFreez;
        
        this.dataView = new Slick.Data.DataView();
        
        this.columnFilters = {};
        
        if($(this.outGridFreezName).length<=0)
        {
            $(this.outGridName).attr("id",this.outGridMainName.substring(1))
            $(this.outGridMainName).append("<div class='grdfreez' id='"+ this.outGridFreezName.substring(1) +"'></div><div  class='grdunfreez' id='"+ this.outGridName.substring(1) +"'></div>")
        }
        
        this.coGridUnFreez = new coGrid(this.outGridName, this.dataView, this.columnFilters)
        this.coGridFreez = new coGrid(this.outGridFreezName, this.dataView, this.columnFilters)
        
        
        this.setFreezUnFreezWidth = function(fW,ufW,tW)
            {
                $(this.outGridMainName).width(tW)
                this.totalwidthfreez = fW * tW/ 100
                $(this.outGridFreezName).width(this.totalwidthfreez)
                
                this.totalwidth = ufW * tW/ 100
                $(this.outGridName).width(this.totalwidth)
                
                
            };
            
        this.totalwidth;
        this.totalwidthfreez;
        this.setColumnWidthPer = function(freezwidth, unfreezwidth)
            {
                 this.coGridFreez.setColumnWidthPer(freezwidth,this.totalwidthfreez)
                 this.coGridUnFreez.setColumnWidthPer(unfreezwidth,this.totalwidth)
            };
        this.setColumnHeading = function(freezHead, unfreezHead) 
            {
                this.coGridFreez.setColumnHeading(freezHead)
                this.coGridUnFreez.setColumnHeading(unfreezHead)
            }
        this.setColumnFields = function(freezHead, unfreezHead) 
            {
                this.coGridFreez.setColumnFields(freezHead)
                this.coGridUnFreez.setColumnFields(unfreezHead)
            }
        this.setSearchableColumns = function(freezHead, unfreezHead) 
            {
                this.coGridFreez.setSearchableColumns(freezHead)
                this.coGridUnFreez.setSearchableColumns(unfreezHead)
            }
        this.setCtrlType = function(freezHead, unfreezHead) 
            {
                this.coGridFreez.setCtrlType(freezHead)
                this.coGridUnFreez.setCtrlType(unfreezHead)
            }
        this.setAlign = function(freezHead, unfreezHead) 
            {
                this.coGridFreez.setAlign(freezHead)
                this.coGridUnFreez.setAlign(unfreezHead)
            }
        this.setColumnAttr = function(attrName,freezHead, unfreezHead) 
            {
                this.coGridFreez.setColumnAttr(attrName,freezHead)
                this.coGridUnFreez.setColumnAttr(attrName,unfreezHead)
            }
        this.setSortableColumns = function(freezHead, unfreezHead) 
            {
                this.coGridFreez.setSortableColumns(freezHead)
                this.coGridUnFreez.setSortableColumns(unfreezHead)
            }
        this.setIdProperty = function(colId) 
            {
                this.coGridFreez.setIdProperty(colId)
                this.coGridUnFreez.setIdProperty(colId)
            }
        this.populateDataFromJson = function (obj) {
            
                return this.coGridFreez.populateDataFromJson(obj)
            
            }
        this.setTotalOn = function(_totalGridName, colNames, _totalGridNameUN, colNamesUN, callerFunction){
            
                this.coGridFreez.setTotalOn(_totalGridName, colNames, callerFunction);
                this.coGridUnFreez.setTotalOn(_totalGridNameUN, colNamesUN, callerFunction);
                this.isTotalRow = true;
            
            }
        this.setOptionArray = function(objListFreez, objListUnFreez){
            
                if(objListFreez!=undefined)
                    if(objListFreez!=null)
                        if(objListFreez.length>0)
                        {
                            for(kk=0;kk<objListFreez.length;kk++)
                            {
                                var o = objListFreez[kk]
                                this.coGridFreez.setOptionArray(o.colIdx, o.dbList, o.fieldval, o.fromList, o.txtField, o.valField)
                            }
                        }
                        
                        
                if(objListUnFreez!=undefined)
                    if(objListUnFreez!=null)
                        if(objListUnFreez.length>0)
                        {
                            for(kk=0;kk<objListUnFreez.length;kk++)
                            {
                                var o = objListUnFreez[kk]
                                this.coGridUnFreez.setOptionArray(o.colIdx, o.dbList, o.fieldval, o.fromList, o.txtField, o.valField)
                            }
                        }
            
            }
        this.setSave = function(objSet){
            
                this.coGridFreez.setSave(objSet)
                this.coGridUnFreez.setSave(objSet)
            
            }
        this.setSearchType = function(searchTypes) {
            
                this.coGridUnFreez.setSearchType(searchTypes)
            
            }
        this.getData = function(){
            
                return this.dataView.getItems();
            }
            
        this.bind = function (dt)
            {
                this.coGridFreez.defaultHeight = this.defaultHeight;
                this.coGridUnFreez.defaultHeight = this.defaultHeight;
                
                $(this.outGridMainName).height(this.defaultHeight);
                $(this.outGridFreezName).height(this.defaultHeight);
                $(this.outGridName).height(this.defaultHeight);
                
                this.coGridFreez.bind(dt)
                this.coGridUnFreez.bind(dt)
                
                this.coGridFreez.thisObjFreez=this;
                this.coGridUnFreez.thisObjFreez=this;
                
                $(this.outGridName).find(".slick-viewport").attr("outGridFreezName",this.outGridFreezName);
                
                $(this.outGridName).find(".slick-viewport").on("scroll",function(){
                
                    $($(this).attr("outGridFreezName")).find(".slick-viewport").scrollTop( $(this).scrollTop());
                
                })
                
                
                this.coGridUnFreez.on({
                    
                        click : function(e,arg,item) {
                            
                                //alert($('div:eq(0)',$('.slick-row:eq('+ arg.row +')', arg.grid.thisObj.thisObjFreez.coGridFreez.outGridName )).length);
                                //$('div:eq(0)',$('.slick-row:eq('+ arg.row +')', arg.grid.thisObj.thisObjFreez.coGridFreez.outGridName )).trigger("click")
                                
                                //var st = $('.slick-row:eq('+ arg.row +')', arg.grid.thisObj.outGridName).attr("style")
                                //st = "[style ='" + st + "']"
                                
                                //$('div:eq(0)',$(st)[0]).trigger("click")
                                
                            
                            
                            }
                    
                    })
                
                if(!this.isTotalRow)
				{
					$(this.outGridFreezName).height($(this.outGridFreezName).height()-16)
					$(this.outGridFreezName).css("top","-16px")
				}
				else 
				{
						 
					 
					$(this.outGridFreezName).css("top","0px")
				}
					
                
                
                
            }
			
        this.SlickSetHeadingArr = function(freezHead, unfreezHead) 
            {
				this.coGridFreez.SlickSetHeadingArr(freezHead)
                this.coGridUnFreez.SlickSetHeadingArr(unfreezHead)
				
                if(!this.isTotalRow)
				{
					$(this.outGridFreezName).height($(this.outGridFreezName).height()-16)
					$(this.outGridFreezName).css("top","-16px")
				}
				else 
				{
						 
					 
					$(this.outGridFreezName).css("top","0px")
				}
            }
        
        
        
            
    } 
