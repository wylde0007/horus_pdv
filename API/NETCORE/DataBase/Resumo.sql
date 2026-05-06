IF DB_ID(N'HorusPdv') IS NULL
BEGIN
    CREATE DATABASE HorusPdv;
END;
GO

USE HorusPdv;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'Fornecedores', N'U') IS NULL
BEGIN
    CREATE TABLE Fornecedores
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_Fornecedores PRIMARY KEY,
        CompanyName NVARCHAR(180) NOT NULL CONSTRAINT DF_Fornecedores_CompanyName DEFAULT N'',
        FantasyName NVARCHAR(180) NOT NULL CONSTRAINT DF_Fornecedores_FantasyName DEFAULT N'',
        Cnpj NVARCHAR(30) NOT NULL CONSTRAINT DF_Fornecedores_Cnpj DEFAULT N'',
        Cep NVARCHAR(20) NOT NULL CONSTRAINT DF_Fornecedores_Cep DEFAULT N'',
        City NVARCHAR(120) NOT NULL CONSTRAINT DF_Fornecedores_City DEFAULT N'',
        State NVARCHAR(2) NOT NULL CONSTRAINT DF_Fornecedores_State DEFAULT N'',
        Address NVARCHAR(180) NOT NULL CONSTRAINT DF_Fornecedores_Address DEFAULT N'',
        Neighborhood NVARCHAR(120) NOT NULL CONSTRAINT DF_Fornecedores_Neighborhood DEFAULT N'',
        StreetComplement NVARCHAR(180) NOT NULL CONSTRAINT DF_Fornecedores_StreetComplement DEFAULT N'',
        Number NVARCHAR(30) NOT NULL CONSTRAINT DF_Fornecedores_Number DEFAULT N'',
        ReferencePoint NVARCHAR(180) NOT NULL CONSTRAINT DF_Fornecedores_ReferencePoint DEFAULT N'',
        Telephone NVARCHAR(30) NOT NULL CONSTRAINT DF_Fornecedores_Telephone DEFAULT N'',
        Cellphone NVARCHAR(30) NOT NULL CONSTRAINT DF_Fornecedores_Cellphone DEFAULT N'',
        Email NVARCHAR(180) NOT NULL CONSTRAINT DF_Fornecedores_Email DEFAULT N'',
        CONSTRAINT UQ_Fornecedores_Cnpj UNIQUE (Cnpj)
    );
END;

IF OBJECT_ID(N'Produtos', N'U') IS NULL
BEGIN
    CREATE TABLE Produtos
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_Produtos PRIMARY KEY,
        ProductImageUrl NVARCHAR(500) NOT NULL CONSTRAINT DF_Produtos_ProductImageUrl DEFAULT N'',
        ProductImageName NVARCHAR(180) NOT NULL CONSTRAINT DF_Produtos_ProductImageName DEFAULT N'',
        ProductName NVARCHAR(180) NOT NULL CONSTRAINT DF_Produtos_ProductName DEFAULT N'',
        ProductCode NVARCHAR(80) NOT NULL,
        ProductSupplier NVARCHAR(180) NOT NULL CONSTRAINT DF_Produtos_ProductSupplier DEFAULT N'',
        SupplierId NVARCHAR(40) NULL,
        ProductDescription NVARCHAR(500) NOT NULL CONSTRAINT DF_Produtos_ProductDescription DEFAULT N'',
        ProductQnt NVARCHAR(30) NOT NULL CONSTRAINT DF_Produtos_ProductQnt DEFAULT N'0',
        ProductUnitPrice NVARCHAR(30) NOT NULL CONSTRAINT DF_Produtos_ProductUnitPrice DEFAULT N'0,00',
        ProductSalePrice NVARCHAR(30) NOT NULL CONSTRAINT DF_Produtos_ProductSalePrice DEFAULT N'0,00',
        TotalPriceOnProduct NVARCHAR(30) NOT NULL CONSTRAINT DF_Produtos_TotalPriceOnProduct DEFAULT N'0,00',
        CONSTRAINT UQ_Produtos_ProductCode UNIQUE (ProductCode),
        CONSTRAINT FK_Produtos_Fornecedores FOREIGN KEY (SupplierId) REFERENCES Fornecedores (Id) ON DELETE SET NULL
    );
END;

IF OBJECT_ID(N'Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE Clientes
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
        CustomerName NVARCHAR(180) NOT NULL CONSTRAINT DF_Clientes_CustomerName DEFAULT N'',
        Document NVARCHAR(30) NOT NULL CONSTRAINT DF_Clientes_Document DEFAULT N'',
        BirthDate NVARCHAR(20) NOT NULL CONSTRAINT DF_Clientes_BirthDate DEFAULT N'',
        Age NVARCHAR(10) NOT NULL CONSTRAINT DF_Clientes_Age DEFAULT N'',
        Cep NVARCHAR(20) NOT NULL CONSTRAINT DF_Clientes_Cep DEFAULT N'',
        City NVARCHAR(120) NOT NULL CONSTRAINT DF_Clientes_City DEFAULT N'',
        State NVARCHAR(2) NOT NULL CONSTRAINT DF_Clientes_State DEFAULT N'',
        Address NVARCHAR(180) NOT NULL CONSTRAINT DF_Clientes_Address DEFAULT N'',
        Neighborhood NVARCHAR(120) NOT NULL CONSTRAINT DF_Clientes_Neighborhood DEFAULT N'',
        StreetComplement NVARCHAR(180) NOT NULL CONSTRAINT DF_Clientes_StreetComplement DEFAULT N'',
        Number NVARCHAR(30) NOT NULL CONSTRAINT DF_Clientes_Number DEFAULT N'',
        ReferencePoint NVARCHAR(180) NOT NULL CONSTRAINT DF_Clientes_ReferencePoint DEFAULT N'',
        Telephone NVARCHAR(30) NOT NULL CONSTRAINT DF_Clientes_Telephone DEFAULT N'',
        Cellphone NVARCHAR(30) NOT NULL CONSTRAINT DF_Clientes_Cellphone DEFAULT N'',
        Email NVARCHAR(180) NOT NULL CONSTRAINT DF_Clientes_Email DEFAULT N'',
        CONSTRAINT UQ_Clientes_Document UNIQUE (Document)
    );
END;

IF OBJECT_ID(N'Empresas', N'U') IS NULL
BEGIN
    CREATE TABLE Empresas
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_Empresas PRIMARY KEY,
        FantasyName NVARCHAR(180) NOT NULL CONSTRAINT DF_Empresas_FantasyName DEFAULT N'',
        CorporateName NVARCHAR(180) NOT NULL CONSTRAINT DF_Empresas_CorporateName DEFAULT N'',
        Cnpj NVARCHAR(30) NOT NULL CONSTRAINT DF_Empresas_Cnpj DEFAULT N'',
        StateRegistration NVARCHAR(60) NOT NULL CONSTRAINT DF_Empresas_StateRegistration DEFAULT N'',
        Website NVARCHAR(250) NOT NULL CONSTRAINT DF_Empresas_Website DEFAULT N'',
        Email NVARCHAR(180) NOT NULL CONSTRAINT DF_Empresas_Email DEFAULT N'',
        SacPhone NVARCHAR(30) NOT NULL CONSTRAINT DF_Empresas_SacPhone DEFAULT N'',
        Phone NVARCHAR(30) NOT NULL CONSTRAINT DF_Empresas_Phone DEFAULT N'',
        Mobile NVARCHAR(30) NOT NULL CONSTRAINT DF_Empresas_Mobile DEFAULT N'',
        Cep NVARCHAR(20) NOT NULL CONSTRAINT DF_Empresas_Cep DEFAULT N'',
        Address NVARCHAR(180) NOT NULL CONSTRAINT DF_Empresas_Address DEFAULT N'',
        Number NVARCHAR(30) NOT NULL CONSTRAINT DF_Empresas_Number DEFAULT N'',
        Neighborhood NVARCHAR(120) NOT NULL CONSTRAINT DF_Empresas_Neighborhood DEFAULT N'',
        City NVARCHAR(120) NOT NULL CONSTRAINT DF_Empresas_City DEFAULT N'',
        Uf NVARCHAR(2) NOT NULL CONSTRAINT DF_Empresas_Uf DEFAULT N'',
        Complement NVARCHAR(180) NOT NULL CONSTRAINT DF_Empresas_Complement DEFAULT N''
    );
END;

IF OBJECT_ID(N'Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE Usuarios
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
        Cpf NVARCHAR(30) NOT NULL,
        Name NVARCHAR(180) NOT NULL,
        Email NVARCHAR(180) NOT NULL,
        Phone NVARCHAR(30) NOT NULL CONSTRAINT DF_Usuarios_Phone DEFAULT N'',
        Role NVARCHAR(60) NOT NULL CONSTRAINT DF_Usuarios_Role DEFAULT N'atendente',
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Usuarios_Status DEFAULT N'ativo',
        CreatedAt NVARCHAR(30) NOT NULL CONSTRAINT DF_Usuarios_CreatedAt DEFAULT CONVERT(NVARCHAR(30), SYSUTCDATETIME(), 23),
        LastLoginAt NVARCHAR(60) NOT NULL CONSTRAINT DF_Usuarios_LastLoginAt DEFAULT N'-',
        PasswordHash NVARCHAR(300) NOT NULL,
        MustChangePassword BIT NOT NULL CONSTRAINT DF_Usuarios_MustChangePassword DEFAULT 1,
        CONSTRAINT UQ_Usuarios_Cpf UNIQUE (Cpf),
        CONSTRAINT UQ_Usuarios_Email UNIQUE (Email)
    );
END;

IF OBJECT_ID(N'Sessoes', N'U') IS NULL
BEGIN
    CREATE TABLE Sessoes
    (
        Id NVARCHAR(80) NOT NULL CONSTRAINT PK_Sessoes PRIMARY KEY,
        UserId NVARCHAR(40) NOT NULL,
        Device NVARCHAR(120) NOT NULL CONSTRAINT DF_Sessoes_Device DEFAULT N'',
        Location NVARCHAR(150) NOT NULL CONSTRAINT DF_Sessoes_Location DEFAULT N'',
        Ip NVARCHAR(64) NOT NULL CONSTRAINT DF_Sessoes_Ip DEFAULT N'',
        LastActive NVARCHAR(80) NOT NULL CONSTRAINT DF_Sessoes_LastActive DEFAULT N'Agora mesmo',
        Platform NVARCHAR(20) NOT NULL CONSTRAINT DF_Sessoes_Platform DEFAULT N'desktop',
        CreatedAt DATETIMEOFFSET NOT NULL CONSTRAINT DF_Sessoes_CreatedAt DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_Sessoes_Usuarios FOREIGN KEY (UserId) REFERENCES Usuarios (Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_Sessoes_UserId ON Sessoes (UserId);
END;

IF OBJECT_ID(N'PasswordResetTokens', N'U') IS NULL
BEGIN
    CREATE TABLE PasswordResetTokens
    (
        Token NVARCHAR(120) NOT NULL CONSTRAINT PK_PasswordResetTokens PRIMARY KEY,
        UserId NVARCHAR(40) NOT NULL,
        Email NVARCHAR(180) NOT NULL,
        CreatedAt DATETIMEOFFSET NOT NULL,
        ExpiresAt DATETIMEOFFSET NOT NULL,
        ConsumedAt DATETIMEOFFSET NULL,
        CONSTRAINT FK_PasswordResetTokens_Usuarios FOREIGN KEY (UserId) REFERENCES Usuarios (Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_PasswordResetTokens_UserId ON PasswordResetTokens (UserId);
END;

IF OBJECT_ID(N'CaixaSessoes', N'U') IS NULL
BEGIN
    CREATE TABLE CaixaSessoes
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_CaixaSessoes PRIMARY KEY,
        OpenedAt DATETIMEOFFSET NOT NULL,
        ClosedAt DATETIMEOFFSET NULL,
        OpeningAmount NVARCHAR(30) NOT NULL CONSTRAINT DF_CaixaSessoes_OpeningAmount DEFAULT N'0,00',
        ClosingAmount NVARCHAR(30) NOT NULL CONSTRAINT DF_CaixaSessoes_ClosingAmount DEFAULT N'0,00',
        OperatorId NVARCHAR(40) NOT NULL,
        OperatorName NVARCHAR(180) NOT NULL CONSTRAINT DF_CaixaSessoes_OperatorName DEFAULT N'',
        ClosedById NVARCHAR(40) NOT NULL CONSTRAINT DF_CaixaSessoes_ClosedById DEFAULT N'',
        ClosedByName NVARCHAR(180) NOT NULL CONSTRAINT DF_CaixaSessoes_ClosedByName DEFAULT N'',
        Note NVARCHAR(500) NOT NULL CONSTRAINT DF_CaixaSessoes_Note DEFAULT N'',
        CONSTRAINT FK_CaixaSessoes_Usuarios FOREIGN KEY (OperatorId) REFERENCES Usuarios (Id)
    );
    CREATE INDEX IX_CaixaSessoes_ClosedAt ON CaixaSessoes (ClosedAt);
END;

IF OBJECT_ID(N'Vendas', N'U') IS NULL
BEGIN
    CREATE TABLE Vendas
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_Vendas PRIMARY KEY,
        SaleNumber NVARCHAR(30) NOT NULL,
        CustomerName NVARCHAR(180) NOT NULL CONSTRAINT DF_Vendas_CustomerName DEFAULT N'Consumidor',
        CustomerCpf NVARCHAR(30) NOT NULL CONSTRAINT DF_Vendas_CustomerCpf DEFAULT N'-',
        SaleDate DATETIMEOFFSET NOT NULL CONSTRAINT DF_Vendas_SaleDate DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT UQ_Vendas_SaleNumber UNIQUE (SaleNumber)
    );
END;

IF OBJECT_ID(N'VendaItens', N'U') IS NULL
BEGIN
    CREATE TABLE VendaItens
    (
        Id NVARCHAR(40) NOT NULL CONSTRAINT PK_VendaItens PRIMARY KEY,
        VendaId NVARCHAR(40) NOT NULL,
        ProductCode NVARCHAR(80) NOT NULL,
        ProductName NVARCHAR(180) NOT NULL,
        Quantity INT NOT NULL,
        CONSTRAINT FK_VendaItens_Vendas FOREIGN KEY (VendaId) REFERENCES Vendas (Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_VendaItens_VendaId ON VendaItens (VendaId);
END;

IF OBJECT_ID(N'ModulosMercado', N'U') IS NULL
BEGIN
    CREATE TABLE ModulosMercado
    (
        Id NVARCHAR(80) NOT NULL CONSTRAINT PK_ModulosMercado PRIMARY KEY,
        Title NVARCHAR(180) NOT NULL
    );
END;

IF OBJECT_ID(N'ModuloMercadoRegistros', N'U') IS NULL
BEGIN
    CREATE TABLE ModuloMercadoRegistros
    (
        Id NVARCHAR(100) NOT NULL CONSTRAINT PK_ModuloMercadoRegistros PRIMARY KEY,
        ModuleId NVARCHAR(80) NOT NULL,
        Title NVARCHAR(180) NOT NULL,
        Description NVARCHAR(500) NOT NULL CONSTRAINT DF_ModuloMercadoRegistros_Description DEFAULT N'',
        Status NVARCHAR(80) NOT NULL CONSTRAINT DF_ModuloMercadoRegistros_Status DEFAULT N'',
        Amount NVARCHAR(40) NOT NULL CONSTRAINT DF_ModuloMercadoRegistros_Amount DEFAULT N'',
        Meta NVARCHAR(180) NOT NULL CONSTRAINT DF_ModuloMercadoRegistros_Meta DEFAULT N'',
        CONSTRAINT FK_ModuloMercadoRegistros_ModulosMercado FOREIGN KEY (ModuleId) REFERENCES ModulosMercado (Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_ModuloMercadoRegistros_ModuleId ON ModuloMercadoRegistros (ModuleId);
END;

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id = N'usr-001')
BEGIN
    INSERT INTO Usuarios (Id, Cpf, Name, Email, Phone, Role, Status, CreatedAt, LastLoginAt, PasswordHash, MustChangePassword)
    VALUES
        (N'usr-001', N'123.456.789-01', N'Flávio Oliveira', N'flavio@hpdv.com.br', N'(11) 98888-1111', N'administrador', N'ativo', N'2026-02-10', N'-', N'100000.kop8te8YGY/xSBBtEPR1yA==.iff4Jd546alYO+CLav8GVyX+p0cquoJK6fEpl6upHZc=', 0),
        (N'usr-002', N'234.567.890-12', N'Maria Santos', N'maria@hpdv.com.br', N'(11) 97777-2222', N'gerente', N'ativo', N'2026-02-15', N'-', N'100000.LPJzIY8cWIKmbMrdOWghJA==./g/K2agBRS8vR+qIfbe7zrTV3528etwqkF+PsYH8ONg=', 0),
        (N'usr-003', N'345.678.901-23', N'João Costa', N'joao@hpdv.com.br', N'(11) 96666-3333', N'atendente', N'inativo', N'2026-03-01', N'-', N'100000.7+juWboKfCxabxeBX30K9A==.XdVmx1FOROPd0vGy/IeNrjXMwhFm+j0PN7PxLw4pjNM=', 1);
END;

IF NOT EXISTS (SELECT 1 FROM Fornecedores WHERE Id = N'fr-001')
BEGIN
    INSERT INTO Fornecedores (Id, CompanyName, FantasyName, Cnpj, Cep, City, State, Address, Neighborhood, Number, Telephone, Cellphone, Email)
    VALUES
        (N'fr-001', N'Distribuidora Alfa LTDA', N'Distribuidora Alfa', N'12.345.678/0001-95', N'01001-000', N'São Paulo', N'SP', N'Praça da Sé', N'Sé', N'100', N'(11) 3322-1100', N'(11) 98888-3344', N'comercial@alfa.com.br'),
        (N'fr-002', N'Atacado Vitória LTDA', N'Atacado Vitória', N'98.765.432/0001-10', N'20040-020', N'Rio de Janeiro', N'RJ', N'Rua da Quitanda', N'Centro', N'55', N'(21) 2222-1000', N'(21) 97777-2211', N'vendas@vitoria.com.br');
END;

IF NOT EXISTS (SELECT 1 FROM Produtos WHERE Id = N'pr-001')
BEGIN
    INSERT INTO Produtos (Id, ProductName, ProductCode, ProductSupplier, SupplierId, ProductDescription, ProductQnt, ProductUnitPrice, ProductSalePrice, TotalPriceOnProduct)
    VALUES
        (N'pr-001', N'Café Tradicional 500g', N'CAF500', N'Distribuidora Alfa', N'fr-001', N'Café torrado e moído 500g', N'120', N'14,90', N'18,90', N'1.788,00'),
        (N'pr-002', N'Óleo de Soja', N'OLE900', N'Atacado Vitória', N'fr-002', N'Óleo de soja 900ml', N'56', N'3,29', N'6,99', N'184,24'),
        (N'pr-003', N'Erva Chimarrão', N'ERV500', N'Distribuidora Alfa', N'fr-001', N'Erva mate para chimarrão 500g', N'24', N'9,80', N'15,00', N'235,20');
END;

IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Id = N'cl-001')
BEGIN
    INSERT INTO Clientes (Id, CustomerName, Document, BirthDate, Age, Cep, City, State, Address, Neighborhood, Number, Telephone, Cellphone, Email)
    VALUES (N'cl-001', N'Ana Martins', N'123.456.789-09', N'16/10/1991', N'34', N'06010-000', N'Osasco', N'SP', N'Rua Primitiva Vianco', N'Centro', N'100', N'(11) 3681-1000', N'(11) 99888-1122', N'ana.martins@email.com');
END;

IF NOT EXISTS (SELECT 1 FROM Empresas WHERE Id = N'empresa-principal')
BEGIN
    INSERT INTO Empresas (Id, FantasyName, CorporateName, Cnpj, StateRegistration, Website, Email, SacPhone, Phone, Mobile, Cep, Address, Number, Neighborhood, City, Uf, Complement)
    VALUES (N'empresa-principal', N'Festa & Fantasia', N'Festa & Fantasia Comercio LTDA', N'06.332.765/0001-05', N'123.456.789.110', N'https://www.horuspdv.com.br', N'contato@hpdv.com.br', N'(11) 3000-1000', N'(11) 3681-1000', N'(11) 98888-1000', N'06010-000', N'Rua Primitiva Vianco', N'100', N'Centro', N'Osasco', N'SP', N'Sala 12');
END;

IF NOT EXISTS (SELECT 1 FROM CaixaSessoes WHERE Id = N'cx-001')
BEGIN
    INSERT INTO CaixaSessoes (Id, OpenedAt, ClosedAt, OpeningAmount, ClosingAmount, OperatorId, OperatorName, ClosedById, ClosedByName, Note)
    VALUES (N'cx-001', DATEADD(HOUR, 8, CONVERT(DATETIMEOFFSET, CONVERT(DATE, DATEADD(DAY, -1, SYSDATETIMEOFFSET())))), DATEADD(HOUR, 18, CONVERT(DATETIMEOFFSET, CONVERT(DATE, DATEADD(DAY, -1, SYSDATETIMEOFFSET())))), N'250,00', N'3.418,90', N'usr-001', N'Flávio Oliveira', N'usr-001', N'Flávio Oliveira', N'Fechamento do período anterior.');
END;

IF NOT EXISTS (SELECT 1 FROM Vendas WHERE Id = N'sale-15039')
BEGIN
    INSERT INTO Vendas (Id, SaleNumber, CustomerName, CustomerCpf, SaleDate)
    VALUES
        (N'sale-15039', N'15039', N'Ana Martins', N'123.456.789-09', CONVERT(DATETIMEOFFSET, N'2026-03-21T14:12:08+00:00')),
        (N'sale-15038', N'15038', N'Lucas Souza', N'427.632.180-01', CONVERT(DATETIMEOFFSET, N'2026-03-21T13:42:11+00:00')),
        (N'sale-15037', N'15037', N'Beatriz Lima', N'064.822.390-16', CONVERT(DATETIMEOFFSET, N'2026-03-21T12:55:46+00:00'));

    INSERT INTO VendaItens (Id, VendaId, ProductCode, ProductName, Quantity)
    VALUES
        (N'sale-15039-item-001', N'sale-15039', N'CAF500', N'Café Tradicional 500g', 3),
        (N'sale-15038-item-001', N'sale-15038', N'ACH400', N'Achocolatado 400g', 1),
        (N'sale-15037-item-001', N'sale-15037', N'ARR5KG', N'Arroz Tipo 1 5kg', 2);
END;

IF NOT EXISTS (SELECT 1 FROM ModulosMercado WHERE Id = N'estoque')
BEGIN
    INSERT INTO ModulosMercado (Id, Title)
    VALUES
        (N'fiscal', N'Fiscal NFC-e / NF-e'),
        (N'pagamentos', N'Pagamentos Integrados'),
        (N'estoque', N'Estoque e Inventário'),
        (N'caixa', N'Abertura e Fechamento de Caixa'),
        (N'compras', N'Compras e Reposição'),
        (N'devolucoes', N'Trocas e Devoluções'),
        (N'crm-fidelidade', N'CRM e Fidelidade'),
        (N'omnichannel', N'Omnichannel e Integrações');

    INSERT INTO ModuloMercadoRegistros (Id, ModuleId, Title, Description, Status, Amount, Meta)
    SELECT CONCAT(Id, N'-001'), Id, CONCAT(Title, N' - Registro 001'), N'Registro principal do módulo.', N'Ativo', N'R$ 184,90', N'Sincronizado agora'
    FROM ModulosMercado
    WHERE Id IN (N'fiscal', N'pagamentos', N'estoque', N'caixa', N'compras', N'devolucoes', N'crm-fidelidade', N'omnichannel');

    INSERT INTO ModuloMercadoRegistros (Id, ModuleId, Title, Description, Status, Amount, Meta)
    SELECT CONCAT(Id, N'-002'), Id, CONCAT(Title, N' - Registro 002'), N'Item aguardando validação operacional.', N'Pendente', N'R$ 59,80', N'Prioridade média'
    FROM ModulosMercado
    WHERE Id IN (N'fiscal', N'pagamentos', N'estoque', N'caixa', N'compras', N'devolucoes', N'crm-fidelidade', N'omnichannel');

    INSERT INTO ModuloMercadoRegistros (Id, ModuleId, Title, Description, Status, Amount, Meta)
    SELECT CONCAT(Id, N'-003'), Id, CONCAT(Title, N' - Registro 003'), N'Evento de auditoria e acompanhamento.', N'Auditado', N'R$ 1.240,00', N'Responsável: Administrador'
    FROM ModulosMercado
    WHERE Id IN (N'fiscal', N'pagamentos', N'estoque', N'caixa', N'compras', N'devolucoes', N'crm-fidelidade', N'omnichannel');
END;
