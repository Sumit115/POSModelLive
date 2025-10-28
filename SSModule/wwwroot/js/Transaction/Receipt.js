
var tranModel = null;
var ControllerName = "";
var TranAlias = "";
var ModeFormForEdit = 1;
var cgRtn = null;
var UDIRtn = null;
$(document).ready(function () {
    $('#btnDeleteRecord').hide();
    ModeFormForEdit = Handler.isNullOrEmpty($("#hdModeFormForEdit").val()) ? 1 : parseInt($("#hdModeFormForEdit").val());
    Common.InputFormat();
    ControllerName = $("#hdControllerName").val();
    Load();
    tranModel.TrnStatus = Handler.isNullOrEmpty(tranModel.TrnStatus) ? "P" : tranModel.TrnStatus.replace('\u0000', '');
    TranAlias = tranModel.ExtProperties.TranAlias;
    $("#hdFormId").val(tranModel.ExtProperties.FKFormID);
    $("#hdGridName").val('dtl');
    $("#btnApplyPromotion").hide();
    $("#btnClose").parent().hide();
    $("#btnOpen").parent().hide();




    //$(document.body).keyup(function (e) {
    //    if (e.keyCode == 120 && e.ctrlKey) {
    //        AutoFillLastRecord();
    //    }
    //});
    $('#btnServerSave').click(function (e) {

        if ($("#TranForm").valid()) {
            SaveRecord();
        }
        return false;
    });
     

    $(".trn").change(function () {
        var fieldName = $(this).attr("id");
        tranModel[fieldName] = $(this).val();
    });
    $("#EntryNo").change(function () {
        DatabyEntryNo();
    });
    $("#PayMode").change(function () {
        var val = $(this).val();
        $('.divCheque,.divCard,#dvCashTxtbox,#dvChequeTxtbox,#dvCreditTxtbox').hide();
        $("#CashAmt,#ChequeAmt,#CreditCardAmt").val('0');
        tranModel.CashAmt = tranModel.ChequeAmt = tranModel.CreditCardAmt = 0
        if (val == "Cheque") { $('.divCheque,#dvChequeTxtbox').show(); }
        else if (val == "Card") { $('.divCard,#dvCreditTxtbox').show(); }
        else { $("#dvCashTxtbox").show(); }
    });
});

function Load() {

    var PkId = $("#PkId").val();
    tranModel = JSON.parse($("#hdData").val());
    TranAlias = tranModel.ExtProperties.TranAlias;


}


function setHeaderData(data) {
    Common.Set(".trn-header", data, "");
    return false;
}

function setFooterData(data) {
    Common.Set(".trn-footer", data, "");
    return false;
}

function setRebateAcc() {  
    tranModel.FkRebateAccId = $("#FkRebateAccId").val();
}
function setInterestAcc() {
    
    tranModel.FkInterestAccId = $("#FkInterestAccId").val();
}
function setBankCheque() {
    tranModel.FKBankChequeID = $("#FKBankChequeID").val();
}
function setBankPost() {
    tranModel.FKBankPostID = $("#FKBankPostID").val();
}
function setBankCreditCard() {
    tranModel.FKBankCreditCardID = $("#FKBankCreditCardID").val();
} 
function SaveRecord() {

    Common.Get(".form", "", function (flag, _d) {

        if (flag) {

            tranModel.PkId = $('#PkId').val();
            tranModel.FkPartyId = $('#FkPartyId').val();
            tranModel.EntryDate = $('#EntryDate').val();
            tranModel.GRDate = $('#GRDate').val();
            tranModel.TranDetails = [];
            if ((tranModel.FkPartyId > 0)) {
                if (tranModel.FKSeriesId > 0) {

                    // tranModel.TranDetails = GetDataFromGrid(true, false);


                    //var filteredDetails = tranModel.TranDetails.filter(x => x.ModeForm != 2);
                    //if (tranModel.TranDetails.length >= 0 && filteredDetails.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: Handler.currentPath() + 'Create',
                            data: { model: tranModel },
                            datatype: "json",
                            success: function (res) {

                                if (res.status == "success") {
                                    alert('Save Successfully..');
                                    if (ControllerName == 'SalesInvoiceTouch') {
                                        location.reload();
                                    } else {
                                        window.location = Handler.currentPath() + 'List';
                                    }
                                }
                                else {

                                    alert(res.msg);
                                    tranModel = res.data;
                                    BindGrid('DDT', tranModel.TranDetails);
                                }
                            }
                            , error: function (xhr, status, error) {
                                if (xhr.status === 400) {
                                    // Handle Bad Request
                                    let errorMessage = xhr.responseJSON?.message || xhr.responseText || "Bad Request";
                                    alert("Error 400: " + errorMessage);
                                } else {
                                    alert("Error: " + xhr.status + " - " + error);
                                }
                                $(".loader").hide();
                            }
                        });

                    //}
                    //else
                    //    alert("Insert Valid Product Data..");
                }
                else
                    alert("Please Select Series");
            }
            else
                alert("Please Select Party");
        }
        else
            alert("Some Error found.Please Check");
    });
}

 
function setParty() {

    var FkPartyId = $("#FkPartyId").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetParty',
        data: { model: tranModel, FkPartyId: FkPartyId },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $('#FkPartyId').val(tranModel.FkPartyId);
                $('#drpFkPartyId').val(tranModel.PartyName);
                $('#FKPostAccID').val(tranModel.FKPostAccID);
                $('#drpFKPostAccID').val(tranModel.Account);  
            }
            else
                alert(res.msg);
        }
    });
} 
function setAccount() {

    var FkAccountID = $("#FKPostAccID").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetAccount',
        data: { model: tranModel, FkAccountID: FkAccountID },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $('#FkPartyId').val(tranModel.FkPartyId);
                $('#drpFkPartyId').val(tranModel.PartyName);
                $('#FKPostAccID').val(tranModel.FKPostAccID);
                $('#drpFKPostAccID').val(tranModel.Account);
            }
            else
                alert(res.msg);
        }
    });
}
function setSeries() {
    var FKSeriesId = $("#FKSeriesId").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'SetSeries',
        data: { model: tranModel, FKSeriesId: FKSeriesId },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                tranModel = res.data;
                $("#FKSeriesId").val(FKSeriesId);
                $('#PartyCredit').val(tranModel.PartyCredit);
            }
            else
                alert(res.msg);
        }
    });
}

function setReferBy() {

    tranModel["FKReferById"] = $("#FKReferById").val();
}
function setSalesPer() {

    tranModel['FKSalesPerId'] = $("#FKSalesPerId").val();
}
 

function trandtldropList(data) {

    var output = []
    $.ajax({
        url: Handler.currentPath() + 'trandtldropList', data: data, async: false, dataType: 'JSON', success: function (result) {

            output = result;

        }, error: function (request, status, error) {
        }
    });
    //console.log(output);
    return output;

}

 
function DatabyEntryNo() {
    $(".loader").show();
    var EntryNo = $("#EntryNo").val();
    var FKSeriesId = $("#FKSeriesId").val();
    $.ajax({
        type: "POST",
        url: Handler.currentPath() + 'GetIdbyEntryNo',
        data: { EntryNo: EntryNo, FKSeriesId: FKSeriesId },
        datatype: "json",
        success: function (res) {
            if (res.status == "success") {
                if (res.data > 0) {
                    var pk_Id = res.data;
                    window.location.href = Handler.currentPath() + "Create/" + pk_Id + "/" + FKSeriesId;
                }
                else {
                    alert('Not Found');
                    $(".loader").hide();
                    location.reload()
                }
            }
            else {
                alert(res.msg);
                $(".loader").hide();
                location.reload()
            }

        }
    })
}

function VoucherDetail() {

    var pagetitle = $(".pagetitle").text().replace(/\s+/g, " ").trim();
    if (tranModel.PkId > 0) {

        var url = "/Transactions/Voucher/View/" + tranModel.PkId + "/" + tranModel.FKSeriesId + "/" + pagetitle;

        window.open(url);
    }
}

  