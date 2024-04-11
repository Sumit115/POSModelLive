var FormId = $("#hdFormId").val();
var GridId = "WUCHM1", GridHeight = "50vh", pageNo = 1, pageSize = 1000, filterclass = "filter", IdProperty = "sno";
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
                //if (res.status == "success") {
                //var jo = JSON.parse(res.data);
                bindGrid1(GridId, res, IdProperty);
                //}
                //else
                //    alert(res.msg);
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
        cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());

    });
};