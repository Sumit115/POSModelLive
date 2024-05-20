
var tranModel = null;
$(document).ready(function () {
    $("#txtSearchBarcode").focus();
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

    $("#txtSearchBarcode").change(function () {
        BarcodeScan($(this).val());
        $(this).val('');
        $(this).focus();
    });

});

function Load() {
    var PkId = $("#PkId").val();
    tranModel = JSON.parse($("#hdData").val());
    if (PkId > 0) {
        $(tranModel.TranDetails).each(function (i, v) {
            v["Product"] = parseInt(v.FkProductId);
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
        //var ProductList = JSON.parse($("#hdProductList").val());
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
        //columns.push({ id: v.FieldName, name: v.Caption, field: v.FieldName, width: colwidth, editor: Slick.Editors.Dropdown2, 
        //cssClass: "text-edit", fieldval: "FKSalesPerID", event: SalesPersonList, KeyID: "PKID", KeyValue: "Employee", 
        //Keyfield: "Employee,Code,Prefix,Address,Pincode,Email,PAN,DOB,DOW,DOJ,PaymentMode,EmpGender,MaritalStatus,Phone1,Phone2,
        //Fax, BloodGroups, Bank, BankAccNo, PFAccNo, ESIAccNo, SecurityID, Reference, Locality, Station, Department, Designation, 
        //Qualification, Branch, Status, UserName"
        //});
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
                        //cg.columns[KK]["Keyfield"] = "Product";
                        //cg.setOptionArray(kk, ProductList, "ProductName", false, "Product", "PkProductId", "1");
                        //arrmapData.push({ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" });
                        break
                    case "Batch":
                        cg.columns[kk]["event"] = trandtldropList;
                        cg.columns[kk]["fieldval"] = "Batch";
                        cg.columns[kk]["KeyID"] = "PkLotId";
                        cg.columns[kk]["KeyValue"] = "Batch";
                        cg.columns[kk]["Keyfield"] = "Batch";
                        cg.columns[kk]["RowValue"] = "FkProductId";
                        
                        break
                    case "Color":
                        cg.columns[kk]["event"] = trandtldropList;
                        cg.columns[kk]["fieldval"] = "Color";
                        cg.columns[kk]["KeyID"] = "Color";
                        cg.columns[kk]["KeyValue"] = "Color";
                        cg.columns[kk]["Keyfield"] = "Color";
                        cg.columns[kk]["RowValue"] = "FkProductId";
                        break
                    case "MRP":
                        cg.columns[kk]["event"] = trandtldropList;
                        cg.columns[kk]["fieldval"] = "MRP";
                        cg.columns[kk]["KeyID"] = "MRP";
                        cg.columns[kk]["KeyValue"] = "MRP";
                        cg.columns[kk]["Keyfield"] = "MRP";
                        cg.columns[kk]["RowValue"] = "FkProductId";
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

                if (field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
                    alert("Select Product Frist");
                    cg_ClearRow(args)
                    cg.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
                }
            }
        });
        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {
                
                var field = cg.columns[args.cell].field;
                if (field == "Product") {
                    var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);

                    var data = cg.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.mode != 2); });

                    if (data.length <= 1) {
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
                    ColumnChange(args, args.row, "Batch");
                }
                else if (field == "Color") {
                    ColumnChange(args, args.row, "Color");
                }
            }
        });

        //---------------    ---------------   ---------------   ---------------/
        cg.outGrid.onClick.subscribe(function (e, args) {

            if (args.cell != undefined) {
                var field = cg.columns[args.cell].field;

                var PkProductId = args.grid.getDataItem(args.row)["FkProductId"];
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
    args.item["FkProductId"] = 0;
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
function BarcodeScan(barcode) {
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'BarcodeScan',
        data: { model: tranModel, barcode: barcode },
        datatype: "json", success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $(tranModel.TranDetails).each(function (i, v) {
                    v["ProductName"] = parseInt(v.FkProductId);
                });
                BindGrid('DDT', tranModel.TranDetails);

                setFooterData(tranModel);
                setPaymentDetail(tranModel);

            }
            else
                alert(res.msg);
        }
    })
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
                    setFooterData(tranModel);
                    setPaymentDetail(tranModel);
                    setGridRowData(args, tranModel.TranDetails, rowIndex, fieldName);

                }
                else
                    alert(res.msg);
            }
        });
    }
}

function FooterChange(fieldName) {

    console.log(tranModel);

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'FooterChange',
        data: { model: tranModel, fieldName: fieldName },
        datatype: "json", success: function (res) {
            console.log(res);
            if (res.status == "success") {

                tranModel = res.data;

                //if (fieldName == "CashDiscType" || fieldName == "CashDiscount") {
                //    $(tranModel.TranDetails).each(function (i, v) {
                //        v["ProductName"] = parseInt(v.FkProductId);
                //    });
                //    BindGrid('DDT', tranModel.TranDetails);
                //    //$("#hdData").val(JSON.stringify(tranModel));
                //    //Load();
                //}

                setFooterData(tranModel);
                setPaymentDetail(tranModel);

            }
            else
                alert(res.msg);
        }
    })
}
function PaymentDetail() {
    console.log(tranModel);

    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetPaymentDetail',
        data: { model: tranModel },
        datatype: "json", success: function (res) {

            if (res.status == "success") {
                tranModel = res.data;

                setPaymentDetail(tranModel);
                $('.model-paymentdetail').modal('toggle');;
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
function setPaymentDetail(data) {
    Common.Set(".model-paymentdetail", data, "");
    return false;
}


function setGridRowData(args, data, rowIndex, fieldName) {

    if (fieldName == 'Delete') {
        args.grid.getDataItem(args.row).ModeForm = 2
    }
    else {
        args.item["FkProductId"] = data[rowIndex].FkProductId;
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
            if (!Handler.isNullOrEmpty(element.Product) && !Handler.isNullOrEmpty(element.Qty)) {

                if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
                _d.push(element);
                return element
            }
        }
        else {


            if (!Handler.isNullOrEmpty(element.Product)) {
                if (element.FkProductId > 0) { element.SrNo = element.SrNo; }
                else { SrNo++; element.SrNo = SrNo; }
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
        h["Product"] = "sobc";
        focu["Product"] = { "focusable": false };
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