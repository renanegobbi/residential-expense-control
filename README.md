
# Controle de despesas residenciais
Sistema completo de gestão de despesas residenciais com .NET 8 Web API, EF Core, SQLite e React (TypeScript).

# Sumário

- [1. Tecnologias e ferramentas](#1-tecnologias-e-ferramentas)
  - [1.1 Backend (API)](#11-backend-api)
  - [1.2 Frontend (React)](#12-frontend-react)

- [2. Sobre o projeto](#2-sobre-o-projeto)
  - [2.1 Explicação do projeto](#21-explicação-do-projeto)
  - [2.2 Regras de negócio](#22-regras-de-negócio)
  - [2.3 Arquitetura](#23-arquitetura)
  - [2.4 Modelo de dados (Banco)](#24-modelo-de-dados-banco)
  - [2.5 Funcionalidades implementadas](#25-funcionalidades-implementadas)

- [3. Demonstração](#3-demonstração)

- [4. Como usar](#4-como-usar)
  - [4.1 Clonar o repositório](#41-clonar-o-repositório)
    - [4.1.1 Acessar a pasta do projeto](#411-acessar-a-pasta-do-projeto ) 
  - [4.2 Executando o Backend (API)](#42-executando-o-backend-api)
    - [4.2.1 Visual Studio](#421-visual-studio)
    - [4.2.2 .NET CLI](#422-net-cli)
  - [4.3 Executando o Frontend](#43-executando-o-frontend)

- [Licença](#licença)

---

# 1. Tecnologias e ferramentas

### 1.1 Backend (API)

O backend da aplicação foi desenvolvido utilizando **ASP.NET Core Web API** com arquitetura em camadas, separando responsabilidades entre **Api, Domain, Core e Infrastructure**.

Tecnologias utilizadas:

- [.NET 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0) - Plataforma utilizada para o desenvolvimento da aplicação.
- [Swagger](https://swagger.io/) - framework que disponibiliza Ferramentas para gerar a documentação da API.
- [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/) - Mapeador de banco de dados de objeto para .NET.
- [SQLite](https://www.sqlite.org/docs.html) - Banco de dados relacional.
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/) - Biblioteca .NET para construir regras de validação.

Ferramentas utilizadas:

- [Visual Studio 2022 Community](https://visualstudio.microsoft.com/pt-br/vs/community/) - IDE utilizada para desenvolver a solução.
- [DBeaver Community](https://dbeaver.io/download/) - Ferramenta de banco de dados multiplataforma para desenvolvedores.

### 1.2 Frontend (React)

O frontend da aplicação foi desenvolvido utilizando **React com TypeScript**, com foco em organização do código, separação de responsabilidades e comunicação com a API através de requisições HTTP.

Tecnologias e bibliotecas utilizadas:

- [React 19](https://react.dev/blog/2024/12/05/react-19) - Biblioteca principal para construção da interface do usuário.
- [TypeScript](https://www.typescriptlang.org/) - Tipagem estática para maior segurança e organização do código.
- [Vite](https://vite.dev/) - Ferramenta moderna de build e desenvolvimento para aplicações React, oferecendo inicialização rápida e hot reload.
- [Redux Toolkit](https://redux-toolkit.js.org/) - Gerenciamento global de estado da aplicação.
- [React Router DOM](https://reactrouter.com/) - Controle de rotas e navegação entre páginas.
- [Axios](https://axios-http.com/) - Cliente HTTP utilizado para comunicação com a API.
- [Styled Components](https://styled-components.com/) - Estilização de componentes utilizando CSS-in-JS.
- [React Icons](https://react-icons.github.io/react-icons/) - Conjunto de ícones para uso em componentes React.
- [React Spinners](https://www.npmjs.com/package/react-spinners) - Componentes de loading para feedback visual durante requisições.
- [ESLint](https://eslint.org/) - Ferramenta de análise estática para padronização e qualidade de código.

Ferramentas utilizadas:

- [Visual Studio Code](https://code.visualstudio.com/) - Editor utilizado para desenvolvimento do frontend.
- [Node.js / npm](https://nodejs.org/pt-br) - Gerenciamento de dependências e execução da aplicação.

---

# 2. Sobre o projeto

### 2.1 Explicação do projeto

O **Residential Expense Control** é uma aplicação full stack desenvolvida para gerenciamento de despesas e receitas residenciais.                                 
O sistema permite registrar pessoas, categorias e transações financeiras, possibilitando o controle de receitas, despesas e o saldo financeiro de cada pessoa. 
A aplicação foi desenvolvida utilizando **ASP.NET Core Web API** no backend e **React com TypeScript** no frontend.

### 2.2 Regras de negócio

- Pessoas menores de 18 anos **podem registrar apenas despesas**.
- O tipo da transação deve ser compatível com a finalidade da categoria.
- Ao remover uma pessoa, todas as suas transações também são removidas.

### 2.3 Arquitetura

O backend foi estruturado seguindo uma arquitetura em camadas, separando responsabilidades entre **API, Domain, Core e Infrastructure**, facilitando manutenção, organização do código e aplicação das regras de negócio.

```
🗂️ residential-expense-control
│
├── src
│ ├── ResidentialExpenseControl.Api
│ │ → Camada de apresentação (Web API)
│ │ → Controllers, filtros, configurações e documentação Swagger
│ │
│ ├── ResidentialExpenseControl.Core
│ │ → Componentes compartilhados da aplicação
│ │ → Entidades base e contratos comuns
│ │
│ ├── ResidentialExpenseControl.Domain
│ │ → Camada de domínio da aplicação
│ │ → Entidades, regras de negócio, validações e serviços
│ │
│ └── ResidentialExpenseControl.Infrastructure
│ → Camada de infraestrutura
│ → Acesso a dados, DbContext, repositórios, migrations e seed
│
├── frontend
│ └── personal-finances
│ → Aplicação React responsável pela interface do usuário
│
└── README.md
  → Documentação do projeto
```

## 2.4 Modelo de dados (Banco)

A aplicação utiliza **SQLite** para persistência local dos dados.
Na primeira execução, o banco de dados é criado automaticamente dentro da pasta `Data` do projeto `ResidentialExpenseControl.Api`.
Além disso, um processo de seed é executado para popular dados iniciais e facilitar o uso da aplicação.

A estrutura do banco de dados é composta pelas seguintes tabelas:

- **People**
- **Categories**
- **Transactions**

<p align="">
  <img src="https://github.com/renanegobbi/residential-expense-control/blob/main/github/BD.png"/>
</p>

### 2.5 Funcionalidades implementadas

A aplicação permite o gerenciamento de despesas e receitas residenciais através dos seguintes módulos:

### Cadastro de Pessoas

Permite o gerenciamento de pessoas que participam do controle financeiro.

Funcionalidades disponíveis:

- Criação de novas pessoas
- Edição de dados de pessoas cadastradas
- Remoção de pessoas
- Listagem de todas as pessoas cadastradas

Cada pessoa possui os seguintes campos:

| Campo (Banco/API) | Nome no sistema | Descrição |
|------------------|----------------|-----------|
| Id | Identificador | Identificador único da pessoa |
| Name | Nome | Nome da pessoa |
| Age | Idade | Idade da pessoa |

### Cadastro de Categorias

Permite a criação e consulta de categorias utilizadas para classificar transações financeiras.

Funcionalidades disponíveis:

- Criação de categorias
- Edição de dados de categorias cadastradas
- Remoção de categorias
- Listagem de categorias cadastradas

Cada categoria possui os seguintes campos:

| Campo (Banco/API) | Nome no sistema | Descrição |
|------------------|----------------|-----------|
| Id | Identificador | Identificador único da categoria |
| Description | Descrição | Descrição da categoria utilizada para classificar transações |
| Purpose | Finalidade | Define se a categoria pode ser usada para Receita, Despesa ou Ambas |

### Cadastro de Transações

Permite registrar movimentações financeiras associadas a uma pessoa e a uma categoria.

Funcionalidades disponíveis:

- Registro de novas transações
- Edição de dados de transações cadastradas
- Remoção de transações
- Listagem de transações cadastradas

Cada transação possui os seguintes campos:

| Campo (Banco/API) | Nome no sistema | Descrição |
|------------------|----------------|-----------|
| Id | Identificador | Identificador único da transação |
| Description | Descrição | Descrição da transação financeira |
| Amount | Valor | Valor da transação |
| Type | Tipo | Define se a transação é Receita ou Despesa |
| CategoryId | Categoria | Identificador da categoria associada à transação |
| PersonId | Pessoa | Identificador da pessoa responsável pela transação |

### Consulta de Totais por Pessoa

Permite visualizar um resumo financeiro por pessoa cadastrada.

Para cada pessoa são exibidos:

- Total de receitas
- Total de despesas
- Saldo final (receitas - despesas)

Também é apresentado um **resumo geral consolidado** com:

- Total geral de receitas
- Total geral de despesas
- Saldo líquido da aplicação

### Consulta de Totais por Categoria

Permite visualizar um resumo financeiro agrupado por categoria.

Para cada categoria são exibidos:

- Total de receitas
- Total de despesas
- Saldo final

Também é apresentado um **total geral consolidado** considerando todas as categorias.

# 3. Demonstração

A seguir estão algumas telas da aplicação e da documentação da API.

### Documentação da API (Swagger)

A API possui documentação interativa utilizando Swagger, permitindo visualizar e testar os endpoints disponíveis.

<p align="center">
  <img src="https://github.com/renanegobbi/residential-expense-control/blob/main/github/API.PNG"/>
</p>

### Tela de Pessoas

Interface responsável pelo cadastro e gerenciamento das pessoas cadastradas no sistema.

<p align="center">
  <img src="https://github.com/renanegobbi/residential-expense-control/blob/main/github/Frontend-Pessoas.PNG"/>
</p>

### Tela de Transações

Tela utilizada para registrar transações financeiras associadas a uma pessoa e uma categoria.

<p align="center">
  <img src="https://github.com/renanegobbi/residential-expense-control/blob/main/github/Frontend-Transa%C3%A7%C3%B5es.PNG"/>
</p>

### Tela de Categorias

Permite visualizar e cadastrar categorias utilizadas para classificar receitas e despesas.

<p align="center">
  <img src="https://github.com/renanegobbi/residential-expense-control/blob/main/github/Frontend-Categorias.PNG"/>
</p>

### Tela de Totais

Apresenta um resumo financeiro com totais de receitas, despesas e saldo consolidado.

<p align="center">
  <img src="https://github.com/renanegobbi/residential-expense-control/blob/main/github/Frontend-Totais.PNG"/>
</p>

# 4. Como usar

### 4.1 Clonar o repositório
```bash
git clone https://github.com/renanegobbi/residential-expense-control.git
```

### 4.1.1 Acessar a pasta do projeto
```bash
cd residential-expense-control
```

## 4.2. Executando o Backend (API)
***Pré-requisito: .NET 8 SDK***    

Existem duas formas de executar o backend:

### 4.2.1 Visual Studio

- Abra a solução no **Visual Studio** 2022
- Abrir a solution (**ResidentialExpenseControl.sln**) no Visual Studio.
- No Solution Explorer, localizar o projeto **ResidentialExpenseControl.Api** e o defina como projeto de inicialização.
- No menu superior, selecionar o perfil de execução (por exemplo, **ResidentialExpenseControl - Dev**).
- Clicar no botão **Run/Play** para iniciar.
- A API iniciará e a documentação poderá ser acessada em: https://localhost:5001/docs/index.html

### 4.2.2 .NET CLI
- Abra o terminal e execute os seguintes comandos:
```bash
cd src/ResidentialExpenseControl.Api
```
```bash
dotnet restore
```
```bash
dotnet run --launch-profile "ResidentialExpenseControl - Dev"
```
- Após a inicialização do aplicativo, a API estará disponível em: https://localhost:5001/docs/index.html

## 4.3. Executando o Frontend

***Pré-requisito: Node.js 20 ou superior*** 
- Certifique-se de que o backend esteja em execução antes de iniciar o frontend.
- Abra um terminal e execute:

```bash
cd frontend/personal-finances
```
```bash
npm install
```
```bash
npm run dev
```

- Após iniciar, a aplicação estará disponível em: http://localhost:5173

## Licença
Este projeto está sob a licença do MIT. Consulte a [LICENÇA](https://github.com/TesteReteste/lim/blob/master/LICENSE) para obter mais informações.
