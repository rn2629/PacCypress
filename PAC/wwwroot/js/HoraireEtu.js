
scheduler.config.xml_date = "%Y-%m-%d %H:%i";
scheduler.form_blocks["my_editor"] = {
    render: function (sns) {
        return "<div class='dhx_cal_ltext' style='height:75px;'>" +
            "&nbsp;<input name='text' type='number' max='3' min='1' placeholder='Entre 1 et 3'><br/>"+
            "<p style='color:red;'>3 est le PLUS Prioritaire</p><br/>";
    },
    set_value: function (node, value, ev) {
        node.querySelector("[name='text']").value = value;
    },
    get_value: function (node, ev) {
        ev.text = node.querySelector("[name='text']").value.toString();
        ev.priority = node.querySelector("[name='text']").value;
        return node.querySelector("[name='text']").value;
    },
    focus: function (node) {
        var input = node.querySelector("[name='text']");
        input.select();
        input.focus();
    }
};
scheduler.config.day_date = "%D";
var format = scheduler.date.date_to_str(scheduler.config.day_date);
scheduler.templates.week_scale_date = function (date) {
    return format(date);
};
scheduler.locale.labels.section_description = "Priorit\u00e9";
scheduler.locale.labels.new_event = "Priorit\u00e9 de la disponibilit\u00e9";
scheduler.config.lightbox.sections = [
    { name: "description", map_to: "priority", type: "my_editor", focus: true },
    { name: "time", height: 72, type: "time", map_to: "auto" }
];

scheduler.ignore_week = function (date) {
    if (date.getDay() == 6 || date.getDay() == 0) //hides Saturdays and Sundays
        return true;
};
scheduler.config.first_hour = 8;
scheduler.config.last_hour = 22;
scheduler.templates.event_text = function (start, end, event) {
    return "Priorit\u00e9  : " + event.priority;
}

scheduler.config.details_on_create = true;
scheduler.init("scheduler_here", new Date(2020, 8, 1), "week");

// load data from backend
scheduler.load("/api/eventsEtu", "json");
// connect backend to scheduler
var dp = new dataProcessor("/api/eventsEtu");
dp.init(scheduler);
// set data exchange mode
dp.setTransactionMode("REST", true);