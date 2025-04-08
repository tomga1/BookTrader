// Services/EmailSender.cs
using MailKit.Net.Smtp;
using MimeKit;

public class EmailSender
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("BookTrader", "tomasgarcia003@gmail.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html") { Text = body };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("tomasgarcia003@gmail.com", "zioj iqmm fggg cqmy");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
