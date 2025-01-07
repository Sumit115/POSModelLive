var Model = null;
var cgIn = null;
var cgOut = null;

$(document).ready(function () {

    Common.InputFormat();
    //$('#btnServerSave').click(function (e) {
    //    if ($("#loginform1").valid()) {
    //        SaveRecord();
    //    }
    //    return false;
    //});
    bindProduct_In();
    bindBatch_In();
    bindColor_In();
    bindProduct_Out();
    bindBatch_Out();
    bindColor_Out();
    Load();
});
function Load() {
    var PkRecipeId = $("#PkRecipeId").val();
    Model = JSON.parse($("#hdData").val());
    if (PkRecipeId > 0) {

        //BindGrid('DDT', JSON.parse($("#hdGridIn").val()), Model.VoucherDetails);
        //BindGrid('DDT', JSON.parse($("#hdGridOut").val()), Model.VoucherDetails);
    }

    else {

        //BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), []);
        //BindGridOut('DDTOut', JSON.parse($("#hdGridOut").val()), []);

    }
}

function bindProduct_In() {
    _Custdropdown["FkProductId_In"] = new CustomDDL("FkProductId_In", "#drpListFkProductId_In span");

    _Custdropdown.FkProductId_In.onLoad.call(function (arg) {

        arg["name"] = 'Product';
        //arg["InvSrNo"] = TranModel.TransDetail[TranModel.CurrentIndex].SrNo;
        var result = "";
        $.ajax({
            url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList',
            async: false,
            type: 'GET',
            data: arg,
            dataType: 'JSON',
            success: function (res) {
                result = res;


            }
        });
        return result;
    })
}
function bindBatch_In() {
    _Custdropdown["Batch_In"] = new CustomDDL("Batch_In", "#drpListBatch_In span");

    _Custdropdown.Batch_In.onLoad.call(function (arg) {
        if ($("#FkProductId_In").val() > 0) {

            arg["FkProductId"] = $("#FkProductId_In").val();
            //Common.ajax(Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct?FkProductId=' + FkProductId, {}, "Please Wait...", function (res) {
            //    Handler.hide();
            //    result = res;
            //});

            //arg["InvSrNo"] = TranModel.TransDetail[TranModel.CurrentIndex].SrNo;
            var result = "";
            $.ajax({
                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct',
                async: false,
                type: 'POST',
                data: arg,
                dataType: 'JSON',
                success: function (res) {

                    result = res;


                }
            });
            return result;
        } else { alert('Please Select Product'); }
    })
}
function bindColor_In() {
    $('#Color_In').autocomplete({
        // minLength: 3,
        source: function (request, response) {
            var FkProductId = $("#FkProductId_In").val();

            var data = { name: "Color", pageNo: 1, pageSize: 1000, search: request.term, RowParam: FkProductId, ExtraParam: "" };

            $.ajax({
                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
                    Handler.hide();
                    if (res.length > 0)
                        response($.map(res, function (item) {

                            return { label: item.Color, value: item.Color }; //updated code
                        }));

                }, error: function (request, status, error) {
                    alert("Error");
                }
            });


        },
        select: function (i, v) {

            //var _md = v.item.Model;
            //$("#Name").val(_md.Name);
            //$("#Mobile").val(_md.Mobile);
            //$("#UserId").val(_md.UserId);
            //$("#Address").val(_md.FullAddress);
            //$("#txtAccountBalance").val(_md.AccountBalance);
            //$("#Mobile,#txtAccountBalance,#Name").attr("readonly", "readonly");
            //var checkRunData = $("#CheckRunDescription");
            //var checkRunID = $("#CheckRunID");
            //checkRunData.val(ui.item.label);
            //checkRunID.val(ui.item.value);


        }
    });
}

function AddProduct_In() {
    debugger;
    var rowCount = $('#tblProduct_In tbody tr').length;
    var allValues = $('#tblProduct_In tbody tr input[class="SrNo"]').map(function () { return +this.value; }).toArray();
    var SrNo = Math.max.apply(Math, allValues);
    SrNo = SrNo > 0 ? SrNo + 1 : 1;
    var FkProductId = $("#FkProductId_In").val();
    if (FkProductId > 0) {

        var html = '<tr index="' + rowCount + '">';
        html += '<td class="tabel-td-xs">  <input  id="Recipe_dtl_' + rowCount + '__Product" name="Recipe_dtl[' + rowCount + '].Product" type="text" value="' + $("#drpFkProductId_In").val() + '" tabindex="-1"> </td>';
        html += '<td class="tabel-td-xs">  <input  id="Recipe_dtl_' + rowCount + '__Batch" name="Recipe_dtl[' + rowCount + '].Batch" type="text" value="' + $("#drpBatch_In").val() + '" tabindex="-1"> </td>';
        html += '<td class="tabel-td-xs">  <input  id="Recipe_dtl_' + rowCount + '__Color" name="Recipe_dtl[' + rowCount + '].Color" type="text" value="' + $("#Color_In").val() + '" tabindex="-1"> </td>';
        html += '<td class="tabel-td-xs">';
        html += '<input id="Recipe_dtl_' + rowCount + '__TranType" name="Recipe_dtl[' + rowCount + '].TranType" type="hidden"   value="I">';
        html += '<input id="Recipe_dtl_' + rowCount + '__SrNo" name="Recipe_dtl[' + rowCount + '].SrNo" type="hidden" class="SrNo" value="' + SrNo + '">';
        html += '<input id="Recipe_dtl_' + rowCount + '__FkProductId" name="Recipe_dtl[' + rowCount + '].FkProductId" type="hidden" value="' + $("#FkProductId_In").val() + '">';
        html += '<span class="action-icon" onclick="UpdateSize(this,' + rowCount + ',\'del\')"><i class="fa fa-trash" /></span> </td>';

        html += '</tr>';

        $("#tblProduct_In tbody").append(html);

        //  $("#txtSize").val('');
        //$("#FklocalityGridId").prop('selectedIndex', 0);
        //DropDownReset('FklocalityGridId');

    }
    else { alert("Insert Product"); }
    // }
    return false;
}
//-----------------------------------OUT-----------------
function bindProduct_Out() {
    _Custdropdown["FkProductId_Out"] = new CustomDDL("FkProductId_Out", "#drpListFkProductId_Out span");

    _Custdropdown.FkProductId_Out.onLoad.call(function (arg) {

        arg["name"] = 'Product';
        //arg["InvSrNo"] = TranModel.TransDetail[TranModel.CurrentIndex].SrNo;
        var result = "";
        $.ajax({
            url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList',
            async: false,
            type: 'GET',
            data: arg,
            dataType: 'JSON',
            success: function (res) {
                result = res;


            }
        });
        return result;
    })
}
function bindBatch_Out() {
    _Custdropdown["Batch_Out"] = new CustomDDL("Batch_Out", "#drpListBatch_Out span");

    _Custdropdown.Batch_Out.onLoad.call(function (arg) {
        if ($("#FkProductId_Out").val() > 0) {

            arg["FkProductId"] = $("#FkProductId_Out").val();
            //Common.ajax(Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct?FkProductId=' + FkProductId, {}, "Please Wait...", function (res) {
            //    Handler.hide();
            //    result = res;
            //});

            //arg["InvSrNo"] = TranModel.TransDetail[TranModel.CurrentIndex].SrNo;
            var result = "";
            $.ajax({
                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct',
                async: false,
                type: 'POST',
                data: arg,
                dataType: 'JSON',
                success: function (res) {

                    result = res;


                }
            });
            return result;
        } else { alert('Please Select Product'); }
    })
}
function bindColor_Out() {
    $('#Color_Out').autocomplete({
        // minLength: 3,
        source: function (request, response) {
            var FkProductId = $("#FkProductId_Out").val();

            var data = { name: "Color", pageNo: 1, pageSize: 1000, search: request.term, RowParam: FkProductId, ExtraParam: "" };

            $.ajax({
                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
                    Handler.hide();
                    if (res.length > 0)
                        response($.map(res, function (item) {

                            return { label: item.Color, value: item.Color }; //updated code
                        }));

                }, error: function (request, status, error) {
                    alert("Error");
                }
            });


        },
        select: function (i, v) {

            //var _md = v.item.Model;
            //$("#Name").val(_md.Name);
            //$("#Mobile").val(_md.Mobile);
            //$("#UserId").val(_md.UserId);
            //$("#Address").val(_md.FullAddress);
            //$("#txtAccountBalance").val(_md.AccountBalance);
            //$("#Mobile,#txtAccountBalance,#Name").attr("readonly", "readonly");
            //var checkRunData = $("#CheckRunDescription");
            //var checkRunID = $("#CheckRunID");
            //checkRunData.val(ui.item.label);
            //checkRunID.val(ui.item.value);


        }
    });
}

function AddProduct_Out() {
    debugger;
    var rowCount = $('#tblProduct_Out tbody tr').length;
    var allValues = $('#tblProduct_Out tbody tr input[class="SrNo"]').map(function () { return +this.value; }).toArray();
    var SrNo = Math.max.apply(Math, allValues);
    SrNo = SrNo > 0 ? SrNo + 1 : 1;
    var FkProductId = $("#FkProductId_Out").val();
    if (FkProductId > 0) {

        var html = '<tr index="' + rowCount + '">';
        html += '<td class="tabel-td-xs">  <input  id="Recipe_dtl_' + rowCount + '__Product" name="Recipe_dtl[' + rowCount + '].Product" type="text" value="' + $("#drpFkProductId_Out").val() + '" tabindex="-1"> </td>';
        html += '<td class="tabel-td-xs">  <input  id="Recipe_dtl_' + rowCount + '__Batch" name="Recipe_dtl[' + rowCount + '].Batch" type="text" value="' + $("#drpBatch_Out").val() + '" tabindex="-1"> </td>';
        html += '<td class="tabel-td-xs">  <input  id="Recipe_dtl_' + rowCount + '__Color" name="Recipe_dtl[' + rowCount + '].Color" type="text" value="' + $("#Color_Out").val() + '" tabindex="-1"> </td>';
        html += '<td class="tabel-td-xs">';
        html += '<input id="Recipe_dtl_' + rowCount + '__TranType" name="Recipe_dtl[' + rowCount + '].TranType" type="hidden"   value="I">';
        html += '<input id="Recipe_dtl_' + rowCount + '__SrNo" name="Recipe_dtl[' + rowCount + '].SrNo" type="hidden" class="SrNo" value="' + SrNo + '">';
        html += '<input id="Recipe_dtl_' + rowCount + '__FkProductId" name="Recipe_dtl[' + rowCount + '].FkProductId" type="hidden" value="' + $("#FkProductId_Out").val() + '">';
        html += '<span class="action-icon" onclick="UpdateSize(this,' + rowCount + ',\'del\')"><i class="fa fa-trash" /></span> </td>';

        html += '</tr>';

        $("#tblProduct_Out tbody").append(html);

        //  $("#txtSize").val('');
        //$("#FklocalityGridId").prop('selectedIndex', 0);
        //DropDownReset('FklocalityGridId');

    }
    else { alert("Insert Product"); }
    // }
    return false;
}
function UpdateSize(obj, index, action) {
    //if (action === 'edit') {
    //    $("#hidSizeIndex").val(index);
    //    $("#FklocalityGridId").val($("#Recipe_dtl_lst_" + index + "__FKSizeID").val());
    //    // $("#drpFklocalityGridId").val($("#Recipe_dtl_lst_" + index + "__Size").val());
    //    $("#OpeningBalance").val($("#Recipe_dtl_lst_" + index + "__Size").val());
    //    $('#thDrCr').find("input[value='" + $("#Recipe_dtl_lst_" + index + "__type").val() + "']").prop('checked', true);
    //    $(obj).closest('tr').removeClass('tbl-delete');
    //} else

    if (action === 'del') {
        $(obj).closest('tr').remove();
        //$(obj).closest('tr').addClass('tbl-delete');
        //$("#Recipe_dtl_lst_" + index + "__Mode").val('2');
    }
}





//function BindGridIn(GridId, GridStructerJson, data) {

//    $("#" + GridId).empty();
//    Common.GridFromJson(GridStructerJson, function (s) {
//        //var ProductList = (ControllerName == "SalesReturn" || ControllerName == "SalesCrNote") ? [] : JSON.parse($("#hdProductList").val());
//        var ProductLotList = [];
//        cgIn = new coGrid("#" + GridId);
//        cgIn.setColumnHeading(s.ColumnHeading);
//        cgIn.setColumnWidthPer(s.ColumnWidthPer, 1200);
//        cgIn.setColumnFields(s.ColumnFields);
//        cgIn.setAlign(s.Align);
//        cgIn.defaultHeight = "300px";
//        cgIn._MinRows = 50;
//        cgIn.setIdProperty("sno");
//        cgIn.setCtrlType(s.setCtrlType);

//        var f = s.ColumnFields.split('~');
//        var s = s.setCtrlType.split('~');
//        console.log(s);
//        var arrmapData = []
//        var DrpIndex = {};

//        for (kk = 0; kk < s.length; kk++) {
//            var sl = s[kk];
//            var fl = f[kk];
//            if (sl == "C" || sl == "L" || sl == "CD") {
//                DrpIndex[fl] = kk;
//                switch (fl) {
//                    case "Product":
//                        cgIn.columns[kk]["event"] = trandtldropList;
//                        cgIn.columns[kk]["fieldval"] = "FkProductId";
//                        cgIn.columns[kk]["KeyID"] = "PkProductId";
//                        cgIn.columns[kk]["KeyValue"] = "Product";
//                        break
//                    //case "Color":
//                    //    cgIn.columns[kk]["event"] = trandtldropList;
//                    //    cgIn.columns[kk]["fieldval"] = "Color";
//                    //    cgIn.columns[kk]["KeyID"] = "Color";
//                    //    cgIn.columns[kk]["KeyValue"] = "Color";
//                    //    cgIn.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
//                    //    cgIn.columns[kk]["RowValue"] = "FkProductId";
//                    //    cgIn.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias; 

//                    //    break
//                }
//            }
//        }
//        var obdt = cgIn.populateDataFromJson({
//            srcData: data,
//            mapData: arrmapData
//        });
//        cgIn.applyAddNewRow();
//        cgIn.bind(data);


//        /*---------------    ---------------   ---------------   ---------------*/
//        cgIn.outGrid.onBeforeEditCell.subscribe(function (e, args) {
//            if (args.cell != undefined) {

//                var field = cgIn.columns[args.cell].field;

//                if (field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
//                    alert("Select Product Frist");
//                    cg_ClearRow(cgIn, args)
//                    cgIn.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
//                }
//                else {
//                    if (field == "Batch" || field == "Color" || field == "Qty") {
//                        var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);
//                        if (field == "Batch") {
//                            Common.ajax(Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct?FkProductId=' + FkProductId, {}, "Please Wait...", function (res) {
//                                Handler.hide();
//                                cgIn.setOptionArray(DrpIndex["Batch"], res, "Batch", false, "Size", "Size", "1");
//                            });
//                        }
//                        if (field == "Color") {
//                            var data = { name: field, pageNo: 1, pageSize: 1000, search: '', RowParam: FkProductId, ExtraParam: "" };

//                            $.ajax({
//                                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
//                                    Handler.hide();
//                                    if (res.length > 0)
//                                        cgIn.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
//                                }, error: function (request, status, error) {
//                                }
//                            });
//                            //Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
//                            //    Handler.hide();
//                            //    cgIn.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
//                            //});
//                        }
//                    }
//                }
//            }
//        });
//        //---------------    ---------------   ---------------   ---------------/
//        cgIn.outGrid.onCellChange.subscribe(function (e, args) {
//            if (args.cell != undefined) {

//                var field = cgIn.columns[args.cell].field;
//                var FkRecipeId = args.item["FkRecipeId"]

//                if (field == "Product") {

//                    args.item["FkRecipeId"] = "0";
//                    var array = cgIn.getData().filter(x => x.FkProductId > 0);
//                    let number = Math.max.apply(Math, array.map(function (o) { return o.SrNo; }));
//                    let SrNo = number > 0 ? number : 0;
//                    if (FkRecipeId > 0) { }
//                    else { args.item["SrNo"] = (SrNo + 1); }

//                    args.item["TranType"] = 'I';
//                }
//                else if (field == "Qty") {

//                }
//                else if (field == "MRP") {

//                }
//                cgIn.updateRefreshDataRow(args.row);
//                cgIn.updateAndRefreshTotal();
//                //cgIn.gotoCell(args.row, args.cell + 1);
//                args.grid.gotoCell(args.row, args.cell, true)
//            }
//        });


//        //---------------    ---------------   ---------------   ---------------/
//        cgIn.outGrid.onClick.subscribe(function (e, args) {

//            if (args.cell != undefined) {
//                var field = cgIn.columns[args.cell].field;

//                var FkProductId = args.grid.getDataItem(args.row)["FkProductId"];
//                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
//                if (field == "Delete") {
//                    var data = $.grep(cgIn.getData(), function (e) {
//                        return e.SrNo != SrNo;
//                    });
//                    // console.log(data);
//                    BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), data);

//                }

//            }
//        });

//        //---------------    ---------------   ---------------   ---------------/
//        cgIn.outGrid.onContextMenu.subscribe(function (e, args) {

//            e.preventDefault();
//            var j = cgIn.outGrid.getCellFromEvent(e);
//            $("#contextMenu")
//                .data("row", j.row)
//                .css("top", e.pageY - 90)
//                .css("left", e.pageX - 60)
//                .show();
//            $("body").one("click", function () {
//                $("#contextMenu").hide();
//            });
//        });
//        $("#contextMenu").click(function (e) {
//            if (!$(e.target).is("li")) {
//                return;
//            }
//            if (!UDI.outGrid.getEditorLock().commitCurrentEdit()) {
//                return;
//            }

//            var row = $(this).data("row");
//            var command = $(e.target).attr("data");
//            if (command == "EditColumn") {
//                Common.GridColSetup(tranModel.ExtProperties.FKFormID, "dtl", function () {
//                    var _dtl = GetDataFromGrid();
//                    //  BindGrid('DDT', _dtl);
//                });
//            }

//        });

//        //---------------    ---------------   ---------------   ---------------/
//    });
//}
//function BindGridOut(GridId, GridStructerJson, data) {

//    $("#" + GridId).empty();
//    Common.GridFromJson(GridStructerJson, function (s) {
//        //var ProductList = (ControllerName == "SalesReturn" || ControllerName == "SalesCrNote") ? [] : JSON.parse($("#hdProductList").val());
//        var ProductLotList = [];
//        cgOut = new coGrid("#" + GridId);
//        cgOut.setColumnHeading(s.ColumnHeading);
//        cgOut.setColumnWidthPer(s.ColumnWidthPer, 1200);
//        cgOut.setColumnFields(s.ColumnFields);
//        cgOut.setAlign(s.Align);
//        cgOut.defaultHeight = "300px";
//        cgOut._MinRows = 50;
//        cgOut.setIdProperty("sno");
//        cgOut.setCtrlType(s.setCtrlType);

//        var f = s.ColumnFields.split('~');
//        var s = s.setCtrlType.split('~');
//        console.log(s);
//        var arrmapData = []
//        var DrpIndex = {};

//        for (kk = 0; kk < s.length; kk++) {
//            var sl = s[kk];
//            var fl = f[kk];
//            if (sl == "C" || sl == "L" || sl == "CD") {
//                DrpIndex[fl] = kk;
//                switch (fl) {
//                    case "Product":
//                        cgOut.columns[kk]["event"] = trandtldropList;
//                        cgOut.columns[kk]["fieldval"] = "FkProductId";
//                        cgOut.columns[kk]["KeyID"] = "PkProductId";
//                        cgOut.columns[kk]["KeyValue"] = "Product";
//                        break
//                    //case "Color":
//                    //    cgOut.columns[kk]["event"] = trandtldropList;
//                    //    cgOut.columns[kk]["fieldval"] = "Color";
//                    //    cgOut.columns[kk]["KeyID"] = "Color";
//                    //    cgOut.columns[kk]["KeyValue"] = "Color";
//                    //    cgOut.columns[kk]["Keyfield"] = "Color";//,Batch,MRP,SaleRate,PurchaseRate";
//                    //    cgOut.columns[kk]["RowValue"] = "FkProductId";
//                    //    cgOut.columns[kk]["ExtraValue"] = ""; //tranModel.ExtProperties.TranAlias; 

//                    //    break
//                }
//            }
//        }
//        var obdt = cgOut.populateDataFromJson({
//            srcData: data,
//            mapData: arrmapData
//        });
//        cgOut.applyAddNewRow();
//        cgOut.bind(data);


//        /*---------------    ---------------   ---------------   ---------------*/
//        cgOut.outGrid.onBeforeEditCell.subscribe(function (e, args) {
//            if (args.cell != undefined) {

//                var field = cgOut.columns[args.cell].field;

//                if (field != "Product" && Common.isNullOrEmpty(args.item["Product"])) {
//                    alert("Select Product Frist");
//                    cg_ClearRow(cgOut, args)
//                    cgOut.outGrid.gotoCell(args.row, DrpIndex["Product"], true);
//                }
//                else {
//                    if (field == "Batch" || field == "Color" || field == "Qty") {
//                        var FkProductId = Common.isNullOrEmpty(args.item["FkProductId"]) ? 0 : parseFloat(args.item["FkProductId"]);
//                        if (field == "Batch") {
//                            Common.ajax(Handler.rootPath() + 'Transactions/PurchaseInvoice/CategorySizeListByProduct?FkProductId=' + FkProductId, {}, "Please Wait...", function (res) {
//                                Handler.hide();
//                                cgOut.setOptionArray(DrpIndex["Batch"], res, "Batch", false, "Size", "Size", "1");
//                            });
//                        }
//                        if (field == "Color") {
//                            var data = { name: field, pageNo: 1, pageSize: 1000, search: '', RowParam: FkProductId, ExtraParam: "" };

//                            $.ajax({
//                                url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (res) {
//                                    Handler.hide();
//                                    if (res.length > 0)
//                                        cgOut.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
//                                }, error: function (request, status, error) {
//                                }
//                            });
//                            //Common.ajax(Handler.currentPath() + "ProductLotDtlList?FkProductId=" + FkProductId + "&Batch=" + Batch + "&Color=" + Color + "", {}, "Please Wait...", function (res) {
//                            //    Handler.hide();
//                            //    cgOut.setOptionArray(DrpIndex["Color"], res, "Color", false, "Color", "Color", "1");
//                            //});
//                        }
//                    }
//                }
//            }
//        });
//        //---------------    ---------------   ---------------   ---------------/
//        cgOut.outGrid.onCellChange.subscribe(function (e, args) {
//            if (args.cell != undefined) {

//                var field = cgOut.columns[args.cell].field;
//                var FkRecipeId = args.item["FkRecipeId"]

//                if (field == "Product") {
//                    args.item["FkRecipeId"] = "0";
//                    var array = cgOut.getData().filter(x => x.FkProductId > 0);
//                    let number = Math.max.apply(Math, array.map(function (o) { return o.SrNo; }));
//                    let SrNo = number > 0 ? number : 0;
//                    if (FkRecipeId > 0) { }
//                    else { args.item["SrNo"] = (SrNo + 1); }
//                    args.item["TranType"] = 'O';
//                }
//                else if (field == "Qty") {

//                }
//                else if (field == "MRP") {

//                }
//                cgOut.updateRefreshDataRow(args.row);
//                cgOut.updateAndRefreshTotal();
//                //cgOut.gotoCell(args.row, args.cell + 1);
//                args.grid.gotoCell(args.row, args.cell, true)
//            }
//        });


//        //---------------    ---------------   ---------------   ---------------/
//        cgOut.outGrid.onClick.subscribe(function (e, args) {

//            if (args.cell != undefined) {
//                var field = cgOut.columns[args.cell].field;

//                var FkProductId = args.grid.getDataItem(args.row)["FkProductId"];
//                var SrNo = args.grid.getDataItem(args.row)["SrNo"];
//                if (field == "Delete") {
//                    var data = $.grep(cgOut.getData(), function (e) {
//                        return e.SrNo != SrNo;
//                    });
//                    // console.log(data);
//                    BindGridIn('DDTIn', JSON.parse($("#hdGridIn").val()), data);

//                }

//            }
//        });

//        //---------------    ---------------   ---------------   ---------------/
//        cgOut.outGrid.onContextMenu.subscribe(function (e, args) {

//            e.preventDefault();
//            var j = cgOut.outGrid.getCellFromEvent(e);
//            $("#contextMenu")
//                .data("row", j.row)
//                .css("top", e.pageY - 90)
//                .css("left", e.pageX - 60)
//                .show();
//            $("body").one("click", function () {
//                $("#contextMenu").hide();
//            });
//        });
//        $("#contextMenu").click(function (e) {
//            if (!$(e.target).is("li")) {
//                return;
//            }
//            if (!UDI.outGrid.getEditorLock().commitCurrentEdit()) {
//                return;
//            }

//            var row = $(this).data("row");
//            var command = $(e.target).attr("data");
//            if (command == "EditColumn") {
//                Common.GridColSetup(tranModel.ExtProperties.FKFormID, "dtl", function () {
//                    var _dtl = GetDataFromGrid();
//                    //  BindGrid('DDT', _dtl);
//                });
//            }

//        });

//        //---------------    ---------------   ---------------   ---------------/
//    });
//}
//function cg_ClearRow(grid, args) {

//    //args.item["FkRecipeId"] = "0";
//    args.item["SrNo"] = 0;
//    args.item["FkProductId"] = 0;
//    args.item["ProductName_Text"] = "";
//    args.item["TranType"] = "";
//    args.item["MRP"] = "";
//    args.item["Qty"] = "";
//    args.item["Batch"] = "";
//    args.item["Color"] = "";
//    grid.updateRefreshDataRow(args.row);
//}
//function trandtldropList(data) {

//    var output = []
//    $.ajax({
//        url: Handler.rootPath() + 'Transactions/PurchaseInvoice/trandtldropList', data: data, async: false, dataType: 'JSON', success: function (result) {

//            output = result;

//        }, error: function (request, status, error) {
//        }
//    });
//    return output;

//}
//function GetDataFromGrid() {

//    var _d = [];
//    cgIn.getData().filter(function (element) {
//        if (!Handler.isNullOrEmpty(element.Product) && element.FkProductId > 0) {
//            _d.push(element);
//            return element
//        }
//    });
//    cgOut.getData().filter(function (element) {
//        if (!Handler.isNullOrEmpty(element.Product) && element.FkProductId > 0) {
//            _d.push(element);
//            return element
//        }
//    });
//    return _d
//}
//function SaveRecord() {
//    Common.Get(".form", "", function (flag, _d) {

//        if (flag) {

//            _d.PkRecipeId = $('#PkRecipeId').val();
//            _d.Recipe_dtl = GetDataFromGrid();

//            if (_d.Recipe_dtl.length > 0) {
//                console.log(_d);
//                $.ajax({
//                    type: "POST",
//                    url: Handler.currentPath() + 'Create',
//                    data: { model: _d },
//                    datatype: "json",
//                    success: function (res) {

//                        if (res.status == "success") {
//                            alert('Save Successfully..');
//                            window.location = Handler.currentPath() + 'List';
//                        }
//                        else
//                            alert(res.msg);
//                    }
//                });
//            }
//            else
//                alert("Insert Valid Product Data..");

//        }
//        else
//            alert("Some Error found.Please Check");
//    });
//}