/* File Created: Agosto 05, 2015 */
//Configuración del Programa de Visita al Cliente
function ShowDialogTareaProgramaInsert() {
    $("#ProgramaVisita").dialog({
        height: 450, autoOpen: false, width: 600, modal: true,
        buttons: {
            ACEPTAR: function () {
                bValid = true;
                bValid = ValidateToInsert();
                if (bValid == true) {
                    TareaProgramaInsert();
                    $("#ProgramaVisita").dialog("close");
                }
            },
            CANCELAR: function () {
                $(this).dialog("close");
            }
        },
    });
}
function ShowDialogTareaProgramaDelete(tareaProgramaID, fua) {
    $("#DeleteTarea").dialog({
        autoOpen: false,height: 200,width: 300,modal: true,
        buttons: {
            BORRAR: function () {
                DeleteTarea(tareaProgramaID, fua);
                $(this).dialog("close");
            },
            CANCELAR: function () {
                $(this).dialog("close");
            }

        },
        close: function () {
        }
    });
}
function DeleteTarea(tareaProgramaID, fua) {
    /*var tareaprogramamediciondto = {
        "tareaprogramamedicionid": parseInt(tareaProgramaID, 10)
    }
    //GetCalendario
    var tareaProxy = new VITADAT.Web.PortalUsuarios.MedicionesService();
    tareaProxy.DeleteTareaProgramaMedicion(tareaprogramamediciondto, DeleteEventOfCalender(tareaprogramamediciondto), ErrorService, null);*/
    return;
}
function ValidateToInsert() {
    /*tips = $(".validateTips");
    tips.text("")
    tips.addClass("validateTips");
    var tipoVisita = $("#<%= ddlTipoVisita.ClientID %>").val();
    if (tipoVisita == "Seleccionar") {
        tips.text("Debe seleccionar un Tipo de Visita")
        return false;
    }
    return true;*/
}
function TareaProgramaInsert() {
    /*var dias = "";
    var tipoMedicionID = $("#<%= ddlTipoVisita.ClientID %>").val();
    var fechaInicial = GetfechaInicial($('#<%= txtFechaTareaPrograma.ClientID %>').val());
    var fechaFinal = GetfechaFinal($('#<%= txtFechaFinal.ClientID %>').val());
    var comentario = $('#<%= txtComentario.ClientID %>').val();
    var hora = $('#<%= txtHora.ClientID %>').val();
    var usuarioperfilid = $("#<%= hdnUsuarioPerfilID.ClientID %>").val();
    var pacienteID = $("#<%= hdnPaciente.ClientID %>").val();
    var programamediciondto = {};
    var programamediciondto = {
        "tipomedicionid": parseInt(tipoMedicionID, 10),
        "fechainicial": fechaInicial,
        "fechafinal": fechaFinal,
        "comentario": comentario,
        "horaprogramada": hora,
        "dias": dias,
        "usuarioperfilid": usuarioperfilid,
        "pacienteid": pacienteID
    }
    var tareaProxy = new VITADAT.Web.PortalUsuarios.MedicionesService();
    tareaProxy.InsertCompleteProgramaMedicion(programamediciondto, null, errorInsertar, null);
    tareaProxy.LastProgramaMedicion(programamediciondto, AddEventToCalendertareaProgramaMediciondto, errorInsertar, null);*/
    return;
}
//Manejo del fullCalendar
function AddEventToCalendertareaProgramaMediciondto(tareaProgramaMediciondto) {
    var tareaJson = DTOToData(tareaProgramaMediciondto);
    $('#calendar').fullCalendar("addEventSource", tareaJson);
    return;
}
//dtoToJson TareasProgramaMedicion
function DTOToData(tareaprogramamediciondto) {
    var mydata = [];
    /*for (var i = 0; i < tareaprogramamediciondto.length; i++) {
        var tarea = tareaprogramamediciondto[i];
        if (Date.parse(tarea.fechaprogramada) < date) {
            var backgroundColor = "#C0D9D9";
            if (tarea.estatus == false) {
                var textColor = 'Red';
            }
            else {
                var textColor = 'Black';
            }
        }
        else {
            if (tarea.tipomedicionid == 1) {
                var backgroundColor = "#6ed242";
                var textColor = 'Black';
            }
            else if (tarea.tipomedicionid == 2) {
                var backgroundColor = "#ffff00"; 7
                var textColor = 'Black';
            }
            else if (tarea.tipomedicionid == 3) {
                var backgroundColor = "#ffc200";
                var textColor = 'Black';
            }
        }
        var objJson = {
            "id": tarea.tareaprogramamedicionid.toString(),
            "title": tarea.tipomedicionnombre,
            "start": tarea.fechaprogramada,
            "backgroundColor": backgroundColor,
            "textColor": textColor,
            "borderColor": backgroundColor,
            "comentario": tarea.comentario
        };
        mydata.push(objJson);
    }*/
    return mydata;
}