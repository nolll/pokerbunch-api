CREATE TABLE [dbo].[Video] (
    [VideoId]    INT            IDENTITY (1, 1) NOT NULL,
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

