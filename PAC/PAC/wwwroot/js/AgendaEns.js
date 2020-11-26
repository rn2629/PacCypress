
scheduler.config.xml_date = "%Y-%m-%d %H:%i";
scheduler.form_blocks["my_editor"] = {
    render: function (sns) {
        return "<div class='dhx_cal_ltext' style='height:60px;'>" +
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
    { name: "description", map_to: "text", type: "my_editor", focus: true },
    { name: "time", height: 72, type: "time", map_to: "auto" }
];

scheduler.ignore_month = function (date) {
    if (date.getDay() == 6 || date.getDay() == 0) //hides Saturdays and Sundays
        return true;
};
scheduler.ignore_week = function (date) {
    if (date.getDay() == 6 || date.getDay() == 0) //hides Saturdays and Sundays
        return true;
};

//Empèche la modification des horaires pour le prof de soutien
scheduler.config.drag_move = false;
scheduler.config.first_hour = 8;
scheduler.config.last_hour = 22;
scheduler.attachEvent("onClick", function (id, ev) {
    post('/Rencontre', { name: id });
    return true;
});
function post(path, params, method = 'post') {

    // The rest of this code assumes you are not using a library.
    // It can be made less wordy if you use one.
    const form = document.createElement('form');

    form.method = method;
    form.action = path;

    for (const key in params) {
        if (params.hasOwnProperty(key)) {
            const hiddenField = document.createElement('input');
            hiddenField.type = 'hidden';
            hiddenField.name = key;
            hiddenField.value = params[key];

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}
scheduler.init("scheduler_here", new Date(), "month");

scheduler.config.max_month_events = 100;
scheduler.load("/api/eventsEtuEns/GetRencontreEns", "json");
// connect backend to scheduler
var dp = new dataProcessor("/api/eventsEtuEns/");
dp.init(scheduler);
// set data exchange mode
dp.setTransactionMode("REST", true);