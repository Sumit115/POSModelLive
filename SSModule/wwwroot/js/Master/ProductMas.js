
var VariationList = null;
var AddonList = null;
$(document).ready(function () {
    //  console.log($("#hdProductAddons").val())
    //  console.log(JSON.parse($("#hdProductAddons").val()))
    AddonList = JSON.parse($("#hdAddonList").val())
    VariationList = JSON.parse($("#hdVariationList").val())
    console.log($("#IsVariation").val());
    if ($("#IsAddon").val() == 'True') {
        $('#chkIsAddon').prop('checked', true);

        BindProductAddon(JSON.parse($("#hdProductAddons").val()))
    }
    if ($("#IsVariation").val() == 'True') {
        $('#chkIsVariation').prop('checked', true);
        $('#chkIsAddon').prop('checked', false);
        $("#div_Addon,#div_Variation").html('');
        $("#IsVariation,#IsAddon").val(false);

        $("#btnaddNewVariation").show();
        $("#IsVariation").val(true);
        if ($("#hdProductVariation").val() != null && $("#hdProductVariation").val() != "" && $("#hdProductVariation").val() != "null") {
            var _d = JSON.parse($("#hdProductVariation").val());
            if (_d.length > 0) {
                $.each(_d, function (index, value) {
                    BindVariationhtml(index, value)
                 });
                //console.log(_d);
            }

        }
    }

    $.ajax({
        type: "POST",
        url: '/Master/Category/CategoryList',
        dataType: "json",
        success: function (data) {


            var s = '<option value="-1">Please Select a Category</option>';
            $.each(data, function (index, value) {
                s += '<option value="' + data[index].pkId + '">' + data[index].categoryName + '</option>';
            });
            $("#fkcategory").html(s);
        }
    });

    $('#chkIsAddon').on("change", function () {
        BindProductAddon([]);
    });


    $('#chkIsVariation').on("change", function () {
        $('#chkIsAddon').prop('checked', false);
        $("#div_Addon,#div_Variation").html('');
        $("#btnaddNewVariation").hide();
        $("#IsVariation,#IsAddon").val(false);
        
        if ($(this).is(':checked')) {
            $("#IsVariation").val(true);
            $("#btnaddNewVariation").show();
            BindVariationhtml(0, {})

        }
    });

});
function BindProductAddon(data) {

    $('#chkIsVariation').prop('checked', false);
    $("#div_Addon,#div_Variation").html('');
    $("#btnaddNewVariation").hide();
    $("#IsVariation,#IsAddon").val(false);
    
    if ($("#chkIsAddon").is(':checked')) {
        $("#IsAddon").val(true);

        html = "<div class=row>";
        $.each(AddonList, function (i, v) {
            
            let ischecked = false;
            $.map(data, function (elementOfArray, indexInArray) {
                // console.log(elementOfArray);
                if (elementOfArray.Value == AddonList[i].PkId) {

                    ischecked = true;
                }
            });
            //console.log(ischecked);
            html += '<div class="col-sm-2 form-check">';
            html += ' <input data-val="true" data-val-required="The Value field is required." id="ProductAddons_' + i + '__Value" name="ProductAddons[' + i + '].Value" type="hidden" value="' + AddonList[i].PkId + '" ' + (ischecked ? "checked" : "") + '>';
            html += ' <input data-val="true" data-val-required="The Selected field is required." id="ProductAddons_' + i + '__Selected" name="ProductAddons[' + i + '].Selected" type="checkbox" value="true" ' + (ischecked ? "checked" : "") + '>';
            html += AddonList[i].AddonName;
            html += '  </div> ';


        })
        html += "</div>";


        $("#div_Addon").html(html);

    }
}
//function getAddon() {
//    $.ajax({
//        type: "POST",
//        url: '/Master/Addons/AddonsList',
//        dataType: "json",
//        success: function (data) {
//            AddonList = data;
//        }
//    });
//}
//function GetVariation() {

//    $.ajax({
//        type: "POST",
//        url: '/Master/Variation/VariationList',
//        dataType: "json",
//        success: function (data) {
//            console.log(data);
//            VariationList = data;
//            BindVariationhtml(0);
//        }
//    });

//    // console.log(VariationList);
//}
function AddNewVariation() {
    console.log($(".product-variation").length);
    BindVariationhtml($(".product-variation").length, {})
}
function BindVariationhtml(n, data) {
    console.log(data.FkVariationId);

    var html = '';
    html += '<div class="row product-variation" data-no="' + n + '" style="border: 1px solid #ced4da;padding: 8px;margin: 10px 0;">';

    html += '  <div class="col-md-4">';
    html += '        <div class="form-group">';
    html += '               <label class="control-label">Variation</label>';
    html += '              <select class="form-control ddlVariation" data-val="true" data-val-required="The FkVariationId field is required." id="ProductVariation_' + n + '__FkVariationId" name="ProductVariation[' + n + '].FkVariationId">';

    html += '                        </select > ';
    html += '           </div>';
    html += '       </div>';
    ///////
    html += '  <div class="form-group">';
    html += '                   <label class="control-label" for="ProductVariation_' + n + '__Price">Price</label>';
    html += '                   <input class="form-control" type="text" data-val="true" data-val-number="The field Price must be a number." id="ProductVariation_' + n + '__Price" name="ProductVariation[' + n + '].Price" value="">';
    html += '                   <span class="text-danger field-validation-valid" data-valmsg-for="ProductVariation[' + n + '].Price" data-valmsg-replace="true"></span>';
    html += '               </div>';

    /////
    html += ' <div class="col-md-4">';
    html += '               <div class="form-group">';
    html += '                   <div class="form-check">';
    html += '                   <input class="form-control" type="hidden"      id="ProductVariation_' + n + '__IsAddon" name="ProductVariation[' + n + '].IsAddon" value="false">';
   html += '        <input class="form-check-input" type="checkbox"   id="chkProductVariation_' + n + '__IsAddon" onchange="LoadVariationAddon(this,' + n + ',[])">   ';
    html += '                       <label class="form-check-label" for="ProductVariation_' + n + '__IsAddon">IsAddon</label>';
    html += '                   </div>';
    html += '               </div>';
    html += '           </div>';
    /////////

    html += ' <div id="div_VariationAddon_' + n + '" class="col-md-12">';
    html += ' </div>  ';
    html += ' </div>';
    $("#div_Variation").append(html);

    
    var s = '<option value="-1">Please Select Variation</option>';
    $.each(VariationList, function (index, value) {

        s += '<option value="' + VariationList[index].PkId + '">' + VariationList[index].VariationName + '</option>';
    });
    
    $('#div_Variation #ProductVariation_' + n + '__FkVariationId').html(s);

    if (data.FkVariationId > 0) {
        console.log(data);
        $('#div_Variation #ProductVariation_' + n + '__FkVariationId').val(data.FkVariationId);
        $('#div_Variation #ProductVariation_' + n + '__Price').val(data.Price);
        if (data.IsAddon) {
            $('#ProductVariation_' + n + '__IsAddon').val(true);
            $('#chkProductVariation_' + n + '__IsAddon').prop('checked', true);

            LoadVariationAddon($('#chkProductVariation_' + n + '__IsAddon'), n, data.ProductAddons)
        }
    }
}
function LoadVariationAddon($ctrl, n, data) {
    $("#div_VariationAddon_" + n).html('');
    $('#ProductVariation_' + n + '__IsAddon').val(false);
    if ($($ctrl).is(':checked')) {
        $('#div_Variation #ProductVariation_' + n + '__IsAddon').val(true);

         
        html = "<div class=row>";
        $.each(AddonList, function (i, v) {

            let ischecked = false;
            $.map(data, function (elementOfArray, indexInArray) {
                // console.log(elementOfArray);
                if (elementOfArray.Value == AddonList[i].PkId) {

                    ischecked = true;
                }
            });
            html += '<div class="col-sm-2 form-check">';
            html += ' <input data-val="true" data-val-required="The Value field is required." id="ProductVariation_' + n + '__ProductAddons_' + i + '__Value" name="ProductVariation[' + n + '].ProductAddons[' + i + '].Value" type="hidden" value="' + AddonList[i].PkId + '" ' + (ischecked ? "checked" : "") + '>';
            html += ' <input data-val="true" data-val-required="The Selected field is required." id="ProductVariation_' + n + '__ProductAddons_' + i + '__Selected" name="ProductVariation[' + n + '].ProductAddons[' + i + '].Selected" type="checkbox" value="true" ' + (ischecked ? "checked" : "") + '>';
            html += AddonList[i].AddonName;
            html += '  </div> ';


        })
        html += "</div>";


        $("#div_VariationAddon_" + n).html(html);

    }
}