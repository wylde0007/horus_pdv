CREATE TABLE IF NOT EXISTS Fornecedores
(
    Id VARCHAR(40) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    CompanyName VARCHAR(180) NOT NULL DEFAULT '',
    FantasyName VARCHAR(180) NOT NULL DEFAULT '',
    Cnpj VARCHAR(30) NOT NULL DEFAULT '',
    Cep VARCHAR(20) NOT NULL DEFAULT '',
    City VARCHAR(120) NOT NULL DEFAULT '',
    State VARCHAR(2) NOT NULL DEFAULT '',
    Address VARCHAR(180) NOT NULL DEFAULT '',
    Neighborhood VARCHAR(120) NOT NULL DEFAULT '',
    StreetComplement VARCHAR(180) NOT NULL DEFAULT '',
    Number VARCHAR(30) NOT NULL DEFAULT '',
    ReferencePoint VARCHAR(180) NOT NULL DEFAULT '',
    Telephone VARCHAR(30) NOT NULL DEFAULT '',
    Cellphone VARCHAR(30) NOT NULL DEFAULT '',
    Email VARCHAR(180) NOT NULL DEFAULT '',
    CONSTRAINT UQ_Fornecedores_Company_Cnpj UNIQUE (CompanyId, Cnpj)
);

ALTER TABLE Fornecedores ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';

CREATE TABLE IF NOT EXISTS Produtos
(
    Id VARCHAR(40) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    ProductImageUrl VARCHAR(500) NOT NULL DEFAULT '',
    ProductImageName VARCHAR(180) NOT NULL DEFAULT '',
    ProductName VARCHAR(180) NOT NULL DEFAULT '',
    ProductCode VARCHAR(80) NOT NULL,
    ProductSupplier VARCHAR(180) NOT NULL DEFAULT '',
    SupplierId VARCHAR(40) NULL,
    ProductDescription VARCHAR(500) NOT NULL DEFAULT '',
    ProductQnt VARCHAR(30) NOT NULL DEFAULT '0',
    ProductUnitPrice VARCHAR(30) NOT NULL DEFAULT '0,00',
    ProductSalePrice VARCHAR(30) NOT NULL DEFAULT '0,00',
    TotalPriceOnProduct VARCHAR(30) NOT NULL DEFAULT '0,00',
    CONSTRAINT UQ_Produtos_Company_ProductCode UNIQUE (CompanyId, ProductCode),
    CONSTRAINT FK_Produtos_Fornecedores FOREIGN KEY (SupplierId) REFERENCES Fornecedores (Id) ON DELETE SET NULL
);

ALTER TABLE Produtos ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';

CREATE TABLE IF NOT EXISTS Clientes
(
    Id VARCHAR(40) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    CustomerName VARCHAR(180) NOT NULL DEFAULT '',
    Document VARCHAR(30) NOT NULL DEFAULT '',
    BirthDate VARCHAR(20) NOT NULL DEFAULT '',
    Age VARCHAR(10) NOT NULL DEFAULT '',
    Cep VARCHAR(20) NOT NULL DEFAULT '',
    City VARCHAR(120) NOT NULL DEFAULT '',
    State VARCHAR(2) NOT NULL DEFAULT '',
    Address VARCHAR(180) NOT NULL DEFAULT '',
    Neighborhood VARCHAR(120) NOT NULL DEFAULT '',
    StreetComplement VARCHAR(180) NOT NULL DEFAULT '',
    Number VARCHAR(30) NOT NULL DEFAULT '',
    ReferencePoint VARCHAR(180) NOT NULL DEFAULT '',
    Telephone VARCHAR(30) NOT NULL DEFAULT '',
    Cellphone VARCHAR(30) NOT NULL DEFAULT '',
    Email VARCHAR(180) NOT NULL DEFAULT '',
    CONSTRAINT UQ_Clientes_Company_Document UNIQUE (CompanyId, Document)
);

ALTER TABLE Clientes ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';

CREATE TABLE IF NOT EXISTS Empresas
(
    Id VARCHAR(40) PRIMARY KEY,
    FantasyName VARCHAR(180) NOT NULL DEFAULT '',
    CorporateName VARCHAR(180) NOT NULL DEFAULT '',
    Cnpj VARCHAR(30) NOT NULL DEFAULT '',
    StateRegistration VARCHAR(60) NOT NULL DEFAULT '',
    Website VARCHAR(250) NOT NULL DEFAULT '',
    Email VARCHAR(180) NOT NULL DEFAULT '',
    SacPhone VARCHAR(30) NOT NULL DEFAULT '',
    Phone VARCHAR(30) NOT NULL DEFAULT '',
    Mobile VARCHAR(30) NOT NULL DEFAULT '',
    Cep VARCHAR(20) NOT NULL DEFAULT '',
    Address VARCHAR(180) NOT NULL DEFAULT '',
    Number VARCHAR(30) NOT NULL DEFAULT '',
    Neighborhood VARCHAR(120) NOT NULL DEFAULT '',
    City VARCHAR(120) NOT NULL DEFAULT '',
    Uf VARCHAR(2) NOT NULL DEFAULT '',
    Complement VARCHAR(180) NOT NULL DEFAULT '',
    EmailSmtpEnabled BOOLEAN NOT NULL DEFAULT false,
    EmailSmtpHost VARCHAR(180) NOT NULL DEFAULT 'smtp-mail.outlook.com',
    EmailSmtpPort INT NOT NULL DEFAULT 587,
    EmailSmtpEnableSsl BOOLEAN NOT NULL DEFAULT true,
    EmailSmtpUser VARCHAR(180) NOT NULL DEFAULT '',
    EmailSmtpPassword VARCHAR(500) NOT NULL DEFAULT '',
    EmailSmtpFromEmail VARCHAR(180) NOT NULL DEFAULT '',
    EmailSmtpFromName VARCHAR(180) NOT NULL DEFAULT '',
    EmailSmtpReplyTo VARCHAR(180) NOT NULL DEFAULT ''
);

ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpEnabled BOOLEAN NOT NULL DEFAULT false;
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpHost VARCHAR(180) NOT NULL DEFAULT 'smtp-mail.outlook.com';
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpPort INT NOT NULL DEFAULT 587;
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpEnableSsl BOOLEAN NOT NULL DEFAULT true;
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpUser VARCHAR(180) NOT NULL DEFAULT '';
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpPassword VARCHAR(500) NOT NULL DEFAULT '';
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpFromEmail VARCHAR(180) NOT NULL DEFAULT '';
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpFromName VARCHAR(180) NOT NULL DEFAULT '';
ALTER TABLE Empresas ADD COLUMN IF NOT EXISTS EmailSmtpReplyTo VARCHAR(180) NOT NULL DEFAULT '';

CREATE TABLE IF NOT EXISTS Usuarios
(
    Id VARCHAR(40) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    Cpf VARCHAR(30) NOT NULL,
    Name VARCHAR(180) NOT NULL,
    Email VARCHAR(180) NOT NULL,
    Phone VARCHAR(30) NOT NULL DEFAULT '',
    Role VARCHAR(60) NOT NULL DEFAULT 'atendente',
    Status VARCHAR(20) NOT NULL DEFAULT 'ativo',
    CreatedAt VARCHAR(30) NOT NULL DEFAULT TO_CHAR(NOW() AT TIME ZONE 'UTC', 'YYYY-MM-DD'),
    LastLoginAt VARCHAR(60) NOT NULL DEFAULT '-',
    PasswordHash VARCHAR(300) NOT NULL,
    MustChangePassword BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT UQ_Usuarios_Cpf UNIQUE (Cpf),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email)
);

ALTER TABLE Usuarios ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';

CREATE TABLE IF NOT EXISTS Sessoes
(
    Id VARCHAR(80) PRIMARY KEY,
    UserId VARCHAR(40) NOT NULL,
    Device VARCHAR(120) NOT NULL DEFAULT '',
    Location VARCHAR(150) NOT NULL DEFAULT '',
    Ip VARCHAR(64) NOT NULL DEFAULT '',
    LastActive VARCHAR(80) NOT NULL DEFAULT 'Agora mesmo',
    Platform VARCHAR(20) NOT NULL DEFAULT 'desktop',
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT FK_Sessoes_Usuarios FOREIGN KEY (UserId) REFERENCES Usuarios (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_Sessoes_UserId ON Sessoes (UserId);

DROP TABLE IF EXISTS PasswordResetTokens_Bkp;

CREATE TABLE IF NOT EXISTS PasswordResetTokens
(
    Id VARCHAR(40) PRIMARY KEY,
    UserId VARCHAR(40) NOT NULL,
    Email VARCHAR(180) NOT NULL,
    Cnpj VARCHAR(30) NULL,
    TokenHash VARCHAR(128) NOT NULL,
    CreatedAt TIMESTAMPTZ NOT NULL,
    RequestedAt TIMESTAMPTZ NOT NULL,
    ExpiresAt TIMESTAMPTZ NOT NULL,
    ConsumedAt TIMESTAMPTZ NULL,
    RequestedIp VARCHAR(64) NULL,
    RequestedUserAgent VARCHAR(500) NULL,
    RequestedDevice VARCHAR(120) NULL,
    ResetIp VARCHAR(64) NULL,
    ResetUserAgent VARCHAR(500) NULL,
    ResetDevice VARCHAR(120) NULL,
    UpdatedAt TIMESTAMPTZ NOT NULL,
    CONSTRAINT FK_PasswordResetTokens_Usuarios FOREIGN KEY (UserId) REFERENCES Usuarios (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_PasswordResetTokens_UserId ON PasswordResetTokens (UserId);
CREATE UNIQUE INDEX IF NOT EXISTS IX_PasswordResetTokens_TokenHash ON PasswordResetTokens (TokenHash);
CREATE INDEX IF NOT EXISTS IX_PasswordResetTokens_ExpiresAt ON PasswordResetTokens (ExpiresAt);
CREATE INDEX IF NOT EXISTS IX_PasswordResetTokens_ConsumedAt ON PasswordResetTokens (ConsumedAt);

CREATE TABLE IF NOT EXISTS CaixaSessoes
(
    Id VARCHAR(40) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    OpenedAt TIMESTAMPTZ NOT NULL,
    ClosedAt TIMESTAMPTZ NULL,
    OpeningAmount VARCHAR(30) NOT NULL DEFAULT '0,00',
    ClosingAmount VARCHAR(30) NOT NULL DEFAULT '0,00',
    OperatorId VARCHAR(40) NOT NULL,
    OperatorName VARCHAR(180) NOT NULL DEFAULT '',
    ClosedById VARCHAR(40) NOT NULL DEFAULT '',
    ClosedByName VARCHAR(180) NOT NULL DEFAULT '',
    Note VARCHAR(500) NOT NULL DEFAULT '',
    CONSTRAINT FK_CaixaSessoes_Usuarios FOREIGN KEY (OperatorId) REFERENCES Usuarios (Id)
);

ALTER TABLE CaixaSessoes ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';
CREATE INDEX IF NOT EXISTS IX_CaixaSessoes_ClosedAt ON CaixaSessoes (ClosedAt);

CREATE TABLE IF NOT EXISTS Vendas
(
    Id VARCHAR(40) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    SaleNumber VARCHAR(30) NOT NULL,
    CustomerName VARCHAR(180) NOT NULL DEFAULT 'Consumidor',
    CustomerCpf VARCHAR(30) NOT NULL DEFAULT '-',
    PaymentType VARCHAR(30) NOT NULL DEFAULT '-',
    TotalAmount VARCHAR(30) NOT NULL DEFAULT '0,00',
    OperatorName VARCHAR(180) NOT NULL DEFAULT 'Operador',
    SaleDate TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT UQ_Vendas_SaleNumber UNIQUE (SaleNumber)
);

ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';
ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS CustomerName VARCHAR(180) NOT NULL DEFAULT 'Consumidor';
ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS CustomerCpf VARCHAR(30) NOT NULL DEFAULT '-';
ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS SaleDate TIMESTAMPTZ NOT NULL DEFAULT NOW();
ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS PaymentType VARCHAR(30) NOT NULL DEFAULT '-';
ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS TotalAmount VARCHAR(30) NOT NULL DEFAULT '0,00';
ALTER TABLE Vendas ADD COLUMN IF NOT EXISTS OperatorName VARCHAR(180) NOT NULL DEFAULT 'Operador';

CREATE TABLE IF NOT EXISTS VendaItens
(
    Id VARCHAR(40) PRIMARY KEY,
    VendaId VARCHAR(40) NOT NULL,
    ProductCode VARCHAR(80) NOT NULL,
    ProductName VARCHAR(180) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice VARCHAR(30) NOT NULL DEFAULT '0,00',
    ItemTotal VARCHAR(30) NOT NULL DEFAULT '0,00',
    CONSTRAINT FK_VendaItens_Vendas FOREIGN KEY (VendaId) REFERENCES Vendas (Id) ON DELETE CASCADE
);

ALTER TABLE VendaItens ADD COLUMN IF NOT EXISTS UnitPrice VARCHAR(30) NOT NULL DEFAULT '0,00';
ALTER TABLE VendaItens ADD COLUMN IF NOT EXISTS ItemTotal VARCHAR(30) NOT NULL DEFAULT '0,00';
CREATE INDEX IF NOT EXISTS IX_VendaItens_VendaId ON VendaItens (VendaId);

UPDATE VendaItens i
   SET UnitPrice = COALESCE(NULLIF(TRIM(p.ProductSalePrice), ''), NULLIF(TRIM(p.ProductUnitPrice), ''), '0,00')
  FROM Produtos p
 WHERE p.ProductCode = i.ProductCode
   AND (
        NULLIF(TRIM(i.UnitPrice), '') IS NULL
        OR REPLACE(REPLACE(REPLACE(TRIM(i.UnitPrice), 'R$', ''), '.', ''), ',', '.') IN ('0', '0.00')
   );

WITH normalized AS (
    SELECT
        Id,
        REGEXP_REPLACE(
            REPLACE(REPLACE(REPLACE(TRIM(UnitPrice), 'R$', ''), '.', ''), ',', '.'),
            '\s+',
            '',
            'g'
        ) AS Amount
    FROM VendaItens
)
UPDATE VendaItens i
   SET ItemTotal = REPLACE(TO_CHAR((n.Amount::NUMERIC * i.Quantity), 'FM999999999990.00'), '.', ',')
  FROM normalized n
 WHERE n.Id = i.Id
   AND (
        NULLIF(TRIM(i.ItemTotal), '') IS NULL
        OR REPLACE(REPLACE(REPLACE(TRIM(i.ItemTotal), 'R$', ''), '.', ''), ',', '.') IN ('0', '0.00')
   )
   AND n.Amount ~ '^-?[0-9]+(\.[0-9]+)?$';

CREATE TABLE IF NOT EXISTS ModulosMercado
(
    Id VARCHAR(80) PRIMARY KEY,
    Title VARCHAR(180) NOT NULL
);

CREATE TABLE IF NOT EXISTS ModuloMercadoRegistros
(
    Id VARCHAR(100) PRIMARY KEY,
    CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal',
    ModuleId VARCHAR(80) NOT NULL,
    Title VARCHAR(180) NOT NULL,
    Description VARCHAR(500) NOT NULL DEFAULT '',
    Status VARCHAR(80) NOT NULL DEFAULT '',
    Amount VARCHAR(40) NOT NULL DEFAULT '',
    Meta VARCHAR(180) NOT NULL DEFAULT '',
    CONSTRAINT FK_ModuloMercadoRegistros_ModulosMercado FOREIGN KEY (ModuleId) REFERENCES ModulosMercado (Id) ON DELETE CASCADE
);

ALTER TABLE ModuloMercadoRegistros ADD COLUMN IF NOT EXISTS CompanyId VARCHAR(40) NOT NULL DEFAULT 'empresa-principal';
CREATE INDEX IF NOT EXISTS IX_ModuloMercadoRegistros_ModuleId ON ModuloMercadoRegistros (ModuleId);

INSERT INTO Usuarios
    (Id, CompanyId, Cpf, Name, Email, Phone, Role, Status, CreatedAt, LastLoginAt, PasswordHash, MustChangePassword)
VALUES
    ('usr-001', 'empresa-principal', '06.332.765/0001-05', 'Flávio Oliveira', 'flavio@hpdv.com.br', '(11) 98888-1111', 'administrador', 'ativo', '2026-02-10', '-', '100000.aG9ydXNwZHZzZWVkMTIzNA==.2rLHDQjZmUF6Oolm44OWtYqIU7b1sXCtUV1XOx1JcWc=', false)
ON CONFLICT (Id) DO UPDATE
SET Cpf = EXCLUDED.Cpf,
    CompanyId = EXCLUDED.CompanyId,
    Name = EXCLUDED.Name,
    Email = EXCLUDED.Email,
    Phone = EXCLUDED.Phone,
    Role = EXCLUDED.Role,
    Status = EXCLUDED.Status,
    CreatedAt = EXCLUDED.CreatedAt,
    LastLoginAt = EXCLUDED.LastLoginAt,
    PasswordHash = EXCLUDED.PasswordHash,
    MustChangePassword = EXCLUDED.MustChangePassword;

INSERT INTO Empresas
    (Id, FantasyName, CorporateName, Cnpj, StateRegistration, Website, Email, SacPhone, Phone, Mobile,
     Cep, Address, Number, Neighborhood, City, Uf, Complement, EmailSmtpEnabled, EmailSmtpHost,
     EmailSmtpPort, EmailSmtpEnableSsl, EmailSmtpUser, EmailSmtpPassword, EmailSmtpFromEmail,
     EmailSmtpFromName, EmailSmtpReplyTo)
VALUES
    ('empresa-principal', 'Hórus PDV', 'Hórus PDV LTDA', '06.332.765/0001-05',
     '123.456.789.110', 'https://www.horuspdv.com.br', 'contato@hpdv.com.br',
     '(11) 3000-1000', '(11) 3149-5959', '(11) 98888-1000', '01310-200',
     'Avenida Paulista', '1578', 'Bela Vista', 'São Paulo', 'SP', 'Próximo ao MASP',
     false, 'smtp-mail.outlook.com', 587, true, '', '', 'naoresponderhoruspdv@outlook.com',
     'Hórus PDV', '')
ON CONFLICT (Id) DO UPDATE
SET FantasyName = EXCLUDED.FantasyName,
    CorporateName = EXCLUDED.CorporateName,
    Cnpj = EXCLUDED.Cnpj,
    StateRegistration = EXCLUDED.StateRegistration,
    Website = EXCLUDED.Website,
    Email = EXCLUDED.Email,
    SacPhone = EXCLUDED.SacPhone,
    Phone = EXCLUDED.Phone,
    Mobile = EXCLUDED.Mobile,
    Cep = EXCLUDED.Cep,
    Address = EXCLUDED.Address,
    Number = EXCLUDED.Number,
    Neighborhood = EXCLUDED.Neighborhood,
    City = EXCLUDED.City,
    Uf = EXCLUDED.Uf,
    Complement = EXCLUDED.Complement,
    EmailSmtpHost = CASE WHEN Empresas.EmailSmtpHost = '' THEN EXCLUDED.EmailSmtpHost ELSE Empresas.EmailSmtpHost END,
    EmailSmtpPort = CASE WHEN Empresas.EmailSmtpPort <= 0 THEN EXCLUDED.EmailSmtpPort ELSE Empresas.EmailSmtpPort END,
    EmailSmtpEnableSsl = true,
    EmailSmtpFromEmail = CASE WHEN Empresas.EmailSmtpFromEmail = '' THEN EXCLUDED.EmailSmtpFromEmail ELSE Empresas.EmailSmtpFromEmail END,
    EmailSmtpFromName = CASE WHEN Empresas.EmailSmtpFromName = '' THEN EXCLUDED.EmailSmtpFromName ELSE Empresas.EmailSmtpFromName END;
