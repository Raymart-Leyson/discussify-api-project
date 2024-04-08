USE [BlogDatabase]
GO

SET IDENTITY_INSERT [dbo].[Category] ON
INSERT [dbo].[Category] ([Id], [Name], [Description]) 
VALUES (1, 'Basketball' , 'Basketball is a game played between two teams of five players each on a rectangular court, usually indoors.');

INSERT [dbo].[Category] ([Id], [Name], [Description]) 
VALUES (2, 'Education', 'Education is the act or process of imparting or acquiring particular knowledge or skills, as for a profession.');

INSERT [dbo].[Category] ([Id], [Name], [Description]) 
VALUES (3, 'Transportation', 'Transportation is the movement of goods and persons from place to place and the various means by which such movement is accomplished.');

INSERT [dbo].[Category] ([Id], [Name], [Description]) 
VALUES (4, 'Health', 'Health is the condition of the body and the degree to which it is free from illness, or the state of being well.');
SET IDENTITY_INSERT [dbo].[Category] OFF