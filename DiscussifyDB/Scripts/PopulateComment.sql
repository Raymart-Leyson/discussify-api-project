USE [BlogDatabase]
GO

INSERT INTO Comment(UserId, PostId, Content, DateTimeCreated, DateTimeUpdated)
VALUES
(1, 2, 'Good', '20220512 08:28:09 AM', '20220512 09:28:09 AM'),
(2, 3, 'Spectacular', '20220513 07:28:09 AM', '20220513 08:28:09 AM'),
(3, 4, 'Commendable', '20220514 02:28:09 PM', '20220514 04:28:09 PM'),
(4, 1, 'Awesome', '20220515 06:28:09 PM', '20220515 09:28:09 PM'),
(1, 1, 'Helpful', '20220515 05:30:09 PM', '20220515 07:45:09 PM')
