﻿using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public class EmailSettings
	{
		public static void SendEmail(Email email )
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("shehabsameh987123@gmail.com", "fktn ivsn rnix dpjj");
			client.Send("shehabsameh987123@gmail.com", email.To, email.Title, email.Body);
		}
	}
}
