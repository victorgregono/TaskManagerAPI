namespace TaskManager.Data.SqlQueries
{
    public static class TarefaQueries
    {
        public const string GetPerformanceReport = @"
            SELECT 
                u.Nome AS NomeUsuario,
                COUNT(t.Id) AS TarefasConcluidas,
                ROUND(COUNT(t.Id) * 1.0 / 30, 2) AS MediaTarefasPorDia
            FROM 
                Usuario u
            JOIN 
                Projeto p ON p.UsuarioId = u.Id
            JOIN 
                Tarefa t ON t.ProjetoId = p.Id
            WHERE 
                t.Status = 3  -- Considerando '3' como o status para concluído (ajuste se necessário)
                AND t.DataVencimento >= DATE('now', '-30 days')
                AND u.Funcao COLLATE NOCASE = 'gerente'
            GROUP BY 
                u.Nome;";
    }
}
