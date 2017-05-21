using GigHub.Models;
using System;

namespace GigHub.DTO
{
    public class NotificationDto
    {
        public int Id { get; private set; }

        public DateTime DateTime { get; private set; }

        public NotificationType Type { get; private set; }

        //readonly
        public DateTime? OriginalDateTime { get; private set; }

        public string OriginalVenue { get; private set; }

        //Notification only for one newGig
        public GigDto Gig { get; private set; }
    }
}