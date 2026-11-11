using BroadcastSocialMedia.Models;

namespace BroadcastSocialMedia.ViewModels
{
    public class UsersShowUserViewModel
    {
        //Clicked user
        public ApplicationUser User { get; set; }
        public List<Broadcast> Broadcasts { get; set; }
        public bool IsListening { get; set; }
    }
}
