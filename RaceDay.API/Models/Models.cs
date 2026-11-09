using System.ComponentModel.DataAnnotations;
namespace RaceDay.API.Models;
public class User { public int Id { get; set; } public string FullName { get; set; } = ""; public string Email { get; set; } = ""; public string Password { get; set; } = ""; public string Role { get; set; } = "Participant"; public string? ProfilePictureUrl { get; set; } }
public class Event { public int Id { get; set; } public string Name { get; set; } = ""; public string Description { get; set; } = ""; public DateTime Date { get; set; } public string Location { get; set; } = ""; public double Distance { get; set; } public string EventType { get; set; } = ""; public string? BannerImageUrl { get; set; } public int OrganiserId { get; set; } }
public class Enrolment { public int Id { get; set; } public int EventId { get; set; } public int ParticipantId { get; set; } public int CategoryId { get; set; } public string Status { get; set; } = "Pending"; public Event? Event { get; set; } public User? Participant { get; set; } }
public class Result { public int Id { get; set; } public int EnrolmentId { get; set; } public string FinishTime { get; set; } = ""; public int FinishingPosition { get; set; } public int TotalFinishers { get; set; } }
public class RegisterDto { [Required] public string FullName { get; set; } = ""; [Required] public string Email { get; set; } = ""; [Required] public string Password { get; set; } = ""; public string Role { get; set; } = "Participant"; }
public class LoginDto { public string Email { get; set; } = ""; public string Password { get; set; } = ""; }
