var tranModel = null;
var ControllerName = "";
var TranAlias = "";
var ModeFormForEdit = 1;
$(document).ready(function () {
    document.addEventListener('mousedown', function (e) {
        if (!cg.outGrid.getEditorLock || !cg.outGrid.getEditorLock().isActive()) return;

        if (e.target.closest('.DDT')) return; // click inside grid -> ignore here


        try {
            cg.outGrid.getEditorLock().cancelCurrentEdit();
            $(".Editor_Slick_custdropdown").hide()
        } catch (err) {
            console.error('cancel error', err);
        }
    });


    $('#btnDeleteRecord').hide();
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
            var CodingScheme = v["CodingScheme"];
            if (CodingScheme == 'fixed')
                v["Barcode"] = "";
            else
                v["Barcode"] = "Barcode";

            v["Delete"] = 'Delete';
        });
        $(tranModel.TranReturnDetails).each(function (i, v) {
            v["ModeForm"] = ModeFormForEdit;
            var CodingScheme = v["CodingScheme"];
            if (CodingScheme == 'Unique')
                v["Barcode"] = "Barcode";
            else
                v["Barcode"] = "";

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
                var FkLotId = args.grid.getDataItem(args.row)["FkLotId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                var ModeForm = args.grid.getDataItem(args.row)["ModeForm"];
                var BarcodeText = args.grid.getDataItem(args.row)["Barcode"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete", false);

                } else if (field == "Barcode" && ModeForm != 2 && BarcodeText != "") {
                    if (FkProductId > 0) {
                        BarcodePopupUniqId_ListWith_Textbox(args, args.row);
                    }
                    else
                        alert('select Product');

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
} var cgGridUniqIdTextbox = null;

function BarcodePopupUniqId_ListWith_Textbox(args, rowIndex) {

    tranModel.TranDetails = GetDataFromGrid(true, false);
    //tranModel.TranReturnDetails = GetDataFromGrid(true, true);

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
                    case "Batch": 
                        cgOut.columns[kk]["event"] = trandtldropList;
                        cgOut.columns[kk]["fieldval"] = "FkLotId";
                        cgOut.columns[kk]["KeyID"] = "PkLotId";
                        cgOut.columns[kk]["KeyValue"] = "Batch";
                        cgOut.columns[kk]["Keyfield"] = "Batch,Color,MRP,SaleRate,PurchaseRate,TradeRate,DistributionRate";
                        cgOut.columns[kk]["RowValue"] = "FkProductId";
                        cgOut.columns[kk]["ExtraValue"] = "TranAlias,TranType"; //tranModel.ExtProperties.TranAlias;
                        break
                    case "Color":
                        cgOut.columns[kk]["event"] = trandtldropList;
                        cgOut.columns[kk]["fieldval"] = "FkLotId";
                        cgOut.columns[kk]["KeyID"] = "PkLotId";
                        cgOut.columns[kk]["KeyValue"] = "Color";
                        cgOut.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
                        cgOut.columns[kk]["RowValue"] = "FkProductId,Batch";
                        cgOut.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias; 

                        break
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
               
                if (field == "Product") {
                    args.item["SrNo"] = 0;
                    args.item["TranType"] = 'O';
                    ColumnChange(args, args.row, "Product", true);
                }
                else if (field == "Qty") {

                }
                else if (field == "MRP") {

                } else if (field == "Batch") {
                    var FkLotId = args.item["FkLotId"];
                    if (FkLotId > 0) {
                        ColumnChange(args, args.row, "Batch", true);

                    }   
                }
                else if (field == "Color") {

                    var FkLotId = args.item["FkLotId"];
                    if (FkLotId > 0) {
                        ColumnChange(args, args.row, "Color");

                    } 
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
                var FkLotId = args.grid.getDataItem(args.row)["FkLotId"];
                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
                var ModeForm = args.grid.getDataItem(args.row)["ModeForm"];
                var BarcodeText = args.grid.getDataItem(args.row)["Barcode"];
                if (field == "Delete") {
                    ColumnChange(args, args.row, "Delete", true);
                }
                else if (field == "Barcode" && ModeForm != 2 && BarcodeText != "") {
                    if (FkProductId > 0 && FkLotId > 0) {
                        var CodingScheme = args.grid.getDataItem(args.row)["CodingScheme"];
                        if (CodingScheme == 'Unique')
                            BarcodePopupUniqId_CheckboxList(args, args.row);
                    }
                    else
                        alert('select Size');

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

function BarcodePopupUniqId_CheckboxList(args, rowIndex) {

  //  tranModel.TranDetails = GetDataFromGrid(true, false);
    tranModel.TranReturnDetails = GetDataFromGrid(true, true);

    if (tranModel.TranReturnDetails.length > 0) {
        $(".loader").show();
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'BarcodeList',
            data: { model: tranModel, rowIndex: rowIndex, IsReturn: true },
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

                console.log(res.data);
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
                        let SrNo = tranModel.TranReturnDetails[rowIndex].SrNo;
                        tranModel.UniqIdReturnDetails = tranModel.UniqIdReturnDetails.filter((u) => {
                            return u.SrNo != SrNo
                        })
                        var _existBarcode = [];
                        $(_List).each(function (i, v) {
                            console.log(v);
                            var _exist = $.grep(tranModel.UniqIdReturnDetails, function (item) {
                                return item.Barcode == v.Barcode;
                            });

                            if (_exist.length == 0) {
                                tranModel.UniqIdReturnDetails.push({ SrNo: v.SrNo, Barcode: v.Barcode })
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
function GetAndCheckBarcodeQty(_d, IsReturn) {
    var _NotFound = []
    _d.filter(function (element) {

        let totalQty = (parseFloat(element.Qty) + parseFloat(element.FreeQty))
        if (IsReturn) {
            var _srnoList = tranModel.UniqIdReturnDetails.filter((u) => { return u.SrNo == element.SrNo });
            if (_srnoList.length != totalQty && element.ModeForm != 2 && element.CodingScheme == 'Unique')
                _NotFound.push(element.Product);
        }
        else {
            var _srnoList = tranModel.UniqIdDetails.filter((u) => { return u.SrNo == element.SrNo });
            if (_srnoList.length != totalQty && element.ModeForm != 2 && element.CodingScheme == 'Unique')
                _NotFound.push(element.Product);
        }
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
            tranModel.TranReturnDetails = [];

            if (tranModel.FkPartyId > 0) {
                if (tranModel.FKSeriesId > 0) {

                    tranModel.TranDetails = GetDataFromGrid(true, false);
                    tranModel.TranReturnDetails = GetDataFromGrid(true, true);

                    var filteredDetails = tranModel.TranDetails.filter(x => x.ModeForm != 2);
                    var filteredReturnDetails = tranModel.TranReturnDetails.filter(x => x.ModeForm != 2);
                    if (tranModel.TranDetails.length > 0 && filteredDetails.length > 0 && tranModel.TranReturnDetails.length > 0 && filteredReturnDetails.length > 0) {
                       // var _NotMatch = GetAndCheckBarcodeQty(tranModel.TranDetails, false);
                        var _NotMatchReturn = GetAndCheckBarcodeQty(tranModel.TranReturnDetails, true);
                        //if (_NotMatch.length == 0 || tranModel.TranAlias == 'PJ_O' || tranModel.TranAlias == 'PJ_I' || tranModel.TranAlias == 'PJ_R') {
                            if (_NotMatchReturn.length == 0 || tranModel.TranAlias == 'PJ_O' || tranModel.TranAlias == 'PJ_R') {

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
                                    , error: function (xhr, status, error) {
                                        if (xhr.status === 400) {
                                            // Handle Bad Request
                                            let errorMessage = xhr.responseJSON?.message || xhr.responseText || "Bad Request";
                                            alert("Error 400: " + errorMessage);
                                        } else {
                                            alert("Error: " + xhr.status + " - " + error);
                                        }
                                        $(".loader").hide();
                                    }
                                });
                            } else
                                alert("Barcode And Qty Not Match Product : " + _NotMatchReturn.join(",") + " On Product to be Issued");
                        //} else
                        //    alert("Barcode And Qty Not Match Product : " + _NotMatch.join(",") + " Product to be Received");

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
            
            console.log(data[rowIndex]);
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

            if (IsReturn) {
                if (CodingScheme == 'Unique')
                    args.item["Barcode"] = "Barcode";
                else
                    args.item["Barcode"] = "";
            }
            else {

                if (CodingScheme == 'fixed' || tranModel.TranAlias == "PORD")
                    args.item["Barcode"] = "";
                else
                    args.item["Barcode"] = "Barcode";
            }
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

