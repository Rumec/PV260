using DataLayer.Models;

namespace PresentationLayer.Utils;

public static class Messages
{
    // common
    public const string Quitting = "Quitting...";
    public const string InvalidInput = "Invalid input!";
    public const string GoBack = $"'{UserInput.Back}' for back";
    public const string RepeatAction = $"Try again. ({GoBack})";
    public const string InvalidIdFormat = $"Id has to be a number! {RepeatAction}";
    
    // EmailUi messages
    public const string RegisterEmail = $"Which email address you would like to register? ({GoBack})";
    public const string InvalidEmailAddress = $"Invalid email address. {RepeatAction}";
    public const string InputAnotherEmailAddress = $"Input another email address or {GoBack}";

    public const string ViewEmails = "Printing all registered emails:";
    public static string PrintEmail(Email email) => $"Id: {email.Id}, address: {email.Address}";

    public const string DeleteEmail = $"Which email would you like to remove? ({GoBack})";
    public const string InputAnotherEmailId = $"Input another email ID or {GoBack}";
    public const string EmailDoesNotExist = $"Email with this ID does not exist. {RepeatAction}";

}