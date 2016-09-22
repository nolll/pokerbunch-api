CREATE TABLE [dbo].[CashgameCheckpoint] (
    [CheckpointId] INT      IDENTITY (1, 1) NOT NULL,
    [GameId]       INT      NOT NULL,
    [PlayerId]     INT      NOT NULL,
    [Type]         INT      DEFAULT ((0)) NOT NULL,
    [Amount]       INT      DEFAULT ((0)) NOT NULL,
    [Stack]        INT      DEFAULT ((0)) NOT NULL,
    [Timestamp]    DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([CheckpointId] ASC), 
    CONSTRAINT [FK_CashgameCheckpoint_Game] FOREIGN KEY (GameId) REFERENCES [Game]([GameId]), 
    CONSTRAINT [FK_CashgameCheckpoint_Player] FOREIGN KEY ([PlayerId]) REFERENCES [Player]([PlayerId]) 
);

