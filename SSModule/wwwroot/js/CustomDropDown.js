/**
 *  @author:    Sumit Yadav
 *  @created:   MAr 2024
 *  @description: Commonly used jQuery functions for User services
 *  Last Updated: Sumit Yadav, Mar 02 2024
 **/

var arrHideColumns = ['DateCreated', 'DATE_CREATED', 'DATE_MODIFIED', 'DateModified', 'UserName', 'FKUserId', 'src'];
var DateColumns = [];
let $dropdown = $("#dvCommonCusDropV2List");
let _Custdropdown = [];
const uriList = {
    Branch: "",
    user: "/Master/User/CustomList",
};
$(document).ready(function () {

    GlobleDropDownBind();
    $("body").click(function (event) {
        if (event.target.id != "dvCommonCusDropV2List") {
            if ($dropdown.length > 0) {
                $dropdown.hide().html('');
            }
        }
    });
});

function GlobleDropDownBind(Container) {
    $dropdown = $("#dvCommonCusDropV2List");
    if (Container) {
        $(Container + " .ui-custom-DropDown").each(function () {
            var hid = $(this).attr("id").replace("drpList", "");
            _Custdropdown[hid] = new fnCustomDropDown(hid);
        });
    }
    else {
        $(".ui-custom-DropDown").each(function () {
            var hid = $(this).attr("id").replace("drpList", "");
            if (_Custdropdown[hid] == undefined)
                _Custdropdown[hid] = new fnCustomDropDown(hid);
        });
    }

}

function DropDownReset(ids) {
    $("#" + ids).val("");
    $("#drp" + ids).val("");
}

function DropDownSet(ids, text, value) {
    $("#" + ids).val(value);
    $("#" + ids).attr({ "oldvalue": value, "oldtext": text });

    if (value === null || value === undefined || value.toString().trim() === "") {
        $("#" + ids).val("");
        $("#drp" + ids).val(text);
    }
    else {
        $("#drp" + ids).val(text);
    }
}


function fnCustomDropDown(hid) {
    let $Scope = this;
    let FieldName = hid;
    let Container = "drpList" + hid;
    let $Container = $("#drpList" + hid);
    let $input = $Container.find('input[type="text"]');
    let $span = $Container.find('span');
    let PageNo = 1;
    var stopScrollLoading = false; // Flag to stop further data loading
    let Keyval = "";
    let KeyText = "";
    let columns = [];
    this.onload = null;
    this.onSelect = null;

    var Init = () => {
        bindEvents();
    }


    let debounceTimer;
    var bindEvents = () => {
        $input.off("keyup").on("keyup", function (e) {
            const key = e.keyCode || e.which;

            if (key === 40) {
                if ($dropdown.is(":visible")) {
                    // Down arrow key
                    const firstRow = $dropdown.find("table tbody tr:first");
                    if (firstRow.length) {
                        firstRow.focus();
                        firstRow.addClass("focused-row"); // Add styling for focus (optional)
                    }
                }
                else {
                    // User types something
                    Showdrop();
                }
            }
            else if (key === 38) { // Up key
                if ($dropdown.is(":visible")) {
                    Hidedrop();
                }
            }
            else if (key === 9) { // Tab key
                Hidedrop();
            }
            else if (key != 13 && key != 27) { // Enter key 
                // User types something
                clearTimeout(debounceTimer);
                debounceTimer = setTimeout(() => {
                    Showdrop();
                }, 300);
            }
        });

        $input.off("keydown").on("keydown", function (e) {

            const key = e.keyCode || e.which;
            if (key === 9) { // Tab key
                Hidedrop();
            }
        });

        $input.off("focusout").on("focusout", function () {

            var item = $(this);
            if (!item.hasClass('drpEditable')) {
                if ($('#' + FieldName).val() === '') {
                    item.val('');
                }
            }
        });

        $input.off("focusin").on("focusin", function () {

            if ($(this).val() !== '') {
                $(this).select();
            }
        });

        $span.off("click").on("click", function (e) {
            if ($input.is(":disabled")) return;
            // Toggle the visibility of #dvCommonCusDropV2List
            $dropdown.toggle();

            // If #dvCommonCusDropV2List is visible, fetch data
            if ($dropdown.is(":visible")) {
                Showdrop();
            } else {
                // Optionally, you can hide the dropdown contents when not visible
                $dropdown.html('');
            }
            $input.focus();

            e.stopPropagation();
        });


    }

    var Showdrop = () => {
        if ($input.is(":disabled")) return;
        PageNo = 1; // Reset PageNo
        stopScrollLoading = false;
        get(); // Fetch search results
    }
    var Hidedrop = () => {
        // Hide the dropdown
        // Prevent scrolling
        $dropdown.hide().html('');
    }
    var get = function () {
        if (PageNo == 1) {
            calculateDropdownPosition();
        }
        let data = {
            name: FieldName,
            pageno: PageNo,
            pagesize: 20,
            search: $input.val().trim(),
            param: ''
        };
        let hidExtra = $Container.attr("ExtraParam");
        if (hidExtra !== '') {
            let ExtraParam = '';
            var arrExtra = hidExtra.split(',');
            $(arrExtra).each(function (index, item) {
                ExtraParam += $('#' + item).val() + '~';
            });

            data["ExtraParam"] = ExtraParam.slice(0, -1);
        }


        let hiParent = $Container.attr("Parent");
        if (hiParent) {
            var arrParentId = hiParent.split(',');
            $(arrParentId).each(function (index, item) {
                if (item !== "" && item.length > 3) {
                    data[item] = $("#" + item).val();
                }
            });
        }

        var result = "";
        if (typeof ($Scope.onload) == "function") {
            result = $Scope.onload(data);
        }
        else {
            result = fetchDropdownData(data);
        }
        renderDropdown(result)

    }

    var fetchDropdownData = function (data) {
        var result = "";
        var url = Handler.currentUrl + '/' + FieldName;
        if ($Container.attr("uri"))
            url = uriList[$Container.attr("uri")];
        $.ajax({
            url: url,
            data: data,
            method: 'POST',
            dataType: 'JSON',
            async: false,
            success: function (res) {
                result = res;
            },
            error: function () {
                $dropdown.html("<span class='NoRecord'>Failed to load data. Please try again.</span>");
            }
        });
        return result;
    };

    var calculateDropdownPosition = () => {
        const inputOffset = $input.offset();
        const inputHeight = $input.outerHeight();
        const dropdownHeight = 200; // Set max height for the dropdown
        const viewportHeight = $(window).height(); // Get the height of the visible part of the window
        const scrollTop = $(window).scrollTop(); // Get the current scroll position
        const spaceBelow = viewportHeight - (inputOffset.top - scrollTop + inputHeight); // Space below the input
        const spaceAbove = inputOffset.top - scrollTop; // Space above the input

        // Determine dropdown position: below or above
        let dropdownTop;
        if (spaceBelow >= dropdownHeight || spaceBelow >= spaceAbove) {
            // Enough space below, show dropdown below input
            dropdownTop = inputOffset.top + inputHeight;
        } else {
            // Not enough space below, show dropdown above input
            dropdownTop = inputOffset.top - dropdownHeight;
        }
        const dropdownWidth = $input.outerWidth() > 350 ? $input.outerWidth() : 350;

        // Set dropdown position
        $dropdown.css({
            top: dropdownTop,
            left: inputOffset.left,
            width: dropdownWidth,
            position: "absolute",
            display: "block",
            height: `${dropdownHeight}px`,
            overflowY: "auto",
            zIndex: 1000 // Ensure it's above other elements
        });

        $dropdown.off("scroll").on("scroll", function () {

            if (stopScrollLoading) return; // Stop further data fetching if no more data

            let scrollTop = $(this).scrollTop();
            let scrollHeight = $(this)[0].scrollHeight;
            let clientHeight = $(this)[0].clientHeight;

            // Check if scrolled to the bottom
            if (scrollTop + clientHeight >= scrollHeight - 100) {
                PageNo++;
                get(); // Fetch data for the next page
            }
        });
    }

    var renderDropdown = function (data) {
        // Check if the data is empty or null
        if (!data || data.length === 0) {
            if (PageNo === 1) {
                $dropdown.html("<span class='NoRecord'>Data Not Found</span>");
            }
            stopScrollLoading = true; // Stop further scroll-based data fetching
            return;
        }

        let html = "";

        // If PageNo is 1, create table headers and a new table
        if (PageNo === 1) {
            html += "<table class='custom-dropdown-table' ><thead><tr>";
            columns = Object.keys(data[0]);

            // Add table headers, excluding hidden columns
            columns.forEach((col, index) => {
                if (index == 0)
                    Keyval = col;
                if (index > 0 && arrHideColumns.indexOf(col) === -1 && !col.toLowerCase().startsWith("fk")) {
                    let style = index === 1 && col.length <= 2 ? "style='max-width:300px;'" : "";
                    var colhead = col.replace(/([a-z])([A-Z])/g, '$1 $2');
                    html += `<th ${style}>${colhead}</th>`;
                }
            });
            html += "</tr></thead><tbody>";
        }

        // Generate table rows
        data.forEach((item) => {
            html += "<tr tabindex='-1' hid='" + item[Keyval] + "' >";
            let columns = Object.keys(item);

            columns.forEach((col, index) => {
                if (index > 0 && arrHideColumns.indexOf(col) === -1 && !col.toLowerCase().startsWith("fk")) {
                    let cellData = item[col] !== null ? item[col] : "&nbsp;";
                    if (DateColumns.indexOf(col.toLowerCase()) != -1) {
                        cellData = Handler.formatServerDate(item[col]) == null ? "&nbsp;" : Handler.formatServerDate(item[col]);
                    }
                    let style = index === 1 && col.length <= 2 ? "style='max-width:300px;'" : "";
                    html += `<td ${style}>${cellData}</td>`;
                }
            });

            html += "</tr>";
        });

        // Show the dropdown
        $dropdown.show();
        // If PageNo is 1, close the table HTML
        if (PageNo === 1) {
            html += "</tbody></table>";
            $dropdown.html(html); // Bind the new table for Page 1   

        } else {
            // Append rows to the existing table for subsequent pages
            $("#dvCommonCusDropV2List table tbody").append(html);
        }

        $dropdown.off("click", "table tbody tr").on("click", "table tbody tr", function (e) {
            const $currentRow = $(this);
            // Hide the dropdown
            Hidedrop();

            setData($currentRow);
        });
        $dropdown.off("keydown", "table tbody tr").on("keydown", "table tbody tr", function (e) {
            e.preventDefault();
            const key = e.keyCode || e.which;
            const $currentRow = $(this);

            if (key === 40) { // Down arrow key
                const $nextRow = $currentRow.next("tr");
                if ($nextRow.length) {
                    $currentRow.removeClass("focused-row");
                    $nextRow.addClass("focused-row").focus();
                }
                e.preventDefault(); // Prevent scrolling
            } else if (key === 38) { // Up arrow key
                const $prevRow = $currentRow.prev("tr");
                if ($prevRow.length) {
                    $currentRow.removeClass("focused-row");
                    $prevRow.addClass("focused-row").focus();
                } else {
                    // Focus back on $input if it's the first row
                    $currentRow.removeClass("focused-row");
                    $input.focus();
                }
                e.preventDefault(); // Prevent scrolling
            } else if (key === 27) { // Up arrow key
                Hidedrop();
                $input.focus();
                e.preventDefault(); // Prevent scrolling
            }
            else if (key === 13) { // Enter key
                setData($currentRow);
                Hidedrop();
            }

        });

        // Add tabindex to each row so they are focusable
        $dropdown.off("mouseenter", "table tbody tr").on("mouseenter", "table tbody tr", function () {
            $(this).attr("tabindex", "-1"); // Ensure rows are focusable
        });


    };
    //===============================================

    var setData = function ($currentRow) {
        _data = {};
        var drpValue = $currentRow.attr("hid");
        var drpText = $currentRow.find("td").eq(0).text();
        $input.val(drpText);
        $("#" + FieldName).val(drpValue);
        $currentRow.children("td").each(function (index) {
            _data[columns[index + 1]] = $(this).text();
        })

        let event = $Container.attr("event");
        if (event !== '') {
            eval(event);
        }
        else {
            var arg = {
                value: drpValue,
                text: drpText,
                data: _data
            }
            if (typeof ($Scope.onSelect) == "function")
                $Scope.onSelect(arg);
        }

        $dropdown.html('').hide();
        $input.focus();

    }

    Init();
}

