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

## E-mail SMTP

O envio de e-mail usa Outlook/Office 365 via SMTP. O remetente configurado é:

<pre><code class="language-text">naoresponderhoruspdv@outlook.com
</code></pre>

Não coloque a senha de app em arquivo versionado. Configure localmente com User Secrets:

<pre><code class="language-bash">cd API/NETCORE
dotnet user-secrets set "Email:Enabled" "true"
dotnet user-secrets set "Email:Password" "SUA_SENHA_DE_APP_AQUI"
</code></pre>

Configurações padrão em `appsettings.json`:

<pre><code class="language-json">"Email": {
  "Host": "smtp.office365.com",
  "Port": 587,
  "User": "naoresponderhoruspdv@outlook.com",
  "FromEmail": "naoresponderhoruspdv@outlook.com",
  "FrontendBaseUrl": "http://localhost:5173"
}</code></pre>

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
