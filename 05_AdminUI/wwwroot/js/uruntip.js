let modal_UrunTipYukle = new bootstrap.Modal(document.getElementById('modal_UrunTipYukle'), { keyboard: false });
document.getElementById("uruntipi").classList.add("active");

Data_Getir();
 
function Data_Getir() {

    $.ajax(
        {
            type: "GET",
            url: url + 'api/UrunTip',
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
                            { data: 'Product' },
                            { data: 'Rated_Volt' },
                            { data: 'Panel_Width' },
                            { data: 'Panel_Type' },
                            { data: 'Panel_Curr' },
                            { data: 'Shortc_Curr' }, 
                            { data: 'Ct' }, 
                            { data: 'Vt' }, 
                        ], 
                        order: [[1, 'asc']],
                    });

                }

                $('#tablo tbody').on('click', 'td.dt-control', function () {
                    var tr = $(this).closest('tr');
                    var row = table.row(tr);

                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                        tr.removeClass('shown');
                    } else {
                        // Open this row
                        row.child(format(row.data())).show();
                        tr.addClass('shown');
                    }
                });
            },
        }
    );


    function format(d) {
         
        let altTablo = '';

        if (d.Liste.length > 0) {

            altTablo = '';
            altTablo = '<div class="row m-2 border" style="width:500px">' +
                '<div class="row border-bottom"><div class="col"> <b> İstasyon </b> </div><div class="col"> <b> Dakika  </b></div></div>';

            for (let i = 0; i < d.Liste.length; i++) {
                let item = d.Liste[i];

                altTablo += '<div class="row"><div class="col">' + item.Istasyon + ' </div><div class="col">' + item.Zaman + '</div></div>';

            }
            altTablo += '</div>';
        } 

        return (
                '<table cellpadding="5" cellspacing="0" border="0" style="margin-left:100px;width:100%">' +
                '<tr style="width:100%">' +
                '<td>VT_WITH: <b>' +
                d.Vt_With +
                '</b></td>' +
                '<td>VT_REM:  <b>' +
                d.Vt_Rem +
                '</b></td>' +
                '<td>VT_FIX: <b>' +
                d.Vt_Fix +
                '</b></td>' +
                '<td>SA:  <b>' +
                d.Sa +
                '</b></td>' +
                '<td>CAP_VOLT_IND:  <b>' +
                d.Cap_Volt_Ind +
                '</b></td>' +
                '</tr>' +
                '<tr>' +
                '<td>ES_PRESENT:  <b>' +
                d.Es_Present +
                '</b></td>' +
                '<td>ES_TYPE_SUBS:  <b>' +
                d.Es_Type_Subs +
                '</b></td>' +
                '<td>CT_SEC_CON:  <b>' +
                d.Ct_Sec_Con +
                '</b></td>' +
                '<td>VT_SEC_CON:  <b>' +
                d.Vt_Sec_Con +
                '</b></td>' +
                '</tr>' +
                '</table>' +
                '<div class="row text-center d-flex justify-content-center" style="margin:left:100px">' +
                altTablo +
                '</div>'
            );
         
    }

    

}
 