var FormId = $("#hdFormId").val();
var TranType = $("#hdTranType").val();

Load();

SetAuto("txtParty"
    , JSON.parse($("#hdPartiList").val())
    , "FkPartyId"
    , "PartyName"
    , "PartyMobile"
    , "PartyAddress"
    , ['Party Name', 'Mobile']
);
function Load() {
    var PkId = $("#PkId").val();

    if (PkId > 0) {
        var data = JSON.parse($("#hdData").val());
       // console.log(data.TranDetails);
        $('#FkPartyId').val(data.FkPartyId);
        $('#EntryDate').val(data.EntryDate);
        $('#GRDate').val(data.GRDate);
        $("#txtParty,input[placeholder='Search Party']").val(data.Party.Name);
         $('#PartyName').val(data.Party.Name);
        $('#PartyMobile').val(data.Party.Mobile);

        $(data.TranDetails).each(function (i, v) {
            //v.Description_Text = v.Description;
            // v["Product_Text"] = v.Description;
            v["ProductName"] = parseInt(v.FkProductId);
            v["mode"] = 1;
            v["Delete"] = 'Delete';
        });
        BindGrid('DDT', data.TranDetails);
    }

    else {
        BindGrid('DDT', []);

    }
}
function SetAuto(CtrlId, _data, $c1, $c2, $c3, $c4, _columns) {
    var text2 = $("#" + CtrlId).tautocomplete({
        width: "500px",
        theme: "white",
        placeholder: $("#" + CtrlId).attr("placeholder"),
        columns: _columns,
        data: function () {
            var filterData = [];
            var searchData = eval("/" + text2.searchdata() + "/gi");
            $.each(_data, function (i, v) {
                //console.log(v);
                if (v.Field1.search(new RegExp(searchData)) != -1 || v.Field2.search(new RegExp(searchData)) != -1) {
                    filterData.push(v);
                }
            });

            //console.log(filterData);
            if (filterData.length > 0)
                return filterData;
            else
                return _data;
        },
        onchange: function (data) {


            if (text2.id() > 0) {

                $("#" + $c1).val(text2.id());
                //console.log(text2.id());

                var lst = _data.filter(function (element) { return element.pk_Id == text2.id(); })[0];
                $("#" + $c2).val(lst.Field1);
                $("#" + $c3).val(lst.Field2);
                //$("#" + $c4).val(lst.Field3);

            } else {

                $("#" + $c1).val('0');
                $("#" + $c3).val('');
                //$("#" + $c4).val('');
            }
        }
    });
}
function BindGrid(GridId, data) {

    $("#" + GridId).empty();
    Common.Grid(parseInt(FormId), TranType, function (s) {

       // console.log(s.setCtrlType);
    
     var ProductList = JSON.parse($("#hdProductList").val());
    cg = new coGrid("#" + GridId);
    UDI = cg;
    cg.setColumnHeading(s.ColumnHeading);
    cg.setColumnWidthPer(s.ColumnWidthPer, 1200);
    cg.setColumnFields(s.ColumnFields);
    cg.setAlign(s.Align);
    cg.defaultHeight = "400px";
    cg._MinRows = 50;
    //cg.setSearchType(s.SearchType);
    //cg.setSearchableColumns(s.SearchableColumns);
    //cg.setSortableColumns(s.SortableColumns);
    cg.setIdProperty("sno");
    cg.setCtrlType(s.setCtrlType);
        cg.setOptionArray(0, ProductList, "ProductName", false, "Product", "PkProductId", "1");
        cg.setOptionArray(9, ProductList, "Batch", false, "Product", "Product", "1");
    var obdt = cg.populateDataFromJson({
        srcData: data,
        mapData: [{ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" },
            { data: ProductList, textColumn: "Batch_Text", srcValueColumn: "Batch", destValueColumn: "Product", destTextColumn: "Product" },
        ]
    });
        cg.applyAddNewRow();
    cg.bind(data);
    cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());

    //cg.setColumnHeading("Product Name~Price~QTY~Free Qty~Gross Amt~GST Rate~GST Amount~Net Amount~Action");
    //cg.setColumnWidthPer("20~10~10~10~10~10~10~10~5", 1200);
    //cg.setColumnFields("ProductName_Text~Rate~Qty~FreeQty~GrossAmt~GstRate~GstAmt~NetAmt~Delete");
    //cg.setAlign("C~C~C~C~C~C~C~C");

    //cg.defaultHeight = "400px"; 
    //cg.setIdProperty("sno");
    //cg._MinRows = 50;
    //cg.setCtrlType("L~~F.2~F.2~~~~~");
    //cg.applyAddNewRow();
    //cg.setOptionArray(0, ProductList, "ProductName", false, "Product", "PkProductId", "1");
    //var obdt = cg.populateDataFromJson({
    //    srcData: data,
    //    mapData: [{ data: ProductList, textColumn: "ProductName_Text", srcValueColumn: "ProductName_Text", destValueColumn: "PkProductId", destTextColumn: "Product" },
    //    ]
    //});

    //cg.setTotalOn("~~Qty~FreeQty~GrossAmt~~GstAmt~NetAmt~");

    //cg.bind(data);
    //cg.outGrid.setSelectionModel(new Slick.RowSelectionModel());

    /*---------------    ---------------   ---------------   ---------------*/
   
   
    var _data = cg.data.filter(function (el) {
        return el.PkId > 0;
    });
        
        var filterhash = []
    for (var i = 0; i < _data.length; i++) {
        var cc = cg.data[i];
        
        setdisablecolumn('', cg, cc, filterhash, i, 0);
    }
    cg.outGrid.setCellCssStyles("trialRows", filterhash);

    //---------------    ---------------   ---------------   ---------------/
    cg.outGrid.onCellChange.subscribe(function (e, args) {
        if (args.cell != undefined) {
            var field = cg.columns[args.cell].field;
             if (field == "ProductName_Text") {
                var FkProductId = Common.isNullOrEmpty(args.item["ProductName"]) ? 0 : parseFloat(args.item["ProductName"]);

                 var data = cg.getData().filter(function (element) { return (element.FkProductId == FkProductId && element.mode!=2); });

                if (data.length <= 0) {
                    ColumnChange(args, args.row, "Product");
                }
                else {
                    alert("Product Already Add In List");
                    args.item["PkId"] = 0;
                    args.item["mode"] = 0;
                    args.item["FkProductId"] = 0;
                    args.item["ProductName_Text"] = "";
                    args.item["Product"] = 0;
                    args.item["Rate"] = "";
                    args.item["Qty"] = "";
                    args.item["FreeQty"] = "";
                    args.item["GrossAmt"] = "";
                    args.item["GstRate"] = "";
                    args.item["GstAmt"] = "";
                    args.item["NetAmt"] = "";
                    args.item["Delete"] = '';
                    cg.updateRefreshDataRow(args.row);
                    return false;
                }
            }
            else if (field == "Qty") {
                ColumnChange(args, args.row, "Qty");
            }

            //cg.updateRefreshDataRow(args.row);
            //cg.updateAndRefreshTotal();
           
        }
    });

    //---------------    ---------------   ---------------   ---------------/
    cg.outGrid.onClick.subscribe(function (e, args) {

        if (args.cell != undefined) {
            var field = cg.columns[args.cell].field;

            var PkProductId = args.grid.getDataItem(args.row)["PkProductId"];
            var sno = args.grid.getDataItem(args.row)["sno"];

            if (field == "Delete") {
               

                //if (PkProductId > 0) {

                //    var DeletedProductList = JSON.parse($("#hdDeletedProductList").val());
                //    DeletedProductList.push({ PkProductId: PkProductId });
                //    $("#hdDeletedProductList").val(JSON.stringify(DeletedProductList));
                //}
                ColumnChange(args, args.row, "Delete");
                //var data = $.grep(cg.getData(), function (e) {
                //    return e.sno != sno;
                //});

                //BindGrid('DDT', data);


            }
            //cg.updateRefreshDataRow(args.row);
            //cg.updateAndRefreshTotal();
          
            // //console.log(cg.getData());
        }
    });
      
     //---------------    ---------------   ---------------   ---------------/
       cg.outGrid.onContextMenu.subscribe(function (e, args) {

            e.preventDefault();
            var j = cg.outGrid.getCellFromEvent(e);


            // if (j.cell == 1 || j.cell == 2) {
            $("#contextMenu")
                .data("row", j.row)
                .css("top", e.pageY - 90)
                .css("left", e.pageX - 60)
                .show();
            // }

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
            if (command == "EditColumn") {

                Common.GridColSetup(parseInt(FormId), '', function () {
                    var _dtl = GetDataFromGrid();
                    BindGrid('DDT', _dtl);
                });
            }

        });
       
    //---------------    ---------------   ---------------   ---------------/
    });
}
function ColumnChange(args, rowIndex, fieldName) {

    Common.Get(".form", "", function (flag, _d) {

        _d.PkId = $('#PkId').val();
        //_d.FkPartyId = $('#FkPartyId').val();
        _d.EntryDate = $('#EntryDate').val();
        // _d.GRDate = $('#GRDate').val(); 
        _d.TranDetails = GetDataFromGrid();
        
        if (_d.TranDetails.length > 0) {
            ////console.log(_d);
            $.ajax({
                type: "POST",
                url: '/Transactions/' + $('#hdControllerName').val() + '/ColumnChange',
                data: { model: _d, rowIndex: rowIndex, fieldName: fieldName },
                datatype: "json",
                success: function (res) {

                    if (res.status == "success") {
                         // //console.log(res.data);
                        setFooterData(res.data)
                        setGridRowData(args, res.data.TranDetails, rowIndex, fieldName);
                        var filterhash = []
                        setdisablecolumn('', cg, cg.data[rowIndex], filterhash, rowIndex, 0);
                        cg.outGrid.setCellCssStyles("trialRows", filterhash);
}
                    else
                        alert(res.msg);
                }
            })

        }

    });
}
function FooterChange(fieldName) {

    Common.Get(".form", "", function (flag, _d) {

        _d.PkId = $('#PkId').val();
        //_d.FkPartyId = $('#FkPartyId').val();
        _d.EntryDate = $('#EntryDate').val();
        // _d.GRDate = $('#GRDate').val(); 

        _d.TranDetails = GetDataFromGrid();
        if (_d.TranDetails.length > 0) {
            ////console.log(_d);
            $.ajax({
                type: "POST",
                url: '/Transactions/' + $('#hdControllerName').val() + '/FooterChange',
                data: { model: _d, fieldName: fieldName },
                datatype: "json",
                success: function (res) {

                    if (res.status == "success") {
                        
                        //console.log(res.data);
                        setFooterData(res.data)
                        //setGridRowData(args, res.data.TranDetails, rowIndex, fieldName);
                    }
                    else
                        alert(res.msg);
                }
            })

        }

    });
}
function setFooterData(data) {

    //console.log(data);
    Common.Set(".form", data, "", function () {

        Common.InputFormat();
    });
    return false;

}
function setGridRowData(args, data, rowIndex, fieldName) {
    debugger;
    var sno = args.grid.getDataItem(args.row)["mode"];

    //console.log(data);
    //console.log(rowIndex);
    //console.log(data[rowIndex].Rate);
    //  args.item = data[args.row];
    //args.item["qty"] = data[rowIndex].qty;
    //args.item["pkId"] = 0;
    //args.item["fkProductId"] = 0;
    //args.item["productName_Text"] = "";
    //args.item["product"] = 0;
    if (fieldName == 'Delete') {
        //args.item["mode"] = 2;
        args.grid.getDataItem(args.row).mode = 2
        }
    else {
        args.item["PkProductId"] = data[rowIndex].PkProductId;
        args.item["Rate"] = data[rowIndex].Rate;
        args.item["MRP"] = data[rowIndex].MRP;
        args.item["Qty"] = data[rowIndex].Qty;
        args.item["FreeQty"] = data[rowIndex].FreeQty;
        args.item["GrossAmt"] = data[rowIndex].GrossAmt;
        args.item["GstRate"] = data[rowIndex].GstRate;
        args.item["GstAmt"] = data[rowIndex].GstAmt;
        args.item["NetAmt"] = data[rowIndex].NetAmt;
        args.item["mode"] = data[rowIndex].mode;
        args.item["Batch"] = data[rowIndex].Batch;
        args.item["Color"] = data[rowIndex].Color;
        args.item["MfgDate"] = data[rowIndex].MfgDate;
        args.item["ExpiryDate"] = data[rowIndex].ExpiryDate;
        args.item["SaleRate"] = data[rowIndex].SaleRate;
      //  console.log(data[rowIndex].mode);
        args.item["Delete"] = 'Delete';
      
    }
    cg.updateRefreshDataRow(args.row);
    cg.updateAndRefreshTotal();
    return false;
    //cg.outGrid.setData(data);
    //  cg.updateRefreshData();
    //  cg.updateRefreshDataRow(rowIndex);
    //cg.updateRefreshDataRow(args.row);
}

 
$('#btnServerSave').click(function (e) {
    if ($("#loginform1").valid()) {
        SaveRecord();
    }
    return false;
});
function GetDataFromGrid(ifForsave) {
    debugger;
    var array = cg.getData().filter(x => x.FkProductId > 0);
    let number = Math.max.apply(Math, array.map(function (o) { return o.sno; }));
    let sno = number > 0 ? number : 0;
  //  console.log(cg.getData());

    var _d = [];
    cg.getData().filter(function (element) {
        //console.log(element);
        
        if (ifForsave) {
            
            //if (element.mode != 2) {  }
            if (element.ProductName == '' || element.ProductName == null || element.ProductName == undefined
                //   || element.Price == '' || element.Price == null || element.Price == undefined
                || element.Qty == '' || element.Qty == null || element.Qty == undefined
                || element.Rate == '' || element.Rate == null || element.Rate == undefined
                || element.GrossAmt == '' || element.GrossAmt == null || element.GrossAmt == undefined
                || element.NetAmt == '' || element.NetAmt == null || element.NetAmt == undefined
            ) {

            } else {
                
                if (element.FkProductId > 0) { element.sno = element.sno; }
                else { sno++; element.sno = sno; }
                element.FkProductId = parseInt(element.ProductName);
                _d.push(element);
                return element
            }
        }
        else {
            
            
            if (element.ProductName == '' || element.ProductName == null || element.ProductName == undefined
                ////   || element.Price == '' || element.Price == null || element.Price == undefined
                //|| element.QTY == '' || element.QTY == null || element.QTY == undefined
                //|| element.Rate == '' || element.Rate == null || element.Rate == undefined
                //|| element.GrossAmt == '' || element.GrossAmt == null || element.GrossAmt == undefined
                //|| element.NetAmt == '' || element.NetAmt == null || element.NetAmt == undefined
            ) {

            } else {
                debugger;
                if (element.FkProductId > 0) { element.sno =  element.sno; }
                else { sno++; element.sno = sno; }
              
                element.FkProductId = parseInt(element.ProductName);
                _d.push(element);
                return element
            }
        }
        //
       

    });
    //console.log(_d);
    //var filteredHomes = _d.filter(x => x.mode != 2);
    //console.log(filteredHomes);

    return _d
}
function SaveRecord() {

    Common.Get(".form", "", function (flag, _d) {
        if (flag) {
            _d.PkId = $('#PkId').val();
            _d.FkPartyId = $('#FkPartyId').val();
            _d.EntryDate = $('#EntryDate').val();
            _d.GRDate = $('#GRDate').val();
            _d.TranDetails = [];
            if (_d.FkPartyId > 0) {
                _d.TranDetails = GetDataFromGrid(true);

                var filteredDetails = _d.TranDetails.filter(x => x.mode != 2);
                if (_d.TranDetails.length > 0 && filteredDetails.length > 0) {
                    

                    //console.log(_d);

                    $.ajax({
                        type: "POST",
                        url: '/Transactions/' + $('#hdControllerName').val() + '/SaveRecord',
                        data: { model: _d },
                        datatype: "json",
                        success: function (res) {

                            if (res.status == "success") {
                                alert('Save Successfully..');
                                window.location = '/Transactions/' + $('#hdControllerName').val() + '/List';
                            }
                            else
                                alert(res.msg);
                        }
                    })

                }
                else
                    alert("Insert Valid Product Data..");
            }
            else
                alert("Please Select Party");
        }
        else
            alert("Some Error found.Please Check");
    });
}

function setdisablecolumn(Grid, cg, cc, hash, index, type) {
    var focjson = {};
    var h = (hash[index] = {});
    var focu = {};
    if (cc["FkProductId"] >0) {
        h["ProductName_Text"] = "sobc";
        focu["ProductName_Text"] = { "focusable": true };

        //h["Qty"] = "sdbc";
        //focu["Qty"] = { "focusable": true };
    }
    if (cc["mode"] ==2) {
        h["ProductName_Text"] = "sobc";
        focu["ProductName_Text"] = { "focusable": false };
        h["Delete"] = "sobc";
        focu["Delete"] = { "focusable": false };
        h["Qty"] = "sobc";
        focu["Qty"] = { "focusable": false };

        //h["Qty"] = "sdbc";
        //focu["Qty"] = { "focusable": true };
    }
    //console.log(focu);
    focjson["columns"] = focu;
    return focjson;

};





