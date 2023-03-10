﻿using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace RentalCompany.Application.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    public EmailSender(IOptions<EmailSettings> settings)
    {
        _emailSettings = settings.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailToSend = PrepareEmailToSend(email, subject, htmlMessage);
        SendEmail(emailToSend);

        return Task.CompletedTask;
    }

    private MimeMessage PrepareEmailToSend(string email, string subject, string htmlMessage)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse(_emailSettings.From));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

        return emailToSend;
    }

    private void SendEmail(MimeMessage emailToSend)
    {
        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect(_emailSettings.SmtpServer, _emailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate(_emailSettings.Username, _emailSettings.Password);
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }
    }
}


