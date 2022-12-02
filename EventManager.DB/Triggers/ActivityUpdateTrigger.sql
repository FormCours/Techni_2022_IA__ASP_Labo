CREATE TRIGGER [ActivityUpdateTrigger] ON [Activity]
AFTER UPDATE
AS
BEGIN
	UPDATE [Activity]
	 SET [UpdateDate] = GETDATE()
	WHERE [Id] IN (SELECT [Id] FROM [inserted])
END