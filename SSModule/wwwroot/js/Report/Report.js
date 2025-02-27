 
var RPTOption = {
    FormId: parseInt($("#hdFormId").val()),
    GridId: "WUCHM",
    GridHeight: "90vh",
    pageNo: 1,
    pageSize: 1000,
    IdProperty: "",
    ProductFilter: '',
    CustomerFilter: '',
    Controller: 'SalesStock', 
};
var RPTFilter = {
    Vendor: { Data: [], Filter: null, IdProperty: "PkId", Field: "Name" },
    Customer: { Data: [], Filter: null, IdProperty: "PkId", Field: "Name" },
    Product: { Data: [], Filter: null, IdProperty: "PkProductId", Field: "NameToDisplay" },
    Location: { Data: [], Filter: null, IdProperty: "PKLocationID", Field: "Location" },
    Series: { Data: [], Filter: null, IdProperty: "PkSeriesId", Field: "Series" },
    State: { Data: [], Filter: null, IdProperty: "Value", Field: "Text" },
    TrnStatus: { Data: [{ Value: "P", Text: "Pending", tick: true }, { Value: "I", Text: "Invoice" }, { Value: "C", Text: "Close" }], Filter: '[{ "Text": "P" }]', IdProperty: "Value", Field: "Text" },

};


function ShowGridColumn() {
    Common.GridColSetup(parseInt(RPTOption.FormId), $("#ReportType").val(), function () {
        Render();
    });
}

function ViewData(_d, Export) {
    
    $(".loader").show();
    if (Export == "excel") {
        var param = "";
        $.each(_d, function (i, val) {
            
            if (!Common.isNullOrEmpty(val)) {
                param += i + "=" + val + "&";
            }
        });
        
        var downloadUrl = '/Report/' + Controller + '/Export?Type=' + Export +'&' + param + '';
        var a = document.createElement("a");
        a.href = downloadUrl;
        a.download = "ReportFile.xls";
        document.body.appendChild(a);
        a.click();
        $(".loader").hide();
    }
    else {
        $('#' + RPTOption.GridId).empty();
        $.ajax({
            type: "POST",
            url: 'List',
            data: _d,
            datatype: "json",
            success: function (res) {
                 
                var data = JSON.parse(res.data);
                
                Common.Grid(parseInt(RPTOption.FormId), $("#ReportType").val(), function (s) {
                    
                    var cg = new coGrid("#" + RPTOption.GridId);
                    UDI = cg;
                    cg.setColumnHeading(s.ColumnHeading);
                    cg.setColumnWidthPer(s.ColumnWidthPer, 1200);
                    cg.setColumnFields(s.ColumnFields);
                    cg.setAlign(s.Align);
                    cg.defaultHeight = RPTOption.GridHeight;
                    cg.setSearchType(s.SearchType);
                    cg.setSearchableColumns(s.SearchableColumns);
                    cg.setSortableColumns(s.SortableColumns);
                    cg.setIdProperty(RPTOption.IdProperty);
                    cg.setCtrlType(s.setCtrlType);
                      if (s.TotalOn != '' && s.TotalOn != undefined) {
                        if (s.TotalOn.replace('~') != '') {
                            cg.setTotalOn(s.TotalOn)
                        }
                    } 
                    cg.bind(data);
                    cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());

                   
                });
                $(".loader").hide();
            }
        });
    }
};


var filterGrid = null;
function ShowFilter(type) {

    if (RPTFilter[type].Data.length > 0) {

        showpopupWithData();
    }
    else {
        var _d = {};
        _d["pageNo"] = RPTOption.pageNo;
        _d["pageSize"] = RPTOption.pageSize;
        $.ajax({
            type: "POST",
            url: '/Master/' + type + '/List',
            data: _d,
            datatype: "json",
            success: function (res) {

                if (res.status == "success") {
                    RPTFilter[type].Data = res.data;
                    showpopupWithData();
                }
                else
                    alert(res.msg);

            }
        });

    }
    function showpopupWithData() {
        Handler.popUp(fn_GetPopuphtml(type, RPTFilter[type].Data), { width: "544px", height: "600px" }, function () {
            var cg = new coGrid("#WUCFilter");
            cg.setColumnHeading("Select~Name");
            cg.setColumnWidthPer("10~60", 700);
            cg.setColumnFields("tick~" + RPTFilter[type].Field);
            cg.setAlign("C~L");
            cg.defaultHeight = "500px";
            cg.setSearchType("0~1");
            cg.setSearchableColumns(RPTFilter[type].Field);
            cg.setSortableColumns(RPTFilter[type].Field);
            cg.setIdProperty(RPTFilter[type].IdProperty);
            cg.setCtrlType("B~");
            cg.bind(RPTFilter[type].Data);
            cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());
            filterGrid = cg;
            $("#btnSaveFilter").off("click").on("click", function () {
                var _List = [];
                var Filterlist = filterGrid.getData().filter(function (el) { return el.tick })
                $(Filterlist).each(function (i, v) {
                    if (RPTFilter[type].IdProperty == "PkProductId")
                        _List.push({ PKID: v.PkProductId });
                    if (RPTFilter[type].IdProperty == "PKLocationID")
                        _List.push({ PKID: v.PKLocationID });
                    if (RPTFilter[type].IdProperty == "PkSeriesId")
                        _List.push({ PKID: v.PkSeriesId });
                    if (RPTFilter[type].IdProperty == "PkId")
                        _List.push({ PKID: v.PkId });
                    if (RPTFilter[type].IdProperty == "Text")
                        _List.push({ Text: v.Text });
                    if (RPTFilter[type].IdProperty == "Value")
                        _List.push({ Text: v.Value });
                });
                RPTFilter[type].Filter = JSON.stringify(_List);
                $(".popup_d").hide();
            });
        });
    }
    function fn_GetPopuphtml(type, _data) {
        var htm = '';
        htm += '<div class="card card-outline card-primary">';
        htm += '<div class="card-header">';
        htm += '<h3 class="card-title">' + type + ' Filter</h3>';
        htm += '<div class="card-tools">';
        htm += '<button type="button" id="btnSaveFilter" class="btn btn-outline-secondary" >';
        htm += '<i class="bi bi-send"></i> Done</button></div></div>';
        htm += '<div class="card-body">';
       
        htm += '<div class="row">';
        htm += '<div id="WUCFilter" class="col-12"></div>';
        htm += '</div> ';
        htm += '</div></div>';

        return htm;
    }

}
