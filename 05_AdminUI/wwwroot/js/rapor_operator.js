Filtre_Getir();

function Rapor_Operator() {

    let data = {
        "Istasyon_List": $('#filtre_istasyon').val(),
        "Personel_List": $('#filtre_personel').val(),
        "Barkod": $('#filtre_barkod').val(),
        "ProjeId": $('#filtre_projectid').val(),
        "Tarih": amount.value
    }

    $.ajax(
        {
            type: "POST",
            url: url + 'api/Rapor/Operator',
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

                    document.getElementById("input_List_Excel").value = JSON.stringify(result.obje.tablo);

                    console.log(result.obje);

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
                            { data: 'personel' },
                            { data: 'istasyon' },
                            { data: 'projectId' },
                            { data: 'uretimKod' },
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
                            {
                                data: 'zaman',
                                render: function (data, type, row) {

                                    return data.split('.')[0];
                                }
                            },
                        ],
                        pageLength: 50,
                        order: [[2, 'asc']],
                     });

                    drawPieChart('chart_Personel', 'Personel', result.obje.chart_Personel);
                    drawPieChart('chart_Istasyon', 'Istasyon', result.obje.chart_Istasyon);

                }
            },
        }
    );
}

function drawPieChart(nesne, title, data) {

    if (data.length > 0) {

        $('#panel_Chart').css({ display: "flex" });
         
        var dataTable = new google.visualization.DataTable();

        dataTable.addColumn('string', 'Kod');
        dataTable.addColumn('number', 'Deger');
        dataTable.addColumn({ type: 'string', role: 'tooltip', 'p': { 'html': true } });

        for (let i = 0; i < data.length; i++)
            dataTable.addRow(data[i]);

        var options = {
            title: title,
            pieSliceText: 'label',
            slices: {
                0: { offset: 0.1 },
                1: { offset: 0.1 },
                2: { offset: 0.1 },
                3: { offset: 0.1 },
                4: { offset: 0.1 },
                5: { offset: 0.1 },
            },
            tooltip: { isHtml: true },
            legend: { position: 'right', textStyle: { fontSize: 14 } },
            chartArea: { left: 20, top: 50, width: '90%', height: '70%' }
        };

        var chart = new google.visualization.PieChart(document.getElementById(nesne));
        chart.draw(dataTable, options);
    }
    else
        $("#panel_Chart").css({ display: "none" });
}


function Filtre_Getir() {
    google.charts.load("current", { packages: ["corechart"] });

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


                    preset_list = [];

                    preset_list.push({
                        name: 'Hepsi',
                        all: true,
                        selected: true,
                    });

                    for (let i = 0; i < nesne.personel.length; i++) {

                        let personel = nesne.personel[i];

                        let opt = "<option value='" + personel.id + "' selected>" + personel.ad + " " + personel.soyad + "</option>";

                        $('#filtre_personel').append(opt);
                    }

                    $('#filtre_personel').multiSelect({
                        columns: 2,
                        noneText: 'Personel Seçin',
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