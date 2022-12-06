CREATE TABLE [dbo].[Registration]
(
	[Id] INT NOT NULL IDENTITY,
	[ActivityId] INT NOT NULL,
	[MemberId] INT NOT NULL,
	[NbGuest] INT NOT NULL
	
	CONSTRAINT PK_Registration PRIMARY KEY ([Id]),
	CONSTRAINT CK_Registration__NbGuest CHECK([NbGuest] > 0),
	CONSTRAINT FK_Registration__Activity FOREIGN KEY([ActivityId]) REFERENCES [Activity]([Id]) ON DELETE CASCADE,
	CONSTRAINT FK_Registration__Member FOREIGN KEY([MemberId]) REFERENCES [Member]([Id]) 
)
