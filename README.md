# 💰 Sistema de Controle Financeiro — Baseado em LightNap

Este projeto é uma aplicação Angular moderna voltada para o **controle de finanças pessoais ou empresariais**, construída sobre os princípios do [LightNap](https://github.com/SharpLogic/LightNap) — um starter kit full-stack leve que combina .NET, Angular e PrimeNG para acelerar o desenvolvimento de SPAs (Single Page Applications).


## 🎯 Objetivo

O sistema foi projetado para:

- ✅ Gerenciar receitas, despesas e saldos
- ✅ Exibir informações financeiras de forma clara e responsiva
- ✅ Tratar respostas da API com interceptação inteligente
- ✅ Garantir segurança com autenticação JWT
- ✅ Oferecer uma base escalável para novos módulos financeiros


## 📦 Tecnologias Utilizadas

### Backend (.NET)

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **ASP.NET Identity**
- **JWT Authentication**
- **SQL Server / SQLite / In-Memory**

### Frontend (Angular)

- **Angular 16+**
- **RxJS** para programação reativa
- **TypeScript** com tipagem segura
- **SCSS** para estilos modulares
- **Angular Routing** com alias dinâmicos
- **Interceptors** para tratamento de respostas e erros
- **ToastService** para notificações amigáveis
- **PrimeNG** para componentes visuais ricos

🧠 Arquitetura Baseada no LightNap

O [LightNap](https://github.com/SharpLogic/LightNap) é um kit full-stack que oferece:

- **ASP.NET Web API** como backend
- **SQL Server, SQLite ou In-Memory** como opções de persistência
- **ASP.NET Identity** para autenticação e autorização
- **JWT Token Management** para segurança baseada em tokens
- **Angular + PrimeNG** para o frontend dinâmico
- **Scaffolding** para geração de infraestrutura a partir de entidades

Este projeto adapta esses recursos para o domínio financeiro, mantendo a separação clara entre backend e frontend, com interceptadores, serviços e módulos bem definidos.

src/ ├── backend/ # ASP.NET Web API │ ├── Controllers/ # Endpoints da API │ ├── Models/ # Entidades e DTOs │ ├── Services/ # Regras de negócio │ └── Data/ # Persistência e contexto ├── frontend/ # Aplicação Angular │ ├── app/ │ │ ├── core/ # Serviços e utilitários globais │ │ ├── identity/ # Autenticação e identidade do usuário │ │ ├── routing/ # Alias de rotas e navegação │ │ └── features/ # Módulos de funcionalidades (ex: dashboard, transações) └── environments/ # Configurações por ambiente


---

## ⚙️ Instalação e Execução

### Backend (.NET)

```bash
git clone https://github.com/Vinicius-Fregonesi/Financas-Pessoais.git
# Acesse a pasta do backend
cd src/backend

# Restaure os pacotes
dotnet restore

# Execute a aplicação
dotnet run
# Clone o repositório

# Acesse a pasta
cd financas-pessoais

# Instale as dependências
npm install

# Rode o servidor de desenvolvimento
ng serve
```

A aplicação estará disponível em `http://localhost:4200`.

---

## 🔐 Autenticação

O sistema utiliza **JWT (JSON Web Tokens)** para autenticação. Em caso de sessão expirada (erro 401), o usuário é automaticamente deslogado e redirecionado para a tela de login.

---

## 📡 Interceptor de API

Todas as respostas da API passam por um interceptor que:

- Exibe mensagens amigáveis ao usuário
- Redireciona em caso de sessão expirada
- Filtra mensagens técnicas (stack traces, caminhos de arquivos)
- Clona respostas de sucesso para facilitar o consumo

---

## 📊 Funcionalidades Financeiras (em desenvolvimento)

- Registro de receitas e despesas
- Visualização de saldo atual
- Histórico de transações
- Filtros por período e categoria
- Painel com gráficos e indicadores

---

## 📷 Screenshots (opcional)

Você pode adicionar imagens da interface aqui:

```md
![Tela de Login](./assets/screens/login.png)
![Dashboard Financeiro](./assets/screens/dashboard.png)
```

---

## 🤝 Contribuição

Contribuições são bem-vindas! Para colaborar:

1. Faça um fork deste repositório
2. Crie uma branch com sua feature: `git checkout -b minha-feature`
3. Commit suas alterações: `git commit -m 'feat: minha nova feature'`
4. Push para o repositório remoto: `git push origin minha-feature`
5. Abra um Pull Request

---

## 📚 Créditos

Este projeto foi inspirado no [LightNap](https://github.com/SharpLogic/LightNap), criado pela equipe da SharpLogic. O LightNap combina .NET Web API, ASP.NET Identity, JWT, Angular e PrimeNG para fornecer uma infraestrutura completa para SPAs. Este sistema de controle financeiro adapta esses princípios para o domínio das finanças.

---

## 📄 Licença

Este projeto está licenciado sob a **MIT License**. Veja o arquivo [LICENSE](./LICENSE) para mais detalhes.
```

