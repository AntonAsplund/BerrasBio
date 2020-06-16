CREATE TABLE [dbo].[Tickets] (
    [TicketId]  INT           IDENTITY (1, 1) NOT NULL,
    [Date]      DATETIME2 (7) NOT NULL,
    [SeatId]    INT           NOT NULL,
    [ViewingId] INT           NOT NULL,
    [OrderId]   INT           NOT NULL,
    CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED ([TicketId] ASC),
    CONSTRAINT [FK_Tickets_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([OrderId]),
    CONSTRAINT [FK_Tickets_Seats_SeatId] FOREIGN KEY ([SeatId]) REFERENCES [dbo].[Seats] ([SeatId]),
    CONSTRAINT [FK_Tickets_Viewings_ViewingId] FOREIGN KEY ([ViewingId]) REFERENCES [dbo].[Viewings] ([ViewingId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Tickets_OrderId]
    ON [dbo].[Tickets]([OrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tickets_SeatId]
    ON [dbo].[Tickets]([SeatId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tickets_ViewingId]
    ON [dbo].[Tickets]([ViewingId] ASC);

