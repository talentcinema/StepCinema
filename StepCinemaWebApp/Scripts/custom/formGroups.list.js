
var formGroupTableRactive = Ractive({
    el: "#formGrouptable",
    template: "#fulltable",
    data: {
        tabledata: null
    },
    on: {
        "onAdd": function (context) {
            window.alert("Add");
        },
        "onEdit": function (context, value, index) {
            window.alert(value.formGroupName);
        },
        "onDelete": function (context, value, index) {
            window.alert(value.formGroupName);
        },
        "onFilter": function (context, values) {
            var args = JSON.parse(JSON.stringify(this.get("tabledata.arguments")));
            args.filter = values;
            reloadFormGroupListData(args);
        },
        "datatable.onReload": function (context, arguments) {
            reloadFormGroupListData(arguments);
        }
    }
});


function reloadFormGroupListData(args) {
    var args = args || {
        "filter": {
            "formGroupId": "",
            "formGroupName": ""
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
        formGroupTableRactive.set("tabledata", data);
        console.log(JSON.stringify(data));
    });
}

$(document).ready(function () {
    reloadFormGroupListData(null);
});