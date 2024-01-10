using _01_DbModel.Db;
using _01_Servis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Servis.Helpers
{
    public static class Helpers
    {
        public static DateTime GetEarlier(this DateTime first, DateTime second)
            => first > second ? second : first;

        public static DateTime GetLater(this DateTime first, DateTime second)
            => first < second ? second : first;

        public static IReadOnlyCollection<UretimOlay> Merge(this IEnumerable<UretimOlay> olay_list)
            => olay_list
                .OrderBy(olay => olay.Baslangic)
                .Aggregate(new List<UretimOlay>(),
                    (duzenlenmis_olay_list, olay) =>
                    {
                        string header = "";
                        string body = "";
                        string footer = "";
                        string renk = "";
                        string tooltip = "";
                        string tooltip_stil = "";

                        switch (olay.Tip)
                        {
                            case 1:
                                {
                                    header = "Çalışma";
                                    body = "<div class='row'> <b> " + olay.Kod + "</b> </div> " +
                                            "<div class='row'> <div class='col-5'> Başlangıç : </div> <div class='col'><b>" + olay.Baslangic + "</b> </div></div> " +
                                            "<div class='row'> <div class='col-5'> Bitiş : </div>   <div class='col'> <b>" + olay.Bitis + "</b></div></div>" +
                                            "<div class='row'> <div class='col-5'> Toplam : </div>   <div class='col'> <b>" + new DateTime((olay.Bitis - olay.Baslangic).Ticks).ToShortTimeString() + "</b></div></div> <hr/>";
                                    tooltip_stil = "primary";
                                    renk = "blue";
                                    break;
                                }
                            case 2:
                                {
                                    header = "Duruş";
                                    body = "<div class='row'> <b> " + olay.Kod + "</b> </div> " +
                                            "<div class='row'> <div class='col-5'> Başlangıç : </div> <div class='col'><b>" + olay.Baslangic + "</b> </div></div> " +
                                            "<div class='row'> <div class='col-5'> Bitiş : </div>   <div class='col'> <b>" + olay.Bitis + "</b></div></div>" +
                                            "<div class='row'> <div class='col-5'> Toplam : </div>   <div class='col'> <b>" + new DateTime((olay.Bitis - olay.Baslangic).Ticks).ToShortTimeString() + "</b></div></div> <hr/>";
                                    tooltip_stil = "danger";
                                    renk = "red";
                                    break;
                                }
                            case 3:
                                {
                                    header = "Planlı Duruş";
                                    body = "<div class='row'> <b> " + olay.Kod + "</b> </div> " +
                                            "<div class='row'> <div class='col-5'> Başlangıç : </div> <div class='col'><b>" + olay.Baslangic + "</b> </div></div> " +
                                            "<div class='row'> <div class='col-5'> Bitiş : </div>   <div class='col'> <b>" + olay.Bitis + "</b></div></div>" +
                                            "<div class='row'> <div class='col-5'> Toplam : </div>   <div class='col'> <b>" + new DateTime((olay.Bitis - olay.Baslangic).Ticks).ToShortTimeString() + "</b></div></div> <hr/>";
                                    tooltip_stil = "warning";
                                    renk = "orange";
                                    break;
                                }
                        }

                        tooltip = "<div class='card text-bg-" + tooltip_stil + "' style='width: 16rem;'>" +
                                            "<div class='card-header'>" + header + "</div> " +
                                            "<div class='card-body text-bg-light'>" +
                                                body +
                                            "</div >" +
                                            "<div class='card-footer'> " +
                                                footer +
                                   "</div></div> ";


                        olay.Tooltip_Header = header;
                        olay.Tooltip_Body = body;
                        olay.Tooltip_Footer = footer;
                        olay.Tooltip = "<div class='card text-bg-" + tooltip_stil + "' style='width: 16rem;'>" +
                                        "<div class='card-header'>" + header + "</div> " +
                                        "<div class='card-body text-bg-light'>" +
                                            body +
                                        "</div >" +
                                        "<div class='card-footer'> " +
                                            footer +
                                        "</div></div> ";

                        //var sonraki_calisma = olay_list.Where(p => p.Id != olay.Id && p.Tip == olay.Tip && p.Baslangic < olay.Bitis && p.Baslangic > olay.Baslangic).FirstOrDefault();

                        //if (sonraki_calisma != null)
                        //{
                        //    duzenlenmis_olay_list.Add(new UretimOlay
                        //    {
                        //        Id = olay.Id,
                        //        UretimId = olay.UretimId,
                        //        Baslangic = olay.Baslangic,
                        //        Bitis = sonraki_calisma.Baslangic.AddMilliseconds(-1),
                        //        Kod = olay.Kod,
                        //        Renk = olay.Renk,
                        //        Tip = olay.Tip,
                        //        TipKod = olay.TipKod,
                        //        Tooltip = olay.Tooltip,
                        //        Tooltip_Body = olay.Tooltip_Body,
                        //        Tooltip_Header = olay.Tooltip_Header,
                        //        Tooltip_Footer = olay.Tooltip_Footer,
                        //    });

                        //    olay.Baslangic = sonraki_calisma.Baslangic;

                        //    duzenlenmis_olay_list.Add(olay);

                        //    return duzenlenmis_olay_list;
                        //}



                        if (!duzenlenmis_olay_list.Any())
                        {
                            duzenlenmis_olay_list.Add(olay);

                            return duzenlenmis_olay_list;
                        }

                        var latest = duzenlenmis_olay_list.Where(p => p.Id != olay.Id).Last();

                        if (!latest.IsOverlapping(olay))
                        {
                            duzenlenmis_olay_list.Add(olay);
                            return duzenlenmis_olay_list;
                        }

                        if (latest.Bitis < olay.Bitis)
                            olay.Bitis = latest.Bitis;

                        DateTime from = latest.Baslangic.GetEarlier(olay.Baslangic);
                        DateTime to = latest.Bitis.GetLater(olay.Bitis);

                        body = latest.Tooltip_Body + body;
                        footer = latest.Tooltip_Footer + footer;

                        tooltip = "<div class='card text-bg-" + tooltip_stil + "' style='width: 16rem;'>" +
                                            "<div class='card-header'>" + header + "</div> " +
                                            "<div class='card-body text-bg-light'>" +
                                                body +
                                            "</div >" +
                                            "<div class='card-footer'> " +
                                                footer +
                                            "</div></div> ";
                         
                        duzenlenmis_olay_list[^1] = new UretimOlay
                        {
                            Renk = "dark" + renk,
                            Baslangic = from,
                            Bitis = to,
                            UretimId = olay.UretimId,
                            Kod = latest.Kod + " " + olay.Kod,
                            Tip = olay.Tip,
                            TipKod = olay.TipKod,
                            Tooltip = tooltip,
                            Tooltip_Header = header,
                            Tooltip_Footer = footer,
                        };
                         
                        return duzenlenmis_olay_list;
                    });
    }
}
