using BusinessLayer.Jobs;
using NUnit.Framework;
using Moq;
using PresentationLayer;
using PresentationLayer.UI;
using PresentationLayer.Utils;

namespace Tests.PLTests;

[TestFixture]
public class AppTests
{

    private Mock<IConsoleIoWrapper> _consoleWrapper;
    private Mock<IEmailUi> _emailUi;
    private Mock<IDataSetUi> _dataSetUi;
    private Mock<IDataSyncJob> _dataSyncJob;
    private Mock<IConfigUi> _configUi;
    
    
    [SetUp]
    public void Setup()
    {
        _consoleWrapper = new Mock<IConsoleIoWrapper>();
        _emailUi = new Mock<IEmailUi>();
        _dataSetUi = new Mock<IDataSetUi>();
        _dataSyncJob = new Mock<IDataSyncJob>();
        _configUi = new Mock<IConfigUi>();
    }
    
    private void VerifyQuitting()
    {
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.Quitting), Times.Once);
    }

    private App GetAppInstance()
    {
        return new App(_emailUi.Object, _dataSetUi.Object, _dataSyncJob.Object, _configUi.Object, _consoleWrapper.Object);
    }

    [Test]
    public void TestRun_QuitImmediately()
    {
        // arrange
        _consoleWrapper.Setup(t => t.GetInput())
            .Returns(UserInput.Quit);
        var app = GetAppInstance();
        
        // act
        app.Run();
        
        // assert
        VerifyQuitting();
        _emailUi.Verify(t => t.Run(), Times.Never);
        _dataSetUi.Verify(t => t.Run(), Times.Never);
    }

    [Test]
    [TestCase("xYz")]
    [TestCase("bla bla bla")]
    public void TestRun_ChooseIncorrectInput_ThenQuit(string invalidInput)
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(invalidInput)
            .Returns(UserInput.Quit);
        var app = GetAppInstance();
        
        // act
        app.Run();
        
        // assert
        _consoleWrapper.Verify(t => t.ShowMessage(Messages.InvalidInput), Times.Once);
        VerifyQuitting();
        _emailUi.Verify(t => t.Run(), Times.Never);
        _dataSetUi.Verify(t => t.Run(), Times.Never);
    }

    [Test]
    public void TestRun_ChooseDataSet_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.DataSet)
            .Returns(UserInput.Quit);
        var app = GetAppInstance();
        
        // act
        app.Run();
        
        // assert
        _dataSetUi.Verify(t => t.Run(), Times.Once);
        VerifyQuitting();
        _emailUi.Verify(t => t.Run(), Times.Never);
    }
    
    [Test]
    public void TestRun_ChooseEmail_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.Email)
            .Returns(UserInput.Quit);
        var app = GetAppInstance();
        
        // act
        app.Run();
        
        // assert
        _emailUi.Verify(t => t.Run(), Times.Once);
        VerifyQuitting();
        _dataSetUi.Verify(t => t.Run(), Times.Never);
    }
    
    [Test]
    public void TestRun_ChooseDataSet_ThenChooseEmail_ThenQuit()
    {
        // arrange
        _consoleWrapper.SetupSequence(t => t.GetInput())
            .Returns(UserInput.DataSet)
            .Returns(UserInput.Email)
            .Returns(UserInput.Quit);
        var app = GetAppInstance();
        
        // act
        app.Run();
        
        // assert
        _dataSetUi.Verify(t => t.Run(), Times.Once);
        _emailUi.Verify(t => t.Run(), Times.Once);
        VerifyQuitting();
    }
}