
var tranModel = null;
var ControllerName = "";
var TranAlias = "";
$(document).ready(function () {
    Common.InputFormat();
    Load();

    TranAlias = tranModel.ExtProperties.TranAlias;
    $("#hdFormId").val(tranModel.ExtProperties.FKFormID);
    $("#hdGridName").val('dtl');
    ControllerName = $("#hdControllerName").val();
    if ((TranAlias == "SRTN" || TranAlias == "SCRN" || TranAlias == "SORD" || TranAlias == "PORD" || TranAlias == "PINV")) {
        $(".trn-barcode").hide();
    } else { $(".trn-barcode").show(); $("#txtSearchBarcode").focus(); }

    $('#btnServerSave').click(function (e) {

        if ($("#loginform1").valid()) {
            SaveRecord();
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
        BarcodeScan($(this).val());
        $(this).val('');
        $(this).focus();
    });
    $("#EntryNo").change(function () {
        DatabyEntryNo();
    });
    $("#PartyMobile").change(function () {
        GetWalkingCustomerDetail($(this).val());
    });
});

function Load() {
    
    var PkId = $("#PkId").val();
    tranModel = JSON.parse($("#hdData").val());
    if (PkId > 0) {
        console.clear();
        console.log(tranModel.TranDetails);
        $(tranModel.TranDetails).each(function (i, v) {
            //  v["Product"] = parseInt(v.FkProductId);
            v["ModeForm"] = 1;
            v["Delete"] = 'Delete';
        });
        BindGrid('DDT', tranModel.TranDetails);
        setPaymentDetail(tranModel);
    }

    else {
        BindGrid('DDT', []);

    }
}

function BindGrid(GridId, data) {

    $("#" + GridId).empty();
    Common.Grid(tranModel.ExtProperties.FKFormID, "dtl", function (s) {
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
                        cg.columns[kk]["KeyID"] = "PkProductId";
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

                        if (tranModel.ExtProperties.StockFlag == "I") {
                            args.item["FkLotId"] = 0;
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
            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cg.columns[args.cell].field;

                if (field == "Product") {
                    if ((ControllerName == "SalesReturn" || ControllerName == "SalesCrNote")) {
                        var InvoiceSrNo = Common.isNullOrEmpty(args.item["ProductName"]) ? 0 : parseFloat(args.item["ProductName"]);
                        //var data = ProductList.filter(function (element) { return (element.InvoiceSrNo == InvoiceSrNo); });
                        //var FkProductId = data.PkProductId;
                        //var data = cg.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.InvoiceSrNo == InvoiceSrNo && element.mode != 2); });
                        //if (data.length <= 0) {
                        args.item["InvoiceSrNo"] = InvoiceSrNo;
                        ColumnChange(args, args.row, "ProductReturn");
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
                        ColumnChange(args, args.row, "Product");
                        //}
                        //else {
                        //    alert("Product Already Add In List");
                        //    cg_ClearRow(args);
                        //    return false;
                        //}
                    }
                }
                else if (field == "Qty") {
                    ColumnChange(args, args.row, "Qty");
                }
                else if (field == "MRP") {
                    var MRP = parseFloat(args.item["MRP"]);
                    var Rate = parseFloat(args.item["Rate"]);
                    if (MRP < Rate) {
                        args.item["MRP"] = Rate;
                        alert('Invalid MRP');
                    }
                    ColumnChange(args, args.row, "MRP");
                }
                else if (field == "Rate") {

                    var MRP = parseFloat(args.item["MRP"]);
                    var Rate = parseFloat(args.item["Rate"]);
                    if (MRP < Rate) {
                        args.item["Rate"] = MRP;
                        alert('Invalid Rate');
                    }
                    ColumnChange(args, args.row, "Rate");
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
                    ColumnChange(args, args.row, "TradeDisc");
                }
                else if (field == "Batch") {

                    var FkLotId = args.item["FkLotId"];
                    if (FkLotId > 0) {
                        ColumnChange(args, args.row, "Batch");

                    } else {
                        if (tranModel.ExtProperties.TranType != "P") {
                            args.item["Batch"] = "";
                            cg.updateRefreshDataRow(args.row);
                        }
                    }
                }
                else if (field == "Color") {

                    var FkLotId = args.item["FkLotId"];
                    if (FkLotId > 0) {
                        ColumnChange(args, args.row, "Color");

                    } else {
                        if (tranModel.ExtProperties.TranType != "P") {
                            args.item["Color"] = "";
                            cg.updateRefreshDataRow(args.row);
                        }
                    }
                }
                else if (field == "FKInvoiceID_Text") {
                    ColumnChange(args, args.row, "Inum");
                }

            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cg.columns[args.cell].field;

                var PkProductId = args.grid.getDataItem(args.row)["PkProductId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete");
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
    args.item["ModeForm"] = 0;
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
    args.item["TradeRate"] = "";
    args.item["DistributionRate"] = "";
    args.item["MfgDate"] = "";
    args.item["FKInvoiceID"] = 0;
    args.item["InvoiceSrNo"] = 0;
    args.item["FKInvoiceSrID"] = 0;
    args.item["InvoiceDate"] = "";
    cg.updateRefreshDataRow(args.row);
}
function BarcodeScan(barcode) {
    $(".loader").show();
    tranModel.TranDetails = GetDataFromGrid();

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'BarcodeScan',
        data: { model: tranModel, barcode: barcode },
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

 
function ImportBarcode() {
    if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
        alert('The File APIs are not fully supported in this browser.');
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
            debugger;
            if (!Handler.isNullOrEmpty(event.target.result)) {
                var lst = event.target.result.split('\r\n');
                sumitImportBarcode(lst);
            } else { alert('Invalid File Data'); }
        };
        reader.readAsText($('#Barcodefile')[0].files[0])
    }

    
}
function sumitImportBarcode(barcodelist) {
    debugger;
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
                if (!Handler.isNullOrEmpty(res.ListNotFound)) { alert('Not Found Barcode is :' + res.ListNotFound)}
            }
            else
                alert(res.msg);
            $(".loader").hide();
        }
    })
}
function handleFileLoad(event) {
    debugger;
    var items = event.target.result.split(',');
    var items1 = JSON.stringify(event.target.result).split('\r\n');
    console.log(JSON.stringify(event.target.result));
    console.log(JSON.parse(JSON.stringify(event.target.result)));
   // document.getElementById('fileContent').textContent = event.target.result;
}

function ColumnChange(args, rowIndex, fieldName) {

    tranModel.TranDetails = GetDataFromGrid();

    if (tranModel.TranDetails.length > 0) {
        $(".loader").show();
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'ColumnChange',
            data: { model: tranModel, rowIndex: rowIndex, fieldName: fieldName },
            datatype: "json",
            success: function (res) {

                if (res.status == "success") {
                    tranModel = res.data;
                    setFooterData(tranModel);
                    setPaymentDetail(tranModel);

                    setGridRowData(args, tranModel.TranDetails, rowIndex, fieldName);

                }
                else
                    alert(res.msg);

                $(".loader").hide();
            }
        });
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
            console.log(res);
            if (res.status == "success") {
                console.log(tranModel);

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

    console.log(tranModel);
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

function setFooterData(data) {
    Common.Set(".trn-footer", data, "");
    return false;
}
function setPaymentDetail(data) {
    
    Common.Set(".model-paymentdetail", data, "");
    if (tranModel.FKPostAccID <= 0) { $("#Account").val(''); }
    return false;
}


function setGridRowData(args, data, rowIndex, fieldName) {

    if (fieldName == 'Delete') {
        args.grid.getDataItem(args.row).ModeForm = 2
    }
    else {

        args.item["SrNo"] = data[rowIndex].SrNo;
        args.item["PkProductId"] = data[rowIndex].PkProductId;
        args.item["FkLotId"] = data[rowIndex].FkLotId;
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
        args.item["Barcode"] = data[rowIndex].Barcode;
        args.item["Delete"] = 'Delete';

    }
    cg.updateRefreshDataRow(args.row);
    cg.updateAndRefreshTotal();
    //cg.gotoCell(args.row, args.cell + 1);
    args.grid.gotoCell(args.row, args.cell + 1, true)
    return false;
}

function GetDataFromGrid(ifForsave) {

    var array = cg.getData().filter(x => x.FkProductId > 0);
    let number = Math.max.apply(Math, array.map(function (o) { return o.SrNo; }));
    let SrNo = number > 0 ? number : 0;
    var _d = [];
    cg.getData().filter(function (element) {

        if (ifForsave) {
            if (!Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

                if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                // element.FkProductId = parseInt(element.Product);
                _d.push(element);
                return element
            }
        }
        else {


            if (!Handler.isNullOrEmpty(element.Product) || !Handler.isNullOrEmpty(element.FKInvoiceID)) {
                if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                //element.FkProductId = parseInt(element.Product);
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
            if ((tranModel.FkPartyId > 0) || (tranModel.ExtProperties.DocumentType == "C")) {
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

function trandtldropList(data) {

    var output = []
    $.ajax({
        url: Handler.currentPath() + 'trandtldropList', data: data, async: false, dataType: 'JSON', success: function (result) {

            output = result;

        }, error: function (request, status, error) {
        }
    });
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
        console.log(_List);
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

