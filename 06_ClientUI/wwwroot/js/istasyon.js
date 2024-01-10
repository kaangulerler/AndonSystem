let list_Istasyon = document.getElementById("list_Istasyon");

Istasyon_Getir();

function Istasyon_Getir() {

    document.getElementById("panel_Gosterge").style.visibility = "hidden";
    
    $.ajax(
        {
            type: "GET",
            url: url_Api + 'api/Client/IstasyonList',
            async: false,
            success: function (result) {

                if (result.islem) {
                     
                    let obje = result.obje;

                    list_Istasyon.innerHTML = "";

                    if (obje != null)
                        for (let i = 0; i < obje.length; i++) {
                            list_Istasyon.innerHTML += '<input type="radio" class="btn-check" name="options-istasyon" id="ist_' + obje[i].id + '" autocomplete="off">' +
                                                        '<label id="label_' + obje[i].id + '" class="btn btn-outline-light m-1" for="ist_' + obje[i].id + '">' + obje[i].kod + '</label>';
                        }

                    let kuki = cookie_Get("istasyon_id");
                     
                    if (kuki != "") {

                        let buton = document.getElementById("label_" + kuki);

                        if (buton !== null) {
                            buton.classList.remove("btn-outline-light");
                            buton.classList.add("btn-primary");
                            buton.disabled = true;
                        }
                    } 
                }
            }
        }
    );

}

function Istasyon_Kaydet() {
     
    let ele = document.getElementsByName("options-istasyon");

    let seçili = "";

    for (i = 0; i < ele.length; i++) {
        if (ele[i].checked) {
            seçili = ele[i].id.replace("ist_", "");
            cookie_Set("istasyon_id", seçili);
        }
    }

    AnaSayfa();

}

function AnaSayfa() {
    window.location.href = url_Terminal;
}