using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication9.ViewModels
{

    public class Message
    {
        [Display(Name = "Your email address")]
        [Required(ErrorMessage = "Email required.")]
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@\""]+"
                    + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                    + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                    + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                    + @"[a-zA-Z]{2,}))$", ErrorMessage = "Not a valid email address")]
        public string Sender { get; set; }
        [Required(ErrorMessage = "Subject required.")]
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Message required.")]
        [Display(Name = "Message")]
        public string Body { get; set; }


        public Message() { }
        public Message(string sender, string subject, string body)
        {
            Sender = sender;
            Subject = subject;
            Body = body;

        }

    }
}