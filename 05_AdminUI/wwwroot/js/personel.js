let modal_Personel = new bootstrap.Modal(document.getElementById('modal_Personel'), { keyboard: false });
let modal_Personel_Header = document.getElementById("modal_Personel_Header");
let modal_Personel_Title = document.getElementById("modal_Personel_Title");
let modal_Personel_Input_Kod = document.getElementById("modal_Personel_Input_Kod");
let modal_Personel_Input_Ad = document.getElementById("modal_Personel_Input_Ad");
let modal_Personel_Input_Soyad = document.getElementById("modal_Personel_Input_Soyad");
let modal_Personel_Buton_Kaydet = document.getElementById("modal_Personel_Buton_Kaydet");
let modal_Personel_Button_Kapat = document.getElementById("modal_Personel_Button_Kapat");
let panel_Detay_Button_Kaydet = document.getElementById("panel_Detay_Button_Kaydet");
let modal_Personel_Sil = document.getElementById("modal_Personel_Sil");
let modal_Personel_Sil_Title = document.getElementById("modal_Personel_Sil_Title");
let modal_Personel_Sil_Body = document.getElementById("modal_Personel_Sil_Body");


let modal_Barkod_Goster = new bootstrap.Modal(document.getElementById('modal_Barkod_Goster'), { keyboard: false });
let modal_Barkod_Header = document.getElementById("modal_Barkod_Header");
let modal_Barkod_Title = document.getElementById("modal_Barkod_Title");
document.getElementById("personel").classList.add("active");

Personel_Getir();

let Seçili_Personel;

function Personel_Getir() {
    
    $.ajax(
        {
            type: "GET",
            url: url + 'api/Personel',
            async: false,
            success: function (result) {
                 
                if (result.islem) {
                    let obje = [];
                     
                    for (let i = 0; i < result.obje.length; i++) {

                        let item = result.obje[i];

                        let id = item.id;
                        let buton = "<div class='btn-group btn-block w-100' role='group' aria-label='Basic example'>" +
                            "<button style='max-height:50px;min-width:30px; height:40px' class='btn btn-dark' data-bs-toggle='modal' data-bs-target='#modal_Barkod_Goster' onclick=Personel_Sec(" + '"' + id + '"' + ")> <svg class='bi' width='18' height='18'><use xlink:href='#Urun' /></svg>Barkod</button>" +
                            "<button style='max-height:50px;min-width:30px; height:40px' type='button' class='btn btn-warning btn-block' data-bs-toggle='modal' data-bs-target='#modal_Personel' onclick=Personel_Sec(" + '"' + id + '"' + ")> <svg class='bi' width='18' height='18'><use xlink:href='#duzenle' /></svg>Düzenle </button>" +
                            "<button style='max-height:50px;min-width:30px; height:40px' type='button' class='btn btn-danger btn-block' data-bs-toggle='modal' data-bs-target='#modal_Personel_Sil' onclick=Personel_Sec(" + '"' + id + '"' + ")><svg class='bi' width='18' height='18'><use xlink:href='#Kaldir' /></svg> Sil </button>" +
                            "</div>";
                        let kod = item.kod;
                        let barkod = item.barkod;
                        let ad = item.ad;
                        let soyad = item.soyad;


                        obje.push({
                            id: id,
                            kod: kod,
                            ad: ad,
                            soyad: soyad,
                            barkod: barkod,
                            buton: buton
                        })
                    }

                    Personel_FullList = obje;
                     
                    var table = $('#tablo').DataTable({
                        "language": {
                            "url": "/lib/DataTables/Turkish.json"
                        },
                        async: false,
                        bDestroy: true,
                        dom: 'Bfrtip',
                        data: obje,
                        columns: [
                            { data: 'kod' },
                            { data: 'ad' },
                            { data: 'soyad' },
                            { data: 'buton' },
                        ],
                        buttons: [
                            {
                                className: 'btn btn-success xbuton m-2',
                                text: '<a class="btn-block" style="text-decoration:none; color:white" onclick="Personel_Yeni()" data-bs-toggle="modal" data-bs-target="#modal_Personel"><svg class="bi" width="18" height="18"><use xlink:href="#Ekle" /></svg>  Yeni </a>'
                            },
                        ],
                        order: [[1, 'asc']],
                    });

                }
            },
        }
    );
}

function Personel_Bul(id) {
    for (let i = 0; i < Personel_FullList.length; i++)
        if (Personel_FullList[i].id == id)
            return Personel_FullList[i];
}

function Personel_Yeni() {
    modal_Personel_Header.classList.remove("bg-warning");
    modal_Personel_Header.classList.add("bg-primary");
    modal_Personel_Title.innerHTML = "Yeni Personel";
    modal_Personel_Buton_Kaydet.setAttribute("onclick", "Personel_Ekle()");
    modal_Personel_Input_Kod.value = "";
}

function Personel_Sec(id) {
     
    modal_Personel_Sil_Body.innerHTML = "";

    for (let item = 0; item < Personel_FullList.length; item++) {
        if (Personel_FullList[item].id === id) {

            Seçili_Personel = Personel_FullList[item];

            let yazi = Seçili_Personel.ad + " " + Seçili_Personel.soyad;

            Barkod_Goster(Seçili_Personel.barkod, yazi);

            document.getElementById("barcode_download").setAttribute("download", yazi + ".png");
            document.getElementById("barcode_download").setAttribute("href", document.getElementById("barcode").src);
        }
    }
    modal_Personel_Sil_Title.innerHTML = " Personel Silinecek !";
    modal_Personel_Sil_Body.innerHTML = "<h4>" + Seçili_Personel.kod + " (" + Seçili_Personel.ad + " " + Seçili_Personel.soyad + ")  personel silinecek! Onaylıyor Musunuz?" + "</h4>" + "<br>";
    
    modal_Personel_Header.classList.remove("bg-primary");
    modal_Personel_Header.classList.add("bg-warning");
    modal_Personel_Header.classList.remove("text-white");
    modal_Personel_Header.classList.add("text-dark");
    modal_Personel_Title.innerHTML = Seçili_Personel.kod + " Personelini Düzenliyorsunuz !";

    modal_Personel_Input_Kod.value = Seçili_Personel.kod;
    modal_Personel_Input_Ad.value = Seçili_Personel.ad;
    modal_Personel_Input_Soyad.value = Seçili_Personel.soyad;

    modal_Personel_Buton_Kaydet.setAttribute("onclick", "Personel_Duzenle(" + "'" + Seçili_Personel.id + "'" + ")");

}

function Personel_Ekle() {
    if (modal_Personel_Input_Kod.value != "") {
         
        let Personel = {
            Kod: modal_Personel_Input_Kod.value, 
            Ad: modal_Personel_Input_Ad.value, 
            Soyad: modal_Personel_Input_Soyad.value, 
        }

        let kontrol = false;
        $.ajax(
            {
                type: "POST",
                url: url + 'api/Personel',
                data: JSON.stringify(Personel),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.islem) {
                        kontrol = true;
                        mesaj(result.kod, result.mesaj, 0);
                    }
                    else {
                        mesaj("İşlem Başarısız ", result.mesaj, 1);
                    }

                },
            }
        );

        if (kontrol) {
            Personel_Getir();
            modal_Personel_Button_Kapat.click();
        }
    }
}

function Personel_Sil() {

    $.ajax({
        type: "DELETE",
        url: url + "api/Personel/" + Seçili_Personel.id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {
                $("#modal_Personel_Sil .btn-close").click()
                Personel_Getir();
                mesaj("Personel Sil", Seçili_Personel.kod + " Personel başarıyla silindi.");
            }
            else
                mesaj("Hata " + Seçili_Personel.kod + " Personelsı silerken hata oluştu.", Seçili_Personel.kod + " Personelsı silerken hata oluştu.");
        },
        failure: function (response) {
            mesaj("Hata " + Seçili_Personel.kod + " Personelsı silerken hata oluştu.", Seçili_Personel.kod + " Personelsı silerken hata oluştu.");
        }
    });
}

function Personel_Duzenle() {

    if (modal_Personel_Input_Kod.value != "") {

        let personel = {
            PersonelId: Seçili_Personel.id,
            Kod: modal_Personel_Input_Kod.value,
            Ad: modal_Personel_Input_Ad.value,
            Soyad: modal_Personel_Input_Soyad.value, 
        }

        let kontrol = false;
        $.ajax(
            {
                type: "PUT",
                url: url + 'api/Personel/' + Seçili_Personel.id,
                data: JSON.stringify(personel),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.islem) {
                        kontrol = true;
                        mesaj(result.kod, result.mesaj, 0);
                    }
                    else {
                        mesaj("İşlem Başarısız ", result.mesaj, 1);
                    }
                },
            }
        );

        if (kontrol) {
            Personel_Getir();
            modal_Personel_Button_Kapat.click();
        }
    }

}