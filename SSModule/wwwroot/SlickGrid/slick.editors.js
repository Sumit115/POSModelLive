/***
 * Contains basic SlickGrid editors.
 * @module Editors
 * @namespace Slick
 */

(function ($) {
  // register namespace
  $.extend(true, window, {
    "Slick": {
      "Editors": {
        "Text": TextEditor,
        "Integer": IntegerEditor,
        "Float": FloatEditor,
        "Date": DateEditor,
        "YesNoSelect": YesNoSelectEditor,
        "Checkbox": CheckboxEditor,
        "PercentComplete": PercentCompleteEditor,
        "LongText": LongTextEditor,
        "TextList": TextListEditor,
        "CheckboxAlways": CheckboxAlwaysEditor,
        "ComboSelect": ComboSelectEditor,
        "Date1": Date1Editor,
        "LinkAlways": LinkAlwaysEditor
      }
    }
  });
     

  function TextEditor(args) {
    var $input;
    var defaultValue;
     var oldValue;
    var scope = this;

    this.init = function () {
      $input = $("<INPUT type=text autocomplete='off' class='editor-text' />")
          .appendTo(args.container)
          .on("keydown.nav", function (e) {
            if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
              e.stopImmediatePropagation();
            }
            else
            {
                if(typeof($(this).attr("formatValidator"))!="undefined")
                if($(this).attr("formatValidator")!="")
                {
                    return SlickSetTextFormat($(this)[0],$(this).attr("formatValidator"))
                }
            }
          })
          .focus()
          .select();
          
          if(args.column.maxLength > 0)
            $input.attr({maxlength: args.column.maxLength});
            
          if(args.column.formatValidator !=undefined && args.column.formatValidator !="")
            $input.attr({formatValidator: args.column.formatValidator});
    };

    this.destroy = function () {
      $input.remove();
    };

    this.focus = function () {
      $input.focus();
    };

    this.getValue = function () {
      return $input.val();
    };

    this.setValue = function (val) {
      $input.val(val);
    };

    this.loadValue = function (item) {
      defaultValue = item[args.column.field] || "";
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
      FirstCharCapital();
      return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
    };
    
    function FirstCharCapital() {
       if($input.val() != "" && $input.val() != defaultValue)
       { 
           var rtn = args.column.isFirstCharCapital;
           if(rtn != null && rtn != undefined && rtn=="1")
           {
                var colVal =$input.val();
                var regex = new RegExp("^[ A-Za-z]", "i");
                match = colVal.match(regex); 
                if (match) 
                {
                    var colValU =  colVal.charAt(0).toUpperCase()
                    if(colVal.length > 1)
                        colValU = colValU + colVal.substr(1, colVal.length);
                    $input.val(colValU);
                }
            }
            var rtn = args.column.isFullTextCapital;
            if(rtn != null && rtn != undefined && rtn=="1")
            {
                var colVal =$input.val();
                colVal =  colVal.toUpperCase()
                $input.val(colVal);
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
      
        if(typeof($input.attr("formatValidator"))!="undefined")
        if($input.attr("formatValidator")!="" && $input.val()!="")
        {   
            $input.val($input.val().toUpperCase())
            var isValidFormat = SlickValidateTextFormat($input.val(), $input.attr("formatValidator"))
            if(!isValidFormat)
            {
                return { valid: false, msg:"Invalid Data Fromat" }
            }
        }

      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }

  function IntegerEditor(args) {
    var $input;
    var defaultValue;
    var oldValue;
    var scope = this;

    this.init = function () {
      $input = $("<INPUT type=text  autocomplete='off'  class='editor-text ar' />");

      $input.on("keydown.nav", function (e) {
        if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
          e.stopImmediatePropagation();
        }
      });

      $input.appendTo(args.container);
      $input.focus().select();
      

       if(args.column.maxLength > 0)
            $input.attr({maxlength: args.column.maxLength});      
       else     
            $input.attr({maxlength: "12"});
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
      return parseInt($input.val(), 10) || 0;
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state;
    };

    this.isValueChanged = function () {
      oldValue = defaultValue;  
      return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
    };

    this.validate = function () {
      if (isNaN($input.val())) {
        return {
          valid: false,
          msg: "Please enter a valid integer"
        };
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

  function FloatEditor(args) {
    var $input;
    var defaultValue;
    var oldValue;
    var scope = this;

    this.init = function () {
      $input = $("<INPUT type=text  autocomplete='off' class='editor-text ar' />");

      $input.on("keydown.nav", function (e) {
        if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
          e.stopImmediatePropagation();
        }
      });

        if(args.column.maxLength > 0)
            $input.attr({maxlength: args.column.maxLength});
            
      $input.appendTo(args.container);
      $input.focus().select();
      
      $input.width($input.width() - 2);
    };

    this.destroy = function () {
      $input.remove();
    };

    this.focus = function () {
      $input.focus();
    };

    function getDecimalPlaces() {
        // returns the number of fixed decimal places or null
        var rtn = args.column.editorFixedDecimalPlaces;
        if (typeof rtn == 'undefined') {
            rtn = FloatEditor.DefaultDecimalPlaces;
        }
        return (!rtn && rtn!==0 ? null : rtn);
    }
    
    function getCommaSeprateAmount() {
        // returns weather comma seprater applicable or not 
        var rtn = args.column.editorCommaSeprateAmount;
        if (typeof rtn == 'undefined') {
            rtn = FloatEditor.DefaultCommaSeprateAmount;
        }
        return rtn;
    }
    
    function getNonNegative() {
        // returns the weather Non negetive allowd or not
        var rtn = args.column.editorNonNegative;
        if (typeof rtn == 'undefined') {
            rtn = FloatEditor.DefaultNonNegative;
        }
        return (!rtn && rtn!==0 ? null : rtn);
    }

    this.loadValue = function (item) {
      if(item[args.column.field] != undefined)  
        defaultValue = String(item[args.column.field]).replace(new RegExp(',','g'),'');
      else 
        defaultValue = item[args.column.field];  
    
        if(!isNaN(parseFloat(defaultValue)))
        defaultValue = parseFloat(defaultValue)

      var decPlaces = getDecimalPlaces();
      if (decPlaces !== null
      && (defaultValue || defaultValue===0)
      && defaultValue.toFixed) {
        defaultValue = defaultValue.toFixed(decPlaces);
      }

      $input.val(defaultValue);
      $input[0].defaultValue = defaultValue;
      $input.select();
    };
    
    this.prevSerializeValue = function () {
      return {input:oldValue};
    };

    this.serializeValue = function () {
      var rtn ='';
      if($input.val() != '')
      {
          var rtn = parseFloat($input.val()) || 0;
          var decPlaces = getDecimalPlaces();
          if (decPlaces !== null
          && (rtn || rtn===0)
          && rtn.toFixed) {
            rtn = parseFloat(rtn).toFixed(decPlaces);
          }
      }
      return rtn;
    };

    this.applyValue = function (item, state) {
      if(getCommaSeprateAmount())
      {
         if(state != '')
           state = SlickFormatAmount(state); 
      } 
      item[args.column.field] = state;
    };

    this.isValueChanged = function () {
       oldValue = defaultValue;  
      var input_val =$input.val();
      if(input_val != '')
      {
        input_val =input_val.replace(new RegExp(',','g'),'');
        var decPlaces = getDecimalPlaces();
        if(decPlaces !== null)
          input_val = parseFloat(input_val).toFixed(decPlaces);
      }
      return (!(input_val == "" && defaultValue == null)) && (input_val != defaultValue);
    };

    this.validate = function () {
      var NonNegative  = getNonNegative();
      var OtherValOk=true;
      if(NonNegative)
      {
        var inputval = $input.val();
        if($input.val().indexOf("-") >=0)
            OtherValOk=false;
      }
      if (isNaN($input.val()) || OtherValOk ==false) {
        return {
          valid: false,
          msg: "Please enter a valid number"
        };
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

  FloatEditor.DefaultDecimalPlaces = null;
  FloatEditor.DefaultCommaSeprateAmount = false;
  FloatEditor.DefaultNonNegative = false;

  function DateEditor(args) {
    var $input;
    var defaultValue;
    var scope = this;
    var calendarOpen = false;

    this.init = function () {
      $input = $("<INPUT type=text  autocomplete='off' class='editor-text' />");
      $input.appendTo(args.container);
      $input.focus().select();
      $input.datepicker({
        showOn: "button",
        buttonImageOnly: true,
         beforeShow: function () {
          calendarOpen = true
        },
        onClose: function () {
          calendarOpen = false
        }
      });
      $input.width($input.width() - 2);
    };

    this.destroy = function () {
      $.datepicker.dpDiv.stop(true, true);
      $input.datepicker("hide");
      $input.datepicker("destroy");
      $input.remove();
    };

    this.show = function () {
      if (calendarOpen) {
        $.datepicker.dpDiv.stop(true, true).show();
      }
    };

    this.hide = function () {
      if (calendarOpen) {
        $.datepicker.dpDiv.stop(true, true).hide();
      }
    };

    this.position = function (position) {
      if (!calendarOpen) {
        return;
      }
      $.datepicker.dpDiv
          .css("top", position.top + 30)
          .css("left", position.left);
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

  function YesNoSelectEditor(args) {
    var $select;
    var defaultValue;
    var scope = this;

    this.init = function () {
      $select = $("<SELECT tabIndex='0' class='editor-yesno'><OPTION value='yes'>Yes</OPTION><OPTION value='no'>No</OPTION></SELECT>");
      $select.appendTo(args.container);
      $select.focus();
    };

    this.destroy = function () {
      $select.remove();
    };

    this.focus = function () {
      $select.focus();
    };

    this.loadValue = function (item) {
      $select.val((defaultValue = item[args.column.field]) ? "yes" : "no");
      $select.select();
    };

    this.serializeValue = function () {
      return ($select.val() == "yes");
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state;
    };

    this.isValueChanged = function () {
      return ($select.val() != defaultValue);
    };

    this.validate = function () {
      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }

  function CheckboxEditor(args) {
    var $select;
    var defaultValue;
    var scope = this;

    this.init = function () {
      $select = $("<INPUT type=checkbox value='true' class='editor-checkbox' hideFocus>");
      $select.appendTo(args.container);
      $select.focus();
    };

    this.destroy = function () {
      $select.remove();
    };

    this.focus = function () {
      $select.focus();
    };

    this.loadValue = function (item) {
      defaultValue = !!item[args.column.field];
      if (defaultValue) {
        $select.prop('checked', true);
      } else {
        $select.prop('checked', false);
      }
    };

    this.serializeValue = function () {
      return $select.prop('checked');
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state;
    };

    this.isValueChanged = function () {
      return (this.serializeValue() !== defaultValue);
    };

    this.validate = function () {
      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }

  function PercentCompleteEditor(args) {
    var $input, $picker;
    var defaultValue;
    var scope = this;

    this.init = function () {
      $input = $("<INPUT type=text  autocomplete='off' class='editor-percentcomplete' />");
      $input.width($(args.container).innerWidth() - 25);
      $input.appendTo(args.container);

      $picker = $("<div class='editor-percentcomplete-picker' />").appendTo(args.container);
      $picker.append("<div class='editor-percentcomplete-helper'><div class='editor-percentcomplete-wrapper'><div class='editor-percentcomplete-slider' /><div class='editor-percentcomplete-buttons' /></div></div>");

      $picker.find(".editor-percentcomplete-buttons").append("<button val=0>Not started</button><br/><button val=50>In Progress</button><br/><button val=100>Complete</button>");

      $input.focus().select();

      $picker.find(".editor-percentcomplete-slider").slider({
        orientation: "vertical",
        range: "min",
        value: defaultValue,
        slide: function (event, ui) {
          $input.val(ui.value)
        }
      });

      $picker.find(".editor-percentcomplete-buttons button").on("click", function (e) {
        $input.val($(this).attr("val"));
        $picker.find(".editor-percentcomplete-slider").slider("value", $(this).attr("val"));
      })
    };

    this.destroy = function () {
      $input.remove();
      $picker.remove();
    };

    this.focus = function () {
      $input.focus();
    };

    this.loadValue = function (item) {
      $input.val(defaultValue = item[args.column.field]);
      $input.select();
    };

    this.serializeValue = function () {
      return parseInt($input.val(), 10) || 0;
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state;
    };

    this.isValueChanged = function () {
      return (!($input.val() == "" && defaultValue == null)) && ((parseInt($input.val(), 10) || 0) != defaultValue);
    };

    this.validate = function () {
      if (isNaN(parseInt($input.val(), 10))) {
        return {
          valid: false,
          msg: "Please enter a valid positive number"
        };
      }

      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }

  /*
   * An example of a "detached" editor.
   * The UI is added onto document BODY and .position(), .show() and .hide() are implemented.
   * KeyDown events are also handled to provide handling for Tab, Shift-Tab, Esc and Ctrl-Enter.
   */
  function LongTextEditor(args) {
    var $input, $wrapper;
    var defaultValue;
    var scope = this;

    this.init = function () {
      var $container = $("body");

      $wrapper = $("<DIV style='z-index:10000;position:absolute;background:white;padding:5px;border:3px solid gray; -moz-border-radius:10px; border-radius:10px;'/>")
          .appendTo($container);

      $input = $("<TEXTAREA hidefocus rows=5 style='background:white;width:250px;height:80px;border:0;outline:0'>")
          .appendTo($wrapper);

      $("<DIV style='text-align:right'><BUTTON>Save</BUTTON><BUTTON>Cancel</BUTTON></DIV>")
          .appendTo($wrapper);

      $wrapper.find("button:first").on("click", this.save);
      $wrapper.find("button:last").on("click", this.cancel);
      $input.on("keydown", this.handleKeyDown);

      scope.position(args.position);
      $input.focus().select();
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
      $input.select();
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
  
  function TextListEditor(args) {
    var $input,$id, $picker;
    var defaultValue;
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
          ListSize = parseInt((parseInt(GridHeight/2) - (headersHeight * headersNo)) / 20); 
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
          if(ListSize < 2)
            ListSize=2;
          if(TopDiff > (eval(GridHeight)/2))
          {
              ListTopMarginAdd =80;
              if(args.grid.getOptions().ListTopMarginAdd != undefined)
                ListTopMarginAdd = eval(args.grid.getOptions().ListTopMarginAdd);
              var TopMarginVal=ListHight +ListTopMarginAdd;
              style = style + "margin-top:-"+TopMarginVal+"px;";
          }
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
      
      $picker = $("<div class='editor-textlist-picker' />").appendTo(args.container);
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
      
      $input.on("focus", function (e) {
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
                    $id.val($input.val());
            }
            else if (e.which == 39 || e.which == 37) {
					e.stopImmediatePropagation();
            }
            else if (e.which == 38 || e.which == 40) {
                $picker.find(".editor-textlist-list").focus();
                if(navigator != undefined)
                {
                    if(navigator.userAgent != undefined)
                    {
                        if(navigator.userAgent.toLowerCase().indexOf('chrome') > 0)
                        {
                            if($picker.find(".editor-textlist-list").val() == null) {
                                $picker.find(".editor-textlist-list option:eq(0)").prop('selected','true');
                                $picker.find(".editor-textlist-list").val( $picker.find(".editor-textlist-list option:eq(0)").val() );                                
                                args.item[args.column.field] = $picker.find(".editor-textlist-list option:eq(0)").text();
                                args.item[args.column.fieldval] = $picker.find(".editor-textlist-list option:eq(0)").val();                                                                
                                $input.val( args.item[args.column.field] );
                                $id.val(args.item[args.column.fieldval]);
                            }
                            else {
                                args.item[args.column.field] = $picker.find(".editor-textlist-list option:eq(0)").text();
                                args.item[args.column.fieldval] = $picker.find(".editor-textlist-list option:eq(0)").val();                                                                
                                $input.val( args.item[args.column.field] );
                                $id.val(args.item[args.column.fieldval]);
                             
                            }
                        }
                    }
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
                    var regex = new RegExp("^" + lowerText, "i");
                    match = lowerOptionText.match(regex); 
                    if (match || lowerText==lowerOptionText) {
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
      $input.val(item[args.column.field]);
      $id.val(item[args.column.fieldval]);
      $input.select();
    };

    this.serializeValue = function () {
      return {input:$input.val(), id: $id.val()};
    };

    this.applyValue = function (item, state) {
      item[args.column.field] = state.input;
      item[args.column.fieldval] = state.id;
    };

    this.isValueChanged = function () {
        return ($id.val() != defaultValue);
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
  
  function CheckboxAlwaysEditor(args) {
    var $select;
    var defaultValue;
    var scope = this;

    this.init = function () {
      $select = $("<INPUT type=checkbox value='true' class='editor-checkbox' hideFocus>");
      $select.appendTo(args.container);
      $select.focus();
    };

    this.destroy = function () {
      $select.remove();
    };

    this.focus = function () {
      $select.focus();
    };

    this.loadValue = function (item) {
      var eventname = '';
      if(event != undefined)
      {
          if(event.type != null)
          {  
            if(event.type != undefined)  
                eventname = event.type;
          }
      }
      defaultValue = !!item[args.column.field];
      if (defaultValue) {
        $select.prop('checked', (eventname == 'click' ? false:true));
      } else {
        $select.prop('checked', (eventname == 'click' ? true:false));
      }
    };

    this.serializeValue = function () {
      return $select.prop('checked');
    };

    this.applyValue = function (item, state) {
        item[args.column.field] = state;
    };

    this.isValueChanged = function () {
      return (this.serializeValue() !== defaultValue);
    };

    this.validate = function () {
      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }
  function LinkAlwaysEditor(args) {
    var $select;
    var defaultValue;
    var scope = this;

    this.init = function () {
      $select = $("<a style='COLOR: #009' href='#'>Click here</a>");
      $select.appendTo(args.container);
      $select.focus();
    };
    this.init();
  }
  
  function ComboSelectEditor(args) {
    var $select;
    var defaultValue;
    var defaultText;
    var oldText;
    var oldValue;
    var scope = this;
    
    this.init = function () {
      var optlist= SlickGetOptionList(args.column.optionsArray, args.column);
      var style="style='max-width:"+$(args.container).innerWidth()+"px; min-width:"+$(args.container).innerWidth()+"px;'";
      $select = $("<SELECT tabIndex='0' class='editor-combo' "+style+">"+optlist+"</SELECT>");
      $select.appendTo(args.container);
      $select.focus();
      
       $select.on("keydown", function (e) {
            if (e.which == 38 || e.which == 40) {
                e.stopImmediatePropagation();
           } 
      })
      
    };
    
    
    this.destroy = function () {
      $select.remove();
    };

    this.focus = function () {
      $select.focus();
    };

    this.loadValue = function (item) {
      defaultValue = item[args.column.fieldval];
      defaultText = item[args.column.field];
      $select.val(item[args.column.fieldval]);
      $select.focus();
    };

    this.prevSerializeValue = function () {
      return {input:oldText, id: oldValue};
    };

    this.serializeValue = function () {
        return {cmb:$select.children(':selected').text(), cmbval:$select.val()};
    };

    this.applyValue = function (item, state) {
         item[args.column.field] = state.cmb;
         item[args.column.fieldval] = state.cmbval;
    };

    this.isValueChanged = function () {
        oldText = defaultText;
        oldValue = defaultValue;
       return ($select.val() != defaultValue);
    };

    this.validate = function () {
      return {
        valid: true,
        msg: null
      };
    };

    this.init();
  }
   function Date1Editor(args) {
    var $input;
    var defaultValue;
    var oldValue;
    var scope = this;

    this.init = function () {
      $input = $("<INPUT type=text  autocomplete='off' class='editor-text ac' />");
      
      $input.appendTo(args.container);
      $input.focus().select();
        
      $input.on("keydown", function (e) {
            SlickBeforeenterDt(this);
      });
      
      $input.on("keyup", function (e) {
            SlickEnterdataDt(this);
      });
      
       $input.on("focus", function (e) {
            SlickOnfocusDt(this);
      });
//      $input.on("blur", function (e) {
//            SlickOnleaveDt(this);
//      });
      
      

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
            var d = SlickOnleaveDt(t);
            
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
})(jQuery);
