﻿/// <reference path="/Scripts/jquery-1.8.0.min.js" />
/// <reference path="/Scripts/jquery-ui-1.8.11.js" />
/// <reference path="/Scripts/Common/jQuery.CCICommon.js" />
/// <reference path="/Scripts/Common/jQuery.ui.ATK.js" />
/// <reference path="../jquery-1.8.0.min.js" />

var _newtableformat = "No";
var _orderId = "1";
var _securityId;
var _shortName;
var _quoteId;
var _name;
var _address;
var _city;
var _state;
var _zip;
var _userType;
var _defaultPosition = 0;
var _isRecommended;
var _customerId;
var _customerData;
var _defaultOrderId = "1";
var InternetAccessCount = 0;
var DialToneCount = 0;
var NetworkingCount = 0;
var FeesCount = 0;
var NonCityCount = 0;
var quoteId = "0";
var quoteName = "";
//var isRecommended = true;
var cloneCounter = 0;

try {
    $(document).ready(function () {
        SetVisualElements();
    });  // document.ready
} catch (e) {
    alert(e);
}

// --- START: VISUAL ELEMENTS CODE --------------------------------------------------
function setLearnMoreLinks() {
    $(".learn-more-drop-content").hide();
    $(".learn-more-drop").click(function () {
        
        $(this).next(".learn-more-drop-content").toggle();
        return false;
    });
}
function SetVisualElements() {
    //HideLoading();
    setLearnMoreLinks();
    ClientAutoComplete();
    SetLearnMoreRadioButtons();
    SetTabOrder();
    VariableMRCPicklists();

    AddNewRow();
    MoreLess();
    ExpandedData();
    NonExpandedData();

    SetTotalsSidebar();
    InstallationCost();
}

function InstallationCost() {
    $("#installationcost-me").click(function () {
        $("#creditcardinfo").hide();
        $("#Header_CreditCardName").val("");
        $("#Header_ExpirationDate").val("");
        $("#Header_SecurityCode").val("");
        $("#Header_Amount").val("");
    });
    $("#installationcost-dealer").click(function () {
        $("#creditcardinfo").show();
    });
    $("#installationcost-custom").click(function () {
        $("#creditcardinfo").hide();
        $("#Header_CreditCardName").val("");
        $("#Header_ExpirationDate").val("");
        $("#Header_SecurityCode").val("");
        $("#Header_Amount").val("");
    });
}

function ClientAutoComplete() {
    $("#Header_LegalName").autocomplete({
        source: function (request, response) {
            var _criteria = $("#Header_LegalName").val();
            var obj = $.getClients(_criteria);
            response(obj);
        },
        focus: function (event, ui) {
            this.value = ui.item.label;
            event.preventDefault(); // Prevent the default focus behavior. 
        },
        select: function (event, ui) {
            if (ui.item) {
                event.preventDefault(); //allow me to update the text on the textbox
                LoadClient(ui.item.value);
            }
        },
        change: function () {
        }
    });
}

function SetLearnMoreRadioButtons() {
    //Phones Provider question section
    $(".totainlearn-more-drop#phones-hide").click(function () {
        $(this).nextAll(".learn-more-content:first").hide(300);
    });
    $(".learn-more-drop#phones-show").click(function () {
        $(this).next(".learn-more-content").show(300);
    });

    //Carrier Services Provider question section
    $(".learn-more-drop#carrier-hide").click(function () {
        $(this).nextAll(".learn-more-content.carrier-existing:first").hide(300);
        $(this).nextAll(".learn-more-content.carrier-elsewhere:first").hide(300);
    });
    $(".learn-more-drop#carrier-existing").click(function () {
        $(this).nextAll(".learn-more-content.carrier-elsewhere:first").hide(300);
        $(this).nextAll(".learn-more-content.carrier-existing:first").show(300);
    });
    $(".learn-more-drop#carrier-elsewhere").click(function () {
        $(this).next(".learn-more-content.carrier-existing").hide(300);
        $(this).nextAll(".learn-more-content.carrier-elsewhere:first").show(300);
    });
}

function SetTabOrder() {
    $("#Header_ShortName").prop("tabIndex", 1);
    $("#Header_LegalName").prop("tabIndex", 10);
    $("#Header_Address1").prop("tabIndex", 20);
    $("#Header_Address2").prop("tabIndex", 30);
    $("#Header_City").prop("tabIndex", 40);
    $("#Header_State").prop("tabIndex", 50);
    $("#Header_Zip").prop("tabIndex", 60);
    $("#Header_ContractTerm1").prop("tabIndex", 70); //Term Length section
    $("#Header_ContractTerm2").prop("tabIndex", 71);
    $("#Header_ContractTerm3").prop("tabIndex", 72);

    $("#installationcost-custom").prop("tabIndex", 80);
    $("#installationcost-me").prop("tabIndex", 81); //Installation Fee & Credit Card Info sections
    $("#creditinfo").find("input").prop("tabIndex", 90);
    
    $("#LinesTrunksTable").find("input, select").prop("tabIndex", 90);
    $("#FeaturesTable").find("input, select").prop("tabIndex", 100);
    $("#FaxingTable").find("input, select").prop("tabIndex", 110);
    $("#PhoneNumbersTable").find("input, select").prop("tabIndex", 120);
    
    $("#phones-hide").prop("tabIndex", 130); //Phones Provider question
    $("#OtherPhonesTable").find("input").prop("tabIndex", 140);
    
    $("#PhonesTable").find("input, select").prop("tabIndex", 150);
    $("#OtherEquipmentTable").find("input, select").prop("tabIndex", 160);
    $("#NonEquipmentTable").find("input, select").prop("tabIndex", 170);
    
    $("#carrier-hide").prop("tabIndex", 180); //Carrier Services Provider question
    $("#CurrentCarrierTable").find("input").prop("tabIndex", 190);
    $("#OtherCarriersTable").find("input").prop("tabIndex", 200);
    
    $("#DialToneTable").find("input, select").prop("tabIndex", 210);
    $("#InternetAccessTable").find("input, select").prop("tabIndex", 220);
    $("#NetworkingTable").find("input, select").prop("tabIndex", 230);
    $("#FeesTable").find("input, select").prop("tabIndex", 240);
}

function VariableMRCPicklists() {
    //alert("VariableMRCPicklists");
    $(".QuoteTable select").each(function () {
        var list = $(this);
        var cell = $(this.parentNode);
        var elmId = this.nextElementSibling.id;
        var cell2 = document.getElementById(elmId);

        if (list.find(":selected").text().toLowerCase() == "variable") {
            cell.css("width", "128px");
            //CAC- should show always--> cell.find(":text").show();
        }
        else if (cell2.value == "") {
            cell2.value = list.find(":selected").text();
            //alert(("Id: " + elmId + " Value: " + elmId.valueOf() + "  Cell2: " + cell2.value));
            //cell2.text = list.find(":selected").text();
        }

        //cell.text = list.find(":selected").text();
        cell.find(":text").show();
        //cell.find(":value").show();
    });

//    $(".QuoteTable select").mouseup(function () {
//        var list = $(this);
//        var cell = $(this.parentNode);
//        var elmId = this.nextElementSibling.id;
//        var cell2 = document.getElementById(elmId);

//        //nextElementSibling: input#LinesTrunks_0__Variable


//        if (list.find(":selected").text().toLowerCase() == "variable") {
//            cell.css("width", "128px");
//            cell.find(":text").show();
//        } else {
//            cell.css("width", "75px");
//            //cell.text = list.find(":selected").text();
//            cell2.value = list.find(":selected").text();
//            cell.find(":text").show();
//            //cell2.show();
//        }
//    });
//    
    $(".QuoteTable select").change(function () {
        var list = $(this);
        var cell = $(this.parentNode);
        var elmId = this.nextElementSibling.id;
        var cell2 = document.getElementById(elmId);

        //nextElementSibling: input#LinesTrunks_0__Variable


        if (list.find(":selected").text().toLowerCase() == "variable") {
            cell.css("width", "128px");
            cell.find(":text").show();
        } else {
            cell.css("width", "75px");
            //cell.text = list.find(":selected").text();
            cell2.value = list.find(":selected").text();
            cell.find(":text").show();
        }
    });
}

function VariableMRCPicklists2(containerElement) {
    alert("smaller!");
    var container = $(containerElement);
    container.find(".QuoteTable select").each(function () {
        var list = $(this);
        var cell = $(this.parentNode);

        if (list.find(":selected").text().toLowerCase() == "variable") {
            cell.css("width", "128px");
            cell.find(":text").show();
        }
    });
    
    container.find(".QuoteTable select").change(function () {
        var list = $(this);
        var cell = $(this.parentNode);

        if (list.find(":selected").text().toLowerCase() == "variable") {
            cell.css("width", "128px");
            cell.find(":text").show();
        } else {
            cell.css("width", "75px");K
            cell.find(":text").hide();
        }
    }); 
}

function MoreLess() {
    $(".moreless-linesAndTrunks").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("LinesAndTrunks", quoteId, false);
            //if (html == "")
            //    DoLoginAgain();
            $("#LinesTrunks").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("LinesAndTrunks", quoteId, true);
            $("#LinesTrunks").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-features").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("Features", quoteId, false);
            $("#Features").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("Features", quoteId, true);
            $("#Features").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-faxing").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("Faxing", quoteId, false);
            $("#Faxing").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("Faxing", quoteId, true);
            $("#Faxing").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-phoneNumbers").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("PhoneNumbers", quoteId, false);
            $("#PhoneNumbers").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("PhoneNumbers", quoteId, true);
            $("#PhoneNumbers").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-phones").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("Phones", quoteId, false);
            $("#Phones").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("Phones", quoteId, true);
            $("#Phones").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-otherEquipment").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("OtherEquipment", quoteId, false);
            $("#OtherEquipment").html(html);
        }
        else {
            $(this).text("+ More");
           
            var html = $.LoadPartial("OtherEquipment", quoteId, true);
            $("#OtherEquipment").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-dialTone").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("DialTone", quoteId, false);
            $("#DialTone").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("DialTone", quoteId, true);
            $("#DialTone").html(html);
        }
        VariableMRCPicklists();
        return false;
    });


    $(".moreless-equipmentRental").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("EquipmentRental", quoteId, false);
            $("#EquipmentRental").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("EquipmentRental", quoteId, true);
            $("#EquipmentRental").html(html);
        }
        VariableMRCPicklists();
        return false;
    });


    $(".moreless-moreUsoc").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("MoreUsoc", quoteId, false);
            $("#MoreUsoc").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("MoreUsoc", quoteId, true);
            $("#MoreUsoc").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-internetAccess").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("InternetAccess", quoteId, false);
            $("#InternetAccess").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("InternetAccess", quoteId, true);
            $("#InternetAccess").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".moreless-networking").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("Networking", quoteId, false);
            $("#Networking").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("Networking", quoteId, true);
            $("#Networking").html(html);
        }
        VariableMRCPicklists();
        return false;
    });
}

function ExpandedData() {
    $("#morelines").click(function () {
        SaveQuote();
        CustomerId = CustomerId == null ? "0" : CustomerId;
        quoteId = quoteId == null ? "0" : quoteId;
        if ($(this).text() == "+ More") {
            $(this).text("- Less");
            var html = $.LoadPartial("LinesAndTrunks", quoteId, false);
            $("#LinesTruncks").html(html);
        }
        else {
            $(this).text("+ More");
            var html = $.LoadPartial("LinesAndTrunks", quoteId, true);
            $("#LinesTruncks").html(html);
        }
        VariableMRCPicklists();
        return false;
    });

    $(".another-number").click(function () {
        $(".remove-number").show();
        $(".another-number").hide();
        //VariableMRCPicklists(); //won't work here - load() is async !
        //Could we just reset a specific section? - VariableMRCPicklists2(this.parentNode)
        return false;
    });

    $(".another-phone").click(function () {
        $("#Phones").load("/Quote/PhoneSection", { QuoteId: quoteId, IsRecommended: false, IsLinkAction: true, IsLinkAction: true }, function (resp, stat, xhr) { VariableMRCPicklists(); }); 
        $(".remove-phone").show();
        $(".another-phone").hide();
        return false;
    });

    $(".another-equipment").click(function () {
        
        $("#OtherEquipment").load("/Quote/OtherEquipmentSection", { QuoteId: quoteId, IsRecommended: false, IsLinkAction: true }, function (resp, stat, xhr) { VariableMRCPicklists(); });
        $(".remove-equipment").show();
        $(".another-equipment").hide();
        return false;
    });
}
function NonExpandedData() {
    $(".remove-number").click(function () {
        
        $("#PhoneNumbers").load("/Quote/PhoneNumberSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true }, function (resp, stat, xhr) { VariableMRCPicklists(); });
        $(".another-number").show();
        $(".remove-number").hide();
        return false;
    });

    $(".remove-phone").click(function () {

        $("#Phones").load("/Quote/PhoneSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true, IsLinkAction: true }, function (resp, stat, xhr) { VariableMRCPicklists(); });
        $(".another-phone").show();
        $(".remove-phone").hide();
        return false;
    });

    $(".remove-equipment").click(function () {
        
        $("#OtherEquipment").load("/Quote/OtherEquipmentSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true }, function (resp, stat, xhr) { VariableMRCPicklists(); });
        $(".another-equipment").show();
        $(".remove-equipment").hide();
        return false;
    });
}

function AddNewRow() {
    $(".add-otherPhones").click(function () {
        var idx = $("#OtherPhonesTable tbody tr").length;
        var row = "<tr><td><input id='ManualPhones_IDX__ItemId' name='ManualPhones[IDX].ItemId' type='hidden' value='' /></td>\
                        <td><input id='ManualPhones_IDX__MakeModel' name='ManualPhones[IDX].MakeModel' type='text' /></td>\
                        <td><input id='ManualPhones_IDX__VendorName' name='ManualPhones[IDX].VendorName' type='text' /></td>\
                        <td><input id='ManualPhones_IDX__VendorEmail' name='ManualPhones[IDX].VendorEmail' type='text' /></td>\
                        <td><input id='ManualPhones_IDX__VendorPhone' name='ManualPhones[IDX].VendorPhone' type='text' /></td></tr>";
        $("#OtherPhonesTable tbody").append(row.replace(/IDX/g, idx));
        return false;
    });

    //These 3 may no longer be needed:
//CAC-    $(".add-dialTone").click(function () {
//        DialToneCount++;
//        
//        $("#DialTone").load("/Quote/DialToneSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true, Section: "Dial Tone", NumberOfElements: DialToneCount });
//        return false;
//        });
    $(".add-dialTone").click(function () {
        DialToneCount++;

        
        $("#DialTone").load("/Quote/DialToneSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true, Section: "Dial Tone", NumberOfElements: DialToneCount });
        return false;
    });
//    $(".add-internetAccess").click(function () {
//        InternetAccessCount++;
//   
//        $("#InternetAccess").load("/Quote/InternetAccessSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true, Section: "InternetAccess", NumberOfElements: InternetAccessCount });
//        return false;
//    });
//    $(".add-networking").click(function () {
//        NetworkingCount++;
//        
//        $("#Networking").load("/Quote/NetworkingSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true, Section: "Networking", NumberOfElements: NetworkingCount });
//        return false;
//    });

    $(".add-otherEquipment").click(function () {
        var idx = $("#OtherEquipment tbody tr").length;
        var row = "<tr><td style='display: none'><input id='OtherEquipment_IDX__ItemId' name='OtherEquipment[IDX].ItemId' type='hidden' value=''></td>\
                        <td><input id='OtherEquipment_IDX__Quantity' name='OtherEquipment[IDX].Quantity' type='text'></td>\
                        <td class='Description'><input id='OtherEquipment_IDX__Description' name='OtherEquipment[IDX].Description' type='text'></td>\
                        <td><input id='OtherEquipment_IDX__DealerCost' name='OtherEquipment[IDX].DealerCost' type='text'></td>\
                        <td><input id='OtherEquipment_IDX__Price' name='OtherEquipment[IDX].Price' type='text'></td></tr>";
        
        $("#OtherEquipment tbody").append(row.replace(/IDX/g, idx));
        return false;
    });

    $(".add-nonCityEquipment").click(function () {
        var idx = $("#NonEquipmentTable tbody tr").length;
        var row = "<tr><td style='display: none'><input id='NonEquipment_IDX__ItemId' name='NonEquipment[IDX].ItemId' type='hidden' value=''></td>\
                        <td><input id='NonEquipment_IDX__Quantity' name='NonEquipment[IDX].Quantity' type='text'></td>\
                        <td class='Description'><input id='NonEquipment_IDX__Description' name='NonEquipment[IDX].Description' type='text'></td>\
                        <td><input id='NonEquipment_IDX__DealerCost' name='NonEquipment[IDX].DealerCost' type='text'></td>\
                        <td><input id='NonEquipment_IDX__Price' name='NonEquipment[IDX].Price' type='text'></td></tr>";
        
        $("#NonEquipmentTable tbody").append(row.replace(/IDX/g, idx));
        //$("#NonCityEquipment").load("/Quote/NonCityEquipmentSection", { QuoteId: activeQuoteId, IsRecommended: true, IsLinkAction: true, Section: "Non Equipment", NumberOfElements: NonCityCount }, function (resp, stat, xhr) { VariableMRCPicklists(); }); //check if this is needed here
        return false;
    });

    $(".add-currentCarrier").click(function () {
        var idx = $("#CurrentCarrierTable tbody tr").length;
        var row = "<tr><td><input id='CurrentCarrier_IDX__ItemId' name='CurrentCarrier[IDX].ItemId' type='hidden' /></td>\
                        <td><input id='CurrentCarrier_IDX__Name' name='CurrentCarrier[IDX].Name' type='text' /></td>\
                        <td><input id='CurrentCarrier_IDX__Connection' name='CurrentCarrier[IDX].Connection' type='text' /></td>\
                        <td><input id='CurrentCarrier_IDX__Expiration' name='CurrentCarrier[IDX].Expiration' type='text' /></td></tr>";
        $("#CurrentCarrierTable tbody").append(row.replace(/IDX/g, idx));
        return false;
    });

    $(".add-otherCarriers").click(function () {
        var idx = $("#OtherCarriersTable tbody tr").length;
        var row = "<tr><td><input id='OtherCarriers_IDX__ItemId' name='OtherCarriers[IDX].ItemId' type='text' value='' /></td>\
                        <td><input id='OtherCarriers_IDX__Name'  name='OtherCarriers[IDX].Name' type='text' /></td>\
                        <td><input id='OtherCarriers_IDX__Contact'name='OtherCarriers[IDX].Contact' type='text' /></td>\
                        <td><input id='OtherCarriers_IDX__Email'  name='OtherCarriers[IDX].Email' type='text' /></td>\
                        <td><input id='OtherCarriers_IDX__Phone' name='OtherCarriers[IDX].Phone' type='text' /></td></tr>";
        $("#OtherCarriersTable tbody").append(row.replace(/IDX/g, idx));
        return false;
    });

    $(".add-fees").click(function () {
        var idx = $("#FeesTable tbody tr").length;
        var row = "<tr><td style='display: none;'><input id='Fees_IDX__ItemId' name='Fees[IDX].ItemId' type='hidden' value='WINSC'></td>\
                        <td><input id='Fees_IDX__Quantity' name='Fees[IDX].Quantity' type='text'></td>\
                        <td class='Description'><input id='Fees_IDX__Description' name='Fees[IDX].Description' type='text'></td>\
                        <td><input id='Fees_IDX__Monthly' name='Fees[IDX].Monthly' type='text'></td><td><input id='Fees_IDX__Install' name='Fees[IDX].Install' type='text'></td></tr>";
        
        $("#FeesTable tbody").append(row.replace(/IDX/g, idx));
        // $("#Fees").load("/Quote/FeesSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: true, Section: "Fees", NumberOfElements: FeesCount });
        return false;
    });
}

function SetAddRow() {
    $(".add-another").click(function () {
        $(".quote-form-lineS").clone().appendTo(".quote-form-lineS");
    });
    $(".qdadd-another").click(function () {
        $(".quote-form-lineQ").clone().appendTo(".quote-form-lineQ");
    });

    $(".epadd-another").click(function () {
        $(".quote-form-lineEP").clone().appendTo(".quote-form-lineEP");
    });
    $(".eoeadd-another").click(function () {
        $(".quote-form-lineEOE").clone().appendTo(".quote-form-lineEOE");
    });

    $(".nceadd-another").click(function () {
        $(".quote-form-lineNCE").clone().appendTo(".quote-form-lineNCE");
    });

    $(".ASFadd-another").click(function () {
        $(".quote-form-lineASF").clone().appendTo(".quote-form-lineASF");
    });
    $(".NETadd-another").click(function () {
        $(".quote-form-lineNET").clone().appendTo(".quote-form-lineNET");
    });

    $(".dtadd-another").click(function () {
        $(".quote-form-lineDT").clone().appendTo(".quote-form-lineDT");
    });

    $(".csadd-another").click(function () {
        $(".quote-form-lineCS").clone().appendTo(".quote-form-lineCS");
    });

    $(".IAadd-another").click(function () {
        $(".quote-form-lineIA").clone().appendTo(".quote-form-lineIA");
    });
}

function SetTotalsSidebar() {
    QuoteNameAutoComplete();
    SetStickBar();

    $("#savequote").click(function () {
        // Must have customer name
        if ($("#Header_LegalName").val() === "") {
            NoCustomerNameDialog();
            return;
        } //CAC- use customer name if quote name (shortname) is empty
        if ($("#Header_ShortName").val() === "") {
            $("#Header_ShortName").val(($("#Header_LegalName").val()));
        }

        SaveQuote();

    });

    $("#printquote").click(function () {
        if (quoteId > 0) {
            var path = "/reports/QuoteReport/?quoteId=" + quoteId;
            var win = window.open(path, "_blank", "status=no, menubar=no, titlebar=no");
            win.focus();
        }
        else
            alert("Quote needs to be saved for printing");
    });

    $("#emailquote").click(function () {
        var win = window.showModalDialog("/Quote/MailTo/?QuoteId=" + quoteId, "_blank", "status=no, menubar=no, titlebar=no, width=320, height=450");
    });

    $(".clone-quote").click(function () {
        if (quoteId == null || quoteId == 0)
            alert("Cannot clone this quote until is saved");
        else {
            quoteId = "0";
            quoteName = ""
            SetQuoteNameAndId();
            alert("Quote cloned!");
        }
    });

    $(".new-quote").click(function () {
//        if (quoteId > 0 || $("#Header_LegalName").val() != "") {
//            QuoteNotSavedDialog();
//            return;
//        }
        
        quoteId = "0";
        quoteName = "";
        LoadQuote();
    });
}

function QuoteNameAutoComplete() {
    $("#Header_ShortName").autocomplete({
        source: function (request, response) {
            var _criteria = $("#Header_ShortName").val();
            var obj = $.getQuotesNames(_criteria);
            response(obj);
        },
        focus: function (event, ui) {
            this.value = ui.item.label;
            event.preventDefault(); // Prevent the default focus behavior. 
        },
        select: function (event, ui) {
            if (ui.item) {
                event.preventDefault(); //allow me to update the text on the textbox
                quoteId = ui.item.value;
                quoteName = ui.item.label;
                LoadQuote(ui.item.value, ui.item.label);
                cloneCounter = 0;
            }
        },
        change: function () {
        }
    });
}

function SetStickBar() {
    var msie6 = $.browser == 'msie' && $.browser.version < 7;
    if (!msie6) {
        if ($('#quote-sidebar').offset() == null)
            return;

        var top = $('#quote-sidebar').offset().top - parseFloat($('#quote-sidebar').css('margin-top').replace('auto', 0));
        $(window).scroll(function (event) {
            var y = $(this).scrollTop();
            if (y >= top) {
                $('#quote-sidebar').addClass('fixed');
            } else {
                var obj = $('#quote-sidebar');
                if (obj != undefined)
                    obj.removeClass('fixed');
            }
        });
    }  
}

// --- Dialogs ----------

function QuoteNotSavedDialog() {
    var abort = function() { $(this).dialog("close"); }
    var getNewQuote = function() { $(this).dialog("close"); quoteId = "0"; quoteName = ""; LoadQuote(); }
    var setDefaultButton = function() { $(this).parent().find(".ui-dialog-buttonpane button:eq(1)").focus(); }
    var options = { width: 475, height: 'auto', modal: true, title: "Warning", buttons: { Yes: getNewQuote, No: abort}, open: setDefaultButton }
    $("#QuoteAlertMSG").html("<br/><p>There may be data to be saved. Create new quote without saving?</p><br/>").css("text-align", "center");
    $("#QuoteAlertMSG").dialog(options);
}

function NoQuoteNameDialog() {
    var options = { width: 300, height: "auto", modal: true, title: "Save", buttons: { OK: function() { $(this).dialog("close"); } } };
    $("#QuoteAlertMSG").html("<br/><p>The Quote Name can't be empty.</p><br/>").css("text-align", "center");
    $("#QuoteAlertMSG").dialog(options);
}

function NoCustomerNameDialog() {
    var options = { width: 300, height: "auto", modal: true, title: "Save", buttons: { OK: function () { $(this).dialog("close"); } } };
    $("#QuoteAlertMSG").html("<br/><p>The Customer Name can't be empty.</p><br/>").css("text-align", "center");
    $("#QuoteAlertMSG").dialog(options);
}

// --- END: VISUAL ELEMENTS CODE ----------------------------------------------------


function SetQuoteNameAndId() {
    $("#quote-id").text(quoteId);
    $("#Header_ShortName").val(quoteName);
}

function SaveQuote() {
    try {
        ShowLoading("Saving...");
        var form = $("form");
        var data = form.serialize();
        var result = $.SaveOrder(data);
        if (result && result.errors) {
            if (result.errors.length > 0) {
                HideLoading();
                alert(result.errors[0]);

            }
        }
        quoteId = $.getValue(result, 0, "id");
        quoteName = $.getValue(result, 0, "shortname");
        CustomerId = $.getValue(result, 0, "customer");
        $("#Header_Customer").val(CustomerId);
        LoadQuoteTotals();
        SetTotalsSidebar();
        HideLoading();
        //alert("Quote Saved!");
    } catch (e) {
        alert(e);
    }
}

//We use quoteId, and quoteName as a global var.
function LoadQuote() {
    LoadQuoteHeader();
    LoadQuoteDetail();
    LoadQuoteTotals();
    SetVisualElements();
}

function LoadQuoteHeader() {
    var html = $.getQuoteHeader(quoteId, quoteName);
    if (html != "") {
        $("#Quote_CustomerInfo").html(html);
    }
}

function LoadQuoteDetail() {
    var html = $.getQuoteDetail(quoteId, quoteName);
    if (html != "") {
        $("#Quote_Detail").html(html);
    }
}

function LoadQuoteTotals() {
    var html = $.getQuoteTotals(quoteId, quoteName);
    if (html != "") {
        $("#Total").html(html);
    }
    $("#totalretail").text($("#Header_TotalOneTime").val());
    $("#totaldealer").text($("#Header_TotalDealerOneTime").val());
}

function LoadTotals(quoteId, quoteName) {
    $("#OtherTotalsData").load("/Quote/LoadOtherTotalsData", { UseMain: true });
    $("#TotalValueArea").load("/Quote/LoadTotalValueArea", { UseMain: true });
    $("#QuoteIdSection").load("/Quote/LoadQuoteIdSection", { UseMain: true });
}

//function LoadDetails(quoteId, quoteName) {
//    $("#LinesTruncks").load("/Quote/LinesAndTrunksSection", { UseMain: true });
//    $("#Features").load("/Quote/FeatureSection", { UseMain: true });
//    $("#Faxing").load("/Quote/FaxingSection", { UseMain: true });
//    $("#Phones").load("/Quote/PhoneSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
//    $("#PhoneNumbers").load("/Quote/PhoneNumberSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
//    $("#OtherEquipment").load("/Quote/OtherEquispmentSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
//    $("#InternetAccess").load("/Quote/InternetAccessSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false, Section: "InternetAccess", NumberOfElements: 0 });
//    $("#DialTone").load("/Quote/DialToneSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false, Section: "Dial Tone", NumberOfElements: 0 });
//    $("#Networking").load("/Quote/NetworkingSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false, Section: "Networking", NumberOfElements: 0 });
//    $("#Fees").load("/Quote/FeesSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false, Section: "Fees", NumberOfElements: 0 });
//    $("#NonCityEquipment").load("/Quote/NonCityEquipmentSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false, Section: "Non Equipment", NumberOfElements: 0 });
////    $("#NonCityEquipment").load("/Quote/NonCityEquipmentSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
////    $("#DialTone").load("/Quote/DialToneSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
////    $("#InternetAccess").load("/Quote/InternetAccessSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
////    $("#Networking").load("/Quote/NetworkingSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
////    $("#Fees").load("/Quote/FeesSection", { QuoteId: quoteId, IsRecommended: true, IsLinkAction: false });
//}


function CleanFields() {
    quoteName = "";
    quoteId = "0";
    LoadQuote();
}

function CleanHeader() {
    $("#Header_Name").val("");
    $("#Header_LegalName").val("");
    $("#Header_City").val("");
    $("#Header_Address1").val("");
    $("#Header_State").val("");
    $("#Header_Zip").val("");
    $("#Header_Address2").val("");
    $("#Header_ShortName").val("");
}

function LoadClient(customerid) {
    var obj = $.getClientInfo(customerid);
    $("#Header_Name").val($.getValue(obj, 0, "legalname"));
    $("#Header_City").val($.getValue(obj, 0, "city"));
    $("#Header_Address1").val($.getValue(obj, 0, "address1")    );
    $("#Header_State").val($.getValue(obj, 0, "state"));
    $("#Header_Zip").val($.getValue(obj, 0, "zip"));
    $("#Header_Address2").val($.getValue(obj, 0, "address2"));
    $("#Header_Customer").val(customerid);
}

function CustomerExist(QuoteData) {
    var cont = 0;
    jQuery.each(QuoteData.table, function (i) {
        cont++;
    }); 
    if (cont > 0)
        return true;
    return false;
}

function OrderExist(QuoteData) {
    var cont = 0;
    jQuery.each(QuoteData.table, function (i) {
        cont++;
    });
    if (cont > 0)
        return true;
    return false;
}

function CustomerId(CustomerTable) {
    if (CustomerTable.table[0][0].value == null)
        _customerId = "";
    else
        _customerId = CustomerTable.table[0][0].value;
}

function NoOrdersPopUp(id) {
    var cancel = function () { $(this).dialog("close"); }
    var neworder = function () { $("#CustomerId").val(id); $("#Quote_Body").load("/Quote/PartialDetail", { QuoteId: 0 }); $(this).dialog("close"); }
    var okDialogOpt = { width: 475, height: 'auto', autoResize: true, modal: true, buttons: { "New Order": neworder, "Cancel": cancel} }
    $("#orderItems").html("No orders found for this user").css("text-align", "center");
    $("#orderItems").dialog(okDialogOpt);
}

function CustomerInformation(Id, DataInfo, IsRecommended) {
    if (Id > 0) {
        _customerId = DataInfo.table[0][0].value;
        _name = DataInfo.table[0][1].value;
        _address = DataInfo.table[0][2].value;
        _state = DataInfo.table[0][4].value;
        _city = DataInfo.table[0][3].value;
        _zip = DataInfo.table[0][5].value;
        $("#CustomerId").val(_customerId);
        $("#QuoteId").val(Id.toString());
        $("#Name").val(_name.toString());
        $("#Address").val(_address);
        $("#City").val(_city);
        $("#State").val(_state);
        $("#Zip").val(_zip);
    }
    else {
        _customerId = "0";
        _name = $("#Name").val();
        _address = $("#Address").val();
        _state = $("#State").val();
        _city = $("#City").val();
        _zip = $("#Zip").val();
    }
    $("#Quote_Body").load("/Quote/DetailPage", { QuoteId: Id, IsRecommended: IsRecommended }); //end of load partial detail   
}

//-------------------------------END OF VALIDATION AREA---------------------------------------------------------------------------


//* ----------------------------------------------------- THIS IS FOR SAVING THE DATA ----------------------------------------------------------- */
function SaveOrderDetail(JsonStr) {
    $.UpdateOrderDetail(JsonStr);
}


function LoadHeaderPage(quoteid, isrecommended) {
    html = $.getHeaderPage(quoteid, isrecommended);
    $("#QuotePage").html(html);
}

function LoadDetailPage(quoteid, isrecommended) {
    html = $.getDetailPage(quoteid, isrecommended);
    $("Quote_Detail").html(html);
}

function GetQuoteId() {
    var _criteria = $("#Header_ShortName").val();
    var obj = $.getQuotesNames(_criteria);
    for (var i = 0; i < obj.length; i++) {
        if (obj[i].label == _criteria) {
            _quoteId = obj[i].value;
            return true;
        }
    }
    _quoteId = 0;
    return false;
}

jQuery.extend({
    ResetCounters: function (reset) {
    if (reset) {
        InternetAccessCount = 0;
        DialToneCount = 0;
        NetworkingCount = 0;
        FeesCount = 0;
        NonCityCount = 0;
        }
    return true;
    }
});

function ShowLoading(msg) {
    $("#loadingmsg #msg").text(msg);
    $("#loadingmsg").css("display", "visible");
    
}
function HideLoading() {
    $("#loadingmsg").css("display", "none");
}