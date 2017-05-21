using GigHub.Models;
using GigHub.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

     
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();

            var artistupcominggig = _context.Gigs
                .Where(a => a.ArtistId == userId
                && a.DateTime > DateTime.Now
                && !a.IsCanceled)
                .Include(g => g.Genre)
                .ToList();

            return View(artistupcominggig);
        }

        //loading all gigs which we are attended by user.
  
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            //load all gigs which are attended by user
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(g => g.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            //loading all the gigs which is attended by user
            var attendances = _context.Attendances
                    .Where(a => a.AttendeeId == userId &&
                     a.Gig.DateTime > DateTime.Now)
                    .ToList()
                    .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Gig's I am attending!",
                Attendances = attendances

            };

            //it directs to gigs.cshtml file
            return View("Gigs", viewModel);
        }

        //public ActionResult Following()
        //{
        //    var userId = User.Identity.GetUserId();

        //    var following = _context.Followings
        //        .Where(f => f.FollowerId == userId)
        //        .Select(a => a.FolloweeId)
        //        .ToList();

        //    return View(following);
        //}

        // GET: Gigs

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        //to view details of artists
        public ActionResult Details(int id)
        {


            // we are loading gig with given Id

            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id);

            //if gig is null
            if (gig == null)
                return HttpNotFound();

            //initializing the viewmodel
            var viewModel = new GigsDetailsViewModel()
            {
                Gig = gig
            };

            //validate the user
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _context.Attendances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
            }



            return View("Details", viewModel);



        }


        public ActionResult Create()
        {
            var genre = _context.Genres.ToList();

            var viewModel = new GigFormViewModel
            {
                Genres = genre,
                Heading = "Add a Gig!"
            };

            return View("GigForm", viewModel);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel formViewModel)
        {
            //var artist = _context.Users.Single(u=>u.Id==artistId);
            //var genre = _context.Genres.Single(g=>g.Id==formViewModel.Genre);
            if (!ModelState.IsValid)
            {
                formViewModel.Genres = _context.Genres.ToList();
                return View("GigForm", formViewModel);
            }

            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = formViewModel.GetDateTime()/*DateTime.Parse(string.Format(formViewModel.Date, formViewModel.Time))*/,
                GenreId = formViewModel.Genre,
                Venue = formViewModel.Venue,

            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                // Initialize the Id property otherwise it add a new gig instead of updating
                Id = gig.Id,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:MM"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a Gig!"
            };

            return View("GigForm", viewModel);
        }



        

        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel formViewModel)
        {
            //var artist = _context.Users.Single(u=>u.Id==artistId);
            //var genre = _context.Genres.Single(g=>g.Id==formViewModel.Genre);
            if (!ModelState.IsValid)
            {
                formViewModel.Genres = _context.Genres.ToList();
                return View("GigForm", formViewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(a => a.Attendances.Select(g => g.Attendee))
                .Single(g => g.Id == formViewModel.Id && g.ArtistId == userId);

            //updating the elements in the gig
            gig.Modify(formViewModel.GetDateTime(), formViewModel.Venue, formViewModel.Genre);
            //gig.Venue = formViewModel.Venue;
            //gig.DateTime = formViewModel.GetDateTime();
            //gig.GenreId = formViewModel.Genre;



            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }




    }
}