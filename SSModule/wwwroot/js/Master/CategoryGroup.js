
$(document).ready(function () {

    Common.InputFormat();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
})
function GenerateAlias() {
    debugger;
    if ($("#Alias").val() == "") {
        $.ajax({
            type: "POST",
            url: '/Master/Location/GetAlias',
            data: {},
            datatype: "json",
            success: function (res) {
                if (res != "") {
                    $("#Alias").val(res);
                }
            }
        });
    }

}

function AddUser() {
    var foundL = false;
    if ($("#drpFkUserId").val().replace('"', '').replace('"', '') == "") {
        Alertdialog("Please Select User");
        return false;
    }
    else {
        $('#tblUserLoc tbody tr').each(function (index) {
            var val = $('#UserLoclnk_' + index + '__FKUserID').val();
            var vald = $("#UserLoclnk_" + index + "__ModeForm").val();
            if (val == $("#FkUserId").val()) {
                foundL = true;
                if (vald == '2') {
                    RowAction($("#UserLoclnk_" + index + "__ModeForm").next(), index, 'undo');
                };
            }
        });

        if (foundL == false) {
            var i = $('#tblUserLoc tbody tr').length;
            var html = `<tr index="${i}">
                        <td>
                            <input id="UserLoclnk_${i}__UserName" name="UserLoclnk[${i}].UserName" type="text" value="${$("#drpFkUserId").val().replace('"', '').replace('"', '')}" tabindex="-1" readonly="readonly" /> 
                            <input id="UserLoclnk_${i}__FKUserID" name="UserLoclnk[${i}].FKUserID" type="hidden" value="${$("#FkUserId").val()}" /> 
                        
                        </td>
                        <td>
                            <input id="UserLoclnk_${i}__ModeForm" name="UserLoclnk[${i}].ModeForm" type="hidden" value="0" /> 
                            <button type="button" class="btn grid-close-btn" onclick="RowAction(this,${i},\'del\')">
                                <i class="bi bi-x-circle"></i>
                            </button>
                        </td>
                        </tr>`;
            $("#tblUserLoc tbody").append(html);
        }
    }

    DropDownReset('FkUserId');
    return false;
}

function RowAction(obj, index, action) {
    if (action == 'del') {
        $(obj).attr('onclick', "RowAction(this,${i},\'undo\')");
        $(obj).html('<i class="bi bi-arrow-counterclockwise"></i>');
        $(obj).closest('tr').addClass('tbl-delete');
        $(obj).prev("input[id$='__ModeForm']").val('2');
        $(obj).closest('tr').find(".action-icon").css("pointer-events", "none");
    }
    else if (action == 'undo') {
        $(obj).attr('onclick', "RowAction(this,${i},'del')")
        $(obj).html('<i class="bi bi-x-circle"></i>');
        $(obj).closest('tr').removeClass('tbl-delete');
        $(obj).prev("input[id$='__ModeForm']").val('0');
        $(obj).closest('tr').find(".action-icon").css("pointer-events", "all");
    }
}