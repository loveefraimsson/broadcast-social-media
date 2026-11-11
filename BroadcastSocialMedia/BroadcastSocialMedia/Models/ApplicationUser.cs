using Microsoft.AspNetCore.Identity;

namespace BroadcastSocialMedia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public ICollection<Broadcast> Broadcasts { get; set; } = new List<Broadcast>();

        // Who the user is listening to
        public ICollection<ApplicationUser> ListeningTo { get; set; } = new List<ApplicationUser>();

        // Users who listen to the application user
        public ICollection<ApplicationUser> Listeners { get; set; } = new List<ApplicationUser>();
    }
}
