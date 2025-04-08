$(document).ready(function () {
    Common.InputFormat();
   $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
})

function addSizes() {

    var rowCount = $('#tblSize tbody tr').length;


    var index = $("#hidSizeIndex").val();
    var Exists = false;

    var Size = $("#txtSize").val();
    if (Size != '') {
        $('#tblSize tr').each(function (ind) {
            if ($("#CategorySize_lst_" + ind + "__Size").val() === Size && index !== ind.toString() && $("#CategorySize_lst_" + ind + "__Mode").val() != '2') {
                alert("Already Exists");
                Exists = true;
            }
        });
        if (!Exists) {


            if (index === "") {
                var html = '<tr index="' + rowCount + '">';
                html += '<td class="tabel-td-xs">  <input  id="CategorySize_lst_' + rowCount + '__Size" name="CategorySize_lst[' + rowCount + '].Size" type="text" value="' + $("#txtSize").val() + '" tabindex="-1"> </td>';
                html += '<td class="tabel-td-xs">';
                html += '<input id="CategorySize_lst_' + rowCount + '__Mode" name="CategorySize_lst[' + rowCount + '].Mode" type="hidden" value="0">';
                html += '<input id="CategorySize_lst_' + rowCount + '__PkId" name="CategorySize_lst[' + rowCount + '].PkId" type="hidden" value="0">';
                html += '<span class="action-icon" onclick="UpdateSize(this,' + rowCount + ',\'del\')"><i class="fa fa-trash" /></span> </td>';

                html += '</tr>';

                $("#tblSize tbody").append(html);
            } else {
                // $("#CategorySize_lst_" + index + "__Size").val($("#txtSize").val());
                //  $("#CategorySize_lst_" + index + "__Mode").val('1');

            }
            $("#txtSize").val('');
            //$("#FklocalityGridId").prop('selectedIndex', 0);
            //DropDownReset('FklocalityGridId');
        }
    }
    else { alert("Insert Size"); }
    // }
    return false;
}

function UpdateSize(obj, index, action) {
    //if (action === 'edit') {
    //    $("#hidSizeIndex").val(index);
    //    $("#FklocalityGridId").val($("#CategorySize_lst_" + index + "__FKSizeID").val());
    //    // $("#drpFklocalityGridId").val($("#CategorySize_lst_" + index + "__Size").val());
    //    $("#OpeningBalance").val($("#CategorySize_lst_" + index + "__Size").val());
    //    $('#thDrCr').find("input[value='" + $("#CategorySize_lst_" + index + "__type").val() + "']").prop('checked', true);
    //    $(obj).closest('tr').removeClass('tbl-delete');
    //} else

    if (action === 'del') {
        $(obj).closest('tr').addClass('tbl-delete');
        $("#CategorySize_lst_" + index + "__Mode").val('2');
    }
}