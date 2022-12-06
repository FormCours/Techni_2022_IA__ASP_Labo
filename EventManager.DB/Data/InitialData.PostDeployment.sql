-- Member
SET IDENTITY_INSERT [Member] ON;

INSERT INTO [dbo].[Member] ([Id], [Pseudo],[Email],[HashPwd],[Firstname],[Lastname],[Birthdate])
 VALUES (1, 'Della', 'della.duck@demo.be','$argon2id$v=19$m=65536,t=3,p=1$w2oAgHGMcXgXnBdHeACX3Q$1wbionGgGQ8509Ve5O6A4APighPd456mYl64OiT+Pbc','Della','Duck','1990-03-13')

INSERT INTO [dbo].[Member] ([Id],[Pseudo],[Email],[HashPwd],[Firstname],[Lastname],[Birthdate])
 VALUES (2, 'Zaza', 'zaza.vanderquak@demo.be','$argon2id$v=19$m=65536,t=3,p=1$TcHcBA9TF6Ld8CEbGe/7IQ$kjJfQytTFpJni/aoXt8CMwdRy9dc8JgfvjmHW+Ly/WM','Zaza', NULL, NULL)

SET IDENTITY_INSERT [Member] OFF;
GO

-- Activity
SET IDENTITY_INSERT [Activity] ON;

INSERT INTO [dbo].[Activity]([Id], [Name], [Description], [StartDate], [EndDate], [ImageName], [ImageSrc], [MaxGuest], [CreatorId], [IsCancel], [CreateDate], [UpdateDate])
 VALUES(1, 'Event Exemple', NULL, '2023-01-02 11:30', '2023-01-02 16:00', NULL, NULL, 42, 1, 0, '2022-11-30 14:28:22.130', NULL)

SET IDENTITY_INSERT [Activity] OFF;

-- Registration 
SET IDENTITY_INSERT [Registration] ON;

INSERT INTO [dbo].[Registration] ([Id],[ActivityId],[MemberId],[NbGuest])
 VALUES (1, 1, 2, 10)

SET IDENTITY_INSERT [Registration] OFF;