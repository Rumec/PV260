using System.Threading.Tasks;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using NUnit.Framework;
using Moq;
using PresentationLayer;
using PresentationLayer.UI;
using PresentationLayer.Utils;

namespace Tests.PLTests.UITests;

[TestFixture]
public class EmailUiTests
{

    private Mock<IConsoleWrapper> _consoleWrapper;
    private Mock<IUserEmailService> _userMailService;


    [SetUp]
    public void Setup()
    {
        _consoleWrapper = new Mock<IConsoleWrapper>();
        _userMailService = new Mock<IUserEmailService>();
    }
    

    [Test]
    [TestCase(UserInput.Quit)]
    [TestCase(UserInput.Back)]
    public async Task TestRun_QuitImmediately(string userInput)
    {
        // arrange
        _consoleWrapper.Setup(t => t.ReadLine())
            .Returns(userInput);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        await emailUi.Run();
        
        // assert
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
    }
    
    
    // -------------------------[ DeleteEmail tests ]----------------------------

    private const string InvalidInputFormatMessage = "Id has to be a number!";
    private const string InvalidEmailIdMessage = "Email with this ID does not exist. Try again";


    [Test]
    public void TestRun_DeleteEmail_InvalidInputFormat_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.DeleteEmail)
            .Returns("invalidStringId")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidInputFormatMessage), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    [Test]
    public void TestRun_DeleteEmail_InvalidEmailId_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.DeleteEmail)
            .Returns("1")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        _userMailService.Setup(t => t.RemoveEmail(It.IsAny<int>()))
            .Throws(new EmailDoesNotExistException(1));
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidInputFormatMessage), Times.Never);
        _userMailService.Verify(t => t.RemoveEmail(1), Times.Once);
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailIdMessage), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    [Test]
    public void TestRun_DeleteEmail_ValidEmailId_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.DeleteEmail)
            .Returns("1")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidInputFormatMessage), Times.Never);
        _userMailService.Verify(t => t.RemoveEmail(1), Times.Once);
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailIdMessage), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    [Test]
    public void TestRun_DeleteEmail_MultipleValidEmailIds_OneInvalidInputFormat_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.DeleteEmail)
            .Returns("1")
            .Returns("2")
            .Returns("Invalid id")
            .Returns("3")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidInputFormatMessage), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Exactly(3));
        _userMailService.Verify(t => t.RemoveEmail(1), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(2), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(3), Times.Once);
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailIdMessage), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    
    // --------------------------[ RegisterEmail tests ]-----------------------------

    // probably not ideal, but reduces issues in case the message changes
    // could use e.g. regex (the error message will likely contain '[T/t]ry again', '[I/i]nvalid' etc.),
    //   or create a singular source for error messages (like we did with UserInput)
    // using It.IsAny<string>() is not viable, since we don't account for PrintMenu() calls;
    //   also creates problems if another _consoleWrapper.WriteLine(...) is added in the called functions
    private const string InvalidEmailMessage = $"Invalid email address. Try again. ('{UserInput.Back}' for back)";

    
    [Test]
    public void TestRun_RegisterEmail_ValidEmail_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.RegisterEmail)
            .Returns("test@gmail.com")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailMessage), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail("test@gmail.com"), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    [Test]
    public void TestRun_RegisterEmail_InvalidEmail_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.RegisterEmail)
            .Returns("invalidEmail")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailMessage), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    [Test]
    public void TestRun_RegisterEmail_InvalidEmail_ThenValidEmail_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.RegisterEmail)
            .Returns("invalidEmail")
            .Returns("test@gmail.com")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailMessage), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail("test@gmail.com"), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
    [Test]
    public void TestRun_RegisterEmail_MultipleValidEmails_OneInvalid_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.ReadLine())
            .Returns(UserInput.RegisterEmail)
            .Returns("test1@gmail.com")
            .Returns("test2@gmail.com")
            .Returns("invalidEmail")
            .Returns("test3@gmail.com")
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.WriteLine(InvalidEmailMessage), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Exactly(3));
        _userMailService.Verify(t => t.RegisterNewEmail("test1@gmail.com"), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail("test2@gmail.com"), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail("test3@gmail.com"), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
    }
    
}