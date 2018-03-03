var startRactive = Ractive({
    template: "#startTemplate",
    el: "#startArea",
    data: {
        data: {
            studyNumber: "",
            siteId: "",
            subjectId: "",
            screeningNo: "",
            registeredNo: "",
            protocolTitle: ""
        }
    },
    on: {
        "onClick": function (context) {
            console.log("click fired");
            var data = this.get("data");
            SaveData(data);
        }
    }
});

function SaveData(args) {
    args.detailId = $("#startArea").data("detail");
    $.ajax({
        url: window.CRFBaseUrl + "CRFForm/SaveStart",
        type: "POST",
        data: JSON.stringify({ "model": args }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        console.log(JSON.stringify(data));
        screen = $("#startArea");
        if (data.model && data.model.status === 1) {
            window.location.href = window.CRFBaseUrl + "CRFForm/Page/"
                + screen.data("next")
                + "?detail=" + data.model.detailId
                + "&period=" + screen.data("period");
        }
    });
}