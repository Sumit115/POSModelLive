
$(document).ready(function () {

    Common.InputFormat();
         $('#btnServerSave').click(function (e) {
            e.preventDefault();
            $("form").submit();
         }); 

    if ($('#PkAccountGroupId').val() > 0)
        $("#btnDeleteRecord").parent().show();

    $('#btnDeleteRecord').click(function (e) {
        let pk_Id = $("#PkAccountGroupId").val();

        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'DeleteRecord',
            data: { PKID: pk_Id },
            datatype: "json",
            success: function (res) {
                console.log(res);
                if (res == "") {
                    window.location.href = Handler.currentPath() + "List";

                }
                else
                    alert(res);
            }
        })
    });
}) 

function GenerateAlias() {
    debugger;
    if ($("#GroupAlias").val() == "") {
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'GetAlias',
            data: {},
            datatype: "json",
            success: function (res) {
                if (res != "") {
                    $("#GroupAlias").val(res);
                }
            }
        });
    }

}