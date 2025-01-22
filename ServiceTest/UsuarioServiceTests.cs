using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Net;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Profiles;
using TaskManager.Application.Services;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;
using Xunit;

namespace ServiceTest
{
    public class UsuarioServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUsuarioService _usuarioService;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;

        public UsuarioServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

            // Congela a dependência do repositório
            _usuarioRepositoryMock = _fixture.Freeze<Mock<IUsuarioRepository>>();

            // Configura uma instância real do Mapper
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(MappingProfile).Assembly);  // Escaneia o assembly pelo MappingProfile
            });
            var mapper = mapperConfiguration.CreateMapper();
            _fixture.Inject<IMapper>(mapper);

            // AutoFixture resolve automaticamente a instância de UsuarioService com as dependências reais
            _usuarioService = _fixture.Create<UsuarioService>();
        }

        [Fact]
        [Trait("Category", "Create")]
        [Trait("Scenario", "Valid User")]
        [Trait("ExpectedResult", "Success")]
        public async Task Create_ShouldReturnSuccess_WhenValidUsuario()
        {
            // Arrange
            var usuarioViewModel = _fixture.Build<UsuarioViewModel>()
                                           .With(u => u.Nome, "teste123")
                                           .With(u => u.Funcao, "gerente")
                                           .Create();

            _usuarioRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            // Act
            var result = await _usuarioService.Create(usuarioViewModel);

            // Assert
            result.IsValid.Should().BeTrue();
            _usuarioRepositoryMock.Verify(repo => repo.InsertAsync(It.Is<Usuario>(u => u.Nome == usuarioViewModel.Nome && u.Funcao == usuarioViewModel.Funcao)), Times.Once);
        }

        [Fact]
        [Trait("Category", "Create")]
        [Trait("Scenario", "Invalid User")]
        [Trait("ExpectedResult", "Error")]
        public async Task Create_ShouldReturnError_WhenInvalidUsuario()
        {
            // Arrange
            var usuarioViewModel = _fixture.Build<UsuarioViewModel>()
                                           .With(u => u.Nome, string.Empty) // Nome inválido
                                           .Create();

            // Act
            var result = await _usuarioService.Create(usuarioViewModel);

            // Assert
            result.IsValid.Should().BeFalse();
            _usuarioRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "FindAll")]
        [Trait("Scenario", "Usuarios Exist")]
        [Trait("ExpectedResult", "Success")]
        public async Task FindAll_ShouldReturnSuccess_WhenUsuariosExist()
        {
            // Arrange
            var usuarios = _fixture.CreateMany<Usuario>(3).ToList();
            _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(usuarios);

            // Act
            var result = await _usuarioService.FindAll();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(usuarios);
        }

        [Fact]
        [Trait("Category", "FindAll")]
        [Trait("Scenario", "No Usuarios")]
        [Trait("ExpectedResult", "Error")]
        public async Task FindAll_ShouldReturnError_WhenNoUsuariosExist()
        {
            // Arrange
            var usuarios = new List<Usuario>();
            _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(usuarios);

            // Act
            var result = await _usuarioService.FindAll();

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        [Trait("Category", "FindById")]
        [Trait("Scenario", "Usuario Exists")]
        [Trait("ExpectedResult", "Success")]
        public async Task FindById_ShouldReturnSuccess_WhenUsuarioExists()
        {
            // Arrange
            var usuario = _fixture.Create<Usuario>();
            _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(usuario);

            // Act
            var result = await _usuarioService.FindById(usuario.Id);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(usuario);
        }

        [Fact]
        [Trait("Category", "FindById")]
        [Trait("Scenario", "Usuario Does Not Exist")]
        [Trait("ExpectedResult", "Error")]
        public async Task FindById_ShouldReturnError_WhenUsuarioDoesNotExist()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Usuario)null);

            // Act
            var result = await _usuarioService.FindById(1);

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
