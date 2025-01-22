# TaskManagerAPI

Este projeto oferece uma API para gerenciamento de tarefas com funcionalidades para relatórios de desempenho e gerenciamento de acesso. Abaixo estão as instruções para iniciar o projeto utilizando Docker Compose e detalhes sobre o endpoint protegido por uma política de segurança.

## Como Iniciar o Projeto com Docker Compose

1. Certifique-se de ter o Docker e o Docker Compose instalados na sua máquina.
2. No diretório raiz do projeto, localize o arquivo `docker-compose.yml`.
3. Para iniciar os serviços, execute o comando:

   ```bash
   docker-compose up --build
   ```

4. Aguarde até que todos os contêineres sejam inicializados corretamente.
5. A API estará disponível nos seguintes endpoints:

   ```
   http://localhost:8080
   http://localhost:8081
   ```

## Acessando o Relatório de Desempenho

### Endpoint Protegido

- Rota: `GET /relatorio-desempenho`
- Controlador: `TaskManagerController`

```csharp
/// <summary>
/// Gera relatórios de desempenho, como o número médio de tarefas concluídas
/// por usuário nos últimos 30 dias.
/// </summary>
/// <returns>Relatório de desempenho dos usuários.</returns>
/// <response code="200">Retorna o relatório de desempenho.</response>
/// <response code="403">Acesso negado. Apenas usuários com a função de "gerente" podem acessar este endpoint.</response>
/// <response code="500">Erro interno do servidor.</response>
[HttpGet("relatorio-desempenho")]
[Authorize(Policy = "AdminPolicy")]
[ProducesResponseType(typeof(IEnumerable<object>), 200)]
[ProducesResponseType(401)]
[ProducesResponseType(500)]
public async Task<ActionResult> GetPerformanceReport()
```

### Requisitos para Acessar

Este endpoint é protegido e requer autenticação usando uma política de segurança. Para acessar o relatório de desempenho, você precisa fornecer um cabeçalho HTTP com as seguintes informações:

- **Chave do Cabeçalho**: `X-User-Role`
- **Valor**: `Admin`

### Exemplo de Requisição no Postman

1. Abra o Postman.
2. Crie uma nova requisição GET com a URL:

   ```
   http://localhost:8080/api/tarefa/relatorio-desempenho
   ```

3. Adicione o cabeçalho:

   - **Key**: `X-User-Role`
   - **Value**: `Admin`

4. Envie a requisição.

### Possíveis Códigos de Resposta

- **200 OK**: Sucesso na obtenção do relatório de desempenho.
- **401 Unauthorized**: Falha na autenticação devido à falta de permissões adequadas.
- **500 Internal Server Error**: Erro inesperado no servidor.

## Observações Importantes

- Certifique-se de configurar corretamente as variáveis de ambiente definidas no `docker-compose.yml` para que a aplicação funcione corretamente.
- Para acessar endpoints protegidos, verifique se o sistema de autenticação e a política `AdminPolicy` estão configurados corretamente no `Startup.cs` ou `Program.cs`, dependendo da versão do .NET usada.
- Para testar endpoints, recomenda-se o uso do Postman para facilitar a configuração de cabeçalhos e autenticação.

## Contribuição

Se desejar contribuir com melhorias, sinta-se à vontade para abrir um Pull Request ou relatar problemas na página de issues.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

