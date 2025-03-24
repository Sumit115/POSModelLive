


function DeleteRecord() {
    let PKID = $("#PKID").val();
    if (PKID > 0) {
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'Delete',
            data: { PKID },
            datatype: "json",
            success: function (res) {
                console.log(res);
                if (res == "") {
                    window.location.href = Handler.currentPath() + "List";

                }
                else
                    alert(res);
            },
            error: function (xhr, status, error) {
                if (xhr.status == 404) { alert('coming soon'); }
                //console.error('AJAX Error:', status, error);
                //console.log('Status Code:', xhr.status);
                //console.log('Response Text:', xhr.responseText);

            }
        })
    }
}


function GenerateAlias($id) {
    debugger;
    if ($("#" + $id).val() == "") {
        $.ajax({
            type: "POST",
            url: Handler.currentPath() + 'GetAlias',
            data: {},
            datatype: "json",
            success: function (res) {
                if (res != "") {
                    $("#" + $id).val(res);
                }
            }
        });
    }

}