
$(document).ready(function () {

    Common.InputFormat();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
})
 