
var CRFStatusTableRactive = Ractive({
    el: "#crfstatustable",
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
            reloadCRFStatusListData(args);
        },
        "datatable.onReload": function (context, arguments) {
            reloadCRFStatusListData(arguments);
        }
    }
});


function reloadCRFStatusListData(args) {
    var studyId = $("#crfstatustable").data("study");
    var args = args || {
        "filter": {
            "studyId": studyId,
            "subjectId": "",
            "statusId": "-1",
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

    args.filter.studyId = studyId;

    $.ajax({
        url: window.CRFBaseUrl + "CRFStatus/GetListJson",
        type: "POST",
        data: JSON.stringify({ "model": args }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        CRFStatusTableRactive.set("tabledata", data);
        console.log(JSON.stringify(data));
    });
}

$(document).ready(function () {
    reloadCRFStatusListData(null);
});