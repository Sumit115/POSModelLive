
var FormId = $("#hdFormId").val();
var GridId = "WUCHM", GridName = "", GridHeight = "90vh", pageNo = 1, pageSize = 10000, filterclass = "filter", IdProperty = "";

function ShowGridColumn() {
    
    Common.GridColSetup(parseInt(FormId), GridName, function () {
        View();
    });
}
function View() {
    $('#' + GridId).empty();
    Common.Get("." + filterclass, "", function (flag, _d) {
        if (flag) {
           
            _d["pageNo"] = pageNo;
            _d["pageSize"] = pageSize;
            if (typeof RPTFilter !== 'undefined') {
                _d["LocationFilter"] = RPTFilter.Location.Filter; 
            }
           
            $.ajax({
                type: "POST",
                url: Handler.currentPath() + 'List',
                data: _d,
                datatype: "json",
                success: function (res) {
                    console.log(res);
                    if (res.status == "success") {
                        bindGrid(GridId, res.data, IdProperty);
                    }
                    else
                        alert(res.msg);
                }
            })

        }
        else
            alert("Some Error found.Please Check");
    });
};
function bindGrid(GridId, data, IdProperty) {
    
    Common.Grid(parseInt(FormId), GridName, function (s) {
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
        cg.outGrid.onDblClick.subscribe(function (e, args) {
            if (args.cell != undefined) {
                var pk_Id = args.grid.getDataItem(args.row)[IdProperty];
                if (window.location.href.indexOf("Transactions") > 0 && window.location.href.indexOf("Voucher") <= 0) {
                    var FKSeriesId = args.grid.getDataItem(args.row)["FKSeriesId"];
                    window.location.href = "Create/" + pk_Id + "/" + FKSeriesId;
                }
                else if (window.location.href.indexOf("Voucher") > 0) { }
                else if (window.location.href.indexOf("List/") > 0) {
                    var type = location.href.substring(location.href.indexOf("List/") + 5);
                    window.location.href = Handler.currentPath() + "Create/" + type + "/" + pk_Id;

                }
                else
                    window.location.href = "Create/" + pk_Id;
            }
        });

        cg.outGrid.onContextMenu.subscribe(function (e, args) {
            e.preventDefault();
            var j = cg.outGrid.getCellFromEvent(e);
            $("#contextMenu")
                .data("row", j.row)
                .css("top", e.pageY)
                .css("left", e.pageX)
                .show();

            $("body").one("click", function () {
                $("#contextMenu").hide();
            });
        });

        $("#contextMenu").off('click').on('click', function (e) {
            
            if (!$(e.target).is("li")) {
                return;
            }
            if (!UDI.outGrid.getEditorLock().commitCurrentEdit()) {
                return;
            }
            var row = $(this).data("row");
            var command = $(e.target).attr("data");

            var pk_Id = UDI.outGrid.getDataItem(row)[IdProperty];
            var FkSeriesId = UDI.outGrid.getDataItem(row).FKSeriesId;
            if (command == "Edit") {
                
                if (location.href.indexOf("List/") != -1) {
                    var type = location.href.substring(location.href.indexOf("List/") + 5);
                    window.location.href = Handler.currentPath() + "Create/" + type + "/" + pk_Id;
                }
                else { window.location.href = Handler.currentPath() + "Create/" + pk_Id; }
                
            }
            else if (command == "Delete") {
                $.ajax({
                    type: "POST",
                    url: 'DeleteRecord',
                    data: { PKID: pk_Id },
                    datatype: "json",
                    success: function (res) {
                        console.log(res);
                        if (res == "") {
                            View()
                        }
                        else
                            alert(res);
                    }
                })

            }
            else if (command == "InvoiceDownload") {
               
                $(".loader").show();
                $.ajax({
                    type: "POST",
                    url: 'InvoicePrint_Pdf_Url',
                    data: { PkId: pk_Id, FkSeriesId: FkSeriesId },
                    datatype: "json",
                    success: function (res) {
                      
                        if (res.status == "success") {
                            window.open(res.data.InvoiceUrl, '_blank');
                        }
                        else
                            alert(res);
                        $(".loader").hide();
                        return;
                    }
                })

            }
        });
    });
};

function ExportToExcel() {
    Common.Get("." + filterclass, "", function (flag, _d) {
        if (flag) {

            //  var url = $(location).attr('href').replace('List', 'Export');
            var downloadUrl = 'Export';
            var a = document.createElement("a");
            a.href = downloadUrl;
            a.download = "ReportFile.xls";
            document.body.appendChild(a);
            a.click();
        }
        else
            alert("Some Error found.Please Check");
    });

};

