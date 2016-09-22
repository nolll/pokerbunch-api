CREATE TABLE [dbo].[Event] (
    [EventId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [BunchId] INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);

