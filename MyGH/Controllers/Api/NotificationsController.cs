using AutoMapper;
using GigHub.DTO;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        //creating a database access point
        private ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        //We want list of notification from the domain class called notifications
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            //we want the notification relating to user logged in
            var userId = User.Identity.GetUserId();

            var notifications = _context.UserNotifications
                //for currently logged in user and only new notifications which is not read
                .Where(u => u.UserId == userId && !u.IsRead)
                .Select(u => u.Notification)
                .Include(a => a.Gig.Artist)
                .ToList();

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                //for currently logged in user and only new notifications which is not read
                .Where(u => u.UserId == userId && !u.IsRead)
                .ToList();
            //iterating over the list of notifications
            notifications.ForEach(n=>n.Read());
            _context.SaveChanges();
            return Ok();
        }

    }
}
