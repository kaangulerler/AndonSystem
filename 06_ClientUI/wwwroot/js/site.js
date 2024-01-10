
let panel_Gosterge = document.getElementById("panel_Gosterge");
let buton_Connection = document.getElementById("buton_Connection");

const connection = new signalR.HubConnectionBuilder()
    .withUrl(url_Api + 'soketHub')
    .withAutomaticReconnect([0, 0, 10, 10])
    .build();

const panel_BaglantiYok = new bootstrap.Modal('#panel_BaglantiYok', {
    keyboard: false,
})

const panel_CalismaYok = new bootstrap.Modal('#panel_CalismaYok', {
    keyboard: false,
})

let yasam_dongusu = 0;
let çalışma_Durum = 0;

function istasyon_Git() {
    window.location.href = url_Terminal + 'Istasyon';
}

document.addEventListener('DOMContentLoaded', function () {

    buton_Connection = document.getElementById("buton_Connection");

    id = istasyonId_Kontrol();
     
    async function start() {
        try {
            await connection.start(); 
            console.clear();
        } catch (err) {
            console.log(err);
            setTimeout(start, 1000);
        }
    };

    connection.onclose(async () => {
        console.log("onclose")
        await start();
    });

    start();

    //connection = new signalR.HubConnectionBuilder()
    //    .withUrl(url_Api + 'soketHub')
    //    .withAutomaticReconnect()
    //    .build();

    connection.on(id, function (xid, message) {
        Zaman = Date.now();
         
        yasam_dongusu = 0;

        let gelen_mesaj = JSON.parse(message);

        if (gelen_mesaj.Fonksiyon == 0) {
            let nesne = JSON.parse(gelen_mesaj.Nesne);
            if (nesne.Calisma.Hedef === 0)
                panelÇalışma(false);
            else {
                panelÇalışma(true);
                calismaGoster(nesne);
            }
        }

        if (gelen_mesaj.Fonksiyon == 1) {

            let nesne = JSON.parse(gelen_mesaj.Nesne);
             
            if (nesne.BarkodTip == 1) {
                barkod_Personel = nesne.Barkod;
            }

            if (nesne.BarkodTip == 2) {
                barkod_Pano = nesne.Barkod;
            }

            if (nesne.BarkodTip == 3) {

                barkod_Durus = nesne.Barkod; 
            }

            mesaj(nesne.Baslik, nesne.Mesaj, nesne.PanelTip); 
        }

        if (gelen_mesaj.Fonksiyon == 10) {
            window.location.reload(true);
        }


        if (message === "Online") {
             
            yasam_dongusu = 0;
        }
    });
     
    setInterval(timer_Yasam, 1000); 
});
  
function istasyonId_Kontrol() {

    let kontrol = cookie_Get("istasyon_id");

    if (kontrol === "" || window.location.href.search("/Istasyon") > -1) {
        document.getElementById("panel_Gosterge").style.visibility = "hidden";
    }
    else {
        document.getElementById("panel_Gosterge").style.visibility = "visible";
    }

    if (kontrol === "" && window.location.href.search("/Istasyon") == -1)
        istasyon_Git();
    else
        return kontrol;
}

function connection_durum(durum) {
    if (id != "" && window.location.href.search("Istasyon") == -1)
        if (durum) {

            buton_Connection.classList.add("btn-primary");
            buton_Connection.classList.remove("btn-danger");
            panel_BaglantiYok.hide();
            if (çalışma_Durum == 1)
                panel_CalismaYok.hide();
            else
                panel_CalismaYok.show();
        }
        else {
            buton_Connection.classList.remove("btn-primary");
            buton_Connection.classList.add("btn-danger");
            panel_BaglantiYok.show();
            panel_CalismaYok.hide();
        }
}

function signalr_mesaj_gonder(nesne) {

    console.log(nesne);
    connection.invoke("Send", id, "Servis", JSON.stringify(nesne)); 
}

function timer_Yasam() {
    if (connection.state === signalR.HubConnectionState.Connected)
        connection.invoke("Send", id, "Servis", "Online"); // Yaşam döngüsü için

    if (yasam_dongusu > 10) {
        connection_durum(false); 
    }
    else
        connection_durum(true);

    yasam_dongusu++;
}