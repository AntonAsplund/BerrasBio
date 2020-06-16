CREATE TABLE [dbo].[Seats] (
    [SeatId]     INT IDENTITY (1, 1) NOT NULL,
    [SalonId]    INT NOT NULL,
    [SeatNumber] INT NOT NULL,
    CONSTRAINT [PK_Seats] PRIMARY KEY CLUSTERED ([SeatId] ASC),
    CONSTRAINT [FK_Seats_Salons_SalonId] FOREIGN KEY ([SalonId]) REFERENCES [dbo].[Salons] ([SalonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Seats_SalonId]
    ON [dbo].[Seats]([SalonId] ASC);

