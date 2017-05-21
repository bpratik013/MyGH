using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = _context.Gigs
                .Include(a=>a.Attendances.Select(g=>g.Attendee))
                .Single(g => g.Id == id && g.ArtistId == userId);

            //to check if it is already cancelled
            if (gig.IsCanceled)
                return NotFound();
            
            //var attendees = _context.Attendances
            //    .Where(g => g.GigId == id)
            //    .Select(g => g.Attendee)
            //    .ToList();

            gig.Cancel();
            _context.SaveChanges();

            return Ok();
        }
    }
}
