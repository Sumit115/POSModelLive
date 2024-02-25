// JScript File
function SlickBeforeenterDt(txtb)
{
    try
    {
        var dtbox=txtb;
        if(event.keyCode==9) return;
        if(event.keyCode==13) return;
        if(event.keyCode==8) return;
        
        var kc = event.keyCode;
        if(kc== 37 || kc== 38 || kc== 39 || kc== 40)
            return;
        
        dtbox.value=SlickRtrimDt(dtbox.value);
        if(dtbox.value.length==10)
            dtbox.select();
        prevlu=dtbox.value;
        if(dtbox.value.length==1 && (event.keyCode==111 || event.keyCode==109 || event.keyCode==191 || event.keyCode==189))
        {
            dtbox.value= "0" + dtbox.value;
        }
        if(dtbox.value.length==2 && event.keyCode!=8)
        {
            dtbox.value=dtbox.value + "/";
        }
        if(dtbox.value.length==4 && (event.keyCode==111 || event.keyCode==109 || event.keyCode==191 || event.keyCode==189))
        {   
            var dt=dtbox.value.split("/")
            dtbox.value= dt[0] +  "/0" + dt[1];
        }
        if(dtbox.value.length==5 && event.keyCode!=8)
        {
            dtbox.value=dtbox.value + "/";
        }
        if(dtbox.value.length==0 && event.keyCode!=8 && ((event.keyCode>=52  && event.keyCode<=57) || (event.keyCode>=100  && event.keyCode<=105) ))
        {
            dtbox.value=dtbox.value + "0";
        }
        if(dtbox.value.length==3 && event.keyCode!=8 && ((event.keyCode>=52  && event.keyCode<=57) || (event.keyCode>=100  && event.keyCode<=105) ))
        {
            dtbox.value=dtbox.value + "0";
        }
        if(dtbox.value.length==6 && event.keyCode!=8 && event.keyCode==25)
        {
            dtbox.value=dtbox.value + "20";
        }
        invl="F"
        var len=dtbox.value.length;
        var ky=event.keyCode;
        if(len==0 || len==1 || len==3 || len==4 || len==6 || len==7 || len==8 || len==9 || len==10)
        {
            if((ky<48 || ky>57) && (ky <96 || ky>105) && ky!=8  && ky!=17  && ky!=16 )
            {
                event.keyCode=0;
                dtbox.value=prevlu;
                invl="T"
            }
        }
    }
    catch (e)
    {
        alert(e);
        dtbox.value='';
    }
}
var invl = "F";
function SlickEnterdataDt(txtb)
{
    try
    {
        var kc = event.keyCode;
        if(kc== 37 || kc== 38 || kc== 39 || kc== 40)
            return;
        if(event.keyCode==9) return;
        if(event.keyCode==13) return;
        if(event.keyCode==8) return;
        
        var dtbox=txtb;
        if (invl=="T")
        {
            dtbox.value=prevlu;
        }
        if(dtbox.value.length==2 && event.keyCode!=8)
        {
            if(parseInt(dtbox.value,10)>31)
            {
                dtbox.value=dtbox.value.substring(0,1); // + "/";
            }
        }
        if(dtbox.value.length==2 && event.keyCode!=8)
        {
            dtbox.value=dtbox.value + "/";
        }
        //alert(event.keyCode)
        
        if(dtbox.value.length==1 && event.keyCode==111)
        {
            dtbox.value="0"+ dtbox.value + "/"

        }
        
        if(dtbox.value.length==4 && event.keyCode==111)
        {
            
            var dt=dtbox.value.split("/");
            
            if(parseInt(dt[1],10)==1)
            {
                
                dtbox.value=dt[0] + "/0" + dt[1] + "/";
            }
        }
     
        if(dtbox.value.length==4 && event.keyCode!=8)
        {
            //alert("A")
            var dt=dtbox.value.split("/");
            if(parseInt(dt[1],10)>=2)
            {
            
                dtbox.value=dt[0] + "/0" + dt[1];
            }
        }
        
       
        if(dtbox.value.length==5 && event.keyCode!=8)
        {
            var dt=dtbox.value.split("/");
            if(parseInt(dt[1],10)>12)
            {
                dtbox.value=dt[0] + "/1";
            }
        }   
        if(dtbox.value.length==5 && event.keyCode!=8)
        {
            dtbox.value=dtbox.value + "/"
        }   
        if(dtbox.value.length==7 )
        {   
            var dt=dtbox.value.split("/")
            if(dt[2]=="0")
            {
                dtbox.value= dt[0] +  "/" + dt[1] +  "/20" + dt[2];
            }
            if(dt[2]=="9" || dt[2]=="8"  || dt[2]=="7"  || dt[2]=="6"  || dt[2]=="5"  || dt[2]=="4")
            {
                dtbox.value= dt[0] +  "/" + dt[1] +  "/19" + dt[2];
            }
            
        }
        if(dtbox.value.length==8 )
        {   
            var dt=dtbox.value.split("/")
            if(dt[2].substring(0,1)=="1" && dt[2].substring(1,2)!="9" && dt[2].substring(1,2)!="8")
            {
                dtbox.value= dt[0] +  "/" + dt[1] +  "/20" + dt[2];
            }
        }
    }
    catch (e)
    {
        dtbox.value='';
    }
}
function SlickOnfocusDt(txtbx)
{
    try
    {
        var txtdtbx= txtbx;
        if(txtdtbx.value !="")
        {
            txtdtbx.value=SlickFormatnumDt(txtdtbx.value);
            txtdtbx.focus();
            txtdtbx.select();
        }
    }
    catch (e)
    {
        alert(e);
        txtdtbx.value='';
    }
   // onenter(txtbx);   
}
function SlickOnleaveDt(txtbx)
{
    try
    {
//        alert("C");
        var txtdtbx= txtbx;
        if(txtdtbx.value!="")
        {
            if(txtdtbx.value.length<8) 
            {
               txtdtbx.value='';
               return false;
            }
            txtdtbx.value=SlickFormatstrDt(txtdtbx.value);
            if(txtdtbx.value=="")
                return false;
            return true;
        } 
        return true;
    }
    catch (e)
    {        
        txtdtbx.value=SlickFormatstrDt(txtdtbx.value);
            if(txtdtbx.value=="")
                return false;
        return true;
    }   
    return true;
}
function SlickFormatnumDt(vlu)
{
    dt=vlu.split("-");
    if(dt.length!=3)
    {
        dt=vlu.split("/");
    }
    var dy=dt[0];
    var mon=dt[1];
    var yr=dt[2];
    var dt=dy + "/" + SlickGetmonthstrDt(mon) + "/" + yr;;
    if(dt.length!=10 && dt.length!=11)
        return ''
    if(SlickIsValidDt(dt)==false)
    {
//        alert('Invaliddt:' + dt); 
        return ''
    }
   return dt
}
function SlickFormatstrDt(vlu)
{
    var dtt=vlu;
    dt=vlu.split("-");
    if(!(vlu.length==11 && isNaN(parseInt(dt[1],10))))
    {
        if(dt.length!=3)
        {
            dt=vlu.split("/");
        }
        var dy=dt[0];
        var mon=dt[1];
        var yr=dt[2];
        if(yr.length==2)
        {
            yr="20"+yr;
        }
        if(yr.length==1)
        {
           yr="200"+yr;
        }
        var dttt=new Date();
        if(parseInt(yr,10)>2100)
            yr=dttt.getFullYear()
        dtt=dy + "-" + SlickGetmonthnumDt(mon) + "-" + yr;
    }
    
    if(dtt.length!=10 && dtt.length!=11)
        return ''
    if(SlickIsValidDt(dtt)==false)
        return ''
    return dtt
    
}


    function SlickValidateTextFormat(txt,frmt)
    {
        if($.trim(txt).length!=$.trim(frmt).length)
            return false;
        
        //[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}
        var regx = "";
        
        var k=0; 
        for(k=0; k < frmt.length; k++) 
        { 
            var ch = frmt.substring(k,k+1); 
            if(ch=="X")
            {
                regx = regx + "[a-zA-Z]{1}";
            }
            else if(ch=="9")
            {
                regx = regx + "[0-9]{1}";
            }
            else
            {
                regx = regx + "[a-zA-Z0-9]{1}";
            }
        }
        
        var Rjxp = new RegExp(regx)
        
        var mtch = txt.match(Rjxp)
        if(mtch==null)
            return false;
        
        return true;
    }
    function SlickSetTextFormat(txt,frmt)
    {
        var kc=event.keyCode;
        ///alert(kc);
        if(kc==8 || kc==13 || kc==9 || kc==37 || kc==38 || kc==39 || kc==40 || kc==46 || kc==13)
            return;
        var tx=txt;
        var ln=tx.value.length;
        var lp=ln+1;
        //alert(ln);
        window.status="";
        if(frmt.substring(ln,lp)=="X")
        {
            
            if(kc<65 || kc>90)
            {
                window.status="Enter Character";
                return false;
            }
                
        }
        else if(frmt.substring(ln,lp)=="9")
        {
            if((kc<96 || kc>105) && (kc<48 || kc>57))
            {
                window.status="Enter Number";
                return false;
            }
        
        }
        
        

    }


function SlickGetDateInStr(value)
{
    //  if value == "2017-08-04T00:00:00"
    //  then it returns "04-Aug-2017"
    
    if(value==null || value=="")
            return "";
        if(value.length==11)
                return value;
            
        var dt = new Date(value);
        
        if(isNaN(dt))
			dt = new Date(value.replace(/-/g,' '));
			
			
        var d ;
        var m ;
        var y ;
        
        if(isNaN(dt))
        {
            var dvvv = value.substring(0,10).split('-')
            var d = dvvv[2] 
            var m = dvvv[1]
            var y = dvvv[0]
        }
        else
        {
            var d = dt.getDate(); 
            var m = dt.getMonth()+1 
            var y = dt.getFullYear()
        }
        
        if(typeof(d)=="string" && d.length<1)
            d = "0" + d;
        else if(typeof(d)=="number" && d<10)
            d = "0" + d;
        
        
        var nd = SlickFormatstrDt(d + "/" + m + "/" + y)
         
        return nd;
    
    
}


function SlickGetmonthnumDt(vl)
{
    switch(vl)
    {
        case "01": { return "Jan"; break;}
        case "02":{ return "Feb";break;}
        case "03":{ return "Mar";break;}
        case "04":{ return "Apr";break;}
        case "05":{ return "May";break;}
        case "06":{ return "Jun";break;}
        case "07":{ return "Jul";break;}
        case "08":{ return "Aug";break;}
        case "09":{ return "Sep";break;}
        case "1": { return "Jan"; break;}
        case "2":{ return "Feb";break;}
        case "3":{ return "Mar";break;}
        case "4":{ return "Apr";break;}
        case "5":{ return "May";break;}
        case "6":{ return "Jun";break;}
        case "7":{ return "Jul";break;}
        case "8":{ return "Aug";break;}
        case "9":{ return "Sep";break;}
        case "10":{ return "Oct";break;}
        case "11":{ return "Nov";break;}
        case "12":{ return "Dec";break;}
    }
}
function SlickGetmonthstrDt(vl)
{
    switch(vl)
    {
        case "Jan":{ return "01";break;}
        case "Feb":{ return "02";break;}
        case "Mar":{ return "03";break;}
        case "Apr":{ return "04";break;}
        case "May":{ return "05";break;}
        case "Jun":{ return "06";break;}
        case "Jul":{ return "07";break;}
        case "Aug":{ return "08";break;}
        case "Sep":{ return "09";break;}
        case "Oct":{ return "10";break;}
        case "Nov":{ return "11";break;}
        case "Dec":{ return "12";break;}
         default: {return vl;break;}
    }
}

function SlickIsValidDt(dt)
{
    try
    {
    var myDt=Date.parse(dt)
        if(myDt=='NaN')
        {
            return false;
        }
        else
        {
            var bol;
            if(dt.length==11)
                bol=SlickValidationDt(dt,1)
            else
                bol=SlickValidationDt(dt,0)
                //alert(bol);
            if(bol==true)
                return true;
            else
            {   
                
                return false;
            }
        }
        
    }
    catch (e)
    {
        return false;
    }
    
} 

function SlickValidationDt(dt,typ)
{
    //alert("Validation")
    var Message="Invalid Date";
    
    if(typ==0)
    {
     
    var dts=dt.split('/')
    var m=dts[1];
    }
    else
    {
     
     var dts=dt.split('-')   
    var m=dts[1];
    m=SlickGetmonthstrDt(m);
        
  
    }
    var d=dts[0];
    var y=dts[2];
    //alert(dt)
    //alert("A4")
    
    if(parseInt(d,10)>31 ||parseInt(m,10)>12 ||parseInt(d,10)==0||parseInt(m,10)==0||parseInt(y,10)==0)  
		{
		     //alert("A5")
			alert(Message);
			//control.select();
			return false;
		}
		else
		{
			 
			if( (parseInt(m,10)=="4"||parseInt(m,10)=="6"||parseInt(m,10)=="9"||parseInt(m,10)=="11") && parseInt(d,10)>30)
			{
			     //alert("A6")
				alert(Message);
				//control.select();
				return false;
			}
			else
			{	  
				if(parseInt(m,10)==2 && parseInt(d,10)>29)
				{
                 //alert("A7")
				alert("Invalid date ");	
				//control.select();
				return false;			
				}
				else
				{
                    //alert("A98")
					if(parseInt(y,10)%4!=0 && parseInt(m,10)==2 && parseInt(d,10)==29)
					{
					    // alert("A8")
					alert(Message);
					//control.select();
					return false;
					}
					else //if ok 
					{ //alert("A41")
					    //alert(y)
						if(y.length==4)
						{
						
						//alert("Success")
						return true;
						}									
							
					} //end else	
					
				} //else after 29 check					

			} //elsa after no of days 30 check
			
		} //else after no of days in month check
        //alert("Success0")
    return true;
} 
function SlickRtrimDt(str, chars) 
{
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
    
}
 
function SlickIsDecimalInput(Th)
{
    
    Th.maxLength=18;
   
    var str= Th.value.toString();
   
    // if string lenght is zero and user want to enter + - sign
    if ((str.length==0) &&  (event.keyCode==45  || event.keyCode==43  )) //(event.keyCode==107  || event.keyCode==187 ||event.keyCode==109  || event.keyCode==189 ))
    {
        return false;
    }
    // if string lenght is > zero and Stop user to enter + - sign
    else if ((str.length>0) && (event.keyCode==45  || event.keyCode==43  ))  //(event.keyCode==107  || event.keyCode==187 ||event.keyCode==109  || event.keyCode==189 ))
    {
        return false;
    }
    // if user already entered . sign than not accept .
    else if ((str.indexOf('.')>=0) && (event.keyCode==46))   //(event.keyCode==190  || event.keyCode==110 ))
    {
        return false;
    }
    // if user not entered . sign than accept .
    else if ((str.indexOf('.')<0) && (event.keyCode==46))   //(event.keyCode==190  || event.keyCode==110 ))
    {
        return true;
    }

//--------------------- checking event keycode ------------------------------------
    if((event.keyCode>=48 && event.keyCode<=57)  )
	{	   
		   
		   return true;
	}
	// allow delete and backspace
	else if ((str.length>0) && (event.keyCode==8  || event.keyCode==46  ))
    {      
        return true;
    }
	else
	{
	    return false;
	}
}

function SlickFormatuptodecimals(amount,DecimalPlace)
{
    var retval = amount;
    if(amount != "")
        retval = parseFloat(amount).toFixed(DecimalPlace);
    return retval;
}

function SlickGetOptionList(optionsArray, column)
{
    var valField = "val";
    var txtField = "txt";
    var styleField = "";
    
    if(typeof(optionsArray) == "function")
        optionsArray = optionsArray();
    
    if(column!=undefined && column!=null)
    {
        if(column.valField!=undefined && column.valField!=null && column.valField!="")
            valField = column.valField;
            
        if(column.txtField!=undefined && column.txtField!=null && column.txtField!="")
            txtField = column.txtField;
            
    }

    var cmblen = optionsArray.length;
    var optlist="";
    for(var i=0;i<cmblen;i++)
    {
		var cStyle = " "
		if(typeof(column.styleField)!="undefined")
		{
			styleField = column.styleField;
			if(typeof(optionsArray[i][styleField])!="undefined" && optionsArray[i][styleField]!="")
				cStyle = " style='"+ optionsArray[i][styleField] +"' "
		}

       optlist= optlist+"<option "+ cStyle +" value='" + optionsArray[i][valField] + "' title='" + optionsArray[i][txtField] + "'>" + optionsArray[i][txtField] + "</option>" ;
	}
    
    return optlist;  
    
}
var SlickajaxReturnValue = "";
function SlickCallGridAjaxUpdate(Action,ajxpag,RowId,fieldName,fieldValue)
{
    var  qStr = "SlickAction="+Action+"&RowId=" +RowId + "&fieldName=" + fieldName + "&fieldValue=" + fieldValue;
    console.log(qStr);
    $.ajax({dataType: "json",data:qStr, url:ajxpag, type:"POST"}).done( function(res) {
        SlickAfterGridAjax(res);
    });
    return SlickajaxReturnValue;
}
function SlickCallGridAjaxAdd(Action,ajxpag,RowIndex,fieldName,fieldValue)
{
    var qStr = "SlickAction="+Action+"&RowIndex=" + RowIndex + "&fieldName=" +fieldName+ "&fieldValue=" + fieldValue;
    console.log(qStr);
    $.ajax({dataType: "json",async:false, data:qStr, url:ajxpag, type:"POST"}).done( function(res) {
        SlickAfterGridAjax(res);
    });
    return SlickajaxReturnValue;
}
function SlickCallGridAjaxDelete(Action,ajxpag,RowId,RowIndex)
{
    var qStr = "SlickAction="+Action+"&RowId=" +RowId+"&RowIndex=" +RowIndex;
    console.log(qStr);
    $.ajax({dataType: "json",data:qStr, url:ajxpag, type:"POST"}).done( function(res) {
        SlickAfterGridAjax(res);
    });
    return SlickajaxReturnValue;
}
function SlickAfterGridAjax(r)
{
     if(r.RowId != null)
        SlickajaxReturnValue=r.RowId;
}
function SlickFormatAmount(txt)
{
	var amt="";
	var txtstr=new String();
	txtstr=txt.toString();
	var txtPreFix='';
	try
	{
		if(txtstr.length>0 && txtstr!=" ")
		{
		    if(txtstr.substr(0,1) == '-')
		    {
		        txtPreFix = txtstr.substr(0,1);
		        txtstr = txtstr.substr(1);
		    }
			txtstr=txtstr.replace(',','');
			amt=txtstr.split('.');
			amtLen=amt[0].length;
			var amtTxt=new String();
			amtTxt=amt[0];
			if(amtLen>3)
			{
				txtHundred=amtTxt.slice(amtLen-3);
				beforeHund=amtTxt.slice(0,amtLen-3);
				txtstr=beforeHund+","+txtHundred;
				for(cindx=beforeHund.length-1;cindx!=0;cindx--)
				{
					if(beforeHund.length%2==0)
					{
						if(cindx%2==0)
							txtstr=txtstr.substr(0,cindx)+","+txtstr.substr(cindx,txtstr.length);
					}
					else
					{
						if(cindx%2!=0)
							txtstr=txtstr.substr(0,cindx)+","+txtstr.substr(cindx,txtstr.length);
					}
				}
			}
			else
				txtstr=amtTxt;
				
			if(amt.length>1)
				txtstr=txtPreFix+txtstr + "." + amt[1];
			
			return txtstr;
		}
	}
	catch(e){}
	return txt;
}
function SlickRemoveComma(txt)
{
    var retval = String(txt);
    if (retval.indexOf(",") > -1)
        retval=retval.replace(new RegExp(',','g'),'')
    return retval; 
}
function SlickSetHeading(GrDName)
  {
        
       $('#'+GrDName+' [uh=1]').remove();
      
       if($('#'+GrDName).attr("gha") != undefined && $('#'+GrDName).attr("gha") != null)
       {
            var gridAddedHight=0;
            gridAddedHight=$('#'+GrDName).attr("gha");
            $('#'+GrDName).css('height', $('#'+GrDName).height()- gridAddedHight);
            $('#'+GrDName).removeAttr("gha");
       }
       H_hight =$('#'+GrDName+' .slick-header').height();
       var divwidth= new Array();
       var i=0;
       var Tot_hight=0;
       var HeadingDivStyle =$('#'+GrDName+' .slick-header-columns').attr('style');
       $('#'+GrDName+' .slick-header-columns div').each(function(){
            var id = $(this).attr('id');
            if(id != undefined)
            {
                divwidth[i] = $(this).css('width').replace('px','');
                i=i+1;
            }
        });
        for(j = 1; j < arguments.length; j=j+1)
        {
            Tot_hight =Tot_hight + H_hight;
            var HArray = arguments[j].split('~');   
            var H='<div class="slick-header-columns" style="'+HeadingDivStyle+'" unselectable="on" uh="1">';
            var MergeColLen=0;
            for(k = 0; k < HArray.length; k=k+1)
            {
                var n = k+1;
                if(HArray[k] == '') 
                    H= H+'<div title="" class="ui-state-default slick-header-column" id="H_'+String(k)+'" style="width: '+String(divwidth[k])+'px;"><span class="slick-column-name">'+HArray[k]+'</span></div>'
                else
                {
                    var same = false;
                    if(k < HArray.length - 1)
                    {
                       if(HArray[k] == HArray[n])
                            same=true;
                    }
                    MergeColLen = MergeColLen +eval(divwidth[k]); 
                    if(!same)
                    {
                        H= H+'<div title="" class="ui-state-default slick-header-column" id="H_'+String(k)+'" style="width: '+String(MergeColLen)+'px;"><span class="slick-column-name">'+HArray[k]+'</span></div>'
                        MergeColLen =0;
                    }
                }    
            }
            H= H+'</div>';
            $('#'+GrDName+' .slick-header').prepend(H);
        }
        $('#'+GrDName).attr("gha", Tot_hight);
        $('#'+GrDName).css('height', $('#'+GrDName).height()+Tot_hight);
    }


function bindSingleGrid_With_SlickGridObject(cg, dc)
{
    
    cg.setColumnHeading(dc.ColumnHeading);
    
    cg.setColumnWidth(dc.ColumnWidth);
    
    
    
    var mapDataArr = [];
    
    if(typeof(dc.opt)!="undefined")
    {
        for(op=0; op<dc.opt.length;op++)
        {
            var oo = dc.opt[op];
            dc.ColumnFields = dc.ColumnFields.replace(oo.field,oo.field + "_text")
            if(oo.tt==undefined)
            {
                oo.tt = "txt";
                oo.vv = "val";
            }
            cg.setOptionArray(oo.colidx,oo.data,oo.field, true, oo.tt, oo.vv)
            
            var obM = {
                        data:oo.data,
                        srcValueColumn : oo.field,
                        destValueColumn : oo.vv,
                        destTextColumn : oo.tt,
                        textColumn : oo.field + "_text"
                    };
            mapDataArr.push(obM)
            
        }
        
    }
    
    cg.setColumnFields(dc.ColumnFields);
    
    cg.applyAddNewRow();
    
     
    
    cg.setIdProperty("RowId")
    cg.setCtrlType(dc.CtrlType);
    cg.setAlign(dc.Align);
    
    
    
    
    
    cg.defaultHeight = defaultHeight;
    
//    if(typeof(dc.opt)!="undefined")
//    {
//        for(op=0; op<dc.opt.length;op++)
//        {
//            var oo = dc.opt[op];
//            cg.setOptionArray(oo.colidx,oo.data,oo.field)
//        }
//        
//    }
    
    if(typeof(dc.att)!="undefined")
    {
        for(op=0; op<dc.att.length;op++)
        {
            var oo = dc.att[op];
            cg.setColumnAttr(oo.f,oo.v)
        }
    }
    if(typeof(dc.totalCols)!="undefined")
    {
        cg.setTotalOn(dc.totalCols)
    }
    if(typeof(dc.minRows)!="undefined")
    {
        cg.SetMinRow(dc.minRows)
    }
    
    
    
    var obdt = cg.populateDataFromJson(
                {
                    srcData : dc.data , 
                    mapData : mapDataArr
                }
            )
    
    cg.bind(obdt);
    if(dc.SetHeadingArr2!="")
        cg.SlickSetHeadingArr(dc.SetHeadingArr1 ,dc.SetHeadingArr2)
    else if(dc.SetHeadingArr1!="")
        cg.SlickSetHeadingArr(dc.SetHeadingArr1)
    
    cg.setSave({
    
        url:dc.sSaveUrl,
        fixPara : dc.fixPara  ,
        addDeleteMenu : true
         
    })
    
}

///                                                 time key down


function SlicksetTimekd(txt)
{
    var t = txt;
    var v = t.value;
    var l = v.length;
    var kd= parseFloat(event.keyCode);
    
//    if(kd>36 && kd<41) // UP and Down Keys
//        return
    if(kd==39 || kd==8 || kd==9 || kd==46) // back
        return
    if(l==1)
    {   if(v=="2")
        if((kd>51 || kd<48)&&(kd<96 || kd>99))
        {
            return false;
        }
    }
    
    if(l==3)
    {   
        if((kd>53 || kd<48)&&(kd<96 || kd>101))
        {
            return false;
        }
    }
    if(l==6)
    {   
        if((kd>53 || kd<48)&&(kd<96 || kd>101))
        {
            return false;
        }
    }
     if(l==8)
    {   
        if(kd==39 || kd==8 || kd==9 || kd==46 || kd==13) // back
        {   return;
            
        }
        t.value ='';
        return true;
     }
     if(l == 2)
     {
        t.value = v + ":";
        setTimekd(txt)
     }
     if(l == 5)
     {
        t.value = v + ":";
        setTimekd(txt)
     }
    
 }
function SlickvalidateTime(txt)
{
    var t=txt;
    if(t.value=="") return;
    var dt=t.value+":00:00"
    var tt=t.split(':');
    
    
    
}
function SlicksetTimeblr(txt)
{
    var t=txt;
    if(t.value=="") return;
    var sp = t.value.split(':')
    var tm = "";
    if(sp.length>0)
        if(sp[0].length==1)
            tm= "0" + sp[0]
        else 
            tm= sp[0]

    if(sp.length>1)
        if(sp[1].length==0)
            tm= tm + ":00" ;
        else if(sp[1].length==1)
            tm= tm + ":" + "0" + sp[1]
        else 
            tm= tm + ":" + sp[1]
    else 
         tm=  tm + ":00" 
    if(sp.length>2)
        if(sp[2].length==0)
            tm= tm + ":00" ;
        else if(sp[2].length==1)
            tm= tm + ":" +  "0" + sp[2]
        else 
            tm=  tm + ":" + sp[2]
    else 
         tm=  tm + ":00" 
    t.value=tm;
    
    
    
}
function SlicksetTimeku(txt)
{
    var t = txt;
    var v = t.value;
    var l = v.length;
    var kd=event.keyCode;
    if(l==1)
    {
        if(v!="0" && v!="1" && v!="2")
            {t.value='0'+v;
            setTimeku(txt)
            }
    }
    //alert(event.keyCode)
//    if(l==2)
//    {
//        t.value=t.value+':';
//    }
    if(l==2)
    {
        if(kd==8)
           {t.value=t.value.substring(0,1);return;}
           
        t.value=t.value+':';
    }
    //if((l==2||l==3) && event.keyCode==8)
    //    t.value='';
    
    if(l==5)
    {
        if(kd==8)
           {t.value=t.value.substring(0,4);return;}
           
        t.value=t.value+':';
    }
    
     
}

