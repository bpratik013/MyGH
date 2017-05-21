using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GigHub.ViewModel
{
    public class GigFormViewModel
    {
        public int Id { get; set; }
        //server side validation
        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        // return action as called by user
        public string Action
        {
            get
            {
                //creating lambda expresssins for update and create, so in future when we rafctor our code, the program will not break.
                Expression<Func<GigsController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<GigsController, ActionResult>> create = 
                    (c => c.Create(this));

                var action = (Id != 0) ? update : create;

                return (action.Body as MethodCallExpression).Method.Name;

                
            } 
        }

        //getdatetime help us to prevent the reflection instead of using DateTime
        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format(Date, Time));
        }

        
       
    }
}