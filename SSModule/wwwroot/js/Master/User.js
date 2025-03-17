
$(document).ready(function () {
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
})
UploadImage = (id) => {
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


function AddLocation() {
    var foundL = false;
    if ($("#drpFkLocationID").val().replace('"', '').replace('"', '') == "") {
        Alertdialog("Please Select User");
        return false;
    }
    else {
        $('#tblUserLoc tbody tr').each(function (index) {
            var val = $('#UserLoclnk_' + index + '__FkLocationID').val();
            var vald = $("#UserLoclnk_" + index + "__ModeForm").val();
            if (val == $("#FkLocationID").val()) {
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
                            <input id="UserLoclnk_${i}__LocationName" name="UserLoclnk[${i}].LocationName" type="text" value="${$("#drpFkLocationID").val().replace('"', '').replace('"', '')}" tabindex="-1" readonly="readonly" /> 
                            <input id="UserLoclnk_${i}__FkLocationID" name="UserLoclnk[${i}].FkLocationID" type="hidden" value="${$("#FkLocationID").val()}" /> 
                        
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

    DropDownReset('FkLocationID');
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
