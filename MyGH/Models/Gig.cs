using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
	//CLASS
    public class Gig
    {   
	//Attributes
        public int Id { get; set; }

        public bool IsCanceled { get; set; }    

        public ApplicationUser Artist { get; set; }

        //Foreign key
        //In Application User each artist is defined as string
        [Required]
        public string  ArtistId { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public DateTime DateTime { get; set; }

        //foreign Key
        [Required]
        public byte GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;

            var notificaton = Notification.GigCanceled(this);

            foreach (var attendee in Attendances.Select(g => g.Attendee))
            {
                attendee.Notify(notificaton);
            }
        }

        
        public void Modify(DateTime dateTime, string venue, byte genre)
        {
            var notification =  Notification.GigUpdated(this,DateTime,Venue);
            

            Venue = venue;
            GenreId = genre;
            DateTime = dateTime;

            foreach (var attendee in Attendances.Select(g => g.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}
