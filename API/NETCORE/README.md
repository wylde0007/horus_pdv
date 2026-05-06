# Hórus PDV API .NET

API ASP.NET Core do Hórus PDV.

## Stack

- .NET 8
- ASP.NET Core Web API
- Microsoft.Data.SqlClient
- SQL Server
- Swagger
- JWT

## Banco Local

Suba o SQL Server via Docker:

<pre><code class="language-bash">docker run -d \
  --name sqlserver \
  -e ACCEPT_EULA=Y \
  -e SA_PASSWORD='Senha@12345' \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
</code></pre>

Connection string padrão:

<pre><code class="language-json">"ConnectionStrings": {
  "HorusPdv": "Server=localhost,1433;Database=HorusPdv;User Id=sa;Password=Senha@12345;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=True"
}</code></pre>

Ao iniciar, a API executa `DataBase/Resumo.sql` para criar o banco `HorusPdv`, criar tabelas/relacionamentos e inserir os dados iniciais quando ainda não existirem.

Script principal:

<pre><code class="language-text">DataBase/Resumo.sql
</code></pre>

## Rodando

<pre><code class="language-bash">dotnet restore
dotnet build
dotnet run --urls http://localhost:5260
</code></pre>

Swagger:

<pre><code class="language-text">http://localhost:5260/swagger
</code></pre>

## Tabelas

- `Usuarios`
- `Sessoes`
- `PasswordResetTokens`
- `Produtos`
- `Fornecedores`
- `Clientes`
- `Empresas`
- `CaixaSessoes`
- `Vendas`
- `VendaItens`
- `ModulosMercado`
- `ModuloMercadoRegistros`

## Organização dos Repositórios

- `Repositories/DataAccess`: classes `*AD`, usadas para transportar dados vindos do banco.
- `Repositories/DatabaseAccess`: classes `*AB`, responsáveis por SQL e acesso ao banco.
- `Services`: regras de negócio e orquestração, sem `SqlCommand`.
- `Controllers`: HTTP/API, sem SQL direto.

## Próximo Passo Técnico

Criar scripts SQL incrementais para cada evolução de schema antes de produção.
