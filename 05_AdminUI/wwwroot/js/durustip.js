let modal_Durus = new bootstrap.Modal(document.getElementById('modal_Durus'), { keyboard: false });
let modal_Durus_Header = document.getElementById("modal_Durus_Header");
let modal_Durus_Title = document.getElementById("modal_Durus_Title");
let modal_Durus_Input_Kod = document.getElementById("modal_Durus_Input_Kod");
let modal_Durus_Input_DurusId = document.getElementById("modal_Durus_Input_DurusId");
let modal_Durus_Buton_Kaydet = document.getElementById("modal_Durus_Buton_Kaydet");
let modal_Durus_Sil = new bootstrap.Modal(document.getElementById('modal_Durus_Sil'), { keyboard: false });

let modal_Durus_Tasi = new bootstrap.Modal(document.getElementById('modal_Durus_Tasi'), { keyboard: false });
let modal_Durus_Tasi_Header = document.getElementById("modal_Durus_Tasi_Header");
let modal_Durus_Tasi_Title = document.getElementById("modal_Durus_Tasi_Title");
let modal_Durus_Tasi_Input_Kod1 = document.getElementById("modal_Durus_Tasi_Input_Kod1");
let modal_Durus_Tasi_Input_Kod2 = document.getElementById("modal_Durus_Tasi_Input_Kod2");
let modal_Durus_Tasi_Buton_Kaydet = document.getElementById("modal_Durus_Tasi_Buton_Kaydet");

let panel_Detay_Baslik = document.getElementById("panel_Detay_Baslik");
let panel_Detay_Button_Duzenle = document.getElementById("panel_Detay_Button_Duzenle");
let panel_Detay_List = document.getElementById("panel_Detay_List");

let List_TreeMenuAçıkItemler = [];
let Durus_List = [];
let Durus_FullListe = [];
let Durus_Seçili;
let Durus_Taşınan;
let Durus_Bırakılan;
//Seçili linkin özelliklerini değiştirir
document.getElementById("durustipi").classList.add("active");

Durus_Getir();
Durus_Seç("");

function TreeGetir(DurusListesi) {

    if (DurusListesi) {

        //DurusListesi = DurusListesi.sort((a, b) => parseFloat(b.inverseDurusTip.length) - parseFloat(a.inverseDurusTip.length));

        let liste = "";
         
        for (let i = 0; i < DurusListesi.length; i++) {

            Durus_FullListe.push(DurusListesi[i]);

            let id = DurusListesi[i].id;
            let kod = DurusListesi[i].kod;

            let göster = "";

            let aktifmi = "";

            if (Durus_Seçili !== null && Durus_Seçili !== undefined) {

                if (Durus_Seçili.id === id)
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


            if (DurusListesi[i].inverseDurusTip.length > 0) {

                let altDuruslar = TreeGetir(DurusListesi[i].inverseDurusTip);


                let item = '<div id="akordiyon_' + id + '" draggable="true" class="accordion-item p-0 m-0 border-0" style="margin-left:20px; background-color:#121212" ondragstart="Durus_DragStart(this,event)" > ' +
                    ' <h2 class="accordion-header" id="panelhead_' + id + '">' +
                    ' <button id="btn_' + id + '" onclick="Durus_Seç(' + "'" + id + "'" + ')" class="accordionMenusu accordion-button ' + aktifmi + ' collapsed" type="button"ondragover="Durus_AllowDrop(event)" ondrop="Durus_Drop(this,event)" data-bs-toggle="collapse" data-bs-target="#panel_' + id + '" aria-expanded="true" aria-controls="panel_' + id + '">' +
                    '<svg id="ikon_' + id + '" class="bi" width="24" height="24"><use xlink:href="#DurusDolu" /></svg>' +
                    '<span  id="text_' + id + '"style="margin-left:10px">' +
                    kod +
                    '</span>' +
                    '</button>' +
                    '</h2>' +
                    '<div id="panel_' + id + '" class="accordion-collapse collapse p-0  border-0 ' + göster + '" style="margin-left:20px" aria-labelledby="' + id + '">' +
                    '<div class="accordionGovde accordion-body p-0 m-0">' +
                    altDuruslar +
                    '</div>' +
                    '</div>' +
                    '</div>';

                liste += item;
            }
            else {
                let item = '<div id="akordiyon_' + id + '" draggable="true"  class=" p-0 m-0 border-0" style="margin-left:20px ;background-color:#121212" ondragstart="Durus_DragStart(this,event)"> ' +
                    ' <button id="btn_' + id + '" onclick="Durus_Seç(' + "'" + id + "'" + ')" class="btn  text-start flex-grow-1 accordionMenusu  ' + aktifmi + ' w-100" type="button" ondragover="Durus_AllowDrop(event)" ondrop="Durus_Drop(this,event)" data-bs-toggle="collapse" data-bs-target="#panel_' + id + '" aria-expanded="true" aria-controls="panel_' + id + '">' +
                    '<svg  id="ikon_' + id + '" style="margin-left:7px" class="bi" width="24" height="24"><use xlink:href="#DurusBos" /></svg>' +
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

function Durus_Getir() {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Durus/Tree',
            async: false,
            success: function (result) {

                if (result.islem) {
                    Durus_FullListe = [];
                    Durus_Taşınan = null;
                    Durus_Bırakılan = null;
                    Durus_List = result.obje;
                    document.getElementById("treeMenu").innerHTML = TreeGetir(result.obje);
                    Durus_UstDurus(0);

                }
            },
        }
    );
}

function Durus_Seç(id) {

    Durus_Seçili = null;

    let selectList = "";

    for (let i = 0; i < Durus_FullListe.length; i++) {
        let seçili = "";
        if (Durus_FullListe[i].id === id) {
            Durus_Seçili = Durus_FullListe[i];
            seçili = "selected";
            panel_Detay_Baslik.innerHTML = "<b>" + Durus_Seçili.kod + "</b> Durusu";

            Barkod_Goster(Durus_Seçili.barkod, Durus_Seçili.kod);
        }
        selectList += '<option value=' + Durus_FullListe[i].id + '  ' + seçili + '>' + Durus_FullListe[i].kod + '</option>';


    }


    if (Durus_Seçili !== null) {

        panel_Detay_Button_Duzenle.style.display = "block";
        modal_Durus_Title.innerHTML = "Durus Düzenle";

        modal_Durus_Input_Kod.value = Durus_Seçili.kod;
        modal_Durus_Sil_Kod.innerHTML = Durus_Seçili.kod;

        modal_Durus_Header.classList.add('bg-warning');
        modal_Durus_Header.classList.remove('text-white');
        modal_Durus_Header.classList.remove('bg-primary');

        modal_Durus_Buton_Kaydet.classList.add('btn-warning');
        modal_Durus_Buton_Kaydet.classList.remove('btn-primary');
        modal_Durus_Buton_Kaydet.setAttribute("onClick", "javascript: Durus_Duzenle();");

        let aktifDuruslar = document.getElementsByClassName("accordionMenusuAktif");

        if (aktifDuruslar.length > 0)
            for (let i = 0; i < aktifDuruslar.length; i++)
                aktifDuruslar[i].classList.remove("accordionMenusuAktif");

        document.getElementById("btn_" + id).classList.add("accordionMenusuAktif");

        let acikDuruslar = document.getElementsByClassName("show");

        if (acikDuruslar.length > 0)
            for (let i = 0; i < acikDuruslar.length; i++)
                List_TreeMenuAçıkItemler.push(acikDuruslar[i].id.replace("panel_", ""));

        List_TreeMenuAçıkItemler.push(id);


    }
    else {
        panel_Detay_Button_Duzenle.style.display = "none";
    }


}

function Durus_Bul(id) {

    for (let i = 0; i < Durus_FullListe.length; i++)
        if (Durus_FullListe[i].id === id)
            return Durus_FullListe[i];
}

function Durus_Yeni() {

    Durus_Seçili = null;

    modal_Durus_Header.classList.remove('bg-warning');
    modal_Durus_Header.classList.add('text-white');
    modal_Durus_Header.classList.add('bg-primary');

    modal_Durus_Buton_Kaydet.classList.remove('btn-warning');
    modal_Durus_Buton_Kaydet.classList.add('btn-primary');
    modal_Durus_Buton_Kaydet.setAttribute("onClick", "javascript: Durus_Ekle();");
    modal_Durus_Title.innerHTML = "Yeni Durus";
    modal_Durus_Input_Kod.value = "";

}

function Durus_Ekle() {

    let Durusid = "";

    if (Durus_Seçili != null)
        Durusid = Durus_Seçili.id;
    else
        Durusid = yeniGuid;

    if (modal_Durus_Input_Kod.value != "") {

        let obje = { "Kod": modal_Durus_Input_Kod.value, "DurusTipId": Durusid };

        $.ajax({
            type: "POST",
            url: url + "api/Durus",
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.islem == true) {

                    Durus_Getir();
                    $("#modal_Durus .btn-close").click();
                    mesaj("Başarılı", response.obje.kod + " Duruşu başarıyla eklendi.", 0);

                }
                else
                    mesaj("Hata", response.obje.kod + " Duruşu eklenirken hata oluştu.", 1);
                ;

            },
            failure: function (response) {
                alert(response);
            }
        });
    }
    else {
        mesaj("Hata", "Durus Kodu Boş Bırakılamaz", 1);
    }
}

function Durus_Duzenle() {

    if (modal_Durus_Input_Kod.value != "") {

        let obje = { "Kod": modal_Durus_Input_Kod.value };

        $.ajax({
            type: "PUT",
            url: url + "api/Durus/" + Durus_Seçili.id,
            data: JSON.stringify(obje),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.islem == true) {

                    Durus_Getir();
                    $("#modal_Durus .btn-close").click();
                    mesaj("Başarılı", response.obje.kod + " Durusu başarıyla düzenlendi.", 0);
                }
                else
                    mesaj("Hata", response.obje.kod + " Durusu düzenlenirken hata oluştu.", 1);

            },
            failure: function (response) {
                mesaj("Hata", response.obje.kod + " Durusu düzenlenirken hata oluştu.", 1);
            }
        });
    }
    else {

    }
}

function Durus_Sil() {
    if (Durus_Seçili != null) {
        $.ajax({
            type: "DELETE",
            url: url + "api/Durus/" + Durus_Seçili.id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.islem == true) {
                    $("#modal_Durus_Sil .btn-close").click()
                    Durus_Getir();
                    mesaj("Silme Başarılı", response.obje.kod + " Durusu başarıyla silindi.", 0);
                }
                else
                    mesaj("Hata", response.obje.kod + " Durusu silerken hata oluştu.", 1);
            },
            failure: function (response) {
                mesaj("Hata", response.obje.kod + " Durusu silerken hata oluştu.", 1);
            }
        });
    }
}

function Durus_Sil_BosMu() {
    if (Durus_Seçili != null) {
        modal_Durus_Sil.show();
    }
    else
        mesaj("Hata !", "Duruş seçmeden silme işlemi yapılamaz.", 1);
}

document.getElementById("modal_Durus_Sil").addEventListener("hidden.bs.modal", () => { panel_Detay_Baslik.innerHTML = ""; });

function Durus_Tasi() {

    let obje = { "Id1": Durus_Taşınan.id, "Id2": Durus_Bırakılan.id };

    $.ajax({
        type: "POST",
        url: url + "api/Durus/Tasi",
        data: JSON.stringify(obje),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            if (response.islem == true) {

                Durus_Getir();
                $("#modal_Durus_Tasi .btn-close").click();
                mesaj("Başarılı", response.mesaj, 0);

            }
            else
                mesaj("Hata", response.obje.kod + " Durus eklenirken hata oluştu.", 1);
        },
        failure: function (response) {
            alert(response);
        }
    });

}

function Durus_Yeni_DragStart(ev) {
    İşlem = 1;
    Durus_UstDurus(1);
}

function Durus_AllowDrop(ev) {
    ev.preventDefault();

    let id = ev.path[1].id.split("_");

    let panel = document.getElementById("panel_" + id[1]);
    if (panel != null)
        panel.classList.add("show");
    Durus_Seç(id[1]);
}

function Durus_Drag_Yeni(obje) {
    İşlem = 1;
}

function Durus_Drop(obje, ev) {

    if (İşlem === 1) {

        İşlem = 0;
        Durus_Yeni();

        let id = obje.id.replace("btn_", "");

        if (id !== "UstDurus") {
            Durus_Seçili = Durus_Bul(id);

        }
        else {
            Durus_Seçili = null;
        }
        modal_Durus.show();
    }
    else {
        if (İşlem === 2) {

            let id = obje.id.replace("btn_", "");

            if (id !== "UstDurus") {
                Durus_Bırakılan = Durus_Bul(id);

                if (Durus_Taşınan.id !== Durus_Bırakılan.id) {
                    modal_Durus_Tasi_Input_Kod1.innerHTML = Durus_Taşınan.kod;
                    modal_Durus_Tasi_Input_Kod2.innerHTML = Durus_Bırakılan.kod;
                    modal_Durus_Tasi.show();
                }
                else {
                    Durus_Getir();
                }
            }
            else {
                Durus_Bırakılan = {
                    id: yeniGuid
                }
                modal_Durus_Tasi_Input_Kod1.innerHTML = Durus_Taşınan.kod;
                modal_Durus_Tasi_Input_Kod2.innerHTML = "Ana Durus";
                modal_Durus_Tasi.show();
            }
        }
        else {
            if (İşlem === 3) {

                İşlem = 0;
                let üstId = obje.id.replace("btn_", "");
                Durus_Seç(üstId);
            }

        }
    }

}

function Durus_DragStart(obje, ev) {
    İşlem = 2;
    let id = obje.id.replace("akordiyon_", "");
    Durus_Taşınan = Durus_Bul(id);

    if (Durus_Taşınan.DurusId !== null)
        Durus_UstDurus(1);
    Durus_DropKilit(Durus_Taşınan.inverseDurusTip);
    ev.stopPropagation();

}

function Durus_DropKilit(obje) {
    if (obje) {
        for (let i = 0; i < obje.length; i++) {
            let id = obje[i].id;

            $('#btn_' + id).prop("ondragover", null);
            $('#btn_' + id).prop("ondrop", null);

            if (obje[i].inverseDurusTip.length > 0)
                Durus_DropKilit(obje[i].inverseDurusTip);
        }
    }
}

function Durus_UstDurus(goster) {
    if (goster == 1)
        akordiyon_UstDurus.style.display = "block";
    else
        akordiyon_UstDurus.style.display = "none";
}

