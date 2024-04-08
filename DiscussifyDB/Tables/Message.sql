CREATE TABLE [dbo].[Message]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[AnonymousId] BIGINT NULL,
	[Content] TEXT NOT NULL, 
	[Votes] INT NOT NULL DEFAULT 0,
	[DateTimeCreated] DATETIME NOT NULL, 
	[DateTimeUpdated] DATETIME NOT NULL,
	CONSTRAINT [FK_AnonymousMessage] FOREIGN KEY ([AnonymousId]) REFERENCES [dbo].[Anonymous]([Id]) ON DELETE CASCADE
)
GO