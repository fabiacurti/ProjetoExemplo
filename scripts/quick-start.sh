#!/bin/bash

# =============================================================================
# 🚀 QUICK START - CI/CD Setup
# =============================================================================
# 
# Script rápido para configurar o ambiente CI/CD do projeto
# 
# Uso: ./quick-start.sh
# =============================================================================

# Cores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
MAGENTA='\033[0;35m'
CYAN='\033[0;36m'
NC='\033[0m'

clear

echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${CYAN}🚀 ProjetoExemplo - Quick Start CI/CD${NC}"
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

# Verificar se está em um repositório Git
if [ ! -d ".git" ]; then
    echo -e "${RED}❌ Erro: Este diretório não é um repositório Git!${NC}"
    echo -e "${YELLOW}   Execute este script na raiz do repositório.${NC}"
    exit 1
fi

# Detectar arquivo .sln
echo -e "${BLUE}🔍 Detectando projeto...${NC}"
SLN_FILE=$(find . -maxdepth 1 -name "*.sln" | head -n 1)

if [ -z "$SLN_FILE" ]; then
    echo -e "${RED}❌ ERRO: Nenhum arquivo .sln encontrado na raiz do projeto${NC}"
    exit 1
fi

PROJECT_NAME=$(basename "$SLN_FILE" .sln)
echo -e "${GREEN}✅ Projeto detectado: ${PROJECT_NAME}${NC}"
echo -e "${BLUE}   Arquivo: ${SLN_FILE}${NC}"
echo ""

echo -e "${BLUE}📋 Verificando ambiente...${NC}"
echo ""

# Função para verificar comando
check_command() {
    if command -v $1 &> /dev/null; then
        echo -e "${GREEN}✅ $1 instalado${NC}"
        return 0
    else
        echo -e "${YELLOW}⚠️  $1 não encontrado${NC}"
        return 1
    fi
}

# Verificar dependências
check_command git
check_command dotnet
check_command gh 2>/dev/null || echo -e "${YELLOW}⚠️  GitHub CLI não instalado (opcional)${NC}"

echo ""
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}🔧 Passo 1: Instalar Git Hooks${NC}"
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

if [ -f ".git/hooks/pre-commit" ]; then
    echo -e "${YELLOW}⚠️  Hook pre-commit já existe${NC}"
    read -p "Deseja reinstalar? (s/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Ss]$ ]]; then
        chmod +x .git/hooks/pre-commit
        echo -e "${GREEN}✅ Hook reinstalado${NC}"
    fi
else
    if [ -f "install-hooks.sh" ]; then
        chmod +x install-hooks.sh
        ./install-hooks.sh
    else
        chmod +x .git/hooks/pre-commit 2>/dev/null
        echo -e "${GREEN}✅ Hook configurado${NC}"
    fi
fi

echo ""
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}🔧 Passo 2: Verificar Branches${NC}"
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

CURRENT_BRANCH=$(git branch --show-current)
echo -e "${BLUE}📍 Branch atual: ${NC}${CURRENT_BRANCH}"

# Verificar se developer existe
if git show-ref --verify --quiet refs/heads/developer; then
    echo -e "${GREEN}✅ Branch 'developer' existe${NC}"
else
    echo -e "${YELLOW}⚠️  Branch 'developer' não existe${NC}"
    read -p "Deseja criar? (s/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Ss]$ ]]; then
        git checkout -b developer 2>/dev/null || git checkout developer
        git push origin developer
        echo -e "${GREEN}✅ Branch 'developer' criada${NC}"
    fi
fi

echo ""
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}🔧 Passo 3: Testar Configuração${NC}"
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

echo -e "${BLUE}Verificando se projeto compila...${NC}"
if dotnet build "$SLN_FILE" > /dev/null 2>&1; then
    echo -e "${GREEN}✅ Build bem-sucedido${NC}"
else
    echo -e "${YELLOW}⚠️  Build falhou - verifique o projeto${NC}"
fi

echo -e "${BLUE}Verificando testes...${NC}"
if dotnet test "$SLN_FILE" > /dev/null 2>&1; then
    echo -e "${GREEN}✅ Testes passando${NC}"
else
    echo -e "${YELLOW}⚠️  Alguns testes falharam${NC}"
fi

echo ""
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}🔧 Passo 4: Verificar GitHub Actions${NC}"
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

if [ -f ".github/workflows/validate-feature.yml" ]; then
    echo -e "${GREEN}✅ validate-feature.yml encontrado${NC}"
else
    echo -e "${RED}❌ validate-feature.yml não encontrado${NC}"
fi

if [ -f ".github/workflows/create-release.yml" ]; then
    echo -e "${GREEN}✅ create-release.yml encontrado${NC}"
else
    echo -e "${RED}❌ create-release.yml não encontrado${NC}"
fi

echo ""
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${GREEN}✅ Setup Concluído!${NC}"
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""
echo -e "${MAGENTA}📚 Próximos Passos:${NC}"
echo ""
echo -e "${YELLOW}1. Configure Branch Protection no GitHub:${NC}"
echo -e "   ${CYAN}https://github.com/{owner}/{repo}/settings/branches${NC}"
echo ""
echo -e "${YELLOW}2. Crie uma feature branch:${NC}"
echo -e "   ${GREEN}git checkout -b feature/minha-funcionalidade${NC}"
echo ""
echo -e "${YELLOW}3. Faça commits e push:${NC}"
echo -e "   ${GREEN}git commit -m \"feat: nova funcionalidade\"${NC}"
echo -e "   ${GREEN}git push origin feature/minha-funcionalidade${NC}"
echo ""
echo -e "${YELLOW}4. Abra um Pull Request no GitHub${NC}"
echo ""
echo -e "${MAGENTA}📖 Documentação:${NC}"
echo -e "   ${CYAN}CI-CD-WORKFLOW.md${NC} - Guia completo"
echo -e "   ${CYAN}SETUP-SUMMARY.md${NC}  - Resumo dos arquivos"
echo ""
echo -e "${MAGENTA}🆘 Ajuda:${NC}"
echo -e "   ${CYAN}Leia o CI-CD-WORKFLOW.md para detalhes${NC}"
echo ""
echo -e "${CYAN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""
echo -e "${GREEN}🎉 Tudo pronto! Boa codificação! 🚀${NC}"
echo ""
