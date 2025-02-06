var tranModel = null;
var ControllerName = "";
var TranAlias = "";
var ModeFormForEdit = 1;
$(document).ready(function () {

    Common.InputFormat();
    $('#btnServerSave').click(function (e) {
        if ($("#TranForm").valid()) {
            SaveRecord();
        }
        return false;
    });

    ModeFormForEdit = Handler.isNullOrEmpty($("#hdModeFormForEdit").val()) ? 1 : parseInt($("#hdModeFormForEdit").val());
    Common.InputFormat();
    Load();
    tranModel.TrnStatus = Handler.isNullOrEmpty(tranModel.TrnStatus) ? "P" : tranModel.TrnStatus.replace('\u0000', '');
    TranAlias = tranModel.ExtProperties.TranAlias;
    $("#hdFormId").val(tranModel.ExtProperties.FKFormID);
    ControllerName = $("#hdControllerName").val();

});
function Load() {

    var PkId = $("#PkId").val();
    tranModel = JSON.parse($("#hdData").val());
    if (PkId > 0) {
        //var arrayIn = tranModel.TranDetails.filter(x => x.TranType == "I");
        //var arrayOut = tranModel.TranReturnDetails.filter(x => x.TranType == "O"); 
        $(tranModel.TranDetails).each(function (i, v) {
            v["ModeForm"] = ModeFormForEdit;
            v["Delete"] = 'Delete';
        });
        $(tranModel.TranReturnDetails).each(function (i, v) {
            v["ModeForm"] = ModeFormForEdit;
            v["Delete"] = 'Delete';
        });
        BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), tranModel.TranDetails);
        BindGridOut('DDTOut', JSON.parse($("#hdGridOut").val()), tranModel.TranReturnDetails);
    }

    else {

        BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), []);
        BindGridOut('DDTOut', JSON.parse($("#hdGridOut").val()), []);

    }
}





function BindGridIn(GridId, GridStructerJson, data) {

    $("#" + GridId).empty();
    Common.GridFromJson(GridStructerJson, function (s) {
        //var ProductList = (ControllerName == "SalesReturn" || ControllerName == "SalesCrNote") ? [] : JSON.parse($("#hdProductList").val());
        var ProductLotList = [];
        cgIn = new coGrid("#" + GridId);
        cgIn.setColumnHeading(s.ColumnHeading);
        cgIn.setColumnWidthPer(s.ColumnWidthPer, 1200);
        cgIn.setColumnFields(s.ColumnFields);
        cgIn.setAlign(s.Align);
        cgIn.defaultHeight = "300px";
        cgIn._MinRows = 50;
        cgIn.setIdProperty("SrNo");
        cgIn.setCtrlType(s.setCtrlType);

        var f = s.ColumnFields.split('~');
        var s = s.setCtrlType.split('~');
        console.log(s);
        var arrmapData = []
        var DrpIndex = {};

        for (kk = 0; kk < s.length; kk++) {
            var sl = s[kk];
            var fl = f[kk];
            if (sl == "C" || sl == "L" || sl == "CD") {
                DrpIndex[fl] = kk;
                switch (fl) {
                    case "Product":
                        cgIn.columns[kk]["event"] = trandtldropList;
                        cgIn.columns[kk]["fieldval"] = "FkProductId";
                        cgIn.columns[kk]["KeyID"] = "PkProductId";
                        cgIn.columns[kk]["KeyValue"] = "Product";
                        break
                    //case "Batch":
                    //    cgIn.columns[kk]["event"] = trandtldropList;
                    //    cgIn.columns[kk]["fieldval"] = "FkLotId";
                    //    cgIn.columns[kk]["KeyID"] = "PkLotId";
                    //    cgIn.columns[kk]["KeyValue"] = "Batch";
                    //    cgIn.columns[kk]["Keyfield"] = "Batch,Color,MRP,SaleRate,PurchaseRate,TradeRate,DistributionRate";
                    //    cgIn.columns[kk]["RowValue"] = "FkProductId";
                    //    cgIn.columns[kk]["ExtraValue"] = "TranAlias,TranType"; //tranModel.ExtProperties.TranAlias;
                    //    break
                    //case "Color":
                    //    cgIn.columns[kk]["event"] = trandtldropList;
                    //    cgIn.columns[kk]["fieldval"] = "Color";
                    //    cgIn.columns[kk]["KeyID"] = "Color";
                    //    cgIn.columns[kk]["KeyValue"] = "Color";
                    //    cgIn.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
                    //    cgIn.columns[kk]["RowValue"] = "FkProductId";
                    //    cgIn.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias;

                    //    break
                }
            }
        }
        var obdt = cgIn.populateDataFromJson({
            srcData: data,
            mapData: arrmapData
        });
        cgIn.applyAddNewRow();
        cgIn.bind(data);


        /*---------------    ---------------   ---------------   ---------------*/
        cgIn.outGrid.onBeforeEditCell.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cgIn.columns[args.cell].field;

                if (field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
                    alert("Select Product Frist");
                    cg_ClearRow(cgIn, args)
                    cgIn.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
                }
                else {
                    if (field == "Batch" || field == "Color" || field == "Qty") {

                        var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);
                        if (field == "Batch") {
                            Common.ajax(Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct?FkProductId=' + FkProductId, {}, "Please Wait...", function (res) {
                                Handler.hide();
                                cgIn.setOptionArray(DrpIndex["Batch"], res, "Batch", false, "Size", "Size", "1");
                            });
                        }
                        if (field == "Color") {
                            var data = { name: field, pageNo: 1, pageSize: 1000, search: '', RowParam: FkProductId, ExtraParam: "" };

                            $.ajax({
                                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
                                    Handler.hide();
                                    if (res.length > 0)
                                        cgIn.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                                }, error: function (request, status, error) {
                                }
                            });
                            //Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
                            //    Handler.hide();
                            //    cgIn.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                            //});
                        }
                    }
                }
            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cgIn.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cgIn.columns[args.cell].field;
                var FkRecipeId = args.item["FkRecipeId"]

                if (field == "Product") {

                    args.item["SrNo"] = 0;
                    args.item["TranType"] = 'I';
                    ColumnChange(args, args.row, "Product", false);

                }
                else if (field == "Qty") {

                }
                else if (field == "MRP") {

                }
                cgIn.updateRefreshDataRow(args.row);
                cgIn.updateAndRefreshTotal();
                //cgIn.gotoCell(args.row, args.cell + 1);
                args.grid.gotoCell(args.row, args.cell, true)
            }
        });


        //---------------    ---------------   ---------------   ---------------/
        cgIn.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cgIn.columns[args.cell].field;

                var FkProductId = args.grid.getDataItem(args.row)["FkProductId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete", false);

                }

            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cgIn.outGrid.onContextMenu.subscribe(function (e, args) {

            e.preventDefault();
            var j = cgIn.outGrid.getCellFromEvent(e);
            $("#contextMenuIn")
                .data("row", j.row)
                .css("top", e.pageY - 90)
                .css("left", e.pageX - 60)
                .show();
            $("body").one("click", function () {
                $("#contextMenuIn").hide();
            });
        });
        $("#contextMenuIn").click(function (e) {
            if (!$(e.target).is("li")) {
                return;
            }
            if (!cgIn.outGrid.getEditorLock().commitCurrentEdit()) {
                return;
            }

            var row = $(this).data("row");
            var command = $(e.target).attr("data");
            if (command == "EditColumn") {
                Common.GridColSetup(tranModel.ExtProperties.FKFormID, "dtl", function () {
                    //  var _dtl = GetDataFromGrid(false);
                    Common.GridStrucher(tranModel.ExtProperties.FKFormID, "dtl", function (s) {
                        //  BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), tranModel.TranDetails);
                        $("#hdGridIn").val(JSON.stringify(s));
                       BindGridIn('DDTIn', s, tranModel.TranDetails);
                    });
                });
            }

        });

        //---------------    ---------------   ---------------   ---------------/
    });
}
function BindGridOut(GridId, GridStructerJson, data) {

    $("#" + GridId).empty();
    Common.GridFromJson(GridStructerJson, function (s) {
        //var ProductList = (ControllerName == "SalesReturn" || ControllerName == "SalesCrNote") ? [] : JSON.parse($("#hdProductList").val());
        var ProductLotList = [];
        cgOut = new coGrid("#" + GridId);
        cgOut.setColumnHeading(s.ColumnHeading);
        cgOut.setColumnWidthPer(s.ColumnWidthPer, 1200);
        cgOut.setColumnFields(s.ColumnFields);
        cgOut.setAlign(s.Align);
        cgOut.defaultHeight = "300px";
        cgOut._MinRows = 50;
        cgOut.setIdProperty("SrNo");
        cgOut.setCtrlType(s.setCtrlType);

        var f = s.ColumnFields.split('~');
        var s = s.setCtrlType.split('~');
        console.log(s);
        var arrmapData = []
        var DrpIndex = {};

        for (kk = 0; kk < s.length; kk++) {
            var sl = s[kk];
            var fl = f[kk];
            if (sl == "C" || sl == "L" || sl == "CD") {
                DrpIndex[fl] = kk;
                switch (fl) {
                    case "Product":
                        cgOut.columns[kk]["event"] = trandtldropList;
                        cgOut.columns[kk]["fieldval"] = "FkProductId";
                        cgOut.columns[kk]["KeyID"] = "PkProductId";
                        cgOut.columns[kk]["KeyValue"] = "Product";
                        break
                    //case "Color":
                    //    cgOut.columns[kk]["event"] = trandtldropList;
                    //    cgOut.columns[kk]["fieldval"] = "Color";
                    //    cgOut.columns[kk]["KeyID"] = "Color";
                    //    cgOut.columns[kk]["KeyValue"] = "Color";
                    //    cgOut.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
                    //    cgOut.columns[kk]["RowValue"] = "FkProductId";
                    //    cgOut.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias;

                    //    break
                }
            }
        }
        var obdt = cgOut.populateDataFromJson({
            srcData: data,
            mapData: arrmapData
        });
        cgOut.applyAddNewRow();
        cgOut.bind(data);


        /*---------------    ---------------   ---------------   ---------------*/
        cgOut.outGrid.onBeforeEditCell.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cgOut.columns[args.cell].field;

                if (field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
                    alert("Select Product Frist");
                    cg_ClearRow(cgOut, args)
                    cgOut.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
                }
                else {
                    if (field == "Batch" || field == "Color" || field == "Qty") {
                        var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);
                        if (field == "Batch") {
                            Common.ajax(Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct?FkProductId=' + FkProductId, {}, "Please Wait...", function (res) {
                                Handler.hide();
                                cgOut.setOptionArray(DrpIndex["Batch"], res, "Batch", false, "Size", "Size", "1");
                            });
                        }
                        if (field == "Color") {
                            var data = { name: field, pageNo: 1, pageSize: 1000, search: '', RowParam: FkProductId, ExtraParam: "" };

                            $.ajax({
                                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
                                    Handler.hide();
                                    if (res.length > 0)
                                        cgOut.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                                }, error: function (request, status, error) {
                                }
                            });
                            //Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
                            //    Handler.hide();
                            //    cgOut.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                            //});
                        }
                    }
                }
            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cgOut.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cgOut.columns[args.cell].field;
                var FkRecipeId = args.item["FkRecipeId"]

                if (field == "Product") {
                    args.item["SrNo"] = 0;
                    args.item["TranType"] = 'O';
                    ColumnChange(args, args.row, "Product", true);
                }
                else if (field == "Qty") {

                }
                else if (field == "MRP") {

                }
                cgOut.updateRefreshDataRow(args.row);
                cgOut.updateAndRefreshTotal();
                //cgOut.gotoCell(args.row, args.cell + 1);
                args.grid.gotoCell(args.row, args.cell, true)
            }
        });


        //---------------    ---------------   ---------------   ---------------/
        cgOut.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cgOut.columns[args.cell].field;

                var FkProductId = args.grid.getDataItem(args.row)["FkProductId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete",true);

                }

            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cgOut.outGrid.onContextMenu.subscribe(function (e, args) {

            e.preventDefault();
            var j = cgOut.outGrid.getCellFromEvent(e);
            $("#contextMenuOut")
                .data("row", j.row)
                .css("top", e.pageY - 90)
                .css("left", e.pageX - 60)
                .show();
            $("body").one("click", function () {
                $("#contextMenuOut").hide();
            });
        });
        $("#contextMenuOut").click(function (e) {
            if (!$(e.target).is("li")) {
                return;
            }
            if (!cgOut.outGrid.getEditorLock().commitCurrentEdit()) {
                return;
            }

            var row = $(this).data("row");
            var command = $(e.target).attr("data");
            if (command == "EditColumn") {
                
                Common.GridColSetup(tranModel.ExtProperties.FKFormID, "rtn", function () {
                    //  var _dtl = GetDataFromGrid(false);
                    Common.GridStrucher(tranModel.ExtProperties.FKFormID, "rtn", function (s) {
                        //  BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), tranModel.TranDetails);
                        $("#hdGridOut").val(JSON.stringify(s));
                        BindGridOut('DDTOut', s, tranModel.TranReturnDetails);
                    });
                });
            }

        });

        //---------------    ---------------   ---------------   ---------------/
    });
}
function cg_ClearRow(grid, args) {

    //args.item["FkRecipeId"] = "0";
    args.item["SrNo"] = 0;
    args.item["FkProductId"] = 0;
    args.item["ProductName_Text"] = "";
    args.item["TranType"] = "";
    args.item["MRP"] = "";
    args.item["Qty"] = "";
    args.item["Batch"] = "";
    args.item["Color"] = "";
    grid.updateRefreshDataRow(args.row);
}
function trandtldropList(data) {

    var output = []
    $.ajax({
        url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (result) {

            output = result;

        }, error: function (request, status, error) {
        }
    });
    return output;

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

                BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), tranModel.TranDetails);
                BindGridOut('DDTOut', JSON.parse($("#hdGridOut").val()), tranModel.TranReturnDetails);

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
function SaveRecord() {
    Common.Get(".form", "", function (flag, _d) {

        if (flag) {

            tranModel.PkId = $('#PkId').val();
            tranModel.FkPartyId = $('#FkPartyId').val();
            tranModel.EntryDate = $('#EntryDate').val();
            tranModel.GRDate = $('#GRDate').val();
            tranModel.TranDetails = [];

            if (tranModel.FkPartyId > 0) {
                if (tranModel.FKSeriesId > 0) {

                    tranModel.TranDetails = GetDataFromGrid(true, false);
                    tranModel.TranReturnDetails = GetDataFromGrid(true, true);

                    var filteredDetails = tranModel.TranDetails.filter(x => x.ModeForm != 2);
                    if (tranModel.TranDetails.length > 0 && filteredDetails.length > 0) {
                        /* alert('Ok');*/
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
                                else {
                                    alert(res.msg);
                                    tranModel = res.data;
                                    BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), tranModel.TranDetails);
                                    BindGridOut('DDTOut', JSON.parse($("#hdGridOut").val()), tranModel.TranReturnDetails);
                                }
                            }
                        });
                    }
                    else
                        alert("Insert Valid Product Data..");
                }
                else
                    alert("Please Select Series");
            }
            else
                alert("Please Select Party");

        }
        else
            alert("Some Error found.Please Check");
    });
}

function ColumnChange(args, rowIndex, fieldName, IsReturn) {

    tranModel.TranDetails = GetDataFromGrid(false, false);
    tranModel.TranReturnDetails = GetDataFromGrid(false, true);

    if ((IsReturn ? tranModel.TranReturnDetails.length : tranModel.TranDetails.length) > 0) {
        // if (Handler.isNullOrEmpty(tranModel.TranDetails[rowIndex].LinkSrNo) || tranModel.TranDetails[rowIndex].LinkSrNo <= 0 || fieldName == 'Delete') {
        $(".loader").show();
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'ColumnChange',
            data: { model: tranModel, rowIndex: rowIndex, fieldName: fieldName, IsReturn: IsReturn },
            datatype: "json",
            success: function (res) {

                if (res.status == "success") {
                    tranModel = res.data;

                    //if (Handler.isNullOrEmpty(tranModel.TranDetails[rowIndex].PromotionType)) {
                    setGridRowData(args, (IsReturn ? tranModel.TranReturnDetails : tranModel.TranDetails), rowIndex, fieldName, IsReturn);
                    //} else {
                    //BindGrid('DDT', tranModel.TranDetails);
                    //args.grid.gotoCell(args.row, args.cell + 1, true)

                    // }
                }
                else
                    alert(res.msg);

                $(".loader").hide();
            }
        });
        // } else { alert('Promotion Artical Not Update'); BindGrid('DDT', tranModel.TranDetails); }
    }
}
function setGridRowData(args, data, rowIndex, fieldName, IsReturn) {

    if (fieldName == 'Delete') {
        args.grid.getDataItem(args.row).ModeForm = 2
    }
    else {
        if (data[rowIndex] != undefined) {

            args.item["SrNo"] = data[rowIndex].SrNo;
            args.item["PkProductId"] = data[rowIndex].PkProductId;
            args.item["FkProductId"] = data[rowIndex].FkProductId;
            args.item["FkBrandId"] = data[rowIndex].FkBrandId;
            args.item["FKProdCatgId"] = data[rowIndex].FKProdCatgId;
            args.item["FkLotId"] = data[rowIndex].FkLotId;
            args.item["FKOrderID"] = data[rowIndex].FKOrderID;
            args.item["FKOrderSrID"] = data[rowIndex].FKOrderSrID;
            args.item["OrderSrNo"] = data[rowIndex].OrderSrNo;
            args.item["Rate"] = data[rowIndex].Rate;
            args.item["MRP"] = data[rowIndex].MRP;
            args.item["Qty"] = data[rowIndex].Qty;
            args.item["FreeQty"] = data[rowIndex].FreeQty;
            args.item["GrossAmt"] = data[rowIndex].GrossAmt;
            args.item["GstRate"] = data[rowIndex].GstRate;
            args.item["GstAmt"] = data[rowIndex].GstAmt;
            args.item["TradeDisc"] = data[rowIndex].TradeDisc;
            args.item["TradeDiscAmt"] = data[rowIndex].TradeDiscAmt;
            args.item["TradeDiscType"] = data[rowIndex].TradeDiscType;
            args.item["NetAmt"] = data[rowIndex].NetAmt;
            args.item["ModeForm"] = data[rowIndex].ModeForm;
            args.item["Batch"] = data[rowIndex].Batch;
            args.item["Color"] = data[rowIndex].Color;
            args.item["MfgDate"] = data[rowIndex].MfgDate;
            args.item["ExpiryDate"] = data[rowIndex].ExpiryDate;
            args.item["SaleRate"] = data[rowIndex].SaleRate;
            args.item["TradeRate"] = data[rowIndex].TradeRate;
            args.item["DistributionRate"] = data[rowIndex].DistributionRate;
            args.item["InvoiceDate"] = data[rowIndex].InvoiceDate;
            args.item["FKInvoiceID_Text"] = data[rowIndex].FKInvoiceID_Text;
            args.item["FKInvoiceSrID"] = data[rowIndex].FKInvoiceSrID;
            args.item["FKLocationID"] = data[rowIndex].FKLocationID;
            args.item["ReturnTypeID"] = data[rowIndex].ReturnTypeID;
            args.item["TranType"] = data[rowIndex].TranType;
            // args.item["Barcode"] = "Barcode";//data[rowIndex].Barcode;
            args.item["Delete"] = 'Delete';

            var CodingScheme = data[rowIndex].CodingScheme;
            args.item["CodingScheme"] = CodingScheme;
        }
    }
    if (IsReturn) {
        cgOut.updateRefreshDataRow(args.row);
        cgOut.updateAndRefreshTotal();
    } else {
        cgIn.updateRefreshDataRow(args.row);
        cgIn.updateAndRefreshTotal();
    }
    //cg.gotoCell(args.row, args.cell + 1);
    args.grid.gotoCell(args.row, args.cell + 1, true)
    $(".loader").hide();
    return false;
}
function GetDataFromGrid(ifForsave, IsReturn) {
    var _d = [];
    if (IsReturn) {
        var arrayOut = cgOut.getData().filter(x => x.FkProductId > 0);
        let numberOut = Math.max.apply(Math, arrayOut.map(function (o) { return o.SrNo; }));
        let SrNoOut = numberOut > 0 ? numberOut : 1000;

        cgOut.getData().filter(function (element) {

            if (ifForsave) {
                if (!Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

                    if (element.FkProductId > 0 && element.SrNo > 0) { element.SrNo = element.SrNo; }
                    else { SrNoOut++; element.SrNo = SrNoOut; }
                    element.TranType = "O"
                    _d.push(element);

                    return element
                }
            }
            else {
                if (!Handler.isNullOrEmpty(element.Product)) {

                    if (element.FkProductId > 0 && element.SrNo > 0) { element.SrNo = element.SrNo; }
                    else { SrNoOut++; element.SrNo = SrNoOut; }
                    element.TranType = "O"
                    _d.push(element);
                    return element
                }
            }
        });
    }
    else {

        var arrayIn = cgIn.getData().filter(x => x.FkProductId > 0);
        let numberIn = Math.max.apply(Math, arrayIn.map(function (o) { return o.SrNo; }));
        let SrNoIn = numberIn > 0 ? numberIn : 0;

        cgIn.getData().filter(function (element) {

            if (ifForsave) {
                if (!Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

                    if (element.FkProductId > 0 && element.SrNo > 0) { element.SrNo = element.SrNo; }
                    else { SrNoIn++; element.SrNo = SrNoIn; }
                    element.TranType = "I"
                    _d.push(element);

                    return element
                }
            }
            else {

                if (!Handler.isNullOrEmpty(element.Product)) {

                    if (element.FkProductId > 0 && element.SrNo > 0) { element.SrNo = element.SrNo; }
                    else { SrNoIn++; element.SrNo = SrNoIn; }
                    element.TranType = "I"
                    _d.push(element);
                    return element
                }
            }
        });
    }

    return _d
}

