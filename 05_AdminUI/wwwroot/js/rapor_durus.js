Filtre_Getir();
  
function Rapor_Durus() {

    let data = {
        "Istasyon_List": $('#filtre_istasyon').val(),
        "Durus_List": $('#filtre_durus').val(),
        "Tarih": amount.value
    }

    $.ajax(
        {
            type: "POST",
            url: url + 'api/Rapor/Durus',
            async: false,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.islem) {
                     
                    document.getElementById("input_List_Excel").value = JSON.stringify(result.obje.tablo);


                    let panel_Orta = document.getElementById("panel_Orta");

                    if (result.obje.tablo.length > 0)
                        panel_Orta.style.display = "block";
                    else
                        panel_Orta.style.display = "none";
                     
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
                            { data: 'durus' },
                            { data: 'istasyon' },
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
                                    return data.split(".")[0];;
                                }
                            }
                            
                        ],
                        pageLength: 50,
                        order: [[0, 'asc']],
                     });

                    drawPieDurusChart(result.obje.chart);
                }
            },
        }
    );
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

                    for (let i = 0; i < nesne.durus.length; i++) {

                        let item = nesne.durus[i];

                        let option = "<option value='" + item.id + "' selected>" + item.kod + "</option>";

                        $('#filtre_durus').append(option);
                    }

                    let xpreset_list = [];

                    xpreset_list.push({
                        name: 'Hepsi',
                        all: true,
                        selected: true,
                    });

                    $('#filtre_durus').multiSelect({
                        columns: 2,
                        noneText: 'Duruş Tipi',
                        presets: xpreset_list, 
                        search: true,
                        selectAll: true,

                    });

                    $('.multi-select-button').addClass('form-control');

                }
            },
        }
    );
}

function drawPieDurusChart(data) {

    if (data.length > 0) {

        $("#panel_Chart").css({ display: "flex" });


        var dataTable = new google.visualization.DataTable();

        dataTable.addColumn('string', 'Kod');
        dataTable.addColumn('number', 'Deger');
        dataTable.addColumn({ type: 'string', role: 'tooltip', 'p': { 'html': true } });

        for (let i = 0; i < data.length; i++)
            dataTable.addRow(data[i]);

        var options = {
            title: 'Duruşlar',
            pieSliceText: 'label',
            slices: {
                0: { offset: 0.1, color: "Red" },
                1: { offset: 0.1, color: "IndianRed" },
                2: { offset: 0.1, color: "FireBrick" },
                3: { offset: 0.1, color: "Salmon" },
                4: { offset: 0.1, color: "Crimson" },
                5: { offset: 0.1, color: "FireBrick" },
            },
            tooltip: { isHtml: true },
            legend: { position: 'right', textStyle: { fontSize: 14 } },
            chartArea: { left: 20, top: 50, width: '90%', height: '70%' }
        };

        var chart = new google.visualization.PieChart(document.getElementById('chart_Durus'));
        chart.draw(dataTable, options);
    }
    else
        $("#panel_Chart").css({ display: "none" });
}
