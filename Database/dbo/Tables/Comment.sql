CREATE TABLE [dbo].[Comment] (
    [CommentId]   INT             IDENTITY (1, 1) NOT NULL,
    [PlayerId]    INT             NOT NULL,
    [Date]        DATETIME        NOT NULL,
    [CommentText] NVARCHAR (1024) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC)
);

