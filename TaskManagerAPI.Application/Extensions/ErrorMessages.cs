using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Extensions
{
    internal class ErrorMessages
    {
        public static string UserNotFoundError(Type entityType) => "Tabela usuario esta vazia";
        public static string IdNotFoundError(Type entityType, int Id) => $"Id: {Id} nao encontrado";
        public static string TaskNotFoundError(Type entityType, int id) => $"tarefa não encontrada - {id}";
        public static string ProjectNotFoundError(Type entityType, int id) => $"Projeto não encontrada - {id}";
        public static string ErrorUpdatingError(Type entityType) => $"Erro ao atualizar {entityType.Name}";
        public static string ErrorCreatingError(Type entityType) => $"Erro ao criar {entityType.Name}";
        public static string ProjectDeletionErrorDueToPendingTasks() => "Não é possível remover o projeto enquanto houver tarefas pendentes.";
        public static string DesempenhoNaoEncontrado() => "Nenhum desempenho encontrado.";
        public static string ProjectNameAlreadyExistsError(string projectName) => $"Um projeto com este nome já existe: {projectName}";
        public static string ProjectTaskLimitReached() => "O projeto já atingiu o limite máximo de 20 tarefas.";
    }
}
