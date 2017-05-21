using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }

        public DateTime DateTime { get; private set; }

        public NotificationType Type { get; private set; }
        
        //readonly
        public DateTime? OriginalDateTime { get; private set; }

        public string OriginalVenue { get; private set; }

        //Notification only for one newGig
        [Required]
        public Gig Gig { get; private set; }

        public Notification()
        {

        }

        //it should not be used anywhere
        private Notification(NotificationType type, Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException("gig");

            Type = type;
            Gig = gig;
            DateTime = DateTime.Now;
        }

        //simple factory patterns
        //responsible for creating notification 
        public static Notification GigCreated(Gig gig)
        {
          return  new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigUpdated(Gig newGig,DateTime originalDateTime,string originalVenue)
        {
            var notifications = new Notification(NotificationType.GigUpdated, newGig);
            notifications.OriginalDateTime = originalDateTime;
            notifications.OriginalVenue = originalVenue;

            return notifications;
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }

    }
}