/*
*sort Button Attr  =  sort-expression;
*
*
*
*/

function DataTable(divId) {
    /*
    ***Properties***
    */
    this.isRunning = false;
    this.Paging = 1;
    this.currentPageID = 1;
    this.pageSize = 20;
    this.sortExpression = '';
    this.sortDirection = 1;
    this.accessUrl = '';

    this.addUrl = '';
    this.deleteUrl = '';
    this.deletebyClassName = 'icon-trash';
    this.disableUrl = '';
    this.gridDivId = divId;
    this.searchBoxClassName = 'trsearch';
    this.searchBoxInnerTable = true;
    this.searchButtonClassName = 'search-button';
    this.searchButtonInnerTable = true;
    this.searchParameters = [];
    this.sortButtonClassName = 'sorting';
    this.sortButtonAsc = 'sorting_asc';
    this.sortButtonDesc = 'sorting_desc';
    this.selectPageClassName = "selectPageSize";
    this.selectPagingClassName = "pageRequest";
    //    this.otherParameters = [];
    //this.maxDays = 30;

    /*
    ***END Properties***
    */

}

DataTable.prototype.bindEvents = function () {
    /*
    ***Add Events***
    */
    if (this.selectPageClassName != '')
        bindTableEvent('change', '#' + this.gridDivId + ' .' + this.selectPageClassName, 'selectPageSizeEvent', this);

    if (this.selectPagingClassName != '')
        bindTableEvent('click', '#' + this.gridDivId + ' .' + this.selectPagingClassName, 'selectPageEvent', this);

    if (this.searchBoxClassName != '')
        if (this.searchButtonInnerTable)
            bindTableEvent('click', '#' + this.gridDivId + ' .' + this.searchButtonClassName, 'search-button', this);
        else
            bindTableEvent('click', '.' + this.searchButtonClassName, 'search-button', this);

    if (this.sortButtonClassName != '')
        bindTableEvent('click', '#' + this.gridDivId + ' .' + this.sortButtonClassName, 'sort-row', this);
    if (this.deleteUrl != '' && this.deletebyClassName != '')
        bindTableEvent('click', '#' + this.gridDivId + ' .' + this.deletebyClassName, 'remove-row', this);

    /*
    /*
    ***END Add Events***
    */

}

DataTable.prototype.asynList = function () {
    var jsonParameters = { "Paging": this.Paging, "CurrentPageID": this.currentPageID, "PageSize": this.pageSize, "SortingColumn": this.sortExpression, "SortingOrder": this.sortDirection, "SearchParameter": this.searchParameters };
    var obj = this;
    //NProgress.configure({
    //    parent: '#' + obj.gridDivId
    //});
    //NProgress.start();
    obj.isRunning = true;

    $('.loading').show();
    $.ajax({
        type: 'POST',
        //contentType: 'application/json',
        data: { "TablePaging": jsonParameters },//JSON.stringify({ TablePaging: jsonParameters }),
        url: this.accessUrl,
        success: function (result) {
            obj.isRunning = false;
            $('.loading').hide();
            //NProgress.done();
            $('#' + obj.gridDivId).html(result);

            $('.' + obj.sortButtonClassName).removeClass(obj.sortButtonAsc);
            $('.' + obj.sortButtonClassName).removeClass(obj.sortButtonDesc);
            var sortButtonClassName = '.' + obj.sortButtonClassName;
            $(sortButtonClassName).each(function () {
                if (typeof $(this).attr('sort-expression') !== "undefined") {
                    if ($(this).attr('sort-expression').toLowerCase() == obj.sortExpression.toLowerCase()) {
                        if (obj.sortDirection == 1) {
                            $(this).addClass(obj.sortButtonAsc);
                        }
                        else {
                            $(this).addClass(obj.sortButtonDesc);
                        }
                    }
                }
            });
            $(obj.searchParameters).each(function (index, data) {

                var searchFilterElements = "#" + obj.gridDivId + " #" + data.Key;

                $(searchFilterElements).val(data.Value);
            });
            if (obj.selectPageClassName != '') {
                var controlelementid = '#' + obj.gridDivId + ' .' + obj.selectPageClassName;
                $(controlelementid).val(obj.pageSize);
            }
        },
        error: function (request, status, error) {
            obj.isRunning = false;
            //NProgress.done();
            $('.loading').hide();
            // look for status of 401 and redirect to login
            if (status == 403 || status == 401) {
                window.location.href = "/login";
            }
        }
    });
}


DataTable.prototype.pageRequest = function (pageid) {
    this.currentPageID = parseInt(pageid);
    this.asynList();
}

DataTable.prototype.searchRequest = function () {

    this.searchParameters.splice(0, this.searchParameters.length);
    var obj = this;
    var searchFilterElements;

    if (obj.searchBoxInnerTable)
        searchFilterElements = "#" + this.gridDivId + " ." + this.searchBoxClassName;
    else
        searchFilterElements = "." + this.searchBoxClassName;


    $(searchFilterElements).find('input:text').each(function () {
        if ($(this).val() != '' && $(this).val() != null) {
            var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
            obj.searchParameters.push(parameter);
        }
    });
    $(searchFilterElements).find('input:checkbox').each(function () {
        var parameter = { Key: $(this).attr('name'), Value: $(this).is(':checked') }
        obj.searchParameters.push(parameter);
    });
    $(searchFilterElements).find('select').each(function () {
        if ($(this)[0].selectedIndex != 0) {
            var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
            obj.searchParameters.push(parameter);
        }
    });

    //    $(_otherParameters).each(function () {
    //        if ($('#' + this).val() != '' && $('#' + this).val() != null) {
    //            var parameter = { Key: $('#' + this).attr('name'), Value: $('#' + this).val() }
    //            this.searchParameters.push(parameter);
    //        }
    //    });
    this.currentPageID = 1;
    this.asynList();
}

DataTable.prototype.sortRequest = function (sortColumn) {
    if (this.sortExpression == sortColumn) {
        if (this.sortDirection == 1) {
            this.sortDirection = 2;
        }
        else {
            this.sortDirection = 1;
        }
    }
    else {
        this.sortDirection = 1;
    }
    this.currentPageID = 1;
    this.sortExpression = sortColumn;
    this.asynList();
}

bindTableEvent = function (eventName, control, condition, obj) {
    switch (condition) {
        case 'selectPageSizeEvent':
            $(document).on(eventName, control, function () {
                var _pageSizeValue = parseInt(this.value);
                obj.pageSize = _pageSizeValue;
                obj.currentPageID = 1;
                obj.asynList();
            });
            break;
        case 'selectPageEvent':
            $(document).on(eventName, control, function () {
                obj.pageRequest($(this).attr('pageRequest'));
            });
            break;
        case 'search-button':
            $(document).on(eventName, control, function () {
                obj.searchRequest();
            });
            break;
        case 'sort-row':
            $(document).on(eventName, control, function () {
                obj.sortRequest($(this).attr('sort-expression'));
            });
            break;
        case 'remove-row':
            $(document).on(eventName, control, function () {
                return obj.deleteRequest($(this).attr('datatoken'));
            });
            break;
    }
};

DataTable.prototype.deleteRequest = function (removeId) {
    var r = confirm("Are you sure you want to delete selected row?")
    var obj = this;
    if (r == true) {
        window.location = this.deleteUrl + '/' + removeId;
    }
    else {
        return false;
    }
}

DataTable.prototype.deleteRequest = function (removeId) {
    var r = confirm("Are you sure you want to delete selected row?")
    var obj = this;
    if (r == true) {
        window.location = this.deleteUrl + '/' + removeId;
    }
    else {
        return false;
    }
}

DirectDeleteRequest = function (Url) {
    debugger
    var r = confirm("Are you sure you want to delete selected row?")
    var obj = this;
    if (r == true) {
        window.location = Url;
    }
    else {
        return false;
    }
}




//DirectdeleterequestwithReason = function (url, Id) {
//    debugger
//    var Reason = prompt("Please Enter Reason", "");

//    if (Reason != null && Reason != "") {
//        Id = Id ;
//        window.location = url + Id;
//        alert("Data Deleted ");
//    }
//    else {
//        var r = alert("sorry without resion case not delete");
//    }
//}