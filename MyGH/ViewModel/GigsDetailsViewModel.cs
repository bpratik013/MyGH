﻿using GigHub.Models;

namespace GigHub.ViewModel
{
    public class GigsDetailsViewModel
    {
        public Gig Gig { get; set; }
        public bool IsAttending { get; set; }
        public bool IsFollowing { get; set; }
    }
}