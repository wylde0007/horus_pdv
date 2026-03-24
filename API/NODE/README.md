# API NODE

## Objetivo
Esta pasta é reservada para a implementação da API em Node.js do projeto Hórus PDV.

## Status
Em desenvolvimento.

## Escopo sugerido
- Autenticação e sessão de usuário.
- Endpoints de produtos, estoque e preços.
- Operação de venda (abertura, itens, pagamento e cancelamento).
- Integrações e webhooks futuros.

## Estrutura recomendada
- `src/`: código principal.
- `src/modules/`: módulos de domínio.
- `src/shared/`: utilitários e infraestrutura comum.
- `tests/`: testes unitários e integração.
- `.env.example`: variáveis de ambiente de exemplo.

## Como iniciar (quando a API existir)
1. Instalar dependências (`npm install`).
2. Copiar `.env.example` para `.env` e configurar.
3. Executar migrations/seeds (se necessário).
4. Rodar em modo desenvolvimento (`npm run dev`).

## Boas práticas
- Organizar por módulos de negócio.
- Validar entrada/saída em todas as rotas.
- Implementar logging estruturado e tratamento centralizado de erros.
