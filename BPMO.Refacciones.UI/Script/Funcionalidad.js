function shTexto(cajaDeTexto) {
    $(cajaDeTexto).jLabel({ speed: 300, opacity: 0.1 });
}

//Mostrar imagen con efecto fade-in
function shElemento(Imagen) {
    Imagen.css('display', 'none');
    Imagen.fadeIn(2000);
}

//Efecto de selección del botón
function SeleccionarBoton(boton) {
    $(boton).hover(function () {
        $(this).addClass('BotonSeleccionado');
    }, function () {
        $(this).removeClass('BotonSeleccionado');
    }
    );
}
//Mensaje de error
function MsjError(lblError) {
    $("#dialog:ui-dialog").dialog("destroy");
    $(lblError).addClass("DialogoError");

    $(lblError).dialog({
        modal: true, height: 350, width: 460,
        show: "blind", hide: "explode",
        title: "Desplegar sistemas",
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
            }
        }
    });
}
//controla el comportamiento de las notificaciones
function SHTexto(Notificacion) {
    $(Notificacion).parent().next().slideToggle('slow');
    var valor = $(Notificacion).css("backgroundImage");
    if (valor.indexOf('Arriba') != -1) {
        $(Notificacion).css({ 'backgroundImage': 'url(../Imagenes/Abajo.png)' });
    }
    else {
        $(Notificacion).css({ 'backgroundImage': 'url(../Imagenes/Arriba.png)' });
    }
}
//controla el comportamiento del menú lateral mediante el evento click
function SHMenuLateral() {
    $('#Deslizar').click(function () {
        $('.tvMenuSecundario').fadeToggle(1000);
        $("#MenuSlide").animate({
            width: "toggle"
        }, 800);
        var botonSH = $("#Deslizar");
        if (botonSH.attr("class") == "classBoton") {
            this.src = "../Imagenes/Izquierda.png";
        } else {
            this.src = "../Imagenes/Derecha.png";
        }
        $('.classBoton').toggleClass("on");
    });
}
//controla el comportamiento del menú lateral mediante el evento hover
function SHMenuLateralHover() {
    $('#BarraDeslizadora').hover(function () {
        $("#MenuSlide").animate({
            width: "show"
        }, 800);
        $('.tvMenuSecundario').fadeIn(1000);
    });
    $('#MenuContenedor').mouseleave(function () {
        $("#MenuSlide").animate({
            width: "hide"
        }, 800);
        $('.tvMenuSecundario').fadeOut(1000);
    });
}
function OcultarMenuLateral() {
    $('#MenuSlide').hide();
}
//código anexado checar al final
function UbicarPie() {
    var pantalla = screen.availHeight - 110;
    var documento = $(document.body).innerHeight();
    $("#PieMDI").css({ 'position': 'absolute', 'top': pantalla > documento ? pantalla : documento, 'background': 'transparent' });

}
//Controla el slide de los elementos para ocultar o mostrar
function SHDIV(boton) {
    $(boton).parent().next().stop(false, true).slideToggle('slow');
    var valor = boton.src;
    if (valor.indexOf('Abajo') != -1) {
        boton.src = '../Imagenes/FlechaArriba.png';
        boton.title = 'Mostrar';
        return 'false';
    }
    else {
        boton.src = '../Imagenes/FlechaAbajo.png';
        boton.title = 'Ocultar';
        return 'true';
    }
}
//Controla el slide de los elementos para ocultar o mostrar
function SHDIVNotificaciones(boton) {
    $(boton).parent().next().stop(false, true).slideToggle('slow');
    var valor = boton.src;
    if (valor.indexOf('Abajo') != -1) {
        boton.src = '../Imagenes/Arriba.jpg';
        boton.title = 'Mostrar';
        $(boton).parent().css('backgroundColor', 'white');
        $(boton).parent().css('color', '#5c5e5d');
        return 'false';
    }
    else {
        boton.src = '../Imagenes/Abajo.jpg';
        boton.title = 'Ocultar';
        $(boton).parent().css('backgroundColor', '#5c5e5d');
        $(boton).parent().css('color', 'white');
        return 'true';

    }
}

//Validar campos requeridos
function ValidatePage(aVAlidar) {
    if (typeof (Page_ClientValidate) == 'function') {
        Page_ClientValidate();
    }
    if (!Page_IsValid) {
        MensajeGrowUI("Falta información necesaria para el registro de " + aVAlidar + ".", "4");
        return;
    }
}
//Ejecuta el boton de buscar cuando se le da enter a un text
function EjecutarBotonBusqueda() {
    $('form').keypress(function (e) {
        if (e.which == 13) {
            return false;
        }
    });
    $("input[type='text']").filter(function () { return !$(this).attr('onchange') }).keypress(function (e) {
        if (e.which == 13) {
            $('.BotonBuscarCatalogo').click();
            $('.BotonBuscarAsignacion').click();
            $('.BotonComando').click();
        }
    });
}
//Despliega la información de la bitacora
function MostrarInformacionBitacora(boton) {    
    if ($('#InformacionBItacora:animated').length == 0) {
        var $boton = $(boton);
        var posicionActual = $boton.offset();
        var horizontal = posicionActual.top + 30;
        var vertical = posicionActual.left - 210;
        if ($boton.hasClass('BotonInformacionBitacora')) {
            $boton.removeClass('BotonInformacionBitacora');
            $('#InformacionBItacora').slideToggle('slow');
        } else {
            $boton.addClass('BotonInformacionBitacora');
            $('#InformacionBItacora').slideToggle('slow').css({ 'top': horizontal, 'left': vertical }).draggable();
        }
    }    
}          
