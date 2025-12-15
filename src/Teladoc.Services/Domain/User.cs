namespace Teladoc.Services.Domain;

public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Nickname { get; set; }
    public int? FriendCount { get; set; }

    public int Age {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - this.DateOfBirth.Year;

            if (this.DateOfBirth.Date > today.AddYears(-age))
                age--;
        
            return age;
        }
    }
}