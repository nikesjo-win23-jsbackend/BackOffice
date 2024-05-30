using System.ComponentModel.DataAnnotations;

namespace BackOffice.Business.Models;

public class UserInfoModel
{
    public string Id { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "First name")]
    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "Last name")]
    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "Your email address is invalid")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Bio")]
    [DataType(DataType.MultilineText)]
    public string? Biography { get; set; }
    public AddressModel? Address { get; set; }

}

public class AddressModel
{
    [DataType(DataType.Text)]
    [Display(Name = "Addressline_1")]
    [Required(ErrorMessage = "Address is required")]
    [MinLength(2, ErrorMessage = "Address is required")]
    public string AddressLine_1 { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "AddressLine_2")]
    public string? AddressLine_2 { get; set; }

    [Display(Name = "Postal Code")]
    [DataType(DataType.PostalCode)]
    [Required(ErrorMessage = "Postal code is required")]
    public string PostalCode { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "City")]
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = null!;
}