

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

function init() {

    //Initialise la gestion des classes des evénements et Lance GetEvSize() pour update le stacking
    scheduler.templates.event_class = function (start, end, ev) {
        GetEvSise();
        return getJumeler(ev);
    };

    //Fait une recherche avec l'API pour les classes qui ont une rencontre
    function getJumeler(ev) {

        $.ajax({
            type: "GET",
            url: "/api/eventsEtuEns/GetRencontre/" + ev.id,
            dataType: "text",
            success: function (responseText) { setJumeler(responseText, ev.id); }
        });
        return scheduler.getEvent(ev.id).event_class;
    }

    //Gere les données après les avoir reçus de l'API
    function setJumeler(response, id) {
        if (response == "true" && scheduler.getEvent(id).event_class != "seanceJum") {
            scheduler.getEvent(id).event_class = "seanceJum";
            scheduler.updateEvent(id);
        }

        else {
            scheduler.getEvent(id).event_class = "prof_event";
        }



    }

    //Initialise l'affichage des Enseignants
    scheduler.templates.event_bar_text = function (start, end, ev) {

        return getUserName(start, end, ev);
    }

    //Fait une requette à l'API pour ressortir le nom des Enseignants selon leurs horaires
    function getUserName(start, end, ev) {
        $.ajax({
            type: "GET",
            url: "/api/eventsEtuEns/GetUserNameEns/" + ev.id,
            dataType: "text",
            success: function (responseText) { setUserData(responseText, ev.id); }
        });
        return scheduler.getEvent(ev.id).event_bar_text;
    }

    //Gere les noms d'utilisateurs après les reçus
    function setUserData(data, id) {
        if (scheduler.getEvent(id).event_bar_text != data) {
            scheduler.getEvent(id).event_bar_text = data;
            scheduler.updateEvent(id);
        }
    }

    //JumpStart l'affiche correct des horaires
    setTimeout(function () {
        GetEvSise();
        scheduler.setCurrentView(scheduler._currentDate());
        setTimeout(function () {
            scheduler.setCurrentView(scheduler._currentDate());
        }, 1000);
    }, 3000);
}

//Change la variable de top pour empècher le stacking
function GetEvSise() {

    var rect = document.getElementsByClassName("dhx_cal_event_clear");
    var prevTop;
    var bot = 0;
    for (i = 0; i < rect.length; i++) {
        if (prevTop != null) {
            if (prevTop.style.left == rect.item(i).style.left) {
                bot = prevTop.clientHeight;
                rect.item(i).style.top = CalculSize(prevTop.style.top, bot);
            }

        }
        scheduler.xy.bar_height = 70;
        prevTop = rect.item(i);
    }


}

//Calcul la distance pour enlever le stacking
function CalculSize(prevTop, bottom) {
    var tempTop = prevTop.split("px");
    var result = parseFloat(tempTop[0]) + bottom
    return result + "px";

}

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
