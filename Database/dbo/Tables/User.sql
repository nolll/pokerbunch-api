CREATE TABLE [dbo].[User] (
    [UserId]      INT           IDENTITY (1, 1) NOT NULL,
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

