USE [BlogDatabase]
GO

SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([Id], [Username], [Password], [FirstName], [LastName], [EmailAddress], [DateTimeCreated]) 
VALUES (1, 'davidmaranga', 'fNLv9z8R5cP6XhlZw57BazKko9GbB1zeHLOQvzc4aZw=', 'Xavier David', 'Maranga', 'davidmaranga@gmail.com', '20220118 10:34:09 AM');

INSERT [dbo].[User] ([Id], [Username], [Password], [FirstName], [LastName], [EmailAddress], [DateTimeCreated]) 
VALUES (2, 'jonasangelo', 'meh/L/TGTCTcmgH27X6PABs3LEXjZlPmgqGScsM8xMw=', 'Jonas Angelo', 'Clamor', 'jonasangelo@gmail.com', '20220401 01:25:11 PM');

INSERT [dbo].[User] ([Id], [Username], [Password], [FirstName], [LastName], [EmailAddress], [DateTimeCreated]) 
VALUES (3, 'johnwilliam', '1tzSWJLsHsvnxF4QnpN4uwbLBR2q1AsfdOH5f1/8M5o=', 'John William', 'Miones', 'johnwilliam@gmail.com', '20220512 12:28:09 AM');

INSERT [dbo].[User] ([Id], [Username], [Password], [FirstName], [LastName], [EmailAddress], [DateTimeCreated]) 
VALUES (4, 'jaimesedward', 'Sb5XWa44vKPXEoXIKa9Qgje6wX5RmHdXf1+BWP7Ude8=', 'Jaimes Edward', 'Cabante', 'jaimesedward@gmail.com', '20220824 05:01:55 PM');
SET IDENTITY_INSERT [dbo].[User] OFF