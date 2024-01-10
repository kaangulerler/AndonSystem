// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Barkod_Goster(barkod, kod) {

    JsBarcode("#barcode", barkod, {
        text: kod,
        fontSize: 48,
        width:3
    });

    document.getElementById("barcode_download").setAttribute("download", kod + ".png");
    document.getElementById("barcode_download").setAttribute("href", document.getElementById("barcode").src);
}

function pdfindir() {


    let listeData = [];

    let data = {
        barkod: "",
        list: [],
    }

    var doc = new jsPDF();
    doc.text(20, 20, 'Hello world!');
    doc.text(20, 30, 'This is client-side Javascript, pumping out a PDF.');
    doc.addPage();
    doc.text(20, 20, 'Do you like that?');

    let img = document.createElement("img");

    JsBarcode(img, "deneme", {
        text: "Ali"
    });

    doc.setProperties({
        title: 'PDF Title',
        subject: 'Info about PDF',
        author: 'PDFAuthor',
        keywords: 'generated, javascript, web 2.0, ajax',
        creator: 'My Company'
    });

    doc.addPage();
    doc.text(20, 20, 'Hello world!');

    doc.addImage(img.src, "PNG", 15, 40, 180, 180);

    let newWindow = window.open('/');
    fetch(doc.output('datauristring')).then(res => res.blob()).then(blob => {
        newWindow.location = URL.createObjectURL(blob);
    })
}

function menuAktif(index = -1) {
    if (index != -1) {
        document.cookie = index;
    }
    var menuLink = document.getElementsByClassName("menuLink");
    menuLink[parseInt(document.cookie)].addClass = "active";
    console.log(menuLink);
}

function xpdfindir(listeData = []) {

    var doc = new jsPDF();

    //let listeData = [];

    //let data = {
    //    barkod: "",
    //    list : [],
    //}

    //let img = document.createElement("img");
    let img = new Image();

    for (let i = 0; i < 1; i++) {
        let odak = listeData[i];

        JsBarcode(img, odak.Barkod, {
            displayValue: false,
            height: 240,
        });

        let ortalaResim = 10;
        let satir = 100;

        let d1 = odak.ProjectId.toString();
        let d2 = odak.ProjectName.toString();
        let d3 = odak.Switchgear.toString();
        let d4 = odak.Panel_No.toString();

        doc.addImage(img.src, "PNG", 25, 0, 160, 0);

        satir += 70;

        doc.setFontSize(16);
        doc.text(ortalaResim, satir, "PROJECT_NAME");
        satir += 15;

        d2 = d2.replaceAll("İ", "I").replaceAll("Ş", "S").replaceAll("Ğ", "G");

        let harf_say = d2.length;
        if (harf_say < 20)
            doc.setFontSize(32);
        else {
            if (harf_say < 40)
                doc.setFontSize(22);
            else
                doc.setFontSize(16);
        }

        doc.text(ortalaResim, satir, d2);
        satir += 15;
        doc.setFontSize(16);
        doc.text(ortalaResim, satir, "PROJECT_ID");
        doc.text(ortalaResim + 50, satir, "SWITCHGEAR");
        doc.text(ortalaResim + 150, satir, "PANEL_NO");
        satir += 15;
        doc.setFontSize(32);
        doc.text(ortalaResim, satir, d1);
        doc.text(ortalaResim + 50, satir, d3);
        doc.text(ortalaResim + 150, satir, d4);

        if (i == listeData.length - 1) {
            break;
        } else {
            doc.addPage();
        }
    }

    let newWindow = window.open('/');
    fetch(doc.output('datauristring')).then(res => res.blob()).then(blob => {
        newWindow.location = URL.createObjectURL(blob);
    });

}