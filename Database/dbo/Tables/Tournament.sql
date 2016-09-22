CREATE TABLE [dbo].[Tournament] (
    [TournamentId] INT           IDENTITY (1, 1) NOT NULL,
    [HomegameId]   INT           NOT NULL,
    [Buyin]        INT           NOT NULL,
    [Date]         DATE          NOT NULL,
    [Duration]     INT           NOT NULL,
    [Location]     NVARCHAR (50) NOT NULL,
    [Timestamp]    DATETIME      NOT NULL,
    [Published]    BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC)
);

