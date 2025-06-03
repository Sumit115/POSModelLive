
$(document).ready(function () {

    Common.InputFormat();
         $('#btnServerSave').click(function (e) {
            e.preventDefault();
            $("form").submit();
         }); 

    
}) 

function GenerateAlias() {
    
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