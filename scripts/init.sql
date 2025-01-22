-- scripts/init.sql

-- Dropar tabelas existentes
DROP TABLE IF EXISTS HistoricoTarefa;
DROP TABLE IF EXISTS Comentario;
DROP TABLE IF EXISTS Tarefa;
DROP TABLE IF EXISTS Projeto;
DROP TABLE IF EXISTS Usuario;

-- Criar tabelas
CREATE TABLE IF NOT EXISTS Usuario (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL,
    Funcao TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Projeto (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL,
    UsuarioId INTEGER NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Tarefa (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Titulo TEXT NOT NULL,
    Descricao TEXT,
    DataVencimento DATETIME NOT NULL,
    Status INTEGER NOT NULL,
    Prioridade INTEGER NOT NULL,
    ProjetoId INTEGER NOT NULL,
    FOREIGN KEY (ProjetoId) REFERENCES Projeto(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Comentario (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Texto TEXT NOT NULL,
    DataComentario DATETIME NOT NULL,
    TarefaId INTEGER NOT NULL,
    UsuarioId INTEGER NOT NULL,
    FOREIGN KEY (TarefaId) REFERENCES Tarefa(Id) ON DELETE CASCADE,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS HistoricoTarefa (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CampoModificado TEXT NOT NULL,
    ValorAntigo TEXT,
    ValorNovo TEXT,
    DataModificacao DATETIME NOT NULL,
    TarefaId INTEGER NOT NULL,
    UsuarioId INTEGER NOT NULL,
    FOREIGN KEY (TarefaId) REFERENCES Tarefa(Id) ON DELETE CASCADE,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);

-- Inserir dados fictícios na tabela Usuario
INSERT INTO Usuario (Nome, Funcao) VALUES ('João Silva', 'Desenvolvedor');
INSERT INTO Usuario (Nome, Funcao) VALUES ('Maria Oliveira', 'Gerente');
INSERT INTO Usuario (Nome, Funcao) VALUES ('Carlos Souza', 'Analista');
INSERT INTO Usuario (Nome, Funcao) VALUES ('Ana Pereira', 'Gerente');

-- Inserir dados fictícios na tabela Projeto
INSERT INTO Projeto (Nome, UsuarioId) VALUES ('Projeto A', 2); -- Associado a Maria Oliveira (Gerente)
INSERT INTO Projeto (Nome, UsuarioId) VALUES ('Projeto B', 4); -- Associado a Ana Pereira (Gerente)
INSERT INTO Projeto (Nome, UsuarioId) VALUES ('Projeto C', 1); -- Associado a João Silva (Desenvolvedor)

-- Inserir dados fictícios na tabela Tarefa
INSERT INTO Tarefa (Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId) VALUES ('Tarefa 1', 'Descrição da Tarefa 1', '2024-12-25', 3, 1, 1);
INSERT INTO Tarefa (Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId) VALUES ('Tarefa 2', 'Descrição da Tarefa 2', '2024-12-21', 3, 2, 1);
INSERT INTO Tarefa (Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId) VALUES ('Tarefa 3', 'Descrição da Tarefa 3', '2023-09-30', 3, 3, 2);
INSERT INTO Tarefa (Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId) VALUES ('Tarefa 4', 'Descrição da Tarefa 4', '2023-09-26', 3, 1, 2);
INSERT INTO Tarefa (Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId) VALUES ('Tarefa 5', 'Descrição da Tarefa 5', '2023-09-28', 3, 2, 3);
INSERT INTO Tarefa (Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId) VALUES ('Tarefa 6', 'Descrição da Tarefa 6', '2023-09-24', 3, 3, 3);

-- Inserir dados fictícios na tabela Comentario
INSERT INTO Comentario (Texto, DataComentario, TarefaId, UsuarioId) VALUES ('Comentário 1', '2023-09-01', 1, 1);
INSERT INTO Comentario (Texto, DataComentario, TarefaId, UsuarioId) VALUES ('Comentário 2', '2023-09-02', 2, 2);
INSERT INTO Comentario (Texto, DataComentario, TarefaId, UsuarioId) VALUES ('Comentário 3', '2023-09-03', 3, 3);
INSERT INTO Comentario (Texto, DataComentario, TarefaId, UsuarioId) VALUES ('Comentário 4', '2023-09-04', 4, 4);
INSERT INTO Comentario (Texto, DataComentario, TarefaId, UsuarioId) VALUES ('Comentário 5', '2023-09-05', 5, 1);
INSERT INTO Comentario (Texto, DataComentario, TarefaId, UsuarioId) VALUES ('Comentário 6', '2023-09-06', 6, 2);

-- Inserir dados fictícios na tabela HistoricoTarefa
INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES ('Status', '0', '1', '2023-09-01', 1, 1);
INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES ('Prioridade', '1', '2', '2023-09-02', 2, 2);
INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES ('Descricao', 'Descrição antiga', 'Descrição nova', '2023-09-03', 3, 3);
INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES ('Status', '1', '2', '2023-09-04', 4, 4);
INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES ('Prioridade', '2', '3', '2023-09-05', 5, 1);
INSERT INTO HistoricoTarefa (CampoModificado, ValorAntigo, ValorNovo, DataModificacao, TarefaId, UsuarioId) VALUES ('Descricao', 'Descrição antiga', 'Descrição nova', '2023-09-06', 6, 2);
