using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TheCrawler
{
    public partial class Ops
    {
        //public List<Match> FindMatchesByAlgo(List<Match> matches)
        //{
        //    List<Match> lsreturn = new List<Match>();

        //    foreach (var item in matches)
        //    {
        //        item.Bet01_Key = item.Dict.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
        //        item.Bet01_Val = item.Dict.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;

        //        item.Bet01_Diff12 = item.Dict["1"] - item.Dict["2"];
        //        item.Bet01_DiffX1 = item.Dict["X"] - item.Dict["1"];
        //        item.Bet01_Diff2X = item.Dict["2"] - item.Dict["X"];
        //    }

        //    lsreturn.AddRange(matches.OrderBy(o => o.Bet01_Val).ToList());

        //    return lsreturn;
        //}

        //public string PrepareBody(List<Match> matches)
        //{
        //    string content = "";
        //    content = "Meciurile zilei:" + Environment.NewLine;
        //    content += Environment.NewLine;

        //    int iteration = 1;

        //    foreach (var match in matches)
        //    {
        //        content += iteration++.ToString();
        //        content += ". ";
        //        content += match.HomeTeam;
        //        content += " vs ";
        //        content += match.AwayTeam;
        //        content += Environment.NewLine;
        //        content += "Cota minima: ";
        //        content += match.Bet01_Key;
        //        content += ", ";
        //        content += match.Bet01_Val;
        //        content += Environment.NewLine;

        //        content += "1 - 2 = ";
        //        content += match.Bet01_Diff12;
        //        content += Environment.NewLine;
        //        content += "X - 1 = ";
        //        content += match.Bet01_DiffX1;
        //        content += Environment.NewLine;
        //        content += "2 - X = ";
        //        content += match.Bet01_Diff2X;
        //        content += Environment.NewLine;

        //        content += "------------------";
        //        content += Environment.NewLine;

        //        foreach (var dict in match.Dict)
        //        {
        //            content += dict.Key;
        //            content += " -> ";
        //            content += dict.Value;
        //            content += Environment.NewLine;
        //        }

        //        content += Environment.NewLine;
        //    }

        //    return content;
        //}

        public void SendMail(string content)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("oiganbet@gmail.com", "OiganBet1234+");

            List<MailMessage> mails = new List<MailMessage>();
            mails.Add(new MailMessage("oiganbet@gmail.com", "alex_radu@live.com", "Daily Report", content));
            mails.Add(new MailMessage("oiganbet@gmail.com", "alex.bogdan.radu@gmail.com", "Daily Report", content));

            foreach (var mail in mails)
            {
                client.Send(mail);
            }
        }
    }
}
