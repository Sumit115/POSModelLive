/***
 * Contains Advance SlickGrid editors.
 * @module Editors
 * @namespace Slick
 */

(function ($) {
  // register namespace
  $.extend(true, window, {
    "Slick": {
      "Editors": {
        "TextListNew": TextListEditorNew ,
        "ButtonEditor": ButtonEditor, 
        "Time1": Time1Editor,
        "MultiSelect":MultiSelectDropdownEditor
      }
    }
  });
  
  function ButtonEditor(args) {
    var $select;
    var defaultValue;
    var scope = this;

    this.init = function () {
    
        defaultValue = args.item[args.column.field];
        
        if(args.column.fixedText!=undefined && args.column.fixedText!=null && args.column.fixedText!="")
            defaultValue = args.column.fixedText;
        
        if(defaultValue==undefined || defaultValue==null || defaultValue=="")
            defaultValue = "...";
        
        if(typeof(args.column.checkHide)=="function")
        {
            if(args.column.checkHide(args.item))
            {
                $select = $('<span />');
                return '';
            }
        }
        
            
      $select = $('<input  type="button" value="'+ defaultValue +'" >');
      $select.appendTo(args.container);
      $select.focus();
      $select.on("click",function(){
        
            if(args.column.onButtonClick!=undefined && event.type=="click")
                args.column.onButtonClick(args)
        
        })
       
       if(args.column.onButtonClick!=undefined && event.type=="click")
                args.column.onButtonClick(args)
        
      
    };

    this.destroy = function () {
      
    };

    this.focus = function () {
      $select.focus();
    };

    this.loadValue = function (item) {
        defaultValue = item[args.column.field];
        
        if(args.column.fixedText!=undefined && args.column.fixedText!=null && args.column.fixedText!="")
            defaultValue = args.column.fixedText;
        
        if(defaultValue==undefined || defaultValue==null || defaultValue=="")
            defaultValue = "...";
        
        
        $select.val(defaultValue);
    };

    this.serializeValue = function () {
      return $select.prop('button');
    };

    this.applyValue = function (item, state) {
       
    };

    this.isValueChanged = function () {
      return false;
    };

    this.validate = function () {
      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }
  
function Time1Editor(args) {
    var $input;
    var defaultValue;
    var oldValue;
    var scope = this;

    this.init = function () {
      $input = $("<INPUT type=text class='editor-text ac' />");
      
      $input.appendTo(args.container);
      $input.focus().select();
        
      $input.on("keydown", function (e) {
            SlicksetTimekd(this);
      });
      
      $input.on("keyup", function (e) {
            SlicksetTimeku(this);
      });
      
//       $input.on("focus", function (e) {
//            SlickOnfocusDt(this);
//      });
      $input.on("blur", function (e) {
            SlicksetTimeblr(this);
      });
      
      

    };

    this.destroy = function () {
      $input.remove();
    };

    this.focus = function () {
      $input.focus();
    };

    this.loadValue = function (item) {
      defaultValue = item[args.column.field];
      $input.val(defaultValue);
      $input[0].defaultValue = defaultValue;
      $input.select();
    };
    
    this.prevSerializeValue = function () {
      return {input:oldValue};
    };

    this.serializeValue = function () {
      return $input.val();
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state;
    };

    this.isValueChanged = function () {
      oldValue = defaultValue;    
      return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
    };

    this.validate = function () {
      if($input.val() != "")
      {  
        var t = $input[0];
        SlicksetTimeblr(t);
            var d = true; //SlickvalidateTime(t);
            
          if (d==false) {
            return {
              valid: false,
              msg: "Invalid Date !"
            };
          }
      }
      if (args.column.validator) {
        var validationResults = args.column.validator($input.val());
        if (!validationResults.valid) {
          return validationResults;
        }
      }
      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }
  
  
  function TextListEditorNew(args) {
    var $input,$id, $picker;
    var defaultValue;
    var defaultText;
    var oldText;
    var oldValue;
    var scope = this;
    
    this.init = function () {
      $input = $("<INPUT type=text  autocomplete='off' id='txtlst' class='editor-textlist' />");
      $input.width($(args.container).innerWidth());
      $input.appendTo(args.container);
      
       if(args.column.maxLength > 0)
           $input.attr({maxlength: args.column.maxLength});      
      
      $id = $("<input type='hidden' name='lth' />");
      $id.appendTo(args.container);
      
      var ListSize ='10';
      var ListHight = '';
      var GridTop =-1;
      var style ="style='min-width:"+$(args.container).innerWidth()+"px;";
      var MarginTop ='';
      if(args.grid.getOptions().GridName != undefined)
      {
          var GridName = args.grid.getOptions().GridName;
          var headersNo = $('#'+GridName + ' .slick-header-columns').length;
          var headersHeight = $('#'+GridName + ' .slick-header-columns').height();
          if(args.grid.getOptions().headerRowHeight != undefined)
          {
              var hedrRwHeit = eval(args.grid.getOptions().headerRowHeight);
              headersHeight = headersHeight + hedrRwHeit;
          }
          GridTop= $('#'+GridName).offset().top;
          var CellTop=$(args.container).offset().top;
          var GridHeight = $('#'+GridName).height();
          ListSize = parseInt((parseInt($('.slick-viewport', args.grid.thisObj.outGridName).prop("clientHeight"),10)-30) / 20); 
          ListHight = parseInt(GridHeight/2);
          ListHight = ListHight - (headersHeight * headersNo);
          var TopDiff =CellTop-GridTop;
          
          var _topdiff = TopDiff -  (headersHeight * headersNo);
          var _gridheight = parseInt(GridHeight/2) -  (headersHeight * headersNo);
          var _colhight = $(args.container).height();       
          var _diff =Math.abs(_topdiff - _gridheight);
          if(_diff > _colhight)
          {
                if(TopDiff > (eval(GridHeight)/2))
                    _diff = _diff -10;
                ListHight = ListHight + _diff;
                ListSize = ListSize + (_diff/_colhight);
          } 
          if(TopDiff > (eval(GridHeight)/2))
          {
              ListTopMarginAdd =80;
              if(args.grid.getOptions().ListTopMarginAdd != undefined)
                ListTopMarginAdd = eval(args.grid.getOptions().ListTopMarginAdd);
              var TopMarginVal=ListHight +ListTopMarginAdd;
              style = style + "margin-top:-"+TopMarginVal+"px;";
          }
          
          var dif = 0;
          var MaxListHight=500;
          if(ListHight > MaxListHight)
             dif =ListHight-MaxListHight;
          if(TopDiff > (eval(GridHeight)/2))
          {
              ListTopMarginAdd =80;
              if(args.grid.getOptions().ListTopMarginAdd != undefined)
                ListTopMarginAdd = eval(args.grid.getOptions().ListTopMarginAdd);
              var TopMarginVal=ListHight +ListTopMarginAdd;
              if(ListHight > MaxListHight)
              {
                TopMarginVal = TopMarginVal - dif;
                ListHight =MaxListHight;
              }  
              style = style + "margin-top:-"+TopMarginVal+"px;";
          }
          else if(ListHight > MaxListHight)
            ListHight =MaxListHight;
            
          if(args.grid.getOptions().showFooterRow==true)
          {
            var hhh = $('.slick-footerrow-columns','#'+GridName).height();
            ListHight = ListHight - hhh;
          }
          if(ListHight<45)
            ListHight = 45;
            
          style = style + "height:"+ListHight+"px;";
      }
      style = style + "'";
      var optlist= SlickGetOptionList(args.column.optionsArray, args.column);    
      
      if(ListSize<2) ListSize=2;
      
      $picker = $("<div class='editor-textlist-picker' style='display:none' />").appendTo(args.container);
      $picker.append("<SELECT tabIndex='0' class='editor-textlist-list' " + style + " size="+ListSize+">"+optlist+"</SELECT>");
      $picker.width($(args.container).innerWidth());
      $picker.appendTo(args.container);
      $input.focus().select();

      $picker.find(".editor-textlist-list").on("click", function (e) {
            args.commitChanges();
      })
      
      $picker.find(".editor-textlist-list").on("change", function (e) {
        $input.val($picker.find(".editor-textlist-list :selected").text());
        $id.val($picker.find(".editor-textlist-list").val());
      })
      
      
      $input.on("contextmenu", function (e){  
					var c = e;
					var d = args;
					args.grid.onContextMenu.notify(args,e,args.grid)
					e.stopImmediatePropagation();
					})
					
      $input.on("focus click", function (e) {
             
            var match =false;
            var preselected =-1;
            var text = $(this).val(); 
            var options = $picker.find(".editor-textlist-list")[0].options; 
            for (var i = 0; i < options.length; i++) {
                var option = options[i]; 
                if(option.selected)
                    preselected=i;
                if(text != '')
                {    
                    var optionText = option.text; 
                    var lowerOptionText = optionText.toLowerCase();
                    var lowerText = text.toLowerCase(); 
                    if (lowerOptionText == lowerText) {
                        option.selected = true;
                        
                        return;
                    }
                }
            }
            if(preselected >=0)
              options[preselected].selected = false;
            
        });
      
      $input.on("keydown", function (e) {
            $picker.show();
            if (e.which == 13 || e.which == 9)
            {
                
                if($picker.find(".editor-textlist-list option:selected").length >0)
                {
                    $input.val($picker.find(".editor-textlist-list :selected").text());
                    $id.val($picker.find(".editor-textlist-list").val());
                }
                else if(getIsSlickItemFromList())
                {
                    if($input.val() != "")
                    {
                        var ListMsg ='Enter from given list only.';
                        if(args.column.editorListMsg != undefined && args.column.editorListMsg != '')
                            ListMsg =args.column.editorListMsg;          
                        alert(ListMsg);
                        $picker.find(".editor-textlist-list").focus();
                        e.stopImmediatePropagation();
                    }
                    else
                    {
                        args.item[args.column.field] = "";
                        args.item[args.column.fieldval]="";
                    }
                }
                else 
                {
                    var vvll = $input.val();
                    $($picker.find(".editor-textlist-list option")).each(function(t){ if($(this).text()==$input.val()) vvll = $(this).val(); })
                    $id.val(vvll);
                }
            }
            else if (e.which == 39 || e.which == 37) {
					e.stopImmediatePropagation();
            }
            else if (e.which == 38 || e.which == 40) {
                $picker.find(".editor-textlist-list").focus();
                if($('option','.editor-textlist-list').length>0)
                {
                    $('.editor-textlist-list').val($('option:eq(0)','.editor-textlist-list').val())
                    $('.editor-textlist-list').trigger("change");
                }
                e.stopImmediatePropagation();
            }
      });
      $picker.find(".editor-textlist-list").on("keydown", function (e) {
            if (e.which == 8) {
                $input.focus().select();
                args.item[args.column.field] = "";
                args.item[args.column.fieldval]="";
            } 
            else if (e.which == 38 || e.which == 40) {
                e.stopImmediatePropagation();
           } 
      })
      $input.on("keyup", function (e) {
            var preselected =-1;
            var text = $(this).val().toLowerCase(); 
            var options = $picker.find(".editor-textlist-list")[0].options; 
            for (var i = 0; i < options.length; i++) {
                var option = options[i]; 
                if(option.selected)
                    preselected=i;
                if(text != '')
                {    
                    var textLen = text.length;
                    var optionText = option.text.toLowerCase(); 
                    if (text === optionText || text === optionText.substring(0,textLen)) {
                        option.selected = true;
                        return;
                    }
                }
            }
            if(preselected >=0)
              options[preselected].selected = false;
            
        });
    };

    this.destroy = function () {
      $input.remove();
      $id.remove();
      $picker.remove();
    };

    this.focus = function () {
      $input.focus();
    };
    
    function getIsSlickItemFromList() {
        // returns weather Given item from List
        var rtn = args.column.editorIsItemFromList;
        if (typeof rtn == 'undefined') {
            rtn = true;
        }
        return rtn;
    }

    this.loadValue = function (item) {
         
      defaultValue = item[args.column.fieldval];
      defaultText = item[args.column.field];
      $input.val(item[args.column.field]);
      $id.val(item[args.column.fieldval]);
      $input.select();
    };

    this.prevSerializeValue = function () {
      return {input:oldText, id: oldValue};
    };
    this.serializeValue = function () {
      return {input:$input.val(), id: $id.val()};
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state.input;
      item[args.column.fieldval] = state.id;
    };

    this.isValueChanged = function () {
        oldText = defaultText;
        oldValue = defaultValue;
        FirstCharCapital();
        return ($id.val() != defaultValue);
    };
    
    function FirstCharCapital() {
       if($id.val() != "" && $id.val() != defaultValue)
       { 
           var rtn = args.column.isFirstCharCapital;
           if(rtn != null && rtn != undefined && rtn=="1")
           {
                var colVal =$id.val();
                var regex = new RegExp("^[ A-Za-z]", "i");
                match = colVal.match(regex); 
                if (match) 
                {
                    var colValU =  colVal.charAt(0).toUpperCase()
                    if(colVal.length > 1)
                        colValU = colValU + colVal.substr(1, colVal.length);
                    $id.val(colValU); 
                    $input.val(colValU);
                }
            }
        }
    }
    
    this.validate = function () {
      if (args.column.validator) {
        var validationResults = args.column.validator($input.val());
        if (!validationResults.valid) {
          return validationResults;
        }
      }

      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }

    function MultiSelectDropdownEditor(args) {
        var $input, $wrapper, $checkBoxInput, selectedchkBoxArray = [];
        var defaultValue;
        var scope = this;
        // check scope get this value

        var chkBoxListData = getChkBoxDataList(args);
        var chkBoxAllValues = chkBoxListData.AllValues;
        //chkBoxAllValues.sort();
        var selectedchkBox = chkBoxListData.SelectedValues;
        if (!(selectedchkBox == undefined || selectedchkBox == '')) {
            if (selectedchkBox.length > 0) selectedchkBoxArray = selectedchkBox.split(";");
        }
        this.init = function () {

            if (chkBoxAllValues.length != 0) {
                var $container = $("body");
                $wrapper = $("<DIV style='z-index:10000;position:absolute;background:white;padding:5px;border:3px solid gray; -moz-border-radius:10px; border-radius:10px;'/>")
                    .appendTo($container);

                for (var i = 0; i < chkBoxAllValues.length; i++) {
                    if (!(selectedchkBoxArray == undefined || selectedchkBoxArray == '')) {
                        if (selectedchkBoxArray.length > 0 && selectedchkBoxArray.indexOf(chkBoxAllValues[i]) > -1) {
                            $checkBoxInput = $("<input class='chkBox' type='checkbox' name='" + chkBoxAllValues[i] + "' id='chkBox_" + i + "' checked='checked'/>" + chkBoxAllValues[i] + "<br />");
                        }
                        else
                            $checkBoxInput = $("<input class='chkBox' type='checkbox' name='" + chkBoxAllValues[i] + "' id='chkBox_" + i + "'/>" + chkBoxAllValues[i] + "<br />");
                    }
                    else
                        $checkBoxInput = $("<input class='chkBox' type='checkbox' name='" + chkBoxAllValues[i] + "' id='chkBox_" + i + "'/>" + chkBoxAllValues[i] + "<br />");

                    $wrapper.append($checkBoxInput);
                }

                $wrapper.append("<br/><br/>");

                $input = $("<TEXTAREA style='display:none;' hidefocus rows=25 style='background:white;width:150px;height:100px;border:1px solid;outline:0'>")
                    .appendTo($wrapper);

                $("<DIV style='text-align:right'><BUTTON>Save</BUTTON><BUTTON>Cancel</BUTTON></DIV>")
                    .appendTo($wrapper);

                $wrapper.find("button:first").on("click", this.save);
                $wrapper.find("button:last").on("click", this.cancel);
                $input.on("keydown", this.handleKeyDown);
            }
            else {

                alert("Dropdown list is empty. Kindly provide data for this dropdown list");
            }
            scope.position(args.position);
            $input.focus().select();

            $('input[type="checkbox"]').change(function () {
                var name = $(this).prop('name');
                var chkboxId = $(this).prop('id');
                var check = $(this).prop('checked');
                var currentValue = $input.val();
                if (check) {
                    var allSelectedValues = '';
                    $('input[type="checkbox"]').each(function () {
                        var isChecked = $(this).prop('checked');
                        var name = $(this).prop('name');
                        var currentChekBoxId = $(this).prop('id');
                        if (isChecked) {
                            if (allSelectedValues.length == 0) allSelectedValues = name;
                            else allSelectedValues = allSelectedValues + ";" + name;
                        }
                    });
                    $input.val('');
                    $input.val(allSelectedValues);
                }
                else {
                    var allSelectedValues = '';
                    $('input[type="checkbox"]').each(function () {
                        var isChecked = $(this).prop('checked');

                        var name = $(this).prop('name');
                        var currentChekBoxId = $(this).prop('id');
                        if (isChecked) {
                            if (allSelectedValues.length == 0) allSelectedValues = name;
                            else allSelectedValues = allSelectedValues + ";" + name;
                        }
                    });
                    $input.val('');
                    $input.val(allSelectedValues);
                }
            });
            var allSelValues = '';
            $('input[type="checkbox"]').each(function () {
                var isChecked = $(this).prop('checked');

                var name = $(this).prop('name');
                var currentChekBoxId = $(this).prop('id');
                if (isChecked) {
                    if (allSelValues.length == 0) allSelValues = name;
                    else allSelValues = allSelValues + ";" + name;
                }
            });
            $input.val('');
            $input.val(allSelValues);
        };

        this.handleKeyDown = function (e) {
            if (e.which == $.ui.keyCode.ENTER && e.ctrlKey) {
                scope.save();
            } else if (e.which == $.ui.keyCode.ESCAPE) {
                e.preventDefault();
                scope.cancel();
            } else if (e.which == $.ui.keyCode.TAB && e.shiftKey) {
                e.preventDefault();
                args.grid.navigatePrev();
            } else if (e.which == $.ui.keyCode.TAB) {
                e.preventDefault();
                args.grid.navigateNext();
            }
        };

        this.save = function () {
            args.commitChanges();
            $wrapper.hide();
        };

        this.cancel = function () {
            $input.val(defaultValue);
            args.cancelChanges();
        };

        this.hide = function () {
            $wrapper.hide();
        };

        this.show = function () {
            $wrapper.show();
        };

        this.position = function (position) {
            $wrapper
                .css("top", position.top - 5)
                .css("left", position.left - 5)
        };

        this.destroy = function () {
            $wrapper.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.loadValue = function (item) {
            $input.val(defaultValue = item[args.column.field]);
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            return {
                valid: true,
                msg: null
            };
        };

        this.init();
    }

    
})(jQuery);

/*
    * An example of a "Multi-Select Dropdown" editor.
    * "DropdownListData" is an array to store all the checkbox options required for the dropdowwn multi-select field.
    */

function getChkBoxDataList(args) {
        var Data =
        {
            "AllValues": args.column.optionsArray,
            "SelectedValues": args.item[args.column.field]
            /*
             * args.item.country is used to read the value of the field "country" of a particular row.
             * This "SelectedValues" array generates prepopulated data if you want to retrieve data from your data base.
             * Lets for emxample for row no 1 : you have 2 countries, this field captures the name of these countries(should be seprated by semicolon) and mark the checkboxes of those country as checked.
             */
        }
    return Data;
    
    /*
     * add else if conditions if you have another multi-select dropdown list as well.
     */

}