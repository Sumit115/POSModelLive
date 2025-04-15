$(document).ready(function () {

    Common.InputFormat();
    BindCity();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
});

function BindCity() {
    debugger;

    var StateName = $("#StateName").val();
    $("#FkCityId").empty();

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

                if ($("#hdCityId").val() > 0) {
                    $("#FkCityId").val($("#hdCityId").val());
                }
            }
        })

    } else { $("#FkCityId").append("<option value=''>-Select-</option>"); }
}

function GenerateAlias() {
    if ($("#Code").val() == "") {
        $.ajax({
            type: "POST",
            url: '/Master/Customer/GetAlias',
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