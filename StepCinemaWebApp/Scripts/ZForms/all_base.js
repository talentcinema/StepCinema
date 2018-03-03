Ractive.components.acmultiple = Ractive.extend({
    template: "#acmultipletemplate",
    data: {
        field: null,
        lovs: null,
        getLov: function (a) {
            var x = 'lovs.' + a + ".items";
            return (this.get(x) || null);
        }
    },
    computed: {
        difference: function () {
            return this.get("field.compareValue") && this.get("field.value") && this.get("field.compareValue") !== this.get("field.value");
        },
        formValue: {
            get : function() {
                var val = this.get("field.value");
                if (val) { return val.split(","); }
                else { return []; }
            },
            set: function (v) {
                this.set('field.value', Array.isArray(v) ? v.join(',') : ((v !== null && v !== undefined) ? v.toString() : v));
                //console.log(this.get('value'));
            }
        }
    }
});


Ractive.components.acinput = Ractive.extend({
    data: {
        field: null,
        lovs: null,
        getLov: function (a) {
            var x = 'lovs.' + a + ".items";
            return (this.get(x) || null);
        },
        isdirty: false
    },
    computed : {
        difference: function () {
            return this.get("field.compareValue") && this.get("field.value") && this.get("field.compareValue") !== this.get("field.value");
        }
    },
    template: "#acinputtemplate",
    observe: {
        "field.value": function (newValue, oldValue, path) {
            var type = this.get('field.type');
            var required = this.get('field.required');
            var isdirty = false;
            switch (type) {
                case "date": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (moment(newValue, "D-MMM-YYYY", true).isValid()) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "datetime": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (moment(newValue, "D-MMM-YYYY h:mm a", true).isValid()) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "time": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (moment(newValue, "h:mm a", true).isValid()) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "text": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (newValue !== "" && newValue !== null && newValue !== undefined) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "string": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (newValue !== "" && newValue !== null && newValue !== undefined) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "int": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (/^\d{0,10}$/.test(newValue)) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "float": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if (/^\d{0,10}(\.\d{0,5})?$/.test(newValue)) { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "list": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if ((newValue || "") !== "") { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "list-radio": {
                    if ((!required) && ((newValue || "") === "")) { isdirty = false; }
                    else if ((newValue || "") !== "") { isdirty = false; }
                    else { isdirty = true; }
                    break;
                }
                case "multiple": {
                    break;
                }
                case "static": {
                    break;
                }
                default: {
                    console.log((type || "") + " : invalid");
                    break;
                }
            }
            if (this.get("isdirty") !== isdirty) {
                this.set("isdirty", isdirty);
            }
        }
    }
});