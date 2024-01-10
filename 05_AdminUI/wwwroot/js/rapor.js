let collapse_filtre_durum = "";
let chart_data = [];

 
$(function () {

    let baslangic = moment().add({ days: -60 }).format("DD.MM.YYYY");
    let bitis = moment().format("DD.MM.YYYY");

    function toSec(dateString) {
        // Extract a date from a string value with a specified format.
        // See: https://api.jqueryui.com/datepicker/
        var dt = $.datepicker.parseDate("dd.mm.yy", dateString);
        return dt.getTime() / 1000;
    }

    function secToDate(s) {
        var dt = new Date(s * 1000);
        // Format a date into a string value with a specified format.
        // See: https://api.jqueryui.com/datepicker/
        return $.datepicker.formatDate("dd.mm.yy", dt);
    }

    $("#slider-range").slider({
        range: true,
        min: toSec(baslangic),
        max: toSec(bitis),
        step: 86400,
        values: [toSec(moment().add({ days: -15 }).format("DD.MM.YYYY")), toSec(bitis)],
        slide: function (event, ui) {
            $("#amount").val(secToDate(ui.values[0]) + " - " + secToDate(ui.values[1]));
        }
    });

    $("#amount").val(secToDate($("#slider-range").slider("values", 0)) +
        " - " + secToDate($("#slider-range").slider("values", 1)));

});

