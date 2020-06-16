CREATE TABLE [dbo].[Viewings] (
    [ViewingId] INT           IDENTITY (1, 1) NOT NULL,
    [MovieId]   INT           NOT NULL,
    [StartTime] DATETIME2 (7) NOT NULL,
    [SalonId]   INT           NOT NULL,
    CONSTRAINT [PK_Viewings] PRIMARY KEY CLUSTERED ([ViewingId] ASC),
    CONSTRAINT [FK_Viewings_Movies_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [dbo].[Movies] ([MovieId]),
    CONSTRAINT [FK_Viewings_Salons_SalonId] FOREIGN KEY ([SalonId]) REFERENCES [dbo].[Salons] ([SalonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Viewings_MovieId]
    ON [dbo].[Viewings]([MovieId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Viewings_SalonId]
    ON [dbo].[Viewings]([SalonId] ASC);

