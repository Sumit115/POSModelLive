var coGrid = function (grd) {
    var thisObj;
    var columnFilters;
    this.data;
    this.columns;
    this.options;
    this.fields;
    this.dataTypes;
    this.alignTypes = "";
    this.ctrlTypes;
    this.ctrlTypesArray;
    this.outGridName = grd;
    this.contaxtMenuName = grd + "_contextMenu";
    this.gridType = "unfreez";
    this.outGrid;
    this.dataView;
    this.GridDataChanged = false;
    this.sortcol = null;
    this.sortdir = null;
    this.srColumn = null;
    this.searchColumns = [];
    this.checkAllColumns = [];
    this.objMapData = {};
    this.isReadOnly = false;
    this.RowIdIncrease = 1000;
    this.selectOnSearch = false;
    this.objectIdProperty = "";

    //$(grd).on('blur', 'input.editor-text', function() {
    //if(thisObj.outGrid.getEditorLock().isActive())
    //   thisObj.outGrid.getEditorLock().commitCurrentEdit();
    //});

    if (arguments.length > 0)
        this.dataView = arguments[1]


    if (arguments.length > 1)
        columnFilters = arguments[2]


    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function indexOf(member, startFrom) {
            /*
            In non-strict mode, if the 'this' variable is null or undefined, then it is
            set to the window object. Otherwise, 'this' is automatically converted to an
            object. In strict mode, if the 'this' variable is null or undefined, a
            'TypeError' is thrown.
            */
            if (this == null) {
                throw new TypeError("Array.prototype.indexOf() - can't convert '" + this + "' to object");
            }

            var
              index = isFinite(startFrom) ? Math.floor(startFrom) : 0,
              that = this instanceof Object ? this : new Object(this),
              length = isFinite(that.length) ? Math.floor(that.length) : 0;

            if (index >= length) {
                return -1;
            }

            if (index < 0) {
                index = Math.max(length + index, 0);
            }

            if (member === undefined) {
                /*
                  Since 'member' is undefined, keys that don't exist will have the same
                  value as 'member', and thus do need to be checked.
                */
                do {
                    if (index in that && that[index] === undefined) {
                        return index;
                    }
                } while (++index < length);
            } else {
                do {
                    if (that[index] === member) {
                        return index;
                    }
                } while (++index < length);
            }

            return -1;
        };
    }

    this.setOption = function (opt, val) {
        this.options[opt] = val;
    };
    this.setSortableColumns = function (sortVals, _srColumn) {

        this.options.multiColumnSort = true;
        //this.options.explicitInitialization= true;
        //this.options.showHeaderRow= true;
        //alert(sortVals);
        this.setColumnAttrWithValue("sortable", sortVals, true)

        if (_srColumn != null && _srColumn != "")
            this.srColumn = _srColumn;

    };
    this.setCheckboxColumns = function (cols) {

        var c = cols.split('~');
        for (kk = 0; kk < c.length; kk++) {
            var cl = c[kk];
            var col = this.columns[cl]
            col.formatter = Slick.Formatters.CheckmarkAll;
            col.editor = Slick.Editors.CheckboxEditor;
        }

    };
    this.setLinkColumn = function (cols) {

        var c = cols.split('~');
        for (kk = 0; kk < c.length; kk++) {
            var cl = c[kk];
            var col = this.columns[cl]
            col.formatter = Slick.Formatters.LinkAll;
        }

    };

    this.setLinkJSColumn = function (cols) {

        var c = cols.split('~');
        for (kk = 0; kk < c.length; kk++) {
            var cl = c[kk];
            var col = this.columns[cl]
            col.formatter = Slick.Formatters.LinkAllJS;
        }

    };

    this.setCheckAllCheckboxColumns = function (_checkAllColumns) {

        this.options.showHeaderRow = true;
        this.checkAllColumns = _checkAllColumns.split('~');


    };

    this.setSearchableColumns = function (_searchColumns) {

        this.options.showHeaderRow = true;
        this.options.explicitInitialization = true;
        this.searchColumns = _searchColumns.split('~');


    };

    this.addBlankRowAt = function (index) {
        var maxCount = this.addNewMaxRowIdCount();

        var nw = {};
        nw[this.objectIdProperty] = maxCount;
        nw["record_added_auto"] = "1";

        this.dataView.insertItem(index, nw);
        this.updateRefreshData();

    }

    this.deleteLastBlankRow = function () {
        var ro = this.data.length - 1;

        if (typeof (this.data[ro].record_added_auto) != 'undefined') {
            if (this.data[ro].record_added_auto == "1") {
                this.data.splice(ro, 1);
                this.updateRefreshData();
                return true;
            }

        }
        return false;

    }
    this.MaxRowIdCount = 0;
    this.getMaxRowIdCount = function () {

        try {
            if (this.MaxRowIdCount <= 0) {
                for (var kk = 0; kk < this.data.length; kk++) {
                    var nVal = parseInt(this.data[kk][this.objectIdProperty], 10);
                    if (this.MaxRowIdCount < nVal)
                        this.MaxRowIdCount = nVal;
                }
                this.MaxRowIdCount = this.MaxRowIdCount + this.RowIdIncrease;

            }
        }
        catch (e) { }

        return this.MaxRowIdCount;


    }
    this.addNewMaxRowIdCount = function () {

        var maxCount = this.getMaxRowIdCount();
        maxCount = maxCount + 1;
        this.MaxRowIdCount = maxCount;
        return this.MaxRowIdCount;

    }

    this.addBlankRows = function () {
        var maxCount = this.addNewMaxRowIdCount();

        var nw = {};
        nw[this.objectIdProperty] = maxCount;
        nw["record_added_auto"] = "1";
        this.data.push(nw);

    }

    this.refresh = function () {

        var GrDName = this.outGridName.replace('#', '');
        if (this.defaultHeight != null && this.defaultHeight != undefined)
            $('#' + GrDName + '').css("height", this.defaultHeight)

        this.outGrid.resizeCanvas();

    }

    this.expand = function (obj) {


        var high = this.defaultHeight;
        var minrow = this._MinRows;

        var GrDName = this.outGridName.replace('#', '');

        if (typeof (obj) != "undefined") {
            if (typeof (obj.height) != "undefined")
                high = obj.height;

            if (typeof (obj.minrows) != "undefined" && parseInt(obj.minrows, 10) > 0)
                minrow = parseInt(obj.minrows, 10);
        }

        this.defaultHeight = high;
        $('#' + GrDName + '').css("height", this.defaultHeight)



        if (this._MinRows < minrow) {
            while (this.getData().length < minrow)
                this.addBlankRowAt(this.getData().length);
        }
        else if (this._MinRows > minrow) {
            var bdelete = true;
            while (bdelete && this.getData().length > minrow)
                bdelete = this.deleteLastBlankRow();
        }

        this._MinRows = minrow;
        this.outGrid.resizeCanvas();

    }
    this.bind = function () {


        $('#gridstyle').remove()
        $('<style type="text/css" id="gridstyle" > .__gridDisable div{ background-color:rgb(235, 235, 228) } </style>').appendTo('head')

        var GrDName = this.outGridName.replace('#', '');
        if (this.defaultHeight != null && this.defaultHeight != undefined)
            $('#' + GrDName + '').css("height", this.defaultHeight)

        if (arguments.length > 0) {
            this.data = arguments[0];
            if (this._MinRows > 0) {

                while (this.data.length < this._MinRows) {
                    var nw = {};
                    nw[this.objectIdProperty] = this.addNewMaxRowIdCount();
                    nw["record_added_auto"] = "1";
                    this.data.push(nw);

                }
            }
        }




        if (this.objectIdProperty == "") {
            this.objectIdProperty = this.fields[0];
        }

        this.options.GridName = GrDName;

        this.outGrid = new Slick.Grid(this.outGridName, this.dataView, this.columns, this.options);

        this.outGrid.thisObj = this;


        if (typeof (this.metaFunction) == "function")
            this.dataView.getItemMetadata = this.metaFunction;

        if (this.options.multiColumnSort) {
            this.bindForSort();
        }
        if (this.options.showHeaderRow) {



            this.bindForSearch();
        }

        this.outGrid.init();


        this.dataView.beginUpdate();
        this.dataView.setItems(this.data, this.objectIdProperty);
        this.dataView.setFilter(this.filter);
        this.dataView.setFilterArgs({ tObj: this })
        this.dataView.endUpdate();



        if (!this.options.showHeaderRow) {
            this.outGrid.invalidate();
            this.outGrid.render();
        }


        this.outGrid.setSelectionModel(new Slick.CellSelectionModel());
        this.outGrid.__dataChanged = false;

        this.outGrid.onCellChange.subscribe(function (e, args) {
            var ob__this = this;
            if (typeof (ob__this.thisObj) == "undefined")
                ob__this = args.grid;
            ob__this.__dataChanged = true;
            if (ob__this.thisObj.isTotalGrid)
                ob__this.thisObj.updateAndRefreshTotal();
        })

        $(this.outGridName).on('blur', 'input.editor-checkbox', function () {

            //Slick.GlobalEditorLock.commitCurrentEdit();
        });


        if (this.options.enableAddRow) {
            this.bindForAddNew();
        }

        if (this.isTotalGrid)
            this.ApplyTotal();

        //$(this.TotalGridName).html("Total : --- ")
        this.outGrid.onBeforeEditCell.subscribe(function (e, args) {

            var dp = $(thisObj.outGridName).prop("disabled");
            if (dp != undefined)
                if (dp == "disabled" || dp == true) {
                    if (args.column.ignoreDisableGrid != undefined && args.column.ignoreDisableGrid == true)
                        return true;

                    e.stopImmediatePropagation();
                    return false;
                }

            dp = $(thisObj.outGridName).prop("griddisabled");
            if (dp != undefined)
                if (dp == "disabled" || dp == true) {
                    if (args.column.ignoreDisableGrid != undefined && args.column.ignoreDisableGrid == true)
                        return true;
                    e.stopImmediatePropagation();
                    return false;
                }

        });


        this.outGrid.onClick.subscribe(function (e, arg) {

            if ($(e.target).prop("type") == "checkbox") {
                var dp = $(thisObj.outGridName).prop("disabled");
                if (dp != undefined)
                    if (dp == "disabled") {
                        $(e.target)[0].checked = !($(e.target)[0].checked)
                        //e.stopImmediatePropagation();
                        return false;
                    }
                var item = thisObj.outGrid.getDataItem(arg.row);
                item[thisObj.columns[arg.cell].field] = $(e.target)[0].checked;


            }


        })

        this.enableCopy();



    };


    this.isTotalGrid = false;
    this.TotalGridName = "";
    this.TotalColNames = "";
    this.totalCaller;
    this.setTotalOn = function (_totalGridName, colNames, callerFunction) {

        this.options.createFooterRow = true;
        this.options.showFooterRow = true;

        $(_totalGridName).hide();

        this.TotalGridName = _totalGridName;
        this.isTotalGrid = true;

        if (colNames == undefined || colNames == "")
            colNames = _totalGridName;

        this.TotalColNames = colNames;
        this.TotalData[0] = {};
        if (this.TotalColNames.length > 0) {
            var clnm = this.fields;
            for (cc = 0; cc < clnm.length; cc++)
                if (this.TotalColNames.indexOf(clnm[cc]) >= 0)
                    this.TotalData[0][clnm[cc]] = 0;
        }

        this.totalCaller = callerFunction;


    };

    this.updateTotalData = function () {
        var i = 0, l = 0;
        l = this.dataView.getLength();

        if (this.TotalColNames.length > 0) {
            var clnm = this.TotalColNames.split('~');

            for (i = 0; i < l; i++) {
                var dvRow = this.dataView.getItem(i);
                for (cc = 0; cc < clnm.length; cc++) {
                    var cll = clnm[cc];
                    if (this.TotalColNames.indexOf(cll) >= 0)
                        if (!isNaN(parseFloat(dvRow[cll]))) {
                            var toFx = 0;
                            toFx = "2";// $(this.columns).filterArray("[field='"+cll+"']")[0].decimalPlaces
                            this.TotalData[0][cll] = parseFloat(parseFloat(this.TotalData[0][cll]) + parseFloat(dvRow[cll])).toFixed(toFx);
                        }
                }
            }
        }
    };


    this.gridTotal;

    this.TotalData = [];
    this.TotalDataView;
    this.footerHeight;

    this.ApplyTotal = function () {

        if (this.footerHeight != undefined && this.footerHeight != null)
            $('.slick-footerrow-column', this.outGridName).css("height", this.footerHeight)
        else
            $('.slick-footerrow-column', this.outGridName).css("height", "25")

        if (this.alignTypes != "" && this.alignTypes != undefined) {
            var al = this.alignTypes.split("~");
            for (cc = 0; cc < al.length; cc++) {
                if (al[cc] != "")
                    $($('.slick-footerrow-column')[cc]).addClass(al[cc]);
            }

        }

        this.updateTotalData();

        this.updateAndRefreshTotal();


        return;

        this.TotalDataView = new Slick.Data.DataView();

        var options1 = { editable: false, enableAddRow: false, enableCellNavigation: true, enableColumnReorder: false, asyncEditorLoading: false, multiSelect: false };


        var tcolumns = []; //this.columns;

        for (var i = 0; i < this.columns.length; i++)
            tcolumns[i] = $.extend(true, {}, this.columns[i]);


        var ct = this.ctrlTypes.split('~')

        for (kk = 0; kk < ct.length; kk++) {
            if (ct[kk] == "B") {
                tcolumns[kk].editor = Slick.Editors.Text
                tcolumns[kk].formatter = null;
            }
        }


        this.gridTotal = new Slick.Grid(this.TotalGridName, this.TotalData, tcolumns, options1);
        $(this.TotalGridName).find(".slick-header").hide();
        $(this.TotalGridName).find(".slick-viewport").css("overflow-y", "hidden");

        $(this.outGridName).find(".slick-viewport").css("overflow-x", "hidden");

        $(this.TotalGridName).find(".slick-viewport").on("scroll", function () {

            $(thisObj.outGridName).find(".slick-viewport").scrollLeft($(this).scrollLeft());

        })


        this.gridTotal.init();

        //$(this.TotalGridName).css("height","300px");


        this.gridTotal.invalidate();

        this.gridTotal.render();


    };

    this.TotalCaption = "Total";
    this.TotalCaptionIndex = -1;

    this.updateAndRefreshTotal = function () {


        if (this.options.createFooterRow) {
            var clnm = this.fields;
            for (cc = 0; cc < clnm.length; cc++) {
                if (this.TotalColNames.indexOf(clnm[cc]) >= 0)
                    this.TotalData[0][clnm[cc]] = 0;
                if (this.TotalCaptionIndex == cc) {
                    this.TotalData[0][clnm[cc]] = this.TotalCaption;
                }
            }
            this.updateTotalData();
            if (this.TotalData.length > 0) {
                var tcols = this.TotalColNames.split("~");
                for (kk = 0; kk < tcols.length; kk++) {
                    var cll = tcols[kk];
                    $(this.outGrid.getFooterRowColumn(cll)).text(this.TotalData[0][cll])
                }

                if (this.totalCaller != undefined) {
                    this.totalCaller(this.TotalData, this.outGrid);
                }

            }

        }


        return;

        if (this.gridTotal != undefined) {
            var clnm = this.fields;
            for (cc = 0; cc < clnm.length; cc++) {
                if (this.TotalColNames.indexOf(clnm[cc]) >= 0)
                    this.TotalData[0][clnm[cc]] = 0;
                if (this.TotalCaptionIndex == cc) {
                    this.TotalData[0][clnm[cc]] = this.TotalCaption;
                }
            }
            this.updateTotalData();
            this.gridTotal.invalidate();
            this.gridTotal.render();
        }

    };

    this.bindForSort = function () {

        thisObj = this;

        this.outGrid.onSort.subscribe(function (e, args) {
            if (thisObj.sortdir === null) {
                thisObj.sortdir = args.sortCols[0].sortAsc;
            }
            else {
                thisObj.sortdir = !thisObj.sortdir;
            }

            thisObj.sortcol = args.sortCols[0].sortCol.field;
            thisObj.dataView.sort(thisObj.comparer, thisObj.sortdir);

            if (thisObj.srColumn != null) {
                for (var i = 0, l = thisObj.dataView.getLength() ; i < l; i++) {
                    var cc = thisObj.dataView.getItem(i);
                    cc[thisObj.srColumn] = (i + 1);
                }
            }

            thisObj.outGrid.invalidate();
            thisObj.outGrid.render();
        });

    };

    this.applyAddNewRow = function (callFunction) {
        this.setOption("", true);
        this.setOption("enableAddRow", true);
        this.addCallFunction = callFunction;

    };
    this.setFocusFalse = function (colIds) {



    };

    this._MinRows = 0;

    this.SetMinRow = function (minRows) {
        this._MinRows = minRows;
    };

    this.addCallFunction = null;

    this.bindForAddNew = function (callFunction) {
        thisObj = this;
        this.outGrid.onAddNewRow.subscribe(function (e, args) {
            var item = args.item;

            var roooid = thisObj.addNewMaxRowIdCount();


            if (thisObj.addCallFunction != undefined)
                roooid = thisObj.addCallFunction();

            item[thisObj.objectIdProperty] = roooid;

            thisObj.outGrid.invalidateRow(thisObj.data.length);
            thisObj.data.push(item);
            thisObj.dataView.beginUpdate();
            thisObj.dataView.setItems(thisObj.data, thisObj.objectIdProperty)
            thisObj.dataView.endUpdate();
            thisObj.refreshData();


            if (typeof (thisObj.metaFunction) == "function")
                thisObj.dataView.getItemMetadata = thisObj.metaFunction;

        });
    };

    this.refreshData = function () {


        this.outGrid.invalidate();
        this.outGrid.render();

    };
    this.refreshDataRow = function (rowId) {


        this.outGrid.invalidate(rowId);
        this.outGrid.render();

    };

    this.clearData = function () {

        this.data = [];
        this.updateRefreshData();

    }

    this.updateRefreshData = function () {

        this.dataView.beginUpdate();
        this.dataView.setItems(this.data, this.objectIdProperty)
        this.dataView.endUpdate();
        this.outGrid.invalidate();
        this.outGrid.render();


        if (typeof (this.metaFunction) == "function")
            this.dataView.getItemMetadata = this.metaFunction;


    };
    this.updateRefreshDataRow = function (rowId) {

        this.dataView.beginUpdate();
        this.dataView.setItems(this.data, this.objectIdProperty)
        this.dataView.endUpdate();
        this.outGrid.invalidateRow(rowId);
        this.outGrid.render();


        if (typeof (this.metaFunction) == "function")
            this.dataView.getItemMetadata = this.metaFunction;


    };
    this.bindForSearch = function () {

        thisObj = this;


        this.dataView.onRowCountChanged.subscribe(function (e, args) {
            thisObj.outGrid.updateRowCount();
            //thisObj.outGrid.render();
            if (thisObj.srColumn != null) {
                for (var i = 0, l = thisObj.dataView.getLength() ; i < l; i++) {
                    var cc = thisObj.dataView.getItem(i);
                    cc[thisObj.srColumn] = (i + 1);
                }
            }
        });

        this.dataView.onRowsChanged.subscribe(function (e, args) {
            thisObj.outGrid.invalidateRows(args.rows);
            thisObj.outGrid.render();



            //thisObj.updateAndRefreshTotal();

        });

        $(this.outGrid.getHeaderRow()).on("change keydown keyup blur", ":input", function (e) {

            if (e.type == "keyup" && e.keyCode == 40) {
                $('div:eq(0)', $('.slick-row:eq(0)', this.outGrid)).trigger("click")
            }
            var columnId = $(this).data("columnId");
            //alert(columnId);
            if (columnId != null && e.type != "keydown") {
                columnFilters[columnId] = $.trim($(this).val());
                thisObj.dataView.refresh();
                thisObj.updateAndRefreshTotal();
            }
            if (e.type == "keyup" && columnId != null) {
                if (typeof (thisObj.onAfterSearch) != "undefined")
                    thisObj.onAfterSearch()
            }

            //$('#HLnk_Know_Bank').text( $('#HLnk_Know_Bank').text() + "~" + e.type)
            if (thisObj.selectOnSearch) {
                if (e.type == "keyup") {

                    $('div:eq(0)', $('.slick-row:eq(0)', this.outGrid)).trigger("click")

                    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != undefined) {
                        $(e.target).focus();
                    }
                }
                if (e.type == "keydown" && e.keyCode == 13) {
                    $('div:eq(0)', $('.slick-row:eq(0)', this.outGrid)).trigger("dblclick")
                }
            }
        });

        this.outGrid.onHeaderRowCellRendered.subscribe(function (e, args) {

            //alert(args.node);
            $(args.node).empty();

            if (thisObj.searchColumns.length > 0) {
                if (thisObj.searchColumns.indexOf(args.column.field) >= 0) {
                    $("<input type='text' class='width98' autocomplete='off'  />")
                       .data("columnId", args.column.id)
                       .val(columnFilters[args.column.id])
                       .appendTo(args.node);
                }
            }
            if (thisObj.checkAllColumns.length > 0) {
                if (thisObj.checkAllColumns.indexOf(args.column.field) >= 0) {
                    var $chk = $("<input type='checkbox' class='cg_chkall'  colnm='" + args.column.field + "'  id='select_" + args.column.field + "' />");
                    $chk.appendTo(args.node);
                    $chk.on("click", function () {
                        var colnm = $(this).attr("colnm")
                        for (var i = 0, l = thisObj.dataView.getLength() ; i < l; i++) {
                            var item = thisObj.dataView.getItem(i);
                            item[colnm] = $(this).prop("checked");
                        }

                        thisObj.outGrid.invalidate();
                        thisObj.outGrid.render();
                    });
                }
            }

            if (thisObj.onHeaderRowRender != undefined)
                thisObj.onHeaderRowRender(args);


            if (thisObj.focusHeader) {
                $('input:eq(0)', thisObj.outGrid.getHeaderRow()).focus();
            }


        });



    };

    this.cogird_chkAll = function (colName) { alert(colName) };

    this.filter = function (item) {
        if (arguments.length > 0)
            if (arguments[1] != undefined)
                if (arguments[1].tObj != undefined)
                    thisObj = arguments[1].tObj;

        for (var columnId in columnFilters) {
            if (columnId !== undefined && columnFilters[columnId] !== "") {


                var c = thisObj.outGrid.getColumns()[thisObj.outGrid.getColumnIndex(columnId)];

                if (c != undefined && c.field != undefined) {

                    var x = item[c.field] === null ? "" : item[c.field].toString().toLowerCase();
                    var y = columnFilters[columnId].toString().toLowerCase();

                    if (c.searchType == 0 && x.indexOf(y) != 0) {
                        //  Search in Starting
                        return false;
                    }
                    if (c.searchType == 1 && x.indexOf(y) < 0) {
                        //  Search Anywhere in string
                        return false;
                    }
                }
                else {

                    var x = item[columnId] === null ? "" : item[columnId].toString().toLowerCase();
                    var y = columnFilters[columnId].toString().toLowerCase();

                    if (x.indexOf(y) != 0) {
                        //  Search in Starting
                        return false;
                    }
                }



            }
        }
        return true;
    };



    this.comparer = function (a, b) {
        var x = a[thisObj.sortcol], y = b[thisObj.sortcol];
        x = (x === null ? "" : x.toUpperCase());
        y = (y === null ? "" : y.toUpperCase());
        return (x > y ? 1 : -1);
    };


    this.isDataChanged = function () {
        return this.outGrid.__dataChanged;
    };
    this.initGrid = function () {
        this.options = {
            editable: true, enableAddRow: false, enableCellNavigation: true,
            enableColumnReorder: false, asyncEditorLoading: false, multiSelect: false
        };
        this.data = [];
        this.columns = [];

        if (this.dataView == undefined || this.dataView == null)
            this.dataView = new Slick.Data.DataView();

        if (columnFilters == undefined || columnFilters == null)
            columnFilters = {};

        //this.outGrid = new Slick.Grid(this.outGridName, this.dataView, this.columns, this.options );

    };

    this.setColumnHeading = function (colNames) {

        var s = colNames.split('~');
        var k = 0;
        for (k = 0; k < s.length; k++) {
            this.columns.push({ id: s[k], name: s[k] });
        }

    };
    this.oncellchange = function (callback) {

        this.outGrid.onCellChange.subscribe(function (e, args) {

            callback(e, args);

        })


    };
    this.setAlign = function (alignVals) {

        var ali = alignVals.replace(/R/g, "ar").replace(/C/g, "ac").replace(/L/g, "al")
        this.alignTypes = ali;
        this.setColumnAttr("cssClass", ali)

    };
    this.setDataType = function (dataVals) {
        this.dataTypes = dataVals.split('~');

        var ali = dataVals.replace(/I/g, "ar").replace(/F/g, "ar").replace(/D/g, "ac")

        this.setColumnAttr("cssClass", ali)

    };
    this.setColumnFields = function (colNames) {
        thisObj = this;
        this.fields = colNames.split('~');
        this.setColumnAttr("field", colNames);

        var stype = "";
        for (kk = 0; kk < this.fields.length; kk++)
            stype = stype + "0~";
        this.setColumnAttr("searchType", stype);
        this.setColumnAttr("id", colNames);



    };
    this.setIdProperty = function (colName) {
        this.objectIdProperty = colName;
    };
    this.getData = function () {
        return this.outGrid.getData().getItems();
    };


    this.setButtonClick = function (colIdx, callerFunction) {


        if (colIdx < 0) {
            for (k = 0; k < this.columns.length; k++) {
                var col = this.columns[k];
                col.onButtonClick = function (args) {
                    callerFunction(args)
                }
            }
        }
        else {
            var col = this.columns[colIdx];
            col.onButtonClick = function (args) {
                callerFunction(args)
            }
        }

    }

    this.setOptionArray = function (colIdx, dbList, fieldval, fromList, txtField, valField, sendMapCol) {

        var col = this.columns[colIdx];
        col.optionsArray = dbList;
        if (fieldval != undefined && fieldval != null && fieldval != "")
            col.fieldval = fieldval;
        if (fromList != undefined && fromList != null && fromList == false)
            col.editorIsItemFromList = false;

        if (sendMapCol != undefined && sendMapCol != null && sendMapCol == "1")
            col.sendMapColumn = "1";

        if (txtField != undefined && txtField != null && txtField != "")
            col.txtField = txtField;
        else
            col.txtField = "txt";

        if (valField != undefined && valField != null && valField != "")
            col.valField = valField;
        else
            col.valField = "val";

    };
    this.populateDataFromJson = function (obj) {

        var objData = obj.srcData;
        var objArray = obj.mapData;
        this.objMapData = obj.mapData;
        var addAutoNumber = obj.addAutoNumber;
        var addSrNumber = obj.addSrNumber;

        var dateFormateColumns = obj.dateFormateColumns;
        if ((objArray != undefined && objArray != null) || addAutoNumber != undefined || dateFormateColumns != undefined) {
            for (kk = 0; kk < objData.length; kk++) {
                if (obj.beforeItrateRow != undefined) {
                    obj.beforeItrateRow(objData[kk])
                }

                if (addAutoNumber != undefined && addAutoNumber != null) {
                    objData[kk][addAutoNumber] = kk + 1;
                }
                if (addSrNumber != undefined && addSrNumber != null) {
                    objData[kk][addSrNumber] = kk + 1;
                }

                if (dateFormateColumns != undefined && dateFormateColumns != null) {
                    for (dd = 0; dd < dateFormateColumns.length; dd++) {
                        var col = dateFormateColumns[dd];
                        var ddt = objData[kk][col];
                        if (ddt != null && ddt != "") {
                            ddt = ddt.substring(0, 10);
                            var d = ddt.split('-');
                            var dt = ddt;
                            if (d.length == 3)
                                dt = d[2] + "-" + d[1] + "-" + d[0];


                            ddt = SlickFormatstrDt(dt)
                            objData[kk][col] = ddt;
                        }
                    }
                }

                if ((objArray != undefined && objArray != null)) {
                    for (cc = 0; cc < objArray.length; cc++) {
                        var oo = objArray[cc];
                        var odt = oo.data;

                        var vlu = $.trim(objData[kk][oo.srcValueColumn])
                        objData[kk][oo.srcValueColumn] = vlu;

                        var fltr = "[" + oo.destValueColumn + "='" + vlu + "']"
                        obfltr = $(odt).filterArray(fltr)

                        var txtData = vlu
                        if (obfltr.length > 0)
                            txtData = obfltr[0][oo.destTextColumn];

                        objData[kk][oo.textColumn] = txtData;
                    }
                }
            }
        }
        return objData;

    };


    this.setSearchType = function (searchTypes) {

        var s = searchTypes.split('~');
        for (k = 0; k < s.length && k < this.columns.length; k++) {
            var col = this.columns[k]
            col.searchType = s[k];
        }


    }

    this.setCtrlType = function (ctrlTypes) {
        this.ctrlTypes = ctrlTypes;
        var s = ctrlTypes.split('~');
        this.ctrlTypesArray = s;
        var k = 0;
        for (k = 0; k < s.length && k < this.columns.length; k++) {
            var col = this.columns[k]
            var a = "";
            var sa = s[k]
            if (sa.length > 1) {
                var d = s[k].split('.');
                sa = d[0];
                a = d[1];
            }
            switch (sa) {
                case "": col.focusable = false; break;
                case "T": col.editor = Slick.Editors.Text; break;
                case "BT": col.editor = Slick.Editors.ButtonEditor;
                    col.formatter = Slick.Formatters.ButtonFormatter; break;
                case "BD": col.formatter = Slick.Formatters.DeleteFormatter; break;
                case "I":
                    col.decimalPlaces = 0;
                    col.editor = Slick.Editors.Integer; break;
                case "D": col.editor = Slick.Editors.Date1;
                    break;
                case "D1": col.editor = Slick.Editors.Date1;
                    col.formatter = Slick.Formatters.newDateFormatter;
                    break;
                case "D2": col.focusable = false;
                    col.formatter = Slick.Formatters.newDateFormatter;
                    break;
                case "D3": col.editor = Slick.Editors.Time1;
                    break;
                case "C": col.editor = Slick.Editors.ComboSelect; break;
                case "L": col.editor = Slick.Editors.TextList; break;
                case "LN": col.editor = Slick.Editors.TextListNew; break;
                case "CD": col.editor = Slick.Editors.Dropdown2; break;
                case "MS": col.editor = Slick.Editors.MultiSelect; break;
                case "B": col.editor = Slick.Editors.CheckboxEditor;
                    col.formatter = Slick.Formatters.CheckmarkAll; break;
                case "F": col.editor = Slick.Editors.Float;
                    if (a != "") {
                        col.decimalPlaces = parseInt(a, 10);
                        col.editorFixedDecimalPlaces = parseInt(a, 10);
                    }
                    col.formatter = Slick.Formatters.newDecimalFormatter; break;
                    break;
            }
        }

    };


    this.setColumnSetting = function (caller) {
        var k = 0;
        for (k = 0; k < this.columns.length; k++) {
            var col = this.columns[k]
            caller(col, k);
        }
        /*
        ---     How to Use
        
        
        
    cg.setColumnSetting(function(col,k){
    
            if(k==1 || k==3)
            {
                 
                col.formatter=Slick.Formatters.newDateFormatter;
                row, cell, value, columnDef, dataContext
            }
            
    
        })
        
        
        */

    }

    this.addDataRow = function (data1) {
        var jsn = {};
        if (this.fields.length > 0) {
            var d = data1.split('~');
            for (kk = 0; kk < this.fields.length && kk < d.length ; kk++) {
                switch (this.dataTypes[kk]) {
                    case "I": jsn[this.fields[kk]] = parseInt(d[kk], 10); break;
                    case "F": jsn[this.fields[kk]] = parseFloat(d[kk]); break;
                    default: jsn[this.fields[kk]] = d[kk]; break;
                }

            }
            this.data.push(jsn);
        }
    };


    this.setColumnWidth = function (colNames) {
        var s = colNames.split('~');
        var k = 0;
        for (k = 0; k < s.length && k < this.columns.length; k++) {
            var col = this.columns[k]
            col["width"] = parseInt(s[k], 10);
        }
        //this.setColumnAttr("width",colNames);
    };
    this.setColumnWidthPer = function (colNames, totalwidth) {
        var s = colNames.split('~');
        var k = 0;
        for (k = 0; k < s.length && k < this.columns.length; k++) {
            var col = this.columns[k]

            var colw = parseInt(parseInt(totalwidth, 10) * parseInt(s[k], 10) / 100, 10);

            col["width"] = colw;




        }
        //this.setColumnAttr("width",colNames);
    };
    this.setColumnAttr = function (attrName, colNames) {
        var s = colNames.split('~');
        var k = 0;
        for (k = 0; k < s.length && k < this.columns.length; k++) {
            var col = this.columns[k]
            col[attrName] = s[k];
        }

    };
    this.setColumnAttrWithValue = function (attrName, colNames, cellValue) {
        var s = colNames.split('~');
        var k = 0;
        var cclstr = "~~" + colNames + "~~";
        for (k = 0; k < this.columns.length; k++) {
            var col = this.columns[k]
            var flll = "~" + col.field + "~"
            if (cclstr.indexOf(flll) > 0)
                col[attrName] = cellValue;
        }

    };
    this.initGrid();

    this.SlickSetHeadingArr = function () {

        this.SlickSetHeading(arguments);


    };

    this.menuItems = [];

    this.callMenuObj;


    this.setContextMenu = function (objData) {

        for (kk = 0; kk < objData.menuItems.length; kk++)
            if (this.menuItems.indexOf(objData.menuItems[kk]) < 0)
                this.menuItems.push(objData.menuItems[kk]);

        objData.menuItems = this.menuItems;

        if (typeof (this.callMenuObj) == "undefined")
            this.callMenuObj = objData;
        else {
            this.callMenuObj.menuItems = objData.menuItems;
            if (objData != null && objData.onMenuEventDelete != undefined) {
                this.callMenuObj.onMenuEventDelete = objData.onMenuEventDelete;
            }
            else if (objData != null && objData.onMenuEvent != undefined) {
                this.callMenuObj.onMenuEvent = objData.onMenuEvent;
            }
        }

        menuCreate(this.callMenuObj);

    };

    this.focusHeader = true;
    this.focus = function () {

        this.focusHeader = true;


    }

    this.onEnterKey = function (caller) {


        this.outGrid.onKeyDown.subscribe(function (e, arg) {

            if (e.keyCode == 13) {
                caller(e, arg);
            }


        });

    };

    function menuCreate(objData) {

        //menyArray, actionCallBack

        var callerObj = objData;
        thisObj.outGrid.onContextMenu.unsubscribe()
        thisObj.outGrid.onContextMenu.subscribe(function (e) {

            $('.menuClass').hide();
            var cell = thisObj.outGrid.getCellFromEvent(e);

            if (typeof (thisObj.callMenuObj.onMenuValidate) != "undefined") {
                if (!thisObj.callMenuObj.onMenuValidate(cell, thisObj))
                    return;
            }

            e.preventDefault();
            $(thisObj.contaxtMenuName)
                .data("row", cell.row)
                .data("col", cell.cell)
                .data("thisObj", thisObj)
              .css("top", e.pageY -88)
              .css("left", e.pageX - 52)
              .show();



        })


        var mnuStr = '<ul  class="menuClass" id="' + thisObj.contaxtMenuName.replace('#', '') + '" style="display:none;position:absolute">';
        var mnuList = "";

        objData.menuItems.sort();

        for (kk = 0; kk < objData.menuItems.length; kk++) {
            mnuList = mnuList + '<li class="menuItemClass" action="' + objData.menuItems[kk] + '">' + objData.menuItems[kk] + '</li>'
        }
        $(thisObj.contaxtMenuName).remove();
        if ($(thisObj.contaxtMenuName, 'body').length > 0) {
            $(thisObj.contaxtMenuName).html('')
            $(thisObj.contaxtMenuName).append(mnuList)
        }
        else {
            mnuStr = mnuStr + mnuList;
            mnuStr = mnuStr + "</ul>"
            $('body').append(mnuStr)
        }
        $('body').on("click", function () {

            $('.menuClass').hide();

        });

        $(thisObj.contaxtMenuName).click(function (e) {

            if (!$(e.target).is("li")) {
                return;
            }

            var objDt = {};
            objDt.row = $(this).data().row;
            objDt.col = $(this).data().col;
            objDt.thisObj = $(this).data().thisObj;
            objDt.e = e;
            objDt.action = $(e.target).attr("action")
            objDt.actionText = $(e.target).text()


            if (callerObj != null && callerObj.onMenuEventDelete != undefined && objDt.action == "Delete") {
                callerObj.onMenuEventDelete(objDt);
            }
            else if (callerObj != null && callerObj.onMenuEvent != undefined) {
                callerObj.onMenuEvent(objDt);
            }



            $('.menuClass').hide();
        })


    };

    this.onObj;
    this.on = function (obj) {

        this.onObj = obj;
        if (obj.click != undefined) {
            this.outGrid.onClick.subscribe(function (e, arg) {

                var itm = arg.grid.getDataItem(arg.row);

                obj.click(e, arg, itm);

            })
        }

        if (obj.dblclick != undefined) {
            this.outGrid.onDblClick.subscribe(function (e, arg) {

                var itm = arg.grid.getDataItem(arg.row);
                obj.dblclick(e, arg, itm);

            })
        }

        if (obj.EnterKey != undefined || obj.keydown != undefined) {
            this.outGrid.onKeyDown.subscribe(function (e, arg) {

                var itm = arg.grid.getDataItem(arg.row);

                if (obj.keydown != undefined)
                    obj.keydown(e, arg, itm);

                if (e.keyCode == 13) {
                    if (obj.EnterKey != undefined)
                        obj.EnterKey(e, arg, itm);
                }


            });
        }



        if (obj.cellchange != undefined) {

            this.outGrid.onCellChange.subscribe(function (e, arg) {

                var itm = arg.grid.getDataItem(arg.row);
                obj.cellchange(e, arg, itm);

            })


        }


        if (obj.beforeeditcell != undefined) {

            this.outGrid.onBeforeEditCell.subscribe(function (e, arg) {

                var itm = arg.grid.getDataItem(arg.row);
                if (obj.beforeeditcell(e, arg, itm) == false)
                    return false;

            })


        }

    }


    this.setSave = function (objSet) {

        //var sUrl = objSet.url;
        //var fixedPara = objSet.fixPara;

        if (objSet.addDeleteMenu != undefined) {
            if (objSet.addDeleteMenu == true) {
                thisObj.setContextMenu({

                    menuItems: ["Delete"],
                    onMenuEventDelete: function (objDt) {

                        switch (objDt.action) {
                            case "Delete":

                                if (!confirm("Are you sure you want to delete this entry?"))
                                    return;

                                var arg = {};
                                arg.action = "DELETE";
                                arg.row = objDt.row;
                                arg.cell = objDt.col;
                                thisObj = objDt.thisObj;
                                arg.item = thisObj.outGrid.getDataItem(arg.row)
                                objSet.callerAfterDelete = function (rOj) {

                                    var ro = rOj.arg.row;
                                    thisObj.data.splice(ro, 1);
                                    thisObj.updateRefreshData();
                                    if (thisObj.isTotalGrid)
                                        thisObj.updateAndRefreshTotal();


                                }

                                if (objSet.callBeforeOnDelete != undefined)
                                    objSet.callBeforeOnDelete(arg, objSet)

                                saveData(objSet, arg);

                                break;
                        }

                    }

                })
            }

        }


        this.outGrid.onActiveCellChanged.subscribe(function (e, arg) {

            $('.menuClass').hide();
            if (arg.grid.thisObj.ctrlTypesArray[arg.cell] == "C") {
                if (arg.grid.getDataItem(arg.row) != undefined) {
                    var CellText = arg.grid.getDataItem(arg.row)[arg.grid.thisObj.fields[arg.cell]];

                    var jArray = arg.grid.thisObj.columns[arg.cell].optionsArray
                    var txtttt = arg.grid.thisObj.columns[arg.cell].txtField
                    var jArrObj = $(jArray).filterArray("[" + txtttt + "='" + CellText + "']")

                    if (jArrObj.length > 0) {
                        CellValue = jArrObj[0][arg.grid.thisObj.columns[arg.cell].valField];

                        $('.editor-combo').val(CellValue);
                    }
                    else if ($('.editor-combo').length > 0)
                        $('.editor-combo')[0].selectedIndex = -1;
                }
                else {
                    if ($('.editor-combo').length > 0)
                        $('.editor-combo')[0].selectedIndex = -1;
                }
            }



        })

        this.outGrid.onAddNewRow.subscribe(function (e, arg) {

            arg.row = arg.grid.getActiveCell().row;
            arg.cell = arg.grid.getActiveCell().cell;

            arg.action = "INSERT";
            arg.item["record_added_auto"] = "1";
            arg.item["rowid"] = "";


            if (this.thisObj.onObj != undefined)
                if (this.thisObj.onObj.cellchange != undefined)
                    this.thisObj.onObj.cellchange(e, arg, arg.item)

            if (objSet.callerBefore != undefined)
                objSet.callerBefore(e, arg, objSet)

            saveData(objSet, arg);


        })
        this.outGrid.onCellChange.subscribe(function (e, arg) {
            $('.menuClass').hide();
            if (objSet.callerBefore != undefined)
                objSet.callerBefore(e, arg, objSet)

            arg.action = "UPDATE";
            saveData(objSet, arg);


        })
        this.outGrid.onClick.subscribe(function (e, arg) {

            if ($(e.target).prop("type") == "checkbox") {
                var dp = $(thisObj.outGridName).prop("disabled");
                if (dp != undefined)
                    if (dp == "disabled") {
                        //                                        $(e.target)[0].checked = !($(e.target)[0].checked)
                        //e.stopImmediatePropagation();
                        return false;
                    }
                var item = thisObj.outGrid.getDataItem(arg.row);
                //                                item[thisObj.columns[arg.cell].field]=$(e.target)[0].checked;

                arg.action = "UPDATE";
                arg.item = item;
                if (objSet.callerBefore != undefined)
                    objSet.callerBefore(e, arg, objSet)


                saveData(objSet, arg);

            }

        })

        this.callSave = function (cell, row, itm, callaft) {

            var arg = {};
            arg.cell = cell;
            arg.row = row;
            arg.item = itm;
            arg.action = "UPDATE";

            if (typeof (callaft) == "function")
                objSet.callerAfter = callaft

            saveData(objSet, arg);

        }

        this.callDelete = function (__row) {

            var arg = {};
            arg.action = "DELETE";
            arg.row = __row;
            arg.cell = 0;
            thisObj = this;
            arg.item = thisObj.outGrid.getDataItem(arg.row)
            objSet.callerAfterDelete = function (rOj) {

                var ro = rOj.arg.row;
                thisObj.data.splice(ro, 1);
                thisObj.updateRefreshData();
                if (thisObj.isTotalGrid)
                    thisObj.updateAndRefreshTotal();
            }

            saveData(objSet, arg);


        }

        var saveData = function (objSet, arg) {



            var forPost = false;
            var __slickMethod = "GET";
            var __slickAsync = true
            if (typeof (objSet.method) == "string")
                if (objSet.method == "POST") {
                    forPost = true;
                    __slickMethod = "POST";
                }

            if (typeof (objSet.async) == "boolean")
                if (objSet.async == false) {
                    __slickAsync = false;
                }

            var sUrl = objSet.url
            var sUrlPost = "";

            var sendColIdColumn = false;

            if (typeof (objSet.sendColIdColumn) != "undefined")
                sendColIdColumn = objSet.sendColIdColumn;

            if (sUrl.indexOf("?") < 0)
                sUrl = sUrl + "?"
            else
                sUrl = sUrl + "&"

            if (objSet.fixPara != undefined)
                sUrl = sUrl + objSet.fixPara

            var ro = arg.row;
            var col = arg.cell;
            var ColId = thisObj.fields[col];
            var ColIdText = thisObj.fields[col];


            if (thisObj.getData().length <= 0) {
                return;
            }

            if (typeof (arg.item) == "undefined" && typeof (arg.grid) != "undefined")
                arg.item = arg.grid.thisObj.dataView.getItemByIdx(arg.row)

            var rodata = arg.item; //thisObj.getData()[ro];

            var CellText = rodata[ColId];
            var dataType = thisObj.ctrlTypesArray[col]

            var idColName = thisObj.objectIdProperty;
            var idColVal = rodata[thisObj.objectIdProperty]

            if (rodata["record_added_auto"] != undefined)
                if (rodata["record_added_auto"] != null)
                    if (rodata["record_added_auto"] == "1")
                        idColVal = "";

            var CellValue = "";

            dataType = dataType.split('.')[0];

            if (arg.action != "DELETE")
                switch (dataType) {
                    case "B": dataType = "boolean";

                        if (objSet.fixParaExtra != undefined)
                        { }
                        else
                            objSet.fixParaExtra = "";

                        objSet.fixParaExtra = objSet.fixParaExtra + "&CheckBox=" + CellText

                        break;
                    case "C":
                    case "L":
                    case "LN":

                        var fld = thisObj.columns[col].field;
                        var fldval = thisObj.columns[col].fieldval;


                        CellText = arg.item[fld];
                        CellValue = arg.item[fldval];
                        ColId = fldval;

                        if (fld == fldval) {
                            var jArray = thisObj.columns[col].optionsArray
                            var fltr = "[" + thisObj.columns[col].valField + "='" + CellValue + "']"
                            var jArrObj = $(jArray).filterArray(fltr)
                            if (jArrObj.length > 0) {
                                CellText = jArrObj[0][thisObj.columns[col].txtField];
                                arg.item[ColId] = CellText;
                            }
                        }

                        if (typeof (thisObj.columns[col].sendMapColumn) != 'undefined')
                            if (thisObj.columns[col].sendMapColumn == "1") {

                                ColId = thisObj.columns[col].fieldval

                                if (thisObj.objMapData != undefined && thisObj.objMapData != null)
                                    if (thisObj.objMapData.length > 0) {
                                        var obDt = $(thisObj.objMapData).filterArray("[textColumn='" + ColId + "']");
                                        if (obDt.length > 0)
                                            ColId = obDt[0].srcValueColumn

                                    }
                            }

                        /*
                        var jArray = thisObj.columns[col].optionsArray
                        var colfield = "";
                        var fltr = "["+ thisObj.columns[col].valField +"='"+CellText+"']"
                        var jArrObj = $(jArray).filterArray(fltr)
                                                            
                        CellText = jArrObj[0][thisObj.columns[col].txtField];
                        CellValue = jArrObj[0][thisObj.columns[col].valField];
                        
                        arg.item[ColId] = CellText;
                        
                        
                        ColId =  thisObj.columns[col].fieldval
                        
                        if(thisObj.objMapData!=undefined && thisObj.objMapData!=null)
                            if(thisObj.objMapData.length>0)
                            {
                                var obDt = $(thisObj.objMapData).filterArray("[textColumn='"+ ColId +"']");
                                if(obDt.length>0)
                                    ColId =  obDt[0].srcValueColumn
                                    
                            }
                        
                        */
                        dataType = "string";
                        this.__dataChanged = true;

                        break;
                    case "F": dataType = "number"; break;
                    case "D": dataType = "date"; break;
                    case "D1": dataType = "date"; break;
                    default: dataType = "string"; break;
                }



            if (objSet.isInvalid != undefined && objSet.isInvalid == true) {
                objSet.isInvalid = false;
                return;
            }

            objSet.isInvalid = false;



            if (objSet.fixParaExtra != undefined) {
                sUrl = sUrl + objSet.fixParaExtra
                objSet.fixParaExtra = "";
            }
            if (CellValue == "")
                CellValue = "-1";

            CellText = encodeURIComponent(CellText)
            CellValue = encodeURIComponent(CellValue)

            if (thisObj.columns[col].CellTextDelimiter != undefined) {
                if (thisObj.columns[col].CellTextDelimiter != "") {
                    CellText = thisObj.columns[col].CellTextDelimiter + CellText + thisObj.columns[col].CellTextDelimiter;
                }
            }

            if (forPost) {
                sUrlPost = sUrl;
                sUrl = "type=post";
            }

            sUrl = sUrl + "&ActionType=" + arg.action + "&RowIndex=" + ro + "&ColIndex=" + col + "&" + idColName + "=" + idColVal + "&ColId=" + ColId + "&CellText=" + CellText + "&CellValue=" + CellValue + "&DataType=" + dataType + ""
            if (sUrl.toLowerCase().indexOf("&rowid=") < 0 && sUrl.toLowerCase().indexOf("?rowid=") < 0)
                sUrl = sUrl + "&RowId=" + idColVal;

            if (arg.action.toLowerCase() == "insert")
                sUrl = sUrl + "&InsertCommand=LAST";


            if (objSet.sendCompleteRow != undefined)
                if (objSet.sendCompleteRow == true) {
                    for (kk = 0; kk < thisObj.fields.length; kk++) {
                        if ((ColId != thisObj.fields[kk] && ColIdText != thisObj.fields[kk]) || sendColIdColumn) {
                            var vvl = "";
                            if (rodata[thisObj.fields[kk]] != null)
                                vvl = rodata[thisObj.fields[kk]];
                            if (thisObj.ctrlTypesArray[kk] == "B" && vvl == "")
                                vvl = "false";
                            sUrl = sUrl + "&" + thisObj.fields[kk] + "=" + encodeURIComponent(vvl)

                            //  Sending Value Part of column
                            if (typeof (thisObj.columns[kk].fieldval) != "undefined" && thisObj.columns[kk].fieldval != thisObj.fields[kk]) {
                                var vcol = thisObj.columns[kk].fieldval;

                                vvl = "";
                                if (rodata[vcol] != null)
                                    vvl = rodata[vcol];
                                sUrl = sUrl + "&" + vcol + "=" + encodeURIComponent(vvl)
                            }

                        }
                    }
                }
            if (objSet.sendColumns != undefined) {
                var fl = objSet.sendColumns.split("~");
                for (kk = 0; kk < fl.length; kk++) {
                    if (ColId != fl[kk] || sendColIdColumn) {
                        var vvl = "";
                        if (rodata[fl[kk]] != null)
                            vvl = rodata[fl[kk]];

                        sUrl = sUrl + "&" + fl[kk] + "=" + encodeURIComponent(vvl)
                    }
                }
            }


            var resObj = {};
            resObj.status = "pending";
            resObj.objSet = objSet;
            resObj.arg = arg;






            if (!forPost) {
                sUrlPost = sUrl;
                sUrl = "";

            }


            $.ajax({

                url: sUrlPost,
                method: __slickMethod,
                async: __slickAsync,
                data: sUrl,
                cache: false,
                success: function (res) {

                    if (res.status != "success") {
                        arg.item[arg.grid.thisObj.columns[arg.cell].field] = "";
                        arg.grid.thisObj.updateRefreshDataRow(arg.row);
                        arg.grid.gotoCell(arg.row, arg.cell, true)
                        alert(res.msg);
                        return
                    }
                    else if (res.id != undefined)
                        r = res.id;
                    else if (res.data != undefined && res.data.id != undefined)
                        r = res.data.id;


                    if (arg.action.toLowerCase() == "insert") {
                        rodata[thisObj.objectIdProperty] = r;
                        if (!isNaN(parseInt(r, 10)))
                            if (rodata["record_added_auto"] != undefined || rodata[thisObj.objectIdProperty] == "")
                                if (rodata["record_added_auto"] != null || rodata[thisObj.objectIdProperty] == "")
                                    if (rodata["record_added_auto"] == "1" || rodata[thisObj.objectIdProperty] == "") {
                                        try {
                                            rodata[thisObj.objectIdProperty] = parseInt(r, 10);
                                            rodata["record_added_auto"] = "0";
                                        }
                                        catch (e) { }
                                    }
                        arg.action = "UPDATE";
                        saveData(objSet, arg);

                    }

                    if (!isNaN(parseInt(r, 10)))
                        if (rodata["record_added_auto"] != undefined || rodata[thisObj.objectIdProperty] == "")
                            if (rodata["record_added_auto"] != null || rodata[thisObj.objectIdProperty] == "")
                                if (rodata["record_added_auto"] == "1" || rodata[thisObj.objectIdProperty] == "") {
                                    try {
                                        rodata[thisObj.objectIdProperty] = parseInt(r, 10);
                                        rodata["record_added_auto"] = "0";
                                    }
                                    catch (e) { }
                                }

                    resObj.status = "success";
                    resObj.responseString = res;

                    if (arg.action.toLowerCase() == "delete") {

                        if (objSet.callerAfterDelete != undefined) {

                            objSet.callerAfterDelete(resObj);
                        }
                    }


                },
                error: function (e) {

                    resObj.status = "error";
                    resObj.errorObj = e;

                }


            }).done(function () {


                if (objSet.callerAfter != undefined) {

                    objSet.callerAfter(resObj);
                }

            })





        }

        var saveDataV2 = function (objSet, arg) {
            var forPost = false;
            var __slickMethod = "GET";
            var __slickAsync = true
            if (typeof (objSet.method) == "string")
                if (objSet.method == "POST") {
                    forPost = true;
                    __slickMethod = "POST";
                }

            if (typeof (objSet.async) == "boolean")
                if (objSet.async == false) {
                    __slickAsync = false;
                }

            var sUrl = objSet.url
            var sUrlPost = "";

            var sendColIdColumn = false;

            if (typeof (objSet.sendColIdColumn) != "undefined")
                sendColIdColumn = objSet.sendColIdColumn;

            if (sUrl.indexOf("?") < 0)
                sUrl = sUrl + "?"
            else
                sUrl = sUrl + "&"

            if (objSet.fixPara != undefined)
                sUrl = sUrl + objSet.fixPara

            var ro = arg.row;
            var col = arg.cell;
            var ColId = thisObj.fields[col];
            var ColIdText = thisObj.fields[col];


            if (thisObj.getData().length <= 0) {
                return;
            }

            if (typeof (arg.item) == "undefined" && typeof (arg.grid) != "undefined")
                arg.item = arg.grid.thisObj.dataView.getItemByIdx(arg.row)

            var rodata = arg.item; //thisObj.getData()[ro];

            var CellText = rodata[ColId];
            var dataType = thisObj.ctrlTypesArray[col]

            var idColName = thisObj.objectIdProperty;
            var idColVal = rodata[thisObj.objectIdProperty]

            if (rodata["record_added_auto"] != undefined)
                if (rodata["record_added_auto"] != null)
                    if (rodata["record_added_auto"] == "1")
                        idColVal = "";

            var CellValue = "";

            dataType = dataType.split('.')[0];

            if (arg.action != "DELETE")
                switch (dataType) {
                    case "B": dataType = "boolean";

                        if (objSet.fixParaExtra != undefined)
                        { }
                        else
                            objSet.fixParaExtra = "";

                        objSet.fixParaExtra = objSet.fixParaExtra + "&CheckBox=" + CellText

                        break;
                    case "C":
                    case "L":
                    case "LN":

                        var fld = thisObj.columns[col].field;
                        var fldval = thisObj.columns[col].fieldval;


                        CellText = arg.item[fld];
                        CellValue = arg.item[fldval];
                        ColId = fldval;

                        if (fld == fldval) {
                            var jArray = thisObj.columns[col].optionsArray
                            var fltr = "[" + thisObj.columns[col].valField + "='" + CellValue + "']"
                            var jArrObj = $(jArray).filterArray(fltr)
                            if (jArrObj.length > 0) {
                                CellText = jArrObj[0][thisObj.columns[col].txtField];
                                arg.item[ColId] = CellText;
                            }
                        }

                        if (typeof (thisObj.columns[col].sendMapColumn) != 'undefined')
                            if (thisObj.columns[col].sendMapColumn == "1") {

                                ColId = thisObj.columns[col].fieldval

                                if (thisObj.objMapData != undefined && thisObj.objMapData != null)
                                    if (thisObj.objMapData.length > 0) {
                                        var obDt = $(thisObj.objMapData).filterArray("[textColumn='" + ColId + "']");
                                        if (obDt.length > 0)
                                            ColId = obDt[0].srcValueColumn

                                    }
                            }
                        dataType = "string";
                        this.__dataChanged = true;

                        break;
                    case "F": dataType = "number"; break;
                    case "D": dataType = "date"; break;
                    case "D1": dataType = "date"; break;
                    default: dataType = "string"; break;
                }



            if (objSet.isInvalid != undefined && objSet.isInvalid == true) {
                objSet.isInvalid = false;
                return;
            }

            objSet.isInvalid = false;



            if (objSet.fixParaExtra != undefined) {
                sUrl = sUrl + objSet.fixParaExtra
                objSet.fixParaExtra = "";
            }
            if (CellValue == "")
                CellValue = "-1";

            CellText = encodeURIComponent(CellText)
            CellValue = encodeURIComponent(CellValue)

            if (thisObj.columns[col].CellTextDelimiter != undefined) {
                if (thisObj.columns[col].CellTextDelimiter != "") {
                    CellText = thisObj.columns[col].CellTextDelimiter + CellText + thisObj.columns[col].CellTextDelimiter;
                }
            }

            if (forPost) {
                sUrlPost = sUrl;
                sUrl = "type=post";
            }

            var PostData = {
                "ActionType": arg.action,
                "RowIndex": ro,
                "ColIndex": col,
                "ColId": ColId,
                "CellText": CellText,
                "CellValue": CellValue,
                "DataType": dataType
            };

            PostData[idColName] = idColVal;
            sUrl = sUrl + "&ActionType=" + arg.action + "&RowIndex=" + ro + "&ColIndex=" + col + "&" + idColName + "=" + idColVal + "&ColId=" + ColId + "&CellText=" + CellText + "&CellValue=" + CellValue + "&DataType=" + dataType + ""
            if (sUrl.toLowerCase().indexOf("&rowid=") < 0 && sUrl.toLowerCase().indexOf("?rowid=") < 0) {
                sUrl = sUrl + "&RowId=" + idColVal;
                PostData["RowId"] = idColVal;
            }

            if (arg.action.toLowerCase() == "insert") {
                sUrl = sUrl + "&InsertCommand=LAST";
                PostData["InsertCommand"] = "LAST";
            }


            if (objSet.sendCompleteRow != undefined)
                if (objSet.sendCompleteRow == true) {
                    for (kk = 0; kk < thisObj.fields.length; kk++) {
                        if ((ColId != thisObj.fields[kk] && ColIdText != thisObj.fields[kk]) || sendColIdColumn) {
                            var vvl = "";
                            if (rodata[thisObj.fields[kk]] != null)
                                vvl = rodata[thisObj.fields[kk]];
                            if (thisObj.ctrlTypesArray[kk] == "B" && vvl == "")
                                vvl = "false";
                            sUrl = sUrl + "&" + thisObj.fields[kk] + "=" + encodeURIComponent(vvl)

                            //  Sending Value Part of column
                            if (typeof (thisObj.columns[kk].fieldval) != "undefined" && thisObj.columns[kk].fieldval != thisObj.fields[kk]) {
                                var vcol = thisObj.columns[kk].fieldval;

                                vvl = "";
                                if (rodata[vcol] != null)
                                    vvl = rodata[vcol];
                                sUrl = sUrl + "&" + vcol + "=" + encodeURIComponent(vvl)
                            }

                        }
                    }
                }
            if (objSet.sendColumns != undefined) {
                var fl = objSet.sendColumns.split("~");
                for (kk = 0; kk < fl.length; kk++) {
                    if (ColId != fl[kk] || sendColIdColumn) {
                        var vvl = "";
                        if (rodata[fl[kk]] != null)
                            vvl = rodata[fl[kk]];

                        sUrl = sUrl + "&" + fl[kk] + "=" + encodeURIComponent(vvl)
                    }
                }
            }


            var resObj = {};
            resObj.status = "pending";
            resObj.objSet = objSet;
            resObj.arg = arg;

            if (!forPost) {
                sUrlPost = sUrl;
                sUrl = "";

            }


            $.ajax({

                url: sUrlPost,
                method: __slickMethod,
                async: __slickAsync,
                data: sUrl,
                cache: false,
                success: function (res) {

                    if (res.status != "success") {
                        arg.item[arg.grid.thisObj.columns[arg.cell].field] = "";
                        arg.grid.thisObj.updateRefreshDataRow(arg.row);
                        arg.grid.gotoCell(arg.row, arg.cell, true)
                        alert(res.msg);
                        return
                    }
                    else if (res.id != undefined)
                        r = res.id;
                    else if (res.data != undefined && res.data.id != undefined)
                        r = res.data.id;

                    if (arg.action.toLowerCase() == "insert") {
                        rodata[thisObj.objectIdProperty] = r;
                        if (!isNaN(parseInt(r, 10)))
                            if (rodata["record_added_auto"] != undefined || rodata[thisObj.objectIdProperty] == "")
                                if (rodata["record_added_auto"] != null || rodata[thisObj.objectIdProperty] == "")
                                    if (rodata["record_added_auto"] == "1" || rodata[thisObj.objectIdProperty] == "") {
                                        try {
                                            rodata[thisObj.objectIdProperty] = parseInt(r, 10);
                                            rodata["record_added_auto"] = "0";
                                        }
                                        catch (e) { }
                                    }
                        arg.action = "UPDATE";
                        saveData(objSet, arg);

                    }

                    if (!isNaN(parseInt(r, 10)))
                        if (rodata["record_added_auto"] != undefined || rodata[thisObj.objectIdProperty] == "")
                            if (rodata["record_added_auto"] != null || rodata[thisObj.objectIdProperty] == "")
                                if (rodata["record_added_auto"] == "1" || rodata[thisObj.objectIdProperty] == "") {
                                    try {
                                        rodata[thisObj.objectIdProperty] = parseInt(r, 10);
                                        rodata["record_added_auto"] = "0";
                                    }
                                    catch (e) { }
                                }

                    resObj.status = "success";
                    resObj.responseString = resStr;

                    if (arg.action.toLowerCase() == "delete") {

                        if (objSet.callerAfterDelete != undefined) {

                            objSet.callerAfterDelete(resObj);
                        }
                    }


                },
                error: function (e) {

                    resObj.status = "error";
                    resObj.errorObj = e;

                }


            }).done(function () {


                if (objSet.callerAfter != undefined) {

                    objSet.callerAfter(resObj);
                }

            })





        }



    };

    this.onCheckBoxClick = function (callerFunction) {

        $(':checkbox', this.outGridName).on("click", function () {



            callerFunction(this);

        })

        this.outGrid.onScroll.subscribe(function (e, arg) {
            $(':checkbox', this.outGridName).off("click")
            $(':checkbox', this.outGridName).on("click", function () {



                callerFunction(this);

            })
        })

    }

    this.setReadOnly = function (bl) {


        if (typeof (bl) != "undefined")
            this.isReadOnly = bl;
        else
            this.isReadOnly = true;

        if (this.isReadOnly) {
            $(this.outGridName).prop("disabled", "disabled")
            $(this.outGridName).addClass("__gridDisable")
        }
        else {
            $(this.outGridName).prop("disabled", "")
            $(this.outGridName).removeClass("__gridDisable")
        }


    }

    this.enableCopy = function () {

        this.outGrid.onKeyDown.subscribe(function (e, arg) {

            if (e.ctrlKey) {
                if (e.keyCode == 67) {

                    ranges = arg.grid.getSelectionModel().getSelectedRanges();
                    _grid = arg.grid
                    {
                        var columns = _grid.getColumns();
                        var hash = {};
                        var copyString = "";
                        for (var i = 0; i < ranges.length; i++) {
                            for (var j = ranges[i].fromRow; j <= ranges[i].toRow; j++) {
                                var itm = _grid.getData().getItem(j);
                                hash[j] = {};
                                for (var k = ranges[i].fromCell; k <= ranges[i].toCell; k++) {
                                    hash[j][columns[k].id] = "copied";
                                    copyString = copyString + "" + itm[columns[k].id] + "\t"
                                }
                                copyString = copyString.substring(0, copyString.length - 1)
                                copyString = copyString + "\r\n";
                            }
                            copyString = copyString.substring(0, copyString.length - 2)
                        }
                        //alert(copyString );
                        //_grid.setCellCssStyles("copy-manager", hash);
                        $('body').append("<textarea id=\"txt_to_copy\"></textarea>")
                        $('#txt_to_copy').text(copyString)
                        $('#txt_to_copy').select();
                        document.execCommand("copy");
                        $('#txt_to_copy').remove();
                        //$('#txt_to_copy').text(copyString)

                    }

                }
            }


        });

    }

    this.onButtonClick = function (callerFunction) {

        $(':button', this.outGridName).on("click", function () {



            callerFunction(this);

        })

        this.outGrid.onScroll.subscribe(function (e, arg) {
            $(':button', this.outGridName).off("click")
            $(':button', this.outGridName).on("click", function () {



                callerFunction(this);

            })
        })

    }
    this.defaultHeight;

    this.SlickSetHeading = function (MergeH) {
        var GrDName = this.outGridName.replace('#', '');
        var sResizable = "";


        if (this.defaultHeight != null && this.defaultHeight != undefined)
            $('#' + GrDName + '').css("height", this.defaultHeight)

        var divwidth = new Array();
        var i = 0;
        var HeadingDivStyle = $('#' + GrDName + ' .slick-header-columns').attr('style');
        $('#' + GrDName + ' .slick-header-columns div').each(function () {
            var id = $(this).attr('id');
            if (id != undefined) {
                divwidth[i] = $(this).css('width').replace('px', '');
                i = i + 1;
            }
        });

        for (j = 0; j < MergeH.length; j = j + 1) {
            var HArray = MergeH[j].split('~');
            var H = '<div class="slick-header-columns" style="' + HeadingDivStyle + '" unselectable="on">';
            var MergeColLen = 0;
            var OldHdr = "";
            sResizable = "";
            for (k = 0; k < HArray.length; k = k + 1) {
                sResizable = sResizable + "false~";
                var n = k + 1;
                //if(HArray[k] == '') 
                //    H= H+'<div title="" class="ui-state-default slick-header-column HD_'+String(j)+ '" id="H_'+String(k)+'" style="width: '+String(divwidth[k])+'px;"><span class="slick-column-name">'+HArray[k]+'</span></div>'
                //else
                //{
                var same = false;
                if (k < HArray.length - 1) {
                    if (HArray[k] == HArray[n] || HArray[n] == '') {
                        if (OldHdr == "")
                            OldHdr = HArray[k];
                        same = true;
                    }
                }

                if (HArray[k] == '' && OldHdr != '')
                    HArray[k] = OldHdr;

                MergeColLen = MergeColLen + eval(divwidth[k]);
                if (!same) {
                    H = H + '<div title="" class="ui-state-default slick-header-column HD_' + String(j) + '" id="H_' + String(k) + '" style="width: ' + String(MergeColLen) + 'px;"><span class="slick-column-name">' + HArray[k] + '</span></div>'
                    MergeColLen = 0;
                    OldHdr = "";
                }
                //}   
                if (j == 0)
                    this.outGrid.getColumns(0)[k].resizable = false;

            }
            H = H + '</div>';
            $('#' + GrDName + ' .slick-header').prepend(H);

            var h1 = $('.HD_' + String(j) + '').css("height");
            var h2 = $('#' + GrDName + '').css("height");

            h1 = parseInt(h1, 10)
            h2 = parseInt(h2, 10)

            var h3 = h1 + h2;

            $('#' + GrDName + '').css("height", h3 + "px");



        }
    }


}

