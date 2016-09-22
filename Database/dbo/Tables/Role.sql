CREATE TABLE [dbo].[Role] (
    [RoleId]   INT           IDENTITY (1, 1) NOT NULL,
    [RoleName] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

