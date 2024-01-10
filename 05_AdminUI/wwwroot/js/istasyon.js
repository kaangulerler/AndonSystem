let modal_Lokasyon = new bootstrap.Modal(document.getElementById('modal_Lokasyon'), { keyboard: false });
let modal_Lokasyon_Header = document.getElementById("modal_Lokasyon_Header");
let modal_Lokasyon_Title = document.getElementById("modal_Lokasyon_Title");
let modal_Lokasyon_Input_Kod = document.getElementById("modal_Lokasyon_Input_Kod");
let modal_Lokasyon_Input_LokasyonId = document.getElementById("modal_Lokasyon_Input_LokasyonId");
let modal_Lokasyon_Buton_Kaydet = document.getElementById("modal_Lokasyon_Buton_Kaydet");
let modal_Lokasyon_Sil = new bootstrap.Modal(document.getElementById('modal_Lokasyon_Sil'), { keyboard: false });

let modal_Lokasyon_Tasi = new bootstrap.Modal(document.getElementById('modal_Lokasyon_Tasi'), { keyboard: false });
let modal_Lokasyon_Tasi_Header = document.getElementById("modal_Lokasyon_Tasi_Header");
let modal_Lokasyon_Tasi_Title = document.getElementById("modal_Lokasyon_Tasi_Title");
let modal_Lokasyon_Tasi_Input_Kod1 = document.getElementById("modal_Lokasyon_Tasi_Input_Kod1");
let modal_Lokasyon_Tasi_Input_Kod2 = document.getElementById("modal_Lokasyon_Tasi_Input_Kod2");
let modal_Lokasyon_Tasi_Buton_Kaydet = document.getElementById("modal_Lokasyon_Tasi_Buton_Kaydet");

let modal_Istasyon_Tasi = new bootstrap.Modal(document.getElementById('modal_Istasyon_Tasi'), { keyboard: false });
let modal_Istasyon_Tasi_Header = document.getElementById("modal_Istasyon_Tasi_Header");
let modal_Istasyon_Tasi_Title = document.getElementById("modal_Istasyon_Tasi_Title");
let modal_Istasyon_Tasi_Input_Kod1 = document.getElementById("modal_Istasyon_Tasi_Input_Kod1");
let modal_Istasyon_Tasi_Input_Kod2 = document.getElementById("modal_Istasyon_Tasi_Input_Kod2");
let modal_Istasyon_Tasi_Buton_Kaydet = document.getElementById("modal_Istasyon_Tasi_Buton_Kaydet");


let panel_Detay_Baslik = document.getElementById("panel_Detay_Baslik");
let panel_Detay_Button_Duzenle = document.getElementById("panel_Detay_Button_Duzenle");
let panel_Detay_Button_Sil = document.getElementById("panel_Detay_Button_Sil");
let panel_Detay_List = document.getElementById("panel_Detay_List");

let modal_Istasyon = new bootstrap.Modal(document.getElementById('modal_Istasyon'), { keyboard: false });
let modal_Istasyon_Header = document.getElementById("modal_Istasyon_Header");
let modal_Istasyon_Title = document.getElementById("modal_Istasyon_Title");
let modal_Istasyon_Input_Kod = document.getElementById("modal_Istasyon_Input_Kod");
let modal_Istasyon_Input_LokasyonId = document.getElementById("modal_Istasyon_Input_LokasyonId");
let modal_Istasyon_Buton_Kaydet = document.getElementById("modal_Istasyon_Buton_Kaydet");
let modal_Barkod_Buton_Kaydet = document.getElementById("modal_Barkod_Buton_Kaydet");

let modal_Vardiya = new bootstrap.Modal(document.getElementById('modal_Vardiya'), { keyboard: false });
let modal_Vardiya_List = document.getElementById("modal_Vardiya_List");

 
let modal_Plan = new bootstrap.Modal(document.getElementById('modal_Plan'), { keyboard: false });
let modal_Plan_Header = document.getElementById("modal_Plan_Header");
let modal_Plan_Title = document.getElementById("modal_Plan_Title");
let modal_Plan_List = document.getElementById("modal_Plan_List");
let modal_Plan_Input_Aciklama = document.getElementById("modal_Plan_Input_Aciklama");
let modal_Plan_Input_Bas = document.getElementById("modal_Plan_Input_Bas");
let modal_Plan_Input_Bit = document.getElementById("modal_Plan_Input_Bit");
let modal_Plan_Input_Hedef = document.getElementById("modal_Plan_Input_Hedef");


let modal_Durus = new bootstrap.Modal(document.getElementById('modal_Plan'), { keyboard: false });
let modal_Durus_Header = document.getElementById("modal_Durus_Header");
let modal_Durus_Title = document.getElementById("modal_Durus_Title");
let modal_Durus_List = document.getElementById("modal_Durus_List");
let modal_Durus_Input_Aciklama = document.getElementById("modal_Durus_Input_Aciklama");
let modal_Durus_Input_Bas = document.getElementById("modal_Durus_Input_Bas");
let modal_Durus_Input_Bit = document.getElementById("modal_Durus_Input_Bit");

let işlem_Tipi = 0;


let List_TreeMenuAçıkItemler = [];
let Lokasyon_List = [];
let Lokasyon_FullListe = [];
let Lokasyon_Seçili;
let Lokasyon_Taşınan;
let Lokasyon_Bırakılan;

let Istasyon_List = [];
let Istasyon_FullListe = [];
let Istasyon_Seçili;

document.getElementById("istasyon").classList.add("active");

let Vardiya_FullListe = [];

let poplist = [];

Lokasyon_Getir();
Lokasyon_Seç();

function TreeGetir(lokasyonListesi) {

    if (lokasyonListesi) {

        let liste = "";

        for (let i = 0; i < lokasyonListesi.length; i++) {

            Lokasyon_FullListe.push(lokasyonListesi[i]);

            for (let k = 0; k < lokasyonListesi[i].t3_Istasyon.length; k++)
                Istasyon_FullListe.push(lokasyonListesi[i].t3_Istasyon[k]);


            let id = lokasyonListesi[i].id;
            let kod = lokasyonListesi[i].kod;

            let göster = "";

            let aktifmi = "";

            if (Lokasyon_Seçili !== null && Lokasyon_Seçili !== undefined) {

                if (Lokasyon_Seçili.id === id)
                    aktifmi = "accordionMenusuAktif";
                else
                    aktifmi = "";
            }
            else
                aktifmi = "";

            for (let j = 0; j < List_TreeMenuAçıkItemler.length; j++)
                if (id === List_TreeMenuAçıkItemler[j]) {
                    göster = "show";
                    break;
                }

            if (lokasyonListesi[i].inverseUst.length > 0 || lokasyonListesi[i].t3_Istasyon.length > 0) {

                let altLokasyonlar = TreeGetir(lokasyonListesi[i].inverseUst);

                let item = '<div id="akordiyon_' + id + '" draggable="false" class="accordion-item p-0 m-0 border-0" style="margin-left:20px; background-color:#121212" ondragstart="Lokasyon_DragStart(this,event)" > ' +
                    ' <h2 class="accordion-header" id="panelhead_' + id + '">' +
                    ' <button id="btn_' + id + '" onclick="Lokasyon_Seç(' + "'" + id + "'" + ')" class="accordionMenusu accordion-button ' + aktifmi + ' collapsed" type="button"ondragover="Lokasyon_AllowDrop(event)" ondrop="Lokasyon_Drop(this,event)" data-bs-toggle="collapse" data-bs-target="#panel_' + id + '" aria-expanded="true" aria-controls="panel_' + id + '">' +
                    '<svg id="ikon_' + id + '" class="bi" width="24" height="24"><use xlink:href="#LokasyonDolu" /></svg>' +
                    '<span  id="text_' + id + '"style="margin-left:10px">' +
                    kod +
                    '</span>' +
                    '</button>' +
                    '</h2>' +
                    '<div id="panel_' + id + '" class="accordion-collapse collapse p-0  border-0 ' + göster + '" style="margin-left:20px" aria-labelledby="' + id + '">' +
                    '<div class="accordionGovde accordion-body p-0 m-0">' +
                    altLokasyonlar +
                    '</div>' +
                    '</div>' +
                    '</div>';

                liste += item;
            }
            else {
                let item = '<div id="akordiyon_' + id + '" draggable="false"  class=" p-0 m-0 border-0" style="margin-left:20px ;background-color:#121212" ondragstart="Lokasyon_DragStart(this,event)"> ' +
                    ' <button id="btn_' + id + '" onclick="Lokasyon_Seç(' + "'" + id + "'" + ')" class="btn  text-start flex-grow-1 accordionMenusu  ' + aktifmi + ' w-100" type="button" ondragover="Lokasyon_AllowDrop(event)" ondrop="Lokasyon_Drop(this,event)" data-bs-toggle="collapse" data-bs-target="#panel_' + id + '" aria-expanded="true" aria-controls="panel_' + id + '">' +
                    '<svg  id="ikon_' + id + '" style="margin-left:7px" class="bi" width="24" height="24"><use xlink:href="#LokasyonBos" /></svg>' +
                    '<span  id="text_' + id + '" style="margin-left:10px">' +
                    kod +
                    '</span>' +
                    '</button>' +
                    '</div>';

                liste += item;
            }
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
                    Lokasyon_Taşınan = null;
                    Lokasyon_Bırakılan = null;
                    Lokasyon_List = result.obje;
                    Istasyon_FullListe = [];
                    document.getElementById("treeMenu").innerHTML = TreeGetir(result.obje);

                    if (Lokasyon_Seçili != null) { 
                        Lokasyon_Seç(Lokasyon_Seçili.id);
                    }

                    if (Istasyon_Seçili != null)
                        Istasyon_Seç(Istasyon_Seçili.id);

                }
            },
        }
    );

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Vardiya',
            async: false,
            success: function (result) {

                if (result.islem) {
                    modal_Vardiya_List.innerHTML = "";
                    Vardiya_FullListe = result.obje;

                    for (let i = 0; i < result.obje.length; i++) {
                        let vard = result.obje[i];
                        modal_Vardiya_List.innerHTML += '<div class="row mb-1"> <div class="col form-check"> <input class="form-check-input" onchange="Vardiya_Kontrol()" type="checkbox" value="" id="chk_' + vard.id + '">  <label class="form-check-label w-100 row" for="chk_' + vard.id + '"> <div class="col"> ' + vard.kod + ' </div>  <div class="col">( ' + vard.baslangic.slice(0, vard.baslangic.length - 3) + ' - ' + vard.bitis.slice(0, vard.bitis.length - 3) + ' )</div></label> </div> <div class="col-4"> <div class="input-group"><span class="input-group-text">HEDEF</span> <input id="modal_Vardiya_Hedef_' + vard.id + '" class="form-control" type="number" min="1" placeholder="Min 1" value="1"></div> </div></div> ';
                         
                    }
                }
            },
        }
    );
}

function Lokasyon_Seç(id) {

    Lokasyon_Seçili = null;

    let selectList = "";
    modal_Istasyon_Input_LokasyonId.innerHTML = "";

    for (let i = 0; i < Lokasyon_FullListe.length; i++) {
        let seçili = "";
        if (Lokasyon_FullListe[i].id === id) {
            Lokasyon_Seçili = Lokasyon_FullListe[i];
            seçili = "selected";
            panel_Detay_Baslik.innerHTML = "<b>" + Lokasyon_Seçili.kod + "</b> Lokasyonu";

            Plan_Getir(id);
        }
        selectList += '<option value=' + Lokasyon_FullListe[i].id + '  ' + seçili + '>' + Lokasyon_FullListe[i].kod + '</option>';
    }

    modal_Istasyon_Input_LokasyonId.innerHTML = selectList;

    if (Lokasyon_Seçili !== null) {
         
        if (Lokasyon_Seçili.ustId == null)
            panel_Detay_Buton_Sil.style.display = "none";
        else
            panel_Detay_Buton_Sil.style.display = "block";

        panel_Detay_Button_Duzenle.style.display = "block";
        panel_Detay_Button_Yeni.style.display = "block";
        modal_Lokasyon_Title.innerHTML = "Lokasyon Düzenle";

        modal_Lokasyon_Input_Kod.value = Lokasyon_Seçili.kod;
        modal_Lokasyon_Sil_Kod.innerHTML = Lokasyon_Seçili.kod;

        modal_Lokasyon_Header.classList.add('bg-warning');
        modal_Lokasyon_Header.classList.remove('text-white');
        modal_Lokasyon_Header.classList.remove('bg-primary');

        modal_Lokasyon_Buton_Kaydet.classList.add('btn-warning');
        modal_Lokasyon_Buton_Kaydet.classList.remove('btn-primary');
        modal_Lokasyon_Buton_Kaydet.setAttribute("onClick", "javascript: Lokasyon_Duzenle();");

        let aktifLokasyonlar = document.getElementsByClassName("accordionMenusuAktif");

        if (aktifLokasyonlar.length > 0)
            for (let i = 0; i < aktifLokasyonlar.length; i++)
                aktifLokasyonlar[i].classList.remove("accordionMenusuAktif");

        document.getElementById("btn_" + id).classList.add("accordionMenusuAktif");

        let acikLokasyonlar = document.getElementsByClassName("show");

        if (acikLokasyonlar.length > 0)
            for (let i = 0; i < acikLokasyonlar.length; i++)
                List_TreeMenuAçıkItemler.push(acikLokasyonlar[i].id.replace("panel_", ""));

        List_TreeMenuAçıkItemler.push(id);

        panel_Detay_List.innerHTML = "";

        for (let i = 0; i < Lokasyon_Seçili.t3_Istasyon.length; i++) {
            let istasyon = Lokasyon_Seçili.t3_Istasyon[i];

            let vardiyaGoster = '';
            let planliCalismaGoster = '';

            let vardiyaListItem = '';

            let vardiyaList = istasyon.t3_IstasyonVardiya;

             
            for (let x = 0; x < vardiyaList.length; x++)
                vardiyaList[x].t3_Vardiya = Vardiya_Bul(vardiyaList[x].vardiyaId);

            vardiyaList = vardiyaList.sort((a, b) => (a.t3_Vardiya.kod < b.t3_Vardiya.kod) ? -1 : ((b.t3_Vardiya.kod > a.t3_Vardiya.kod) ? 1 : 0));
              
            for (let x = 0; x < vardiyaList.length; x++) {

                let vard = vardiyaList[x].t3_Vardiya;

                let vardiyaKod = vard.kod;
                let vardiyaBas = vard.baslangic;
                let vardiyaBit = vard.bitis;
                 
                vardiyaListItem += '<li class="list-group-item d-flex justify-content-between align-items-start"> ' +
                                    '    <div class="ms-2 me-auto"> ' +
                                    '      <div class="fw-bold">' + vardiyaKod + '</div> ' + 
                                    '(' + vardiyaBas + ' - ' + vardiyaBit + ')' +
                                    '     </div> ' +
                                    '<span class="badge bg-primary rounded-pill fs-6">' + vardiyaList[x].hedef + '</span>'
                                    '</li> ';
            }

            let planliCalismaListItem = '';

            for (let x = 0; x < istasyon.t3_PlanliCalisma.length; x++) {

                let calisma = istasyon.t3_PlanliCalisma[x];

                let calismaKod = calisma.kod;
                let calismaBas = calisma.baslangic;
                let calismaBit = calisma.bitis;

                planliCalismaListItem += '<li class="list-group-item d-flex justify-content-between align-items-start"> ' +
                    '    <div class="ms-2 me-auto"> ' +
                    '      <div class="fw-bold">' + calismaKod + '</div> ' + 
                    '(' + calismaBas + ' - ' + calismaBit + ')' +
                    '     </div> ' +
                    '<span class="badge bg-primary rounded-pill fs-6">' + istasyon.t3_PlanliCalisma[x].hedef + '</span>'
                    '</li> ';
            }


            if (vardiyaList.length > 0)
                vardiyaGoster = '<button class="btn btn-outline-primary" type="button" data-bs-toggle="collapse" data-bs-target="#collapseVardiya_' + istasyon.id + '">Vardiya Listesi</button>' +
                    '<div class="collapse" id="collapseVardiya_' + istasyon.id + '">' +
                    '<ol class="list-group list-group-numbered">' +
                    vardiyaListItem +
                    '</ol>' +
                    '</div> ';

            if (istasyon.t3_PlanliCalisma.length > 0)
                planliCalismaGoster = '<button class="btn btn-outline-dark mt-1" type="button" data-bs-toggle="collapse" data-bs-target="#collapsePlan_' + istasyon.id + '">Planlı Çalışmalar </button>' +
                    '<div class="collapse" id="collapsePlan_' + istasyon.id + '">' +
                    '<ol class="list-group list-group-numbered">' +
                    planliCalismaListItem +
                    '</ol>' +
                    '</div> ';

            panel_Detay_List.innerHTML += '<div class="position-static card text-bg-light m-1 p-1" style="width:300px;"> ' +
                '<div class="card-header">' +
                '<div class = "btn-group"> ' +
                '   <button id="pop_' + istasyon.id + '" class="btn btn-primary" type="button" data-bs-toggle="popover" tabindex="0"><svg class="bi" width="18" height="18"><use xlink:href="#Ayarlar" /></svg> </button> ' +
                '</div> ' +
                '   <span class="card-title" style="margin-left:12px">' + istasyon.kod + '</span> ' +
                '</div>' +
                '<div class="card-body"> ' +
                vardiyaGoster +
                planliCalismaGoster +
                '</div> ' +
                '</div>';
        }

        const list = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))

        for (let i = 0; i < list.length; i++) {

            let id = list[i].id.replace("pop_", "");

            let popupButon = ['<div class="btn-group p-0">',
                '<button onclick="Istasyon_Seç(' + "'" + id + "'" + ')" class="btn btn-success w-100" data-bs-toggle="modal" data-bs-target="#modal_Plan" style="display: block;"> <svg class="bi" width="18" height="18"><use xlink:href="#Vardiya" /></svg></button> ',
                '<button onclick="Istasyon_Seç(' + "'" + id + "'" + ')" class="btn btn-info w-100" data-bs-toggle="modal" data-bs-target="#modal_Vardiya" style="display: block;"> <svg class="bi" width="18" height="18"><use xlink:href="#Vardiya" /></svg></button> ',
                '<button onclick="Istasyon_Seç(' + "'" + id + "'" + ')" class="btn btn-warning w-100" data-bs-toggle="modal" data-bs-target="#modal_Durus" style="display: block;"> <svg class="bi" width="18" height="18"><use xlink:href="#Duraklat" /></svg></button> ',
                '</div> '].join('\n');

            $('#pop_' + id).popover({
                placement: 'bottom',
                html: true,
                sanitize: false,
                content: popupButon,
            });
            poplist.push($('#pop_' + id));
        }

    }
    else {
        panel_Detay_Button_Duzenle.style.display = "none";
        panel_Detay_Button_Yeni.style.display = "none";
    }

}

function Lokasyon_Bul(id) {

    for (let i = 0; i < Lokasyon_FullListe.length; i++)
        if (Lokasyon_FullListe[i].id === id)
            return Lokasyon_FullListe[i];
}

function Lokasyon_Yeni() {

    if (Lokasyon_Seçili !== null && Lokasyon_Seçili != undefined) {
        modal_Lokasyon_Header.classList.remove('bg-warning');
        modal_Lokasyon_Header.classList.add('text-white');
        modal_Lokasyon_Header.classList.add('bg-primary');

        modal_Lokasyon_Buton_Kaydet.classList.remove('btn-warning');
        modal_Lokasyon_Buton_Kaydet.classList.add('btn-primary');
        modal_Lokasyon_Buton_Kaydet.setAttribute("onClick", "javascript: Lokasyon_Ekle();");
        modal_Lokasyon_Title.innerHTML = "Yeni Lokasyon";
        modal_Lokasyon_Input_Kod.value = "";
        modal_Lokasyon.show();
    }
    else
        mesaj("Uyarı", "Lütfen Lokasyon eklemek istediğiniz hattı seçin yada Yeni butonunu üzerine sürükleyin.", 2)
}

function Lokasyon_Ekle() {

    let lokasyonid = "";

    if (Lokasyon_Seçili != null)
        lokasyonid = Lokasyon_Seçili.id;
    else
        lokasyonid = yeniGuid;

    if (modal_Lokasyon_Input_Kod.value != "") {

        let obje = { "Kod": modal_Lokasyon_Input_Kod.value, "LokasyonId": lokasyonid };

        $.ajax({
            type: "POST",
            url: url + "api/Lokasyon",
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.islem == true) {

                    Lokasyon_Getir();
                    $("#modal_Lokasyon .btn-close").click();
                    mesaj("Başarılı", response.obje.kod + " Istasyonu başarıyla eklendi.", 0);

                }
                else
                    mesaj("Hata", response.obje.kod + " Lokasyon eklenirken hata oluştu.", 1);
                ;

            },
            failure: function (response) {
                alert(response);
            }
        });
    }
    else {
        mesaj("Hata", "Lokasyon Kodu Boş Bırakılamaz", 1);
    }
}

function Lokasyon_Duzenle() {

    if (modal_Lokasyon_Input_Kod.value != "") {

        let obje = { "Kod": modal_Lokasyon_Input_Kod.value };

        $.ajax({
            type: "PUT",
            url: url + "api/Lokasyon/" + Lokasyon_Seçili.id,
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.islem == true) {

                    Lokasyon_Getir();
                    $("#modal_Lokasyon .btn-close").click();
                    mesaj("Başarılı", response.obje.kod + " Lokasyonu başarıyla düzenlendi.", 0);
                }
                else
                    mesaj("Hata", response.obje.kod + " Lokasyonu düzenlenirken hata oluştu.", 1);

            },
            failure: function (response) {
                mesaj("Hata", response.obje.kod + " Lokasyonu düzenlenirken hata oluştu.", 1);
            }
        });
    }
    else {

    }
}

function Lokasyon_Sil() {

    $.ajax({
        type: "DELETE",
        url: url + "api/Lokasyon/" + Lokasyon_Seçili.id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {
                $("#modal_Lokasyon_Sil .btn-close").click()
                Lokasyon_Getir();
                mesaj("Silme Başarılı", response.obje.kod + " Lokasyonu başarıyla silindi.", 0);
            }
            else
                mesaj("Hata", response.obje.kod + " Lokasyonu silerken hata oluştu.", 1);
        },
        failure: function (response) {
            mesaj("Hata", response.obje.kod + " Lokasyonu silerken hata oluştu.", 1);
        }
    });
}

function Lokasyon_Sil_BosMu() {
    if (Lokasyon_Seçili != null) {
        modal_Lokasyon_Sil.show();
    }
    else
        mesaj("Hata !", "Lokasyon seçmeden silme işlemi yapılamaz.", 1);
}


function Lokasyon_Tasi() {

    let obje = { "Id1": Lokasyon_Taşınan.id, "Id2": Lokasyon_Bırakılan.id };

    $.ajax({
        type: "POST",
        url: url + "api/Lokasyon/Tasi",
        data: JSON.stringify(obje),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            if (response.islem == true) {

                Lokasyon_Getir();
                $("#modal_Lokasyon_Tasi .btn-close").click();
                mesaj("Başarılı", response.mesaj, 0);

            }
            else
                mesaj("Hata", response.obje.kod + " Lokasyon eklenirken hata oluştu.", 1);
        },
        failure: function (response) {
            alert(response);
        }
    });

}

function Lokasyon_Yeni_DragStart(ev) {
    İşlem = 1;
}

function Lokasyon_AllowDrop(ev) {
    ev.preventDefault();

    let id = ev.path[1].id.split("_");

    let panel = document.getElementById("panel_" + id[1]);
    let buton = document.getElementById("btn_" + id[1]);

    if (panel != null) {

        panel.classList.add("show");
        //buton.classList.add("accordionMenusuAktif");
    }
    // 
    //
}

function Lokasyon_Drag_Yeni(obje) {
    İşlem = 1;
}

function Lokasyon_Drop(obje, ev) {

    Lokasyon_Seç(obje.id.replace("btn_", ""));

    if (İşlem === 1) {

        İşlem = 0;

        let id = obje.id.replace("btn_", "");

        if (id !== "UstLokasyon") {
            Lokasyon_Seçili = Lokasyon_Bul(id);
            Lokasyon_Yeni();
            modal_Lokasyon.show();
        }
        else {
            Lokasyon_Seçili = null;
        }


    }
    else {
        if (İşlem === 2) {

            let id = obje.id.replace("btn_", "");

            if (id !== "UstLokasyon") {
                Lokasyon_Bırakılan = Lokasyon_Bul(id);

                if (Lokasyon_Taşınan.id !== Lokasyon_Bırakılan.id) {
                    modal_Lokasyon_Tasi_Input_Kod1.innerHTML = Lokasyon_Taşınan.kod;
                    modal_Lokasyon_Tasi_Input_Kod2.innerHTML = Lokasyon_Bırakılan.kod;
                    modal_Lokasyon_Tasi.show();
                }
                else {
                    Lokasyon_Getir();
                }
            }
            else {
                Lokasyon_Bırakılan = {
                    id: yeniGuid
                }
                modal_Lokasyon_Tasi_Input_Kod1.innerHTML = Lokasyon_Taşınan.kod;
                modal_Lokasyon_Tasi_Input_Kod2.innerHTML = "Ana Lokasyon";
                modal_Lokasyon_Tasi.show();
            }
        }
        else {
            if (İşlem === 3) {

                modal_Istasyon.show();
                İşlem = 0;
                let üstId = obje.id.replace("btn_", "");
                Istasyon_Seç(üstId);

                Istasyon_Yeni();
            }
            else {
                if (İşlem === 4) {

                    let id = obje.id.replace("btn_", "");

                    if (id !== "UstLokasyon") {
                        Lokasyon_Bırakılan = Lokasyon_Bul(id);
                        modal_Istasyon_Tasi_Input_Kod1.innerHTML = Istasyon_Taşınan.kod;
                        modal_Istasyon_Tasi_Input_Kod2.innerHTML = Lokasyon_Bırakılan.kod;
                        modal_Istasyon_Tasi.show();
                    }
                    else {
                        Lokasyon_Bırakılan = {
                            id: yeniGuid
                        }
                        modal_Istasyon_Tasi_Input_Kod1.innerHTML = Istasyon_Taşınan.kod;
                        modal_Istasyon_Tasi_Input_Kod2.innerHTML = "Ana Lokasyon";
                        modal_Istasyon_Tasi.show();

                    }
                }
            }
        }
    }
}

function Lokasyon_DragStart(obje, ev) {
    İşlem = 2;
    let id = obje.id.replace("akordiyon_", "");
    Lokasyon_Taşınan = Lokasyon_Bul(id);

    if (Lokasyon_Taşınan.lokasyonId !== null)
        Lokasyon_DropKilit(Lokasyon_Taşınan.inverseUst);
    ev.stopPropagation();

}

function Lokasyon_DropKilit(obje) {
    if (obje) {
        for (let i = 0; i < obje.length; i++) {
            let id = obje[i].id;

            $('#btn_' + id).prop("ondragover", null);
            $('#btn_' + id).prop("ondrop", null);

            if (obje[i].inverseUst.length > 0)
                Lokasyon_DropKilit(obje[i].inverseUst);
        }
    }
}

function Lokasyon_Islem_Tip(modal_Baslik) {
    if (modal_Baslik == 0) {
        modal_Plan_Title.innerHTML = Lokasyon_Seçili.kod + " Lokasyonu Fazla Çalışma Planı";

    } else {
        modal_Vardiya_Title.innerHTML = Lokasyon_Seçili.kod + " Lokasyonu Vardiya Planı";

    }
    işlem_Tipi = 1;

}

function Istasyon_Yeni_DragStart(obje) {
    İşlem = 3;
}

function Istasyon_Yeni() {

    modal_Istasyon_Header.classList.remove('bg-warning');
    modal_Istasyon_Header.classList.add('text-white');
    modal_Istasyon_Header.classList.add('bg-primary');

    modal_Istasyon_Buton_Kaydet.classList.remove('btn-warning');
    modal_Istasyon_Buton_Kaydet.classList.add('btn-primary');
    modal_Istasyon_Buton_Kaydet.setAttribute("onClick", "javascript: Istasyon_Ekle();");

    modal_Istasyon_Title.innerHTML = "Yeni Istasyon";
    modal_Istasyon_Input_Kod.value = "";

}

function Istasyon_Seç(id) {

    işlem_Tipi = 0;

    for (let i = 0; i < Istasyon_FullListe.length; i++) {
        if (Istasyon_FullListe[i].id === id) {
            Istasyon_Seçili = Istasyon_FullListe[i];

            Plan_Getir(id);
            Plan_Durus_Getir(id);

            Barkod_Goster(Istasyon_Seçili.barkod, Istasyon_Seçili.kod);

            let chk_List = document.getElementsByClassName("form-check-input");

            for (let i = 0; i < chk_List.length; i++) {

                let item = chk_List[i];
                item.checked = false;
            }

            for (let x = 0; x < Istasyon_Seçili.t3_IstasyonVardiya.length; x++) {
                let vard = Istasyon_Seçili.t3_IstasyonVardiya[x];

                if (vard.aktif) {
                    let chk = document.getElementById("chk_" + vard.vardiyaId);
                    chk.checked = true;
                     
                    let inp = document.getElementById("modal_Vardiya_Hedef_" + vard.vardiyaId);
                    inp.value = vard.hedef; 
                }
            }


            modal_Plan_Title.innerHTML = Istasyon_Seçili.kod + " İstasyonu Fazla Çalışma Planı";
            modal_Vardiya_Title.innerHTML = Istasyon_Seçili.kod + " İstasyonu Vardiya Planı";
            Vardiya_Getir(id);

        }
    }

    modal_Istasyon_Header.classList.add('bg-warning');
    modal_Istasyon_Header.classList.remove('text-white');
    modal_Istasyon_Header.classList.remove('bg-primary');

    modal_Istasyon_Buton_Kaydet.classList.add('btn-warning');
    modal_Istasyon_Buton_Kaydet.classList.remove('btn-primary');
    modal_Istasyon_Buton_Kaydet.setAttribute("onClick", "javascript: Istasyon_Duzenle();");

    modal_Istasyon_Title.innerHTML = "Istasyon Düzenle";

    modal_Istasyon_Input_Kod.value = Istasyon_Seçili.kod;
    modal_Istasyon_Sil_Kod.innerHTML = Istasyon_Seçili.kod;

}

function Istasyon_Ekle() {

    if (modal_Istasyon_Input_Kod.value != "") {

        let obje = {
            "LokasyonId": modal_Istasyon_Input_LokasyonId.value,
            "Kod": modal_Istasyon_Input_Kod.value,
            "SiraNo": 0,
        };

        $.ajax({
            type: "POST",
            url: url + "api/Istasyon",
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (response) {

                if (response.islem == true) {
                    Lokasyon_Getir();
                    $("#modal_Istasyon .btn-close").click();
                    mesaj("Başarılı", response.obje.kod + " Istasyonu başarıyla eklendi.", 0);
                    Lokasyon_Seç(response.obje.lokasyonId);

                }
                else
                    mesaj("Hata", response.obje.kod + " Istasyonu eklerken hata oluştu.", 1);

            },
            failure: function (response) {
                mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
            }
        });
    }
    else {

    }
}

function Istasyon_Duzenle() {

    if (modal_Istasyon_Input_Kod.value != "") {
        let obje = {
            "LokasyonId": modal_Istasyon_Input_LokasyonId.value,
            "Kod": modal_Istasyon_Input_Kod.value,
            "SiraNo": 0,
        };

        $.ajax({
            type: "PUT",
            url: url + "api/Istasyon/" + Istasyon_Seçili.id,
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.islem == true) {

                    Lokasyon_Getir();

                    Lokasyon_Seç(response.obje.lokasyonId);
                    $("#modal_Istasyon .btn-close").click()
                    mesaj("Başarılı", response.obje.kod + " Istasyonı başarıyla düzenlendi.", 0);
                }
                else
                    mesaj("Lokasyona", response.obje.kod + " Istasyonunu düzenlerken hata oluştu.", 1);

            },
            failure: function (response) {
                alert(response);
            }
        });
    }
    else {
        mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
    }
}

function Istasyon_Sil() {

    $.ajax({
        type: "DELETE",
        url: url + "api/Istasyon/" + Istasyon_Seçili.id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {
                $("#modal_Istasyon_Sil .btn-close").click()

                Lokasyon_Getir();

                Lokasyon_Seç(Lokasyon_Seçili.id);

                mesaj("Başarılı", response.obje.kod + " Lokasyonu başarıyla silindi.", 0);
            }
            else
                mesaj("Başarılı", response.obje.kod + " Istasyonu silinirken hata oluştu.", 1);
        },
        failure: function (response) {
            mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
        }
    });
}

function Istasyon_Tasi() {
    // İstasyon taşımak için put ile güncelleme kullanılacakır.

    //let obje = { "Id1": Istasyon_Taşınan.id, "Id2": Lokasyon_Bırakılan.id };

    //$.ajax({
    //    type: "POST",
    //    url: url + "api/Istasyon/Tasi",
    //    data: JSON.stringify(obje),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (response) {

    //        if (response.islem == true) {

    //            Lokasyon_Getir();
    //            $("#modal_Istasyon_Tasi .btn-close").click();
    //            mesaj("Başarılı", response.mesaj, 0);

    //        }
    //        else
    //            mesaj("Hata", response.obje.kod + " Istasyon eklenirken hata oluştu.", 1);
    //    },
    //    failure: function (response) {
    //        alert(response);
    //    }
    //});

    let obje = {
        "LokasyonId": Lokasyon_Bırakılan.id,
        "Kod": Istasyon_Taşınan.kod,
    };

    $.ajax({
        type: "PUT",
        url: url + "api/Istasyon/" + Istasyon_Taşınan.id,
        data: JSON.stringify(obje),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            if (response.islem == true) {



                Lokasyon_Seç(response.obje.lokasyonId);
                $("#modal_Istasyon .btn-close").click()
                mesaj("Başarılı", response.obje.kod + " Istasyonı başarıyla düzenlendi.", 0);
                Lokasyon_Getir();

            }
            else
                mesaj("Lokasyona", response.obje.kod + " Istasyonunu düzenlerken hata oluştu.", 1);

        },
        failure: function (response) {
            alert(response);
        }
    });

}

function Istasyon_Bul(id) {

    for (let i = 0; i < Istasyon_FullListe.length; i++)
        if (Istasyon_FullListe[i].id === id)
            return Istasyon_FullListe[i];
}

function Istasyon_DragStart(obje) {

    İşlem = 4;
    let id = obje.id.replace("tasi_Button_", "");
    Istasyon_Taşınan = Istasyon_Bul(id);
}

function Plan_Ekle() {

    let bas = modal_Plan_Input_Bas.value;
    let bit = modal_Plan_Input_Bit.value;
     
    if (bas >= bit)
        mesaj("Hata", "Bitiş değeri başlanıç değerine eşit veya küçük olamaz.", 1);
    else {

        let obje = {
            "Id": yeniGuid,
            "Aciklama": modal_Plan_Input_Aciklama.value,
            "Baslangic": bas,
            "Bitis": bit,
            "Tip": false,
            "Hedef": 1,
        };

        if (işlem_Tipi == 1) {
            obje.Id = Lokasyon_Seçili.id;
            obje.Tip = true;
        }
        else {
            obje.Id = Istasyon_Seçili.id;
            obje.Hedef = modal_Plan_Input_Hedef.value;
        }

        $.ajax({
            type: "POST",
            url: url + "api/Plan",
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (response) {

                if (response.islem) {

                    if (işlem_Tipi == 0) {
                        Plan_Getir(Istasyon_Seçili.id);
                    }
                    else {
                        Plan_Getir(Lokasyon_Seçili.id);
                    }

                    Lokasyon_Getir();
                }
                else {
                    mesaj("Hata", response.mesaj + "<br>" + response.obje, 1);
                }

            },
            failure: function (response) {
                mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
            }

        });

    }
}

function Plan_Sil(id) {

    $.ajax({
        type: "DELETE",
        url: url + "api/Plan/" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {

                if (işlem_Tipi == 0)
                    Plan_Getir(Istasyon_Seçili.id);
                else
                    Plan_Getir(Lokasyon_Seçili.id);

                mesaj(response.kod, response.mesaj, 0);
            }
            else
                mesaj(response.kod, response.mesaj, 1);
        },
        failure: function (response) {
            mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
        }
    });
}

function Plan_Getir(id) {

    
    $.ajax(
        {
            type: "GET",
            url: url + 'api/Plan/' + id,
            async: false,
            success: function (result) {

                if (result.islem) {
                     
                    try {
                        $('#tablo').DataTable({
                            "language": {
                                "url": "/lib/DataTables/Turkish.json"
                            },
                            async: false,
                            bDestroy: true,
                            dom: 'Bfrtip',
                            data: result.obje,
                            columns: [
                                { data: 'istasyon' },
                                { data: 'kod' },
                                { data: 'bas' },
                                { data: 'bit' },
                                { data: 'hedef' },
                                { data: 'zaman' },
                                { data: 'buton' },
                            ]
                        });
                    }
                    catch (err) {
                        console.log(err.message);
                    }
                }
            },
        }
    );
}

function Vardiya_Bul(id) {

    for (let i = 0; i < Vardiya_FullListe.length; i++)
        if (Vardiya_FullListe[i].id === id)
            return Vardiya_FullListe[i];

    return "";
}

function Vardiya_Kontrol() {

    let chk_List = document.getElementsByClassName("form-check-input");

    for (let i = 0; i < chk_List.length; i++)
        chk_List[i].disabled = false;

    for (let i = 0; i < Vardiya_FullListe.length; i++) {

        //Diğerleri
        let xvard = Vardiya_FullListe[i];
        let item = document.getElementById("chk_" + xvard.id);

        if (item.checked) {
            for (let x = 0; x < Vardiya_FullListe.length; x++) {

                // Seçili olanı al

                let vard = Vardiya_FullListe[x];

                if (xvard.id != vard.id) {
                    let xitem = document.getElementById("chk_" + vard.id);

                    let kontrol = false;
                    xvardBas = addDays(xvard.baslangic, 0);
                    xvardBit = addDays(xvard.bitis, 0);
                    vardBas = addDays(vard.baslangic, 0);
                    vardBit = addDays(vard.bitis, 0);

                    xvardBas.setDate(1);
                    xvardBit.setDate(1);
                    vardBas.setDate(1);
                    vardBit.setDate(1);

                    if (xvardBas > vardBas) {
                        if (xvardBas > xvardBit) {
                            if (vardBas < xvardBit) {
                                kontrol = true;
                            }
                            if (xvardBit > vardBas) {
                                if (xvardBit < vardBit) {
                                    kontrol = true;
                                }
                            }
                        }
                        if (vardBas > vardBit) {
                            kontrol = true;
                        }
                        else {
                            if (xvardBas < vardBit) {
                                kontrol = true;
                            }
                        }
                    } else {
                        if (vardBas > vardBit) {
                            if (xvardBas < vardBit) {
                                kontrol = true;
                            }
                        }
                        if (xvardBas > xvardBit) {
                            if (xvardBit < vardBas) {
                                kontrol = true;
                            }
                            if (xvardBit > vardBas) {
                                kontrol = true;
                            }
                        } else {
                            if (xvardBit > vardBas) {
                                kontrol = true;
                            }
                        }
                    }

                    if (kontrol) {

                        xitem.checked = false;
                        xitem.disabled = true;
                    }
                }
            }

        }
    }
}

function Vardiya_Getir(istasyonId) {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/IstasyonVardiya/' + istasyonId,
            async: false,
            success: function (result) {

                if (result.islem) {
                    let obje = result.obje;

                    Vardiya_Kontrol();
                }
            },
        }
    );
}

function Vardiya_Kaydet() {

    let chk_List = document.getElementsByClassName("form-check-input");
    let kontrol = false;
    let List_VardiyaId = [];

    for (let i = 0; i < chk_List.length; i++) {

        let item = chk_List[i];

        if (item.checked) {
            let id = item.id.replace("chk_", "");
            let hedef = document.getElementById("modal_Vardiya_Hedef_" + id).value;

            let listItem = {
                VardiyaId: id,
                Hedef: hedef,
            };

            List_VardiyaId.push(listItem);
        }
    }

    let id;

    if (işlem_Tipi == 0)
        id = Istasyon_Seçili.id;
    else
        id = Lokasyon_Seçili.id;
     
    $.ajax({
        type: "POST",
        url: url + "api/IstasyonVardiya/" + id,
        data: JSON.stringify(List_VardiyaId),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {

            if (response.islem == true) {
                Lokasyon_Getir();
                $("#modal_Vardiya .btn-close").click();
                mesaj("Başarılı", "Vardiyalar başarıyla kaydedildi.", 0);
            }
            else {
                mesaj("Hata", "Vardiya kaydı sırasında hata oluştu.", 1);
                console.log(response.obje);
            }
        },
        failure: function (response) {
            mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
        }
    });

}

document.getElementById('modal_Durus').addEventListener('shown.bs.modal', event => {
    modal_Durus_Input_Bas.value = moment().format('YYYY-MM-DDT16:00')
    modal_Durus_Input_Bas.min = moment().format('YYYY-MM-DDT16:00')
    modal_Durus_Input_Bit.value = moment().format('YYYY-MM-DDT18:00')
    modal_Durus_Input_Bit.min = moment().format('YYYY-MM-DDT16:00')
    popupKapat()
})

function Planli_Durus_Ekle() {
    let bas = modal_Durus_Input_Bas.value;
    let bit = modal_Durus_Input_Bit.value;
    if (bas >= bit)
        mesaj("Hata", "Bitiş değeri başlanıç değerine eşit veya küçük olamaz.", 1);
    else {

        let obje = {
            "PlanliDurusId": yeniGuid,
            "kod": modal_Durus_Input_Aciklama.value,
            "Baslangic": bas,
            "Bitis": bit,
            "Tip": false,
        };

        if (işlem_Tipi == 1) {
            obje.Id = Lokasyon_Seçili.id;
            obje.Tip = true;
        }
        else
            obje.Id = Istasyon_Seçili.id;

        $.ajax({
            type: "POST",
            url: url + "api/PlanliDurus",
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (response) {
                if (response.islem) {

                    if (işlem_Tipi == 0) {
                        Plan_Durus_Getir(Istasyon_Seçili.id);
                    }
                    else {
                        Plan_Durus_Getir(Lokasyon_Seçili.id);
                    }
                }
                else {
                    mesaj("Hata", response.mesaj + "<br>" + response.obje, 1);
                }

            },
            failure: function (response) {
                mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
            }

        });

    }

}

function Planli_Durus_Sil(id) {

    $.ajax({
        type: "DELETE",
        url: url + "api/PlanliDurus/" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {

                if (işlem_Tipi == 0)
                    Plan_Durus_Getir(Istasyon_Seçili.id);
                else
                    Plan_Durus_Getir(Lokasyon_Seçili.id);

                mesaj(response.kod, response.mesaj, 0);
            }
            else
                mesaj(response.kod, response.mesaj, 1);
        },
        failure: function (response) {
            mesaj("Hata", "Istasyon kodu boş bırakılamaz.", 1);
        }
    });
}

function Plan_Durus_Getir(id) {

    modal_Durus_Title.innerHTML = Istasyon_Seçili.kod + " İstasyonu Planlanmış Duruş.";

    $.ajax(
        {
            type: "GET",
            url: url + 'api/PlanliDurus/' + id,
            async: false,
            success: function (result) {

                if (result.islem) {


                    var table = $('#tabloPlanDurus').DataTable({
                        "language": {
                            "url": "/lib/DataTables/Turkish.json"
                        },
                        async: false,
                        bDestroy: true,
                        dom: 'Bfrtip',
                        data: result.obje,
                        columns: [
                            { data: 'istasyon' },
                            { data: 'kod' },
                            { data: 'bas' },
                            { data: 'bit' },
                            { data: 'zaman' },
                            { data: 'buton' },
                        ]
                    });

                }
            },
        }
    );

}
 
function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

document.getElementById('modal_Plan').addEventListener('shown.bs.modal', event => {
    modal_Plan_Input_Bas.value = moment().format('YYYY-MM-DDT16:00')
    modal_Plan_Input_Bas.min = moment().format('YYYY-MM-DDT16:00')
    modal_Plan_Input_Bit.value = moment().format('YYYY-MM-DDT18:00')
    modal_Plan_Input_Bit.min = moment().format('YYYY-MM-DDT16:00')

    popupKapat();
})

document.getElementById("modal_Lokasyon_Sil").addEventListener("hidden.bs.modal", () => {
    panel_Detay_Baslik.innerHTML = "";
    popupKapat();
});

document.getElementById("modal_Barkod").addEventListener("shown.bs.modal", () => {     popupKapat();      });
document.getElementById("modal_Vardiya").addEventListener("shown.bs.modal", () => {     popupKapat();      });

function popupKapat() { 
    $("[data-bs-toggle='popover']").popover('hide');
}




 