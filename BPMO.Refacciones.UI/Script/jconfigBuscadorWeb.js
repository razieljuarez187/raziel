/* File Created: julio 17, 2012 */
//CONFIGURACIÓN DEL Buscador Web
/*--------------------------------------------*/
var srcFrame = "", hdnShowBuscador = "#hdnShowBuscador", width = "", height = "";
$.BuscadorWeb = function (opts) {
    opts = $.extend({}, $.BuscadorWeb.defaults, opts || {});
    if (opts.url === undefined || opts.xml === undefined || opts.guid === undefined || opts.btnSender === undefined) {
        alert("Buscador Web: Los siguientes datos son requeridos: url, xml, guid, btnSender");
        return;
    }
    if (opts.preCarga != null)
        opts.preCarga();
    if (!$(hdnShowBuscador).length || !$("input[name$='btnResult']").length) {
        alert("Buscador Web, debe definir las etiquetas siguientes: HiddenField(hdnShowBuscador), Button(btnResult)");
        return;
    }
    $(hdnShowBuscador).val("1");
    if (opts.features.dialogWidth != undefined) { width = opts.features.dialogWidth; } else { width = '700px'; }
    if (opts.features.dialogHeight != undefined) { height = opts.features.dialogHeight; } else { height = '350px'; }
    srcFrame = opts.url + "?cfg=" + opts.xml + "&pktId=" + opts.guid;
}
//Configuración Default del Buscador
$.BuscadorWeb.defaults = {
    url: '../Buscador/UI/BuscadorUI.aspx',
    xml: undefined,
    guid: undefined,
    btnSender: undefined,
    features: {
        border: 'thick',
        dialogWidth: '700px',
        dialogHeight: '350px',
        center: 'yes',
        help: 'no',
        maximize: '0',
        minimize: 'no'
    },
    preCarga: null
};
//Verifica si se debe desplegar el buscador
function initBuscador() {
    if ($(hdnShowBuscador).val() == "1") {
        $.blockUI({
            message: $('<div id="dvContentBuscador" style="width:80%;display:none;">' +
                        '<div id="dvTitleBuscador" class="ui-widget-header ui-dialog-titlebar ui-corner-all blockTitle" style="width:125%;height: 22px; background-color: #5c5e5d !important;border-top-left-radius: 5px; border-top-right-radius: 5px; color: white;cursor:move">Buscador' +
                            '<span id="closeDialog" onclick="cerrarBuscador();" class="ui-icon ui-icon-close" role="presentation" style="width:20px; height:20px;float:right; cursor:pointer">Cerrar</span>' +
                        '</div>'+
                        '<iframe id="ifBuscador" src="' + srcFrame + '" frameborder="0" style="border-radius:0px 0px 5px 5px;" scrolling="yes" height="' + height + '" width="' + width + '">Tu navegador no soporta frames!</iframe>' +
                    '</div>'),
            css: {
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                'border-radius': '10px',
                width: width,
                height: (parseInt(height.replace("px", "")) + 22) + 'px',
                top:  ($(window).height() - 400) /2 + 'px', 
                left: ($(window).width() - width.replace("px","")) /2 + 'px'
            }
        });
        $("#dvContentBuscador").parent().draggable();
    }
}
//Cerrar el Buscador
function cerrarBuscador() {
    $.unblockUI();
    $(hdnShowBuscador).val("0");
}
/*--------------------------------------------*/