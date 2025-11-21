# ProjetoExemplo - Template .NET 8 🚀

Template padrão para APIs ASP.NET Core com .NET 8, incluindo estrutura de projeto, testes automatizados e CI/CD com GitHub Actions.

## 📋 Sobre o Projeto

Este é um projeto template para desenvolvimento de APIs .NET, estruturado em camadas e com automação completa de CI/CD.

**Framework:** .NET 8.0  
**Última Migração:** 2025-11-21 (de .NET Core 3.1 para .NET 8)

## 🏗️ Estrutura do Projeto

```
ProjetoExemplo/
├── API/              # Camada de apresentação (Controllers, Filters)
├── Business/         # Camada de negócios (Services, DTOs, Hubs)
├── Data/             # Camada de dados (Repositories, Models)
├── Testes/           # Testes unitários
├── .github/          # Workflows e configurações do GitHub Actions
└── scripts/          # Scripts auxiliares
```

## 🚀 Como Começar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (ou ajustar connection string)
- Visual Studio 2022+ ou VS Code

### Instalação

1. Clone o repositório:
```bash
git clone https://github.com/fabiacurti/ProjetoExemplo.git
cd ProjetoExemplo
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure a connection string em `API/appsettings.json`

4. Execute o projeto:
```bash
dotnet run --project API
```

5. Acesse a documentação Swagger:
```
https://localhost:5001/swagger/index.html
```

## 🧪 Executar Testes

```bash
dotnet test
```

## 📦 Principais Pacotes

- **ASP.NET Core 8.0** - Framework web
- **Swagger/OpenAPI** - Documentação da API
- **AutoMapper 13.0** - Mapeamento objeto-objeto
- **Dapper 2.1** - Micro ORM
- **NLog 5.3** - Logging
- **ClosedXML 0.104** - Manipulação de Excel
- **xUnit + Moq + FluentAssertions** - Testes

## 🔄 Workflow Git / CI-CD

### Estratégia de Branches

- **`main`** - Produção (protegida)
- **`developer`** - Desenvolvimento (protegida)
- **`feature/*`** - Novas funcionalidades
- **`hotfix/*`** - Correções urgentes

### GitHub Actions

O projeto possui workflows automáticos:

#### 🔍 Validação de Features (`validate-feature.yml`)
- Dispara em push para `feature/*` e `hotfix/*`
- Executa build e testes
- Cria PR automaticamente se bem-sucedido

#### 🚀 Release Automática (`create-release.yml`)
- Dispara em merge para `main`
- Valida PRs antes do merge
- Cria releases automáticas

### ⚠️ Configuração Obrigatória

Para que os workflows funcionem corretamente, **configure as permissões do GitHub Actions**:

1. Vá em **Settings** → **Actions** → **General**
2. Em "Workflow permissions", selecione: **"Read and write permissions"**
3. ✅ Marque: **"Allow GitHub Actions to create and approve pull requests"**

📖 [Guia completo de configuração](.github/GITHUB_ACTIONS_SETUP.md)

## 🎯 Fluxo de Trabalho

1. **Criar feature:**
```bash
git checkout developer
git pull
git checkout -b feature/minha-funcionalidade
```

2. **Desenvolver e commitar:**
```bash
git add .
git commit -m "feat: adiciona nova funcionalidade"
git push origin feature/minha-funcionalidade
```

3. **GitHub Actions automaticamente:**
   - ✅ Valida o código
   - ✅ Executa testes
   - ✅ Cria PR para `developer`

4. **Após aprovação do PR:**
   - Merge para `developer`
   - Depois: criar PR de `developer` → `main`
   - Após merge em `main`: release automática

## 📝 Convenções de Commit

Seguimos o padrão [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` Nova funcionalidade
- `fix:` Correção de bug
- `docs:` Alteração em documentação
- `test:` Adiciona ou corrige testes
- `refactor:` Refatoração de código
- `chore:` Tarefas de build, configs, etc.

**Dica:** Use `[skip-pr]` na mensagem para evitar criação automática de PR.

## 🔒 Proteção de Branches

Branches `main` e `developer` são protegidas e requerem:
- ✅ Pull Request aprovado
- ✅ Testes passando
- ✅ Build bem-sucedido
- ✅ Código revisado

## 🛠️ Tecnologias Utilizadas

- **Backend:** ASP.NET Core 8.0 (C#)
- **ORM:** Dapper
- **Logging:** NLog
- **Testes:** xUnit, Moq, FluentAssertions
- **Documentação:** Swagger/OpenAPI
- **CI/CD:** GitHub Actions
- **Real-time:** SignalR

## 📚 Documentação Adicional

- [Configuração do GitHub Actions](.github/GITHUB_ACTIONS_SETUP.md)
- [Proteção de Branches](.github/branch-protection.yml)
- [Swagger/API Docs](https://localhost:5001/swagger)

## 🤝 Contribuindo

1. Faça fork do projeto
2. Crie uma branch feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'feat: add amazing feature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é um template de uso interno.

## 👥 Contato

**Owner:** fabiacurti  
**Repositório:** [github.com/fabiacurti/ProjetoExemplo](https://github.com/fabiacurti/ProjetoExemplo)

---

**Versão do Template:** .NET 8.0  
**Última Atualização:** 2025-11-21
