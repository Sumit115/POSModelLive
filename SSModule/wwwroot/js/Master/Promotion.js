$(document).ready(function () {

    Common.InputFormat();
    var PromotionDuring = $("#PromotionDuring").val();
    $("#btnServerBack").attr("href", "/Master/Promotion/List/" + PromotionDuring);
    $("#Promotion option[value*='Free Point']").prop('disabled', true);
    if ($("#PromotionApplyOn").val() != "Invoice Value") {
        $("#Promotion option[value*='Free Point']").prop('disabled', true);
    }
    hideShow();
    $('#btnServerSave').click(function (e) {
        e.preventDefault();
        $("form").submit();
    });
});

function hideShow(Ischange) {

    var _applyOn = $("#PromotionApplyOn").val();
    var _promotion = $("#Promotion").val();
    $("#div_Promotionddl,#div_FreeProductSingle,#div_FreeProductList").show();
    $("#div_PromotionLnk,#divCategory,#divBrand,#divProduct").hide();


    if (Ischange) {
        $("#FKProductId,#FKCategoryId,#FkBrandId,#FkPromotionProdId,#PromotionApplyAmt,#PromotionApplyAmt2,#PromotionApplyQty,#PromotionApplyQty2,#PromotionAmt,#PromotionQty").val('0');
        $("#drpFKProductId,#drpFKCategoryId,#drpFkBrandId,#drpFkPromotionProdId").val('');
        $("#FkPromotionApplyUnitId,FkPromotionUnitId").val('1')
        $("#Promotion option[value*='Free Point']").prop('disabled', true);
        $("#Promotion option[value*='Free Qty'],#Promotion option[value*='Trade Discount']").prop('disabled', false);
        $("#Promotion").val(_promotion);

        //$("#Promotion").val('Free Product');
        // $("#tblPromotionLnk tbody").hide('');
        $('#tblPromotionLnk tbody').html('');
    }
    if (_applyOn == "Product" || _applyOn == "Category") {
        $("#Promotion option[value*='Flat Rate'],#Promotion option[value*='Flat Qty']").prop('disabled', false);
    } else {
        $("#Promotion option[value*='Flat Rate'],#Promotion option[value*='Flat Qty']").prop('disabled', true);
        if (_promotion == 'Flat Rate' || _promotion == 'Flat Qty') {
            $("#Promotion").val('Free Product');
            _promotion = 'Free Product';
        }
    }
    $("#divheading").text(_promotion);

    $("#PromotionApplyAmt,#PromotionApplyAmt2,#PromotionApplyQty,#PromotionApplyQty2,#PromotionAmt,#FkPromotionApplyUnitId,#drpFkPromotionProdId,#PromotionQty,#FkPromotionUnitId").attr("readonly", "readonly");
    if (_applyOn == "Product" || _applyOn == "Category" || _applyOn == "Brand") {
        $("#divheadingApplyOn").text("Apply On");

        $("#div" + _applyOn + ",#div_PromotionLnk").show();
        $("#tblPromotionLnkHeading").text(_applyOn);
        if (_promotion == "Free Product") {
            $("#PromotionApplyQty,#drpFkPromotionProdId,#PromotionQty").removeAttr("readonly");
        }
        else if (_promotion == "Free Qty") {
            $("#PromotionApplyQty,#PromotionQty").removeAttr("readonly");
        }
        else if (_promotion == "Trade Discount" || _promotion == "Scheme Discount") {
            $("#PromotionApplyQty,#PromotionAmt").removeAttr("readonly");
        }
        else if (_promotion == "Free Point") {
            $("#PromotionApplyAmt,#PromotionAmt").removeAttr("readonly");
        }
        else if (_promotion == "Flat Rate" && _applyOn != "Brand") {
            $("#PromotionApplyAmt,#PromotionApplyAmt2,#PromotionAmt").removeAttr("readonly");
        }
        else if (_promotion == "Flat Qty" && _applyOn != "Brand") {
            $("#PromotionApplyQty,#PromotionApplyQty2,#PromotionAmt").removeAttr("readonly");
        }
    }
    else if (_applyOn == "Invoice Value") {
        $("#divheadingApplyOn").text("");
        $("#PromotionApplyAmt").removeAttr("readonly");
        $("#Promotion option[value*='Free Point']").prop('disabled', false);
        $("#Promotion option[value*='Free Qty'],#Promotion option[value*='Trade Discount']").prop('disabled', true);

        if (_promotion == "Free Product" || _promotion == "Free Qty") {
            $("#drpFkPromotionProdId,#PromotionQty").removeAttr("readonly");
        }
        else if (_promotion == "Trade Discount" || _promotion == "Scheme Discount") {
            $("#PromotionAmt").removeAttr("readonly");
        }
        else if (_promotion == "Free Point") {
            $("#PromotionAmt").removeAttr("readonly");
        }


    }
    else if (_applyOn == "XonX") {

        $("#divProduct,#divBrand,#div_Promotionddl,#div_FreeProductSingle,#div_FreeProductList").hide();
        $("#divheadingApplyOn").text("Add Category");
        $("#div_PromotionLnk,#divCategory,#div_FreeProductSingle").show();
        $("#PromotionApplyQty,#FkPromotionApplyUnitId,#PromotionQty").removeAttr("readonly");
        $("#tblPromotionLnkHeading").text("Category");

    }
}

function addLocations() {

    var rowCount = $('#tblLocation tbody tr').length;


    var index = $("#hidLocationIndex").val();
    var Exists = false;

    var FkLocationId = $("#FkLocationId").val();
    if (FkLocationId != '') {
        $('#tblLocation tr').each(function (ind) {
            if ($("#PromotionLocation_lst_" + ind + "__FkLocationId").val() == FkLocationId && index !== ind.toString() && $("#PromotionLocation_lst_" + ind + "__Mode").val() != '2') {
                alert("Already Exists");
                Exists = true;
            }
        });

        if (!Exists) {


            if (index === "") {
                var html = '<tr index="' + rowCount + '">';
                html += '<td class="tabel-td-xs">  <input  id="PromotionLocation_lst_' + rowCount + '__LocationName" name="PromotionLocation_lst[' + rowCount + '].LocationName" type="text" value="' + $("#drpFkLocationId").val() + '" tabindex="-1"> </td>';
                html += '<td class="tabel-td-xs">';
                html += '<input id="PromotionLocation_lst_' + rowCount + '__Mode" name="PromotionLocation_lst[' + rowCount + '].Mode" type="hidden" value="0">';
                html += '<input id="PromotionLocation_lst_' + rowCount + '__PkId" name="PromotionLocation_lst[' + rowCount + '].PkId" type="hidden" value="0">';
                html += '<input id="PromotionLocation_lst_' + rowCount + '__FkLocationId" name="PromotionLocation_lst[' + rowCount + '].FkLocationId" type="hidden" value="' + $("#FkLocationId").val() + '">';
                html += '<span class="action-icon" onclick="UpdateLocation(this,' + rowCount + ',\'del\')"><i class="fa fa-trash" /></span> </td>';

                html += '</tr>';

                $("#tblLocation tbody").append(html);
            } else {
                // $("#PromotionLocation_lst_" + index + "__Location").val($("#txtLocation").val());
                //  $("#PromotionLocation_lst_" + index + "__Mode").val('1');

            }
            $("#drpFkLocationId").val('');
            //$("#FklocalityGridId").prop('selectedIndex', 0);
            //DropDownReset('FklocalityGridId');
        }
    }
    else { alert("Insert Location"); }
    // }
    return false;
}

function UpdateLocation(obj, index, action) {
    //if (action === 'edit') {
    //    $("#hidLocationIndex").val(index);
    //    $("#FklocalityGridId").val($("#PromotionLocation_lst_" + index + "__FKLocationID").val());
    //    // $("#drpFklocalityGridId").val($("#PromotionLocation_lst_" + index + "__Location").val());
    //    $("#OpeningBalance").val($("#PromotionLocation_lst_" + index + "__Location").val());
    //    $('#thDrCr').find("input[value='" + $("#PromotionLocation_lst_" + index + "__type").val() + "']").prop('checked', true);
    //    $(obj).closest('tr').removeClass('tbl-delete');
    //} else

    if (action === 'del') {
        $(obj).closest('tr').addClass('tbl-delete');
        $("#PromotionLocation_lst_" + index + "__Mode").val('2');
    }
}

function addPromotionLnk() {
    var _PromotionLnkFor = $("#tblPromotionLnkHeading").text();
    var rowCount = $('#tblPromotionLnk tbody tr').length;


    var index = $("#hidPromotionLnkIndex").val();
    var Exists = false;

    var FkLinkId = $("#Fk" + _PromotionLnkFor + "Id").val();
    var FkLinkName = $("#drpFk" + _PromotionLnkFor + "Id").val();
    if (FkLinkId != '') {
        $('#tblPromotionLnk tr').each(function (ind) {
            if ($("#PromotionLnk_lst_" + ind + "__FkLinkId").val() == FkLinkId && index !== ind.toString() && $("#PromotionLnk_lst_" + ind + "__Mode").val() != '2') {
                alert("Already Exists");
                Exists = true;
            }
        });

        if (!Exists) {


            if (index === "") {
                var html = '<tr index="' + rowCount + '">';
                html += '<td class="tabel-td-xs">  <input  id="PromotionLnk_lst_' + rowCount + '__LocationName" name="PromotionLnk_lst[' + rowCount + '].LocationName" type="text" value="' + FkLinkName + '" tabindex="-1"> </td>';
                html += '<td class="tabel-td-xs">';
                html += '<input id="PromotionLnk_lst_' + rowCount + '__Mode" name="PromotionLnk_lst[' + rowCount + '].Mode" type="hidden" value="0">';
                html += '<input id="PromotionLnk_lst_' + rowCount + '__PkId" name="PromotionLnk_lst[' + rowCount + '].PkId" type="hidden" value="0">';
                html += '<input id="PromotionLnk_lst_' + rowCount + '__FkLinkId" name="PromotionLnk_lst[' + rowCount + '].FkLinkId" type="hidden" value="' + FkLinkId + '">';
                html += '<span class="action-icon" onclick="UpdatePromotionLnk(this,' + rowCount + ',\'del\')"><i class="fa fa-trash" /></span> </td>';

                html += '</tr>';

                $("#tblPromotionLnk tbody").append(html);
            } else {
                // $("#PromotionLnk_lst_" + index + "__Location").val($("#txtLocation").val());
                //  $("#PromotionLnk_lst_" + index + "__Mode").val('1');

            }
            $("#drpFk" + _PromotionLnkFor + "Id").val('');
            //$("#FklocalityGridId").prop('selectedIndex', 0);
            //DropDownReset('FklocalityGridId');
        }
    }
    else { alert("Insert " + _PromotionLnkFor); }
    // }
    return false;
}

function UpdatePromotionLnk(obj, index, action) {
    //if (action === 'edit') {
    //    $("#hidLocationIndex").val(index);
    //    $("#FklocalityGridId").val($("#PromotionLnk_lst_" + index + "__FKLinkId").val());
    //    // $("#drpFklocalityGridId").val($("#PromotionLnk_lst_" + index + "__Location").val());
    //    $("#OpeningBalance").val($("#PromotionLnk_lst_" + index + "__Location").val());
    //    $('#thDrCr').find("input[value='" + $("#PromotionLnk_lst_" + index + "__type").val() + "']").prop('checked', true);
    //    $(obj).closest('tr').removeClass('tbl-delete');
    //} else

    if (action === 'del') {
        $(obj).closest('tr').addClass('tbl-delete');
        $("#PromotionLnk_lst_" + index + "__Mode").val('2');
    }
}