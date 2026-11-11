using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BroadcastSocialMedia.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(UsersIndexViewModel  viewModel)
        {
            if (viewModel.Search != null)
            {
                var users = await _dbContext.Users.Where(u => u.Name.Contains(viewModel.Search))
                    .ToListAsync();
                viewModel.Result = users;
            }

            return View(viewModel);
        }


        [Route("/Users/{id}")]
        public async Task<IActionResult> ShowUser(string id)
        {
            //Get all broadcasts from the clicked user. Compare incomming id with the id of the user in database
            var broadcasts = await _dbContext.Broadcasts.Where(b => b.User.Id == id)
                .OrderByDescending(b => b.Published)
                .ToListAsync();
            //Clicked user
            var user = await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            var viewModel = new UsersShowUserViewModel()
            {
                Broadcasts = broadcasts,
                User = user
            };

            return View(viewModel);
        }


        [HttpPost, Route("/Users/Listen")]
        public async Task<IActionResult> ListenToUser(UsersListenToUserViewModel viewModel)
        {
            var loggedInUserId = _userManager.GetUserId(User);

            var loggedInUser = await _dbContext.Users
                .Include(u => u.ListeningTo)
                .FirstOrDefaultAsync(u => u.Id == loggedInUserId);

            var userToListenTo = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == viewModel.UserId);

            if (loggedInUser == null || userToListenTo == null)
                return NotFound();

            // The user cannot listen to themselves
            if (loggedInUser.Id == userToListenTo.Id)
                return Redirect("/");

            //Find the user in the list
            var alreadyListening = loggedInUser.ListeningTo
                .FirstOrDefault(u => u.Id == userToListenTo.Id);

            if (alreadyListening == null)
            {
                loggedInUser.ListeningTo.Add(userToListenTo);
            }
            else
            {
                loggedInUser.ListeningTo.Remove(alreadyListening);
            }

            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }
    }
}
