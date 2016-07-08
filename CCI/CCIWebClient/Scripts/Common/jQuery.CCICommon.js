
jQuery.extend({
    getOrderScreen: function (securityid, newtableformat, orderid) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getOrderScreen",
            data: { securityid: securityid, newtableformat: newtableformat, orderid: orderid },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    getOrderDetail: function (orderid) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getOrderDetail",
            data: { orderid: orderid },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    getOrderHeader: function (orderid, ordername) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getOrderHeader",
            data: { orderid: orderid, ordername: ordername },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});



jQuery.extend({
    getOrderTotal: function (orderid) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getOrderTotal",
            data: { orderid: orderid },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    getCustomerSuggestions: function (name, address, city, state, zip, dealer) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getCustomerSuggestions",
            data: { name: name, address: address, city: city, state: state, zip: zip, dealer: dealer },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

jQuery.extend({
    getUserType: function () {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getUserType",
            data: {},
            type: "get",
            dataType: "text",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

 

jQuery.extend({
    UpdateOrderDetail: function (jsonstr) {
        var obj = null;
        try {
            $.ajax({

                url: "/CCICommon/updateOrderDetailRecordJson",
                type: "POST",
                data: { json: jsonstr },
                // dataType: "json",
                //"Content-type", "application/x-www-form-urlencoded",
                datatype: "application/x-www-form-urlencoded",
                dataType: "json",
                cache: false,
                timeout: 30000,
                async: false,
                success: function (data) {
                    obj = eval('(' + data + ')');
                },
                error: function (err) {
                    alert(err);
                }
            });
        } catch (e) {
            alert(e);
        }
        return obj;
    } // end function
});  //end jquery

jQuery.extend({
    getOrderHeaderHtml: function (orderId, name) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/GetSuggestedCustomer",
            data: { orderId: orderId, name: name},
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    getQuotesNames: function (criteria) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/GetQuoteName",
            data: { criteria: criteria },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                var data = eval('(' + data + ')');
                 myobjc = $.map(data.table.rows, function (item) {
                    return {
                        label: item[1],
                        value: item[0]
                    };
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    getClients: function (criteria) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getClient",
            data: { criteria: criteria },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                var data = eval('(' + data + ')');
                myobjc = $.map(data.table.rows, function (item) {
                    return {
                        label: item[1],
                        value: item[0]
                    };
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

jQuery.extend({
    getClientInfo: function (customerid) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getCustomerInfo",
            data: { customerid: customerid },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
             },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    SaveOrder: function (data) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/SaveOrder",
            data: data,
            type: "post",
            async: false,
            cache: false,
            success: function (data) {
                myobjc = eval('(' + data + ')');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

jQuery.extend({
    getValue: function (ob, row, col) {
        var obj;
        if (ob.results != undefined)
            obj = ob.results;
        else
            obj = ob;
        for (var i = 0; i < obj.table.columns.length; i++) {
            if (obj.table.columns[i] == col)
                return obj.table.rows[row][i];
        }

    }
});


jQuery.extend({
    getQuoteHeader: function (quoteid, quoteName) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/getQuoteHeader",
            data: { quoteid: quoteid, quoteName: quoteName },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    LoadPartial: function (partialname, quoteid, isrecommended) {
        var myobjc = null;
        var url = "/Quote/LoadPartial";
        $.ajax({
            url: url,
            data: { partialName: partialname, quoteId: quoteid, isRecommended: isrecommended },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            statusCode: { 302: function () { LoginAgain(); } },

            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});
jQuery.extend({
    getQuoteDetail: function (quoteId, quoteName) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/getQuoteDetail",
            data: { quoteId: quoteId, quoteName: quoteName },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});


jQuery.extend({
    getQuoteTotals: function (quoteid, quoteName) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/getOrderTotals",
            data: { Quoteid: quoteid, quoteName: quoteName },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

jQuery.extend({
    getDetailPage: function (quoteid, isrecommended) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/DetailPage",
            data: { Quoteid: quoteid, isRecommended: isrecommended },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

jQuery.extend({
    getHeaderPage: function (quoteid, ordername) {
        var myobjc = null;
        $.ajax({
            url: "/CCICommon/getOrderHeader",
            data: { Quoteid: quoteid, ordername: ordername },
            type: "get",
            dataType: "json",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

jQuery.extend({
    getTotalPage: function (quoteid, isrecommended) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/TotalPage",
            data: { Quoteid: quoteid, isRecommended: isrecommended },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});



jQuery.extend({
    getSuggestedCustomers: function (name, address, suite, city, state, zip) {
        var myobjc = null;
        $.ajax({
            url: "/Quote/GetSuggestedCustomers",
            data: { Name: name, Address: address, Suite: suite, City: city, State: state, Zip: zip },
            type: "get",
            dataType: "html",
            timeout: 30000,
            async: false,
            cache: false,
            success: function (data) {
                myobjc = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
        return myobjc;
    }
});

function LoginAgain() {
    window.location.href = window.location.protocol + "//" + window.location.host + "/";
}