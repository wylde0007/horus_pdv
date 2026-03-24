# API NETCORE

## Objetivo
Esta pasta é reservada para a implementação da API em .NET (ASP.NET Core) do projeto Hórus PDV.

## Status
Em desenvolvimento.

## Escopo sugerido
- Autenticação e autorização.
- Cadastro e consulta de produtos.
- Gestão de clientes.
- Vendas, itens de venda e fechamento.
- Relatórios e indicadores.

## Estrutura recomendada
- `src/`: código-fonte da solução.
- `tests/`: testes automatizados.
- `docs/`: documentação técnica e contratos.
- `.env.example`: variáveis de ambiente de exemplo.

## Como iniciar (quando a API existir)
1. Restaurar dependências.
2. Configurar variáveis de ambiente.
3. Aplicar migrations (se houver banco relacional).
4. Subir a aplicação em ambiente local.

## Boas práticas
- Seguir Clean Code e separação por camadas (Domain, Application, Infrastructure, API).
- Versionar contratos e endpoints.
- Garantir cobertura mínima de testes para regras críticas.
