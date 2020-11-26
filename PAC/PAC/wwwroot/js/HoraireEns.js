
//scheduler.config.xml_date = "%Y-%m-%d %H:%i";
scheduler.form_blocks["my_editor"] = {
    render: function (sns) {
        return "<div class='dhx_cal_ltext' style='height:70px;'>" +
            "Nom Cours&nbsp;<input name='text' type='text'><br/>" +
            "Local&nbsp;<input name='location' type='text'></div>";
    },
    set_value: function (node, value, ev) {

        node.querySelector("[name='text']").value = value || "";
        node.querySelector("[name='location']").value = ev.descrip || "";
    },
    get_value: function (node, ev) {
        ev.descrip = node.querySelector("[name='location']").value;
        return node.querySelector("[name='text']").value;
    },
    focus: function (node) {
        var input = node.querySelector("[name='text']");
        input.select();
        input.focus();
    }
};
scheduler.templates.event_class = function (start, end, ev) {
    return "prof_event";
};

scheduler.locale.labels.section_description = "D\u00e9tails";

scheduler.config.lightbox.sections = [
    { name: "description", height: 100,map_to: "text", type: "my_editor", focus: true },
    { name: "time", height: 72, type: "time", map_to: "auto" }
];

scheduler.ignore_week = function (date) {
    if (date.getDay() == 6 || date.getDay() == 0) //hides Saturdays and Sundays
        return true;
};
scheduler.config.first_hour = 8;
scheduler.config.last_hour = 22;
scheduler.locale.labels.new_event = "Nom du Cours";

scheduler.init("scheduler_here");

// load data from backend
scheduler.load("/api/events/", "json");
// connect backend to scheduler
var dp = new dataProcessor("/api/events/");
dp.init(scheduler);
// set data exchange mode
dp.setTransactionMode("REST", true);