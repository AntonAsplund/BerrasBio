CREATE TABLE [dbo].[Order] (
    [OrderId] INT IDENTITY (1, 1) NOT NULL,
    [UserId]  INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_Order_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Order_UserId]
    ON [dbo].[Order]([UserId] ASC);

