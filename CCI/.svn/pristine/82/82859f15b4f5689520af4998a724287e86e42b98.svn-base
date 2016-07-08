/// <reference path="/Scripts/jquery-1.8.0.min.js" />
/// <reference path="/Scripts/jquery-ui-1.8.11.js" />
/// <reference path="/Scripts/Common/jQuery.CCICommon.js" />
/// <reference path="/Scripts/Common/jQuery.ui.ATK.js" />

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

try {
    $(document).ready(function () {
        LoadSelectCustomer();
    });                    // document.ready
} catch (e) {
    alert(e);
}


function LoadSelectCustomer() {
    $("#ValidateInfobtn").click(function () {
        var name = $("#Name").val();
        if (name == "") {
            alert("Please enter a valid customer name");
        }
        else {
            _userType = "dealer"; 
            _customerData = $.getCustomerSuggestions($("#Name").val(), $("#Address").val(), $("#City").val(), $("#State").val(), $("#Zip").val(), _userType);
            var headerOrders = $.getOrderHeader("1", $("#Name").val());
            var html = $.getOrderHeaderHtml("1", $("#Name").val());
            if (!CustomerExist(_customerData)) {
                $("#orderItems").html("No orders found for this user").css("text-align", "center");
                $("#orderItems").dialog({ width: 475, height: 'auto', buttons: {
                    "New Order": function () {
                        _isRecommended = true;
                        CustomerInformation(0, _customerData, _isRecommended);
                        $(this).dialog("close");
                    },
                    "Cancel": function () { $(this).dialog("close"); }
                }
                });
                $("#orderItems").dialog('open');


            }
            else {
                if (OrderExist(headerOrders)) {
                    $("#orderItems").html(html).css("text-align", "center");
                    $("#orderItems").dialog({ width: 475, height: 'auto', buttons: {
                        "New Order": function () {
                            _isRecommended = true;
                            CustomerInformation(0, _customerData, _isRecommended);
                            $(this).dialog("close");
                        },
                        "Cancel": function () { $(this).dialog("close"); }
                    }
                    });
                    $("#orderItems").dialog('open');
                    $("#orderItems input").click(function () {
                        var id = $(this).attr("id");
                        _isRecommended = true;
                        CustomerInformation(id, _customerData, _isRecommended);
                        $(this).closest(".ui-dialog-content").dialog("close");
                    }); // end of input click
                }
                else {
                    CustomerId(_customerData);
                    NoOrdersPopUp(_customerId);
                }
            } //end first nested else
        } //end first else
    });   //end validate info button click

}//end function

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
    var okDialogOpt = { width: 475, height: 'auto', autoResize: true, modal: true, buttons: { "New Order": neworder,"Cancel": cancel} }
    $("#orderItems").html("No orders found for this user").css("text-align", "center");
    $("#orderItems").dialog(okDialogOpt);
}

function CustomerInformation(Id, DataInfo, IsRecommended) {
    if(Id > 0) {
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
    $("#Quote_Body").load("/Quote/PartialDetail", { QuoteId: Id, IsRecommended: IsRecommended }); //end of load partial detail   
}

