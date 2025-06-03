
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
                _d["StateFilter"] = RPTFilter.State.Filter;
            }

            $.ajax({
                type: "POST",
                url: Handler.currentPath() + 'List',
                data: _d,
                datatype: "json",
                success: function (res) {
                    // console.log(res);
                    if (res.status == "success") {
                        bindGrid(GridId, res.data, IdProperty);
                    }
                    else
                        alert(res.msg);
                }
            });

        }
        else
            alert("Some Error found.Please Check");
    });
};
function bindGrid(GridId, data, IdProperty) {

    Common.Grid(parseInt(FormId), GridName, function (s) {
       // console.log(s);
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
        if (s.TotalOn != '' && s.TotalOn != undefined) {
            if (s.TotalOn.replace('~') != '') {
                cg.setTotalOn(s.TotalOn)
            }
        }
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

                } else if (window.location.href.indexOf("EntryLog") > 0) { }
                else
                    window.location.href = "Create/" + pk_Id;
            }
        });

        cg.outGrid.onContextMenu.subscribe(function (e, args) {
            e.preventDefault();
            var j = cg.outGrid.getCellFromEvent(e);

            

            var str = UDI.outGrid.getDataItem(j.row).TrnStatus;
            if (!Handler.isNullOrEmpty(str)) {
                str = str.replace('\u0000', '')
                str = str.replace(/ /g, '');
                str = str.replace(/\s+/g, '');
                str = str.replace(' ', '');

                var TrnStatus = Handler.isNullOrEmpty(str) ? "P" : str;
                if (TrnStatus.indexOf("P") != -1) {
                    $("#contextMenu #contextConvertInvoice").show();
                }
                else { $("#contextMenu #contextConvertInvoice").hide(); }
            }
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
                       // console.log(res);
                        if (res == "") {
                            View()
                        }
                        else
                            alert(res);
                    }
                })

            }
            else if (command == "InvoicePrint" || command == "DueInvoicePrint") {

                $(".loader").show();
                var PrintFormatName = command == "DueInvoicePrint" ? "sizewiseDueqty" : "";
                $.ajax({
                    type: "POST",
                    url: 'InvoicePrint_Pdf_Url',
                    data: { PkId: pk_Id, FkSeriesId: FkSeriesId, PrintFormatName: PrintFormatName },
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
                
                var str = UDI.outGrid.getDataItem(row).TrnStatus;
                var str = str.replace(/ /g, '');
                var str = str.replace(/\s+/g, '');
                var TrnStatus = Handler.isNullOrEmpty(str) ? "P" : str;
               // if (TrnStatus == 'P') {
                if (TrnStatus.indexOf('P') != -1) {
                    window.location.href = Handler.rootPath() + "Transactions/SalesInvoice/ConvertInvoice/" + pk_Id + "/" + FkSeriesId;
                }
                else { alert('Invalid Request'); }
            }
            else if (command == "ConvertLocationInvoice") {
               // console.log(Handler.rootPath());
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
                       // console.log(res);
                        if (res.status == "success") {

                            var htm = '';
                            //htm += '<div class="mb-4 card">';
                            //htm += '   <div class="card-body"> ';
                            //htm += '       <div class="row mb-3"> ';
                            //htm += '           <div class="col-md-6"> ';
                            //htm += '               <div class="card-title">Invoice Bilty</div> ';
                            //htm += '           </div>';
                            //htm += '           <div class="col-md-6 text-center"> ';
                            //// htm += '               <input type="button" id="btnSelectBarcode" value="Done" class="btn btn-success" />';
                            //htm += '           </div> ';
                            //htm += '       </div> ';
                            //htm += '       <div class="row mb-3"> ';
                            
                            htm += res.htmlString;

                            //htm += '       </div> ';
                            htm += '       <div class="row">';
                            htm += '           <div class="col-md-12 text-center"> ';
                            htm += '               <input type="hidden" id="hdFkID" value="0" class="form-control" />';
                            htm += '               <input type="hidden" id="hdFkSeriesId" value="0" class="form-control" />';
                        //    htm += '               <input type="button" id="btnSubmitBilty" value="Submit" class="btn btn-success" />';
                            htm += '           </div> ';

                            htm += '          </div>  ';
                            //htm += '   </div>';
                            //htm += '   </div>';

                            Handler.popUp(htm, { width: "400px", height: "250px" }, function () {
                                $("#hdFkID").val(pk_Id);
                                $("#hdFkSeriesId").val(FkSeriesId); 
                                $('.model-ShippingDetails').show();
                                 GlobleDropDownBind('.model-ShippingDetails');
                                $("#drpListFKBankThroughBankID").removeAttr('event');

                                $("#btnSaveShippDtl").off("click").on("click", function () {
                                   //$(this).closest(".popup_d").hide();

                                    //alert();
                                    var model = {};
                                    model.PkId = $("#hdFkID").val();
                                    model.FKSeriesId = $("#hdFkSeriesId").val();
                                    model.BiltyNo = $("#BiltyNo ").val();
                                    model.BiltyDate = $("#BiltyDate").val();
                                    model.TransportName = $("#TransportName").val();
                                    model.NoOfCases = $("#NoOfCases").val();
                                    model.FreightType = $("#FreightType").val();
                                    model.FreightAmt = $("#FreightAmt").val();
                                    model.ShipingAddress = $("#ShipingAddress").val();
                                    model.PaymentMode = $("#PaymentMode").val();
                                    model.FKBankThroughBankID = $("#FKBankThroughBankID").val();
                                    model.DeliveryDate = $("#DeliveryDate").val();
                                    model.ShippingMode = $("#ShippingMode").val();
                                    model.PaymentDays = $("#PaymentDays").val();
                                    model.EWayDetail = {};
                                    model.EWayDetail.EWayNo = $("#EWayDetail_EWayNo").val();
                                    model.EWayDetail.EWayDate = $("#EWayDetail_EWayDate").val();
                                    model.EWayDetail.VehicleNo = $("#EWayDetail_VehicleNo").val();
                                    model.EWayDetail.TransDocNo = $("#EWayDetail_TransDocNo").val();
                                    model.EWayDetail.TransDocDate = $("#EWayDetail_TransDocDate").val();
                                    model.EWayDetail.TransMode = $("#EWayDetail_TransMode").val();
                                    model.EWayDetail.SupplyType = $("#EWayDetail_SupplyType").val();
                                    model.EWayDetail.Distance = $("#EWayDetail_Distance").val();
                                    model.EWayDetail.VehicleType = $("#EWayDetail_VehicleType").val(); 
                                    console.log(model);
                                    $.ajax({
                                        type: "POST",
                                        url: Handler.currentPath() + 'SaveInvoiceBilty',
                                        data: { model: model }, 
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
            else if (command == "ViewLog") {
                
                //console.log(UDI.outGrid.getDataItem(row));
                var WebUrl = UDI.outGrid.getDataItem(row).WebUrl;
                // var FKID = UDI.outGrid.getDataItem(row).FKID;
                // FKSeriseId = UDI.outGrid.getDataItem(row).FKSeriseId;
                var baseurl = window.location.origin + WebUrl.substring(0, WebUrl.indexOf("List"));
                var _url = '';
                if (WebUrl.indexOf("List/") != -1) {
                    var type = WebUrl.substring(WebUrl.indexOf("List/") + 5);
                    _url = baseurl + "Create/" + type + "/" + pk_Id + "?pageview=log";
                }
                else { _url = baseurl + "Create/" + pk_Id + "?pageview=log"; }

                window.open(
                    _url,
                    '_blank' // <- This is what makes it open in a new window.
                );

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
