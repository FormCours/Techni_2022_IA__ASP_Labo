CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY,
	[Pseudo] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(250) NOT NULL,
	[HashPwd] CHAR(97) NOT NULL,
	[Firstname] NVARCHAR(50) NULL,
	[Lastname] NVARCHAR(50) NULL,
	[Birthdate] DATE NULL,
	
	CONSTRAINT PK_Member PRIMARY KEY ([Id]),
	CONSTRAINT UK_Member__Pseudo UNIQUE ([Pseudo]),
	CONSTRAINT UK_Member__Email UNIQUE ([Email]),
)
