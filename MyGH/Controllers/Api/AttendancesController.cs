using GigHub.DTO;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }


        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var attendeeId = User.Identity.GetUserId();

            //check weather user is already attending the event.
            if(_context.Attendances.Any(a=>a.AttendeeId==attendeeId &&
            a.GigId ==dto.GigId))
                return BadRequest("The attendance already exists.");

            var attendance = new Attendance()
            {
                GigId = dto.GigId,
                AttendeeId = attendeeId
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();

        }

        //Delete method
        //delete the attendance object from Attendances
        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var attendeeId = User.Identity.GetUserId();

            var attendance = _context.Attendances
                .SingleOrDefault(a => a.AttendeeId == attendeeId &&
                                      a.GigId == id);

            if (attendance == null)
                return NotFound();
            

            _context.Attendances.Remove(attendance);
            _context.SaveChanges();

            return Ok(id);
        }
    }
}
