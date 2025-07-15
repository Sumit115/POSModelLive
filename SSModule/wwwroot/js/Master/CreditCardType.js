$(document).ready(function () {

    Common.InputFormat();
   // BindCity();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
});
 