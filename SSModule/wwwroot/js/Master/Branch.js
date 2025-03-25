
$(document).ready(function () {
    Common.InputFormat();
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
function BindCity(CityId) {

    var StateName = $("#State").val();
    $("#FkCityId").empty();
    //

    if (StateName != '') {

        $.ajax({
            type: "POST",
            url: '/Master/City/GetDrpCityByState',
            data: { State: StateName },
            datatype: "json",
            success: function (res) {
                console.log(res);
                $(res).each(function (i, v) {

                    $("#FkCityId").append("<option value='" + v.PkCityId + "'>" + v.CityName + "</option>");
                });

                if (CityId > 0) {
                    $("#FkCityId").val(CityId);
                }
            }
        })

    } else { $("#FkCityId").append("<option value=''>-Select-</option>"); }
}