
var FormId = $("#hdFormId").val();
var GridId = "WUCHM", GridHeight = "90vh", pageNo = 1, pageSize = 1000, filterclass = "filter", IdProperty = "";

function ShowGridColumn() {
    Common.GridColSetup(parseInt(FormId), '', function () {
        View();
    });
}
function View() {
    $('#' + GridId).empty();
    Common.Get("." + filterclass, "", function (flag, _d) {
        if (flag) {
            _d["pageNo"] = pageNo;
            _d["pageSize"] = pageSize;
            $.ajax({
                type: "POST",
                url: 'List',
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
        cg.setIdProperty(IdProperty);
        cg.setCtrlType(s.setCtrlType);
        cg.bind(data);
        cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());
        cg.outGrid.onDblClick.subscribe(function (e, args) {
            if (args.cell != undefined) {
                var pk_Id = args.grid.getDataItem(args.row)[IdProperty];                
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

        $("#contextMenu").click(function (e) {
            if (!$(e.target).is("li")) {
                return;
            }
            if (!UDI.outGrid.getEditorLock().commitCurrentEdit()) {
                return;
            }
            var row = $(this).data("row");
            var command = $(e.target).attr("data");
            var pk_Id = UDI.outGrid.getDataItem(row)[IdProperty];
            if (command == "Edit") {
                window.location.href = "Create/" + pk_Id;
            }
        });
    });
};