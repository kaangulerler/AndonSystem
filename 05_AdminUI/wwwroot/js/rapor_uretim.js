Filtre_Getir();

function Rapor_Uretim() {

    let data = {
        "Istasyon_List": $('#filtre_istasyon').val(),
        "Tarih": amount.value,
        "Barkod": $('#filtre_barkod').val(),
        "ProjectId": $('#filtre_projectid').val(),
        "PanelType": $('#filtre_PanelType').val(),
        "PanelWidth": $('#filtre_PanelWidth').val(),
        "PanelCurr": $('#filtre_PanelCurr').val(),
        "ShortCurr": $('#filtre_ShortCurr').val(),
    }
     
    $.ajax(
        {
            type: "POST",
            url: url + 'api/Rapor/Uretim',
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
                            {
                                className: 'dt-control',
                                orderable: false,
                                data: null,
                                defaultContent: '',
                            },
                            { data: 'istasyon' },
                            { data: 'uretimKod' },
                            {
                                data: 'sureHedef',
                                render: function (data, type, row) {
                                    return data.split(".")[0];;
                                }
                            },
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
                                data: 'sureGercek',
                                render: function (data, type, row) {
                                    return data.split(".")[0];;
                                }
                            }
                        ],
                        pageLength: 50,
                        order: [[2, 'asc']]  
                    });

                    $('#tablo tbody').off('click').on('click', 'td.dt-control', function () {
                        var tr = $(this).closest('tr');
                        var row = table.row(tr);

                        if (row.child.isShown()) {
                            row.child.hide();
                            tr.removeClass('shown');
                        } else {
                            console.log(row.data());
                            row.child(format(row.data())).show();
                            tr.addClass('shown');
                        }
                    });
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

                    for (let i = 0; i < nesne.panelCurr.length; i++) {

                        let item = nesne.panelCurr[i];

                        let option = "<option value='" + item + "'>" + item + "</option>";

                        $('#filtre_PanelCurr').append(option);
                    }

                    $('#filtre_PanelCurr').multiSelect({
                        columns: 2,
                        noneText: 'Panel Current',
                        search: true,
                        selectAll: true,
                    });

                    for (let i = 0; i < nesne.panelType.length; i++) {

                        let item = nesne.panelType[i];

                        let option = "<option value='" + item + "'>" + item + "</option>";

                        $('#filtre_PanelType').append(option);
                    }

                    $('#filtre_PanelType').multiSelect({
                        columns: 2,
                        noneText: 'Panel Type',
                        search: true,
                        selectAll: true,

                    });

                    for (let i = 0; i < nesne.panelWidth.length; i++) {

                        let item = nesne.panelWidth[i];

                        let option = "<option value='" + item + "'>" + item + "</option>";

                        $('#filtre_PanelWidth').append(option);
                    }

                    $('#filtre_PanelWidth').multiSelect({
                        columns: 2,
                        noneText: 'Panel Width',
                        search: true,
                        selectAll: true,
                    });

                    for (let i = 0; i < nesne.shortCurr.length; i++) {

                        let item = nesne.shortCurr[i];

                        let option = "<option value='" + item + "'>" + item + "</option>";

                        $('#filtre_ShortCurr').append(option);
                    }

                    $('#filtre_ShortCurr').multiSelect({
                        columns: 2,
                        noneText: 'Short Curr',
                        search: true,
                        selectAll: true,

                    });

                    $('.multi-select-button').addClass('form-control');

                    Rapor_Uretim();
                }
            },
        }
    );
}


function format(d) {

    let içerik = '';
     
    for (let i = 0; i < d.itemList.length; i++) {
        let item = d.itemList[i];

        içerik += '<tr>' +
            '<td>' +
                item.kod +
            '</td>' +
            '<td>' +
            item.baslangic.split('T')[0] + ' ' + item.baslangic.split('T')[1].split('.')[0] + 
            '</td>' +
            '<td>' +
            item.bitis.split('T')[0] + ' ' + item.bitis.split('T')[1].split('.')[0] + 
            '</td>' +
            '<td>' +
                item.zaman.split(".")[0] +
            '</td>' +
            '</tr>'; 
    }
     
    return (
        '<table class="table table-striped table-sm" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;width:100%">' +
        '<thead><tr><td> Kod </td> <td> Başlangıç </td><td> Bitiş </td><td> Zaman </td> </tr></thead>' +
            içerik + 
        '</tr>' +
        '</table><hr/>'
    );
}