/***
 * Contains basic SlickGrid formatters.
 * 
 * NOTE:  These are merely examples.  You will most likely need to implement something more
 *        robust/extensible/localizable/etc. for your use!
 * 
 * @module Formatters
 * @namespace Slick
 */

(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "Formatters": {
                "PercentComplete": PercentCompleteFormatter,
                "PercentCompleteBar": PercentCompleteBarFormatter,
                "YesNo": YesNoFormatter,
                "Checkmark": CheckmarkFormatter,
                "ListText": ListTextFormatter,
                "CheckmarkAll": CheckmarkAllFormatter,
                "LinkAll": LinkAllFormatter,
                "newDateFormatter": newDateFormatter,
                "newDecimalFormatter": newDecimalFormatter,
                "ButtonFormatter": ButtonFormatter,
                "DeleteFormatter": DeleteFormatter,
                "LinkAllJS": LinkAllJSFormatter
            }
        }
    });

    function PercentCompleteFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value === "") {
            return "-";
        } else if (value < 50) {
            return "<span style='color:red;font-weight:bold;'>" + value + "%</span>";
        } else {
            return "<span style='color:green'>" + value + "%</span>";
        }
    }

    function PercentCompleteBarFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value === "") {
            return "";
        }

        var color;

        if (value < 30) {
            color = "red";
        } else if (value < 70) {
            color = "silver";
        } else {
            color = "green";
        }

        return "<span class='percent-complete-bar' style='background:" + color + ";width:" + value + "%'></span>";
    }

    function YesNoFormatter(row, cell, value, columnDef, dataContext) {
        return value ? "Yes" : "No";
    }

    function CheckmarkFormatter(row, cell, value, columnDef, dataContext) {
        return value ? "<img src='../images/tick.png'>" : "";
    }
    function ListTextFormatter(row, cell, value, columnDef, dataContext) {
        return dataContext.input;
    }
    function CheckmarkAllFormatter(row, cell, value, columnDef, dataContext) {

        if (typeof (value) == "string") {
            if (value.toString().toLowerCase() == "true" || value.toString().toLowerCase() == "1")
                value = true;
            else
                value = false;
        }

        return value ? '<input class="editor-checkbox" type="checkbox" value="' + value + '" checked="true">' : '<input class="editor-checkbox" type="checkbox">';
    }
    function ButtonFormatter(row, cell, value, columnDef, dataContext) {

        var vvl = value;

        if (columnDef.field != undefined && columnDef.field != null && columnDef.field != "")
            vvl = dataContext[columnDef.field];

        if (columnDef.fixedText != undefined && columnDef.fixedText != null && columnDef.fixedText != "")
            vvl = columnDef.fixedText;

        if (typeof (columnDef.checkHide) == "function") {
            if (columnDef.checkHide(dataContext))
                return '';
        }


        return vvl ? '<input type="button" value="' + vvl + '" />' : '<input  type="button" value="..." >';

    }
    function LinkAllFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value == undefined || value == '')
            value = 'Click Here';
        return "<a style='COLOR: #009' href='#'>" + value + "</a>";
    }
    function LinkAllJSFormatter(row, cell, value, columnDef, dataContext) {
        if (value == null || value == undefined || value == '')
            value = 'Click Here';
        return "<a style='COLOR: #009' onclick='callSlickLinkfunction(" + row + "," + cell + ")'>" + value + "</a>";
    }
    function newDateFormatter(row, cell, value, columnDef, dataContext) {

        var nd = SlickGetDateInStr(value)
        dataContext[columnDef.field] = nd;
        return nd;

    }

    function newDecimalFormatter(row, cell, value, columnDef, dataContext) {

        var ndf = value
        if (isNaN(parseFloat(ndf)))
            return "";
        if (columnDef.editorFixedDecimalPlaces != null && typeof (columnDef.editorFixedDecimalPlaces) != "undefined" && !isNaN(parseFloat(value)))
            ndf = parseFloat(value).toFixed(columnDef.editorFixedDecimalPlaces)
        dataContext[columnDef.field] = ndf	//	updating data in item object from integer to float (yogesh)
        return ndf;

    }
    function DeleteFormatter(row, cell, value, columnDef, dataContext) {

        
        return '<i class="fa fa-trash" aria-hidden="true"></i>';

    }


})(jQuery);
