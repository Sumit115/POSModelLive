
var tranModel = null;
$(document).ready(function () {
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
        $(tranModel.TranDetails).each(function (i, v) {
            v["ProductName"] = parseInt(v.FkProductId);
            v["ModeForm"] = 1;
            v["Delete"] = 'Delete';
        });
        BindGrid('DDT', tranModel.TranDetails);
    }

    else {
        BindGrid('DDT', []);
    }
}

function BindGrid(GridId, data) {

    $("#" + GridId).empty();
    Common.Grid(tranModel.ExtProperties.FKFormID, "dtl", function (s) {
        var ProductList = JSON.parse($("#hdProductList").val());
        var ProductLotList = [];
        cg = new coGrid("#" + GridId);
        UDI = cg;
        cg.setColumnHeading(s.ColumnHeading);
        cg.setColumnWidthPer(s.ColumnWidthPer, 1200);
        cg.setColumnFields(s.ColumnFields);
        cg.setAlign(s.Align);
        cg.defaultHeight = "300px";
        cg._MinRows = 50;
        cg.setIdProperty("SrNo");
        cg.setCtrlType(s.setCtrlType);
        cg.setOptionArray(0, ProductList, "ProductName", false, "Product", "PkProductId", "1");
        cg.setOptionArray(9, ProductLotList, "Batch", false, "Batch", "PkLotId", "1");
        cg.setOptionArray(10, ProductLotList, "Color", false, "Color", "PkLotId", "1");
        var obdt = cg.populateDataFromJson({
            srcData: data,
            mapData: [{ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" },
            { data: ProductLotList, textColumn: "Batch_Text", srcValueColumn: "Batch_Text", destValueColumn: "PkLotId", destTextColumn: "Batch" },
            { data: ProductLotList, textColumn: "Color_Text", srcValueColumn: "Color_Text", destValueColumn: "PkLotId", destTextColumn: "Color" },
            ]
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
            if (args.cell != undefined && args.cell > 0) {
                var field = cg.columns[args.cell].field;

                if (!Common.isNullOrEmpty(args.item["ProductName_Text"])) {
                    if (field == "Batch" || field == "Color") {
                        console.log(args.item["Batch"]);
                        var FkProductId = Common.isNullOrEmpty(args.item["ProductName"]) ? 0 : parseFloat(args.item["ProductName"]);
                        var Batch = args.item["Batch"];
                        var Color = args.item["Color"];


                        Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
                            Handler.hide();


                            // cg.setOptionArray(args.cell, res, "Color", false, "Color", "Color", "1");
                            cg.setOptionArray(9, res, "Batch_Text", false, "Batch", "PkLotId", "1");
                            cg.setOptionArray(10, res, "Color_Text", false, "Color", "PkLotId", "1");
                            //  cg.setOptionArray(0, ProductList, "ProductName", false, "Product", "PkProductId", "1");
                            //  cg.outGrid.gotoCell(args.row, args.cell, true)



                        });

                    }
                }
                else {
                    alert("Select Product Frist");

                    cg_ClearRow(args)
                    cg.outGrid.gotoCell(args.row, 0, true)
                }

            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cg.columns[args.cell].field;
                if (field == "ProductName_Text") {
                    var FkProductId = Common.isNullOrEmpty(args.item["ProductName"]) ? 0 : parseFloat(args.item["ProductName"]);

                    var data = cg.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.mode != 2); });

                    if (data.length <= 0) {
                        ColumnChange(args, args.row, "Product");
                    }
                    else {
                        alert("Product Already Add In List");
                        cg_ClearRow(args);
                        return false;
                    }
                }
                else if (field == "Qty") {
                    ColumnChange(args, args.row, "Qty");
                }
                else if (field == "TradeDisc") {
                    ColumnChange(args, args.row, "TradeDisc");
                }
                else if (field == "Batch") {
                    var FkLotId = Common.isNullOrEmpty(args.item["Batch_Text"]) ? 0 : parseFloat(args.item["Batch_Text"]);
                    if (FkLotId > 0 || TranType == "Purchase") {
                        args.item["FkLotId"] = FkLotId > 0 ? FkLotId : 0;
                        cg.updateRefreshDataRow(args.row);
                        ColumnChange(args, args.row, "Batch");
                    } else {
                        args.item["Batch_Text"] = "";
                        args.item["Batch"] = "";
                        cg.updateRefreshDataRow(args.row);
                    }
                }
                else if (field == "Color") {
                    var FkLotId = Common.isNullOrEmpty(args.item["Color_Text"]) ? 0 : parseFloat(args.item["Color_Text"]);
                    if (FkLotId > 0 || TranType == "Purchase") {
                        args.item["FkLotId"] = FkLotId > 0 ? FkLotId : 0;
                        cg.updateRefreshDataRow(args.row);
                        ColumnChange(args, args.row, "Color");

                    } else {
                        args.item["Color_Text"] = "";
                        args.item["Color"] = "";
                        cg.updateRefreshDataRow(args.row);
                    }
                }


                //cg.updateRefreshDataRow(args.row);
                //cg.updateAndRefreshTotal();

            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cg.columns[args.cell].field;

                var PkProductId = args.grid.getDataItem(args.row)["PkProductId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];


            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onContextMenu.subscribe(function (e, args) {

            e.preventDefault();
            var j = cg.outGrid.getCellFromEvent(e);


            // if (j.cell == 1 || j.cell == 2) {
            $("#contextMenu")
                .data("row", j.row)
                .css("top", e.pageY - 90)
                .css("left", e.pageX - 60)
                .show();
            // }

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
    args.item["FkProductId"] = 0;
    args.item["ProductName_Text"] = "";
    args.item["Product"] = 0;
    args.item["MRP"] = "";
    args.item["Rate"] = "";
    args.item["Qty"] = "";
    args.item["FreeQty"] = "";
    args.item["GrossAmt"] = "";
    args.item["GstRate"] = "";
    args.item["GstAmt"] = "";
    args.item["TradeDisc"] = "";
    args.item["TradeDiscAmt"] = "";
    args.item["TradeDiscType"] = "";
    args.item["NetAmt"] = "";
    args.item["Delete"] = '';
    args.item["Batch"] = "";
    args.item["Batch_Text"] = "";
    args.item["Batch_Text"] = "";
    args.item["Batch"] = "";
    args.item["SaleRate"] = "";
    args.item["MfgDate"] = "";
    cg.updateRefreshDataRow(args.row);
}

function ColumnChange(args, rowIndex, fieldName) {

    tranModel.TranDetails = GetDataFromGrid();

    if (tranModel.TranDetails.length > 0) {
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'ColumnChange',
            data: { model: tranModel, rowIndex: rowIndex, fieldName: fieldName },
            datatype: "json",
            success: function (res) {

                if (res.status == "success") {
                    tranModel = res.data;
                    setFooterData(tranModel)
                    setGridRowData(args, tranModel.TranDetails, rowIndex, fieldName);

                }
                else
                    alert(res.msg);
            }
        });
    }
}

function FooterChange(fieldName) {
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'FooterChange',
        data: { model: tranModel, fieldName: fieldName },
        datatype: "json", success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;

                if (fieldName == "CashDiscType" || fieldName == "CashDiscount") {
                    $(tranModel.TranDetails).each(function (i, v) {
                        v["ProductName"] = parseInt(v.FkProductId);
                    });
                    BindGrid('DDT', tranModel.TranDetails);
                    //$("#hdData").val(JSON.stringify(tranModel));
                    //Load();
                }

                setFooterData(tranModel)

            }
            else
                alert(res.msg);
        }
    })
}

function setFooterData(data) {
    Common.Set(".trn-footer", data, "");
    return false;
}

function setGridRowData(args, data, rowIndex, fieldName) {

    if (fieldName == 'Delete') {
        args.grid.getDataItem(args.row).ModeForm = 2
    }
    else {
        args.item["PkProductId"] = data[rowIndex].PkProductId;
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
        args.item["Delete"] = 'Delete';

    }
    cg.updateRefreshDataRow(args.row);
    cg.updateAndRefreshTotal();
    return false;
}

function GetDataFromGrid(ifForsave) {

    var array = cg.getData().filter(x => x.FkProductId > 0);
    let number = Math.max.apply(Math, array.map(function (o) { return o.SrNo; }));
    let SrNo = number > 0 ? number : 0;
    var _d = [];
    cg.getData().filter(function (element) {
        if (ifForsave) {
            if (!Handler.isNullOrEmpty(element.ProductName) && !Handler.isNullOrEmpty(element.Qty)) {

                if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                element.FkProductId = parseInt(element.ProductName);
                _d.push(element);
                return element
            }
        }
        else {


            if (!Handler.isNullOrEmpty(element.ProductName)) {
                if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                element.FkProductId = parseInt(element.ProductName);
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
            tranModel.FkPartyId = $('#FkPartyId').val();
             tranModel.EntryDate = $('#EntryDate').val();
            tranModel.GRDate = $('#GRDate').val();
            tranModel.TranDetails = [];
            if (tranModel.FkPartyId > 0) {
                if (tranModel.FKSeriesId > 0) {
                    tranModel.TranDetails = GetDataFromGrid(true);

                    var filteredDetails = tranModel.TranDetails.filter(x => x.ModeForm != 2);
                    if (tranModel.TranDetails.length > 0 && filteredDetails.length > 0) {
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

function setdisablecolumn(cg, cc, hash, index, type) {
    var focjson = {};
    var h = (hash[index] = {});
    var focu = {};

    if (cc["mode"] == 1) {
        h["ProductName_Text"] = "sobc";
        focu["ProductName_Text"] = { "focusable": false };
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