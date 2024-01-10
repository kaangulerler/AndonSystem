let ilkCalisma = 0;

let chart_Pie = document.getElementById("chart_Pie");
let chart_Personel = document.getElementById("chart_Personel");
let chart_Durus = document.getElementById("chart_Durus");

let label_Kod = document.getElementById("label_Kod");
let label_T = document.getElementById("label_T");
let label_A = document.getElementById("label_A");
let label_D = document.getElementById("label_D");

document.addEventListener('DOMContentLoaded', function () {

    connection = new signalR.HubConnectionBuilder()
        .withUrl(url + 'soketHub')
        .withAutomaticReconnect()
        .build();

    let kanal = "db_" + id;
     
    connection.on(kanal, function (xid, message) { 
        Zaman = Date.now();
         
        let gelen_mesaj = JSON.parse(message);
        let nesne = JSON.parse(gelen_mesaj.Nesne);
        console.log(nesne);

        drawTimeLineChart(nesne.TimeLine, nesne.UretimSayisi); 

        drawPieChart(nesne.Pie); 

        drawPiePersonelChart(nesne.Pie_Personel_Calisma);

        drawPieDurusChart(nesne.Pie_Durus);

        label_Kod.innerHTML = nesne.Panel.Kod;
        label_T.value = nesne.Panel.Target;
        label_A.value = nesne.Panel.Aktuel;
        label_D.value = nesne.Panel.Delta;

        if (nesne.Panel.Delta == 0) {
            label_D.style.background = "yellow";
            label_D.style.color = "black";
        }

        if (nesne.Panel.Delta < 0) {
            label_D.style.background = "red";
            label_D.style.color = "white";
        }

        if (nesne.Panel.Delta > 0) {
            label_D.style.background = "green";
            label_D.style.color = "white";
        }

    });

    connection.start()
        .then(function () {
        })
        .catch(error => {
            console.error(error.message);
        });

    connection.onreconnecting(error => {
        console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    });

    connection.onreconnected(connectionId => {
        console.assert(connection.state === signalR.HubConnectionState.Connected);
    });

});


function drawTimeLineChart(data, say) {

    if (data.length > 0) {
         
        google.charts.load("current", { packages: ["timeline"] });
        var container = document.getElementById('chart_Timeline');
        var chart = new google.visualization.Timeline(container);
        var dataTable = new google.visualization.DataTable();

        dataTable.addColumn({ type: 'string', id: 'w' });
        dataTable.addColumn({ type: 'string', id: 'q' });
        dataTable.addColumn({ type: 'string', role: 'tooltip' });
        dataTable.addColumn({ type: 'string', id: 'style', role: 'style' });
        dataTable.addColumn({ type: 'date', id: 'Start' });
        dataTable.addColumn({ type: 'date', id: 'End' });

        for (let i = 0; i < data.length; i++)
            dataTable.addRow([data[i][0], data[i][1], data[i][2], data[i][3], new Date(data[i][4]), new Date(data[i][5])]);

        var chartHeight = 120 + ((say + 1) * 48);

        var options = {
            timeline: {
                groupByRowLabel: true,
                rowLabelStyle: {
                    fontName: 'Tahoma',
                    fontSize: 12,
                    color: '#333333'
                },
                barLabelStyle: {
                    fontName: 'Tahoma',
                    fontSize: 12
                },
                tooltip: {
                    isHtml: true,
                    trigger: 'selection'
                }
            },

            hAxis: {
                format: 'dd/MM/yyyy HH:mm'
            },

            avoidOverlappingGridLines: true,
            height: chartHeight,
            width: '100%',

        };

        chart.draw(dataTable, options);
    }

}

function drawPieChart(data) { 

    
    if (data.length > 0) {

        chart_Pie.style.display = "block";

        google.charts.load("current", { packages: ["corechart"] });
         
        var dataTable = new google.visualization.DataTable();

        dataTable.addColumn('string', 'Kod');
        dataTable.addColumn('number', 'Deger');
        dataTable.addColumn({ type: 'string', role: 'tooltip', 'p': { 'html': true } });

        for (let i = 0; i < data.length; i++)
            dataTable.addRow(data[i]);

        var options = {
            title: 'Istasyon Genel Durum',
            pieSliceText: 'label',
            slices: {
                0: { color: "darkgray" },
                1: { offset: 0.1, color: "blue" },
                2: { offset: 0.1, color: "red" },
                3: { offset: 0.1, color: "orange" },
            },
            tooltip: { isHtml: true },
            legend: { position: 'bottom', textStyle: { fontSize: 14 } },
            chartArea: { left: 20, top: 50, width: '90%', height: '70%' }
        };

        var chart = new google.visualization.PieChart(document.getElementById('chart_Pie'));
        chart.draw(dataTable, options);
    }
    else
        chart_Pie.style.display = "none";
}

function drawPiePersonelChart(data) {
     
    if (data.length > 0) {
         
        google.charts.load("current", { packages: ["corechart"] });
        var container = document.getElementById('chart_Personel');
        var chart = new google.visualization.Timeline(container);
        var dataTable = new google.visualization.DataTable();

        dataTable.addColumn('string', 'Kod');
        dataTable.addColumn('number', 'Deger');
        dataTable.addColumn({ type: 'string', role: 'tooltip', 'p': { 'html': true } });

        for (let i = 0; i < data.length; i++)
            dataTable.addRow(data[i]);

        var options = {
            title: 'Personel Çalışmaları',
            pieSliceText: 'label',
            slices: {
                0: { offset: 0.1, color: "Blue" },
                1: { offset: 0.1, color: "SteelBlue" },
                2: { offset: 0.1, color: "DodgerBlue" },
                3: { offset: 0.1, color: "CornflowerBlue" },
                4: { offset: 0.1, color: "DarkBlue" },
                5: { offset: 0.1, color: "RoyalBlue" },
                6: { offset: 0.1, color: "Navy" },
            },
            tooltip: { isHtml: true },
            legend: { position: 'bottom', textStyle: { fontSize: 14 } },
            chartArea: { left: 20, top: 50, width: '90%', height: '70%' }
        };

        var chart = new google.visualization.PieChart(document.getElementById('chart_Personel'));
        chart.draw(dataTable, options);
    } 
}

function drawPieDurusChart(data) {
    if (data.length > 0) {
        google.charts.load("current", { packages: ["corechart"] });
        var container = document.getElementById('chart_Durus');
        var chart = new google.visualization.Timeline(container);
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
            legend: { position: 'bottom', textStyle: { fontSize: 14 } },
            chartArea: { left: 20, top: 50, width: '90%', height: '70%' }
        };

        var chart = new google.visualization.PieChart(document.getElementById('chart_Durus'));
        chart.draw(dataTable, options);
    }
}
