/**
 *  @author:    Sumit Yadav
 *  @created:   Nov 2020
 *  @description: Commonly used jQuery functions for User services
 *  Last Updated: Sumit Yadav, Nov 23 2020
 **/

///////////////// /////////////// showWait function :- Loader With msg /////////////// ///////////////
function Handler_showWait(msg) {
    msg = msg == undefined ? "Please wait ..." : msg;
    if ($(".loader").length == 0) {
        var d = document.createElement("div");
        var h = ($(window).height() / 3);
        var w = (document.body.parentElement.offsetWidth / 2.8);
        var img = Handler.rootPath() + "/images/logo-D.png";
        var htm = '<div class="cnt" style="text-align:center;position:fixed;font-size:30px;font-weight:bold;color:#fff;top:' + h + 'px;left:' + w + 'px;z-index:11000;">';
        htm += '<img alt="Please wait..." src="' + img + '" />';
        htm += '<br />';
        htm += '<span class="loadermsg" ></span>';
        htm += '';
        htm += '</div>';
        d.innerHTML = htm;
        document.body.appendChild(d);
        $(d).addClass("loader");
        w = Math.max(document.body.parentElement.offsetWidth, document.body.offsetWidth);
        h = $(window).height();
        $(".loader").css({ "z-index": 11000, "background-color": "black", "position": "fixed", "top": "0px", "opacity": ".5", "filter": "alpha(opacity=50)", "display": "block", "width": w + "px", "height": h + "px" });
    }
    $(".loader .loadermsg").html(msg)
    $(".loader").show();
}
/////////////// /////////////// hideWait function :- hide Loader/other_selector on Page  /////////////// ///////////////
function Handler_hideWait(h) {
    if (h != undefined)
        $(".loader").hide(h);
    else
        $(".loader").hide();
}
/////////////// /////////////// AjaxMethod function :- Get/Post json ajax request with loader/session_expire option /////////////// ///////////////
function Handler_AjaxMethod(m, p, t, d, a, c) {
    var jo = {};
    jo["url"] = m == "this" ? p : Handler.CU + "ActionType=" + m + p;
    jo["cache"] = false;
    jo["async"] = true;
    jo["dataType"] = "json";
    if (t == "POST") { jo["type"] = t; jo["data"] = JSON.stringify(d); }
    if (a != undefined) { jo["beforeSend"] = Handler.loader(a); }
    $.ajax(jo).done(function (res) {
        if (res.status == "redirect")
            window.location.href = res.msg;
        else {
            if (c != null)
                c(res);
        }
    }).fail(function (jqXHr, textStatus, errorThrown) {
        alert(jqXHr.responseText);
        Handler.hide();
    });
}
/////////////// /////////////// NameValueCollection function:- Get QueryString return in Json format /////////////// ///////////////
function Handler_NameValueCollection() {
    var output = {};
    var Qrystr = location.search.substr(1);
    var arrQrystrin = Qrystr.split("&");
    if (arrQrystrin.length > 0) {
        for (var i in arrQrystrin) {
            var temparr = arrQrystrin[i].split("=");
            if (temparr.length > 1 && temparr[1].length > 0) {
                output[temparr[0]] = unescape(temparr[1]);
            }
            else if (temparr.length = 1) {
                output[temparr[0]] = "";
            }
        }
    }
    return output;
}
/////////////// /////////////// showPopUp function:- Get Popup on page /////////////// ///////////////
function Handler_showPopUp(_html, style, callBackFun, closeFun) {
    if ($(".popup_d").length == 0) {
        var d = document.createElement("div");
        var htm = '<div class="popclose" style="margin: auto;width: 720px;margin-top: 95px;margin-bottom: 0px;height: 27px;cursor: pointer;font-size: 26px;color: whitesmoke;text-align: right;" >';
        htm += '<i class="fa fa-times-circle"></i></div>';
        htm += '<div class="popinner" style="margin: auto;width: 680px;height: 420px;background-color: #fff;border-radius:5px">';
        htm += '</div>';
        d.innerHTML = htm;
        $(d).addClass("popup_d");
        //        var _w = Math.max(document.body.parentElement.offsetWidth, document.body.offsetWidth);
        //        var _h = $(window).height();
        document.body.appendChild(d);
        $(".popup_d .popclose").click(function () {
            $(this).closest(".popup_d").hide();
            if (typeof (closeFun) === "function")
                closeFun();
        });
        $(".popup_d").attr('style', 'position: fixed;left: 0px;right: 0px;bottom: 0px;top: 0px;background-color: rgba(0, 0, 0, 0.7);filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000);-ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000)";z-index: 999 !important;');
    }
    if (!Handler.isNullOrEmpty(_html))
        $(".popup_d .popinner").html(_html)
    if (!Handler.isNullOrEmpty(style))
        $(".popup_d .popinner").css(style);

    if (typeof (style.width) != "undefined") {
        var w = style.width.replace("px", "");
        w = (parseInt(w) + 45) + "px";
        $(".popup_d .popclose").css("width", w);
    }
    $(".popup_d").show();
    if (typeof (callBackFun) == "function")
        callBackFun();
    else
        return true;

}
/////////////// /////////////// ActiveXObject function:- Check and Get object /////////////// ///////////////
function Handler_ActiveXObject() {
    if (Handler.bowser() != "Internet Explorer")
        return null;
    try {
        var Obj = new ActiveXObject("CompuOffice.Util");
        if (Obj.Version < 29) {
            alert("Please update \"CompuOffice ActiveX\" and try again !");
            window.open('../CompuOffice%20ActiveX%20Setup.exe');
            return null;
        }
        else if (Obj.Version1 == undefined || Obj.Version1 < 3) {
            alert("Please update \"CompuOffice ActiveX\" and try again !");
            window.open('../CompuOffice%20ActiveX%20Setup.exe');
            return null;
        }
        else
            return Obj;
    }
    catch (e) {
        alert(e.message);
        return null;
    }
}
/////////////// /////////////// ///////////////  Common use Convert functions  /////////////// /////////////// ///////////////
var Handler_Convert = {
    toDate: function (d) { // formate [ 'dd/MM/yyyy', 'dd-MM-yyyy', 'dd/MMM/yyyy', 'dd-MMM-yyyy' ]
        try {
            var dt = null;
            if (typeof (d) === "string") {
                var a = d.replace(/\-/g, '/').split("/"), a30 = [4, 6, 9, 11];
                if (a.length == 3) {
                    var dy = parseInt(a[0] | 0);
                    //str.charAt(0).toUpperCase() + str.slice(1)
                    var m = a[1].length == 3 ? Handler.month.MN.indexOf(a[1]) : parseInt(a[1] | 0);
                    var y = parseInt(a[2] | 0);
                    var isL = (((y % 4 == 0) && (y % 100 != 0)) || (y % 400 == 0));
                    if (dy > 0 && dy <= 31 && y > 0 && m > 0 && m <= 12) {
                        if (a30.indexOf(m) != -1 && dy > 30)
                            throw "";
                        else if (m == 2 && isL && dy > 29)
                            throw "";
                        else if (m == 2 && !isL && dy > 28)
                            throw "";
                        else
                            dt = new Date(m + "/" + dy + "/" + y);
                    }
                }
            }
            return dt;
        }
        catch (e) {
            return null;
        }
    },
    toString: function (v) { return Handler.isNullOrEmpty(v) ? v = "" : v.toString(); },
    toFormat: function (d, f) {
        //d:Datetime, 
        //f:Format [ 'dd-MM-yyyy', 'dd/MM/yyyy', 'MM/dd/yyyy', 'HH:MM:SS', 'HH:MM', 'dd/MM/yyyy HH:MM:SS', 'dd-MM-yyyy HH:MM:SS']
        try {
            if (typeof (d) === "object" || typeof (d) === "function") {
                var dy = Handler.padLeft(d.getDate(), 2, '0'), m = Handler.padLeft((d.getMonth() + 1), 2, '0');
                var mmm = Handler.month.MN[(d.getMonth() + 1)];
                var y = d.getFullYear(), h = d.getUTCHours(), mi = d.getUTCMinutes(), s = d.getUTCSeconds();
                if (f.indexOf("HH:MM") != -1)
                    f = f.replace("HH:MM", h + ":" + mi);
                if (f.indexOf("SS") != -1)
                    f = f.replace("SS", s + "");
                if (f.indexOf("MMM") != -1)
                    f = f.replace("SS", s + "");
                if (f.indexOf("MM") != -1)
                    f = f.replace("MM", m + "");
                if (f.indexOf("dd") != -1)
                    f = f.replace("dd", dy + "");
                if (f.indexOf("yyyy") != -1)
                    f = f.replace("yyyy", y + "");
                return f;
            }
            else
                throw "Invalid Date";
        }
        catch (e) {
            return null;
        }
    }
};

/////////////// /////////////// ///////////////  Common use Formatter Controll  /////////////// /////////////// ///////////////
var Handler_Formatter = {
    PeriodYear: function ($ctrl) { // Formatter [ 'MMM-YYYY' ]
        if ($ctrl.length) {
            $ctrl.on("blur", function () {
                var $c = $(this);
                var V = "";
                if ($c.val().length == 7) {
                    var cyear = new Date().getFullYear();
                    var dt = $c.val().split("-")
                    if (dt.length == 2)
                        if (parseInt(dt[0] | 0) > 0 && parseInt(dt[0] | 0) <= 12 && parseInt(dt[1] | 0) > 1990 && parseInt(dt[1] | 0) <= cyear)
                            V = Handler.month.MN[parseInt(dt[0] | 0)] + "-" + dt[1];
                }
                $c.val(V);
                return;
            }).focus(function () {
                $(this).val('');
            }).keyup(function (event, data) {

                if (event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 8 || event.keyCode == 32) return;
                var $c = $(this);
                var V = $c.val();

                if (V.length == 2 && event.keyCode != 8) {
                    if (parseInt(V, 10) > 12)
                        V = V.substring(0, 1);
                }
                if (V.length == 2 && event.keyCode != 8)
                    V = V + "-";

                if (V.length == 1 && V != "0" && event.keyCode != 97)
                    V = "0" + V + "-";

                if (V.length == 4) {
                    var dt = V.split("-")
                    if (dt[1] == "0")
                        V = dt[0] + "-20" + dt[1];
                    if (dt[1] == "9" || dt[1] == "8" || dt[1] == "7")
                        V = dt[0] + "-19" + dt[1];
                }
                if (V.length == 5) {
                    var dt = V.split("-")
                    if (dt[1].substring(0, 1) == "1" && dt[1].substring(1, 2) != "9")
                        V = dt[0] + "-20" + dt[1];
                }
                $c.val(V);
            }).keydown(function () {
                if (event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 8) return;
                var $c = $(this);
                var V = $c.val();
                if (V.length == 1 && (event.keyCode == 111 || event.keyCode == 109 || event.keyCode == 191 || event.keyCode == 189))
                    $c.val("0" + V + "-");

                if (V.length == 2)
                    $c.val(V + "-");

                var len = $c.val().length;
                var ky = event.keyCode;
                if (len == 0 || len == 1 || len == 3 || len == 4 || len == 5 || len == 6) {
                    if ((ky < 48 || ky > 57) && (ky < 96 || ky > 105) && ky != 8 && ky != 17 && ky != 16) {
                        if (ky != 37)
                            event.preventDefault();
                    }
                }
                if (len == 7)
                    event.preventDefault();

            });
        }
    },
    Upper: function ($ctrl) {
        if ($ctrl.length) {
            $ctrl.keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });
        }
    },
    FristUpper: function ($ctrl) {
        if ($ctrl.length) {
            $ctrl.keyup(function () {
                var str = $(this).val();
                $(this).val(str.charAt(0).toUpperCase() + str.slice(1));
            });
        }
    },
    Camel: function ($ctrl) {
        if ($ctrl.length) {
            $ctrl.keyup(function () {
                var str = $(this).val();
                $(this).val(str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); }));
            });
        }
    },
    Numeric: function ($ctrl) {
        if ($ctrl.length) {
            $ctrl.keyup(function () {
                if (/\D/g.test($(this).val())) {
                    $(this).val($(this).val().replace(/\D/g, ''));
                }
            });
        }
    },
    Date: function ($ctrl) { // Formatter [ 'DD-MM-YYYY' ]
        if ($ctrl.length) {
            $ctrl.on("blur", function () {
                var $c = $(this);
                try {
                    if ($c.val().trim() != "") {
                        var d = Handler.convert.toDate($c.val()), td = new Date();
                        if (d != null) {
                            if (d.getFullYear() >= 1900) {
                                if (Date.parse(d) <= Date.parse(td)) {
                                    $c.val(Handler.convert.toFormat(d, 'dd-MM-yyyy'));
                                }
                                else
                                    throw "You cannot insert the date greater than today or equal.";
                            }
                            else
                                throw "The date should not be less than 01-01-1900";
                        }
                        else
                            throw "Invalid Date";
                    }
                }
                catch (e) {
                    alert(e);;
                    $c.val("");
                }
                return;
            }).focus(function () {
                if (event == null) return;
                try {
                    $(this).select();
                }
                catch (e) {
                    $c.val('');
                }
            }).keyup(function (event, data) {
                try {
                    if (event.keyCode == 0 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 8) return;
                    var $c = $(this);
                    var V = $c.val();
                    if (V.length == 2 && event.keyCode != 8) {
                        if (parseInt(V, 10) > 31)
                            V = V.substring(0, 1);
                    }
                    if (V.length == 2 && event.keyCode != 8)
                        V = $c.val() + "/";

                    if (V.length == 1 && event.keyCode == 111)
                        V = "0" + V + "/";

                    if (V.length == 4 && event.keyCode == 111) {
                        var dt = V.split("/");
                        if (parseInt(dt[1], 10) == 1)
                            V = dt[0] + "/0" + dt[1] + "/";
                    }
                    if (V.length == 4 && event.keyCode != 8) {
                        var dt = V.split("/");
                        if (parseInt(dt[1], 10) >= 2)
                            V = dt[0] + "/0" + dt[1];
                    }
                    if (V.length == 5 && event.keyCode != 8) {
                        var dt = V.split("/");
                        if (parseInt(dt[1], 10) > 12)
                            V = dt[0] + "/1";
                    }
                    if (V.length == 5 && event.keyCode != 8) {
                        V = V + "/"
                    }
                    if (V.length == 7) {
                        var dt = V.split("/")
                        if (dt[2] == "0")
                            V = dt[0] + "/" + dt[1] + "/20" + dt[2];
                        if (dt[2] == "9" || dt[2] == "8" || dt[2] == "7")
                            V = dt[0] + "/" + dt[1] + "/19" + dt[2];
                    }
                    if (V.length == 8) {
                        var dt = V.split("/")
                        if (dt[2].substring(0, 1) == "1" && dt[2].substring(1, 2) != "9")
                            V = dt[0] + "/" + dt[1] + "/20" + dt[2];
                    }
                    $c.val(V);
                }
                catch (e) {
                    $c.val('');
                }
            }).keydown(function () {
                try {
                    if (event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 8 || event.keyCode == 46) return;
                    var $c = $(this);
                    var V = $c.val();
                    if (V.length == 10)
                        $c.select();
                    var prevlu = V;

                    if (V.length == 1 && (event.keyCode == 111 || event.keyCode == 109 || event.keyCode == 191 || event.keyCode == 189))
                        V = "0" + V;
                    if (V.length == 2 && event.keyCode != 8)
                        V = V + "/";

                    if (V.length == 4 && (event.keyCode == 111 || event.keyCode == 109 || event.keyCode == 191 || event.keyCode == 189)) {
                        var dt = V.split("/")
                        V = dt[0] + "/0" + dt[1];
                    }
                    if (V.length == 5 && event.keyCode != 8) {
                        V = V + "/";
                    }
                    if (V.length == 0 && event.keyCode != 8 && ((event.keyCode >= 52 && event.keyCode <= 57) || (event.keyCode >= 100 && event.keyCode <= 105))) {
                        V = V + "0";
                    }
                    if (V.length == 3 && event.keyCode != 8 && ((event.keyCode >= 52 && event.keyCode <= 57) || (event.keyCode >= 100 && event.keyCode <= 105))) {
                        V = V + "0";
                    }
                    if (V.length == 6 && event.keyCode != 8 && event.keyCode == 25) {
                        V = V + "20";
                    }
                    $c.val(V);
                    invl = "F"
                    var len = V.length;
                    var ky = event.keyCode;
                    if (len == 0 || len == 1 || len == 3 || len == 4 || len == 6 || len == 7 || len == 8 || len == 9 || len == 10) {
                        if ((ky < 48 || ky > 57) && (ky < 96 || ky > 105) && ky != 8 && ky != 17 && ky != 16) {
                            if (ky != 37) {
                                event.keyCode = 0;
                                V = prevlu;
                            }
                        }
                    }
                }
                catch (e) {
                    $c.val("");
                }
            });
        }
    }
};
/////////////// /////////////// Common use for Call ChromeHelper /////////////// ///////////////
var Handler_B64_DriverTool = null;
var Handler_B64_Driverconfig = null;
var Handler_cacheId = null;
function Handler_Driver(Option) {
    Handler.loader();
    var _defaultOption = {
        soft: "",
        action: "",
        host: Handler.rootPath(),
        browser: "Chrome",
        isDownload: false,
        param: "",
        file: null, //[{"name":"abc.json", "content":"ABCDEF"}]
        onsuccess: null
    };
    var _dO = $.extend({}, _defaultOption, Option);

    var TUrl = _dO.host + "Master/OfficeAssistance/ChormePostServices.aspx?ActionType=gettool&softName=newtool&Inst=" + Math.random();
    var ConfigUrl = _dO.host + "Master/OfficeAssistance/ChormePostServices.aspx?ActionType=gettool&softName=newtoolconfig&Inst=" + Math.random();

    var _activeX = Handler.activeX();

    if (Handler_B64_DriverTool == null) {
        $.ajax({
            async: false, url: TUrl, cache: false, type: "GET", success: function (xml, status, xhr) {
                Handler_cacheId = xhr.getResponseHeader('SI');
                if (xml != "")
                    Handler_B64_DriverTool = xml;
            }
        });
    }
    if (Handler_B64_Driverconfig == null) {
        $.ajax({ async: false, url: ConfigUrl, cache: false, type: "GET" }).done(function (data) {
            if (data != "")
                Handler_B64_Driverconfig = data;
        });
    }

    var fileContent1 = "";
    var fileContent2 = "";
    var Filename = "Chrome.txt|ChromeHelper.exe.config|ChromeHelper_1.exe.config";
    if (_dO.file != null) {
        if (_dO.file.length > 0) {
            Filename = Filename + "|" + _dO.file[0].name;
            _dO.param += "&Upload1=" + _dO.file[0].name
            fileContent1 = _dO.file[0].content;
        }
        if (_dO.file.length > 1) {
            Filename = Filename + "|" + _dO.file[1].name;
            _dO.param += "&Upload2=" + _dO.file[1].name
            fileContent2 = _dO.file[1].content;
        }
    }

    var parmas = '/soft:"' + _dO.soft + '"';
    parmas += ' /action:"' + _dO.action + '"';
    parmas += ' /b:"' + _dO.browser + '"';
    parmas += ' /host:"' + _dO.host + '"';
    parmas += ' /params:"' + _dO.param + '"';

    if (_dO.isDownload)
        parmas += ' /d';

    if (_dO.onsuccess != null)
        parmas += ' /cache:"' + Handler_cacheId + '"';

    Filename = Filename + "||";
    fileContent1 = encodeBase64(fileContent1);
    fileContent2 = encodeBase64(fileContent2);
    var B64_filecontent = encodeBase64(parmas);

    if (Handler_B64_DriverTool != null && Handler_B64_Driverconfig != null) {
        sResponse = _activeX.RunAutoUtil(Handler_B64_DriverTool, "chromeHelper", parmas, 0, "", Filename, B64_filecontent, Handler_B64_Driverconfig, Handler_B64_Driverconfig, fileContent1, fileContent2);
        if (_dO.onsuccess != null) {
            var getcacheUrl = _dO.host + "Master/OfficeAssistance/ChormePostServices.aspx?ActionType=getcache&Id=" + Handler_cacheId + "&Inst=" + Math.random();
            $.ajax({ async: false, url: getcacheUrl, cache: false, type: "GET" }).responseText;

            var CK = { SF: 0, NOF: 0 };
            var callOnProcess = function () {
                var cacheUrl = _dO.host + "Master/OfficeAssistance/ChormePostServices.aspx?ActionType=Checkcache&Id=" + Handler_cacheId + "&Inst=" + Math.random();
                $.ajax({
                    url: cacheUrl,
                    cache: false,
                    type: "GET",
                    dataType: "JSON",
                    success: function (res, status, xhr) {
                        Handler.hide();
                        if (res.status == "start") {
                            CK.SF = CK.SF + 1;
                            Handler.loader("Work In Process. Please Wait...");
                            window.setTimeout(callOnProcess, 10000);
                        }
                        else if (res.status == "notfound") {
                            if (CK.NOF < 4) {
                                Handler.loader();
                                CK.NOF = CK.NOF + 1;
                                window.setTimeout(callOnProcess, 20000);;
                            }
                            else
                                alert("browser not reachable");
                        }
                        else if (res.status == "done") {
                            if (typeof (_dO.onsuccess) === "function")
                                _dO.onsuccess(res.data);
                            else if (typeof (_dO.onsuccess) === "string")
                                alert(res.data);
                            else
                                alert("Process Complete");
                        }
                        else if (res.status == "error")
                            alert(res.msg);
                    }
                });
            };
            window.setTimeout(callOnProcess, 20000);
        }
        else
            Handler.hide(5000);
    }
}
/////////////// /////////////// Common use functions/Variables /////////////// ///////////////
var Handler = {
    CU: "",
    loader: Handler_showWait,
    hide: Handler_hideWait,
    ajax: Handler_AjaxMethod,
    nvc: Handler_NameValueCollection,
    popUp: Handler_showPopUp,
    activeX: Handler_ActiveXObject,
    convert: Handler_Convert,
    formatter: Handler_Formatter,
    driver: Handler_Driver,
    rootPath: function () {
        var loc = location.href;
        var pathWeb = loc.indexOf('/Web/') != -1 ? '/Web/' : '/';
        if (loc.indexOf("/(") === -1)
            return location.protocol + '//' + location.host + pathWeb;
        else
            return loc.substring(0, loc.indexOf("))") + 2) + pathWeb;
    },
    currentPath: function () {

        var loc = location.href;
        debugger;
        if (location.href.indexOf("Create") != -1)
            return location.href.substring(0, location.href.indexOf("Create"));
        else if (loc.indexOf("Index") != -1)
            return loc.substring(0, loc.indexOf("Index"));
        else
            return location.href.substring(0, location.href.lastIndexOf("/") + 1);
    },
    isDebugMode: function () {
        var l = location.href.toLowerCase();
        return l.indexOf('localhost') != -1;
    },
    isNullOrEmpty: function (v) { return v === "" || v === null || v === undefined },
    parseString: function (v) { return Handler.isNullOrEmpty(v) ? "" : v; },
    month: {
        MN: ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        MFN: ['', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
    },
    pageHeader: function () { //$('#finyearText').text( caption ); 
    },
    dateDiff: function (s, e) {
        return Math.ceil((s.getTime() - e.getTime()) / (1000 * 60 * 60 * 24));
    },
    addDays: function (d, dd) {
        d.setDate(date.getDate() + dd);
        return d;
    },
    padLeft: function (v, c, chr) {
        if (v.toString().length < c) {
            var len = c - v.toString().length;
            for (var i = 0; i < len; i++) {
                v = chr.toString() + v.toString();
            }
        }
        return v.toString();
    },
    startWith: function (v, c) {
        var flag = false;
        if (v.toString().length >= c.toString().length)
            if (v.toString().substring(0, c.toString().length) == c.toString())
                flag = true;
        return flag;
    },
    bowser: function () {
        if (typeof (bowser) == "undefined") {
            $.ajax({
                async: false,
                type: 'script',
                url: Handler.rootPath() + "Master/Script/bowser.min.js",
                cache: false,
                type: "GET"
            }).responseText;
        }
        return bowser.name;
    },
    imgZoom: function (ctrl) {
        var src = $(ctrl).attr("src");
        var htm = '<div class="box" style="text-align: center;">';
        htm += '<img id="imgBankDetail" src="' + src + '" alt="" style="margin: 5px 0px;" onclick="page.imgPopup(this)"/>';
        htm += '</div>';
        Handler.popUp(htm, { width: "500px", height: "500px" }, null);
    }
};

function C_Ajax(url, Postdata, Loadmsg, callBackFunction) {
    var obj = {
        url:  url,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            callBackFunction(data)
        },
        error: function () {
            alert("Error occured!!")
        },
        complete: function () {
            Handler.hide();
        }
    };

    if (!Common.isNullOrEmpty(Postdata)) {
        obj["type"] = "POST";
        obj["data"] = Postdata;
    }
    if (Loadmsg != undefined) {
        obj["beforeSend"] = Handler.loader(Loadmsg);
    }
    $.ajax(obj);
}

function C_setInputFormat() {
    $(".input-number").keyup(function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    $(".input-Amount").css("text-align", "right").keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
    $(".input-int").css("text-align", "right").keyup(function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $(".input-Percentage").css("text-align", "right").keyup(function () {
        var v = this.value;
        if (v != "" && parseFloat(v || 0) > 100)
            v = "0";

        this.value = v.replace(/[^0-9\.]/g, '');
    });
    $('.datepicker').datetimepicker({
        timepicker: false,
        format: 'd/m/Y',
        formatDate: 'Y/m/d'
    });

    $(".upper").change(function () { $(this).val($(this).val().toUpperCase()) });

    $(".data-multiple-add").click(function () {
        var ctrl = $(this).attr("data-multiple_key");
        var val = $("#" + ctrl).val();
        $("#" + ctrl).val("");
        $("#" + ctrl).focus();
        var ulID = $("#" + ctrl).attr("data-multiple");
        if (val === '')
            alert("You must write something!");
        else
            C_multipleli(val, ulID);
    });

    $("[data-confirm-id]").unbind().bind('keyup', function () {
        var pass = $(this).attr("data-confirm-id");
        var ph = $("#" + pass).attr("placeholder");
        var confPass = $(this).attr("id");
        if (Common.isNullOrEmpty($("#" + pass).val()) && Common.isNullOrEmpty($("#" + confPass).val())) {
            $("[data-valmsg-for=" + confPass + "]").html('').attr('style', 'color: red !important');
            return;
        }
        if ($("#" + pass).val() == $("#" + confPass).val()) {
            $("[data-valmsg-for=" + confPass + "]").html("Success, " + ph + " Matched Successfully").attr('style', 'color: green !important');
        } else {
            $("[data-valmsg-for=" + confPass + "]").html(ph + " does not Match").attr('style', 'color: red !important');
        }
    });

    $("input, textarea, select").each(function () {
        $(this).attr("title", $(this).attr("placeholder"));
    });
}

function C_Get(s, src, c) {
    $("[data-valmsg-for]").attr('style', 'color: red !important');
    var lstRadio = [];
    var _d = {};
    var flag = true;
    $(s + " input[type='text']," + s + " input[type='password']," + s + " input[type='radio']," + s + " input[type='file']," + s + " select," + s + " textarea ").each(function () {
        var key = $(this).attr("id");
        var val = $(this).val();
        var req = $(this).attr('required');
        var type = $(this).attr("type");
        var pattern = $(this).attr('pattern');
        var multiple = $(this).attr('data-multiple');
        var minlength = $(this).attr('minlength');
        var maxlength = $(this).attr('maxlength');

        if (type == "file") {
            val = $(s + " #hd" + key + "").val();
        }
        else if (type == "radio") {
            key = $(this).attr('name');
            val = $("input[type='radio'][name='" + key + "']:checked").val()
        }
        else if (multiple != undefined) {
            val = "";
            $(s + ' #' + multiple + '  li').each(function () {
                var x = $(this).text();
                val += x.substr(0, x.length - 1) + "~";
            });
            if (val.length > 0)
                val = val.substr(0, val.length - 1)
        }

        if (req != undefined && val.trim() == "") {
            $(s + " [data-valmsg-for='" + key + "']").html("please Fill Value");
            flag = false;
        }
        else if (pattern != undefined && val.trim() != "") {
            if (pattern == "Email")
                pattern = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
            else if (pattern == "Gstn")
                pattern = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
            else if (pattern == "Pan")
                pattern = /[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
            else if (pattern == "Tan")
                pattern = /[A-Z]{4}[0-9]{5}[A-Z]{1}$/;
            else if (pattern == "PinCode")
                pattern = /^\d{6}$/;
            else if (pattern == "Mobile")
                pattern = /^[1-9]{1}[0-9]{9}$/;

            var patt = new RegExp(pattern);
            if (!patt.test(val)) {
                $(s + " [data-valmsg-for='" + key + "']").html("Invalid Value");
                flag = false;
            }
            else
                $(s + " [data-valmsg-for='" + key + "']").html("")
        }
        else if (minlength != undefined && val.trim() != "") {
            //console.log(minlength);
            var length = val.length;
            if (length < minlength) {
                //console.log("min" + minlength);
                $(s + " [data-valmsg-for='" + key + "']").html("Invalid Value");
                flag = false;
            }

        }
        else if (maxlength != undefined && val.trim() != "") {
            var length = val.length;
            if (length > maxlength) {
                //console.log("max" + length);
                $(s + " [data-valmsg-for='" + key + "']").html("Value Invalid");
                flag = false;
            }

        }
        else {
            $(s + " [data-valmsg-for='" + key + "']").html("")
        }
        _d[key] = val;
    });
    if (c != null)
        c(flag, _d);
}

function C_Set(s, _d, src, c) {
    $(s + " input[type='text']," + s + " input[type='password']," + s + " input[type='radio']," + s + " input[type='file']," + s + " select," + s + " textarea ").each(function () {
        var key = $(this).attr("id");
        var type = $(this).attr("type");
        var multiple = $(this).attr('data-multiple');
        if (_d[key] != undefined && _d[key] != null) {
            if (type == "file") {
                if (_d[key] == "") {
                    $(s + " #img" + key + "").attr("src", "/UploadFile/NA.jpg");
                }
                else {
                    $(s + " #img" + key + "").attr("src", src + _d[key]);
                    $(s + " #hd" + key + "").val(_d[key]);
                }
            }
            else if (multiple != undefined) {
                var arrval = _d[key].split("~");
                $(arrval).each(function (i, v) {
                    C_multipleli(v, multiple)
                });
            }
            else if (type == "radio") {
                key = $(this).attr('name');
                val = $("input[type='radio'][name='" + key + "'][value='" + _d[key] + "']").attr("checked", "checked")
            }
            else
                $(this).val(_d[key]);
        }
    });
    if (c != null)
        c();
}

function C_Reset(s) {
    $(s + " input[type='text']," + s + " input[type='password']," + s + " input[type='file']," + s + " select," + s + " textarea ").each(function () {
        var multiple = $(this).attr('data-multiple');
        var key = $(this).attr("id");
        var type = $(this).attr("type");
        if (multiple != undefined) {
            $(s + ' #' + multiple + '').empty();
        }
        if (type == "file") {
            $(s + " #img" + key + "").attr("src", "/UploadFile/NA.jpg");
            $(s + " #hd" + key + "").val("");
        }
        $(this).val("");
    });
}

function C_multipleli(val, ulID) {
    if (val != "") {
        var li = document.createElement("li");
        var t = document.createTextNode(val);
        li.appendChild(t);
        document.getElementById(ulID).appendChild(li);
        var span = document.createElement("SPAN");
        var txt = document.createTextNode("\u00D7");
        span.className = "close";
        span.appendChild(txt);
        li.appendChild(span);
        li.classList.add("list-group-item");
        var close = document.getElementsByClassName("close");
        var i;
        for (i = 0; i < close.length; i++) {
            close[i].onclick = function () {
                var div = this.parentElement;
                div.remove();
            }
        }
    }
}

function C_APIPin(pin, callBack) {
    var patt = new RegExp(/^\d{6}$/);
    if (pin == "")
        callBack({ status: "warning", msg: "" });
    else if (!patt.test(pin))
        callBack({ status: "error", msg: "invalid Pin Code." });
    else {
        Common.ajax("/API/pincode/" + pin, "", "", function (res) {
            if (res.status == "success") {
                var jo = JSON.parse(res.data)[0]
                if (jo.Status == "Success")
                    callBack({ status: "success", data: jo.PostOffice });
                else
                    callBack({ status: "error", msg: jo.Message });
            }
            else
                callBack({ status: "error", msg: res.msg });
        });
    }
};
//where calling just use event as 1st parameter
function C_preview(event, output) {
    var output = document.getElementById(output);
    output.src = URL.createObjectURL(event.target.files[0]);
    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
}
//alert box
function C_showAlert(AlertEnum, message) {
    var alertDiv = null;
    switch (AlertEnum) {
        case 1:
            alertDiv = "<div style='display:none;' class='alert alert-success alert-dismissable alertN'><button type='button' class='close' data-dismiss='alert'>×</button><b> Success : &nbsp;</ b ><i style='font-weight:normal'> " + message + " </i></div>";
            break;
        case 2:
            alertDiv = "<div style='display:none;' class='alert alert-danger alert-dismissible alertN'><button type='button' class='close' data-dismiss='alert'>×</button><b> Error : &nbsp;</ b ><i style='font-weight:normal'> " + message + " <i></div>";
            break;
        case 3:
            alertDiv = "<div style='display:none;' class='alert alert-info alert-dismissable alertN'><button type='button' class='close' data-dismiss='alert'>×</button><b> Info : &nbsp;</ b ><i style='font-weight:normal'> " + message + " <i></div>";
            break;
        case 4:
            alertDiv = "<div style='display:none;' class='alert alert-warning alert-dismissable alertN'><button type='button' class='close' data-dismiss='alert'>×</button><b> Warning : &nbsp;</b><i style='font-weight:normal'> " + message + " <i></div>";
            break;
    }
    $(".main-container").prepend($(alertDiv).fadeIn(500).delay(10000).fadeOut(500, function () { $(this).remove() }));
}

//enum
const C_AlertEnum = Object.freeze({ "Success": 1, "Danger": 2, "Info": 3, "Warning": 4 });

function C_Upload(pg, $ctrl, C) {
    var formData = new FormData();
    var fileUpload = $($ctrl).get(0);
    var files = fileUpload.files;
    var Id = $($ctrl).attr("id");
    //has file
    if (Common.isNullOrEmpty(files) || files.length == 0) {
        alert("Please choose file.");
        return false;
    }

    // Looping over all files and add it to FormData object
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }
    if ($("#hd" + Id + "").val() != "")
        formData.append('prevImgName', $("#hd" + Id + "").val());


    // Adding one more key to FormData object
    formData.append('pg', pg);
    formData.append('type', Id);

    $.ajax({
        type: "POST",
        url: '/Admin/Upload',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false
    }).done(function (res) {
        if (res.status == "success") {
            $("#img" + Id + "").attr("src", "/UploadFile/" + pg + "/" + res.fileName);
            $("#hd" + Id + "").val(res.fileName);
            if (C != null)
                C(res);
        }
        else
            alert(res.msg);
    }).fail(function (xhr, status, errorThrown) {
        alert('fail to request.');
    });
}

function C_Grid(n, n2, f) {

    //JSON.stringify({ FormId1: n})
    var url = "GridStrucher?FormId=" + n;
    if (n2 != '' && n2 != undefined && n2 != null) {
        url = "GridStrucher_Create?FormId=" + n + "&TranType=" + n2;

    }
    Common.ajax(Handler.currentPath() + url, {}, "Please Wait...", function (res) {
        Handler.hide();
        debugger;
        // console.log(res);
        if (res.PkGridId > 0) {
            // console.log(res);
            var d = JSON.parse(res.JsonData);
            // console.log(d);
            var j = {
                ColumnHeading: '',
                ColumnWidthPer: '',
                ColumnFields: '',
                Align: '',
                SearchType: '',
                SearchableColumns: '',
                SortableColumns: '',
                setCtrlType: ''
            }
            debugger;
            var filtered = d.filter(function (person) { return person.IsActive === 1 });
            filtered.sort((a, b) => (a.Orderby - b.Orderby));
            debugger;
            $(filtered).each(function (i, v) {
                j.ColumnHeading += "~" + v.Heading;
                j.ColumnWidthPer += "~" + v.Width;
                j.ColumnFields += "~" + v.Fields;
                j.Align += "~C";
                j.setCtrlType += "~" + v.CtrlType;
                if (j.SearchType != '') {
                    j.SearchType += "~" + v.SearchType;
                    if (v.SearchType == 1) {
                        if (j.SearchableColumns != '') { j.SearchableColumns += "~" + v.Fields; } else { j.SearchableColumns += v.Fields; }
                    }
                } else {
                    j.SearchType += v.SearchType;
                    if (v.SearchType == 1) {
                        if (j.SearchableColumns != '') { j.SearchableColumns += "~" + v.Fields; } else { j.SearchableColumns += v.Fields; }
                    }
                }

                if (v.Sortable == 1) {
                    if (j.SortableColumns != '') { j.SortableColumns += "~" + v.Fields; } else { j.SortableColumns += v.Fields; }
                }

            });
            j.ColumnHeading = j.ColumnHeading.substr(1);
            j.ColumnWidthPer = j.ColumnWidthPer.substr(1);
            j.ColumnFields = j.ColumnFields.substr(1);
            j.setCtrlType = j.setCtrlType.substr(1);
            j.Align = j.Align.substr(1);
            f(j);
        }
        else
            alert(res.msg);
    });
}

function C_GridColSetup(n, n2, f) {
    debugger;
    var url = "GridStrucher?FormId=" + n;
    if (n2 != '' && n2 != undefined && n2 != null) {
        url = "GridStrucher_Create?FormId=" + n + "&TranType=" + n2;
    }
    Common.ajax(Handler.currentPath() + url , {}, "Please Wait...", function (res) {
        Handler.hide();
        if (res.PkGridId > 0) {
            var d = JSON.parse(res.JsonData);
            var htm = '<style>input[type="checkbox"] {height: 20px;width: 20px;margin-right: 10px;}</style>';
            htm += '<div class="mb-4 card"><div class="card-body">';
            htm += '<div class="card-title">Fields</div><hr />';
            htm += '<input type="hidden" name="hdPkGridId" id="hdPkGridId"  value="' + n + '"  />';
            htm += '<div class="row"><div class="col-md-12 mb-2"  style="max-height:300px;overflow: auto;">';
            htm += '<table class="table table-striped table-bordered table-hover table-full-width">';
            htm += '<thead><tr><th>Column Name</th><th>Width</th><th>IsVisiable</th></thead>';
            htm += '<tbody >';
            $(d).each(function (i, v) {
                htm += '<tr class="trGridColumnw"><td>' + v.Heading + '</td>';
                htm += ' <td> ';
                htm += '<input type="hidden" name="txtGridColumnHeading"  value="' + v.Heading + '"  /> ';
                htm += '<input type="hidden" name="txtGridColumnFields"  value="' + v.Fields + '"  /> ';
                htm += '<input type="hidden" name="txtGridColumnSearchType"  value="' + v.SearchType + '"  /> ';
                htm += '<input type="hidden" name="txtGridColumnSortable"  value="' + v.Sortable + '"  /> ';
                htm += '<input type="hidden" name="txtGridColumnCtrlType"  value="' + v.CtrlType + '"  /> ';
                htm += '<input type="hidden" name="txtGridColumnOrderby"  value="' + v.Orderby + '"  /> ';
                htm += '<input type="text" name="txtGridColumnWidth"  value="' + v.Width + '"  /> ';
                htm += '</td><td>';
                htm += '<input type="checkbox" name="chkGridColumnField" ' + (v.IsActive == 1 ? "checked" : "") + ' value="' + v.pk_Id + '"  /> ';
                htm += '</td></tr>';
            });
            htm += ' </tbody></table>';
            htm += '</div></div> ';
            htm += '<div class="row"><div class="col-md-12">';
            htm += '<input type="button" id="btnSaveGridColSetup" value="Apply" class="btn btn-success"/>';
            htm += '</div></div> ';
            htm += '   </div></div>';
            Handler.popUp(htm, { width: "550px", height: "400px" }, function () {
                $("#btnSaveGridColSetup").click(function () {
                    var ColList = [];
                    $(".trGridColumnw").each(function () {
                        debugger;
                        var chk = $(this).find("[name='chkGridColumnField']");
                        var wid = $(this).find("[name='txtGridColumnWidth']");
                        var hding = $(this).find("[name='txtGridColumnHeading']");
                        var flds = $(this).find("[name='txtGridColumnFields']");
                        var styp = $(this).find("[name='txtGridColumnSearchType']");
                        var sotyp = $(this).find("[name='txtGridColumnSortable']");
                        var ctyp = $(this).find("[name='txtGridColumnCtrlType']");
                        var odrby = $(this).find("[name='txtGridColumnOrderby']");
                        ColList.push({ pk_Id: chk.val(), Width: wid.val(), Heading: hding.val(), Fields: flds.val(), SearchType: styp.val(), Sortable: sotyp.val(), CtrlType: ctyp.val(), Orderby: odrby.val(), IsActive: (chk.prop("checked") ? 1 : 0) });
                    });
                    console.log(ColList);
                    debugger;
                    //Common.ajax("/Master/Customer/ActiveGridColumn", JSON.stringify({ data: ColList }), "Please Wait...", function (res) {
                    //    if (res.status == "success") {
                    //        $(".popup_d").hide();
                    //        f();
                    //    }
                    //    else
                    //        alert(res.msg);
                    //});

                    //$.ajax({
                    //    type: 'POST',
                    //    url: "/Master/Customer/ActiveGridColumn",
                    //    data: JSON.stringify({ md: "abc" }),
                    //    contentType: "application/json; charset=utf-8",
                    //    processData: false,
                    //    success: function (res) {
                    //        alert();
                    //    },
                    //    error: function (err) {
                    //        console.log(err)
                    //    }
                    //})
                    var jsonData = JSON.stringify(ColList);

                    var _d = {
                        PkGridId: $('#hdPkGridId').val(),
                        JsonData: jsonData,
                        FkFormId: $("#hdFormId").val()
                    };
                    //
                    $.ajax({
                        type: "POST",
                        url: '/Master/Customer/ActiveGridColumn',
                        data: { data: _d },
                        datatype: "json",
                        success: function (res) {
                            if (res.status == "success") {
                                $(".popup_d").hide();
                                f();
                            }
                            else
                                alert(res.msg);
                        }
                    })
                    //  dsadassdasda();
                    //to prevent default form submit event
                    return false;
                });

            });
        }
        else
            alert(res.msg);
    });
}



var Common = {
    ajax: C_Ajax,
    InputFormat: C_setInputFormat,
    Get: C_Get,
    Set: C_Set,
    Reset: C_Reset,
    Upload: C_Upload,
    APIPin: C_APIPin,
    isNullOrEmpty: function (v) { return v === "" || v === null || v === undefined },
    preview: C_preview,
    AlertEnum: C_AlertEnum,
    showAlert: C_showAlert,
    Grid: C_Grid,
    GridColSetup: C_GridColSetup
};


jQuery.fn.extend({
    filterArray: function (v) {
        v = v.replace(/\\/g, "\\\\");
        var ar = v.replace("[", "").replace("]", "").split(",");

        return $(this)
            .filter(function (i, n) {
                var flag = false;
                for (i = 0; i < ar.length; i++) {
                    var arr = ar[i].split("="); arr[1] = arr[1].replace(/'/g, "");
                    //alert(arr[1] + "   " + arr[0] + "   " + n[arr[0]]);
                    if (n[arr[0]] === arr[1])
                        flag = true;
                    else {
                        flag = false;
                        break;
                    }
                }
                return flag;
            });
        //return $.filter(v, this, !true);
    }
});