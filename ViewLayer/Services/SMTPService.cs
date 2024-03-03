using System.Net;
using System.Net.Mail;


namespace ViewLayer.Services
{
    public class SMTPService
    {
        public SMTPService()
        {
            from = new MailAddress("internetmanager072@gmail.com", "Интернет провайдер 'Мега-интернет'");
            smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("internetmanager072@gmail.com", "otmhrvkukipnimqv");
            smtp.EnableSsl = true;
        }

        private readonly MailAddress from;
        private readonly SmtpClient smtp;
        private MailAddress to;
        private MailMessage message;


        public void SendMessageToEmail(string toEmail, string subject, string titleText, string bodyText1, string bodyText2)
        {
            to = new MailAddress($"{toEmail}");
            message = new MailMessage(from, to);
            message.IsBodyHtml = true;
            message.Subject = $"{subject}";
            message.Body = CreateHtmlMessage(titleText, bodyText1, bodyText2);
            smtp.EnableSsl = true;
            smtp.Send(message);
        }

        private string CreateHtmlMessage(string titleText, string bodyText1, string bodyText2)
        {
            string htmlMessage = $"<link href=\"//maxcdn.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css\" rel=\"stylesheet\" id=\"bootstrap-css\">\r\n" +
                $"<script src=\"//maxcdn.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js\"></script>\r\n<script src=\"//cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js\"></script>\r\n" +
                $"<!------ Include the above in your HEAD tag ---------->\r\n\r\n" +
                $"<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n    <title></title>\r\n" +
                $"    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n" +
                $"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
                $"    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n" +
                $"    <style type=\"text/css\">\r\n        @media screen {{\r\n" +
                $"            @font-face {{\r\n" +
                $"                font-family: 'Lato';\r\n" +
                $"                font-style: normal;\r\n" +
                $"                font-weight: 400;\r\n" +
                $"                src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n" +
                $"            }}\r\n\r\n" +
                $"            @font-face {{\r\n" +
                $"                font-family: 'Lato';\r\n" +
                $"                font-style: normal;\r\n" +
                $"                font-weight: 700;\r\n" +
                $"                src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n" +
                $"            }}\r\n\r\n" +
                $"            @font-face {{\r\n" +
                $"                font-family: 'Lato';\r\n" +
                $"                font-style: italic;\r\n" +
                $"                font-weight: 400;\r\n" +
                $"                src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n" +
                $"            }}\r\n\r\n" +
                $"            @font-face {{\r\n" +
                $"                font-family: 'Lato';\r\n" +
                $"                font-style: italic;\r\n" +
                $"                font-weight: 700;\r\n" +
                $"                src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        /* CLIENT-SPECIFIC STYLES */\r\n" +
                $"        body,\r\n" +
                $"        table,\r\n" +
                $"        td,\r\n" +
                $"        a {{\r\n" +
                $"            -webkit-text-size-adjust: 100%;\r\n" +
                $"            -ms-text-size-adjust: 100%;\r\n" +
                $"        }}\r\n\r\n        table,\r\n" +
                $"        td {{\r\n" +
                $"            mso-table-lspace: 0pt;\r\n" +
                $"            mso-table-rspace: 0pt;\r\n" +
                $"        }}\r\n\r\n" +
                $"        img {{\r\n" +
                $"            -ms-interpolation-mode: bicubic;\r\n" +
                $"        }}\r\n\r\n" +
                $"        /* RESET STYLES */\r\n" +
                $"        img {{\r\n" +
                $"            border: 0;\r\n" +
                $"            height: auto;\r\n" +
                $"            line-height: 100%;\r\n" +
                $"            outline: none;\r\n" +
                $"            text-decoration: none;\r\n" +
                $"        }}\r\n\r\n" +
                $"        table {{\r\n" +
                $"            border-collapse: collapse !important;\r\n" +
                $"        }}\r\n\r\n" +
                $"        body {{\r\n" +
                $"            height: 100% !important;\r\n" +
                $"            margin: 0 !important;\r\n" +
                $"            padding: 0 !important;\r\n" +
                $"            width: 100% !important;\r\n" +
                $"        }}\r\n\r\n" +
                $"        /* iOS BLUE LINKS */\r\n" +
                $"        a[x-apple-data-detectors] {{\r\n" +
                $"            color: inherit !important;\r\n" +
                $"            text-decoration: none !important;\r\n" +
                $"            font-size: inherit !important;\r\n" +
                $"            font-family: inherit !important;\r\n" +
                $"            font-weight: inherit !important;\r\n" +
                $"            line-height: inherit !important;\r\n" +
                $"        }}\r\n\r\n" +
                $"        /* MOBILE STYLES */\r\n" +
                $"        @media screen and (max-width:600px) {{\r\n" +
                $"            h1 {{\r\n" +
                $"                font-size: 32px !important;\r\n" +
                $"                line-height: 32px !important;\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        /* ANDROID CENTER FIX */\r\n" +
                $"        div[style*=\"margin: 16px 0;\"] {{\r\n" +
                $"            margin: 0 !important;\r\n" +
                $"        }}\r\n" +
                $"    </style>\r\n" +
                $"</head>\r\n\r\n" +
                $"<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n" +
                $"<!-- HIDDEN PREHEADER TEXT -->\r\n" +
                $"<div style=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\"></div>\r\n" +
                $"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                $"    <!-- LOGO -->\r\n" +
                $"    <tr>\r\n" +
                $"        <td bgcolor=\"#6388E7\" align=\"center\">\r\n" +
                $"            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n" +
                $"                <tr>\r\n" +
                $"                    <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n" +
                $"                </tr>\r\n" +
                $"            </table>\r\n" +
                $"        </td>\r\n" +
                $"    </tr>\r\n" +
                $"    <tr>\r\n" +
                $"        <td bgcolor=\"#6388E7\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n" +
                $"            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n" +
                $"                <tr>\r\n" +
                $"                    <td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\" style=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n" +
                $"                        <h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">{titleText}</h1> <img src=\" https://img.icons8.com/?size=256&id=44816&format=png\" width=\"125\" height=\"120\" style=\"display: block; border: 0px;\" />\r\n" +
                $"                    </td>\r\n" +
                $"                </tr>\r\n" +
                $"            </table>\r\n" +
                $"        </td>\r\n" +
                $"    </tr>\r\n" +
                $"    <tr>\r\n" +
                $"        <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n" +
                $"            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n" +
                $"                <tr>\r\n" +
                $"                    <td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n" +
                $"                        <p style=\"margin: 0;\">{bodyText1} <br><br>{bodyText2}</p>\r\n" +
                $"                    </td>\r\n" +
                $"                </tr>\r\n" +
                $"                <tr>\r\n" +
                $"                    <td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n" +
                $"                        <p style=\"margin: 0;\">Если у вас остались вопросы, отправьте ответное сообщение и мы Вам поможем.</p>\r\n" +
                $"                    </td>\r\n" +
                $"                </tr>\r\n" +
                $"                <tr>\r\n" +
                $"                    <td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n" +
                $"                        <p style=\"margin: 0;\">Спасибо что выбрали нас,<br>С уважанием команда \"Мега-интернет\"</p>\r\n" +
                $"                    </td>\r\n" +
                $"                </tr>\r\n" +
                $"            </table>\r\n" +
                $"        </td>\r\n" +
                $"    </tr>\r\n" +
                $"    <tr>\r\n" +
                $"        <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 30px 10px 0px 10px;\">\r\n" +
                $"            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n" +
                $"            </table>\r\n" +
                $"        </td>\r\n" +
                $"    </tr>\r\n" +
                $"    <tr>\r\n" +
                $"        <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n" +
                $"            \r\n" +
                $"        </td>\r\n" +
                $"    </tr>\r\n" +
                $"</table>\r\n" +
                $"</body>\r\n\r\n" +
                $"</html>";
            return htmlMessage;
        }
    }
}
