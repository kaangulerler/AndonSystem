Filtre_Getir();

Rapor_Vardiya();

function Rapor_Vardiya() {

    let data = {
        "Istasyon_List": $('#filtre_istasyon').val(),
        "Tarih": amount.value
    }

    $.ajax(
        {
            type: "POST",
            url: url + 'api/Rapor/Vardiya',
            async: false,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.islem) {

                    let panel_Orta = document.getElementById("panel_Orta");

                    if (result.obje.tablo.length > 0)
                        panel_Orta.style.display = "block";
                    else
                        panel_Orta.style.display = "none";

                    document.getElementById("input_List_Vardiya").value = JSON.stringify(result.obje.tablo);

                    var table = $('#tablo').DataTable({
                        "language": {
                            "url": "/lib/DataTables/Turkish.json"
                        },
                        async: false,
                        bDestroy: true,
                        dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
                            "<'row'<'col-sm-12'tr>>" +
                            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                        data: result.obje.tablo,
                        columns: [
                            { data: 'istasyon.kod' },
                            { data: 'kod' },
                            {
                                data: 'baslangic',
                                render: function (data, type, row) {
                                    return moment(data).format('DD.MM.yyyy HH:mm');
                                }
                            },
                            {
                                data: 'bitis',
                                render: function (data, type, row) {
                                    return moment(data).format('DD.MM.yyyy HH:mm');
                                }
                            },
                            { data: 'hedef' },
                            { data: 'aktuel' },
                            {
                                data: 'delta',
                                render: function (data, type, row) {

                                    if (data > 0)
                                        return '<span class="badge text-bg-success w-100 fs-6">' + data + '</span>'

                                    if (data < 0)
                                        return '<span class="badge text-bg-danger w-100 fs-6">' + data + '</span>'


                                    return '<span class="badge text-bg-light w-100 fs-6">' + data + '</span>';
                                }
                            },
                        ],
                        pageLength: 50,
                        order: [[2, 'asc']],
                        footerCallback: function (row, data, start, end, display) {

                            var api = this.api();

                            var intVal = function (i) {
                                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                            };

                            for (let col = 4; col < 7; col++) {
                                total = api.column(col).data()
                                    .reduce(function (a, b) {
                                        return intVal(a) + intVal(b);
                                    }, 0);

                                pageTotal = api
                                    .column(col, { page: 'current' }).data()
                                    .reduce(function (a, b) {
                                        return intVal(a) + intVal(b);
                                    }, 0);

                                $(api.column(col).footer()).html(pageTotal + ' / ' + total);
                            }
                        },
                    });

                    chart_data = result.obje.chart;
                     
                    google.charts.load('current', { 'packages': ['corechart'] });
                    google.charts.setOnLoadCallback(drawVisualization);

                }
            },
        }
    );
}
  
function Filtre_Getir() {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Rapor/Filtre',
            async: false,
            success: function (result) {

                if (result.islem) {

                    let nesne = result.obje;
                     
                    let preset_list = [];

                    preset_list.push({
                        name: 'Hepsi',
                        all: true,
                        selected: true,
                    });

                    for (let i = 0; i < nesne.istasyon.length; i++) {

                        let hat = nesne.istasyon[i];

                        var optgroup = "<optgroup label='" + hat.kod + "'>";

                        let id_list = [];
                        for (var j = 0; j < hat.t3_Istasyon.length; j++) {
                            let istasyon = hat.t3_Istasyon[j];
                            id_list.push(istasyon.id);

                            if (hat.kod != "MWS")
                                optgroup += "<option value='" + istasyon.id + "' selected>" + istasyon.kod + "</option>";
                            else
                                optgroup += "<option value='" + istasyon.id + "'>" + istasyon.kod + "</option>";
                        }

                        preset_list.push({
                            name: hat.kod,
                            options: id_list
                        })

                        optgroup += "</optgroup>"

                        $('#filtre_istasyon').append(optgroup);
                    }

                    $('#filtre_istasyon').multiSelect({
                        columns: 2,
                        noneText: 'İstasyon Seçin',
                        presets: preset_list,
                        search: true,
                        selectAll: true
                    });

                    $('.multi-select-button').addClass('form-control');

                }
            },
        }
    );
}
 
function drawVisualization() {

    var data = google.visualization.arrayToDataTable(chart_data);

    var options = {
        title: 'Vardiya Grafiği',
        vAxis: { title: 'Üretim Adetleri' },
        //hAxis: { title: 'Month' },
        seriesType: 'bars',
        series: { 5: { type: 'line' } }
    };

    var chart = new google.visualization.ComboChart(document.getElementById('columnchart_material'));
    chart.draw(data, options);
}