CREATE TABLE [dbo].[Player] (
    [PlayerId]   INT           IDENTITY (1, 1) NOT NULL,
    [HomegameId] INT           NOT NULL,
    [UserId]     INT           NULL,
    [RoleId]     INT           NOT NULL,
    [Approved]   BIT           DEFAULT ((0)) NOT NULL,
    [PlayerName] NVARCHAR (50) NULL,
    [Color] NVARCHAR(10) NULL  , 
    PRIMARY KEY CLUSTERED ([PlayerId] ASC)
);

