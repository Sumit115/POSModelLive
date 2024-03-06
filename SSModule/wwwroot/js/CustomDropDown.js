/**
 *  @author:    Sumit Yadav
 *  @created:   MAr 2024
 *  @description: Commonly used jQuery functions for User services
 *  Last Updated: Sumit Yadav, Mar 02 2024
 **/


var arrCusDrpHideColumn = ['CreationDate', 'DATE_MODIFIED'];
var typingTimer;
var custypingTimer;
var doneTypingInterval = 1000;
var doneTypingInterval1 = 500;
var _Custdropdown = {};
var AltDown = false, CtrlDown = false, ShiftDown = false, AltPresed = false; var isClicked = false;
AltlKey = 18, cmdKey = 91, CtrlKey = 17, WKey = 87, cKey = 67, keyNew = 78, keySave = 83, keyCancel = 65; keyTab = 9; ShiftKey = 16;
var keyA = 65, keyB = 66, keyC = 67, keyD = 68, keyE = 69, keyF = 70, keyG = 71, keyH = 72, keyI = 73, keyJ = 74, keyK = 75, keyL = 76,
    keyM = 77, keyN = 78, keyO = 79, keyP = 80, keyQ = 81, keyR = 82, keyS = 83, keyT = 84, keyU = 85, keyV = 86, keyW = 87, keyX = 88,
    keyY = 89, keyZ = 90;
//             A   B C  D  E  F  G  H  I  J  K  L  M   N  O  P  Q  R  S  T  U  V  W  X  Y  Z
var arrKeys = [38, 40, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90];
var IsMobile = false;
$(document).ready(function () {

    $(".cusdropdown").each(function () {
        var hid = $(this).attr("hid");
        var spn = $(this).find('span');
        if (_Custdropdown[hid] == undefined)
            _Custdropdown[hid] = new CustomDDL(hid, spn);
    });

    $(document).mouseup(function (e) {
        var container = $(".auto-drp");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            $(".form-row-cl").find(".auto-drp").each(function (index) {
                if ($(this).find(".panel-collapse").css("display") === "block") {
                    $(this).find(".panel-collapse").css("display", "none");
                }
            });
        }
    });
    $("body").click(function (event) {
        if (event.target.id != "dvCommonCusDropList") {
            if ($("#dvCommonCusDropList").length > 0) {
                $("#dvCommonCusDropList").hide();
                $('#dvMenu').addClass('open').removeClass('menu-open-left');
            }
            if ($(".cusdropdown.open").length > 0) {
                $(".cusdropdown.open").removeClass('open')
            }
            $(".custom-dropdown-inner-deta").hide().html('');
        }
    });
});

function CustomDDL(ctrlId, spn) {

    var _data = {};

    var $drpCtrltxt = $("#drp" + ctrlId);
    var $drpCtrlVal = $("#" + ctrlId);
    var $drpCtrldPageNo = $("#hidPageNo" + ctrlId);
    var $drpCtrlsearch = $("#search" + ctrlId);
    var $drpCtrlList = $("#drpList" + ctrlId);
    var $drpCtrUl = $("#ul" + ctrlId);
    var $drpCtrdvList = $("#dvCommonCusDropList");
    var $drpCtrlExtra = $("#hidExtra" + ctrlId);



    var cbLoad = null;
    var cbSelect = null;

    this.onLoad = {
        call: function (cb) {
            if (typeof (cb) == "function")
                cbLoad = cb;
        }
    }
    this.onSelect = {
        call: function (cb) {
            if (typeof (cb) == "function")
                cbSelect = cb;
        }
    }


    var ListLoad = function (param, evnt) {
        var ExtraParam = '';
        var PageNo = $drpCtrldPageNo.val();
        if (PageNo === '' || evnt === '') {
            PageNo = 1;
            $drpCtrldPageNo.val(1);
        }

        var data = {
            pageno: PageNo,
            pagesize: 20,
            search: $drpCtrltxt.val(),
            hidExtra: "",
            param: ''
        }

        var hidExtra = $drpCtrlExtra.val();
        if (hidExtra !== '') {
            if (hidExtra.indexOf(',') !== -1) {
                var arrExtra = hidExtra.split(',');
                $(arrExtra).each(function (extraIndex, extraItem) {
                    ExtraParam += $('#' + extraItem).val() + '~';
                });
            } else {
                ExtraParam = $('#' + hidExtra).val();
            }
            data["ExtraParam"] = ExtraParam;
        }

        var result = "";
        if (typeof (cbLoad) == "function") {
            var result = cbLoad(data)
            BindList(result, evnt, param, PageNo);
        }
        else {
            var url = Handler.currentPath() + ctrlId;
            $.ajax({
                url: url,
                data: data,
                method: 'POST',
                dataType: 'JSON',
                async: false,
                success: function (res) {
                    BindList(res, evnt, param, PageNo);
                }
                , error: function (jqXHR, exception) {
                }
            });
        }
    }

    var BindList = function (result, evnt, param, PageNo) {
        if (result == "" || result == null) {
            if (evnt !== 'scroll') {
                $drpCtrdvList.html("<span class='drp-not-found'> Data Not Found </span>");
            }
            return false;
        }
        else {
            var html = '';
            var res = JSON.stringify(result);
            if (res.indexOf('Response":"Error:') !== -1) {
                alert(result.Response.replace('Error:', ''));
                hideList();
                return false;
            }

            if (result.length > 0) {
                var cols = GetHeaders(result);
                var KeyID = '';
                var KeyValue = '';
                $.each(cols, function (index, item) {
                    if (index === 0)
                        KeyID = item;
                    else if (index === 1)
                        KeyValue = item;
                });


                if (PageNo === 1) {
                    html = "<li index='-1'><a href='javascript: void(0)'> <table> <thead> <tr>";
                    $.each(cols, function (index, item) {
                        if (index > 0 && arrCusDrpHideColumn.indexOf(item) === -1)
                            html += "<th>" + item + "</th>";
                    });
                    html += "</tr> </thead></table></a> </li>";
                }

                $.each(result, function (index, item) {
                    var clsDelete = '';

                    if (item[KeyValue] === null) {
                        html += "<li index='" + index + "'  hid='" + item[KeyID].toString().replace("'", "^") + "' hidname='" + "" + "' ><a href='javascript: void(0)' calss='dropdown-item' >  <table><tr class='" + clsDelete + "'>";
                    }
                    else {
                        html += "<li index='" + index + "'  hid='" + item[KeyID].toString().replace("'", "^") + "' hidname='" + item[KeyValue].toString().replace("'", "^") + "' ><a href='javascript: void(0)' class='dropdown-item' >  <table><tr class='" + clsDelete + "'>";
                    }

                    $.each(cols, function (innerIndex, innerItem) {
                        if (innerIndex > 0 && arrCusDrpHideColumn.indexOf(innerItem) === -1) {
                            if (item[innerItem] === null)
                                html += "<td class=clsdrp" + innerItem + ">&nbsp;</td>";
                            else
                                html += "<td class=clsdrp" + innerItem + ">" + item[innerItem] + "</td>";
                        }
                    });
                    html += "</tr> </table> </li>";
                });
            }
            else {
                if (evnt !== 'scroll')
                    html = "<span class='drp-not-found'> Data Not Found </span>";
            }

            if (evnt === 'scroll')
                $drpCtrdvList.children("ul").append(html);
            else {
                $drpCtrdvList.children("ul").html(html);
            }
            $drpCtrdvList.show();
            if (param === 'auto') {
                $('#dvCommonCusDropList > ul > li').eq(1).find('a').focus();
            }
            $("#dvCommonCusDropList ul li").on("keyup", function (e) {
                if ($(this).attr('index') === "-1" && e.which === 38) {
                    $("#drp" + ctrlId).focus();
                } else if (e.which === 9) {
                    hideList();
                } else {
                    if (e.which === 13) {
                        $drpCtrlList.removeClass('open');
                        if ($("#hidEvent" + ctrlId).val() !== '') {
                            eval($("#hidEvent" + ctrlId).val());
                        }
                    }
                }
            });
            $("#dvCommonCusDropList ul li").unbind("click");
            $("#dvCommonCusDropList ul li").on("click", function () {
                SelectList(this);
            });
            $drpCtrUl.scroll(function () {
                var pno = $("#hidPageNo" + ctrlId).val();
                $("#dvCommonCusDropList ul li:first-child").css("top", +$(this).scrollTop());
                if (pno === '')
                    pno = 1;
                if ($(this).scrollTop() > (500 * pno)) {
                    pno = parseInt(pno) + 1;

                    $("#hidPageNo" + ctrlId).val(pno);
                    ListLoad("", "", "scroll");
                }
            });
        }
    }

    var SelectList = function (S) {
        _data = {};
        var drpValue = $(S).attr("hid");
        var drpText = $(S).attr("hidname").trim();
        $drpCtrltxt.val(drpText);
        $drpCtrlVal.val(drpValue);
        $(S).find("tr").children("td").each(function () {
            var field = $(this).attr("class").replace("clsdrp", "");
            var fieldValue = $(this).text();
            _data[field] = fieldValue;
        })

        if ($(S).find(".clsdrpBarcode").hasClass(".clsdrpBarcode"))
            $("#Barcode").val($(S).find(".clsdrpBarcode").text());

        if ($("#hidEvent" + ctrlId).val() !== '') {
            eval($("#hidEvent" + ctrlId).val());
        }
        else {
            var arg = {
                value: drpValue,
                text: drpText,
                data: _data
            }
            if (typeof (cbSelect) == "function")
                cbSelect(arg);
        }

        hideList();
        $drpCtrltxt.focus();

    };
    this.Reset = function () {
        $drpCtrlVal.val("");
        $drpCtrltxt.val("");
        $drpCtrldPageNo.val("1");
        $drpCtrlsearch.val("");
    }
    this.Set = function (text, value) {

        $drpCtrlVal.val(value);
        $drpCtrlVal.attr({ "oldvalue": value, "oldtext": text });

        if (value === null || value === undefined || value.toString().trim() === "") {
            $drpCtrlVal.val("");
            $drpCtrltxt.val(text);
        }
        else {
            $drpCtrltxt.val(text);
        }
        $drpCtrldPageNo.val("1");
        $drpCtrlsearch.val(text);
    }

    this.GEt = function () {
        return { "value": $drpCtrlVal.val(), "text": $drpCtrltxt.val() };
    }

    $drpCtrltxt.keydown(function (e) {
        if ($(this).attr('readonly') !== 'readonly') {
            if ($(this).hasClass('drp-num')) {
                if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105) || e.keyCode === 8 || e.keyCode === 9 ||
                    e.keyCode === 37 || e.keyCode === 39 || e.keyCode === 46 || e.keyCode === 190 || e.keyCode === 110) {

                } else {
                    e.preventDefault();
                }
            }
            if (e.which === 9) {                
                hideList();                
            }
            
        }
    }).keyup(function (e) {
        if ($(this).attr('readonly') !== 'readonly') {
            var tempDrpValue = $(this).val();
            if (AltDown !== true && CtrlKey !== true) {
                if (e.which === 38 || e.which === 40) {
                    if ($("#dvCommonCusDropList ul li").length > 1) {
                        $("#dvCommonCusDropList ul li:nth-child(2)").find("a").focus();
                    }
                    $("#dvCommonCusDropList ul").scrollTop(0);
                    
                }

                else if ((e.which < 46 || e.which > 57) && (e.which < 65 || e.which > 90) && (e.which < 96 || e.which > 105) && (e.which !== 8) && (e.which !== 229) && (e.which !== 32) && (e.which !== 115)) {
                    hideList();
                    e.preventDefault(e);
                }
                else {
                    $drpCtrlVal.val('');
                    if (!$drpCtrltxt.hasClass('drpEditable') || e.which == 115) {
                            ShowList(hid, '');
                    }
                }
                // for manage dropdown value on  up/down key
                $(this).val(tempDrpValue);
            }
        }
    }).focusout(function (e) {
        var item = $(this);
        if (!item.hasClass('drpEditable')) {
            var ids = item.attr('id').replace('drp', '');
            if ($('#' + ids).val() === '') {
                item.val('');
            }
        }
    }).focusin(function () {

        if ($(this).val() !== '') {
            $(this).select();
        }
    });

    $(spn).off("click").on("click", function (e) {

        $drpCtrdvList.offset();
        if ($drpCtrltxt.attr("disabled") !== 'disabled') {
            $(".cusdropdown.open").each(function () {
                var droId = $(this).attr("id");
                    $(this).removeClass('open');
            });

            if ($drpCtrlList.hasClass("open")) {
                hideList();
            }
            else {

                ShowList('00001');
            }
            isAutoOpenBatchClicked = true;
            setTimeout(function () {
                isAutoOpenBatchClicked = false;
            }, 3000);
            $drpCtrltxt.focus();

        }
        e.stopPropagation();
    });

    //hideCustomDropdown
    var hideList = function () {
        $drpCtrlList.removeClass("open");
        $drpCtrdvList.children('ul').empty();
        $drpCtrdvList.hide();
    }

    //callCustomDropDown
    var ShowList = function (drpid, param) {
        if (AltDown === false) {
            SetListPosition(function () {
                ListLoad(drpid, "", param, "");
            });
        }
    }

    var SetListPosition = function (callback) {
        if ($drpCtrdvList.length === 0) {
            var htm = '<div id="dvCommonCusDropList" class="grid-dropdown" style="display: none;">'+
                '<ul style="height:200px; overflow-y:scroll;"></ul>'+
                '</div>';
            $("body").append(htm);
            $drpCtrdvList = $("#dvCommonCusDropList");
        }

        var ctrlOffset = $drpCtrltxt.offset();
        var DivWidth = '600px';
        var ElementWidth = $drpCtrdvList.width();
        var LeftPosition = ctrlOffset.left;
        var TopPosition = ctrlOffset.top ;

        if ((window.innerHeight - TopPosition) < 200) {
            TopPosition = TopPosition - (200 + $drpCtrltxt.outerHeight());
        }
        else {
            TopPosition = TopPosition + $drpCtrltxt.outerHeight();
        }

        $drpCtrdvList.css({
            "left": LeftPosition,
            "top": TopPosition,
            "display": "block",
            "width": DivWidth + "!important"
        });

        callback();
    }
    
    function GetHeaders(obj) {
        if (obj.length > 0) {
            var names = Object.keys(obj[0]);
            return names;
        } else
            return [];
    }
    function onChange(drpid) {
        eval($("#hidEvent" + ctrlId).val());
    }



}
