
$(document).ready(function () {

    Common.InputFormat();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
})

function AddUser() {

    var rowCount = $('#tblUserLoc tbody tr').length;
    var Exists = false;

    var FkUserID = $("#FkUserId").val();
    var UserName = $("#drpFkUserId").val();
    if (UserName != '' && FkUserID > 0) {
        $('#tblUserLoc tr').each(function (ind) {
            debugger;
           
        //UserLoclnk_0__FkUserID
        if ($("#UserLoclnk_" + ind + "__FkUserID").val() === FkUserID && $("#UserLoclnk_" + ind + "__ModeForm").val() != '2') {
            alert("Already Exists");
            Exists = true;
        }
    });
    if (!Exists) {

        html = `<tr index="${rowCount}">
                      <td>
                          <input id="UserLoclnk_${rowCount}__UserName" name="UserLoclnk[${rowCount}].UserName" readonly="True" tabindex="-1" type="text" value="${UserName}" autocomplete="off" class="valid" aria-invalid="false">
                          <input data-val="true" data-val-required="The FkUserID field is required." id="UserLoclnk_${rowCount}__FkUserID" name="UserLoclnk[${rowCount}].FkUserID" type="hidden" value="${FkUserID}" autocomplete="off">
                      </td>
                      <td>
                          <input data-val="true" data-val-required="The ModeForm field is required." id="UserLoclnk_${rowCount}__ModeForm" name="UserLoclnk[${rowCount}].ModeForm" type="hidden" value="${rowCount}" autocomplete="off">
                          <button type="button" class="btn grid-close-btn" onclick="RowAction(this,${rowCount},'del')">
                              <i class="bi bi-x-circle"></i>
                          </button>
                          
                      </td>
                </tr>`;

        $("#tblUserLoc tbody").append(html);

        $("#drpFkUserId").val('');
        $("#FkUserId").val('');

    }
}
    else { alert("Insert User"); }
// }
return false;
}

function RowAction(obj, index, action) {

    if (action == 'del') {
        $(obj).attr('onclick', `RowAction(this,${i},\'undo\')`);
        $(obj).html('<i class="bi bi-arrow-counterclockwise"></i>');
        $(obj).closest('tr').addClass('tbl-delete');
        $(obj).prev("input[id$='__ModeForm']").val('2');
        $(obj).closest('tr').find(".action-icon").css("pointer-events", "none");
    }
    else if (action == 'undo') {
        $(obj).attr('onclick', `RowAction(this,${i},\'del\')`)
        $(obj).html('<i class="bi bi-x-circle"></i>');
        $(obj).closest('tr').removeClass('tbl-delete');
        $(obj).prev("input[id$='__ModeForm']").val('0');
        $(obj).closest('tr').find(".action-icon").css("pointer-events", "all");
    }
}

function GenerateAlias() {
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