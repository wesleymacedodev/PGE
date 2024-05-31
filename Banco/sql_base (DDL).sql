CREATE DATABASE [PGE];
GO 

USE [PGE]
GO

CREATE TABLE Pessoa (
    id INT IDENTITY(1,1),
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(11) NOT NULL,
    oab VARCHAR(20)
	CONSTRAINT PK_Pessoa PRIMARY KEY (id),
	CONSTRAINT UQ_Pessoa_CPF UNIQUE (cpf)
);

CREATE TABLE Login (
	id INT IDENTITY(1,1),
	pessoa_id INT NOT NULL, 
	nome VARCHAR(255) NOT NULL,
	password_hash varbinary(MAX) NOT NULL,
	password_salt varbinary(MAX) NOT NULL,
	admin BIT DEFAULT 0,
	CONSTRAINT FK_Login_Pessoa FOREIGN KEY (pessoa_id) REFERENCES Pessoa(id),
	CONSTRAINT PK_Login PRIMARY KEY (id),
	CONSTRAINT UQ_Login_Pessoa UNIQUE (pessoa_id),
	CONSTRAINT UQ_Login_Nome UNIQUE (nome)
);

CREATE TABLE Processo (
    id INT IDENTITY(1,1),
    numero_processo INT NOT NULL,
    parte_id INT NOT NULL,
    responsavel_id INT NOT NULL,
    tema VARCHAR(255) NOT NULL,
	descricao VARCHAR(255),
    valor DECIMAL(15, 2),
    CONSTRAINT FK_Processo_Parte_Pessoa FOREIGN KEY (parte_id) REFERENCES Pessoa(id),
	CONSTRAINT FK_Processo_Responsavel_Pessoa FOREIGN KEY (responsavel_id) REFERENCES Pessoa(id),
	CONSTRAINT PK_Processo PRIMARY KEY (id),
	CONSTRAINT UQ_Processo_Numero_Processo UNIQUE (numero_processo)
);

CREATE TABLE Documento (
    id INT IDENTITY(1,1),
    nome VARCHAR(255) NOT NULL,
    caminho VARCHAR(255) NOT NULL,
    extensao VARCHAR(10) NOT NULL,
    processo_id INT NOT NULL,
    CONSTRAINT FK_Documento_Processo FOREIGN KEY (processo_id) REFERENCES Processo(id),
	CONSTRAINT PK_Documento PRIMARY KEY (id)
);

CREATE TABLE Distribuir (
    id INT IDENTITY(1,1),
    processo_id INT NOT NULL,
    responsavel_antigo_id INT NOT NULL,
    responsavel_novo_id INT NOT NULL,
    data DATE DEFAULT GETDATE(),
    CONSTRAINT FK_Distribuir_Processo FOREIGN KEY (processo_id) REFERENCES Processo(id),
    CONSTRAINT FK_Distribuir_Antigo_Pessoa FOREIGN KEY (responsavel_antigo_id) REFERENCES Pessoa(id),
    CONSTRAINT FK_Distribuir_Nova_Pessoa FOREIGN KEY (responsavel_novo_id) REFERENCES Pessoa(id),
	CONSTRAINT PK_Distribuir PRIMARY KEY (id)
);

