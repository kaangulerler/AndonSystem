let List_TreeMenuAçıkItemler = [];
let Lokasyon_List = [];
let Lokasyon_FullListe = [];
let Lokasyon_Seçili;
let Lokasyon_Taşınan;
let Lokasyon_Bırakılan;

let Istasyon_List = [];
let Istasyon_FullListe = [];
let Istasyon_Seçili;

let panel = document.getElementById("panel");

function TreeGetir(lokasyonListesi) {

    if (lokasyonListesi) {

        let liste = "";

        for (let i = 0; i < lokasyonListesi.length; i++) {

            Lokasyon_FullListe.push(lokasyonListesi[i]);

            for (let k = 0; k < lokasyonListesi[i].t3_Istasyon.length; k++)
                Istasyon_FullListe.push(lokasyonListesi[i].t3_Istasyon[k]);

        }
        return liste;
    }
}

function Lokasyon_Getir() {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Lokasyon/Tree',
            async: false,
            success: function (result) {

                if (result.islem) {
                    Lokasyon_FullListe = [];
                    Lokasyon_List = result.obje;
                    Istasyon_FullListe = [];
                    TreeGetir(result.obje);
                }

                //let item_Panel = "";

                //for (let i = 0; i < Lokasyon_FullListe.length; i++) {
                //    let lokasyon = Lokasyon_FullListe[i];

                //    let item_Lokasyon = '<div class="card m-1 border-dark d-flex justify-content-center"> <div class="row g-0"> <div class="col bg-dark text-white rounded-start" style="max-width:42px"><b> <p class="rotate" id="text_LokasyonKod_' + lokasyon.id + '" style="font-size:24px">' + lokasyon.kod + '</p></b> </div> <div class="col row  d-flex justify-content-center m-0">';

                //    for (let j = 0; j < lokasyon.t3_Istasyon.length; j++) {
                //        let istasyon = lokasyon.t3_Istasyon[j];
                //        item_Lokasyon += '<div class="card text-bg-light m-1 p-0 col istasyon"> <div class="card-header p-1"  id="text_IstasyonKod_' + istasyon.id + '"><b><span>' + istasyon.kod + '</span> </b> <button class="btn btn-light float-end p-0 m-0" style="width:30px" onclick="Detay(' + "'" + istasyon.id + "'" + ')"><svg class="bi" width="18" height="18"><use xlink:href="#dashboard-detay" /></svg></button> </div> <div class="card-body p-0"> <p class="card-title m-0" id="text_IstasyonCalismaKod_' + istasyon.id + '"> ### </p> <p class="card-title m-0" id="text_IstasyonUrunKod_' + istasyon.id + '"> ### </p> </div> <div class="card-footer p-0"> <div class="btn-group w-100 align-bottom" role="group"> <button type="button" class="btn btn-dark buton-sayi" id="text_IstasyonHedef_' + istasyon.id + '"> ### </button> <button type="button" class="btn btn-secondary buton-sayi" id="text_IstasyonAktuel_' + istasyon.id + '"> ### </button> <button type="button" class="btn btn-success buton-sayi" id="text_IstasyonDelta_' + istasyon.id + '"> ### </button> </div> </div></div> ';
                //    }

                //    item_Lokasyon += '</div></div></div>';
                //    item_Panel += item_Lokasyon;
                //}

                //panel.innerHTML = item_Panel;
                Tıkla();
            },
        }
    );
}

document.addEventListener('DOMContentLoaded', function () {

    Lokasyon_Getir();
    id = "dashboard";

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

    console.log(obje);

    for (let i = 0; i < obje.length; i++) {

        let istasyon = obje[i];

        let id = istasyon.IstasyonId;

        let item_IstasyonKod = document.getElementById("text_IstasyonKod_" + id);
        let item_IstasyonCalismaKod = document.getElementById("text_IstasyonCalismaKod_" + id);
        let item_IstasyonDurumKod = document.getElementById("text_IstasyonDurumKod_" + id);

        let item_IstasyonUrunKod = document.getElementById("text_IstasyonUrunKod_" + id);
        let item_IstasyonHedef = document.getElementById("text_IstasyonHedef_" + id);
        let item_IstasyonAktuel = document.getElementById("text_IstasyonAktuel_" + id);
        let item_IstasyonDelta = document.getElementById("text_IstasyonDelta_" + id);

        let calisma_kod = istasyon.Calisma.Kod;
        if (calisma_kod == "")
            calisma_kod = "###";

        if (istasyon.DurumTip == 0) {
            item_IstasyonKod.classList.remove("text-bg-danger");
            item_IstasyonKod.classList.remove("text-bg-primary");
            item_IstasyonKod.classList.add("text-bg-light");
        }
        else {
            if (istasyon.DurumTip == 1) {
                item_IstasyonKod.classList.remove("text-bg-danger");
                item_IstasyonKod.classList.add("text-bg-primary");
                item_IstasyonKod.classList.remove("text-bg-light");
            }
            else {
                item_IstasyonKod.classList.add("text-bg-danger");
                item_IstasyonKod.classList.remove("text-bg-primary");
                item_IstasyonKod.classList.remove("text-bg-light");
            }
        }

        item_IstasyonCalismaKod.innerHTML = calisma_kod;
        item_IstasyonHedef.innerHTML = istasyon.Calisma.Hedef;
        item_IstasyonAktuel.innerHTML = istasyon.Calisma.Aktuel;
        item_IstasyonDelta.innerHTML = istasyon.Calisma.Delta;

        if (istasyon.Calisma.Delta > 0) {
            item_IstasyonDelta.classList.remove("btn-danger");
            item_IstasyonDelta.classList.remove("btn-secondary");
            item_IstasyonDelta.classList.add("btn-success");
        }
        else {
            if (istasyon.Calisma.Delta == 0) {
                item_IstasyonDelta.classList.remove("btn-danger");
                item_IstasyonDelta.classList.add("btn-secondary");
                item_IstasyonDelta.classList.remove("btn-success");
            }
            else {
                item_IstasyonDelta.classList.add("btn-danger");
                item_IstasyonDelta.classList.remove("btn-secondary");
                item_IstasyonDelta.classList.remove("btn-success");
            }
        }

        let durum = "";


        item_IstasyonUrunKod.innerHTML = "";

        if (istasyon.ListUretimAktif.length > 0) {

            for (let j = 0; j < istasyon.ListUretimAktif.length; j++) {

                let aktif_uretim = istasyon.ListUretimAktif[j];
                if (j > 0)
                    item_IstasyonUrunKod.innerHTML += "<br/>"
                item_IstasyonUrunKod.innerHTML += aktif_uretim.Kod;

                if (aktif_uretim.DurumKod != null)
                    durum += aktif_uretim.DurumKod;
            }
        }
        else
            item_IstasyonUrunKod.innerHTML = "";

        if (item_IstasyonDurumKod !== null)
            item_IstasyonDurumKod.innerHTML = durum;

    }
}

function Detay(id) {

    console.log(id);
    window.open('/Dashboard?id=' + id, '_blank').focus();
}

function Tıkla() {

    panel.innerHTML = "";
    let item_Panel = "";

    $("input:checkbox[name=istasyon]:checked").each(function () {
        let seçili = ($(this).val());

        for (let i = 0; i < Lokasyon_FullListe.length; i++) {

            let lokasyon = Lokasyon_FullListe[i];
            if (seçili === lokasyon.kod) {

                let item_Lokasyon = '<div class="card m-1 border-dark d-flex justify-content-center"> <div class="row g-0"> <div class="col bg-dark text-white rounded-start" style="max-width:42px"><b> <p class="rotate" id="text_LokasyonKod_' + lokasyon.id + '" style="font-size:24px">' + lokasyon.kod + '</p></b> </div> <div class="col row  d-flex justify-content-center m-0">';

                for (let j = 0; j < lokasyon.t3_Istasyon.length; j++) {
                    let istasyon = lokasyon.t3_Istasyon[j];

                    if (lokasyon.kod == 'MWS')
                        item_Lokasyon += '<div class="card text-bg-light m-1 p-0 col istasyon"> <div class="card-header p-1"  id="text_IstasyonKod_' + istasyon.id + '"><b><span>' + istasyon.kod + '</span> </b> </div> <div class="card-body p-0"> <p class="card-title m-0" id="text_IstasyonCalismaKod_' + istasyon.id + '"> ### </p> <p class="card-title m-0 fs-2" id="text_IstasyonUrunKod_' + istasyon.id + '"> </p> </div> <div class="card-footer p-0"> <div class="btn-group w-100 align-bottom" role="group"> <button type="button" class="btn btn-dark buton-sayi" id="text_IstasyonHedef_' + istasyon.id + '"> ### </button> <button type="button" class="btn btn-secondary buton-sayi" id="text_IstasyonAktuel_' + istasyon.id + '"> ### </button> <button type="button" class="btn btn-success buton-sayi" id="text_IstasyonDelta_' + istasyon.id + '"> ### </button> </div> </div></div> ';
                    else
                        item_Lokasyon += '<div class="card text-bg-light m-1 p-0 col istasyon"> <div class="card-header p-1"  id="text_IstasyonKod_' + istasyon.id + '"><b><span>' + istasyon.kod + '</span> </b> <button class="btn btn-light float-end p-0 m-0" style="width:30px" onclick="Detay(' + "'" + istasyon.id + "'" + ')"><svg class="bi" width="18" height="18"><use xlink:href="#dashboard-detay" /></svg></button> </div> <div class="card-body p-0">  <p class="card-title" id="text_IstasyonDurumKod_' + istasyon.id + '">   </p> <p class="card-title m-0" id="text_IstasyonCalismaKod_' + istasyon.id + '"> ### </p> <p class="card-title m-0 " id="text_IstasyonUrunKod_' + istasyon.id + '"> ### </p> </div> <div class="card-footer p-0"> <div class="btn-group w-100 align-bottom" role="group"> <button type="button" class="btn btn-dark buton-sayi" id="text_IstasyonHedef_' + istasyon.id + '"> ### </button> <button type="button" class="btn btn-secondary buton-sayi" id="text_IstasyonAktuel_' + istasyon.id + '"> ### </button> <button type="button" class="btn btn-success buton-sayi" id="text_IstasyonDelta_' + istasyon.id + '"> ### </button> </div> </div></div> ';

                }

                item_Lokasyon += '</div></div></div>';
                item_Panel += item_Lokasyon;

            }
        }
    });

    panel.innerHTML = item_Panel;
}