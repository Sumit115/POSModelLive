
var tran = null;
$(document).ready(function () {
    var dt = new Date;
    document.getElementById("FromDate").valueAsDate = new Date();
    document.getElementById("ToDate").valueAsDate = new Date();
    Load();

}); 

var FormId = $("#hdFormId").val();
var GridId = "WUCHM", GridHeight = "90vh", pageNo = 1, pageSize = 1000, filterclass = "filter", IdProperty = "";
var ProductFilter = '', CustomerFilter = '', Controller = 'SalesStock';
function ShowGridColumn() {
    Common.GridColSetup(parseInt(FormId), $("#ReportType").val(), function () {
        Load();
    });
}
function Load() {
    $('#' + GridId).empty();
    Common.Get("." + filterclass, "", function (flag, _d) {
        if (flag) {
            
            console.log(_d);
            _d["pageNo"] = pageNo;
            _d["pageSize"] = pageSize;
            _d["ProductFilter"] = $("#hdProductFilter").val();
            _d["CustomerFilter"] = $("#hdCustomerFilter").val();
            $.ajax({
                type: "POST",
                url: 'List',
                data: _d,
                datatype: "json",
                success: function (res) {
                    var jo = JSON.parse(res.data);
                    bindGrid(GridId, jo, IdProperty);
                }
            })

        }
        else
            alert("Some Error found.Please Check");
    });
};
function bindGrid(GridId, data, IdProperty) {
    Common.Grid(parseInt(FormId), $("#ReportType").val(), function (s) {
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
        cg.setIdProperty(IdProperty);
        cg.setCtrlType(s.setCtrlType);
        cg.bind(data);
        cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());
    });
};

function ExportToExcel() {
    Common.Get("." + filterclass, "", function (flag, _d) {
        if (flag) {
            
            ProductFilter = $("#hdProductFilter").val();
            CustomerFilter = $("#hdCustomerFilter").val();

            var downloadUrl = '/Report/' + Controller +'/Export?FromDate=' + _d.FromDate + '&ToDate=' + _d.ToDate + '&ReportType=' + _d.ReportType + '&TranAlias=' + _d.TranAlias + '&ProductFilter=' + ProductFilter + '&CustomerFilter=' + CustomerFilter + '';
            var a = document.createElement("a");
            a.href = downloadUrl;
            a.download = "ReportFile.xls";
            document.body.appendChild(a);
            a.click();
        }
        else
            alert("Some Error found.Please Check");
    });

} ;
