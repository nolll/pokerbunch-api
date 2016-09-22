CREATE TABLE [dbo].[Homegame] (
    [HomegameId]         INT           IDENTITY (1, 1) NOT NULL,
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

