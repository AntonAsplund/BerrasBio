CREATE TABLE [dbo].[Users] (
    [UserName]    NVARCHAR (450) NULL,
    [Password]    NVARCHAR (MAX) NULL,
    [IsAdmin]     BIT            NOT NULL,
    [PhoneNumber] NVARCHAR (MAX) NULL,
    [UserId]      INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (MAX) NULL,
    [LastName]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_UserName]
    ON [dbo].[Users]([UserName] ASC) WHERE ([UserName] IS NOT NULL);

