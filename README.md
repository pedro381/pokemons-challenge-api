```markdown
# Pokémons Challenge API

Uma API robusta para listar, buscar e capturar Pokémons, além de gerenciar treinadores e suas capturas, utilizando dados da PokéAPI.

---

## Descrição

O **Pokémons Challenge API** é uma aplicação backend desenvolvida para demonstrar habilidades em desenvolvimento de APIs, consumindo a PokéAPI para listagem e consulta de Pokémons. Além disso, a aplicação possibilita o cadastro de treinadores, o registro de capturas de Pokémons e a listagem dos Pokémons capturados por cada treinador.

---

## Tecnologias e Bibliotecas Utilizadas

- **Linguagem:** C#
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQLite
- **Cliente HTTP:** Refit (para consumo da PokéAPI)
- **Documentação:** Swagger (Swashbuckle)
- **Testes:** xUnit, Moq, InMemoryDatabase (para testes unitários)
- **Outras Bibliotecas:** 
  - Microsoft.EntityFrameworkCore.Design (para migrações)
  - Microsoft.AspNetCore.Mvc

---

## Como Instalar e Usar o Projeto

### Pré-requisitos

- [.NET 6 ou superior](https://dotnet.microsoft.com/download)
- [SQLite](https://www.sqlite.org/index.html) (opcional, já que a aplicação usa um arquivo local)
- Git

### Passo a Passo

1. **Clone o Repositório**

   ```bash
   git clone https://github.com/pedro381/pokemons-challenge-api.git
   cd pokemons-challenge-api
   ```

2. **Configuração do Banco de Dados**

   - O projeto está configurado para usar SQLite. Verifique o arquivo `appsettings.json` para a connection string (por padrão: `"Data Source=pokemon.db"`).
   - Execute as migrações para criar o banco de dados:
     
     ```bash
     dotnet ef migrations add InitialCreate --project PokemonsChallenge.Repository --startup-project PokemonsChallenge.Api --output-dir Migrations
     dotnet ef database update --project PokemonsChallenge.Repository --startup-project PokemonsChallenge.Api
     ```

3. **Executar a API**

   - Navegue até o projeto da API:
     
     ```bash
     cd PokemonsChallenge.Api
     dotnet run
     ```
     
   - A API ficará disponível em `https://localhost:7048` (ou outra porta configurada). A documentação interativa do Swagger pode ser acessada em `https://localhost:7048/swagger/index.html`.

4. **Executar os Testes Unitários**

   - Navegue até o diretório do projeto de testes e execute:
     
     ```bash
     dotnet test
     ```

---

## Arquitetura do Projeto

O projeto está organizado em camadas para promover a separação de responsabilidades:

- **API (PokemonsChallenge.Api):**
  - Controllers e configuração do servidor (Program.cs).
  - Documentação via Swagger.

- **Aplicação / Serviço (PokemonsChallenge.Service):**
  - Regras de negócio e serviços (ex.: PokémonService, TrainerService).
  - Interfaces para abstrair a lógica de negócio.

- **Domínio (PokemonsChallenge.Domain):**
  - Entidades, modelos e DTOs (ex.: Trainer, Pokemon, CapturedPokemon, PokemonDto).
  
- **Infraestrutura / Repositório (PokemonsChallenge.Repository):**
  - Implementações de repositórios e acesso a dados via Entity Framework Core.
  - Configuração do DbContext e migrações.

- **Testes (PokemonsChallenge.Test):**
  - Testes unitários para API, Serviços e Repositórios.
  - Utilização do xUnit, Moq e InMemoryDatabase para simulação do banco de dados.

---

## Features do Projeto

- **Listagem de Pokémons Aleatórios:** Consome a PokéAPI para retornar 10 Pokémons aleatórios.
- **Consulta de Pokémon por ID:** Permite buscar os detalhes de um Pokémon específico.
- **Cadastro de Treinador:** Possibilita o registro de treinadores (nome, idade, CPF) no banco de dados SQLite.
- **Captura de Pokémon:** Permite que um treinador capture um Pokémon, registrando a captura no banco de dados.
- **Listagem de Pokémons Capturados:** Exibe a lista de Pokémons capturados por um treinador específico.
- **Interface Mobile-Friendly:** Uma interface web responsiva que funciona bem em dispositivos móveis.
- **Documentação Swagger:** Interface interativa para testar os endpoints da API.
- **Testes Unitários:** Cobertura dos principais cenários utilizando xUnit e Moq.
- **Integração com Refit:** Facilita o consumo da PokéAPI para obtenção dos dados dos Pokémons.

---

## Contribuidores e Desenvolvedores

- **Pedro Souza** – [GitHub](https://github.com/pedro381)

Sinta-se à vontade para contribuir, reportar problemas e sugerir melhorias. Todas as contribuições são bem-vindas!

---

## Conclusão

Agradecemos por explorar o **Pokémons Challenge API**. Este projeto foi desenvolvido para demonstrar uma arquitetura limpa e escalável utilizando tecnologias modernas no ecossistema .NET. Esperamos que este projeto sirva como base para futuras inovações e contribuições.

Se você deseja contribuir, por favor, faça um fork do repositório, crie sua feature branch, implemente suas mudanças e envie um pull request.  
**Obrigado por seu interesse e boas contribuições!**
```
