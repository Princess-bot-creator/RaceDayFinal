namespace RaceDay.MVC.Models;

public class LoginVm { public string Email { get; set; } = ""; public string Password { get; set; } = ""; }
public class RegisterVm
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "Participant";
}
public class LoginResponse { public string Token { get; set; } = ""; public string Role { get; set; } = ""; public int UserId { get; set; } public string FullName { get; set; } = ""; }

public class EventVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }
    public string Location { get; set; } = "";
    public double Distance { get; set; }
    public string EventType { get; set; } = "";
    public string? BannerImageUrl { get; set; }
    public int OrganiserId { get; set; }
    public int EnrolmentCount { get; set; }
}
public class EnrolmentVm { public int Id { get; set; } public int EventId { get; set; } public string EventName { get; set; } = ""; public DateTime EventDate { get; set; } public string ParticipantName { get; set; } = ""; public string Status { get; set; } = "Pending"; }
public class ResultVm { public int Id { get; set; } public string EventName { get; set; } = ""; public DateTime EventDate { get; set; } public string FinishTime { get; set; } = ""; public int FinishingPosition { get; set; } public int TotalFinishers { get; set; } }
public class ProfileVm { public int Id { get; set; } public string FullName { get; set; } = ""; public string Email { get; set; } = ""; public string? ProfilePictureUrl { get; set; } }