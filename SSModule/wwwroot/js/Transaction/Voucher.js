
var tranModel = null;
var ControllerName = "";
var GridName = "dtl";
var MinRows = 50;
$(document).ready(function () {
    
    ControllerName = $("#hdControllerName").val();

    if (ControllerName == "Voucher") { GridName = "viewdtl"; $("#hdGridName").val('viewdtl'); }
    Common.InputFormat();
    $('#btnServerSave').click(function (e) {
        if ($("#loginform1").valid()) {
            SaveRecord();
        }
        return false;
    });

    Load();

    $(".FotChng").change(function () {
        var fieldName = $(this).attr("id");
        tranModel[fieldName] = $(this).val();
        FooterChange(fieldName)
    });


    $(".trn").change(function () {
        var fieldName = $(this).attr("id");
        tranModel[fieldName] = $(this).val();
    });



});

function Load() {
    
    var PkId = $("#PkId").val();
    tranModel = JSON.parse($("#hdData").val());
    if (PkId > 0) {
        //$(tranModel.VoucherDetails).each(function (i, v) {
        //    v["AccountName"] = parseInt(v.FkAccountId);
        //    v["ModeForm"] = 1;
        //    v["Delete"] = 'Delete';
        //});
        if (ControllerName == "Voucher") {
            MinRows = tranModel.VoucherDetails.length;
        }
         BindGrid('DDT', tranModel.VoucherDetails);
    }

    else {
        BindGrid('DDT', []);

    }
}

function BindGrid(GridId, data) {

    $("#" + GridId).empty();
    Common.Grid(tranModel.ExtProperties.FKFormID, GridName, function (s) {
        var AccountList = JSON.parse($("#hdAccountList").val());
        cg = new coGrid("#" + GridId);
        UDI = cg;
        cg.setColumnHeading(s.ColumnHeading);
        cg.setColumnWidthPer(s.ColumnWidthPer, 1200);
        cg.setColumnFields(s.ColumnFields);
        cg.setAlign(s.Align);
        cg.defaultHeight = "300px";
        cg._MinRows = MinRows;
        cg.setIdProperty("SrNo");
        cg.setCtrlType(s.setCtrlType);
        
        var f = s.ColumnFields.split('~');
        var s = s.setCtrlType.split('~');
        var arrmapData = []
        var DrpIndex = {};

        for (kk = 0; kk < s.length; kk++) {
            var sl = s[kk];
            var fl = f[kk];
            if (sl == "C" || sl == "L") {
                DrpIndex[fl] = kk;
                switch (fl) {
                    case "AccountName_Text":
                        cg.setOptionArray(kk, AccountList, "AccountName", false, "Account", "PkAccountId", "1");
                        arrmapData.push({ data: AccountList, textColumn: "AccountName_Text", srcValueColumn: "AccountName_Text", destValueColumn: "PkAccountId", destTextColumn: "Account" });
                        break

                }
            }
        }
        var obdt = cg.populateDataFromJson({
            srcData: data,
            mapData: arrmapData
        });
        cg.applyAddNewRow();
        cg.bind(data);
        /*---------------    ---------------   ---------------   ---------------*/
        //var _data = cg.data.filter(function (el) {
        //    return el.PkId > 0;
        //});
        var filterhash = []
        for (var i = 0; i < data.length; i++) {
            var cc = cg.data[i];

            setdisablecolumn(cg, cc, filterhash, i, 0);
        }
        cg.outGrid.setCellCssStyles("trialRows", filterhash);
        /*---------------    ---------------   ---------------   ---------------*/
        cg.dataView.getItemMetadata = function (row) {
            var cc = cg.outGrid.getDataItem(row);
            if (cc === undefined)
                return true;
            else {
                var hash = {};
                var json = setdisablecolumn(cg, cc, hash, row, 0);
                cg.outGrid.setCellCssStyles("tRow" + String(row), hash);
                if (json != {})
                    return json;
            }
        }
        /*---------------    ---------------   ---------------   ---------------*/
        cg.outGrid.onBeforeEditCell.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cg.columns[args.cell].field;

                if (field != "AccountName_Text" && Common.isNullOrEmpty(args.item["AccountName_Text"])) {
                    alert("Select Account Frist");
                    cg_ClearRow(args)
                    cg.outGrid.gotoCell(args.row, DrpIndex["AccountName_Text"], true);
                }

            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cg.columns[args.cell].field;
                if (field == "AccountName_Text") {

                    var FkAccountId = Common.isNullOrEmpty(args.item["AccountName"]) ? 0 : parseFloat(args.item["AccountName"]);
                    //var data = cg.getData().filter(function (element) { return (element.FkAccountId == FkAccountId && element.mode != 2); });
                    //if (data.length <= 0) {

                    ColumnChange(args, args.row, "Account");
                    //}
                    //else {
                    //    alert("Account Already Add In List");
                    //    cg_ClearRow(args);
                    //    return false;
                    //}

                }
                else if (field == "CreditAmt") {
                    ColumnChange(args, args.row, "Credit");
                }
                else if (field == "DebitAmt") {
                    ColumnChange(args, args.row, "Debit");
                }


            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cg.columns[args.cell].field;

                var PkAccountId = args.grid.getDataItem(args.row)["PkAccountId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];


            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onContextMenu.subscribe(function (e, args) {

            e.preventDefault();
            var j = cg.outGrid.getCellFromEvent(e);
            $("#contextMenu")
                .data("row", j.row)
                .css("top", e.pageY - 90)
                .css("left", e.pageX - 60)
                .show();
            $("body").one("click", function () {
                $("#contextMenu").hide();
            });
        });
        $("#contextMenu").click(function (e) {
            if (!$(e.target).is("li")) {
                return;
            }
            if (!UDI.outGrid.getEditorLock().commitCurrentEdit()) {
                return;
            }

            var row = $(this).data("row");
            var command = $(e.target).attr("data");
            if (command == "EditColumn") {
                Common.GridColSetup(tranModel.ExtProperties.FKFormID, '', function () {
                    var _dtl = GetDataFromGrid();
                    BindGrid('DDT', _dtl);
                });
            }

        });

        //---------------    ---------------   ---------------   ---------------/
    });
}

function cg_ClearRow(args) {
    args.item["PkId"] = 0;
    args.item["mode"] = 0;
    args.item["FkAccountId"] = 0;
    args.item["FKLocationID"] = 0;
    args.item["Account"] = "";
    args.item["SrNo"] = 0;
    args.item["VoucherAmt"] = 0;
    args.item["CreditAmt"] = "";
    args.item["DebitAmt"] = "";
    args.item["CurrentBalance"] = "";
    args.item["Balance"] = 0;
    args.item["AccountGroupName"] = "";
    args.item["AccountName_Text"] = "";
    args.item["Delete"] = 'Delete';
    cg.updateRefreshDataRow(args.row);
}

function ColumnChange(args, rowIndex, fieldName) {

    tranModel.VoucherDetails = GetDataFromGrid();

    if (tranModel.VoucherDetails.length > 0) {
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'VoucherColumnChange',
            data: { model: tranModel, rowIndex: rowIndex, fieldName: fieldName },
            datatype: "json",
            success: function (res) {

                if (res.status == "success") {
                    tranModel = res.data;

                    setGridRowData(args, tranModel.VoucherDetails, rowIndex, fieldName);

                }
                else
                    alert(res.msg);
            }
        });
    }
}

function FooterChange(fieldName) {

    //console.log(tranModel);

    //$.ajax({
    //    type: "POST",
    //    url: Handler.currentPath() + 'FooterChange',
    //    data: { model: tranModel, fieldName: fieldName },
    //    datatype: "json", success: function (res) {
    //        console.log(res);
    //        if (res.status == "success") {

    //            tranModel = res.data;

    //            //if (fieldName == "CashDiscType" || fieldName == "CashDiscount") {
    //            //    $(tranModel.VoucherDetails).each(function (i, v) {
    //            //        v["AccountName"] = parseInt(v.FkAccountId);
    //            //    });
    //            //    BindGrid('DDT', tranModel.VoucherDetails);
    //            //    //$("#hdData").val(JSON.stringify(tranModel));
    //            //    //Load();
    //            //}

    //            setFooterData(tranModel);
    //            setPaymentDetail(tranModel);

    //        }
    //        else
    //            alert(res.msg);
    //    }
    //})
}



function setGridRowData(args, data, rowIndex, fieldName) {

    if (fieldName == 'Delete') {
        args.grid.getDataItem(args.row).ModeForm = 2
    }
    else {
        args.item["SrNo"] = data[rowIndex].SrNo;
        args.item["FKLocationID"] = data[rowIndex].FKLocationID;
        args.item["VoucherAmt"] = data[rowIndex].VoucherAmt;
        args.item["CreditAmt"] = data[rowIndex].CreditAmt;
        args.item["DebitAmt"] = data[rowIndex].DebitAmt;
        args.item["CurrentBalance"] = data[rowIndex].CurrentBalance;
        args.item["Balance"] = data[rowIndex].Balance;
        args.item["AccountGroupName"] = data[rowIndex].AccountGroupName;
        args.item["AccountName_Text"] = data[rowIndex].AccountName_Text;
        args.item["Delete"] = 'Delete';

    }
    cg.updateRefreshDataRow(args.row);
    cg.updateAndRefreshTotal();
    return false;
}

function GetDataFromGrid(ifForsave) {

    var array = cg.getData().filter(x => x.FkAccountId > 0);
    let number = Math.max.apply(Math, array.map(function (o) { return o.SrNo; }));
    let SrNo = number > 0 ? number : 0;
    var _d = [];
    cg.getData().filter(function (element) {
        if (ifForsave) {
            if (!Handler.isNullOrEmpty(element.AccountName) && !Handler.isNullOrEmpty(element.VoucherAmt)) {

                if (element.FkAccountId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                element.FkAccountId = parseInt(element.AccountName);
                _d.push(element);
                return element
            }
        }
        else {


            if (!Handler.isNullOrEmpty(element.AccountName)) {
                if (element.FkAccountId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                element.FkAccountId = parseInt(element.AccountName);
                _d.push(element);
                return element
            }
        }
    });
    return _d
}

function SaveRecord() {
    Common.Get(".form", "", function (flag, _d) {
        if (flag) {
            tranModel.PkId = $('#PkId').val();
            // tranModel.FkPartyId = $('#FkPartyId').val();
            tranModel.EntryDate = $('#EntryDate').val();
            tranModel.GRDate = $('#GRDate').val();
            tranModel.VoucherDetails = [];

            if (tranModel.FKSeriesId > 0) {
                tranModel.VoucherDetails = GetDataFromGrid(true);

                var filteredDetails = tranModel.VoucherDetails.filter(x => x.ModeForm != 2);
                if (tranModel.VoucherDetails.length > 0 && filteredDetails.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: Handler.currentPath() + 'Create',
                        data: { model: tranModel },
                        datatype: "json",
                        success: function (res) {

                            if (res.status == "success") {
                                alert('Save Successfully..');
                                window.location = Handler.currentPath() + 'List';
                            }
                            else
                                alert(res.msg);
                        }
                    });
                }
                else
                    alert("Insert Valid Account Data..");
            }
            else
                alert("Please Select Series");

        }
        else
            alert("Some Error found.Please Check");
    });
}

function setdisablecolumn(cg, cc, hash, index, type) {
    var focjson = {};
    var h = (hash[index] = {});
    var focu = {};

    if (cc["mode"] == 1) {
        h["AccountName_Text"] = "sobc";
        focu["AccountName_Text"] = { "focusable": false };
    }
    else if (cc["mode"] == 2) {
        for (k = 0; k < cg.columns.length - 2; k++) {
            h[cg.columns[k]["field"]] = "sdbc";
            focu[cg.columns[k]["field"]] = { "focusable": false };
        }
    }
    focjson["columns"] = focu;
    return focjson;
}

function setParty() {

    var FkPartyId = $("#FkPartyId").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetParty',
        data: { model: tranModel, FkPartyId: FkPartyId },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $('#FkPartyId').val(tranModel.FkPartyId);
                $('#drpFkPartyId').val(tranModel.PartyName);
                $('#PartyGSTN').val(tranModel.PartyGSTN);
                $('#PartyMobile').val(tranModel.PartyMobile);
                $('#PartyAddress').val(tranModel.PartyAddress);
                $('#PartyCredit').val(tranModel.PartyCredit);

                if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                    tranModel.VoucherDetails = [];
                    BindGrid('DDT', tranModel.VoucherDetails);

                }
            }
            else
                alert(res.msg);
        }
    });
}

function setSeries() {
    var FKSeriesId = $("#FKSeriesId").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetSeries',
        data: { model: tranModel, FKSeriesId: FKSeriesId },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $("#FKSeriesId").val(FKSeriesId);
                $('#PartyCredit').val(tranModel.PartyCredit);
            }
            else
                alert(res.msg);
        }
    });
}