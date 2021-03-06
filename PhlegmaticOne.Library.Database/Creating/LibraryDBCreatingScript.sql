USE [LibraryDataBase]
GO
/****** Object:  Table [dbo].[Abonents]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Abonents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NULL,
	[BirthDate] [date] NOT NULL,
	[GenderId] [int] NOT NULL,
 CONSTRAINT [PK_Abonents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BooksAuthors]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BooksAuthors](
	[BookId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genders]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Genders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genres]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lendings]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lendings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AbonentId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[LendingDate] [date] NOT NULL,
	[IsReturned] [bit] NOT NULL,
	[ReturnDate] [date] NULL,
	[StateId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[States]    Script Date: 15.12.2021 14:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Abonents] ON 

INSERT [dbo].[Abonents] ([Id], [Name], [Surname], [Patronymic], [BirthDate], [GenderId]) VALUES (2012, N'Semen', N'Swallow', N'Sergeevich', CAST(N'2003-02-24' AS Date), 4013)
INSERT [dbo].[Abonents] ([Id], [Name], [Surname], [Patronymic], [BirthDate], [GenderId]) VALUES (2013, N'Alisa', N'Dvachevskaya', N'Vasilevna', CAST(N'2000-01-31' AS Date), 4014)
INSERT [dbo].[Abonents] ([Id], [Name], [Surname], [Patronymic], [BirthDate], [GenderId]) VALUES (2014, N'Elena', N'Wristova', N'Andreevna', CAST(N'2001-01-01' AS Date), 4014)
INSERT [dbo].[Abonents] ([Id], [Name], [Surname], [Patronymic], [BirthDate], [GenderId]) VALUES (2015, N'Slavyana', N'Leadova', N'Alexeevna', CAST(N'2000-11-23' AS Date), 4014)
INSERT [dbo].[Abonents] ([Id], [Name], [Surname], [Patronymic], [BirthDate], [GenderId]) VALUES (2016, N'Yulya', N'Catova', N'Anatolievna', CAST(N'2002-08-11' AS Date), 4014)
SET IDENTITY_INSERT [dbo].[Abonents] OFF
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3012, N'Aleksandr', N'Pushkin')
INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3013, N'Fedor', N'Dostoevsky')
INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3014, N'Mikhail', N'Lermontov')
INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3015, N'Lev', N'Tolstoy')
INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3016, N'Jack', N'London')
INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3017, N'Arcadiy', N'Strugacky')
INSERT [dbo].[Authors] ([Id], [Name], [Surname]) VALUES (3018, N'Boris', N'Strugacky')
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([Id], [Name], [GenreId]) VALUES (8, N'Captain`s daughter', 21)
INSERT [dbo].[Books] ([Id], [Name], [GenreId]) VALUES (9, N'War and Peace', 21)
INSERT [dbo].[Books] ([Id], [Name], [GenreId]) VALUES (10, N'Hero of our time', 21)
INSERT [dbo].[Books] ([Id], [Name], [GenreId]) VALUES (11, N'Crime and Punishment', 23)
INSERT [dbo].[Books] ([Id], [Name], [GenreId]) VALUES (12, N'The Sea Wolf', 24)
INSERT [dbo].[Books] ([Id], [Name], [GenreId]) VALUES (13, N'Roadside Picnic', 27)
SET IDENTITY_INSERT [dbo].[Books] OFF
GO
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (8, 3012)
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (9, 3015)
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (10, 3014)
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (11, 3013)
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (12, 3016)
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (13, 3017)
INSERT [dbo].[BooksAuthors] ([BookId], [AuthorId]) VALUES (13, 3018)
GO
SET IDENTITY_INSERT [dbo].[Genders] ON 

INSERT [dbo].[Genders] ([Id], [Name]) VALUES (4013, N'Male')
INSERT [dbo].[Genders] ([Id], [Name]) VALUES (4014, N'Female')
INSERT [dbo].[Genders] ([Id], [Name]) VALUES (4015, N'Other')
SET IDENTITY_INSERT [dbo].[Genders] OFF
GO
SET IDENTITY_INSERT [dbo].[Genres] ON 

INSERT [dbo].[Genres] ([Id], [Name]) VALUES (21, N'Romance')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (22, N'Detective')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (23, N'Thriller')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (24, N'Adventure')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (25, N'Comedy')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (26, N'Realism')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (27, N'Fantasy')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (28, N'Folklore')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (29, N'Religion')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (30, N'Novel')
SET IDENTITY_INSERT [dbo].[Genres] OFF
GO
SET IDENTITY_INSERT [dbo].[Lendings] ON 

INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (2, 2012, 8, CAST(N'2021-11-11' AS Date), 1, CAST(N'2021-11-20' AS Date), 8)
INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (1002, 2012, 8, CAST(N'2021-11-11' AS Date), 1, CAST(N'2021-12-31' AS Date), 8)
INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (2002, 2012, 10, CAST(N'2021-11-11' AS Date), 1, CAST(N'2021-12-31' AS Date), 8)
INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (2003, 2012, 12, CAST(N'2021-11-11' AS Date), 1, CAST(N'2021-12-31' AS Date), 8)
INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (3, 2013, 9, CAST(N'2021-11-11' AS Date), 0, NULL, NULL)
INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (4, 2014, 10, CAST(N'2021-11-11' AS Date), 0, NULL, NULL)
INSERT [dbo].[Lendings] ([Id], [AbonentId], [BookId], [LendingDate], [IsReturned], [ReturnDate], [StateId]) VALUES (5, 2015, 11, CAST(N'2021-11-11' AS Date), 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Lendings] OFF
GO
SET IDENTITY_INSERT [dbo].[States] ON 

INSERT [dbo].[States] ([Id], [Name]) VALUES (8, N'Excellent')
INSERT [dbo].[States] ([Id], [Name]) VALUES (9, N'Good')
INSERT [dbo].[States] ([Id], [Name]) VALUES (10, N'Satisfactorily')
INSERT [dbo].[States] ([Id], [Name]) VALUES (11, N'Bad')
INSERT [dbo].[States] ([Id], [Name]) VALUES (12, N'Terrible')
SET IDENTITY_INSERT [dbo].[States] OFF
GO
ALTER TABLE [dbo].[Abonents]  WITH CHECK ADD  CONSTRAINT [FK_Abonents_Genders] FOREIGN KEY([GenderId])
REFERENCES [dbo].[Genders] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Abonents] CHECK CONSTRAINT [FK_Abonents_Genders]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_Genres] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_Genres]
GO
ALTER TABLE [dbo].[BooksAuthors]  WITH CHECK ADD  CONSTRAINT [FK_BooksAuthors_Authors] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BooksAuthors] CHECK CONSTRAINT [FK_BooksAuthors_Authors]
GO
ALTER TABLE [dbo].[BooksAuthors]  WITH CHECK ADD  CONSTRAINT [FK_BooksAuthors_Books] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BooksAuthors] CHECK CONSTRAINT [FK_BooksAuthors_Books]
GO
ALTER TABLE [dbo].[Lendings]  WITH CHECK ADD  CONSTRAINT [FK_Lendings_Abonents] FOREIGN KEY([AbonentId])
REFERENCES [dbo].[Abonents] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lendings] CHECK CONSTRAINT [FK_Lendings_Abonents]
GO
ALTER TABLE [dbo].[Lendings]  WITH CHECK ADD  CONSTRAINT [FK_Lendings_Books] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lendings] CHECK CONSTRAINT [FK_Lendings_Books]
GO
ALTER TABLE [dbo].[Lendings]  WITH CHECK ADD  CONSTRAINT [FK_Lendings_States] FOREIGN KEY([StateId])
REFERENCES [dbo].[States] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lendings] CHECK CONSTRAINT [FK_Lendings_States]
GO
