CREATE TABLE [dbo].[UserTwitter] (
    [UserId]      INT            NOT NULL,
    [TwitterName] NVARCHAR (100) NOT NULL,
    [Key]         NVARCHAR (100) NOT NULL,
    [Secret]      NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

