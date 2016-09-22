CREATE TABLE [dbo].[TournamentResult] (
    [TournamentId] INT NOT NULL,
    [PlayerId]     INT NOT NULL,
    [Position]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC, [PlayerId] ASC)
);

