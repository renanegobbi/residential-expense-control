# residential-expense-control
Sistema completo de gestão de despesas residenciais com .NET 8 Web API, EF Core, SQLite e React (TypeScript).

<h4 align="center"> 
  <a href="#Tecnologias-e-ferramentas">Tecnologias e ferramentas</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#sobre-o-projeto">Sobre o projeto</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#Demonstração">Demonstração</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  </br>
  <a href="#Como-usar">Como usar</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#Licença">Licença</a>
</h4>

## Tecnologias e ferramentas

### Backend (API)

O backend da aplicação foi desenvolvido utilizando **ASP.NET Core Web API** com arquitetura em camadas, separando responsabilidades entre **Api, Domain, Core e Infrastructure**.

Tecnologias utilizadas:

- **.NET 8** – Framework principal para construção da Web API.
- **ASP.NET Core Web API** – Criação dos endpoints REST para gerenciamento de pessoas, categorias e transações.
- **Entity Framework Core** – ORM utilizado para persistência e manipulação dos dados.
- **SQLite** – Banco de dados leve utilizado para armazenamento das informações da aplicação.
- **Swagger / Swashbuckle** – Documentação interativa da API para testes dos endpoints.

Ferramentas utilizadas:

- **Visual Studio 2022** – IDE utilizada para desenvolvimento da API.
- **DBeaver** – Ferramenta utilizada para visualização e consulta dos dados no banco SQLite.

---

### Frontend (React)

O frontend da aplicação foi desenvolvido utilizando **React com TypeScript**, com foco em organização do código, separação de responsabilidades e comunicação com a API através de requisições HTTP.

Tecnologias e bibliotecas utilizadas:

- **React 19** – Biblioteca principal para construção da interface do usuário.
- **TypeScript** – Tipagem estática para maior segurança e organização do código.
- **Vite** – Ferramenta moderna de build e desenvolvimento para aplicações React, oferecendo inicialização rápida e hot reload.
- **Redux Toolkit** – Gerenciamento global de estado da aplicação.
- **React Router DOM** – Controle de rotas e navegação entre páginas.
- **Axios** – Cliente HTTP utilizado para comunicação com a API.
- **Styled Components** – Estilização de componentes utilizando CSS-in-JS.
- **React Icons** – Biblioteca de ícones utilizada na interface.
- **React Spinners** – Componentes de loading para feedback visual durante requisições.
- **ESLint** – Ferramenta de análise estática para padronização e qualidade de código.

Ferramentas utilizadas:

- **Visual Studio Code** – Editor utilizado para desenvolvimento do frontend.
- **Node.js / npm** – Gerenciamento de dependências e execução da aplicação.
