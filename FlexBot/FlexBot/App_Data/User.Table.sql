CREATE TABLE [dbo].[Table]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [FirstName] VARCHAR(50) NULL, 
    [LastName] VARCHAR(50) NULL, 
    [Location] INT NULL, 
    [HiringDate] DATE NULL, 
    [ManagerId] INT NULL, 
    [Email] VARCHAR(50) NULL, 
    [Phone] VARCHAR(50) NULL 
)
