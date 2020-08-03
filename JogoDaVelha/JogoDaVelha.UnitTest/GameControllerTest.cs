using JogoDaVelha.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace JogoDaVelha.UnitTest
{
    public class GameControllerTest
    {
        GameController _gameController;
        FakeGameService _fakeGameService;
        public GameControllerTest()
        {
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.Setup(m => m.Value).Returns("3");
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "AppSettings:GameSize"))).Returns(mockConfSection.Object);
            _fakeGameService = new FakeGameService();
            _gameController = new GameController(_fakeGameService, mockConfiguration.Object);
        }

        [Fact]
        public void CreatedTest()
        {
            var createdGame = _gameController.CreateGame();
            Assert.IsType<CreatedResult>(createdGame);
        }
    }
}
