CREATE TABLE [dbo].[TournamentPayout] (
    [TournamentId] INT NOT NULL,
    [Position]     INT NOT NULL,
    [Payout]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC)
);

