var _PrdList = [];
$(document).ready(function () {
    IdProperty = "PKID";
    $("#btnImportExcel").show();
    View();
});
function ImportExcel() {

    $('#ImportDatafile').val('');
    $("#DDTImport").html('');
    $("#DDTImport").hide();
    $('.model-importdata').show();
}
function ImportDataFromFile() {
    if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
        alert('The File APIs are not fully supported in this browser.');
        return;
    }

    var input = document.getElementById('ImportDatafile');
    if (!input) {
        alert("Um, couldn't find the fileinput element.");
    }
    else if (!input.files) {
        alert("This browser doesn't seem to support the `files` property of file inputs.");
    }
    else if (!input.files[0]) {
        alert("Please select a file");
    }
    else {
        var formData = new FormData();
        var file = $('#ImportDatafile')[0].files[0];
        formData.append("file", file);
        $(".loader").show();
        $.ajax({
            url: Handler.currentPath() + 'Importfile',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (res) {
                //console.log(res)
                if (res.status == 'success') {
                    clear_ImportFile();
                    View();
                    alert(res.msg);
                }
                else {
                    if (!Handler.isNullOrEmpty(res.msg)) {
                        alert(res.msg);
                    }
                    if (res.IsLoadGrid) {
                        _PrdList = res.data;
                        var data = _PrdList.filter(item =>
                            !item.FKProdCatgId || !item.FkBrandId || !item.FkUnitId
                        );

                        var ColumnHeading = "Artical~SubSection~Brand~Unit";
                        var ColumnWidthPer = "15~10~10~10";
                        var ColumnFields = "Product~CategoryName~BrandName~UnitName";
                        var Align = "L~L~L~L";
                        var setCtrlType = "~CD~CD~CD";

                        cgImport = new coGrid("#DDTImport");
                        cgImport.setColumnHeading(ColumnHeading);
                        cgImport.setColumnWidthPer(ColumnWidthPer, 1200);
                        cgImport.setColumnFields(ColumnFields);
                        cgImport.setAlign(Align);
                        cgImport.defaultHeight = "300px";
                        cgImport._MinRows = 0;
                        cgImport.setIdProperty("SrNo");
                        cgImport.setCtrlType(setCtrlType);

                        var f = ColumnFields.split('~');
                        var s = setCtrlType.split('~');
                        var arrmapData = []
                        var DrpIndex = {};

                        for (kk = 0; kk < s.length; kk++) {
                            var sl = s[kk];
                            var fl = f[kk];
                            if (sl == "C" || sl == "L" || sl == "CD") {
                                DrpIndex[fl] = kk;
                                switch (fl) {
                                    case "CategoryName":
                                        cgImport.columns[kk]["event"] = dropListMaster;
                                        cgImport.columns[kk]["fieldval"] = "FKProdCatgId";
                                        cgImport.columns[kk]["KeyID"] = "PkCategoryId";
                                        cgImport.columns[kk]["KeyValue"] = "CategoryName";
                                        break
                                    case "BrandName":
                                        cgImport.columns[kk]["event"] = dropListMaster;
                                        cgImport.columns[kk]["fieldval"] = "FkBrandId";
                                        cgImport.columns[kk]["KeyID"] = "PkBrandId";
                                        cgImport.columns[kk]["KeyValue"] = "BrandName";
                                        break
                                    case "UnitName":
                                        cgImport.columns[kk]["event"] = dropListMaster;
                                        cgImport.columns[kk]["fieldval"] = "FkUnitId";
                                        cgImport.columns[kk]["KeyID"] = "PkUnitId";
                                        cgImport.columns[kk]["KeyValue"] = "UnitName";
                                        break
                                }
                            }
                        }
                        var obdt = cgImport.populateDataFromJson({
                            srcData: data,
                            mapData: arrmapData
                        });
                        cgImport.applyAddNewRow();
                        cgImport.bind(data);

                        cgImport.outGrid.onCellChange.subscribe(function (e, args) {
                            if (args.cell != undefined) {

                                var field = cgImport.columns[args.cell].field;
                                //if (field == "Product") {
                                //    var FkProductId = Common.isNullOrEmpty(args.item["Product"]) ? 0 : parseFloat(args.item["Product"]);
                                //    args.item["FkProductId"] = FkProductId; 
                                //}
                                //if (field == "SubCategoryName") {
                                //    var FKProdCatgId = Common.isNullOrEmpty(args.item["SubCategoryName"]) ? 0 : parseFloat(args.item["SubCategoryName"]);
                                //    args.item["FKProdCatgId"] = FKProdCatgId;

                                //}
                                //cg.updateRefreshDataRow(args.row);
                                //cg.updateAndRefreshTotal(); 
                                // args.grid.gotoCell(args.row, args.cell + 1, true)
                                //$(".loader").hide();
                            }
                        });

                        //---------------    ---------------   ---------------   ---------------/
                        cgImport.outGrid.onClick.subscribe(function (e, args) {

                            if (args.cell != undefined) {
                                var field = cgImport.columns[args.cell].field;


                            }
                        });

                        $("#DDTImport").show();
                        $("#btnSubmit_SaveBulk").show();
                        //alert("Upload successful!");
                    }
                    else
                        clear_ImportFile();

                }
                $(".loader").hide();
            },
            error: function (xhr, status, error) {
                alert("Upload failed: " + xhr.responseText);
            }
        });

    }


}

function SaveBulk() {
    debugger;
    var data = cgImport.getData().filter(function (element) { return !element.FKProdCatgId && !Handler.isNullOrEmpty(element.Product); });
    if (data.length == 0) {
        data = cgImport.getData().filter(function (element) { !element.FkBrandId && !Handler.isNullOrEmpty(element.Product); });
        if (data.length == 0) {
            data = cgImport.getData().filter(function (element) { !element.FkUnitId && !Handler.isNullOrEmpty(element.Product); });
            if (data.length == 0) {

                cgImport.getData().filter(function (element) {
                    if (!Handler.isNullOrEmpty(element.Product)) {
                        let index = _PrdList.findIndex(x => x.SrNo === element.SrNo);
                        if (index !== -1) {
                            // 🔹 Update existing object in _prdList
                            _PrdList[index].FKProdCatgId = element.FKProdCatgId;
                            _PrdList[index].FkBrandId = element.FkBrandId;
                            _PrdList[index].FkUnitId = element.FkUnitId;
                        }
                    }
                });
                var dataCheck = _PrdList.filter(item => !item.FKProdCatgId || !item.FkBrandId || !item.FkUnitId);
                console.log(dataCheck);
                if (dataCheck.length == 0) {
                    // if (Handler.isNullOrEmpty(tranModel.TranDetails[rowIndex].LinkSrNo) || tranModel.TranDetails[rowIndex].LinkSrNo <= 0 || fieldName == 'Delete') {
                    $(".loader").show();
                    $.ajax({
                        type: "POST",
                        url: Handler.currentPath() + 'SaveBulk',
                        data: { modelList: _PrdList },
                        datatype: "json",
                        success: function (res) {
                            if (res.status == "success") {
                                clear_ImportFile();
                                View();
                                alert(res.msg);
                            }
                            else
                                alert(res.msg);

                            $(".loader").hide();
                        }
                    });

                    // } else { alert('Promotion Artical Not Update'); BindGrid('DDT', tranModel.TranDetails); }
                } else
                    alert('Please Import Data');
            } else
                alert('Please Select Unit');
        } else
            alert('Please Select Brand');
    } else
        alert('Please Select SubSection');
    // BindGrid('DDT', tranModel.TranDetails)
}
function clear_ImportFile() {
    var modelList = [];
    $('#ImportDatafile').val('');
    $('.model-importdata').hide();
    $("#DDTImport").html('');
    $("#DDTImport").hide();
    $(".loader").hide();
}

function dropListMaster(data) {
    // console.log(data);
    var output = []
    var uri = "";
    if (data.name == 'CategoryName')
        uri = "Category";
    if (data.name == 'BrandName')
        uri = "Brand";
    if (data.name == 'UnitName')
        uri = "Unit";
    $.ajax({
        url: '/CustomDropDown/' + uri, type: 'POST', data: data, async: false, dataType: 'JSON', success: function (result) {
            // console.log(result);
            output = result;

        }, error: function (request, status, error) {
        }
    });



    console.log(output);
    return output;

}

