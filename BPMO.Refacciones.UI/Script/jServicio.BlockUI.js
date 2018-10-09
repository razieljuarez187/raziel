Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

$(document).ready(function () {
    ConfiguracionInicial();
    $('#MenuSecundario').hide();
    $('#MenuSecundario').show(500);
});
function ConfiguracionInicial() {
    $("span:contains('*')").addClass('ColorValidator');
    EstilosMenu();
    EstilosMenuSecundario();
    AsignarImagen();
    SeleccionarOpcionMenuSecundario();
    $(':text, :input').addClass('TipoLetra').css({ 'border-radius': '5px' });
    $(':text').css({ 'height': '20px', 'font-family': 'Century Gothic, Arial, Verdana, Serif', 'padding-right': '5px' });
    $('div.ContenedorMensajes').css({ 'width': '100%', 'margin-bottom': '5px', 'text-align': 'right', 'text-transform': 'uppercase' });
    $("span.Requeridos").addClass("ColorValidator").text("(*)Campos requeridos").css({ 'font-size': '12px', 'margin-right': '5px' });
    $("span.FormatoIncorrecto").addClass("ColorValidator").text("(**)Formato incorrecto").css({ 'font-size': '12px', 'margin-right': '5px' });
    $("#MenuSecundario li > a").css('width', '100%');

}
/*Inicio de Request*/
function BeginRequestHandler(sender, args) {
    var dialog = $('#hdnDialog');
    //Obtener información de la bitácora y guardarla en el objeto
    bitacoraActual.GuardarBitacora($('#InformacionBItacora'));
    if (dialog != undefined) {
        $('#hdnDialog').val("");
    }
    ConfiguracionInicial();
    $.blockUI({
        fadeIn: 1000,
        message: '<img src="../Imagenes/Cargando.gif" />  Espere por favor...',
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            'border-radius': '5px',
            opacity: .5,
            color: '#fff'
        }
    });
}
/*Fin de Request*/
function EndRequestHandler(sender, args) {
    $.unblockUI();
    var dialog = $('#hdnDialog');
    if (dialog != undefined) {
        if ($('#hdnDialog').val() == "") {
            MensajeCambioAdscripcionBlock();
        }
    }
    initClient();
    if (typeof InitControls === "function")
        InitControls();
    ConfiguracionInicial();
    //Mostrar información de la bitácora en caso de estar visible
    bitacoraActual.Mostrar();
    initDialog();
}
/*Alertas GrowUI: 1= Exito, 2=Advertencia, 4= Información*/
function MensajeGrowUI(message, tipo) {
    var $m
    var title = "";
    if (tipo == "1") {
        $m = $('<div class="growlUIExito growlUI"></div>');
        title = "Éxito";
    } else if (tipo == "2") {
        $m = $('<div class="growlUIAlerta growlUI"></div>');
        title = "Advertencia";
    } else if (tipo == "4") {
        $m = $('<div class="growlUIInfo growlUI"></div>');
        title = "Información";
    }
    if (title) $m.append('<h1>' + title + '</h1>');
    if (message) $m.append('<h2>' + message + '</h2>');
    $.blockUI({
        message: $m, fadeIn: 700, fadeOut: 1000, centerY: false,
        timeout: 5000, showOverlay: false,
        css: {
            width: '350px',
            top: '10px',
            left: '',
            right: '10px',
            border: 'none',
            padding: '5px',
            opacity: 0.6,
            cursor: 'default',
            color: '#fff',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            'border-radius': '10px'
        }
    });
    LimpiarHdn();
}

//btnSender:  referencia al botón que dispara el postback al Server
//mensaje: Mensaje a desplegar en la Confirmación
function MensajeConfirmacion(btnSender, mensaje) {
    var $div = $('<div title="Confirmación"></div>');
    $div.append(mensaje);
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({
        closeOnEscape: true,
        modal: true,
        minWidth: 460,
        minHeight: 250,
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
                $(btnSender).click();
            },
            Cancelar: function () {
                $(this).dialog("close");
            }
        }
    });
}

//btnSender:  referencia al botón que dispara el postback al Server
//mensaje: Mensaje a desplegar en la Confirmación
function MensajeInformacion(btnSender, mensaje) {
    var $div = $('<div title="Información"></div>');
    $div.append(mensaje);
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({
        open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog).hide(); },
        closeOnEscape: false,
        modal: true,
        minWidth: 460,
        minHeight: 250,
        buttons: {
            Actualizar: function () {
                $(this).dialog("close");
                $(btnSender).click();
            }
        }
    });
}

//Mostrar Mensaje de Error, con opción Ver Detalle
function MensajeError(mensaje, detalle) {
    $("#dialog-error").remove();
    var $div = $('<div id="dialog-error" title="Error" style="display:none;"></div>');
    $div.append('<div id="dMensajeError">' + mensaje + '</div>');
    var $acordion = $('<div id="accordion"></div>');
    $acordion.append('<h3 class="ui-accordion-header ui-helper-reset ui-state-active ui-corner-top"><a href="#">Ver Detalle</a></h3>');
    $acordion.append('<div id="dDetalle">' + detalle + '</div>');
    $div.append($acordion);
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({
        closeOnEscape: true,
        modal: true,
        minWidth: 460,
        minHeight: 250,
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#accordion").accordion({
        collapsible: true,
        event: "click",
        icons: false,
        active: false
    });
    LimpiarHdn();
}

/*Mensaje en Block: <div id="dialog-info" title="Éxito"></div>*/
function MensajeCambioAdscripcionBlock() {
    var mensaje = "Se requiere cerrar la página actual debido a que ha cambiado su selección de Unidad Operativa-Sucursal-Taller para esta sesión.";
    $("#dialog-cambio").remove();
    var $div = $('<div id="dialog-cambio" title="Información" style="display:none;">' + mensaje + '</div>');
    $($div).dialog({ resizable: false, closeOnEscape: false, modal: true, minWidth: 350, minHeight: 200,
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
                closeParentUI();
                window.parent.EnvioInicio("CAMBIOAS");
            }
        },
        beforeClose: function (event, ui) {
            return false;
        }
    });
}
function EnvioInicio(valueDialog) {
    if (valueDialog == "CAMBIOAS")
        $(location).attr('href', "../Catalogos.UI/default.aspx?pkt=1");
}

//Mensaje Despliegue: <div id="dmsjDespliegue" style="position: absolute; top:0px; display: none;"></div>
function MensajeTop(mensaje, tipo) {
    if (tipo == "1")
        $("#dmsjDespliegue")._addClass("exito");
    else if (tipo == "2")
        $("#dmsjDespliegue")._addClass("alerta");
    else if (tipo == "4")
        $("#dmsjDespliegue")._addClass("info");

    $("#dmsjDespliegue").text(mensaje);
    $("#dmsjDespliegue").fadeIn(800);
    setTimeout(function () { $("#dmsjDespliegue").fadeOut(800); }, 3000); //.fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300)
}

var contadorName;
var contadorVal;
var hdnInicioContador;
function Contador(hdnContador, hdnIniContFinSession) {
    if (hdnContador != undefined && hdnIniContFinSession != undefined) {
        contadorName = hdnContador;
        hdnInicioContador = parseInt($(hdnIniContFinSession).val());
    }
    contadorVal = parseInt($(contadorName).val());
    if (contadorVal == hdnInicioContador)
        InfoFinSession(1);
    if (contadorVal <= hdnInicioContador)
        $("#timeSession").val(contadorVal);
    if (contadorVal == 0) {
        FinSession();
        contadorVal = undefined;
        hdnInicioContador = undefined;
    } else {
        setTimeout("Contador()", 1000)
        contadorVal = contadorVal - 1;
        $(contadorName).val(contadorVal);
    }
}
function BtnFinSession() {
    __doPostBack('', '');
}
var typeClose;
function InfoFinSession() {
    $("#dvFinSession").remove();
    var $div = $('<div id="dvFinSession" title="Información" style="display:none;">La sesión de esta página ha permanecido inactiva.<br/>' +
        'Su sesión expirará en: <input id="timeSession" readonly="readonly" type="text" value="" style="background-color:transparent;border-style:none;width:30px;"/>segundos.</div>');
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({ resizable: false, closeOnEscape: false, modal: true, minWidth: 350, minHeight: 150, hide: "explode",
        buttons: {
            Cancelar: function () {
                typeClose = 1;
                $(this).dialog("close");
            }
        },
        close: function (event, ui) {
            __doPostBack('', '');
        }
    });
}
function FinSession() {
    typeClose = undefined;
    $("#dvFinSession").remove();
    var $div = $('<div id="dvFinSession" title="Información" style="display:none;line-height:20px;">El tiempo de inactividad de esta página ha alcanzado su límite.<br/>' +
        'a) Si ha tenido actividad en otras páginas del módulo su sesión reanudará..<br/>' +
        'b) Si no ha mantenido actividad dentro del módulo será redirigido al logueo para iniciar sesión nuevamente.</div>');
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({ resizable: false, closeOnEscape: false, modal: true, minWidth: 450, minHeight: 250, hide: "explode",
        buttons: {
            Aceptar: function () {
                typeClose = 2;
                $(this).dialog("close");
                BeginRequestHandler();
                BtnFinSession();
            }
        },
        beforeClose: function (event, ui) {
            if (typeClose == undefined)
                return false;
        }
    });
}
var post = '__doPostBack';

//Restablece las variables utilizadas en el Dialog
var width = "800px", height = "600px", pagina = "", title = "", invocarMetodo = "", invocarMetodoError = "";
var ejecutarAccion = false, returnValueOC = undefined;
function resetVarDialog() {
    this.pagina = "";
    this.title = "Catálogo";
    this.width = "800px";
    this.height = "600px"
    this.invocarMetodo = "";
    this.invocarMetodoError = "";
    this.ejecutarAccion = false;
    this.returnValueOC = undefined;
}
//Invocar showModalDialog
/*Recibe 5 parámetros: 
* pagina: Página que se desea abrir como dialogo
* title: Título del dialog
* width: Ancho del dialog
* height: Alto del dialog
* invocarMetodo(opcional): si después de cerrar el dialogo se requiere ejecutar un método
*/
function showDialogModal(pagina, title, width, height, invocarMetodo) {
    if (pagina == undefined) {
        MensajeGrowUI("Debe proveer la url de la página a desplegar como dialogo", "2");
        return false;
    }
    resetVarDialog();
    this.pagina = pagina;
    if (title != undefined && title != "") {
        this.title = title;
    }
    if (width != undefined || height != undefined) {
        this.width = width;
        this.height = height;
    }    
    if (invocarMetodo != undefined && invocarMetodo != "") {
        this.invocarMetodo = invocarMetodo;
    }    
    $("#MainContent_hdnShowDialogModal").val("1");
}
/*
function showDialogModalCompra(pagina, title, width, height, invocarMetodo, invocarMetodoError) {
    resetVarDialog();
    this.pagina = pagina;
    this.title = title;
    this.width = width;
    this.height = height;
    if (invocarMetodo != undefined && invocarMetodo != "") {
        this.invocarMetodo = invocarMetodo;
    }
    if (invocarMetodoError != undefined && invocarMetodoError != "") {
        this.invocarMetodoError = invocarMetodoError;
    }
    $("#MainContent_hdnShowDialogModal").val("OC");
}*/
//Abrir Dialog
function initDialog() {
    $dvShowDialog = $("#MainContent_hdnShowDialogModal");
    if ($dvShowDialog.length == 1 && ($dvShowDialog.val() == "1" || $dvShowDialog.val() == "OC")) {
        var dialog = $('#hdnDialog');
        //Obtener información de la bitácora y guardarla en el objeto
        bitacoraActual.GuardarBitacora($('#InformacionBItacora'));
        /*if (dialog != undefined) {
            $('#hdnDialog').val("");
        }*/
        ConfiguracionInicial();
        $.blockUI({
            message: $('<div id="dvContentDialog" style="width:80%;display:none;">' +
                '<div id="dvTitleDialog" class="ui-widget-header ui-dialog-titlebar ui-corner-all blockTitle" style="width:125%;height: 22px;background-color: #5c5e5d !important;border-top-left-radius: 5px; border-top-right-radius: 5px; color: white;cursor:move">' + this.title +
                    '<span id="closeDialog" onclick="cerrarDialog();" class="ui-icon ui-icon-close" role="presentation" style="width:20px; height:20px;float:right; cursor:pointer">Cerrar</span>' +
                '</div>'+
                '<iframe id="ifPagina" src="' + pagina + '" frameborder="0" style="border-radius:0px 0px 5px 5px;" scrolling="yes" height="' + height + '" width="' + width + '">Tu navegador no soporta frames!</iframe>' +
           '</div>'),
            css: {
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                'border-radius': '10px',
                width: width,
                height: (parseInt(height.replace("px", "")) + 22) + 'px',
                top: ($(window).height() - height.replace("px", "")) / 2 + 'px',
                left: ($(window).width() - width.replace("px", "")) / 2 + 'px'
            }
        });
        $("#dvContentDialog").parent().draggable();
    }
}
//Cerrar el dialog
function cerrarDialog() {
    var esDialogCompra = $("#MainContent_hdnShowDialogModal").val() == "OC";
    $("#MainContent_hdnShowDialogModal").val("0");
    EndRequestHandler();
    if (esDialogCompra) {
        //Funcionalidad unicamente para ConsultarOrdenCompraUI
        if (returnValueOC !== undefined && returnValueOC !== null && returnValueOC !== "") {
            if (returnValueOC == 0) {
                setTimeout(this.invocarMetodoError, 10);
            } else if (returnValueOC == -1) {
                setTimeout("MostrarOrdenServicioNoValida()", 10);
            } else {
                setTimeout(this.invocarMetodo, 10);
            }
        }
    } else {
        if (this.invocarMetodo != undefined && this.invocarMetodo != "" && ejecutarAccion) {
            setTimeout(this.invocarMetodo, 10);
        }
    }
}
//Agrega estilos a los elementos del menú
function EstilosMenu() {
    $('#NavegacionMDI .MenuEstiloElementoEstatico').hover(function () {
        var colorFondo = $('#DatosSesion').css('backgroundColor');
        if (!$(this).hasClass('MenuPrincipalSeleccionado')) {
            $(this).css({ 'backgroundColor': colorFondo, 'color': 'white' });
        }
    }, function () {
        if (!$(this).hasClass('MenuPrincipalSeleccionado')) {
            $(this).css({ 'backgroundColor': 'white', 'color': '#56421f' });
        }
    });
    $('#NavegacionMDI .MenuElementoDinamico').hover(function () {
        $(this).css({ 'backgroundColor': '#bfcbd6', 'color': 'white', 'font-weight': 'bold' });
    }, function () {
        $(this).css({ 'backgroundColor': 'white', 'color': '#56421f', 'font-weight': 'Normal' });
    });
}
//Agrega estilos a los elementos del menú secundario
function EstilosMenuSecundario() {
    var $menuSecundario = $('#MenuSecundario li');
    $menuSecundario.unbind('mouseenter mouseleave');
    $menuSecundario.hover(function () {
        var color = $('#MenuSecundario li.MenuSecundarioSeleccionado').css('backgroundColor');
        var $liSeleccionado = $(this);
        if (!$liSeleccionado.hasClass('MenuSecundarioSeleccionado')) {
            $liSeleccionado.css({ 'backgroundColor': color, 'cursor': 'pointer' });
            $liSeleccionado.find('a').css('color', 'white');
            var urlImagen = $liSeleccionado.find('.IconoMenu').attr('src')
            if (urlImagen != null) {
                urlImagen = urlImagen.replace('.png', 'Seleccionado.png');
                $liSeleccionado.find('.IconoMenu').attr('src', urlImagen);
            }
        }
    }, function () {
        var $liSeleccionado = $(this);
        if (!$liSeleccionado.hasClass('MenuSecundarioSeleccionado')) {
            $liSeleccionado.css('backgroundColor', '#e3e3e3');
            $liSeleccionado.find('a').css('color', 'black');
            var urlImagen = $liSeleccionado.find('.IconoMenu').attr('src')
            if (urlImagen != null) {
                urlImagen = urlImagen.replace('Seleccionado.png', '.png');
                $liSeleccionado.find('.IconoMenu').attr('src', urlImagen);
            }
        }
    });
}
//Redirección a la página contenida en el tag anchor del li
function SeleccionarOpcionMenuSecundario() {
    $('#MenuSecundario li').click(function () {
        window.location = $(this).find('a').attr('href');
    });
}
//Asigna la imagen correspondiente al tipo de notificaciones
function AsignarImagen() {
    $('div.A > img').attr('src', '../Imagenes/icono-informacion.jpg');
    $('div.N > img').attr('src', '../Imagenes/iconomensaje.jpg');
}
//Agrega estilos a los elementos del menú secundario
function EstilosMenuOpciones() {
    $('#MenuOpcionesOrden li:not(.SubMenuSeleccionado)').hover(
     function () {
         $(this).addClass('MenuOpcionesOrdenlihover');
     }, function () {
         $(this).removeClass('MenuOpcionesOrdenlihover');
     });
}
//Agrega estilos a los elementos del menú de los catálogos
function EstilosMenuOpcionesCatalogos() {
    $('#MenuOpcionesCatalogos li:not(.SubMenuSeleccionado, .SubMenuCatalogosDeshabilitado)').hover(
     function () {
         $(this).addClass('MenuOpcionesCatalogoslihover');
     }, function () {
         $(this).removeClass('MenuOpcionesCatalogoslihover');
     });
}
//Agrega estilo a los submenús del menú opciones
function EstilosSubMenuOpciones() {
    $('#SubMenuOrdenServicio li>:submit').addClass('BotonSubmenuHabilitadoOS');
    $('#SubMenuOrdenServicio li>:submit:disabled').removeClass('BotonSubmenuHabilitadoOS');
    $('#SubMenuOrdenServicio li').hover(function () {
        $(this).css({ 'backgroundColor': '#E9581B', 'cursor': 'pointer' });
        $(this).children().filter(function () { return $(this).hasClass('BotonSubmenuHabilitadoOS'); }).addClass('BotonSubmenuSeleccionadoOS');
    }, function () {
        $(this).css({ 'backgroundColor': 'white' });
        $(this).children().removeClass('BotonSubmenuSeleccionadoOS');
    });
}

//Funcionamiento hover para el botón regresar
function EfectoHoverDeBoton(boton, color) {
    var colorMenu = color || '#E9581B';
    boton.hover(
     function () {
         $(this).css({ 'backgroundColor': colorMenu, 'color': 'white', 'cursor': 'pointer' });
     },
     function () {
         $(this).css({ 'backgroundColor': 'transparent', 'color': '#56421f' });
     });
}
//Efectos hover para menús secundarios varios
function EfectoHoverSubMenu(boton) {
    boton.hover(
     function () {
         $(this).css({ 'backgroundColor': '#E9581B', 'cursor': 'pointer' }).children().css('color', 'white');
     },
     function () {
         $(this).css({ 'backgroundColor': '#e3e3e3' }).children().css('color', 'black');
     });
}

/*-------------------------------Sección de objetos--------------------------------------------------*/
//Objeto Bitácora
var bitacoraActual = {
    estado: null,
    posx: 0,
    posy: 0,
    bitacora: null,
    GuardarBitacora: function (objetoBitacora) {
        if (objetoBitacora != null) {
            this.bitacora = objetoBitacora.selector;
            this.estado = objetoBitacora.css('display');
            var posicion = objetoBitacora.offset();
            if (posicion != null) {
                this.posy = posicion.top;
                this.posx = posicion.left;
            }
        }
    },
    Mostrar: function () {
        if (this.bitacora != null && this.estado != null) {
            $(this.bitacora).css({ 'display': this.estado, 'top': this.posy, 'left': this.posx }).draggable();
            if (this.estado == 'hidden' || this.estado == 'none')
                $('.BotonInfoBitacora').removeClass('BotonInformacionBitacora');
            else
                $('.BotonInfoBitacora').addClass('BotonInformacionBitacora');
        }
    }
}
//Modificación de la configuración predeterminada para la función de precarga del buscador
if ($.BuscadorWeb != null && $.BuscadorWeb != undefined) {
    $.BuscadorWeb.defaults.preCarga = function () {
        ConfiguracionInicial();
        bitacoraActual.Mostrar();
    };
}