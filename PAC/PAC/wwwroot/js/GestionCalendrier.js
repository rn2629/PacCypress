


//Crée les config de basse des évenements
scheduler.config.xml_date = "%Y-%m-%d %H:%i";
scheduler.form_blocks["my_editor"] = {
    render: function (sns) {
        return "<div class='dhx_cal_ltext' style='height:60px;'>" +
            "Nom Dours&nbsp;<input name='text' type='text'><br/>" +
            "Local&nbsp;<input name='location' type='text'></div>";
    },
    set_value: function (node, value, ev) {
        if (ev.descrip == null)
            node.querySelector("[name='text']").value = value;
        else
            node.querySelector("[name='text']").value = value;

        node.querySelector("[name='location']").value = ev.local || "";

    },
    get_value: function (node, ev) {
        ev.local = node.querySelector("[name='location']").value;
        return node.querySelector("[name='text']").value;
    },
    focus: function (node) {
        var input = node.querySelector("[name='text']");
        input.select();
        input.focus();
    }
};

scheduler.ignore_month = function (date) {
    if (date.getDay() == 6 || date.getDay() == 0) //hides Saturdays and Sundays
        return true;
};
//Empèche la modification des horaires pour le prof de soutien
scheduler.config.drag_move = false;

scheduler.locale.labels.section_description = "D\u00e9tails";
scheduler.config.lightbox.sections = [
    { name: "description", map_to: "text", type: "my_editor", focus: true },
    { name: "time", height: 72, type: "time", map_to: "auto" }
];

//Crée la Rencontre dans la bd en utilisant l'API
function savePlage(idUser, idCours) {
    $.post("/api/eventsEtuEns/SavePlage",
        {
            idUser: idUser,
            idCours: idCours
        },
        function (data, status) {
            scheduler.updateEvent();
        });
    scheduler.setCurrentView(scheduler._currentDate());
}

//Supprime la rencontre de la bd en utilisant l'API
function deletePlage(idUser, idCours) {
    $.post("/api/eventsEtuEns/DeletePlage",
        {
            idUser: idUser,
            idCours: idCours
        },
        function (data, status) {
            scheduler.updateEvent();
        });
    scheduler.setCurrentView(scheduler._currentDate());
}


//Initialise le calendrier
scheduler.init("scheduler_here", new Date(), "month");

// load data from backend

scheduler.config.max_month_events = 100;
scheduler.load("/api/eventsEtuEns/Get", "json");
scheduler.attachEvent("onUpdateEvent", function () {
    GetEvSise();
});



// connect backend to scheduler
var dp = new dataProcessor("/api/eventsEtuEns");
dp.init(scheduler);

// set data exchange mode
dp.setTransactionMode("REST", true);
