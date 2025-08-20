
$(document).ready(function () {

    $(document).ready(function () {
        Common.InputFormat();
          $('#btnServerSave').click(function (e) {
            e.preventDefault();
            $("form").submit();
        });
         
    })
     
}) 
function Menucolaps(ctrl, id) {
    
    var flag = $(ctrl).find("i").hasClass("fa-angle-right");
    if (flag) {
        $(ctrl).find('label').html('<i class="right fas fa-angle-down"> </i>');
        $('[key="' + id + '"]').show();
    }
    else {
        $('[key="' + id + '"]').hide();

        $('[key="' + id + '"].main-sub').each(function () {
            var chId = $(this).attr("parent");

            $(this).find("i").attr('class', "right fas fa-angle-right");
            $('[key="' + chId + '"]').hide();
        });
        $(ctrl).find('label').html('<i class="right fas fa-angle-right"> </i>');
    }

}