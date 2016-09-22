CREATE TABLE [dbo].[UserSharing] (
    [UserId]      INT           NOT NULL,
    [ServiceName] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [ServiceName] ASC)
);

