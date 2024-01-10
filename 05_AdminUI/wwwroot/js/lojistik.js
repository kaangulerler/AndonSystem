
document.addEventListener('DOMContentLoaded', function () {
     
    id = "lojistik";

    connection = new signalR.HubConnectionBuilder()
        .withUrl(url + 'soketHub')
        .withAutomaticReconnect()
        .build();

    connection.on(id, function (xid, message) {
        Zaman = Date.now(); 

        let gelen_mesaj = JSON.parse(message);
        Mesaj_Ayıkla(gelen_mesaj);
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

function Mesaj_Ayıkla(obje) {
      
    $('#tablo_' + obje.Hat).DataTable({
        "language": {
            "url": "/lib/DataTables/Turkish.json"
        },
        async: false,
        bDestroy: true,
        paging: false,
        ordering: false,
        info: false,
        data: obje.Liste,
        columns: [ 
            { data: 'Istasyon' },
            { data: 'Wbs' },
            { data: 'Panel' }, 
        ],
        pageLength: 50,
        order: [[1, 'asc']]
    });
     
}

