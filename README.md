# Task Management System (Full Stack)

Uma solução completa de gerenciamento de tarefas que inclui autenticação JWT e funcionalidades CRUD, demonstrando proficiência em desenvolvimento Full Stack moderno.

## 🚀 Tecnologias

### Frontend (Client)
* **Framework:** React 18
* **Linguagem:** TypeScript
* **Estilização:** [Mencione sua biblioteca: Tailwind CSS / SASS]
* **Gerenciamento de Estado:** Redux [ou Context API]

### Backend (API)
* **Framework:** ASP.NET Core Web API (C#)
* **Autenticação:** JWT Token para segurança de rotas
* **Banco de Dados:** [Mencione o DB: PostgreSQL / SQL Server]
* **Segurança:** Hashing de senhas com BCrypt
* **Design:** Arquitetura RESTful

## ✨ Principais Funcionalidades

* **Autenticação:** Registro e Login de Usuários com Tokens JWT.
* **CRUD Completo:** Criação, Leitura, Edição e Exclusão de Tarefas.
* **Filtros:** Busca por status, prioridade (Alta, Média, Baixa) e projetos.
* **Estrutura:** Separação clara entre camadas de aplicação (Controllers, Services, Repositories).


## 🛠 Como Rodar o Projeto

1.  Clone ambos os repositórios (`TaskManagement-Client-ReactTS` e `TaskManagement-API-NETCore`).
2.  **API:** Navegue até o diretório da API, instale as dependências e rode o projeto (ex: `dotnet run`).
3.  **Client:** Navegue até o diretório do Client, instale as dependências (`npm install` ou `yarn`), e inicie (`npm start` ou `yarn start`).
4.  Configure as variáveis de ambiente (`.env` file) para apontar o Client para o endereço da API.
