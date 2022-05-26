using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataLayer.Models;
using NUnit.Framework;
using Moq;
using PresentationLayer.UI;
using PresentationLayer.Utils;

namespace Tests.PLTests.UITests;

[TestFixture]
public class EmailUiTests
{

    private Mock<IConsoleIoWrapper> _consoleWrapper;
    private Mock<IUserEmailService> _userMailService;


    [SetUp]
    public void Setup()
    {
        _consoleWrapper = new Mock<IConsoleIoWrapper>();
        _userMailService = new Mock<IUserEmailService>();
    }
    

    [Test]
    [TestCase(UserInput.Quit)]
    [TestCase(UserInput.Back)]
    public async Task TestRun_QuitImmediately(string userInput)
    {
        // arrange
        _consoleWrapper.Setup(t => t.GetInput())
            .Returns(userInput);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        await emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Never);
        
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
    }
    
    
    // -------------------------[ DeleteEmail tests ]----------------------------
    
    [Test]
    [TestCase("idWithInvalidFormat")]
    public void TestRun_DeleteEmail_InvalidIdFormat_ThenQuit(string id)
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.DeleteEmail)
            .Returns(id)
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidIdFormat), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Never);
    }
    
    [Test]
    [TestCase("1")]
    public void TestRun_DeleteEmail_EmailDoesNotExist_ThenQuit(string id)
    {
        Assert.True(int.TryParse(id, out var parsedId));

        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.DeleteEmail)
            .Returns(id)
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        _userMailService.Setup(t => t.RemoveEmail(It.IsAny<int>()))
            .Throws(new EmailDoesNotExistException(parsedId));
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidIdFormat), Times.Never);
        _userMailService.Verify(t => t.RemoveEmail(parsedId), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.EmailDoesNotExist), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Never);
    }
    
    [Test]
    [TestCase("1")]
    public void TestRun_DeleteEmail_ValidEmailId_ThenQuit(string id)
    {
        Assert.True(int.TryParse(id, out var parsedId));
        
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.DeleteEmail)
            .Returns(id)
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidIdFormat), Times.Never);
        _userMailService.Verify(t => t.RemoveEmail(parsedId), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.EmailDoesNotExist), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Never);
    }
    
    [Test]
    [TestCase(new []{"1", "2", "3"}, "Invalid id")]
    public void TestRun_DeleteEmail_MultipleValidEmailIds_OneInvalidInputFormat_ThenQuit(string[] validIds, string invalidId)
    {
        Assert.True(validIds.ToList().Select(id => int.TryParse(id, out _)).All(x => x));
        var parsedIds = validIds.ToList().Select(int.Parse).ToList();
        Assert.GreaterOrEqual(validIds.Length, 3);

        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.DeleteEmail)
            .Returns(validIds[0])
            .Returns(validIds[1])
            .Returns(invalidId)
            .Returns(validIds[2])
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidIdFormat), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Exactly(3));
        
        foreach (var id in parsedIds)
        {
            _userMailService.Verify(t => t.RemoveEmail(id), Times.Once);    
        }
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.EmailDoesNotExist), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Never);
    }
    
    
    // --------------------------[ RegisterEmail tests ]-----------------------------
    
    [Test]
    [TestCase("test@gmail.com")]
    [TestCase("485342@mail.muni.cz")]
    public void TestRun_RegisterEmail_ValidEmail_ThenQuit(string email)
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.RegisterEmail)
            .Returns(email)
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidEmailAddress), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(email), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Never);
    }
    
    [Test]
    [TestCase("invalidEmail")]
    public void TestRun_RegisterEmail_InvalidEmail_ThenQuit(string email)
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.RegisterEmail)
            .Returns(email)
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidEmailAddress), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Never);
    }
    
    [Test]
    [TestCase("test@gmail.com", "invalidEmail")]
    public void TestRun_RegisterEmail_InvalidEmail_ThenValidEmail_ThenQuit(string validEmail, string invalidEmail)
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.RegisterEmail)
            .Returns(invalidEmail)
            .Returns(validEmail)
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidEmailAddress), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(validEmail), Times.Once);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Never);
    }
    
    [Test]
    [TestCase(new []{"test@gmail.com", "485342@mail.muni.cz", "485193@muni.cz"}, "invalidEmail")]
    public void TestRun_RegisterEmail_MultipleValidEmails_OneInvalid_ThenQuit(string[] validEmails, string invalidEmail)
    {
        Assert.GreaterOrEqual(validEmails.Length, 3);
        
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.RegisterEmail)
            .Returns(validEmails[0])
            .Returns(validEmails[1])
            .Returns(invalidEmail)
            .Returns(validEmails[2])
            .Returns(UserInput.Back)
            .Returns(UserInput.Quit);
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidEmailAddress), Times.Once);
        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Exactly(3));
        
        foreach (var validEmail in validEmails)
        {
            _userMailService.Verify(t => t.RegisterNewEmail(validEmail), Times.Once);
        }
        
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Never);
    }
    
    // ------------------------[ ViewEmails tests ]-------------------------
    
    [Test]
    public void TestRun_ViewEmails_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.ViewEmails)
            .Returns(UserInput.Quit);
        
        var emails = new List<Email>
        {
            new() {Address = "test1@gmail.com", Id = 1},
            new() {Address = "test2@gmail.com", Id = 2}
        };
        _userMailService.Setup(t => t.GetAllRegisteredEmails())
            .Returns(Task.FromResult(emails));
        var emailUi = new EmailUi(_userMailService.Object, _consoleWrapper.Object);
        
        // act
        emailUi.Run();
        
        // assert
        _userMailService.Verify(t => t.GetAllRegisteredEmails(), Times.Once);
        
        foreach (var email in emails)
        {
            _consoleWrapper.Verify(t => t.ShowMessage(Messages.PrintEmail(email)), Times.Once);
        }

        _userMailService.Verify(t => t.RegisterNewEmail(It.IsAny<string>()), Times.Never);
        _userMailService.Verify(t => t.RemoveEmail(It.IsAny<int>()), Times.Never);
        
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.ViewEmails), Times.Once);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.RegisterEmail), Times.Never);
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.DeleteEmail), Times.Never);
    }
    
}