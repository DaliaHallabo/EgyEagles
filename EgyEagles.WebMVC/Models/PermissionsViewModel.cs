using EgyEagles.Application.DTOs.Users;

namespace EgyEagles.WebMVC.Models
{
    public class PermissionsViewModel
    {
        public string SelectedUserId { get; set; }
        public List<UserDto> Users { get; set; }
    }

}
