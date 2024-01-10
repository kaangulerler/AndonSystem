document.getElementById("urunler").classList.add("active");
let obje = [];

Data_Getir();

function Data_Getir() {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/Urun',
            async: false,
            success: function (result) {

                obje = JSON.parse(result.obje);
                 
                if (result.islem) {

                    var table = $('#tablo').DataTable({
                        "language": {
                            "url": "/lib/DataTables/Turkish.json"
                        },
                        async: false,
                        bDestroy: true,
                        dom: 'Bfrtip',
                        data: obje,
                        pageLength: 50,
                        columns: [
                            {
                                className: 'dt-control',
                                orderable: false,
                                data: null,
                                defaultContent: '',
                            },
                            { data: 'ProjectId' },
                            { data: 'ProjectName' },
                            { data: 'BomNo' },
                            { data: 'Switchgear' },
                            { data: 'Panel_No' },
                            { data: 'Shortc_Time' },
                            { data: 'Discon' },
                            { data: 'Kod' },
                        ],
                        order: [[1, 'asc']],
                    });

                }

                $('#tablo tbody').on('click', 'td.dt-control', function () {
                    var tr = $(this).closest('tr');
                    var row = table.row(tr);

                    if (row.child.isShown()) {
                        row.child.hide();
                        tr.removeClass('shown');
                    }
                    else {
                        row.child(format(row.data())).show();
                        tr.addClass('shown');
                    }
                });
            },
        }
    );


    function format(d) {
         
        return (
            '<div class="row">' +
            '<div class="col rounded border border-dark urunDetay"> Product : <b>' + d.Tip.Product + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Rated Volt : <b>' + d.Tip.Rated_Volt + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Panel Width : <b>' + d.Tip.Panel_Width + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Panel Type : <b>' + d.Tip.Panel_Type + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Shortc Curr : <b>' + d.Tip.Shortc_Curr + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> CT : <b>' + d.Tip.Ct + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> VT : <b>' + d.Tip.Vt + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Es Present : <b>' + d.Tip.Es_Present + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Es Typr Subs : <b>' + d.Tip.Es_Type_Subs + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Ct Sec Con : <b>' + d.Tip.Ct_Sec_Con + '</b> </div>' +
            '<div class="col rounded border border-dark urunDetay"> Vt Sec Con : <b>' + d.Tip.Vt_Sec_Con + '</b> </div>' + 
            '</div>'
        );
    }

}

function Urun_Seç(id) {
     
    //for (let i = 0; i < obje.length; i++) {
    //    if (obje[i].Id == id) {
    //        let list = [];
    //        list.push(obje[i]);
    //        xpdfindir(list);
    //    }
    //}
   
    for (let item = 0; item < obje.length; item++) {
        if (obje[item].Id === id) {
             
            let Seçili_Ürün = obje[item];
             
            let yazi = Seçili_Ürün.Switchgear + "_" + Seçili_Ürün.Panel_No + "_" + Seçili_Ürün.BomNo ;

            Barkod_Goster(Seçili_Ürün.Barkod, yazi);

            document.getElementById("barcode_download").setAttribute("download", yazi + ".png");
            document.getElementById("barcode_download").setAttribute("href", document.getElementById("barcode").src);
        }
    }

}


   