# 🔧 Configuração do GitHub Actions

## ⚠️ IMPORTANTE: Leia Isso Primeiro!

**Status Atual:** A criação automática de Pull Requests está **DESABILITADA** por padrão devido a restrições de permissão do GitHub Actions.

**O que está funcionando:**
- ✅ Validação automática (build e testes)
- ✅ Verificação de nomenclatura de branches
- ✅ Relatórios de testes

**O que NÃO está funcionando:**
- ❌ Criação automática de Pull Requests

---

## ⚠️ Problema: "GitHub Actions is not permitted to create or approve pull requests"

Este erro ocorre porque o `GITHUB_TOKEN` padrão tem permissões limitadas. Para que os workflows possam criar Pull Requests automaticamente, você precisa habilitar as permissões corretas.

## ✅ Solução: Configurar Permissões do Workflow

### **Passo 1: Habilitar Permissões de Leitura/Escrita**

1. Acesse o repositório no GitHub
2. Vá em **Settings** → **Actions** → **General**
3. Role até a seção **"Workflow permissions"**
4. Selecione: **"Read and write permissions"**
5. ✅ Marque a opção: **"Allow GitHub Actions to create and approve pull requests"**
6. Clique em **Save**

### **Captura de Tela do Local:**
```
Settings
  └── Actions
        └── General
              └── Workflow permissions
                    ├── ○ Read repository contents and packages permissions (padrão)
                    └── ● Read and write permissions (selecione esta)
                          └── ☑ Allow GitHub Actions to create and approve pull requests
```

---

## 🔒 Alternativa: Usar Personal Access Token (PAT)

Se você não quiser habilitar permissões de escrita globalmente, pode usar um PAT:

### **Passo 1: Criar um Personal Access Token**

1. Vá em **Settings** (seu perfil) → **Developer settings** → **Personal access tokens** → **Tokens (classic)**
2. Clique em **Generate new token (classic)**
3. Dê um nome descritivo: `ProjetoExemplo - Auto PR`
4. Selecione o escopo: **`repo`** (acesso completo aos repositórios)
5. Clique em **Generate token**
6. **⚠️ IMPORTANTE:** Copie o token gerado (você não poderá vê-lo novamente)

### **Passo 2: Adicionar o Token como Secret**

1. Vá em **Settings** do repositório → **Secrets and variables** → **Actions**
2. Clique em **New repository secret**
3. Nome: `PAT_TOKEN`
4. Value: Cole o token copiado
5. Clique em **Add secret**

### **Passo 3: Atualizar o Workflow**

Se você criou o PAT, descomente as linhas nos workflows que usam `secrets.PAT_TOKEN`.

---

## 📋 Verificação das Configurações

Após configurar, verifique se:

- ✅ **Workflow permissions** está como "Read and write permissions"
- ✅ Opção **"Allow GitHub Actions to create and approve pull requests"** está marcada
- ✅ O repositório permite workflows para criar PRs

---

## 🚀 Testando a Configuração

Após configurar, faça um push em uma branch `feature/*`:

```bash
git checkout -b feature/teste-workflow
git commit --allow-empty -m "test: verificar criação automática de PR"
git push origin feature/teste-workflow
```

O workflow deve:
1. ✅ Executar build e testes
2. ✅ Criar automaticamente um Pull Request para `developer`
3. ✅ Não mostrar erro de permissão

---

## 🔗 Documentação Oficial

- [Managing GitHub Actions permissions](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/managing-github-actions-settings-for-a-repository)
- [Automatic token authentication](https://docs.github.com/en/actions/security-guides/automatic-token-authentication)
- [Permissions for the GITHUB_TOKEN](https://docs.github.com/en/actions/security-guides/automatic-token-authentication#permissions-for-the-github_token)

---

## 🛡️ Segurança

As permissões configuradas são específicas para workflows no repositório e não afetam outros aspectos de segurança. O `GITHUB_TOKEN` é temporário e expira após a execução do workflow.

### Permissões Configuradas nos Workflows:

**validate-feature.yml:**
- `contents: write` - Necessário para checkout com token
- `pull-requests: write` - Necessário para criar PRs
- `issues: write` - Necessário para vincular issues (opcional)

**create-release.yml:**
- `contents: write` - Necessário para criar releases e tags
- `pull-requests: write` - Necessário para validar PRs

---

## ❓ Troubleshooting

### Erro persiste após configuração?

1. Verifique se você salvou as configurações corretamente
2. Aguarde alguns minutos para as configurações propagarem
3. Execute o workflow novamente
4. Verifique os logs do workflow para detalhes do erro

### Não vejo a opção "Allow GitHub Actions to create and approve pull requests"?

- Esta opção só aparece se você selecionar "Read and write permissions"
- Certifique-se de estar nas configurações do **repositório**, não do seu perfil

---

**Última atualização:** 2025-11-21 (Migração para .NET 8)
