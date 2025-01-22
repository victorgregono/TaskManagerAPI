using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Profiles;
using TaskManager.Application.Services;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Models;
using Xunit;

namespace ServiceTest
{
    public class TarefaServiceTest
    {
        private readonly IFixture _fixture;
        private readonly ITarefaService _tarefaService;
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly Mock<IHistoricoTarefaRepository> _historicoRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IProjetoRepository> _projetoRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public TarefaServiceTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

            // Substitui o comportamento padrão para evitar exceções de recursão
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Congela as dependências dos repositórios
            _tarefaRepositoryMock = _fixture.Freeze<Mock<ITarefaRepository>>();
            _historicoRepositoryMock = _fixture.Freeze<Mock<IHistoricoTarefaRepository>>();
            _usuarioRepositoryMock = _fixture.Freeze<Mock<IUsuarioRepository>>();
            _projetoRepositoryMock = _fixture.Freeze<Mock<IProjetoRepository>>();
            _httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();

            // Configura uma instância real do Mapper
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(MappingProfile).Assembly);  // Escaneia o assembly pelo MappingProfile
            });
            var mapper = mapperConfiguration.CreateMapper();
            _fixture.Inject<IMapper>(mapper);

            // AutoFixture resolve automaticamente a instância de TarefaService com as dependências reais
            _tarefaService = _fixture.Create<TarefaService>();
        }

        [Fact]
        [Trait("Category", "Create")]
        [Trait("Scenario", "Valid Task")]
        [Trait("ExpectedResult", "Success")]
        public async Task Create_ShouldReturnSuccess_WhenValidTarefa()
        {
            // Arrange
            var tarefaViewModel = _fixture.Build<TarefaViewModel>()
                                          .With(t => t.ProjetoId, 1)
                                          .With(t => t.DataVencimento, DateTime.Now.AddDays(1))
                                          .Create();

            var projeto = _fixture.Build<Projeto>()
                                  .With(p => p.Id, tarefaViewModel.ProjetoId)
                                  .Create();

            _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(projeto);
            _tarefaRepositoryMock.Setup(repo => repo.CountByProjetoIdAsync(It.IsAny<int>())).ReturnsAsync(0);
            _tarefaRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);
            _historicoRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<HistoricoTarefa>())).Returns(Task.CompletedTask);

            // Act
            var result = await _tarefaService.Create(tarefaViewModel);

            // Assert
            result.IsValid.Should().BeTrue();
            _tarefaRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Tarefa>()), Times.Once);
            _historicoRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<HistoricoTarefa>()), Times.Once);
        }


        [Fact]
        [Trait("Category", "Create")]
        [Trait("Scenario", "Project Not Found")]
        [Trait("ExpectedResult", "Error")]
        public async Task Create_ShouldReturnError_WhenProjectNotFound()
        {
            // Arrange
            var tarefaViewModel = _fixture.Build<TarefaViewModel>()
                                          .With(t => t.ProjetoId, 1)
                                          .Create();

            _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Projeto)null);

            // Act
            var result = await _tarefaService.Create(tarefaViewModel);

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _tarefaRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Tarefa>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "Update")]
        [Trait("Scenario", "Valid Update")]
        [Trait("ExpectedResult", "Success")]
        public async Task Update_ShouldReturnSuccess_WhenValidUpdate()
        {
            // Arrange
            var tarefaViewModel = _fixture.Build<TarefaViewModel>()
                                          .With(t => t.ProjetoId, 1)
                                          .Create();

            var existingTarefa = _fixture.Build<Tarefa>()
                                         .With(t => t.Id, 1)
                                         .With(t => t.ProjetoId, tarefaViewModel.ProjetoId)
                                         .With(t => t.Prioridade, 0)
                                         .Create();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            _httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(user);
            _tarefaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingTarefa);
            _historicoRepositoryMock.Setup(repo => repo.InsertAllAsync(It.IsAny<IEnumerable<HistoricoTarefa>>())).Returns(Task.CompletedTask);
            _tarefaRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);

            // Act
            var result = await _tarefaService.Update(existingTarefa.Id, tarefaViewModel);

            // Assert
            result.IsValid.Should().BeTrue();
            _historicoRepositoryMock.Verify(repo => repo.InsertAllAsync(It.IsAny<IEnumerable<HistoricoTarefa>>()), Times.Once);
            _tarefaRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Tarefa>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "Delete")]
        [Trait("Scenario", "Valid Delete")]
        [Trait("ExpectedResult", "Success")]
        public async Task Delete_ShouldReturnSuccess_WhenValidDelete()
        {
            // Arrange
            var existingTarefa = _fixture.Build<Tarefa>()
                                         .With(t => t.Id, 1)
                                         .Create();

            _tarefaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingTarefa);
            _tarefaRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _tarefaService.Delete(existingTarefa.Id);

            // Assert
            result.IsValid.Should().BeTrue();
            _tarefaRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "FindAll")]
        [Trait("Scenario", "Tasks Exist")]
        [Trait("ExpectedResult", "Success")]
        public async Task FindAll_ShouldReturnSuccess_WhenTarefasExist()
        {
            // Arrange
            var tarefas = _fixture.CreateMany<Tarefa>(3).ToList();
            _tarefaRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tarefas);

            // Act
            var result = await _tarefaService.FindAll();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(tarefas);
        }

        [Fact]
        [Trait("Category", "FindById")]
        [Trait("Scenario", "Task Exists")]
        [Trait("ExpectedResult", "Success")]
        public async Task FindById_ShouldReturnSuccess_WhenTarefaExists()
        {
            // Arrange
            var tarefa = _fixture.Create<Tarefa>();
            _tarefaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(tarefa);

            // Act
            var result = await _tarefaService.FindById(tarefa.Id);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(tarefa);
        }

        [Fact]
        [Trait("Category", "FindById")]
        [Trait("Scenario", "Task Does Not Exist")]
        [Trait("ExpectedResult", "Error")]
        public async Task FindById_ShouldReturnError_WhenTarefaDoesNotExist()
        {
            // Arrange
            _tarefaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Tarefa)null);

            // Act
            var result = await _tarefaService.FindById(1);

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        [Trait("Category", "GetPerformanceReport")]
        [Trait("Scenario", "User is Admin")]
        [Trait("ExpectedResult", "Success")]
        public async Task GetPerformanceReport_ShouldReturnSuccess_WhenUserIsAdmin()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            var desempenhos = _fixture.CreateMany<DesempenhoUsuario>(3).ToList();

            _httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(user);
            _tarefaRepositoryMock.Setup(repo => repo.GetPerformanceReport()).ReturnsAsync(desempenhos);

            // Act
            var result = await _tarefaService.GetPerformanceReport();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(desempenhos);
        }

        [Fact]
        [Trait("Category", "GetPerformanceReport")]
        [Trait("Scenario", "User is not Admin")]
        [Trait("ExpectedResult", "Error")]
        public async Task GetPerformanceReport_ShouldReturnError_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "User")
            }, "mock"));

            _httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(user);

            // Act
            var result = await _tarefaService.GetPerformanceReport();

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Category", "GetPerformanceReport")]
        [Trait("Scenario", "No Performance Data")]
        [Trait("ExpectedResult", "Error")]
        public async Task GetPerformanceReport_ShouldReturnError_WhenNoPerformanceData()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Role, "Admin")
            ], "mock"));

            _httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(user);
            _tarefaRepositoryMock.Setup(repo => repo.GetPerformanceReport()).ReturnsAsync(new List<DesempenhoUsuario>());

            // Act
            var result = await _tarefaService.GetPerformanceReport();

            // Assert
            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
