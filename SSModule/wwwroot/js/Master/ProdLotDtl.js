var FormId = $("#hdFormId").val();
var GridId = "WUCHM1", GridHeight = "50vh", pageNo = 1, pageSize = 1000, filterclass = "filter", IdProperty = "sno";
var ProdlotData = null;

$(document).ready(function () {
    $('#PkLotId').val('0');
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
    bindGrid1(GridId, [], IdProperty)


    $("form").submit(function (event) {
        event.preventDefault();
        Common.Get("form", "", function (flag, _d) {
            if ($("#FKProductId").val() > 0) {

                _d["FkProductId"] = $("#FKProductId").val();
                $.ajax({
                    type: "POST",
                    url: '/Master/ProductLot/Create',
                    data: _d,
                    datatype: "json",
                    success: function (res) {
                        console.log(res);
                        //if (res.status == "success") {
                        //var jo = JSON.parse(res.data);
                        ViewProductLot();
                        //}
                        //else
                        //    alert(res.msg);
                    }
                })

            }
            else {
                alert("Please Select Product");
            }
        });
    });
});
function ShowGridColumn() {
    Common.GridColSetup(parseInt(FormId), '', function () {
        ViewProductLot();
    });
}
function ViewProductLot() {
    $('#' + GridId).empty();
    if ($("#FKProductId").val() > 0) {
        var _d = {};
        _d["FkProductId"] = $("#FKProductId").val();
        $.ajax({
            type: "POST",
            url: '/Master/ProductLot/LotbyProductId',
            data: _d,
            datatype: "json",
            success: function (res) {
                console.log(res);
                bindGrid1(GridId, res, IdProperty);
            }
        })

    }
    else {
        bindGrid1(GridId, [], IdProperty)
    }
};


function bindGrid1(GridId, data, IdProperty) {
    
    Common.Grid(parseInt(FormId), '', function (s) {

        var cg = new coGrid("#" + GridId);
        UDI = cg;
        cg.setColumnHeading(s.ColumnHeading);
        cg.setColumnWidthPer(s.ColumnWidthPer, 1200);
        cg.setColumnFields(s.ColumnFields);
        cg.setAlign(s.Align);
        cg.defaultHeight = GridHeight;
        cg.setSearchType(s.SearchType);
        cg.setSearchableColumns(s.SearchableColumns);
        cg.setSortableColumns(s.SortableColumns);
        cg.setIdProperty('PkLotId');
        cg.setCtrlType(s.setCtrlType);
        cg.bind(data);
        // cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());
        cg.outGrid.onCellChange.subscribe(function (e, args) {
            if (args.cell != undefined) {

                var field = cg.columns[args.cell].field;
                var PkLotId = parseFloat(args.item["PkLotId"]);
                var FKProductId = parseFloat(args.item["FKProductId"]);
                var MRP = parseFloat(args.item["MRP"]) > 0 ? parseFloat(args.item["MRP"]) : 0;
                var SaleRate = parseFloat(args.item["SaleRate"]) > 0 ? parseFloat(args.item["SaleRate"]) : 0;
                var PurchaseRate = parseFloat(args.item["PurchaseRate"]) > 0 ? parseFloat(args.item["PurchaseRate"]) : 0;
                var TradeRate = parseFloat(args.item["TradeRate"]) > 0 ? parseFloat(args.item["TradeRate"]) : 0;
                var DistributionRate = parseFloat(args.item["DistributionRate"]) > 0 ? parseFloat(args.item["DistributionRate"]) : 0;

                if (field == "MRP") {
                    
                    if (MRP > SaleRate && MRP > PurchaseRate && MRP > TradeRate && MRP > DistributionRate)
                        UpdateProdLotDtl(PkLotId, FKProductId, field, MRP);
                    else {
                        args.grid.gotoCell(args.row, args.cell - 1, true);
                        alert('Invalid MRP');
                        
                    }
                } else if (field == "PurchaseRate") {
                    if (MRP > PurchaseRate)
                        UpdateProdLotDtl(PkLotId, FKProductId, field, PurchaseRate);
                    else {
                        alert('Invalid Purchase Rate');
                        args.grid.gotoCell(args.row, args.cell - 1, true)
                    }
                }
                else if (field == "SaleRate") {
                    if (MRP > SaleRate)
                        UpdateProdLotDtl(PkLotId, FKProductId, field, SaleRate);
                    else {
                        alert('Invalid Sale Rate');
                        args.grid.gotoCell(args.row, args.cell - 1, true)
                    }
                }
                else if (field == "TradeRate") {
                    if (MRP > TradeRate)
                        UpdateProdLotDtl(PkLotId, FKProductId, field, TradeRate);
                    else {
                        alert('Invalid Trade Rate');
                        args.grid.gotoCell(args.row, args.cell - 1, true)
                    }
                }
                else if (field == "DistributionRate") {
                    if (MRP > DistributionRate)
                        UpdateProdLotDtl(PkLotId, FKProductId, field, DistributionRate);
                    else {
                        alert('Invalid Distribution Rate');
                        args.grid.gotoCell(args.row, args.cell - 1, true)
                    }
                }

            }
        });
    });
};

function UpdateProdLotDtl(PkLotId, FKProductId, ColumnName, Value) {
    $(".loader").show();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'UpdateProdLotDtl',
        data: { PkLotId, FKProductId, ColumnName, Value },
        datatype: "json",
        success: function (res) {

            if (res.status == "success") {
                //tranModel = res.data;
                //setFooterData(tranModel);
                //setPaymentDetail(tranModel);

                //setGridRowData(args, tranModel.TranDetails, rowIndex, fieldName);

            }
            else
                alert(res.msg);

            $(".loader").hide();
        }
    });

}
