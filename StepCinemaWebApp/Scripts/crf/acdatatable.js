Ractive.components.datatable = Ractive.extend({
    template: "#actablescript",
    data: { tabledata: null },
    on: {
        "onSort": function (context, name) {
            var args = JSON.parse(JSON.stringify(this.get("tabledata.arguments")));
            var isDirty = false;
            if (args && args.sort) {
                if (args.sort.field === name && args.sort.direction === 'ASC') {
                    args.sort.direction = 'DESC';
                    isDirty = true;
                }
                else if (args.sort.field === name) {
                    args.sort.field = '';
                    args.sort.direction = '';
                    isDirty = true;
                }
                else {
                    args.sort.field = name;
                    args.sort.direction = 'ASC';
                    isDirty = true;
                }
            }

            if (isDirty) {
                console.log("fired");
                this.fire("onReload", context, args);
            }
        },
        "onPage": function (context, page) {
            var args = JSON.parse(JSON.stringify(this.get("tabledata.arguments")));
            var isDirty = false;
            if (args && args.pagination && parseInt(args.pagination.currentPage, 10) !== parseInt(page, 10)) {
                args.pagination.currentPage = page;
                isDirty = true;
            }
            if (isDirty) {
                console.log("fired");
                this.fire("onReload", context, args);
            }
        },
        "onSizeChange": function (context) {
            var args = JSON.parse(JSON.stringify(this.get("tabledata.arguments")));
            if (args) {
                console.log("fired");
                this.fire("onReload", context, args);
            }
        }
    }

});
