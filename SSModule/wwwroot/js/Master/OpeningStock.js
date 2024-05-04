var FormId = '25';
var GridId = "WUCHM1", GridHeight = "50vh", pageNo = 1, pageSize = 1000, filterclass = "filter", IdProperty = "sno";

$(document).ready(function () {
    $("#btnSave").on("click", function () {
        if ($("#hdnFKLotID").val() == "0" || $("#hdnFKProdID").val() == "0") {
            $("#hdnFKLotID").val($("#FKLotID").val());
            $("#hdnFKProdID").val($("#FKProductId").val());
        }
       
    });
   
});


function ShowGridColumn() {
    Common.GridColSetup(parseInt(FormId), '', function () {
        ViewProductLot();
    });
}
function ViewProductLot() {
    if ($("#FKProductId").val() > 0) {
        debugger;
        setTimeout(function () {
            $("#hidExtraFKLotID").val($("#FKProductId").val());
            $("#drpListFKLotID").find(".cusdropdown-icon").trigger("click");
        }, 1000);

       

      //  var _d = {};
      //  _d["FkProductId"] = $("#FKProductId").val();
      //  $.ajax({
      //      type: "POST",
      //      url: '/Master/OpeningStock/FKLotID',
      //      data: _d,
      //      datatype: "json",
      //      success: function (res) {
      //
      //
      //
      //          //var v = "";
      //          //$.each(res, function (index,value) {
      //          //    v += '<option value="' + value.PkLotId +'">' + value.Batch +'</option>';
      //          //});
      //          //$("#drpProdLot").html("");
      //          //$("#drpProdLot").append(v);
      //          //// $("#drpProdLot").trigger("change");
      //          //setTimeout(function () {
      //          //    GetDataByLot();
      //          //}, 500);
      //      }
      //  })

    }
    //else {
    //    bindGrid1(GridId, [], IdProperty)
    //}
};


function GetDataByLot() {

    var _d = {};
    _d["FkProdId"] = $("#FKProductId").val();
   // _d["FkLotId"] = $("#drpProdLot").val();
    $.ajax({
        type: "POST",
        url: '/Master/OpeningStock/ProdStockbyLot',
        data: _d,
        datatype: "json",
        success: function (res) {
            if (res == null || res == "") {
                res = [];
            }
            bindGrid1(GridId, res, IdProperty)
        }
    })
}


function GetLocationData() {
    var vLocationId = $("#FKLocationID").val();
    var vLotId = $("#FKLotID").val();
    if (vLocationId == "" || vLocationId == "0" || vLotId == "") {
        alert("Please Select Valid location and lot");
        return false;
    }
    var _d = {};
    _d["Fklocationid"] = vLocationId;
    _d["LotId"] = vLotId;
    $.ajax({
        type: "POST",
        url: '/Master/OpeningStock/GetDataByLocation',
        data: _d,
        datatype: "json",
        success: function (res) {
            $("#hdnPKStockId").val("0");
            $("#hdnFKProdID").val(0);
            $("#hdnFKLotID").val(0);
            if (res == null || res == "") {

                res = [];
            } else {
                res = JSON.parse(res);
                $("#hdnPKStockId").val(res[0].PKStockId);
                $("#hdnFKProdID").val(res[0].FKProdID);
                $("#hdnFKLotID").val(res[0].FKLotID);
                $("#txtInStock").val(res[0].InStock);
                $("#txtOutStock").val(res[0].OutStock);
                $("#txtCurStock").val(res[0].CurStock);
                $("#txtOpStock").val(res[0].OpStock);
             }
            bindGrid1(GridId, res, IdProperty)
        }
    })

}


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
        cg.setIdProperty('PKStockId');
        cg.setCtrlType(s.setCtrlType);
        cg.bind(data);
        cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());

    });
};