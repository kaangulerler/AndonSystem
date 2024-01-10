
let panel_CalismaVar = document.getElementById("panel_CalismaVar");

let eski_Barkod = "";


let buton_Personel = document.getElementById("buton_Personel");
let buton_Pano = document.getElementById("buton_Pano");
let buton_Durus = document.getElementById("buton_Durus");
let buton_Timer = document.getElementById("buton_Timer");

let label_Zaman = document.getElementById("label_Zaman");

let label_Istasyon = document.getElementById("label_Istasyon");
let label_Calisma = document.getElementById("label_Calisma");
let label_Bas = document.getElementById("label_Bas");
let label_Bit = document.getElementById("label_Bit");
let label_T = document.getElementById("label_T");
let label_A = document.getElementById("label_A");
let label_D = document.getElementById("label_D");

let panel_Aktif = document.getElementById("panel_Aktif");
let tablo_Aktif = document.getElementById("tablo_Aktif");

let panel_Durus = document.getElementById("panel_Durus");
let tablo_Durus = document.getElementById("tablo_Durus");

let panel_Biten = document.getElementById("panel_Biten");
let tablo_Biten;
let tablo_Biten_Data = [];

let input_Barkod = document.getElementById("input_Barkod");
let timer_Sn = setInterval(timer_Saniye, 1000);
let timer_Sn_Sayac = 0;
let timer_Sn_Sıfırla = 30;

barkod_Goster();

input_Barkod.addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
        if (input_Barkod.value.length > 3)
            barkod_Kontrol(input_Barkod.value);
        input_Barkod.value = "";
    }
});

function barkod_Kontrol(barkod) {

    //F0 - Personel
    //F1 - İstasyon
    //F2 - Pano
    //F3 - Duruş Tip
    //F7 - Personel Bitir
    //F8 - Üretim Başlat
    //F9 - Üretim Bitir

    if (barkod.indexOf("IPTAL") > -1) {
         
        let item = {
            Id: id,
            Fonksiyon: 10,
            Nesne: '',
            Tip: false,
            Durum: false
        }
         
        connection.invoke("Send", "Servis", id, JSON.stringify(item)); 
    }

    if (eski_Barkod === barkod) {

        let tip = barkod.substring(0, 2);

        switch (tip) {

            case "F0":
                {
                    let item = {
                        Id: id,
                        Fonksiyon: 1,
                        Nesne: barkod,
                        Tip: false,
                        Durum: false
                    }
                    signalr_mesaj_gonder(item);
                    break;
                }
            case "F1":
                {
                    let item = {
                        Id: id,
                        Fonksiyon: 1,
                        Nesne: barkod,
                        Tip: false,
                        Durum: false
                    }
                    signalr_mesaj_gonder(item);
                    break;
                }
            case "F2":
                {
                    let item = {
                        Id: id,
                        Fonksiyon: 1,
                        Nesne: barkod,
                        Tip: false,
                        Durum: false
                    }
                    signalr_mesaj_gonder(item);
                    break;
                }
            case "F3":
                {
                    barkod_Durus = barkod;

                    if (barkod_Durus != "") {

                        let nesne = {
                            Personel: "",
                            Rulo: id,
                            Pano: barkod_Pano,
                            Durus: barkod_Durus
                        }

                        let item = {
                            Id: id,
                            Fonksiyon: 2,
                            Nesne: JSON.stringify(nesne),
                            Tip: false,
                            Durum: false
                        }

                        barkod_Sıfırla();

                        signalr_mesaj_gonder(item);
                    }

                    break;
                }
            case "F7":
                {
                    if (barkod_Personel != "") {
                        let nesne = {
                            Personel: barkod_Personel,
                            Rulo: id,
                            Pano: "",
                            Durus: ""
                        }

                        let item = {
                            Id: id,
                            Fonksiyon: 7,
                            Nesne: JSON.stringify(nesne),
                            Tip: false,
                            Durum: false
                        }

                        barkod_Sıfırla();
                        signalr_mesaj_gonder(item);
                    }
                    else
                        mesaj("Hata", "Lütfen personel barkodunu okutun.", 0);
                    break;
                }
            case "F8":
                {
                    if (barkod_Personel != "" && barkod_Pano != "") {

                        let nesne = {
                            Personel: barkod_Personel,
                            Rulo: id,
                            Pano: barkod_Pano,
                            Durus: ""
                        }

                        let item = {
                            Id: id,
                            Fonksiyon: 8,
                            Nesne: JSON.stringify(nesne),
                            Tip: false,
                            Durum: false
                        }
                        barkod_Sıfırla();
                        signalr_mesaj_gonder(item);
                    }
                    else
                        mesaj("Hata", "Lütfen önce pano ve personel barkodunu okutun.", 0);

                    break;
                }
            case "F9":
                {
                    let nesne = {
                        Personel: "",
                        Rulo: id,
                        Pano: barkod_Pano,
                        Durus: ""
                    }

                    let item = {
                        Id: id,
                        Fonksiyon: 9,
                        Nesne: JSON.stringify(nesne),
                        Tip: false,
                        Durum: false
                    }
                    barkod_Sıfırla();
                    signalr_mesaj_gonder(item);
                    break;
                }
            default:
                {
                    
                        let item = {
                            Id: id,
                            Fonksiyon: 10,
                            Nesne: '',
                            Tip: false,
                            Durum: false
                        }

                        connection.invoke("Send", "Servis", id, JSON.stringify(item)); 
                       
                }
        }
    }
    else
        eski_Barkod = barkod;
}

function timer_Saniye() {

    if (barkod_Personel != "" || barkod_Durus != "" || barkod_Pano != "" || barkod_Rulo != "") {
        timer_Sn_Sayac++;
        if (timer_Sn_Sayac >= 30) {
            barkod_Sıfırla();
        }
        else {
            let kalan_Zaman = timer_Sn_Sıfırla - timer_Sn_Sayac
            label_Zaman.innerHTML = kalan_Zaman;
        }
        buton_Timer.style.display = "block";
    }
    else {
        buton_Timer.style.display = "none";
    }
    barkod_Goster();
}

function barkod_Sıfırla() {

    console.log("Sıfırla");

    timer_Sn_Sayac = 0;
    eski_Barkod = "";
    barkod_Personel = "";
    barkod_Durus = "";
    barkod_Pano = "";
    barkod_Rulo = "";

    barkod_Goster();
}

function barkod_Goster() {

    if (barkod_Personel != "") {
        buton_Personel.classList.remove("btn-light");
        buton_Personel.classList.add("btn-primary");
    }
    else {
        buton_Personel.classList.remove("btn-primary");
        buton_Personel.classList.add("btn-light");
    }

    if (barkod_Durus != "") {
        buton_Durus.classList.remove("btn-light");
        buton_Durus.classList.add("btn-primary");
    }
    else {
        buton_Durus.classList.remove("btn-primary");
        buton_Durus.classList.add("btn-light");
    }

    if (barkod_Pano != "") {
        buton_Pano.classList.remove("btn-light");
        buton_Pano.classList.add("btn-primary");
    }
    else {
        buton_Pano.classList.remove("btn-primary");
        buton_Pano.classList.add("btn-light");
    }


}

document.addEventListener('DOMContentLoaded', function () {

    $('#tablo_Biten').DataTable({
        "language": {
            "url": "/lib/DataTables/Turkish.json"
        },
        async: false,
        bDestroy: true,
        dom: 'Bfrtip',
        data: tablo_Biten_Data,
        searching: false,
        columns: [
            { data: 'Kod' },
            { data: 'Bas' },
            { data: 'Bit' },
            { data: 'Buton' },
        ],
        order: [[1, 'asc']],
    });
    input_Barkod.focus();
});

function panelÇalışma(varmı) {
    yasam_dongusu = 0;
    if (varmı) {
        çalışma_Durum = 1;
        panel_CalismaVar.style.display = "block";
        input_Barkod.focus();
    }
    else {
        çalışma_Durum = 0;
        panel_CalismaVar.style.display = "none";

    }
}

function calismaGoster(nesne) {

    label_Istasyon.innerHTML = nesne.Istasyon;
    label_Calisma.innerHTML = nesne.Calisma.Kod;
    label_Bas.innerHTML = nesne.Calisma.Bas;
    label_Bit.innerHTML = nesne.Calisma.Bit;
    label_T.value = nesne.Calisma.Hedef;
    label_A.value = nesne.Calisma.Aktuel;
    label_D.value = nesne.Calisma.Delta;

    if (nesne.Calisma.Delta < 0) {
        label_D.style.background = "red";
        label_D.style.color = "white";
    }
    else {
        label_D.style.background = "white";
        label_D.style.color = "black";
    }

    tablo_Aktif.innerHTML = nesne.AktifUretimModel;

}

function uretim_Goster_Biten(obje) {
    var data = obje;
    $('#tablo_Biten').DataTable().clear().rows.add(data).draw();
}


