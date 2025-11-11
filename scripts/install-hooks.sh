#!/bin/bash

# =============================================================================
# 🔧 INSTALADOR DE HOOKS GIT
# =============================================================================
# 
# Este script instala e configura os hooks Git necessários para o projeto.
# 
# Uso: ./install-hooks.sh
# =============================================================================

# Cores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}🔧 Instalador de Hooks Git${NC}"
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

# Verifica se está em um repositório Git
if [ ! -d ".git" ]; then
    echo -e "${RED}❌ Erro: Este diretório não é um repositório Git!${NC}"
    echo -e "${YELLOW}   Execute este script na raiz do repositório.${NC}"
    exit 1
fi

# Cria diretório de hooks se não existir
mkdir -p .git/hooks

# Define o arquivo de origem do hook
HOOK_SOURCE=".git/hooks/pre-commit"

# Verifica se o hook já existe
if [ -f "$HOOK_SOURCE" ]; then
    echo -e "${YELLOW}⚠️  Hook pre-commit já existe!${NC}"
    read -p "Deseja sobrescrever? (s/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Ss]$ ]]; then
        echo -e "${BLUE}ℹ️  Instalação cancelada.${NC}"
        exit 0
    fi
    echo -e "${BLUE}Sobrescrevendo hook existente...${NC}"
fi

# Torna o hook executável
chmod +x "$HOOK_SOURCE"

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✅ Hook pre-commit instalado com sucesso!${NC}"
    echo ""
    echo -e "${BLUE}📍 Localização:${NC} $HOOK_SOURCE"
    echo -e "${BLUE}🔒 Permissões:${NC} Executável"
    echo ""
    echo -e "${GREEN}O hook agora validará:${NC}"
    echo -e "   ${GREEN}✓${NC} Nomenclatura de branches (feature/* ou hotfix/*)"
    echo -e "   ${GREEN}✓${NC} Bloqueio de commits diretos em branches protegidas"
    echo -e "   ${GREEN}✓${NC} Validações antes de cada commit"
    echo ""
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
else
    echo -e "${RED}❌ Erro ao configurar permissões do hook!${NC}"
    exit 1
fi

# Para Windows (Git Bash), verifica se precisa ajustar line endings
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    echo -e "${BLUE}ℹ️  Sistema Windows detectado${NC}"
    echo -e "${BLUE}   Ajustando line endings...${NC}"
    
    # Converte CRLF para LF se necessário
    dos2unix "$HOOK_SOURCE" 2>/dev/null || sed -i 's/\r$//' "$HOOK_SOURCE"
    
    echo -e "${GREEN}✅ Line endings ajustados${NC}"
fi

echo ""
echo -e "${GREEN}🎉 Instalação concluída!${NC}"
echo ""
echo -e "${YELLOW}💡 Dica:${NC} Execute ${GREEN}git commit${NC} para testar o hook."
echo ""
