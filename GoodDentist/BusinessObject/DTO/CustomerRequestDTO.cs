using System.ComponentModel.DataAnnotations;
using BusinessObject.Entity;

namespace BusinessObject.DTO;

public class CustomerRequestDTO
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    public DateOnly? Dob { get; set; }

    [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters")]
    public string? Gender { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string? Email { get; set; }

    [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters")]
    public string? Address { get; set; }
    [Required(ErrorMessage = "Clinic ID is required")]
    public string ClinicId { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Dob.HasValue && Dob.Value > DateOnly.FromDateTime(DateTime.Today))
        {
            yield return new ValidationResult("Date of birth cannot be in the future", new[] { nameof(Dob) });
        }

        if (!string.IsNullOrEmpty(Password) && Password.Length < 6)
        {
            yield return new ValidationResult("Password must be at least 6 characters long", new[] { nameof(Password) });
        }
    }
}