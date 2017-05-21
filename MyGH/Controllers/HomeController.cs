using GigHub.Models;
using GigHub.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(string query=null)
        {
            //loading all the gig in upcoming variable
            //Display the gig which is not cancelled.
            var upcominggig = _context.Gigs
                .Include(a => a.Artist)
                .Include(a=>a.Genre)
                .Where(a=>a.DateTime > DateTime.Now && !a.IsCanceled);
            
            //finding the query gig
            if (!String.IsNullOrWhiteSpace(query))
            {
                upcominggig = upcominggig
                    .Where(g =>
                        g.Artist.Name.Contains(query) ||
                        g.Venue.Contains(query) ||
                        g.Genre.Name.Contains(query));
            }

            //loading all the gigs which is attended by user
            var userId = User.Identity.GetUserId();
            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId &&
                a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a =>a.GigId);
            
            //mapping the data to gigsviewmodel
            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upcominggig,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs",viewModel);
        }

       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}