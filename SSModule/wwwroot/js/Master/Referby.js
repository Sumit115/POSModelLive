$(document).ready(function () {

    Common.InputFormat();
   // BindCity();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
});
 
function GenerateAlias() {
    if ($("#Code").val() == "") {
        $.ajax({
            type: "POST",
            url: '/Master/ReferBy/GetAlias',
            data: {},
            datatype: "json",
            success: function (res) {
                if (res != "") {
                    $("#Code").val(res);
                }
            }
        });
    }

}