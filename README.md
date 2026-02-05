# Desafio Clientes - .NET

API em .NET para cadastro e listagem de clientes.

O projeto implementa:
- POST `/clientes` para cadastro com validações e bloqueio de e-mail duplicado
json
{
  "nome": "Maria",
  "email": "maria@email.com"
}

- GET `/clientes` para listagem dos clientes cadastrados

## Tecnologias utilizadas
- .NET 8
- C#
- ASP.NET Core
- Minimal API
- Persistência em memória (ConcurrentDictionary)

## Breve explicação das decisões técnicas
- Foi utilizada **Minimal API** para manter o projeto simples e direto, adequado ao escopo do desafio.
- A **persistência foi feita em memória** usando `ConcurrentDictionary` para atender ao requisito de persistência sem depender de banco/EF Core, facilitando a execução e avaliação do teste.
- A verificação de **e-mail duplicado** é feita pela chave (e-mail) no dicionário, retornando **409 Conflict** quando já existe.
- As validações de **nome obrigatório** e **e-mail obrigatório/válido** retornam **400 Bad Request** com detalhes dos campos.


## Por que não usei EF Core?
O desafio permite persistência em memória. Optei por essa abordagem para manter o projeto simples e rápido de executar/avaliar,
focando nas regras do endpoint (validações e bloqueio de e-mail duplicado). Em um cenário real, a camada de persistência poderia
ser substituída por EF Core + SQLite sem alterar os endpoints.


## Como rodar o projeto

### Visual Studio 2022
1. Abra a solution
2. Execute com F5

A API será iniciada em uma URL como:
`https://localhost:{porta}`

### Terminal
Na pasta do projeto:

```bash
dotnet restore
dotnet run
