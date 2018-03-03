
var userTableRactive = Ractive({
    el: "#userstable",
    template: "#fulltable",
    data: {
        tabledata: null
    },
    on: {
        "onAdd": function (context) {
            window.alert("Add");
        },
        "onEdit": function (context, value, index) {
            window.alert(value.firstName);
        },
        "onDelete": function (context, value, index) {
            window.alert(value.firstName);
        },
        "onFilter": function (context, values) {
            var args = JSON.parse(JSON.stringify(this.get("tabledata.arguments")));
            args.filter = values;
            reloadUserListData(args);
        },
        "datatable.onReload": function (context, arguments) {
            reloadUserListData(arguments);
        }
    }
});


function reloadUserListData(args) {
    var args = args || {
        "filter": {
            "firstName": "",
            "lastName": "",
            "active" : "Y"
        },
        "sort": {
            "field": "",
            "direction": ""
        },
        "pagination": {
            "pageSize": 30,
            "currentPage": 1,
            "maxPages": 10,
            "count" : 0
        }
    }

    $.ajax({
        url: "GetListJson",
        type: "POST",
        data: JSON.stringify({ "model": args }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        userTableRactive.set("tabledata", data);
        console.log(JSON.stringify(data));
    });
}

$(document).ready(function () {
    reloadUserListData(null);
});