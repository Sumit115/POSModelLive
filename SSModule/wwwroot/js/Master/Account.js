
$(document).ready(function () {

    $(document).ready(function () {
        Common.InputFormat();
        setDiscDate();
        //  FillIFSC();
        $('#btnServerSave').click(function (e) {
            e.preventDefault();
            $("form").submit();
        });

        $('input[name="Status"]').on("change", function (e) {
            $('#DiscDate').val('');
            setDiscDate();
        });
        _Custdropdown.FKBankID.onSelect = function (arg) {
            console.log(arg.data);
            $("#IFSCCode").val(arg.data.IFSCCode);

        }

       
    })
    function setDiscDate() {

        if ($('input[name="Status"]:checked').val() == "Continue") {
            $('#DiscDate').attr('readonly', 'readonly');
        }
        else {
            $('#DiscDate').removeAttr('readonly');

        }
    }
}) 

function addOpeningBalance() {
    var foundL = false;
    if ($("#drpFkLocationId_OpBal").val().replace('"', '').replace('"', '') == "") {
        Alertdialog("Please Select Location");
        return false;
    } else if ($("#OpeningBalance").val() == "") {
        Alertdialog("Please Enter Opening Balance");
        return false;
    }
    else {
        $('#tblOpeningBalance tbody tr').each(function (index) {
            var val = $('#AccountDtl_lst_' + index + '__FKLocationID').val();
            var vald = $("#AccountDtl_lst_" + index + "__ModeForm").val();
            if (val == $("#FkLocationId_OpBal").val()) {
                foundL = true;
                $("#AccountDtl_lst_" + index + "__OpBal").val($("#OpeningBalance").val());
                $("#AccountDtl_lst_" + index + "__type").val($("#sltCrAmt").val().replace('"', ''));
                if (vald == '2') {
                    RowAction($("#AccountDtl_lst_" + index + "__ModeForm").next(), index, 'undo');
                };
            }
        });

        if (foundL == false) {
            var i = $('#tblOpeningBalance tbody tr').length;
            var html = `<tr index="${i}">
                                             <td>
                                                 <input id="AccountDtl_lst_${i}__Location" name="AccountDtl_lst[${i}].Location" type="text" value="${$("#drpFkLocationId_OpBal").val().replace('"', '').replace('"', '')}" tabindex="-1" readonly="readonly" />
                                                         <input id="AccountDtl_lst_${i}__FKLocationID" name="AccountDtl_lst[${i}].FKLocationID" type="hidden" value="${$("#FkLocationId_OpBal").val()}" />

                                             </td>
                                              <td> <input id="AccountDtl_lst_${i}__OpBal" name="AccountDtl_lst[${i}].OpBal" type="text" value="${$("#OpeningBalance").val()}" tabindex="-1" readonly="readonly" />

                                             </td>
                                                      <td>
                                                   <input id="AccountDtl_lst_${i}__type" name="AccountDtl_lst[${i}].type" type="text" value="${$("#sltCrAmt").val().replace('"', '')}" tabindex="-1" readonly="readonly" />

                                                     </td>
                                             <td>
                                                 <input id="AccountDtl_lst_${i}__ModeForm" name="AccountDtl_lst[${i}].ModeForm" type="hidden" value="0" />
                                                 <button type="button" class="btn grid-close-btn" onclick="RowAction(this,${i},\'del\')">
                                                     <i class="bi bi-x-circle"></i>
                                                 </button>
                                             </td>
                                             </tr>`;
            $("#tblOpeningBalance tbody").append(html);
        }
    }

    DropDownReset('FkLocationId_OpBal');
    $("#OpeningBalance").val('0.0');
    return false;
}

function AddLocation() {
    var foundL = false;
    if ($("#drpFkLocationId").val().replace('"', '').replace('"', '') == "") {
        Alertdialog("Please Select Location");
        return false;
    }
    else {
        $('#tblLocationLoc tbody tr').each(function (index) {
            var val = $('#AccountLocation_lst_' + index + '__FKLocationID').val();
            var vald = $("#AccountLocation_lst_" + index + "__ModeForm").val();
            if (val == $("#FkLocationId").val()) {
                foundL = true;
                if (vald == '2') {
                    RowAction($("#AccountLocation_lst_" + index + "__ModeForm").next(), index, 'undo');
                };
            }
        });

        if (foundL == false) {
            var i = $('#tblLocationLoc tbody tr').length;
            var html = `<tr index="${i}">
                                                                                <td>
                                                                                    <input id="AccountLocation_lst_${i}__Location" name="AccountLocation_lst[${i}].Location" type="text" value="${$("#drpFkLocationId").val().replace('"', '').replace('"', '')}" tabindex="-1" readonly="readonly" />
                                                                                            <input id="AccountLocation_lst_${i}__FKLocationID" name="AccountLocation_lst[${i}].FKLocationID" type="hidden" value="${$("#FkLocationId").val()}" />

                                                                                </td>
                                                                                <td>
                                                                                    <input id="AccountLocation_lst_${i}__ModeForm" name="AccountLocation_lst[${i}].ModeForm" type="hidden" value="0" />
                                                                                    <button type="button" class="btn grid-close-btn" onclick="RowAction(this,${i},\'del\')">
                                                                                        <i class="bi bi-x-circle"></i>
                                                                                    </button>
                                                                                </td>
                                                                                </tr>`;
            $("#tblLocationLoc tbody").append(html);
        }
    }

    DropDownReset('FkLocationId');
    return false;
}

function RowAction(obj, index, action) {
    debugger;
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
    debugger;
    if ($("#Alias").val() == "") {
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'GetAlias',
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