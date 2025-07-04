﻿
var tranModel = null;
var ControllerName = "";
var TranAlias = "";
var ModeFormForEdit = 1;
var cgRtn = null;
var UDIRtn = null;
$(document).ready(function () {
    $('#btnDeleteRecord').hide();
    ModeFormForEdit = Handler.isNullOrEmpty($("#hdModeFormForEdit").val()) ? 1 : parseInt($("#hdModeFormForEdit").val());
    Common.InputFormat();
    ControllerName = $("#hdControllerName").val();
    Load();
    tranModel.TrnStatus = Handler.isNullOrEmpty(tranModel.TrnStatus) ? "P" : tranModel.TrnStatus.replace('\u0000', '');
    TranAlias = tranModel.ExtProperties.TranAlias;
    $("#hdFormId").val(tranModel.ExtProperties.FKFormID);
    $("#hdGridName").val('dtl');
    if ((TranAlias == "SRTN" || TranAlias == "SCRN" || TranAlias == "SORD" || TranAlias == "LORD" || TranAlias == "PORD" || TranAlias == "PINV")) {
        $(".trn-barcode").hide();
    } else { $(".trn-barcode").show(); $("#txtSearchBarcode").focus(); }

    if ((TranAlias == "SORD" || TranAlias == "LORD") && tranModel.PkId > 0) {
        if (tranModel.TrnStatus.trim() == 'P' || tranModel.TrnStatus.trim() == 'C') {
            if (tranModel.TrnStatus.trim() == 'C') {
                $("#btnServerSave").hide();
                $("#btnClose").parent().hide();
                $("#btnOpen").parent().show();
            } else {
                $("#btnClose").parent().show();
                $("#btnOpen").parent().hide();
            }
        }
        else if (tranModel.TrnStatus.trim() == 'I') { $("#btnServerSave").hide(); $("#btnOpen").parent().hide(); }
    } else { $("#btnClose,#btnOpen").parent().hide(); }

    if (ControllerName == 'SalesInvoiceTouch') {
        $("#btnServerBack").attr('href', 'javascript:void(0)')
        $("#btnServerBack").click(function () { location.reload(); });
        var $ul = $('#ul_Category li:first');
        BindCategoryProduct_Touch($($ul).find('a'));
    }
    if (tranModel.ExtProperties.TranType == "S" && TranAlias != "SGRN") {
        $("#btnApplyPromotion").show();
    } else { $("#btnApplyPromotion").hide(); }
    $(document.body).keyup(function (e) {
        if (e.keyCode == 120 && e.ctrlKey) {
            AutoFillLastRecord();
        }
    });
    $('#btnServerSave').click(function (e) {

        if ($("#TranForm").valid()) {
            SaveRecord();
        }
        return false;
    });
    $('#btnApplyPromotion').click(function (e) {

        if ($("#TranForm").valid()) {
            ApplyPromotion()
        }
        return false;
    });


    $(".FotChng").change(function () {
        var fieldName = $(this).attr("id");
        tranModel[fieldName] = $(this).val();
        FooterChange(fieldName)
    });
    $(".ratediscount").change(function () {
        var type = $("#RateDiscountType").val();
        var value = $("#RateDiscountValue").val();
        ApplyRateDiscount(type, value);
        //$("#RateDiscountValue").val('')
    });

    $(".trn").change(function () {
        var fieldName = $(this).attr("id");
        tranModel[fieldName] = $(this).val();
    });
    $(".EwayDtl").change(function () {
        var fieldName = $(this).attr("id").replace('EWayDetail_', '');
        tranModel.EWayDetail[fieldName] = $(this).val();
    });
    $(".paymentDtl").change(function () {

        var fieldName = $(this).attr("id");
        var type = $(this).attr("type");
        if (type == "checkbox") {
            if ($(this).prop('checked') === true) {
                tranModel[fieldName] = true;
            } else {
                tranModel[fieldName] = false;
            }

        } else {
            tranModel[fieldName] = $(this).val();
        }
        //  PaymentDetail();
    });
    $("#txtSearchBarcode").change(function () {

        if (tranModel.FKSeriesId > 0) {
            BarcodeScan($(this).val(), false);
            $(this).val('');
            $(this).focus();
        } else { alert('Select Series'); }
    });
    $("#txtSearchBarcodeReturn").change(function () {

        if (tranModel.FKSeriesId > 0) {
            BarcodeScan($(this).val(), true);
            $(this).val('');
            $(this).focus();
        } else { alert('Select Series'); }
    });
    $("#EntryNo").change(function () {
        DatabyEntryNo();
    });
    $("#PartyMobile").change(function () {
        GetWalkingCustomerDetail($(this).val());
    });
    $('#btnSaveShippDtl').click(function (e) {
        $('.model-ShippingDetails').modal('toggle')
    });
    $("input[name=SameAsBilling]").change(function () {
        if ($(this).prop('checked') == true) {
            $("#ShipingAddress").val($("#PartyAddress").val());
        }
    });
});

function Load() {

    var PkId = $("#PkId").val();
    tranModel = JSON.parse($("#hdData").val());
    TranAlias = tranModel.ExtProperties.TranAlias;

    if (PkId > 0 || tranModel.TranDetails.length > 0) {
        console.clear();
        //console.log(tranModel);
        $(tranModel.TranDetails).each(function (i, v) {
            //  v["Product"] = parseInt(v.FkProductId);

            v["ModeForm"] = ModeFormForEdit;
            if (ModeFormForEdit == 0) {
                v["FKSeriesId"] = tranModel.FKSeriesId;
                v["FKOrderID"] = tranModel.FKOrderID;
                v["FKOrderSrID"] = tranModel.FKOrderSrID;
                v["OrderSrNo"] = v.SrNo;
                v["FkId"] = 0;
            }
            var CodingScheme = v["CodingScheme"];
            if (tranModel.ExtProperties.TranType == "P") {
                if (CodingScheme == 'fixed' || tranModel.TranAlias == "PORD")
                    v["Barcode"] = "";
                else
                    v["Barcode"] = "Barcode";
            }
            else if (tranModel.ExtProperties.TranType == "S") {
                if (CodingScheme == 'Unique')
                    v["Barcode"] = "Barcode";
                else
                    v["Barcode"] = "";
            }
            // v["Barcode"] = 'Barcode';
            v["Delete"] = 'Delete';
        });

        BindGrid('DDT', tranModel.TranDetails);
        if (TranAlias == 'SGRN') {
            $(tranModel.TranReturnDetails).each(function (i, v) {
                //  v["Product"] = parseInt(v.FkProductId);

                v["ModeForm"] = ModeFormForEdit;
                if (ModeFormForEdit == 0) {
                    v["FKSeriesId"] = tranModel.FKSeriesId;
                    v["FKOrderID"] = tranModel.FKOrderID;
                    v["FKOrderSrID"] = tranModel.FKOrderSrID;
                    v["OrderSrNo"] = v.SrNo;
                    v["FkId"] = 0;
                }
                var CodingScheme = v["CodingScheme"];
                if (tranModel.ExtProperties.TranType == "P") {
                    if (CodingScheme == 'fixed' || tranModel.TranAlias == "PORD")
                        v["Barcode"] = "";
                    else
                        v["Barcode"] = "Barcode";
                }
                else if (tranModel.ExtProperties.TranType == "S") {
                    if (CodingScheme == 'Unique')
                        v["Barcode"] = "Barcode";
                    else
                        v["Barcode"] = "";
                }
                // v["Barcode"] = 'Barcode';
                v["Delete"] = 'Delete';
            });
            BindGridReturn('DDTReturn', tranModel.TranReturnDetails);
        }
        setPaymentDetail(tranModel);
    }

    else {

        BindGrid('DDT', []);
        if (TranAlias == 'SGRN')
            BindGridReturn('DDTReturn', []);

    }
}

function BindGrid(GridId, data) {

    $("#" + GridId).empty();
    Common.Grid(tranModel.ExtProperties.FKFormID, "dtl", function (s) {


        if (tranModel.FKOrderID > 0 && tranModel.FKOrderSrID) { s.setCtrlType = s.setCtrlType.replace("CD", "").replace("CD", "") }
        //var ProductList = (ControllerName == "SalesReturn" || ControllerName == "SalesCrNote") ? [] : JSON.parse($("#hdProductList").val());
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

        var f = s.ColumnFields.split('~');
        var s = s.setCtrlType.split('~');
        var arrmapData = []
        var DrpIndex = {};

        for (kk = 0; kk < s.length; kk++) {
            var sl = s[kk];
            var fl = f[kk];
            if (sl == "C" || sl == "L" || sl == "CD") {
                DrpIndex[fl] = kk;
                switch (fl) {
                    case "Product":
                        cg.columns[kk]["event"] = trandtldropList;
                        cg.columns[kk]["fieldval"] = "FkProductId";
                        cg.columns[kk]["KeyID"] = "PKID";
                        cg.columns[kk]["KeyValue"] = "Product";
                        if (aaa = "Salereturn") {
                            cg.columns[kk]["RowValue"] = "InvoiceDate,FKInvoiceID";
                            cg.columns[kk]["ExtraValue"] = "FkpartiId";

                        }
                        //cg.columns[KK]["Keyfield"] = "Product,col";// if any saelected column show then comma seprate column name
                        //cg.setOptionArray(kk, ProductList, "ProductName", false, "Product", "PkProductId", "1");
                        //arrmapData.push({ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" });
                        break
                    case "Batch":
                        if (tranModel.ExtProperties.StockFlag != "I") {
                            cg.columns[kk]["event"] = trandtldropList;
                            cg.columns[kk]["fieldval"] = "FkLotId";
                            cg.columns[kk]["KeyID"] = "PkLotId";
                            cg.columns[kk]["KeyValue"] = "Batch";
                            cg.columns[kk]["Keyfield"] = "Batch,Color,MRP,SaleRate,PurchaseRate,TradeRate,DistributionRate";
                            cg.columns[kk]["RowValue"] = "FkProductId";
                            cg.columns[kk]["ExtraValue"] = "TranAlias,TranType"; //tranModel.ExtProperties.TranAlias;
                        }
                        break
                    case "Color":
                        if (tranModel.ExtProperties.StockFlag != "I") {
                            cg.columns[kk]["event"] = trandtldropList;
                            cg.columns[kk]["fieldval"] = "FkLotId";
                            cg.columns[kk]["KeyID"] = "PkLotId";
                            cg.columns[kk]["KeyValue"] = "Color";
                            cg.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
                            cg.columns[kk]["RowValue"] = "FkProductId,Batch";
                            cg.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias; 
                        }
                        break
                    case "MRP":
                        cg.columns[kk]["event"] = trandtldropList;
                        if (tranModel.ExtProperties.StockFlag == "I") {
                            cg.columns[kk]["fieldval"] = "MRP";
                            cg.columns[kk]["KeyID"] = "MRP";
                            cg.columns[kk]["Keyfield"] = "MRP";

                        } else {
                            cg.columns[kk]["fieldval"] = "FkLotId";
                            cg.columns[kk]["KeyID"] = "PkLotId";
                            cg.columns[kk]["Keyfield"] = "MRP";//,Color,Batch,SaleRate,PurchaseRate";

                        }
                        cg.columns[kk]["KeyValue"] = "MRP";
                        cg.columns[kk]["RowValue"] = "FkProductId,Batch,Color";

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
                var LinkSrNo = args.item["LinkSrNo"];
                if (tranModel.FKSeriesId > 0) {
                    if (Handler.isNullOrEmpty(LinkSrNo) || LinkSrNo <= 0) {

                        if (field != "InvoiceDate" && field != "FKInvoiceID_Text" && field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
                            alert("Select Product Frist");
                            cg_ClearRow(args)
                            cg.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
                        }
                        else if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                            if (tranModel.FkPartyId > 0) {
                                if (field == "Product") {
                                    var InvoiceDate = args.item["InvoiceDate"];
                                    var FKInvoiceID = args.item["FKInvoiceID"];
                                    Common.ajax(Handler.currentPath() + "InvoiceProductList?FkPartyId=" + tranModel.FkPartyId + "&FKInvoiceID=" + FKInvoiceID + "&InvoiceDate=" + InvoiceDate + "", {}, "Please Wait...", function (res) {
                                        Handler.hide();
                                        ProductList = res;
                                        $("#hdProductList").val(JSON.stringify(res));
                                        cg.setOptionArray(DrpIndex["ProductName_Text"], res, "ProductName", false, "Product", "InvoiceSrNo", "1");

                                    });
                                }
                                else if (field == "FKInvoiceID_Text") {
                                    var InvoiceDate = args.item["InvoiceDate"];
                                    Common.ajax(Handler.currentPath() + "InvoiceList?FkPartyId=" + tranModel.FkPartyId + "&InvoiceDate=" + InvoiceDate + "", {}, "Please Wait...", function (res) {
                                        Handler.hide();
                                        $("#hdInvoiceList").val(JSON.stringify(res));
                                        cg.setOptionArray(DrpIndex["FKInvoiceID_Text"], res, "FKInvoiceID", false, "Inum", "FKInvoiceID", "1");

                                    });
                                }
                            }
                            else {
                                alert("Select Party Frist");
                                cg_ClearRow(args)
                            }
                        }
                        else {
                            if (field == "Batch" || field == "Color" || field == "MRP") {

                                if (tranModel.ExtProperties.StockFlag == "I" || (TranAlias == "SORD" || TranAlias == "LORD" || TranAlias == "PORD")) {
                                    if (args.item["ModeForm"] != "1" && TranAlias != "SORD" && TranAlias != "LORD") { args.item["FkLotId"] = 0; }
                                    var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);
                                    //var Batch = args.item["Batch"];
                                    //var Color = args.item["Color"];
                                    if (field == "Batch") {
                                        Common.ajax(Handler.currentPath() + "CategorySizeListByProduct?FkProductId=" + FkProductId + "", {}, "Please Wait...", function (res) {
                                            Handler.hide();
                                            cg.setOptionArray(DrpIndex["Batch"], res, "Batch", false, "Size", "Size", "1");
                                        });
                                    }
                                    if (field == "Color") {
                                        var data = { name: field, pageNo: 1, pageSize: 1000, search: '', RowParam: FkProductId, ExtraParam: TranAlias };

                                        $.ajax({
                                            url: Handler.currentPath() + 'trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
                                                Handler.hide();
                                                cg.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                                            }, error: function (request, status, error) {
                                            }
                                        });
                                        //Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
                                        //    Handler.hide();
                                        //    cg.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                                        //});
                                    }

                                }
                            }
                        }
                    } else { alert('Promotion Artical Not Update'); args.grid.gotoCell(args.row, '', true); console.clear(); }
                } else { alert('Select Series'); args.grid.gotoCell(args.row, '', true); console.clear(); }
            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cg.columns[args.cell].field;
                //
                //var LinkSrNo = args.item["LinkSrNo"];
                //var _trandetail = tranModel.TranDetails;
                //if (Handler.isNullOrEmpty(LinkSrNo) || LinkSrNo <= 0 ) {

                if (field == "Product") {
                    args.item["SrNo"] = 0;

                    if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                        var InvoiceSrNo = Common.isNullOrEmpty(args.item["ProductName"]) ? 0 : parseFloat(args.item["ProductName"]);
                        //var data = ProductList.filter(function (element) { return (element.InvoiceSrNo == InvoiceSrNo); });
                        //var FkProductId = data.PkProductId;
                        //var data = cg.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.InvoiceSrNo == InvoiceSrNo && element.mode != 2); });
                        //if (data.length <= 0) {
                        args.item["InvoiceSrNo"] = InvoiceSrNo;
                        ColumnChange(args, args.row, "ProductReturn", false);
                        //}
                        //else {
                        //    alert("Product Already Add In List");
                        //    cg_ClearRow(args);
                        //    return false;
                        //}

                    } else {
                        // var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);

                        //var data = cg.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.ModeForm != 2); });
                        //if (data.length <= 1) {
                        //if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                        //    args.item["FkLotId"] = FkLotId > 0 ? FkLotId : 0; 
                        //}
                        ColumnChange(args, args.row, "Product", false);
                        //}
                        //else {
                        //    alert("Product Already Add In List");
                        //    cg_ClearRow(args);
                        //    return false;
                        //}
                    }
                }
                else if (field == "Qty") {
                    ColumnChange(args, args.row, "Qty", false);
                }
                else if (field == "FreeQty") {
                    ColumnChange(args, args.row, "FreeQty", false);
                }
                else if (field == "MRP") {
                    var MRP = parseFloat(args.item["MRP"]);
                    var Rate = parseFloat(args.item["Rate"]);
                    if (MRP < Rate) {
                        args.item["MRP"] = Rate;
                        alert('Invalid MRP');
                    }
                    ColumnChange(args, args.row, "MRP", false);
                }
                else if (field == "Rate") {

                    var MRP = parseFloat(args.item["MRP"]);
                    var Rate = parseFloat(args.item["Rate"]);
                    if (MRP < Rate) {
                        args.item["Rate"] = MRP;
                        alert('Invalid Rate');
                    }
                    ColumnChange(args, args.row, "Rate", false);
                }
                else if (field == "SaleRate") {
                    //  ColumnChange(args, args.row, "SaleRate");
                }
                else if (field == "TradeRate") {
                    //    ColumnChange(args, args.row, "TradeRate");
                }
                else if (field == "DistributerRate") {
                    // ColumnChange(args, args.row, "DistributerRate");
                }
                else if (field == "TradeDisc") {
                    ColumnChange(args, args.row, "TradeDisc", false);
                }
                else if (field == "Batch") {

                    if (tranModel.ExtProperties.TranType == "P" || TranAlias == "SORD" || TranAlias == "LORD") {

                    }
                    else {
                        var FkLotId = args.item["FkLotId"];
                        if (FkLotId > 0) {
                            ColumnChange(args, args.row, "Batch", false);
                        }
                        else {
                            args.item["Batch"] = "";
                            cg.updateRefreshDataRow(args.row);
                        }
                    }

                }
                else if (field == "Color") {

                    var FkLotId = args.item["FkLotId"];
                    if (FkLotId > 0) {
                        ColumnChange(args, args.row, "Color", false);

                    } else {
                        if (tranModel.ExtProperties.TranType != "P") {
                            args.item["Color"] = "";
                            cg.updateRefreshDataRow(args.row);
                        }
                    }
                }
                else if (field == "FKInvoiceID_Text") {
                    ColumnChange(args, args.row, "Inum", false);
                }
                //} else { alert('Promotion Artical Not Update'); BindGrid('DDT', _trandetail); }
            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cg.columns[args.cell].field;

                var FkProductId = args.grid.getDataItem(args.row)["FkProductId"];
                var FkLotId = args.grid.getDataItem(args.row)["FkLotId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                var ModeForm = args.grid.getDataItem(args.row)["ModeForm"];
                var BarcodeText = args.grid.getDataItem(args.row)["Barcode"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete", false);
                }
                else if (field == "Barcode" && ModeForm != 2 && BarcodeText != "") {

                    if (tranModel.ExtProperties.StockFlag == "I") {

                        if (FkProductId > 0) {
                            BarcodePopupUniqId_ListWith_Textbox(args, args.row);
                        }
                        else
                            alert('select Product');
                    }
                    else {
                        if (FkProductId > 0 && FkLotId > 0) {
                            var CodingScheme = args.grid.getDataItem(args.row)["CodingScheme"];
                            if (CodingScheme == 'Unique')
                                BarcodePopupUniqId_CheckboxList(args, args.row);
                        }
                        else
                            alert('select Size');
                    }
                }
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
                Common.GridColSetup(tranModel.ExtProperties.FKFormID, "dtl", function () {
                    var _dtl = GetDataFromGrid(false, false);
                    BindGrid('DDT', _dtl);
                });
            }

        });

        //---------------    ---------------   ---------------   ---------------/
    });
}
function BindGridReturn(GridId, data) {

    $("#" + GridId).empty();
    Common.Grid(tranModel.ExtProperties.FKFormID, "rtn", function (s) {

        // if (tranModel.FKOrderID > 0 && tranModel.FKOrderSrID) { s.setCtrlType = s.setCtrlType.replace("CD", "").replace("CD", "") }
        //var ProductList = (ControllerName == "SalesReturn" || ControllerName == "SalesCrNote") ? [] : JSON.parse($("#hdProductList").val());
        var ProductLotList = [];
        cgRtn = new coGrid("#" + GridId);
        UDIRtn = cgRtn;
        cgRtn.setColumnHeading(s.ColumnHeading);
        cgRtn.setColumnWidthPer(s.ColumnWidthPer, 1200);
        cgRtn.setColumnFields(s.ColumnFields);
        cgRtn.setAlign(s.Align);
        cgRtn.defaultHeight = "300px";
        cgRtn._MinRows = 50;
        cgRtn.setIdProperty("SrNo");
        cgRtn.setCtrlType(s.setCtrlType);

        var f = s.ColumnFields.split('~');
        var s = s.setCtrlType.split('~');
        var arrmapData = []
        var DrpIndex = {};

        for (kk = 0; kk < s.length; kk++) {
            var sl = s[kk];
            var fl = f[kk];
            if (sl == "C" || sl == "L" || sl == "CD") {
                DrpIndex[fl] = kk;
                switch (fl) {
                    case "Product":
                        cgRtn.columns[kk]["event"] = trandtldropList;
                        cgRtn.columns[kk]["fieldval"] = "FkProductId";
                        cgRtn.columns[kk]["KeyID"] = "PKID";
                        cgRtn.columns[kk]["KeyValue"] = "Product";
                        if (TranAlias = "SGRN") {
                            cgRtn.columns[kk]["RowValue"] = "FKInvoiceID";
                            cgRtn.columns[kk]["ExtraValue"] = "FkPartyId";

                        }
                        //cgRtn.columns[KK]["Keyfield"] = "Product,col";// if any saelected column show then comma seprate column name
                        //cgRtn.setOptionArray(kk, ProductList, "ProductName", false, "Product", "PkProductId", "1");
                        //arrmapData.push({ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" });
                        break
                    case "FKInvoiceID_Text":
                        cgRtn.columns[kk]["event"] = trandtldropList;
                        cgRtn.columns[kk]["fieldval"] = "FKInvoiceID";
                        cgRtn.columns[kk]["KeyID"] = "FKInvoiceID";
                        cgRtn.columns[kk]["KeyValue"] = "Inum";
                        cgRtn.columns[kk]["ExtraValue"] = "FkPartyId";

                        //cgRtn.columns[KK]["Keyfield"] = "Product,col";// if any saelected column show then comma seprate column name
                        //cgRtn.setOptionArray(kk, ProductList, "ProductName", false, "Product", "PkProductId", "1");
                        //arrmapData.push({ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" });
                        break
                    case "Batch":
                        if (tranModel.ExtProperties.StockFlag != "I") {
                            cgRtn.columns[kk]["event"] = trandtldropList;
                            cgRtn.columns[kk]["fieldval"] = "FkLotId";
                            cgRtn.columns[kk]["KeyID"] = "PkLotId";
                            cgRtn.columns[kk]["KeyValue"] = "Batch";
                            cgRtn.columns[kk]["Keyfield"] = "Batch,Color,MRP,SaleRate,PurchaseRate,TradeRate,DistributionRate";
                            cgRtn.columns[kk]["RowValue"] = "FkProductId";
                            cgRtn.columns[kk]["ExtraValue"] = "TranAlias,TranType"; //tranModel.ExtProperties.TranAlias;
                        }
                        break
                    case "Color":
                        if (tranModel.ExtProperties.StockFlag != "I") {
                            cgRtn.columns[kk]["event"] = trandtldropList;
                            cgRtn.columns[kk]["fieldval"] = "FkLotId";
                            cgRtn.columns[kk]["KeyID"] = "PkLotId";
                            cgRtn.columns[kk]["KeyValue"] = "Color";
                            cgRtn.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
                            cgRtn.columns[kk]["RowValue"] = "FkProductId,Batch";
                            cgRtn.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias; 
                        }
                        break
                    case "MRP":
                        cgRtn.columns[kk]["event"] = trandtldropList;
                        if (tranModel.ExtProperties.StockFlag == "I") {
                            cgRtn.columns[kk]["fieldval"] = "MRP";
                            cgRtn.columns[kk]["KeyID"] = "MRP";
                            cgRtn.columns[kk]["Keyfield"] = "MRP";

                        } else {
                            cgRtn.columns[kk]["fieldval"] = "FkLotId";
                            cgRtn.columns[kk]["KeyID"] = "PkLotId";
                            cgRtn.columns[kk]["Keyfield"] = "MRP";//,Color,Batch,SaleRate,PurchaseRate";

                        }
                        cgRtn.columns[kk]["KeyValue"] = "MRP";
                        cgRtn.columns[kk]["RowValue"] = "FkProductId,Batch,Color";

                        break
                }
            }
        }
        var obdt = cgRtn.populateDataFromJson({
            srcData: data,
            mapData: arrmapData
        });
        cgRtn.applyAddNewRow();
        cgRtn.bind(data);
        /*---------------    ---------------   ---------------   ---------------*/
        //var _data = cgRtn.data.filter(function (el) {
        //    return el.PkId > 0;
        //});
        var filterhash = []
        for (var i = 0; i < data.length; i++) {
            var cc = cgRtn.data[i];

            setdisablecolumn(cgRtn, cc, filterhash, i, 0);
        }
        cgRtn.outGrid.setCellCssStyles("trialRows", filterhash);
        /*---------------    ---------------   ---------------   ---------------*/
        cgRtn.dataView.getItemMetadata = function (row) {
            var cc = cgRtn.outGrid.getDataItem(row);
            if (cc === undefined)
                return true;
            else {
                var hash = {};
                var json = setdisablecolumn(cgRtn, cc, hash, row, 0);
                cgRtn.outGrid.setCellCssStyles("tRow" + String(row), hash);
                if (json != {})
                    return json;
            }
        }
        /*---------------    ---------------   ---------------   ---------------*/
        cgRtn.outGrid.onBeforeEditCell.subscribe(function (e, args) {

            if (args.cell != undefined) {

                var field = cgRtn.columns[args.cell].field;
                var LinkSrNo = args.item["LinkSrNo"];
                if (tranModel.FKSeriesId > 0) {
                    if (Handler.isNullOrEmpty(LinkSrNo) || LinkSrNo <= 0) {

                        if (field != "FKInvoiceID_Text" && field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
                            alert("Select Product Frist");
                            cgRtn_ClearRow(args)
                            cgRtn.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
                        }
                        else if (TranAlias == 'SGRN' && (field == "FKInvoiceID_Text" || field == "Product")) {
                            if (field == "Product") {
                                //if (Common.isNullOrEmpty(args.item["FKInvoiceID_Text"])) {
                                //    cgRtn.setOptionArray(DrpIndex["Product"], [], "FkProductId", false, "Product", "PkProductId", "1");

                                //} else {
                                //    cgRtn.setOptionArray(DrpIndex["Product"], [], "InvoiceSrNo", false, "InvoiceSrNo", "InvoiceSrNo", "1");
                                //}
                            }
                            else if (tranModel.FkPartyId > 0) {
                                //if (field == "Product") {
                                //    var FKInvoiceID = args.item["FKInvoiceID"];
                                //    //var data = { name: field, pageNo: 1, pageSize: 1000, search: '', FKInvoiceID: FKInvoiceID };

                                //    //Common.ajax(Handler.currentPath() + "InvoiceProductList", data, "Please Wait...", function (res) {
                                //    //    Handler.hide();
                                //    //    ProductList = res;
                                //    //    $("#hdProductList").val(JSON.stringify(res));
                                //        //cgRtn.setOptionArray(DrpIndex["ProductName_Text"], [], "ProductName", false, "Product", "InvoiceSrNo", "1");

                                //    //});
                                //}
                                //else if (field == "FKInvoiceID_Text") {
                                //    //Common.ajax(Handler.currentPath() + "InvoiceList?FkPartyId=" + tranModel.FkPartyId, {}, "Please Wait...", function (res) {
                                //    //    Handler.hide();
                                //    //    //console.log(res);
                                //    //    $("#hdInvoiceList").val(JSON.stringify(res));
                                //        //cgRtn.setOptionArray(DrpIndex["FKInvoiceID_Text"], [], "FKInvoiceID", false, "Inum", "FKInvoiceID", "1");

                                //  /*  });*/
                                //}
                            }
                            else {
                                alert("Select Party Frist");
                                cgRtn_ClearRow(args)
                            }
                        }
                        else {
                            if (field == "Batch" || field == "Color" || field == "MRP") {

                                if (tranModel.ExtProperties.StockFlag == "I" || (TranAlias == "SORD" || TranAlias == "LORD" || TranAlias == "PORD")) {
                                    if (args.item["ModeForm"] != "1" && TranAlias != "SORD" && TranAlias != "LORD") { args.item["FkLotId"] = 0; }
                                    var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);
                                    //var Batch = args.item["Batch"];
                                    //var Color = args.item["Color"];
                                    if (field == "Batch") {
                                        Common.ajax(Handler.currentPath() + "CategorySizeListByProduct?FkProductId=" + FkProductId + "", {}, "Please Wait...", function (res) {
                                            Handler.hide();
                                            cgRtn.setOptionArray(DrpIndex["Batch"], res, "Batch", false, "Size", "Size", "1");
                                        });
                                    }
                                    if (field == "Color") {
                                        var data = { name: field, pageNo: 1, pageSize: 1000, search: '', RowParam: FkProductId, ExtraParam: TranAlias };

                                        $.ajax({
                                            url: Handler.currentPath() + 'trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
                                                Handler.hide();
                                                cgRtn.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                                            }, error: function (request, status, error) {
                                            }
                                        });
                                        //Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
                                        //    Handler.hide();
                                        //    cgRtn.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
                                        //});
                                    }

                                }
                            }
                        }
                    } else { alert('Promotion Artical Not Update'); args.grid.gotoCell(args.row, '', true); console.clear(); }
                } else { alert('Select Series'); args.grid.gotoCell(args.row, '', true); console.clear(); }
            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cgRtn.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cgRtn.columns[args.cell].field;
                //
                //var LinkSrNo = args.item["LinkSrNo"];
                //var _trandetail = tranModel.TranDetails;
                //if (Handler.isNullOrEmpty(LinkSrNo) || LinkSrNo <= 0 ) {

                if (field == "Product") {

                    args.item["SrNo"] = 0;
                    if (TranAlias == 'SGRN' && !Common.isNullOrEmpty(args.item["FKInvoiceID"])) {
                        //var InvoiceSrNo = Common.isNullOrEmpty(args.item["ProductName"]) ? 0 : parseFloat(args.item["ProductName"]);

                        //var data = ProductList.filter(function (element) { return (element.InvoiceSrNo == InvoiceSrNo); });
                        //var FkProductId = data.PkProductId;
                        //var data = cgRtn.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.InvoiceSrNo == InvoiceSrNo && element.mode != 2); });
                        //if (data.length <= 0) {

                        //args.item["InvoiceSrNo"] = InvoiceSrNo;

                        ColumnChange(args, args.row, "ProductReturn", true);

                        //}
                        //else {
                        //    alert("Product Already Add In List");
                        //    cgRtn_ClearRow(args);
                        //    return false;
                        //}

                    } else {
                        // var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);

                        //var data = cgRtn.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.ModeForm != 2); });
                        //if (data.length <= 1) {
                        //if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                        //    args.item["FkLotId"] = FkLotId > 0 ? FkLotId : 0; 
                        //}
                        ColumnChange(args, args.row, "Product", true);
                        //}
                        //else {
                        //    alert("Product Already Add In List");
                        //    cgRtn_ClearRow(args);
                        //    return false;
                        //}
                    }
                }
                else if (field == "Qty") {
                    ColumnChange(args, args.row, "Qty", true);
                }
                else if (field == "FreeQty") {
                    ColumnChange(args, args.row, "FreeQty", true);
                }
                else if (field == "MRP") {
                    var MRP = parseFloat(args.item["MRP"]);
                    var Rate = parseFloat(args.item["Rate"]);
                    if (MRP < Rate) {
                        args.item["MRP"] = Rate;
                        alert('Invalid MRP');
                    }
                    ColumnChange(args, args.row, "MRP", true);
                }
                else if (field == "Rate") {

                    var MRP = parseFloat(args.item["MRP"]);
                    var Rate = parseFloat(args.item["Rate"]);
                    if (MRP < Rate) {
                        args.item["Rate"] = MRP;
                        alert('Invalid Rate');
                    }
                    ColumnChange(args, args.row, "Rate", true);
                }
                else if (field == "SaleRate") {
                    //  ColumnChange(args, args.row, "SaleRate");
                }
                else if (field == "TradeRate") {
                    //    ColumnChange(args, args.row, "TradeRate");
                }
                else if (field == "DistributerRate") {
                    // ColumnChange(args, args.row, "DistributerRate");
                }
                else if (field == "TradeDisc") {
                    ColumnChange(args, args.row, "TradeDisc", true);
                }
                else if (field == "Batch") {

                    if (tranModel.ExtProperties.TranType == "P" || TranAlias == "SORD" || TranAlias == "LORD") {

                    }
                    else {
                        var FkLotId = args.item["FkLotId"];
                        if (FkLotId > 0) {
                            ColumnChange(args, args.row, "Batch", true);
                        }
                        else {
                            args.item["Batch"] = "";
                            cgRtn.updateRefreshDataRow(args.row);
                        }
                    }

                }
                else if (field == "Color") {

                    var FkLotId = args.item["FkLotId"];
                    if (FkLotId > 0) {
                        ColumnChange(args, args.row, "Color", true);

                    } else {
                        if (tranModel.ExtProperties.TranType != "P") {
                            args.item["Color"] = "";
                            cgRtn.updateRefreshDataRow(args.row);
                        }
                    }
                }
                else if (field == "FKInvoiceID_Text") {
                    ColumnChange(args, args.row, "Inum", true);
                }
                //} else { alert('Promotion Artical Not Update'); BindGrid('DDT', _trandetail); }
            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cgRtn.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cgRtn.columns[args.cell].field;

                var FkProductId = args.grid.getDataItem(args.row)["FkProductId"];
                var FkLotId = args.grid.getDataItem(args.row)["FkLotId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                var ModeForm = args.grid.getDataItem(args.row)["ModeForm"];
                var BarcodeText = args.grid.getDataItem(args.row)["Barcode"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete", true);
                }
                else if (field == "Barcode" && ModeForm != 2 && BarcodeText != "") {

                    if (tranModel.ExtProperties.StockFlag == "I") {

                        if (FkProductId > 0) {
                            BarcodePopupUniqId_ListWith_Textbox(args, args.row);
                        }
                        else
                            alert('select Product');
                    }
                    else {
                        if (FkProductId > 0 && FkLotId > 0) {
                            var CodingScheme = args.grid.getDataItem(args.row)["CodingScheme"];
                            if (CodingScheme == 'Unique')
                                BarcodePopupUniqId_CheckboxList(args, args.row);
                        }
                        else
                            alert('select Size');
                    }
                }
            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cgRtn.outGrid.onContextMenu.subscribe(function (e, args) {

            e.preventDefault();
            var j = cgRtn.outGrid.getCellFromEvent(e);
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
            if (!UDIRtn.outGrid.getEditorLock().commitCurrentEdit()) {
                return;
            }

            var row = $(this).data("row");
            var command = $(e.target).attr("data");
            if (command == "EditColumn") {
                Common.GridColSetup(tranModel.ExtProperties.FKFormID, "rtn", function () {
                    var _dtl = GetDataFromGrid(false, true);
                    BindGridReturn('DDTReturn', _dtl);
                });
            }

        });

        //---------------    ---------------   ---------------   ---------------/
    });
}
function BarcodePopupUniqId_CheckboxList(args, rowIndex) {

    tranModel.TranDetails = GetDataFromGrid();

    if (tranModel.TranDetails.length > 0) {
        $(".loader").show();
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'BarcodeList',
            data: { model: tranModel, rowIndex: rowIndex },
            datatype: "json",
            success: function (res) {
                var htm = '';
                htm += '<div class="mb-4 card">';
                htm += '   <div class="card-body"> ';
                htm += '       <div class="row mb-3"> ';
                htm += '           <div class="col-md-6"> ';
                htm += '               <div class="card-title">Select Barcode</div> ';
                htm += '           </div>';
                htm += '           <div class="col-md-6 text-center"> ';
                htm += '               <input type="button" id="btnSelectBarcode" value="Done" class="btn btn-success" />';
                htm += '           </div> ';
                htm += '       </div> ';
                htm += '       <div class="row">';
                htm += '           <div class="col-md-12" id="WUCFilter"> ';
                htm += '           </div>';
                htm += '          </div>  ';
                htm += '   </div>';
                htm += '   </div>';

                //console.log(res.data);
                Handler.popUp(htm, { width: "400px", height: "500px" }, function () {
                    var cg = new coGrid("#WUCFilter");
                    cg.setColumnHeading("Select~Barcode");
                    cg.setColumnWidthPer("10~30", 800);
                    cg.setColumnFields("IsPrint~Barcode");
                    cg.setAlign("C~L");
                    cg.defaultHeight = "400px";
                    cg.setSearchType("0~1");
                    cg.setSearchableColumns("Barcode");
                    cg.setSortableColumns("Barcode");
                    cg.setCheckAllCheckboxColumns("IsPrint~");
                    cg.setIdProperty("Barcode");
                    cg.setCtrlType("B~");
                    cg.bind(res.data);
                    cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());
                    //filterGridTagPrint = cg;


                    $("#btnSelectBarcode").off("click").on("click", function () {
                        var _List = cg.getData().filter(function (el) { return el.IsPrint })
                        console.clear();
                        let SrNo = tranModel.TranDetails[rowIndex].SrNo;
                        tranModel.UniqIdDetails = tranModel.UniqIdDetails.filter((u) => {
                            return u.SrNo != SrNo
                        })
                        var _existBarcode = [];
                        $(_List).each(function (i, v) {
                            //console.log(v);
                            var _exist = $.grep(tranModel.UniqIdDetails, function (item) {
                                return item.Barcode == v.Barcode;
                            });

                            if (_exist.length == 0) {
                                tranModel.UniqIdDetails.push({ SrNo: v.SrNo, Barcode: v.Barcode })
                            } else {
                                _existBarcode.push(v.Barcode);
                            }
                        })
                        /* RPTFilter[type].Filter = JSON.stringify(_List);*/
                        if (_existBarcode.length > 0) {
                            alert("Barcode Already Exists : " + _existBarcode.join(","));
                        }
                        $(".popup_d").hide();
                    });




                });
                $(".loader").hide();
            }
        });
    }
}

var cgGridUniqIdTextbox = null;

function BarcodePopupUniqId_ListWith_Textbox(args, rowIndex) {

    tranModel.TranDetails = GetDataFromGrid();
    var CodingScheme = args.grid.getDataItem(args.row)["CodingScheme"];
    var SrNo = args.grid.getDataItem(args.row)["SrNo"];
    if (CodingScheme != 'fixed') {
        if (tranModel.TranDetails.length > 0) {
            $(".loader").show();
            var filteredDetails = tranModel.UniqIdDetails.filter(x => x.SrNo == SrNo);

            var htm = '';
            htm += '<div class="mb-4 card">';
            htm += '   <div class="card-body"> ';
            htm += '       <div class="row mb-3"> ';
            htm += '           <div class="col-md-6"> ';
            htm += '               <div class="card-title">Add Barcode</div> ';
            htm += '           </div>';
            htm += '           <div class="col-md-6 text-center"> ';
            // htm += '               <input type="button" id="btnSelectBarcode" value="Done" class="btn btn-success" />';
            htm += '           </div> ';
            htm += '       </div> ';
            htm += '       <div class="row mb-3"> ';
            htm += '           <div class="col-md-8"> ';
            htm += '               <input type="text" id="txtBarcode" value="" class="form-control" />';
            htm += '           </div>';
            htm += '           <div class="col-md-4"> ';
            htm += '               <input type="button" id="btnAddNewBarcode" value="Add" class="btn btn-success w-100" />';
            htm += '           </div> ';
            htm += '       </div> ';
            htm += '       <div class="row">';
            htm += '           <div class="col-md-12" id="WUCFilterUniqIdTextbox"> ';
            htm += '           </div>';
            htm += '           <div class="col-md-12 text-center"> ';
            htm += '               <input type="hidden" id="txtCodingScheme" value="' + CodingScheme + '" />';
            htm += '               <input type="button" id="btnSelectBarcode" value="Done" class="btn btn-success" />';
            htm += '           </div> ';

            htm += '          </div>  ';
            htm += '   </div>';
            htm += '   </div>';

            Handler.popUp(htm, { width: "400px", height: "500px" }, function () { });
            Bind_cgGridUniqIdTextbox(args, rowIndex, filteredDetails)
            $(".loader").hide();
        }
    }
}
function Bind_cgGridUniqIdTextbox(args, rowIndex, _d) {

    cgGridUniqIdTextbox = new coGrid("#WUCFilterUniqIdTextbox");
    cgGridUniqIdTextbox.setColumnHeading("Barcode~Del");
    cgGridUniqIdTextbox.setColumnWidthPer("30~10", 800);
    cgGridUniqIdTextbox.setColumnFields("Barcode~Delete");
    cgGridUniqIdTextbox.setAlign("C~L");
    cgGridUniqIdTextbox.defaultHeight = "400px";
    cgGridUniqIdTextbox.setSearchType("0~1");
    cgGridUniqIdTextbox.setSearchableColumns("Barcode");
    cgGridUniqIdTextbox.setSortableColumns("Barcode");
    //  cg.setCheckAllCheckboxColumns("~~");
    cgGridUniqIdTextbox.setIdProperty("Barcode");
    cgGridUniqIdTextbox.setCtrlType("~BD");
    cgGridUniqIdTextbox.bind(_d);
    cgGridUniqIdTextbox.outGrid.setSelectionModel(new Slick.RowSelectionModel());
    //filterGridTagPrint = cg;
    cgGridUniqIdTextbox.outGrid.onClick.subscribe(function (e, args) {

        if (args.cell != undefined) {
            var field = cgGridUniqIdTextbox.columns[args.cell].field;

            var SrNo = args.grid.getDataItem(args.row)["SrNo"];
            var Barcode = args.grid.getDataItem(args.row)["Barcode"];
            if (field == "Delete") {
                var _List = cgGridUniqIdTextbox.getData().filter(function (el) { return el.Barcode != Barcode });

                Bind_cgGridUniqIdTextbox(args, rowIndex, _List);
            }
        }
    });

    $("#btnAddNewBarcode").off("click").on("click", function () {
        var barcode = $("#txtBarcode").val();
        var CodingScheme = $("#txtCodingScheme").val();
        let SrNo = tranModel.TranDetails[rowIndex].SrNo;
        var _List = cgGridUniqIdTextbox.getData().filter(function (el) { return !Handler.isNullOrEmpty(el.Barcode) })

        if (!Handler.isNullOrEmpty(barcode)) {
            if ((_List.filter(function (el) { return el.SrNo == SrNo }).length == 0 && CodingScheme == 'Lot') || CodingScheme == 'Unique') {
                if (_List.filter(function (el) { return el.Barcode == barcode }).length == 0) {
                    _List.push({ "Barcode": barcode, SrNo: SrNo });
                    Bind_cgGridUniqIdTextbox(args, rowIndex, _List);
                    $("#txtBarcode").val('');
                }
                else
                    alert('Barcode Already exists');
            } else
                alert('Only 1 barcode Required');
        }
        else
            alert('Please Enter Barcode');

    });

    $("#btnSelectBarcode").off("click").on("click", function () {
        var _List = cgGridUniqIdTextbox.getData().filter(function (el) { return !Handler.isNullOrEmpty(el.Barcode) });
        let SrNo = tranModel.TranDetails[rowIndex].SrNo;

        tranModel.UniqIdDetails = tranModel.UniqIdDetails.filter((u) => {
            return u.SrNo != SrNo
        })
        var _existBarcode = [];
        $(_List).each(function (i, v) {
            var _exist = $.grep(tranModel.UniqIdDetails, function (item) {
                return item.Barcode == v.Barcode;
            });

            if (_exist.length == 0) {
                tranModel.UniqIdDetails.push({ SrNo: v.SrNo, Barcode: v.Barcode })
            } else {
                _existBarcode.push(v.Barcode);
            }
        })
        /* RPTFilter[type].Filter = JSON.stringify(_List);*/
        if (_existBarcode.length > 0) {
            alert("Barcode Already Exists : " + _existBarcode.join(","));
        } else { $(".popup_d").hide(); }

    });





}
function BarcodeScan(barcode, IsReturn) {

    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid(false, IsReturn);
    tranModel.TranReturnDetails = tranModel.ExtProperties.TranAlias == "SGRN" ? GetDataFromGrid(false, true) : [];

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'BarcodeScan',
        data: { model: tranModel, barcode: barcode, IsReturn: IsReturn },
        datatype: "json", success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                //$(tranModel.TranDetails).each(function (i, v) {
                //    v["ProductName"] = parseInt(v.FkProductId);
                //});
                if (!IsReturn) {
                    setFooterData(tranModel);
                    setPaymentDetail(tranModel);
                    BindGrid('DDT', tranModel.TranDetails);
                } else {

                    BindGridReturn('DDTReturn', tranModel.TranReturnDetails);
                    setHeaderData(tranModel);
                }
                //if (Handler.isNullOrEmpty(tranModel.TranDetails[rowIndex].PromotionType)) {


                //setFooterData(tranModel);
                //setPaymentDetail(tranModel);

            }
            else
                alert(res.msg);
            $(".loader").hide();
        }
    })
}

function cg_ClearRow(args) {
    args.item["PkId"] = 0;
    args.item["ModeForm"] = 0;
    args.item["FkProductId"] = "";
    args.item["ProductName_Text"] = "";
    args.item["Product"] = "";
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
    args.item["TradeRate"] = "";
    args.item["DistributionRate"] = "";
    args.item["MfgDate"] = "";
    args.item["FKInvoiceID"] = 0;
    args.item["InvoiceSrNo"] = 0;
    args.item["FKInvoiceSrID"] = 0;
    args.item["InvoiceDate"] = "";
    args.item["Barcode"] = "";
    args.item["CodingScheme"] = "";
    cg.updateRefreshDataRow(args.row);
}
function cgRtn_ClearRow(args) {
    args.item["PkId"] = 0;
    args.item["ModeForm"] = 0;
    args.item["FkProductId"] = "";
    args.item["ProductName_Text"] = "";
    args.item["Product"] = "";
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
    args.item["TradeRate"] = "";
    args.item["DistributionRate"] = "";
    args.item["MfgDate"] = "";
    args.item["FKInvoiceID"] = 0;
    args.item["InvoiceSrNo"] = 0;
    args.item["FKInvoiceSrID"] = 0;
    args.item["InvoiceDate"] = "";
    args.item["Barcode"] = "";
    args.item["CodingScheme"] = "";
    cgRtn.updateRefreshDataRow(args.row);
}


function ImportBarcode() {
    if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
        alert('The File APIs are not fully supported in this browser.');
        return;
    }
    if (tranModel.FKSeriesId <= 0) {
        alert('Select Series.');
        return;
    }
    var input = document.getElementById('Barcodefile');
    if (!input) {
        alert("Um, couldn't find the fileinput element.");
    }
    else if (!input.files) {
        alert("This browser doesn't seem to support the `files` property of file inputs.");
    }
    else if (!input.files[0]) {
        alert("Please select a file");
    }
    else {

        const reader = new FileReader()
        reader.onload = function (event) {

            if (!Handler.isNullOrEmpty(event.target.result)) {
                var lst = event.target.result.split('\r\n');
                sumitImportBarcode(lst);
            } else { alert('Invalid File Data'); }
        };
        reader.readAsText($('#Barcodefile')[0].files[0])
    }


}
function sumitImportBarcode(barcodelist) {

    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid();

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'BarcodeFiles',
        data: { model: tranModel, barcodelist: barcodelist },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                //$(tranModel.TranDetails).each(function (i, v) {
                //    v["ProductName"] = parseInt(v.FkProductId);
                //});
                BindGrid('DDT', tranModel.TranDetails);

                setFooterData(tranModel);
                setPaymentDetail(tranModel);
                $("#Barcodefile").val("");
                if (!Handler.isNullOrEmpty(res.ListNotFound)) { alert('Not Found Barcode is :' + res.ListNotFound) }
            }
            else
                alert(res.msg);
            $(".loader").hide();
        }
    })
}
function handleFileLoad(event) {

    var items = event.target.result.split(',');
    var items1 = JSON.stringify(event.target.result).split('\r\n');
    //console.log(JSON.stringify(event.target.result));
    //console.log(JSON.parse(JSON.stringify(event.target.result)));
    // document.getElementById('fileContent').textContent = event.target.result;
}

function ColumnChange(args, rowIndex, fieldName, IsReturn) {

    tranModel.TranDetails = GetDataFromGrid(false, false);
    tranModel.TranReturnDetails = tranModel.ExtProperties.TranAlias == "SGRN" ? GetDataFromGrid(false, true) : [];
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
                    if (!IsReturn) {
                        setFooterData(tranModel);
                        setPaymentDetail(tranModel);
                    }
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

function ApplyRateDiscount(Type, Discount) {
    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid();

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'ApplyRateDiscount',
        data: { model: tranModel, type: Type, discount: Discount },
        datatype: "json", success: function (res) {

            if (res.status == "success") {
                tranModel = res.data;

                BindGrid('DDT', tranModel.TranDetails);

                setFooterData(tranModel);
                setPaymentDetail(tranModel);


            }
            else
                alert(res.msg);

            $(".loader").hide();

        }
    })
}
function FooterChange(fieldName) {

    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid();

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'FooterChange',
        data: { model: tranModel, fieldName: fieldName },
        datatype: "json",
        success: function (res) {
            //console.log(res);
            if (res.status == "success") {
                //console.log(tranModel);

                tranModel = res.data;
                setFooterData(tranModel);
                setPaymentDetail(tranModel);

            }
            else
                alert(res.msg);
            $(".loader").hide();
        }
    })
}
function PaymentDetail() {

    //console.log(tranModel);
    tranModel.TranDetails = GetDataFromGrid();

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetPaymentDetail',
        data: { model: tranModel },
        datatype: "json",
        success: function (res) {

            if (res.status == "success") {

                tranModel = res.data;

                setPaymentDetail(tranModel);
                $('.model-paymentdetail').modal('toggle');
            }
            else
                alert(res.msg);
        }
    })
}
function PaymentPopup() {
    setPaymentDetail(tranModel)
    $('.model-paymentdetail').modal('toggle');
}
function setHeaderData(data) {
    Common.Set(".trn-header", data, "");
    return false;
}

function setFooterData(data) {
    Common.Set(".trn-footer", data, "");
    return false;
}

function setPaymentDetail(data) {

    Common.Set(".model-paymentdetail", data, "");
    if (tranModel.FKPostAccID <= 0) { $("#Account").val(''); }
    return false;
}


function setGridRowData(args, data, rowIndex, fieldName, IsReturn) {

    if (fieldName == 'Delete') {
        args.grid.getDataItem(args.row).ModeForm = 2
    }
    else {

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
        // args.item["Barcode"] = "Barcode";//data[rowIndex].Barcode;
        args.item["Delete"] = 'Delete';

        var CodingScheme = data[rowIndex].CodingScheme;
        args.item["CodingScheme"] = CodingScheme;

        if (tranModel.ExtProperties.TranType == "P") {
            if (CodingScheme == 'fixed' || tranModel.TranAlias == "PORD")
                args.item["Barcode"] = "";
            else
                args.item["Barcode"] = "Barcode";
        }
        else if (tranModel.ExtProperties.TranType == "S") {
            if (CodingScheme == 'Unique')
                args.item["Barcode"] = "Barcode";
            else
                args.item["Barcode"] = "";
        }
    }
    if (IsReturn) {
        cgRtn.updateRefreshDataRow(args.row);
        cgRtn.updateAndRefreshTotal();
    } else {
        cg.updateRefreshDataRow(args.row);
        cg.updateAndRefreshTotal();
    }

    //cg.gotoCell(args.row, args.cell + 1);
    args.grid.gotoCell(args.row, args.cell + 1, true)
    $(".loader").hide();
    return false;
}

function GetDataFromGrid(ifForsave, IsReturn) {

    var _d = [];
    if (IsReturn) {
        var arrayRtn = cgRtn.getData().filter(x => x.FkProductId > 0);
        let numberRtn = Math.max.apply(Math, arrayRtn.map(function (o) { return o.SrNo; }));
        let SrNoRtn = numberRtn > 0 ? numberRtn : 1000;
        cgRtn.getData().filter(function (element) {
            if (ifForsave) {
                if (!Handler.isNullOrEmpty(element.FKInvoiceID) && !Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

                    if (element.FkProductId > 0 && element.SrNo > 0) { element.SrNo = element.SrNo; }
                    else { SrNoRtn++; element.SrNo = SrNoRtn; }
                    element.TranType = "I"
                    _d.push(element);

                    return element
                }
            }
            else {
                if (!Handler.isNullOrEmpty(element.FKInvoiceID) || !Handler.isNullOrEmpty(element.Product)) {

                    if ((element.FKInvoiceID > 0 || element.FkProductId > 0) && element.SrNo > 0) { element.SrNo = element.SrNo; }
                    else { SrNoRtn++; element.SrNo = SrNoRtn; }
                    element.TranType = "I"
                    _d.push(element);
                    return element
                }
            }
        });
    }
    else {
        var array = cg.getData().filter(x => x.FkProductId > 0);
        let number = Math.max.apply(Math, array.map(function (o) { return o.SrNo; }));
        let SrNo = number > 0 ? number : 0;

        cg.getData().filter(function (element) {

            if (ifForsave) {
                if (!Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

                    if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                    else { SrNo++; element.SrNo = SrNo; }
                    // element.FkProductId = parseInt(element.Product);
                    element.TranType = "O"
                    _d.push(element);

                    return element
                }
            }
            else {


                if (!Handler.isNullOrEmpty(element.Product) || !Handler.isNullOrEmpty(element.FKInvoiceID)) {

                    if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                    else { SrNo++; element.SrNo = SrNo; }
                    //element.FkProductId = parseInt(element.Product);
                    element.TranType = "O"
                    _d.push(element);
                    return element
                }
            }
        });
    }
    return _d
}
function GetAndCheckBarcodeQty(_d) {
    var _NotFound = []
    _d.filter(function (element) {

        let totalQty = (parseFloat(element.Qty) + parseFloat(element.FreeQty))
        var _srnoList = tranModel.UniqIdDetails.filter((u) => { return u.SrNo == element.SrNo });
        if (_srnoList.length != totalQty && element.ModeForm != 2 && element.CodingScheme == 'Unique')
            _NotFound.push(element.Product);
    });
    return _NotFound
}

function SaveRecord() {

    Common.Get(".form", "", function (flag, _d) {

        if (flag) {

            tranModel.PkId = $('#PkId').val();
            tranModel.FkPartyId = $('#FkPartyId').val();
            tranModel.EntryDate = $('#EntryDate').val();
            tranModel.GRDate = $('#GRDate').val();
            tranModel.TranDetails = [];
            if ((tranModel.FkPartyId > 0) || (tranModel.ExtProperties.DocumentType == "C")) {
                if (tranModel.FKSeriesId > 0) {

                    tranModel.TranDetails = GetDataFromGrid(true, false);
                    tranModel.TranReturnDetails = tranModel.ExtProperties.TranAlias == "SGRN" ? GetDataFromGrid(true, true) : [];


                    var filteredDetails = tranModel.TranDetails.filter(x => x.ModeForm != 2);
                    if (tranModel.TranDetails.length > 0 && filteredDetails.length > 0) {

                        var _NotMatch = GetAndCheckBarcodeQty(tranModel.TranDetails);
                        if (_NotMatch.length == 0 || tranModel.TranAlias != "SINV" || tranModel.TranAlias != "LINV") {
                            /* alert('Ok');*/
                            //console.log(tranModel);
                            $.ajax({
                                type: "POST",
                                url: Handler.currentPath() + 'Create',
                                data: { model: tranModel },
                                datatype: "json",
                                success: function (res) {

                                    if (res.status == "success") {
                                        alert('Save Successfully..');
                                        if (ControllerName == 'SalesInvoiceTouch') {
                                            location.reload();
                                        } else {
                                            window.location = Handler.currentPath() + 'List';
                                        }
                                    }
                                    else {

                                        alert(res.msg);
                                        tranModel = res.data;
                                        BindGrid('DDT', tranModel.TranDetails);
                                    }
                                }
                            });
                        }
                        else
                            alert("Barcode And Qty Not Match Product : " + _NotMatch.join(","));
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

    if (cc["ModeForm"] == 1) {
        h["ProductName_Text"] = "sobc";
        focu["ProductName_Text"] = { "focusable": false };
    }
    else if (cc["ModeForm"] == 2) {
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
                    tranModel.TranDetails = [];
                    BindGrid('DDT', tranModel.TranDetails);

                }
            }
            else
                alert(res.msg);
        }
    });
}
function GetParty() {

    var PartyMobile = $("#PartyMobile").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'GetParty',
        data: { model: tranModel, PartyMobile: PartyMobile },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $('#FkPartyId').val(tranModel.FkPartyId);
                $('#drpFkPartyId').val(tranModel.PartyName);
                $('#PartyGSTN').val(tranModel.PartyGSTN);
                $('#PartyName').val(tranModel.PartyName);
                // $('#PartyMobile').val(tranModel.PartyMobile);
                $('#PartyAddress').val(tranModel.PartyAddress);
                $('#PartyCredit').val(tranModel.PartyCredit);

                if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                    tranModel.TranDetails = [];
                    BindGrid('DDT', tranModel.TranDetails);

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

function setReferBy() {

    tranModel["FKReferById"] = $("#FKReferById").val();
}
function setSalesPer() {

    tranModel['FKSalesPerId'] = $("#FKSalesPerId").val();
}
function setBankThroughBank() {
    var FKBankThroughBankID = $("#FKBankThroughBankID").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetBankThroughBank',
        data: { model: tranModel, FKBankThroughBankID: FKBankThroughBankID },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
            }
            else
                alert(res.msg);
        }
    });
}

function trandtldropList(data) {

    var output = []
    $.ajax({
        url: Handler.currentPath() + 'trandtldropList', data: data, async: false, dataType: 'JSON', success: function (result) {

            output = result;

        }, error: function (request, status, error) {
        }
    });
    //console.log(output);
    return output;

}

function GetWalkingCustomerDetail(Mobile) {

    if (tranModel.ExtProperties.DocumentType == "C") {
        //$("#FkPartyId").val('0');
        //$("#PartyName,#PartyAddress,#PartyDob,#PartyMarriageDate").removeAttr("readonly"); 

        //Common.ajax(Handler.currentPath() + "GeWalkingCustomerbyMobile?Mobile=" + Mobile + "", {}, "Please Wait...", function (res) {
        //    Handler.hide();
        //    if (res != null) {
        //        $("#FkPartyId").val(res.PkId);
        //        $("#PartyName").val(res.Name);
        //        $("#PartyAddress").val(res.Address);
        //        $("#PartyDob").val(res.Dob);
        //        $("#PartyMarriageDate").val(res.MarriageDate); 

        //        tranModel["PartyName"] = res.Name; 
        //        tranModel["PartyAddress"] = res.Address; 
        //        tranModel["PartyDob"] = res.Dob;
        //        tranModel["PartyMarriageDate"] = res.MarriageDate;
        //        $("#PartyName,#PartyAddress,#PartyDob,#PartyMarriageDate").attr("readonly","readonly"); 


        //    }

        //});
    }
}

function DatabyEntryNo() {
    $(".loader").show();
    var EntryNo = $("#EntryNo").val();
    var FKSeriesId = $("#FKSeriesId").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'GetIdbyEntryNo',
        data: { EntryNo: EntryNo, FKSeriesId: FKSeriesId },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                if (res.data > 0) {
                    var pk_Id = res.data;
                    window.location.href = Handler.currentPath() + "Create/" + pk_Id + "/" + FKSeriesId;
                }
                else {
                    alert('Not Found');
                    $(".loader").hide();
                    location.reload()
                }
            }
            else {
                alert(res.msg);
                $(".loader").hide();
                location.reload()
            }

        }
    })
}

function VoucherDetail() {

    var pagetitle = $(".pagetitle").text().replace(/\s+/g, " ").trim();
    if (tranModel.PkId > 0) {

        var url = "/Transactions/Voucher/View/" + tranModel.PkId + "/" + tranModel.FKSeriesId + "/" + pagetitle;

        window.open(url);
    }
}


function showpopupPrintOption() {//BarcodePrint
    var _List = [];
    cg.getData().filter(function (element) {
        if (!Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

            if (element.FkProductId > 0 && element.FkId > 0 && element.ModeForm == 1) {
                if (ControllerName == "PurchaseInvoice") {
                    _List.push({
                        FKProductId: element.FkProductId,
                        TranInId: element.FkId,
                        TranInSeriesId: tranModel.FKSeriesId,
                        TranInSrNo: element.SrNo,
                        TranOutId: 0,
                        TranOutSeriesId: 0,
                        TranOutSrNo: 0,
                        FkLocationId: tranModel.FKLocationID,
                    });
                }
                else {
                    _List.push({
                        FKProductId: element.FkProductId,
                        TranInId: 0,
                        TranInSeriesId: 0,
                        TranInSrNo: 0,
                        TranOutId: element.FkId,
                        TranOutSeriesId: tranModel.FKSeriesId,
                        TranOutSrNo: element.SrNo,
                        FkLocationId: tranModel.FKLocationID,
                    });
                }

            }
        }
    });

    if (_List.length > 0) {
        //console.log(_List);
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'GetBarcodeList',
            data: { model: _List },
            datatype: "json",
            success: function (res) {
                if (res.status == "success") {

                    Handler_BarcodePrintGridData = res.data;
                    Handler.barcodePrint(function () {

                    });
                } else {
                    alert(res.msg);
                }
            }
        })

    }
    ////////////////


}


function UploadFile() {
    //var TranDetails = GetDataFromGrid(); 
    $(".loader").show();
    var formData = new FormData();
    for (var key in tranModel) {
        formData.append(key, tranModel[key]);
    }
    //formData.append("TranDetails", JSON.stringify(TranDetails));
    formData.append("file", $("#ExcelFile")[0].files[0]);

    //console.log(formData);
    $.ajax({
        type: 'POST',
        url: Handler.currentPath() + 'UploadFile',
        data: formData,
        processData: false,
        contentType: false
    }).done(function (res) {
        if (res.status == "success") {
            tranModel = res.data;
            BindGrid('DDT', tranModel.TranDetails);

            setFooterData(tranModel);
            setPaymentDetail(tranModel);
            $("#ExcelFile").val("");
            if (!Handler.isNullOrEmpty(tranModel.NotFound)) {
                alert('Not Found Product is :' + tranModel.NotFound)
            }
        }
        else
            alert(res.msg);

        $(".loader").hide();
    });
}

$('#btnClose').click(function (e) {
    UpdateTrnSatus('C');

    return false;
});
$('#btnOpen').click(function (e) {
    UpdateTrnSatus('P');

    return false;
});
function UpdateTrnSatus(TrnStatus) {
    $(".loader").show();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'UpdateTrnSatus',
        data: { PkId: tranModel.PkId, FKSeriesId: tranModel.FKSeriesId, TrnStatus: TrnStatus },
        datatype: "json",
        success: function (res) {

            if (res.status == "success") {
                location.reload();
            }
            else
                alert(res.msg);

            $(".loader").hide();
        }
    });
}

function BindCategoryProduct_Touch($cntrl) {
    $("#ul_Category a").removeClass('active');
    $($cntrl).addClass('active');
    var id = ($($cntrl).attr('data-itemid'));
    $('#div_CategoryProduct').html('');
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'GetProductListByCat',
        data: { FkCategoryId: id },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                $(res.data).each(function (i, v) {
                    var htm = '';
                    htm += '<div class="col-md-3">';
                    htm += '<div class="callout callout-success" onclick="ProductTouch(this)" data-itemid="' + v.PkProductId + '">';
                    htm += '<p><strong>' + v.Product + '</strong></p>';
                    htm += '</div>';
                    htm += '</div>';
                    $('#div_CategoryProduct').append(htm);
                });
            } else {
                alert(res.msg);
            }
        }
    })
}

function ProductTouch($cntrl) {
    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid();
    var id = $($cntrl).attr('data-itemid');
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'ProductTouch',
        data: { model: tranModel, PkProductId: id },
        datatype: "json", success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                //$(tranModel.TranDetails).each(function (i, v) {
                //    v["ProductName"] = parseInt(v.FkProductId);
                //});
                BindGrid('DDT', tranModel.TranDetails);

                setFooterData(tranModel);
                setPaymentDetail(tranModel);

            }
            else
                alert(res.msg);
            $(".loader").hide();
        }
    })
}

function AutoFillLastRecord() {

    tranModel.TranDetails = GetDataFromGrid();
    if (tranModel.TranDetails.length > 0) {
        $(".loader").show();
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'AutoFillLastRecord',
            data: { model: tranModel },
            datatype: "json", success: function (res) {
                if (res.status == "success") {
                    tranModel = res.data;
                    //$(tranModel.TranDetails).each(function (i, v) {
                    //    v["ProductName"] = parseInt(v.FkProductId);
                    //});
                    BindGrid('DDT', tranModel.TranDetails);

                    setFooterData(tranModel);
                    setPaymentDetail(tranModel);

                }
                else
                    alert(res.msg);
                $(".loader").hide();
            }
        })
    }
    else
        alert('please insert any 1 record');
}

function ApplyPromotion() {
    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid();

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'ApplyPromotion',
        data: { model: tranModel },
        datatype: "json", success: function (res) {

            if (res.status == "success") {
                tranModel = res.data;

                BindGrid('DDT', tranModel.TranDetails);

                setFooterData(tranModel);
                setPaymentDetail(tranModel);


            }
            else
                alert(res.msg);

            $(".loader").hide();

        }
    })
}
var cgImport = null;
function ImportDataPopup() {

    $('#ImportDatafile').val(''); 
    $("#DDTImport").html('');
    $('.model-importdata').modal('toggle');
}
function ImportDataFromFile() {
    if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
        alert('The File APIs are not fully supported in this browser.');
        return;
    }
    if (tranModel.FKSeriesId <= 0) {
        alert('Select Series.');
        return;
    }
    var input = document.getElementById('ImportDatafile');
    if (!input) {
        alert("Um, couldn't find the fileinput element.");
    }
    else if (!input.files) {
        alert("This browser doesn't seem to support the `files` property of file inputs.");
    }
    else if (!input.files[0]) {
        alert("Please select a file");
    }
    else {
        var formData = new FormData();
        var file = $('#ImportDatafile')[0].files[0];
        formData.append("file", file);

        $.ajax({
            url: Handler.currentPath() + 'ImportDatafile',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (res) {
                //console.log(res)
                if (res.status == 'success') {
                    if (!Handler.isNullOrEmpty(res.msg))
                        alert(res.msg);
                    
                    var data = res.data;
                    var ColumnHeading = "Artical Match~Artical~SubSection~Size~Color~Qty~MRP";
                    var ColumnWidthPer = "10~10~10~10~10~10~10";
                    var ColumnFields = "Product~ProductDisplay~SubCategoryName~Batch~Color~Qty~MRP";
                    var Align = "L~L~L~L~L~C~C";
                    var setCtrlType = "CD~~CD~~~~";

                    cgImport = new coGrid("#DDTImport");
                    cgImport.setColumnHeading(ColumnHeading);
                    cgImport.setColumnWidthPer(ColumnWidthPer, 1200);
                    cgImport.setColumnFields(ColumnFields);
                    cgImport.setAlign(Align);
                    cgImport.defaultHeight = "300px";
                    cgImport._MinRows = 0;
                    cgImport.setIdProperty("SrNo");
                    cgImport.setCtrlType(setCtrlType);

                    var f = ColumnFields.split('~');
                    var s = setCtrlType.split('~');
                    var arrmapData = []
                    var DrpIndex = {};

                    for (kk = 0; kk < s.length; kk++) {
                        var sl = s[kk];
                        var fl = f[kk];
                        if (sl == "C" || sl == "L" || sl == "CD") {
                            DrpIndex[fl] = kk;
                            switch (fl) {
                                case "Product":
                                    cgImport.columns[kk]["event"] = trandtldropList;
                                    cgImport.columns[kk]["fieldval"] = "FkProductId";
                                    cgImport.columns[kk]["KeyID"] = "PKID";
                                    cgImport.columns[kk]["KeyValue"] = "Product";
                                    break
                                case "SubCategoryName":
                                    cgImport.columns[kk]["event"] = trandtldropList;
                                    cgImport.columns[kk]["fieldval"] = "FKProdCatgId";
                                    cgImport.columns[kk]["KeyID"] = "PkCategoryId";
                                    cgImport.columns[kk]["KeyValue"] = "CategoryName";
                                    break
                            }
                        }
                    }
                    var obdt = cgImport.populateDataFromJson({
                        srcData: data,
                        mapData: arrmapData
                    });
                    cgImport.applyAddNewRow();
                    cgImport.bind(data);

                    cgImport.outGrid.onCellChange.subscribe(function (e, args) {
                        if (args.cell != undefined) {
                            
                            var field = cgImport.columns[args.cell].field;
                            //if (field == "Product") {
                            //    var FkProductId = Common.isNullOrEmpty(args.item["Product"]) ? 0 : parseFloat(args.item["Product"]);
                            //    args.item["FkProductId"] = FkProductId; 
                            //}
                            //if (field == "SubCategoryName") {
                            //    var FKProdCatgId = Common.isNullOrEmpty(args.item["SubCategoryName"]) ? 0 : parseFloat(args.item["SubCategoryName"]);
                            //    args.item["FKProdCatgId"] = FKProdCatgId;

                            //}
                            //cg.updateRefreshDataRow(args.row);
                            //cg.updateAndRefreshTotal(); 
                            // args.grid.gotoCell(args.row, args.cell + 1, true)
                            //$(".loader").hide();
                        }
                    });

                    //---------------    ---------------   ---------------   ---------------/
                    cgImport.outGrid.onClick.subscribe(function (e, args) {

                        if (args.cell != undefined) {
                            var field = cgImport.columns[args.cell].field;


                        }
                    });


                    //alert("Upload successful!");
                }
                else
                    alert(res.msg);
            },
            error: function (xhr, status, error) {
                alert("Upload failed: " + xhr.responseText);
            }
        });

    }


}

function BindGrid_ImportedData() {
    
    var data = cgImport.getData().filter(function (element) {  return (parseInt(element.FKProdCatgId) <= 0 && !Handler.isNullOrEmpty(element.ProductDisplay)); });
    if (data.length == 0) {
        var TranReturnDetails = [];
        cgImport.getData().filter(function (element) {
            if (!Handler.isNullOrEmpty(element.ProductDisplay)) {
                TranReturnDetails.push(element);
            }
        });

        if (TranReturnDetails.length > 0) {
            // if (Handler.isNullOrEmpty(tranModel.TranDetails[rowIndex].LinkSrNo) || tranModel.TranDetails[rowIndex].LinkSrNo <= 0 || fieldName == 'Delete') {
            $(".loader").show();
            $.ajax({
                type: "POST",
                url: Handler.currentPath() + 'BindImportData',
                data: { model: tranModel, details: TranReturnDetails },
                datatype: "json",
                success: function (res) {
                    if (res.status == "success") {
                        tranModel = res.data;
                        setFooterData(tranModel);
                        setPaymentDetail(tranModel);
                        BindGrid('DDT', tranModel.TranDetails);
                        $('#ImportDatafile').val('');
                        $('.model-importdata').modal('toggle');
                        $("#DDTImport").html('');
                    } 
                    else
                        alert(res.msg);

                $(".loader").hide();
            }
            });
        // } else { alert('Promotion Artical Not Update'); BindGrid('DDT', tranModel.TranDetails); }
    } else
        alert('Please Import Data');
} else
alert('Please Select SubSection');
    // BindGrid('DDT', tranModel.TranDetails)
} 