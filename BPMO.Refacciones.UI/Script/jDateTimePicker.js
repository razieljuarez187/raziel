(function ($) {
    $.extend($.ui, { DateTimepicker: {} });
    $.datepicker._base_updateDatepicker = $.datepicker._updateDatepicker;
    $.datepicker._updateDatepicker = function (inst) {
        //Agregado
        var input = inst.input[0];
        if ($.datepicker._curInst && $.datepicker._curInst != inst && $.datepicker._datepickerShowing && $.datepicker._lastInput != input)
            return;
        if (typeof (inst.stay_open) !== 'boolean' || inst.stay_open === false) {
            //Termina
            $.datepicker._base_updateDatepicker(inst);
            var val = inst.input.val();
            if (inst.input.hasClass("datetimepicker") == false) return;
            var bottomLayer = $(".ui-datepicker").append("<div class='.datetimepicker'/>");
            bottomLayer.append("Hr:");
            bottomLayer.append(html1);
            bottomLayer.append("Min:");
            bottomLayer.append(html2);
            bottomLayer.append(html3);
            //bottomLayer.append("Seg ");
            if (val.length != 0)
                $.datetimepicker.setTime(val, inst.input);
            else
                $.datetimepicker.setTime(new Date().localeFormat("dd/MM/yyyy HH:mm"), inst.input);
        }
        $(".ui-datepicker-current").hide();
    };

    function DateTimePicker(options) {
        this.defaultDateTimePicker =
			{
			    showTimePicker: false,
			    time_format: 'HH:mm:ss'
			};
        hourHTML = $("<select class='dateimepicker-hour' onchange='$.datetimepicker.isDirty = true;'></select>");
        minutsHTML = $("<select class='dateimepicker-minuts' onchange='$.datetimepicker.isDirty = true;'></select>");
        secondHTML = $("<select class='dateimepicker-second' onchange='$.datetimepicker.isDirty = true;'></select>");

        for (i = 0; i <= 23; i++) html1 = hourHTML.append("<option>" + ((i < 10) ? "0" + i : i) + "</option");
        for (i = 0; i < 60; i = i + 1) html2 = minutsHTML.append("<option>" + ((i < 10) ? "0" + i : i) + "</option");
        for (i = 0; i < 60; i = i + 1) html3 = minutsHTML.append("<option>" + ((i < 10) ? "0" + i : i) + "</option");
    }
    $.fn.extend({
        datetimepicker: function (options) {
            $.datetimepicker._attach(this, options);
        }
    });
    DateTimePicker.prototype = {
        _initTimePicker: function () {
        },
        isDirty: false,
        innerOption: function ($this) {
            return {
                showOn: "button",
                closeText: 'Aceptar',
                buttonImage: "../Imagenes/calendar.gif",
                buttonImageOnly: true,
                autoSize: true,
                showAnim: "slideDown",
                buttonText: "Ver Calendario",
                showButtonPanel: true,
                onSelect: function (a, b) {
                    var hour = $('.ui-datepicker .dateimepicker-hour').val();
                    var minuts = $('.ui-datepicker .dateimepicker-minuts').val();
                    //var second = $('.ui-datepicker .dateimepicker-second').val();
                    var time = hour + ":" + minuts;// + " " + second;
                    $this.val($this.val() + " " + time);
                    $.datetimepicker.setTime($this.val(), $this);
                    $.datetimepicker.reset();
                    if (typeof ConfigFechaPromesa === "function")
                        ConfigFechaPromesa($this.val());
                },
                onClose: function (a, b) {
                    if ($.datetimepicker.isDirty) {
                        $this.val($.datepicker._formatDate(b));
                        var hour = $('.ui-datepicker .dateimepicker-hour').val();
                        var minuts = $('.ui-datepicker .dateimepicker-minuts').val();
                        //var second = $('.ui-datepicker .dateimepicker-second').val();
                        var time = hour + ":" + minuts;// + " " + second;
                        $this.val($this.val() + " " + time);
                        $.datetimepicker.reset();
                        if (typeof ConfigFechaPromesa === "function")
                            ConfigFechaPromesa($this.val());
                    }
                }
            };
        },
        _attach: function ($this, options) {
            $this.datepicker($.fn.extend(this.innerOption($this), options))
				.addClass("datetimepicker");
            this.setTime($this.val(), $this);
        },
        reset: function () {
            this.isDirty = false;
        },
        setTime: function (format, $this) {
            var arrStr = format.split(' ');
            var hour = 12;
            var minuts = 0;
            var second = 0;//"am";
            if (arrStr.length > 1) {
                var strTime = arrStr[1];
                var arr = strTime.split(':');
                if (arr.length > 0) {
                    hour = arr[0];
                    if (arr.length > 1) minuts = arr[1];
                    if (arr.length > 2) second = arr[2];
                }
            }
            $('.ui-datepicker .dateimepicker-hour').val(hour);
            $('.ui-datepicker .dateimepicker-minuts').val(minuts);
            $('.ui-datepicker .dateimepicker-second').val(second);
        }
    };
    /*
    $.datepicker._base_gotoToday = $.datepicker._gotoToday;
    $.datepicker._gotoToday = function (id) {
        var inst = this._getInst($(id)[0]), $dp = inst.dpDiv;
        this._base_gotoToday(id);
        var now = new Date().localeFormat("dd/MM/yyyy HH:mm");
        $.datetimepicker.setTime(now, id);
        $('.ui-datepicker-today', $dp).click();
    };*/

    $.datepicker._base_selectDate = $.datepicker._selectDate;
    $.datepicker._selectDate = function (id, dateStr) {
        var inst = this._getInst($(id)[0]);
        inst.inline = inst.stay_open = true;
        this._base_selectDate(id, dateStr);
        inst.inline = inst.stay_open = false;
        this._notifyChange(inst);
        this._updateDatepicker(inst);
    };
    $.datetimepicker = new DateTimePicker();
    $.datetimepicker._initTimePicker();
} (jQuery));
