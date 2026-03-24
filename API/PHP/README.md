# API PHP

## Objetivo
Esta pasta é reservada para a implementação da API em PHP do projeto Hórus PDV.

## Status
Em desenvolvimento.

## Escopo sugerido
- Gestão de usuários e permissões.
- Cadastro de entidades de negócio (clientes, produtos, fornecedores).
- Fluxo de vendas e emissão de comprovantes.
- Relatórios operacionais e financeiros.

## Estrutura recomendada
- `public/`: ponto de entrada da aplicação.
- `src/`: código da aplicação.
- `config/`: configurações por ambiente.
- `database/`: migrations e seeds.
- `tests/`: testes automatizados.
- `.env.example`: variáveis de ambiente de exemplo.

## Como iniciar (quando a API existir)
1. Instalar dependências com Composer.
2. Configurar arquivo `.env`.
3. Preparar banco de dados/migrations.
4. Executar servidor local.

## Boas práticas
- Separar regras de domínio de código de infraestrutura.
- Padronizar respostas HTTP e mensagens de erro.
- Documentar endpoints e autenticação.
