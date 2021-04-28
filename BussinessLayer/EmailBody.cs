using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class EmailBody
    {
        public static string MailHeader(string subject,string siteurl)
        {
            string header = "<head>";
            header = header + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />";
            header = header + "<link href='https://fonts.googleapis.com/css?family=Nunito:300,400,600' rel='stylesheet' />";
            header = header + "<title>"+ subject + "</title>";
            header = header + "<style>body {margin: 0px;padding: 0px;}</style>";
            header = header + "</head>";

            header = header + "<div style='background-color:#f1f1f1;width:100%;max-width:600px;min-height:600px;margin:0px auto;text-align:center;display:block;' font-family: 'Nunito' , sans-serif; color:#000;'";
            header = header + "font-family: 'Nunito', sans-serif; color:#000;'>";
            header = header + "<table border='0' cellpadding='0' cellspacing='0' width='80%' style='min-height:400px;' align='center'>";
            header = header + "<tr>";
            header = header + "<td style='height:60px;'></td>";
            header = header + "</tr>";
            header = header + "<tr>";
            header = header + "<td>";
            header = header + "<table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#fff'>";
            header = header + "<tr><td style='height:60px;'></td></tr>";
            header = header + "<tr>";
            header = header + "<td align='center'>";
            header = header + "<img src='"+ siteurl + "/Content/images/logo_eventnx.png' />";
            header = header + "</td>";
            header = header + "</tr>";
            header = header + "<tr><td style='height:30px;'></td></tr>";
            header = header + "<tr>";
            header = header + "<td style='text-align:center; font-family: Arial, Helvetica, sans-serif; font-size:24px; color:#333333;'>";
            header = header + "<b>"+subject+"</b>";
            header = header + "</td>";
            header = header + "</tr>";
            header = header + "<tr><td style='height:30px;'></td></tr>";
            return header;
        }
        public static string MailFooter(string email)
        {
            string footer = "<tr><td style = 'height:50px;'></td></tr>";
            footer = footer + "</table>";
            footer = footer + "<table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#d8d8d8' style='margin-top:20px;' >";
            footer = footer + "<tr><td style='height:15px;'></td></tr>";
            footer = footer + "<tr>";
            footer = footer + "<td style='font-size:14px;text-align:center'>";
            footer = footer + "<p style='text-align:center; font-family: Arial, Helvetica, sans-serif; font-size:12px; line-height: 18px;'>";
            footer = footer + "<strong> Email : </strong><a href = 'mailto:"+ email + " style='text-decoration: none; color: #000;''> "+ email + "</a><br/>";
            footer = footer + "<strong> Phone : </strong> +91 97261 88777";
            footer = footer + "</p><p style='text-align:center; font-family: Arial, Helvetica, sans-serif; font-size:12px;'>";
            footer = footer + "<strong>";
            footer = footer + "Address :";
            footer = footer + "</strong>";
            footer = footer + "4th Floor, Aarna One, Kalali,";
            footer = footer + " Vadodara, Gujarat, India.";
            footer = footer + "</p>";
            footer = footer + "</td>";
            footer = footer + "</tr>";
            footer = footer + "<tr><td style='height:15px;'></td></tr>";
            footer = footer + "</table>";
            footer = footer + "<table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin-top:20px;' >";
            footer = footer + "<tr><td style='height:30px;'></td></tr>";
            footer = footer + "</table>";
            footer = footer + "</td>";
            footer = footer + "</tr>";
            footer = footer + "</table>";
            footer = footer + "</div>";
            footer = footer + "</body>";
            footer = footer + "</html>";
            return footer;
        }
    }
}
