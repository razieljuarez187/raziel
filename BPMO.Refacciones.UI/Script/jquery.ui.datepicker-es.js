/* Inicialización en español para la extensión 'UI date picker' para jQuery. */
/* Traducido por Vester (xvester@gmail.com). */
jQuery(function($){
	$.datepicker.regional['es'] = {
		closeText: 'Cerrar',
		prevText: '&#x3c;Ant',
		nextText: 'Sig&#x3e;',
		currentText: 'Hoy',
		monthNames: ['Enero','Febrero','Marzo','Abril','Mayo','Junio',
		'Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'],
		monthNamesShort: ['Ene','Feb','Mar','Abr','May','Jun',
		'Jul','Ago','Sep','Oct','Nov','Dic'],
		dayNames: ['Domingo','Lunes','Martes','Mi&eacute;rcoles','Jueves','Viernes','S&aacute;bado'],
		dayNamesShort: ['Dom','Lun','Mar','Mi&eacute;','Juv','Vie','S&aacute;b'],
		dayNamesMin: ['Do','Lu','Ma','Mi','Ju','Vi','S&aacute;'],
		weekHeader: 'Sm',
		dateFormat: 'dd/mm/yy',
		firstDay: 1,
		isRTL: false,
		showMonthAfterYear: false,
		yearSuffix: ''};
	$.datepicker.setDefaults($.datepicker.regional['es']);
});

function MesPagoBono() {
    var meses = '<option value="NULL">Seleccionar</option>'+
    '<option value="0">Diciembre</option>' +
    '<option value="1">Enero</option>'+
    '<option value="2">Febrero</option>'+
    '<option value="3">Marzo</option>'+
    '<option value="4">Abril</option>'+
    '<option value="5">Mayo</option>'+
    '<option value="6">Junio</option>'+
    '<option value="7">Julio</option>'+
    '<option value="8">Agosto</option>'+
    '<option value="9">Septiembre</option>'+
    '<option value="10">Octubre</option>'+
    '<option value="11">Noviembre</option>'+
    '<option value="12">Diciembre</option>';
    return meses;
}