CREATE TABLE [dbo].[Movies] (
    [MovieId]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (MAX) NULL,
    [LengthMinute]     INT            NOT NULL,
    [IsPlaying]        BIT            NOT NULL,
    [Cathegory]        NVARCHAR (MAX) NULL,
    [PerentalGuidance] INT            DEFAULT ((0)) NOT NULL,
    [Plot]             NVARCHAR (MAX) NULL,
    [ReleaseYear]      INT            DEFAULT ((0)) NOT NULL,
    [Actors]           NVARCHAR (MAX) NULL,
    [Director]         NVARCHAR (MAX) NULL,
    [PosterURL]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED ([MovieId] ASC)
);

