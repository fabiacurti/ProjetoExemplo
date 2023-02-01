USE master
CREATE DATABASE API_Padrao
GO
CREATE LOGIN api_user WITH PASSWORD = '123';
GO
USE API_Padrao
GO
create user api_user for login api_user
GO
EXEC sp_addrolemember N'db_owner', N'api_user'
GO

CREATE TABLE [Log](
	Id int IDENTITY(1,1) NOT NULL,
	Aplicacao nvarchar(255) NOT NULL,
	DataHora datetime NOT NULL,
	Nivel nvarchar(255) NOT NULL,
	Mensagem nvarchar(max) NOT NULL,
	Origem nvarchar(255) NULL,
	Endereco nvarchar(max) NULL,
	Excecao nvarchar(max) NULL,
	Usuario nvarchar(255) NULL
	CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ( Id ASC) 
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE Exemplo(
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	Descricao VARCHAR(255) NOT NULL,
	DataHora DATETIME NOT NULL,
	Quantidade INT NOT NULL,
	Valor FLOAT NOT NULL,
	Ativo BIT NOT NULL
)

CREATE TABLE ExemploSubItem(
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	Descricao VARCHAR(255),
	Ordem INT,
	ExemploId UNIQUEIDENTIFIER NOT NULL,
	Ativo BIT
)

ALTER TABLE ExemploSubItem  WITH CHECK ADD CONSTRAINT [FK_ExemploSubItem_Exemplo] FOREIGN KEY(ExemploId)
REFERENCES Exemplo (Id)
GO