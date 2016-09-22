CREATE TABLE [dbo].[CashgameCheckpoint] (
    [CheckpointId] INT      NOT NULL IDENTITY(1,1),
    [GameId]       INT      NOT NULL,
    [PlayerId]     INT      NOT NULL,
    [Type]         INT      DEFAULT ((0)) NOT NULL,
    [Amount]       INT      DEFAULT ((0)) NOT NULL,
    [Stack]        INT      DEFAULT ((0)) NOT NULL,
    [Timestamp]    DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([CheckpointId] ASC)
);

CREATE TABLE [dbo].[CashgameComment] (
    [GameId]    INT NOT NULL,
    [CommentId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GameId] ASC, [CommentId] ASC)
);

CREATE TABLE [dbo].[Comment] (
    [CommentId]   INT             NOT NULL IDENTITY(1,1),
    [PlayerId]    INT             NOT NULL,
    [Date]        DATETIME        NOT NULL,
    [CommentText] NVARCHAR (1024) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC)
);

CREATE TABLE [dbo].[Game] (
    [GameId]     INT           NOT NULL IDENTITY(1,1),
    [HomegameId] NCHAR (10)    NOT NULL,
    [Date]       DATE          NOT NULL,
    [Location]   NVARCHAR (50) NOT NULL,
    [Timestamp]  DATETIME      NOT NULL,
    [Status]     INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([GameId] ASC)
);

ALTER TABLE [dbo].[Game] ADD CONSTRAINT [DF_Game_Timestamp] DEFAULT (getdate()) FOR [Timestamp]
GO

CREATE TABLE [dbo].[Homegame] (
    [HomegameId]         INT           NOT NULL IDENTITY(1,1),
    [Name]               NVARCHAR (50) NOT NULL,
    [DisplayName]        NVARCHAR (50) NULL,
    [Description]        NVARCHAR (50) NULL,
    [Timezone]           NVARCHAR (50) NOT NULL,
    [DefaultBuyin]       INT           NOT NULL,
    [Currency]           NVARCHAR (3)  NOT NULL,
    [CurrencyLayout]     NVARCHAR (20) NOT NULL,
    [CashgamesEnabled]   BIT           NOT NULL,
    [TournamentsEnabled] BIT           NOT NULL,
    [VideosEnabled]      BIT           NOT NULL,
    [HouseRules]         TEXT          NULL,
    PRIMARY KEY CLUSTERED ([HomegameId] ASC)
);

CREATE TABLE [dbo].[Player] (
    [PlayerId]   INT           NOT NULL IDENTITY(1,1),
    [HomegameId] INT           NOT NULL,
    [UserId]     INT           NULL,
    [RoleId]     INT           NOT NULL,
    [Approved]   BIT           DEFAULT ((0)) NOT NULL,
    [PlayerName] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([PlayerId] ASC)
);

CREATE TABLE [dbo].[Role] (
    [RoleId]   INT           NOT NULL IDENTITY(1,1),
    [RoleName] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

CREATE TABLE [dbo].[Tournament] (
    [TournamentId] INT           NOT NULL IDENTITY(1,1),
    [HomegameId]   INT           NOT NULL,
    [Buyin]        INT           NOT NULL,
    [Date]         DATE          NOT NULL,
    [Duration]     INT           NOT NULL,
    [Location]     NVARCHAR (50) NOT NULL,
    [Timestamp]    DATETIME      NOT NULL,
    [Published]    BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC)
);

CREATE TABLE [dbo].[TournamentComment] (
    [TournamentId] INT NOT NULL,
    [CommentId]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC)
);

CREATE TABLE [dbo].[TournamentPayout] (
    [TournamentId] INT NOT NULL,
    [Position]     INT NOT NULL,
    [Payout]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC)
);

CREATE TABLE [dbo].[TournamentResult] (
    [TournamentId] INT NOT NULL,
    [PlayerId]     INT NOT NULL,
    [Position]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TournamentId] ASC, [PlayerId] ASC)
);

CREATE TABLE [dbo].[User] (
    [UserId]      INT           NOT NULL IDENTITY(1,1),
    [Token]       NVARCHAR (50) NULL,
    [UserName]    NVARCHAR (50) NOT NULL,
    [Password]    NVARCHAR (50) NULL,
    [Salt]        NVARCHAR (50) NULL,
    [RoleId]      INT           NOT NULL,
    [RealName]    NVARCHAR (50) NULL,
    [DisplayName] NVARCHAR (50) NOT NULL,
    [Email]       NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[UserSharing] (
    [UserId]      INT           NOT NULL,
    [ServiceName] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [ServiceName] ASC)
);

CREATE TABLE [dbo].[UserTwitter] (
    [UserId]      INT            NOT NULL,
    [TwitterName] NVARCHAR (100) NOT NULL,
    [Key]         NVARCHAR (100) NOT NULL,
    [Secret]      NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[Video] (
    [VideoId]    INT            NOT NULL IDENTITY(1,1),
    [HomegameId] INT            NOT NULL,
    [Date]       DATE           NOT NULL,
    [Thumbnail]  NVARCHAR (255) NOT NULL,
    [Length]     INT            NOT NULL,
    [Width]      INT            NOT NULL,
    [Height]     INT            NOT NULL,
    [Source]     NVARCHAR (20)  NOT NULL,
    [Type]       NVARCHAR (20)  NOT NULL,
    [Hidden]     BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([VideoId] ASC)
);

