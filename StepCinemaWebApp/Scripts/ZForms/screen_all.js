var ractivePageFormGroup = Ractive({
    template: "#templateforpage",
    data: {
        page: null,
        field: function (fieldName, index, formName) {
            return this.get("page.formGroup." + formName + ".items." + index + "." + fieldName);
        },
        text: function (fieldName, formName) {
            return this.get("page.formGroup." + formName + ".head." + fieldName + ".text");
        },
        form: function (formName) {
            return this.get("page.formGroup." + formName);
        },
        items: function (formName) {
            return this.get("page.formGroup." + formName + ".items");
        },
        order: function (formName) {
            return this.get("page.formGroup." + formName + ".fieldOrder");
        }
    },
    el: "#formgroupcontent"
});

function GetParametersFromPage() {
    var $para = $("#parameters");
    var json1 = $para.html();
    console.log(json1);
    var obj = JSON.parse(json1);
    console.log(obj);
    return obj;
}
function HandleSubjectBox(obj) {
    compText = "";
    compLabel = "CompareId";
    $("#compareDiv").hide();
    switch ($("#templateforpage").data("tag")) {
        case 'sub': {
            compText = obj.subjectId;
            compLabel = "Subject Id : ";
            break;
        }
        case 'reg': {
            compText = obj.registeredNo;
            compLabel = "Registration Number : ";
            break;
        }
        case 'scr': {
            compText = obj.screeningNo;
            compLabel = "Screening Number : ";
            break;
        }
        default: {
            compText = "";
            compLabel = "Compare Id : ";
            break;
        }
    }
    $("#compareText").html(compLabel);
    $("#compareId").attr("data-compare", compText);
    $("#compareId").val("");
    if (compText != "") {
        $("#compareDiv").show();
    }
}

function GetScreenParameters() {
    var obj = GetParametersFromPage();
    $.ajax({
        url: window.CRFBaseUrl + "CRFForm/GetDynamicFormJson",
        type: "POST",
        data: JSON.stringify({ "model": obj }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        var a = JSON.stringify(data, null, 2);
        console.log(data);
        ractivePageFormGroup.set("page", data);
        HandleSubjectBox(obj);
        var b = JSON.stringify(data, null, 2);
        console.log(a === b);
    });
}

function CRFAlert(text, cssClass) {
    var $div = $('<div class="crf-alert" />').text(text).addClass(cssClass).css({
        "position": "fixed",
        "bottom": "0",
        "left": "0",
        "right": "0",
        "z-index": "10000",
        "text-align": "right"
    }).appendTo("body");
    setTimeout(function () {
        $div.remove();
    }, 3000);
}
function SaveScreenParameters() {
    if ($("#compareId").val() !== $("#compareId").attr("data-compare")) {
        CRFAlert($("#compareText").text() + " is not same. Can't Save", "text-danger bg-danger");
        return;
    }
    var obj = GetParametersFromPage();
    var dataObj = {};
    dataObj.model = obj;
    dataObj.page = ractivePageFormGroup.get("page");

    if (dataObj.page) {
        $.ajax({
            url: window.CRFBaseUrl + "CRFForm/SaveInfo",
            type: "POST",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (data) {
            console.log(data);
            window.location.href = $("#nextbutton").attr("href");
        });

    }

}

$(document).ready(function () {
    GetScreenParameters();
    $("#savebutton").click(function () {
        SaveScreenParameters();
    });
});

