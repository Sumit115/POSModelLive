
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
                    url: Handler.currentPath() + 'DeleteRecord',
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
            else if (command == "ConvertInvoice") {
                console.log(Handler.rootPath());
                //window.location.href = Handler.currentPath() + "ConvertInvoice/" + type + "/" + pk_Id;
                window.location.href = Handler.rootPath() + "Transactions/SalesInvoice/ConvertInvoice/" + pk_Id + "/" + FkSeriesId;
            }
            else if (command == "ConvertLocationInvoice") {
                console.log(Handler.rootPath());
                //window.location.href = Handler.currentPath() + "ConvertInvoice/" + type + "/" + pk_Id;
                window.location.href = Handler.rootPath() + "Transactions/LocationTransferInvoice/ConvertInvoice/" + pk_Id + "/" + FkSeriesId;
            }
            else if (command == "InvoiceBilty") {
                var FormId = $("#hdFormId").val();

                $.ajax({
                    type: "POST",
                    url: Handler.currentPath() + 'GetInvoiceBilty',
                    data: { FkID: pk_Id, FKSeriesId: FkSeriesId, FormId: FormId },
                    datatype: "json",
                    success: function (res) {
                        console.log(res);
                        if (res.status == "success") {

                            var htm = '';
                            htm += '<div class="mb-4 card">';
                            htm += '   <div class="card-body"> ';
                            htm += '       <div class="row mb-3"> ';
                            htm += '           <div class="col-md-6"> ';
                            htm += '               <div class="card-title">Invoice Bilty</div> ';
                            htm += '           </div>';
                            htm += '           <div class="col-md-6 text-center"> ';
                            // htm += '               <input type="button" id="btnSelectBarcode" value="Done" class="btn btn-success" />';
                            htm += '           </div> ';
                            htm += '       </div> ';
                            htm += '       <div class="row mb-3"> ';
                            htm += '           <div class="col-md-12"> ';
                            htm += '           <div class="form-group"> ';
                            htm += '           <label class="control-label">Bilty No</label> ';
                            htm += '               <input type="text" id="txtBiltyNo" value="" class="form-control" />';
                            htm += '           </div>';
                            htm += '           </div>';

                            htm += '        <div class="col-md-4">';
                            htm += '         <div class="form-group">';
                            htm += '       <label class="control-label">Bilty Image</label>';
                            htm += '       <input type="file" id="file_BiltyImage" onchange="return UploadImage(' + "'BiltyImage'" + ')" accept="image/*" class="requied d-block" autocomplete="off">';

                            htm += '       <input type="hidden" id="BiltyImage" name="BiltyImage" value="" autocomplete="off">';
                            htm += '       <img id="dummyimage_BiltyImage" class="img-md img-avatar" style="height: 50px;width: 50px; display:none">';

                            htm += '       <span class="text-danger field-validation-valid" data-valmsg-for="BiltyImage" data-valmsg-replace="true"></span>';
                            htm += '     </div>';
                            htm += '    </div>';

                            htm += '       </div> ';
                            htm += '       <div class="row">';
                            htm += '           <div class="col-md-12 text-center"> ';
                            htm += '               <input type="hidden" id="hdFkID" value="0" class="form-control" />';
                            htm += '               <input type="hidden" id="hdFkSeriesId" value="0" class="form-control" />';
                            htm += '               <input type="button" id="btnSubmitBilty" value="Submit" class="btn btn-success" />';
                            htm += '           </div> ';

                            htm += '          </div>  ';
                            htm += '   </div>';
                            htm += '   </div>';

                            Handler.popUp(htm, { width: "400px", height: "250px" }, function () {
                                $("#hdFkID").val(pk_Id);
                                $("#hdFkSeriesId").val(FkSeriesId);
                                $("#txtBiltyNo").val(res.data.BiltyNo);
                                $("#BiltyImage").val(res.data.Image);
                                if (res.data.Image != '') {
                                    $("#dummyimage_BiltyImage").attr("src", res.data.Image);
                                    $("#dummyimage_BiltyImage").show();
                                }
                                $("#btnSubmitBilty").off("click").on("click", function () {

                                    $.ajax({
                                        type: "POST",
                                        url: Handler.currentPath() + 'SaveInvoiceBilty',
                                        data: {
                                            FkID: $("#hdFkID").val(),
                                            FKSeriesId: $("#hdFkSeriesId").val(),
                                            FormId: $("#hdFormId").val(),
                                            BiltyNo: $("#txtBiltyNo").val(),
                                            Image: $("#BiltyImage").val(),
                                        },
                                        datatype: "json",
                                        success: function (res) {
                                            if (res.status == "success") {
                                                alert('Submit Successfully');
                                                $(".popup_d").hide();
                                            }
                                            else { alert(res.msg); }
                                        }
                                    });

                                });


                            });
                            $(".loader").hide();

                        }
                        else
                            alert(res.msg);
                        $(".loader").hide();
                    }
                })

            }

        });
    });
};

function ExportToExcel() {

    Common.Get("." + filterclass, "", function (flag, _d) {

        if (flag) {
            if (typeof RPTFilter !== 'undefined') {
                _d["LocationFilter"] = RPTFilter.Location.Filter;
            }
            _d["pageNo"] = pageNo;
            _d["pageSize"] = pageSize;
            var param = "";
            var Export = 'excel';
            $.each(_d, function (i, val) {

                if (!Common.isNullOrEmpty(val)) {
                    param += i + "=" + val + "&";
                }
            });
            //  var url = $(location).attr('href').replace('List', 'Export');
            //  var downloadUrl = 'Export';
            var downloadUrl = Handler.currentPath() + 'Export?Type=' + Export + '&' + param + '';

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

function UploadImage(id) {

    var file = document.querySelector('input[id="file_' + id + '"]').files[0];
    // $("#FileName").val(file.name);
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {

        //console.log(reader.result);
        $("#" + id).val(reader.result);
        $("#dummyimage_" + id).attr("src", reader.result);
        $("#dummyimage_" + id).show();
        //   $("#myImage").hide();

    };
}
