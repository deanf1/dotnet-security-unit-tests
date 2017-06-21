CREATE TABLE [dbo].[Student] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [LastName]  VARCHAR (MAX) NULL,
    [FirstName] VARCHAR (MAX) NULL,
    [Username] VARCHAR(MAX) NULL, 
    [Password] VARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

