
function GridLayout(option) {
    let $Scope = this;
    $Scope._Columns = [];
    var udfCustomJson = [];
    var GridName = option.GridName;
    var isReport = false;
    var ReportName = option.ReportName;
    var Container = "#dvGridLayoutdialog";
    // Call Back
    this.onSave = null;
    var udfCustomXML = "";


    var SetColumn = (Col) => {
        if (isReport) {
            $Scope._Columns = Col.sort(function (a, b) {
                return (a["Caption"] > b["Caption"]) ? 1 : ((a["Caption"] < b["Caption"]) ? -1 : 0);
            });
            $Scope._Columns = jQuery.grep($Scope._Columns, function (value) {
                return value.ExpCol1 === '' || value.ExpCol1 === null;
            })
        }
        else {
            $Scope._Columns = Col.sort(function (a, b) {
                return (a["Index"] > b["Index"]) ? 1 : ((a["Index"] < b["Index"]) ? -1 : 0);
            });
        }
    }


    var Init = () => {
        if (option.Columns !== undefined && option.Columns != null) {
            SetColumn(option.Columns);
        }
    }

    var Event = () => {
        debugger;
        var ColCheckLenght = $("#SGLBody table tbody tr input[type='checkbox']:checked:visible").length;
        if ($Scope._Columns.filter(x => x.Name == "PKID").length == 0) {
            if (ColCheckLenght == $Scope._Columns.length) {
                $("#SGLBody table thead tr input[type='checkbox']").prop('checked', true);
            }
        } else if (ColCheckLenght == ($Scope._Columns.length - 1)) {
            $("#SGLBody table thead tr input[type='checkbox']").prop('checked', true);
        }
        $(Container + " input[type='number']").focusin(function (e) {
            $(this).select();
        }).focusout(function (e) {
            var val = $(this).val();
            if (val !== '' && parseInt(val) < 100) {
                alert('Width should be greater than 99.');
                $(this).val("100");
                e.preventDefault(e);
                return false;
            }
        });

        // Check Box Up and Down in Table
        $(Container + "  input[type='checkbox']").off('keydown').on("keydown", function (e) {
            var ind = $(this).parents("td").index();
            var $tr = $(this).parents("td").parents("tr");
            if (e.keyCode === 40) {
                $tr.next('tr').children('td:nth-child(' + (ind + 1) + ')').children('input[type="checkbox"]').focus();
                e.preventDefault();
            }
            else if (e.keyCode === 38) {
                $tr.prev('tr').children('td:nth-child(' + (ind + 1) + ')').children('input[type="checkbox"]').focus();
                e.preventDefault();
            }
            return true;
        });

        $("#SGLSave").off('click').on("click", function () {

            UdfColumnValidation("");
        })

        $("[name='SGLSearch']").off('keyup').on("keyup", function () {
            var value = $(this).val().toLowerCase();
            var $row;
            $("#tblSetGridLayout tbody tr").each(function (ind) {
                $row = $(this);
                var id = $row.find("td:first").html().toLowerCase();
                if (id.trim() == "pkid" || id.indexOf(value) !== 0) {
                    $(this).hide();
                }
                else {
                    $(this).show();
                }
            })
        });

        $("#spnSGLSetForAllUser").off('click').on("click", function (e) {
            RestoreGrid("alluser");
        });

        $("#spnSGLRestoreCustomDef").off('click').on("click", function (e) {
            ShowAuthPassword('sgl', 'RestoreGrid("custom")');
        });
        $("#spnSGLRestoreSystemDef").off('click').on("click", function (e) {
            ShowAuthPassword('sgl', 'RestoreSGL(false)');
        });


    }

    this.Bind = () => {
        $("[name='SGLSearch']").val('');
        var flag = GenrateTable();
        UdfDropdown();
        $.each(udfCustomJson, function (index, item) {
            var tempIndex = index + 1;
            $("#txtUdfCol" + tempIndex).val(item.Caption);
            $("#drpUdf" + tempIndex + "Col1 option").filter(function (index) { return $(this).text() === item.ExpCol1; }).attr('selected', 'selected');
            $("#drpUdf" + tempIndex + "Col2 option").filter(function (index) { return $(this).text() === item.ExpCol2; }).attr('selected', 'selected');
            $("#drpUdf" + tempIndex + "Exp").val(item.ExpColFn);
        });

        $(Container + " table").sortable({
            items: 'tr:not(tr:first-child)',
            cursor: 'pointer',
            axis: 'y',
            dropOnEmpty: false,
            start: function (e, ui) {
                ui.item.addClass("selected");
            },
            stop: function (e, ui) {
                ui.item.removeClass("selected");
                $(this).find("tr").each(function (index) {
                    if (index > 0) {
                        $(this).attr("index", index);
                    }
                });
            }
        });

        if ($("#hidIsAdmin").val() !== "true") {
            $("#spnSGLRestoreCustomDef,#spnSGLSetForAllUser").prop("onclick", '').off("click").css("background", "#ccc");
        }
        var popupWidth = 850;
        if (!isReport) {
            $("#dvCustomColumn").hide();
            popupWidth = 550;
            $("#dvSGLSelectColumn").removeClass("col-sm-8");
        }
        $(Container).dialog({
            modal: true,
            title: "Set Grid Layout",
            width: popupWidth
        });

        Event();
        $("#SGLSave").focus();

    }

    var UdfDropdown = () => {
        var objCol1 = $("#drpUdf1Col1");
        objCol1.empty();

        $(".drpudf").val('');
        $(".txtudf").val('');
        objCol1.append($('<option>', { value: '', text: 'Select' }));
        $($Scope._Columns).each(function (i, v) {
            objCol1.append($('<option>', { value: v.DataType, text: v.FieldName }));
        });

        $("#drpUdf1Col2,#drpUdf2Col1,#drpUdf2Col2,#drpUdf3Col1,#drpUdf3Col2").html($("#drpUdf1Col1").html());
    }

    var GenrateTable = () => {
        var rows = [];
        var tempid = '';

        var table = $("<table />").attr({ module: GridName, class: 'table tableheader-fixed', id: 'tblSetGridLayout' });

        var row = $(table).append('<thead>').children('thead').append('<tr />').children('tr');
        row.append($('<th />').html('Column'));

        var div = $('<div />');
        div.attr("class", "form-switch");
        div.append('<span class="d-flex align-items-center gap-2 ps-1"><input style="font-size:13px" class="form-check-input me-2" type="checkbox" role="switch" onclick="SGLCheckAll(this)"/> Visible</span>');

        row.append($('<th />').html(div));

        if (isReport) {
            row.append($('<th />').html('<span> <input type="checkbox" onclick="CheckAll(this)" /> Print</span>'));
            row.append($('<th />').html('<span><input type="checkbox" onclick="CheckAll(this)" /> Mandatory</span>'));
        }
        else {
            row.append($('<th />').html('<span>Width</span>'));
        }

        $(table).append('<tbody />');


        $($Scope._Columns).each(function (i, v) {

            var isVisable = false;
            var isPrint = false;
            var isMandatory = false;
            var isIDColumn = false;
            var show = 'none';
            var colname = '';

            row = $('<tr> </tr>');
            row.attr('index', i);

            if (isReport) {
                colname = String(v.FieldName).toLowerCase();
                row.append($('<td />').html(v.Caption));
                if (v.Print)
                    isPrint = true;
                if (v.IsMandatory)
                    isMandatory = true;
                show = 'table-cell';
                isVisable = v.Visible
            } else {

                colname = String(v.Name).toLowerCase();
                row.append($('<td />').html(v.Caption));
                if (v.Selected)
                    isVisable = true;
            }

            if ((colname.substr(0, 2) === 'fk' && colname.substr((colname.length - 2), 2) === 'id') && colname !== "uniqueid" && colname !== "returntypeid" && colname !== "fklotid" && colname !== "fkaccomid" && colname !== "fkid") {
                row.addClass("hide");
            }

            if (colname.substr(0, 2).toLowerCase() === 'pk' && colname.substr((colname.length - 2), 2).toLowerCase() === 'id') {
                row.attr("style", "display:none;");
            }

            var div = $('<div />');
            div.attr("class", "form-check form-switch");
            var toggle = $('<input />').attr({ class: "form-check-input", type: 'checkbox', role: "switch", checked: isVisable, onclick: "SGLunCheck(this)" })
            div.append(toggle);

            row.append($('<td />').html(div));
            row.append($('<td />').html($('<input />').attr({ type: 'checkbox', checked: isPrint, onclick: "SGLunCheck(this)" })).css("display", show));
            row.append($('<td />').html($('<input />').attr({ type: 'checkbox', checked: isMandatory, onclick: "SGLunCheck(this)" })).css("display", show));

            if (!isReport) {
                row.append($('<td />').html($('<input />').attr({ type: 'number', value: v.Width, class: "txt-sgl-width form-control" })));
            }

            if (v.Condition !== undefined && v.Condition.indexOf('N') != -1) {
                row.addClass("hide");
            }

            if (GridName != "IndexGrid" && (ReportName != "PendingPurchaseBill" && ReportName != "PendingSalesBill")) {
                if ($("#hidTranModel").val() !== undefined) {
                    if (v.Visible === false) {
                        row.css("display", "none");
                        row.addClass("hide");
                    }
                }
            }
            rows.push(row);
        });


        $(table).append(rows);

        $("#SGLBody").html(table);
        $('#dvSGLRoleList').on('shown.bs.modal', function () {
            $(this).off('focusin');
            $(this).off('focusout');
        });

        return true;
    }

    var UdfColumnValidation = (type) => {
        if (Validation()) {
            SaveGridLayout(type);
        }
    }

    var Validation = () => {
        var ColXml = '';
        if ($Scope._Columns.length > 0) {
            for (var i = 1; i <= 3; i++) {
                if ($("#txtUdfCol" + i).val() !== "") {
                    if ($("#drpUdf" + i + "Col1").val() === "" || $("#drpUdf" + i + "Exp").val() === "" || $("#drpUdf" + i + "Col2").val() === "") {
                        Alertdialog('Please select user define column expression');
                        return false;
                    }
                    else if ($("#drpUdf" + i + "Col1").val() !== $("#drpUdf" + i + "Col2").val()) {
                        Alertdialog('Please check  user define column datatype not match.');
                        return false;
                    }
                    else {

                        if (ColXml !== '') {
                            ColXml += ",";
                        }
                        ColXml += '{"FieldName":"udfCol' + i + '",';
                        ColXml += '"Caption":"' + $("#txtUdfCol" + i).val().replace('&', '&amp') + '",';
                        ColXml += '"ColumnWidth":200,';
                        ColXml += '"Visible":true,';
                        ColXml += '"isUdfCol":true,';
                        ColXml += '"Print":true,';
                        ColXml += '"ColumnType":"",';
                        ColXml += '"DataType":"' + $("#drpUdf" + i + "Col1").val() + '",';
                        ColXml += '"ColumnSeq":0,';
                        ColXml += '"ShowTotal":false,';
                        ColXml += '"ShowGrandTotal":false,';
                        ColXml += '"AxisForGraph":"",';
                        ColXml += '"ExpCol1":"' + $("#drpUdf" + i + "Col1 option:selected").text() + '",';
                        ColXml += '"ExpCol2":"' + $("#drpUdf" + i + "Col2 option:selected").text() + '",';
                        ColXml += '"ExpColFn":"' + $("#drpUdf" + i + "Exp").val() + '"}';
                    }
                }
            }
        }
        if (ColXml !== '')
            ColXml = "[" + ColXml + "]";

        udfCustomXML = ColXml;
        $("[name='SGLSearch']").val('');
        return true;
    }

    var SaveGridLayout = (type) => {

        SGL.Type = type;
        Save();
    }


    var Save = () => {
        $("#txtSGLSearch").val('');
        debugger;
        var ColArr = JSON.parse(JSON.stringify($Scope._Columns));
        var IsAnySelect = false;
        var SGLSize = parseInt($("#hidSGLSize").val());
        var TotalSelectedColumn = 0;
        var totalcol = $("#tblSetGridLayout tbody  tr").length;
        $("#tblSetGridLayout tbody  tr").each(function (index, item) {
            var ColName = $(item).find("td:first").html();
            var cnt = $Scope._Columns.length;
            /* var isVisable = $(item).find("input[type=checkbox]").eq(0).prop('checked');*/
            var isVisable = $(item).find("input[type=checkbox]").is(":checked");
            var colWidth = 0;
            var ind = -1;
            if (isReport) {
                var isPrint = $(item).find("input[type=checkbox]").eq(1).prop('checked');
                var isMandatory = $(item).find("input[type=checkbox]").eq(2).prop('checked');
                ind = $Scope._Columns.map(function (d) { return d['Caption']; }).indexOf(ColName);
            } else {
                debugger;
                ind = $Scope._Columns.map(function (d) { return d['Caption']; }).indexOf(ColName);
                colWidth = $(item).find(".txt-sgl-width").val();
                if (!colWidth) {
                    colWidth = "0";
                }
            }
            if (ColName.toLowerCase() == "pkid") {
                isVisable = false;
            }
            if (isVisable)
                TotalSelectedColumn++;

            if (!IsAnySelect && isVisable)
                IsAnySelect = true;
            if (ind !== -1 && index <= cnt) {
                if (isReport) {
                    $Scope._Columns[ind].Visible = isVisable;
                    $Scope._Columns[ind].Print = isPrint;
                    $Scope._Columns[ind].isMandatory = isMandatory;
                }
                else {
                    $Scope._Columns[ind].Selected = isVisable;
                    $Scope._Columns[ind].Index = index;
                    $Scope._Columns[ind].Width = colWidth;
                }
            }
        });
        if (totalcol > 0) {
            if (!IsAnySelect) {
                Alertdialog("Please select atleast one column");
                $Scope._Columns = ColArr;
                return false;
            }
            if (TotalSelectedColumn > SGLSize) {
                AlertConfirm("You have exceeded column selection limit. Please select under " + SGLSize, function () {
                    $Scope._Columns = JSON.parse($("#hidColumnString" + GroupFilter.CurrentLavel).val());
                });
                return false;
            }
        }

        var SetAsDefault = false;
        var RoleFilter = '';
        if (SGL.Type !== '') {
            SetAsDefault = true;
            RoleFilter = $('#hidSGLRoleList').val();
        }
        AlertConfirm("Do you want to save Grid Layout.", function (res) {
            if (res) {

                var data = {
                    ColumnList: JSON.stringify($Scope._Columns),
                    GridName: GridName,
                    ReportName: ReportName,
                    SetAsDefault: SetAsDefault,
                    RoleFilter: RoleFilter,
                    udfCustomXML: udfCustomXML,
                    isReport: isReport,
                    type: SGL.Type
                };

                $.ajax({
                    url: Handler.Domain + 'common/rptSaveGridLayout',
                    data: data,
                    method: 'POST',
                    beforeSend: Handler.loader,
                    async: false,
                    success: function (result) {
                        Handler.hide();
                        if (result.Status == "success") {
                            SetColumn($Scope._Columns);

                            $("#dvGridLayoutdialog").dialog('close');
                            if (typeof ($Scope.onSave) == "function") {
                                $Scope.onSave();
                            }
                            else {
                                Alertdialog('Changes will be reflected after page refresh.', function () {
                                    window.location.reload();
                                });
                            }
                        } else {
                            Alertdialog(result.Message);
                        }
                    }, error: function (request, status, error) {
                        console.log('SaveGridLayout->' + request.responseText);
                        Handler.hide()
                    }
                });
            }
        });
    }

    this.SaveLayoutByColstring = (collist) => {
        AlertConfirm("Do you want to save Grid Layout.", function (res) {
            if (res) {
                var data = {
                    ColumnList: JSON.stringify(collist),
                    GridName: GridName,
                    ReportName: ReportName,
                    SetAsDefault: false,
                    type: SGL.Type
                };

                $.ajax({
                    url: Handler.Domain + 'common/rptSaveGridLayout',
                    data: data,
                    method: 'POST',
                    beforeSend: Handler.loader,
                    async: false,
                    success: function (result) {
                        debugger;
                        Handler.hide()
                        if (result.Status == "success") {
                            Alertdialog('Changes will be reflected after page refresh.', function () {
                                window.location.reload();
                            });
                            //SetColumn(collist);
                            //if (typeof($Scope.onSave) == "function") {
                            //    $Scope.onSave();
                            //}
                            //else {

                            //}
                        } else {
                            Alertdialog(result.Message);
                        }
                    }, error: function (request, status, error) {
                        console.log('SaveGridLayout->' + request.responseText);
                        Handler.hide()
                    }
                });
            }
        });
    }

    this.Export = (FileName) => {
        console.log($Scope._Columns);
        if ($Scope._Columns != null && $Scope._Columns.length > 0) {

            const blob = new Blob([JSON.stringify($Scope._Columns)], { type: 'application/json' });
            const a = document.createElement('a');
            a.href = URL.createObjectURL(blob);
            a.download = FileName + 'dgName.json';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    }

    this.Import = (ColList) => {
        $("#GridLayoutImport").off("change").on("change", function (input) {
            var input = $(this);
            const file = input[0].files[0];
            if (!file) {
                alert('No file selected.');
                return;
            }
            if (file.type !== 'application/json' && !file.name.endsWith('.json')) {
                alert('Please select a valid JSON file.');
                return;
            }
            const reader = new FileReader();
            reader.onload = function (e) {
                try {
                    var HidColData = ColList
                    const jsonData = JSON.parse(e.target.result);
                    if (compareJsonArray(jsonData, HidColData)) {
                        $Scope.SaveLayoutByColstring(jsonData);
                    } else {
                        alert("InValid Json");
                    }
                } catch (err) {
                    alert('Failed to parse JSON file.');
                }
            };
            reader.readAsText(file);
        });

        $("#GridLayoutImport").trigger("click");
    }

    //type = alluser,custom
    var RestoreGrid = (type) => {

        BindRole(type);
        $('#btnSGLRoleOk').off('click').on('click', function () {
            GetSelectedRole(type);
        });

        $('#btnSGLRoleCancel').off('click').on('click', function () {
            HideModel('dvSGLRoleList');
        });
    }


    var BindRole = (type) => {
        $.ajax({
            url: Handler.Domain + 'common/GetRoleList',
            beforeSend: Handler.loader,
            success: function (result) {
                var hidid = '';
                var rows = []; var row;
                var table = $("<table />").attr({ class: 'table tableheader-fixed', id: 'tblRoleFilter' });
                row = $(table).append('<thead>').children('thead').append('<tr />').children('tr');

                var vhtml = '<div class="mst-colum-search-ui no-dropdown">';
                vhtml += '<input class="form-control filter-search" type="text" id="srchRole" placeholder="Search...">';
                vhtml += '<button class="dropdown-toggle"><i class="bi bi-search"></i></button>';
                vhtml += '</div>';

                var div = $('<div />');
                if (type == "alluser") {
                    div.attr("class", "form-switch");
                    div.append('<span class="d-flex align-items-center gap-2 ps-1"><input style="font-size:13px" class="form-check-input me-2" type="checkbox" role="switch" onclick="CheckAll(this)"/></span>');
                }

                var objevent = "";
                if (type == "alluser") {
                    objevent = "unCheck(this)";
                } else {
                    objevent = "unCustomCheck(this)";
                }

                row.append($('<th />').html(div));

                row.append($('<th />').html(vhtml));

                row = $(table).append('<tbody class="invoicegrid-scroll">').children('tbody').append('<tr />').children('tr');
                row = $('<tr> </tr>');
                var div = $('<div />');
                div.attr("class", "form-check form-switch");

                var toggle = $('<input />').attr({ class: "form-check-input", type: 'checkbox', role: "switch", hid: -1, checked: true, onclick: objevent })
                div.append(toggle);

                row.append($('<td />').html(div));
                //row.append($('<td />').html($('<input />').attr({ type: 'checkbox', hid: -1, checked: true })));
                row.append($('<td />').html('Default For All Users'));
                rows.push(row);
                hidid = '-1';

                $.each(result, function (index, item) {
                    row = $('<tr> </tr>');
                    var div = $('<div />');
                    div.attr("class", "form-check form-switch");
                    var toggle = "";
                    var event = "";
                    toggle = $('<input />').attr({ class: "form-check-input", type: 'checkbox', hid: item['PKID'], role: "switch", checked: false, onclick: objevent });
                    div.append(toggle);

                    row.append($('<td />').html(div));
                    /* row.append($('<td />').html($('<input />').attr({ type: 'checkbox', hid: item['PKID'], checked: false, onclick: "unCheck(this)" })));*/
                    row.append($('<td />').html(item['RoleName']));
                    rows.push(row);
                });

                $(table).append(rows);
                $("#dvRoleFilter").html(table);

                $('#srchRole').on('keyup', function () {
                    var searchTerm = $(this).val().toLowerCase();
                    $('#tblRoleFilter tbody tr').each(function () {
                        var rowText = $(this).text().toLowerCase();
                        if (rowText.indexOf(searchTerm) !== -1) {
                            $(this).show();
                        } else {
                            $(this).hide();
                        }
                    });
                });

                $("#hidSGLRoleList").val('');
                $("#dvSGLRoleList").modal('show');
                $('.ui-dialog-titlebar-close').click();
                if (type == "custom") {
                    $("#tblRoleFilter").find('input[type="checkbox"]').not('input[type="checkbox"]:checked').prop('disabled', true);
                }
                Handler.hide();
            }, error: function () {
                Handler.hide()
            }
        });
    }

    var compareJsonArray = (arr1, arr2) => {
        let Temp = JSON.parse(JSON.stringify(arr1));

        Temp.sort(function (a, b) {
            return a.Name.localeCompare(b.Name);
        });
        arr2.sort(function (a, b) {
            return a.Name.localeCompare(b.Name);
        });
        if (Temp.length !== arr2.length) {
            return false;
        }
        for (let i = 0; i < Temp.length; i++) {
            if (Temp[i].Name !== arr2[i].Name) {
                return false;
            }
        }
        return true;
    }


    function ShowAuthPassword(mdl, evnt) {
        $("#GridLayoutAuthPassword").off("keypress").on("keypress", function (e) {
            if (e.keyCode === 13) {
                ValidateAuthPassword();
            }
        });
        $("#btnGridLayoutAuth").off("click").on("click", function () {
            ValidateAuthPassword()
        });
        $("#hidGridLayoutAuthPassword").val(mdl);
        $("#hidGridLayoutAuthEvent").val(evnt);
        $("#GridLayoutAuthPassword").val('');
        $("#dvGridLayoutAuth").dialog({
            modal: true,
            width: 300,
            height: 170,
        });
    }

    var RestoreSGL = (isCustom) => {

        var RoleList = '';
        if (isCustom)
            RoleList = $("#hidSGLRoleList").val();

        var data = {
            GridName: GridName,
            ReportName: ReportName,
            isCustom: isCustom,
            RoleList: RoleList,
            isReport: isReport
        };

        $.ajax({
            url: Handler.Domain + 'common/RestoreGridLayout',
            method: 'POST',
            beforeSend: Handler.loader,
            data: data,
            success: function (result) {

                Handler.hide()
                if (result !== null && result !== '') {
                    $("#hidColList").val(result);
                }
                Alertdialog('Changes will be reflected after page refresh.', function () {
                    window.location.reload();
                });
                $("#dvSetGridLayout").dialog('close');
            }, error: function () {
                Handler.hide()
            }
        });

    }

    function ValidateAuthPassword() {
        var data = { pre: $("#hidGridLayoutAuthPassword").val(), password: $("#GridLayoutAuthPassword").val() };
        ShowHideLoader(true);
        $.ajax({
            url: Handler.Domain + 'common/AuthPassword',
            data: data,
            method: 'POST',
            success: function (result) {
                if (result === true) {
                    $("#GridLayoutAuthPassword,#hidGridLayoutAuthPassword").val('');
                    $("#dvGridLayoutAuth").dialog('close');

                    eval($("#hidGridLayoutAuthEvent").val());
                }
                else
                    Alertdialog('Password Not Correct.');

                ShowHideLoader(false);
            }
        });
    }


    var GetSelectedRole = (type) => {
        var tempID = '';
        $('#tblRoleFilter').find("input[type=checkbox]").each(function (i, item) {
            if (i !== 0) {
                if ($(this).prop('checked')) {
                    tempID += ',' + $(this).attr('hid');
                }
            }
        });
        if (tempID.length > 0)
            tempID = tempID.substr(1, tempID.length - 1);

        $('#hidSGLRoleList').val(tempID);
        $(".ui-dialog-titlebar-close").click();

        HideModel('dvSGLRoleList');

        if (type === 'custom')
            RestoreSGL(true);
        else
            SaveGridLayout(type);
    }

    var HideModel = (id) => {
        $("#" + id).modal('hide');
    }
    Init();
}

function SGLCheckAll(ctrl) {
    var ind = $(ctrl).parents("th").index();
    var $table = $(ctrl).parents("table");
    let isChecked = $(ctrl).is(':checked');
    $table.children('tbody').children('tr').children('td:nth-child(' + (ind + 1) + ')').find('input[type="checkbox"]').prop('checked', isChecked);
}

function unCustomCheck(ctrl) {
    //var ind = $(ctrl).parents("th").index();
    var $table = $(ctrl).parents("table");
    let isChecked = $(ctrl).is(':checked');
    if (isChecked) {
        $($table).find('input[type="checkbox"]').not(ctrl).prop('disabled', true);
    } else {
        $($table).find('input[type="checkbox"]').prop('disabled', false);
    }


}

function SGLunCheck(ctrl) {
    var ind = $(ctrl).parents("td").index();
    var $table = $(ctrl).parents("table");
    let total = $table.children('tbody').children('tr:visible').children('td:nth-child(' + (ind + 1) + ')').find('input[type="checkbox"]').length;
    let checked = $table.children('tbody').children('tr:visible').children('td:nth-child(' + (ind + 1) + ')').find('input[type="checkbox"]:checked').length;
    $table.children('thead').children('tr').children('th:nth-child(' + (ind + 1) + ')').find('input[type="checkbox"]').prop('checked', total === checked);
}
