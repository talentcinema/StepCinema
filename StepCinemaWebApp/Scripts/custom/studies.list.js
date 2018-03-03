
var studyTableRactive = Ractive({
    el: "#studeistable",
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
            reloadStudyListData(args);
        },
        "datatable.onReload": function (context, arguments) {
            reloadStudyListData(arguments);
        }
    }
});


function reloadStudyListData(args) {
    var args = args || {
        "filter": {
            "studyNumber": "",
            "studyName": "",            
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
        url: window.CRFBaseUrl + "Studies/GetListJson",
        type: "POST",
        data: JSON.stringify({ "model": args }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        studyTableRactive.set("tabledata", data);
        console.log(JSON.stringify(data));
    });
}

$(document).ready(function () {
    reloadStudyListData(null);
});