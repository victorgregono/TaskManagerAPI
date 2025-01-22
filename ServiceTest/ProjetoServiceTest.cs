using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Net;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Profiles;
using TaskManager.Application.Services;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;

namespace ServiceTest
{
    public class ProjetoServiceTest
    {
        private readonly IFixture _fixture;
        private readonly IProjetoService _projetoService;
        private readonly Mock<IProjetoRepository> _projetoRepositoryMock;
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;

        public ProjetoServiceTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

            // Substitui o comportamento padrão para evitar exceções de recursão
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Congela as dependências dos repositórios
            _projetoRepositoryMock = _fixture.Freeze<Mock<IProjetoRepository>>();
            _tarefaRepositoryMock = _fixture.Freeze<Mock<ITarefaRepository>>();

            // Configura uma instância real do Mapper
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(MappingProfile).Assembly);  // Escaneia o assembly pelo MappingProfile
            });
            var mapper = mapperConfiguration.CreateMapper();
            _fixture.Inject<IMapper>(mapper);

            // AutoFixture resolve automaticamente a instância de ProjetoService com as dependências reais
            _projetoService = _fixture.Create<ProjetoService>();
        }

        [Fact]
        [Trait("Category", "Create")]
        [Trait("Scenario", "Valid Project")]
        [Trait("ExpectedResult", "Success")]
        public async Task Create_ShouldReturnSuccess_WhenValidProjeto()
        {
            // Arrange
            var projetoViewModel = _fixture.Build<ProjetoViewModel>()
                                           .With(p => p.Nome, "Projeto Teste")
                                           .Create();

            _projetoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Projeto)null);
            _projetoRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Projeto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _projetoService.Create(projetoViewModel);

            // Assert
            result.IsValid.Should().BeTrue();
            _projetoRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Projeto>(p => p.Nome == projetoViewModel.Nome)), Times.Once);
        }

        [Fact]
        [Trait("Category", "Create")]
        [Trait("Scenario", "Project Already Exists")]
        [Trait("ExpectedResult", "Error")]
        public async Task Create_ShouldReturnError_WhenProjetoAlreadyExists()
        {
            // Arrange
            var projetoViewModel = _fixture.Build<ProjetoViewModel>()
                                           .With(p => p.Nome, "Projeto Existente")
                                           .Create();

            var existingProjeto = _fixture.Build<Projeto>()
                                          .With(p => p.Nome, "Projeto Existente")
                                          .Create();

            _projetoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(existingProjeto);

            // Act
            var result = await _projetoService.Create(projetoViewModel);

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            _projetoRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Projeto>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "FindAll")]
        [Trait("Scenario", "Projects Exist")]
        [Trait("ExpectedResult", "Success")]
        public async Task FindAll_ShouldReturnSuccess_WhenProjetosExist()
        {
            // Arrange
            var projetos = _fixture.CreateMany<ProjetoDetalhado>(3).ToList();
            _projetoRepositoryMock.Setup(repo => repo.GetProjetosDetalhadosAsync()).ReturnsAsync(projetos);

            // Act
            var result = await _projetoService.FindAll();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(projetos);
        }

        [Fact]
        [Trait("Category", "FindById")]
        [Trait("Scenario", "Project Exists")]
        [Trait("ExpectedResult", "Success")]
        public async Task FindById_ShouldReturnSuccess_WhenProjetoExists()
        {
            // Arrange
            var projeto = _fixture.Create<Projeto>();
            _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(projeto);

            // Act
            var result = await _projetoService.FindById(projeto.Id);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(projeto);
        }

        [Fact]
        [Trait("Category", "FindById")]
        [Trait("Scenario", "Project Does Not Exist")]
        [Trait("ExpectedResult", "Error")]
        public async Task FindById_ShouldReturnError_WhenProjetoDoesNotExist()
        {
            // Arrange
            _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Projeto)null);

            // Act
            var result = await _projetoService.FindById(1);

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
