CREATE TABLE [dbo].[Activity]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[StartDate] DATETIME2 NOT NULL,
	[EndDate] DATETIME2 NOT NULL,
	[ImageName] NVARCHAR(100) NULL,
	[ImageSrc] VARCHAR(50) NULL,
	[MaxGuest] INT NULL,
	[CreatorId] INT NOT NULL,
	[IsCancel] BIT DEFAULT 0,
	[CreateDate] DATETIME2 DEFAULT (GETDATE()),
	[UpdateDate] DATETIME2 NULL,
	
	CONSTRAINT PK_Activity PRIMARY KEY ([Id]),
	CONSTRAINT CK_Activity__EventDate CHECK([StartDate] < [EndDate]),
	CONSTRAINT CK_Activity__MaxGuest CHECK([MaxGuest] > 0),
	CONSTRAINT FK_Activity__Creator FOREIGN KEY ([CreatorId]) REFERENCES [Member]([Id])
)
