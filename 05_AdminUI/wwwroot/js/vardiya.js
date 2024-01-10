//Vardiya
let modal_Vardiya = new bootstrap.Modal(document.getElementById('modal_Vardiya'), { keyboard: false });
let modal_Vardiya_Header = document.getElementById("modal_Vardiya_Header");
let modal_Vardiya_Title = document.getElementById("modal_Vardiya_Title");
let modal_Vardiya_Input_Kod = document.getElementById("modal_Vardiya_Input_Kod");
let modal_Vardiya_Input_Bas = document.getElementById("modal_Vardiya_Input_Bas");
let modal_Vardiya_Input_Bit = document.getElementById("modal_Vardiya_Input_Bit");
let modal_Vardiya_Buton_Kaydet = document.getElementById("modal_Vardiya_Buton_Kaydet");
let modal_Vardiya_Button_Kapat = document.getElementById("modal_Vardiya_Button_Kapat");
let Vardiya_Gun_List = document.getElementById("Vardiya_Gun_List");
let modal_Vardiya_Sil = document.getElementById("modal_Vardiya_Sil");
let modal_Vardiya_Sil_Title = document.getElementById("modal_Vardiya_Sil_Title");
let modal_Vardiya_Sil_Body = document.getElementById("modal_Vardiya_Sil_Body");

//Mola
let modal_Mola = new bootstrap.Modal(document.getElementById('modal_Mola'), { keyboard: false });
let modal_Mola_Input_Kod = document.getElementById("modal_Mola_Input_Kod");
let modal_Mola_Input_Bas = document.getElementById("modal_Mola_Input_Bas");
let modal_Mola_Input_Bit = document.getElementById("modal_Mola_Input_Bit");
let modal_Mola_Buton_Kaydet = document.getElementById("modal_Mola_Buton_Kaydet");

//Global değişkenler. 
let mola_List = [];
let Vardiya_FullList = [];
let Vardiya_Info;
let temizle;//Molası temizlenecek olan vardiyanın, mola id listesi. JSON formatında.

//----------------

//HTML tarafındaki vardiya ekle kısmındaki inputlar. Bu inputlar vardiya eklenin altındaki mola inputlarını kontrol eder.
let modal_Vardiya_Mola_Input_Kod = document.getElementById("modal_Vardiya_Mola_Input_Kod");
let modal_Vardiya_Mola_Input_Bas = document.getElementById("modal_Vardiya_Mola_Input_Bas");
let modal_Vardiya_Mola_Input_Bit = document.getElementById("modal_Vardiya_Mola_Input_Bit");
//--------------

document.getElementById("vardiya").classList.add("active");
 
/*Checkbox listelemeyi bu şekilde yapmak daha kullanışlı, çünkü html içeriğine müdahale etme imkanı veriyor*/
const gunler_list = ["Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"];
const html = gunler_list.map(gun => `<label for="gun-${gun}">
            </label>
            <input type="checkbox" name="gun" id="id" value="${gun}"> ${gun}
            <br/>`
).join(' ');
document.querySelector("#Vardiya_Gun_List").innerHTML = html;

function Vardiya_Getir() {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Vardiya',
            async: false,
            success: function (result) {

                if (result.islem) {

                    let obje = [];
                    for (let i = 0; i < result.obje.length; i++) {
                        let vardiya_Gun = [];
                        let item = result.obje[i];
                        if (item.gunPazartesi == true) {
                            vardiya_Gun.push("Pazartesi");
                        } if (item.gunSali == true) {
                            vardiya_Gun.push("Salı");
                        } if (item.gunCarsamba == true) {
                            vardiya_Gun.push("Çarşamba");
                        } if (item.gunPersembe == true) {
                            vardiya_Gun.push("Perşembe");
                        } if (item.gunCuma == true) {
                            vardiya_Gun.push("Cuma");
                        } if (item.gunCumartesi == true) {
                            vardiya_Gun.push("Cumartesi");
                        } if (item.gunPazar == true) {
                            vardiya_Gun.push("Pazar");
                        }


                        let id = item.id;
                        let buton = "<div class='btn-group btn-block w-100' role='group' aria-label='Basic example'>" +
                            "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-dark btn-block' data-bs-toggle='modal' data-bs-target='#modal_Mola' onclick=Mola_Getir('" + id + "')> Molalar </button>" +
                            "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-warning btn-block' data-bs-toggle='modal' data-bs-target='#modal_Vardiya' onclick=Vardiya_Sec(" + '"' + id + '"' + ")> Düzenle </button>" +
                            "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' data-bs-toggle='modal' data-bs-target='#modal_Vardiya_Sil' onclick=Vardiya_Sec(" + '"' + id + '"' + ")> Sil </button>" +
                            "</div>";
                        let kod = item.kod;

                        let bas = item.baslangic;
                        let bit = item.bitis;

                        let list_Mola = [];

                        let molaSure = 0;

                        for (let j = 0; j < item.t3_VardiyaMola.length; j++) {
                            let mola_item = item.t3_VardiyaMola[j];
                             
                            let mola_bas = mola_item.baslangic; 
                            let mola_bit = mola_item.bitis;

                            var fark = mola_item.zaman;
                            molaSure += fark;

                            list_Mola.push({
                                id: mola_item.id,
                                kod: mola_item.kod,
                                bas: mola_bas,
                                bit: mola_bit,
                                zaman : fark,
                            });

                        }

                        obje.push({
                            id: id,
                            kod: kod,
                            bas: bas,
                            bit: bit,
                            molaSure: molaSure,
                            buton: buton,
                            gunler_List: vardiya_Gun,
                            listMola: list_Mola,
                        })
                    }

                    Vardiya_FullList = obje;
                     
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
                            { data: 'bas' },
                            { data: 'bit' },
                            { data: 'molaSure' },
                            { data: 'buton' },
                        ],
                        buttons: [
                            {
                                className: 'btn btn-success xbuton m-2',
                                text: '<a class="btn-block" style="text-decoration:none; color:white" onclick="Vardiya_Yeni()" data-bs-toggle="modal" data-bs-target="#modal_Vardiya"><svg class="bi" width="18" height="18"><use xlink:href="#Ekle" /></svg>  Yeni </a>'
                            },
                        ],

                    });

                }
            },
        }
    );
}

function Vardiya_Bul(id) {
    for (let i = 0; i < Vardiya_FullList.length; i++)
        if (Vardiya_FullList[i].id == id)
            return Vardiya_FullList[i];
}

function Vardiya_Yeni() {
    modal_Vardiya_Header.classList.remove("bg-warning");
    modal_Vardiya_Header.classList.add("bg-primary");
    modal_Vardiya_Title.innerHTML = "Yeni Vardiya";
    modal_Vardiya_Buton_Kaydet.setAttribute("onclick", "Vardiya_Ekle()");
    modal_Vardiya_Input_Kod.value = "";
    modal_Vardiya_Input_Bas.value = "";
    modal_Vardiya_Input_Bit.value = "";
    mola_List = [];
}

function Vardiya_Sec(id) {

    let secili_Vardiya;
    secili_Vardiya = id;
    modal_Vardiya_Sil_Body.innerHTML = "";

    for (let item = 0; item < Vardiya_FullList.length; item++) {
        if (Vardiya_FullList[item].id === secili_Vardiya)
            Vardiya_Info = Vardiya_FullList[item];
    }
    /*for (let k = 0; k < Vardiya_Gun_List.childNodes.length; k++) {
        if (Vardiya_Gun_List.childNodes[i].type == "checkbox")
            Vardiya_Gun_List.childNodes[i].checked = false;
    }*/
    $("#Vardiya_Gun_List").children("input").each(function (index, item) {
        item.checked = false;
    });
    for (let i = 0; i < Vardiya_Gun_List.childNodes.length; i++) {
        for (let j = 0; j < Vardiya_Info.gunler_List.length; j++) {
            if (Vardiya_Gun_List.childNodes[i].value == Vardiya_Info.gunler_List[j])
                Vardiya_Gun_List.childNodes[i].checked = true;
        }
    }


    modal_Vardiya_Sil_Title.innerHTML = " Vardiya Silinecek !";
    modal_Vardiya_Sil_Body.innerHTML = "<h4>" + Vardiya_Info.kod + " Vardiyası silinecek! Onaylıyor Musunuz?" + "</h4>" + "<br>";
    modal_Vardiya_Sil_Body.innerHTML += "<h5>Mola Listesi</h5>" + "<br>";

    let vardiya_min = new Date(24 * 60 * 60 * 1000);
    let vardiya_max = new Date(0);

    for (let i = 0; i < Vardiya_Info.listMola.length; i++) {
        let paket = "<div style='display:flex'>" + "<div class='col-md-3'><h6>Mola Adı: </h6>" + Vardiya_Info.listMola[i].kod + "</div>" +
            "<div class='col-md-3'><h6>Mola Başlagıcı: </h6> " + Vardiya_Info.listMola[i].bas + "</div>" +
            "<div class='col-md-3'><h6>Mola Bitişi: </h6>" + Vardiya_Info.listMola[i].bit + "</div>" + "</div>" + "<br>";

        modal_Vardiya_Sil_Body.innerHTML += paket;

        let item = Vardiya_Info.listMola[i];

        console.log(item);

        let vardiya_bas_saat = item.bas.split(":")[0];
        let vardiya_bas_dakika = item.bas.split(":")[1];
        let vardiya_bit_saat = item.bit.split(":")[0];
        let vardiya_bit_dakika = item.bit.split(":")[1];


        let gun = 0;

        let bas = new Date(((gun * 24) * 60 * 60 * 1000) + (vardiya_bas_saat * 60 * 60 * 1000) + (vardiya_bas_dakika * 60 * 1000));

        if (Vardiya_Info.listMola[i].bas > Vardiya_Info.listMola[i].bit) {
            gun++;
        }

        let bit = new Date(((gun * 24) * 60 * 60 * 1000) + (vardiya_bit_saat * 60 * 60 * 1000) + (vardiya_bit_dakika * 60 * 1000));

        if (vardiya_min > bas) {
            modal_Vardiya_Input_Bas.setAttribute("max", Vardiya_Info.listMola[i].bas)
            vardiya_min = bas;
        }

        if (vardiya_max < bit) {
            modal_Vardiya_Input_Bit.setAttribute("min", Vardiya_Info.listMola[i].bit)
            vardiya_max = bit;
        }
    }

    temizle = JSON.stringify(Vardiya_Info.listMola.map((id) => { return id.id }));

    modal_Vardiya_Header.classList.remove("bg-primary");
    modal_Vardiya_Header.classList.add("bg-warning");
    modal_Vardiya_Header.classList.remove("text-white");
    modal_Vardiya_Header.classList.add("text-dark");
    modal_Vardiya_Title.innerHTML = Vardiya_Info.kod + " Vardiyasını Düzenliyorsunuz !";

    modal_Vardiya_Input_Kod.value = Vardiya_Info.kod;
    modal_Vardiya_Input_Bas.value = Vardiya_Info.bas;
    modal_Vardiya_Input_Bit.value = Vardiya_Info.bit;

    modal_Vardiya_Buton_Kaydet.setAttribute("onclick", "Vardiya_Duzenle(" + "'" + Vardiya_Info.id + "'" + ")");

}

function Vardiya_Ekle() {

    if (modal_Vardiya_Input_Kod.value != "" &&
        modal_Vardiya_Input_Bas.value != "" &&
        modal_Vardiya_Input_Bit.value != "") {


         
        let vardiya = {
            Kod: modal_Vardiya_Input_Kod.value,
            Baslangic: modal_Vardiya_Input_Bas.value,
            Bitis: modal_Vardiya_Input_Bit.value,
        }

        let kontrol = false;
        let liste = [];
        let gun_Child = Vardiya_Gun_List.childNodes;
        for (i = 0; i < gun_Child.length; i += 2) {
            if (gun_Child[i].checked) {
                liste.push(gun_Child[i].value);
            }
        }
        vardiya.gunler_List = liste;
        $.ajax(
            {
                type: "POST",
                url: url + 'api/Vardiya',
                data: JSON.stringify(vardiya),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    if (result.islem) {
                        kontrol = true;
                        mesaj("İşlem Başarılı ", result.obje.kod + " Vardiyası sisteme eklendi.");
                    }
                },
            }
        );

        if (kontrol) {
            Vardiya_Getir();
            modal_Vardiya_Button_Kapat.click();
        }
    }
}

function Vardiya_Sil() {

    $.ajax({
        type: "DELETE",
        url: url + "api/Vardiya/" + Vardiya_Info.id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {
                $("#modal_Vardiya_Sil .btn-close").click()
                Vardiya_Getir();
                mesaj("Vardiya Sil", Vardiya_Info.kod + " vardiya başarıyla silindi.");
            }
            else
                mesaj("Hata " + Vardiya_Info.kod + " Vardiyası silerken hata oluştu.", Vardiya_Info.kod + " Vardiyası silerken hata oluştu.");
        },
        failure: function (response) {
            mesaj("Hata " + Vardiya_Info.kod + " Vardiyası silerken hata oluştu.", Vardiya_Info.kod + " Vardiyası silerken hata oluştu.");
        }
    });
}

function Vardiya_Duzenle(id) {

    if (modal_Vardiya_Input_Kod.value != "" &&
        modal_Vardiya_Input_Bas.value != "" &&
        modal_Vardiya_Input_Bit.value != "") {
         
        let vardiya = {
            VardiyaId: id,
            Kod: modal_Vardiya_Input_Kod.value,
            Baslangic: modal_Vardiya_Input_Bas.value,
            Bitis: modal_Vardiya_Input_Bit.value,
        }

        let kontrol = false;
        let liste = [];
        let gun_Child = Vardiya_Gun_List.childNodes;
        for (i = 0; i < gun_Child.length; i += 2) {
            if (gun_Child[i].checked) {
                liste.push(gun_Child[i].value);
            }
        }
        vardiya.gunler_List = liste;
        $.ajax(
            {
                type: "PUT",
                url: url + 'api/Vardiya/' + id,
                data: JSON.stringify(vardiya),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    if (result.islem) {
                        kontrol = true;
                        mesaj("İşlem Başarılı ", result.obje.kod + " Vardiyası sisteme eklendi.");
                    }
                },
            }
        );

        if (kontrol) {
            Vardiya_Getir();
            modal_Vardiya_Button_Kapat.click();
        }
    }

}



function Mola_Ekle(id) {

    if (modal_Mola_Input_Kod.value != "" &&
        modal_Mola_Input_Bas.value != "" &&
        modal_Mola_Input_Bit.value != "") {


         
        let mola = {
            VardiyaId: id,
            Kod: modal_Mola_Input_Kod.value,
            Baslangic:  modal_Mola_Input_Bas.value,
            Bitis: modal_Mola_Input_Bit.value,
        }


        console.log(mola);


        $.ajax(
            {
                type: "POST",
                url: url + 'api/Mola',
                data: JSON.stringify(mola),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    if (result.islem) {
                        kontrol = true;
                        mesaj("İşlem Başarılı ", result.obje.kod + " molası sisteme eklendi.");
                        Vardiya_Getir();
                        Mola_Getir(id);
                    }
                    else {
                        mesaj("İşlem Başarısız ", result.mesaj, 1);

                    }
                },
            }
        );


    }
}

function Mola_Getir(id) {
    modal_Mola_Buton_Kaydet.setAttribute("onclick", "Mola_Ekle('" + id + "')");

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Mola/' + id,
            async: false,
            success: function (result) {

                if (result.islem) {


                    let obje = [];

                    for (let i = 0; i < result.obje.length; i++) {

                        let item = result.obje[i];

                        let id = item.id;
                        let buton = "<div class='btn-group btn-block w-100' role='group' aria-label='Basic example'>" +
                            "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Mola_Sil('" + id + "')> Sil </button>" +
                            "</div>";

                        let kod = item.kod;

                        let bas = item.baslangic;
                        let bit = item.bitis;


                        obje.push({
                            id: id,
                            kod: kod,
                            bas: bas,
                            bit: bit,
                            molaSure: item.zaman,
                            buton: buton,
                        })
                    }

                    var table = $('#tablo_Mola').DataTable({
                        "language": {
                            "url": "/lib/DataTables/Turkish.json"
                        },
                        async: false,
                        bDestroy: true,
                        dom: 'Bfrtip',
                        data: obje,
                        columns: [
                            { data: 'kod' },
                            { data: 'bas' },
                            { data: 'bit' },
                            { data: 'molaSure' },
                            { data: 'buton' },
                        ],
                        order: [[1, 'asc']],
                    });


                }
            },
        }
    );


}

function Mola_Sil(id) {
    $.ajax({
        type: "DELETE",
        url: url + "api/Mola/" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.islem == true) {

                Vardiya_Getir();
                Mola_Getir(response.obje.vardiyaId);

                mesaj("Mola Sil", Vardiya_Info.kod + " mola başarıyla silindi.");
            }
            else
                mesaj("Hata " + Vardiya_Info.kod + " Vardiyası silerken hata oluştu.", Vardiya_Info.kod + " Vardiyası silerken hata oluştu.");
        },
        failure: function (response) {
            mesaj("Hata " + Vardiya_Info.kod + " Vardiyası silerken hata oluştu.", Vardiya_Info.kod + " Vardiyası silerken hata oluştu.");
        }
    });
}

$(document).ready(function () {
    Vardiya_Getir();
});