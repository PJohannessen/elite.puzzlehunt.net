USE [master]
GO
/****** Object:  Database [PuzzleHunt]    Script Date: 12/06/2011 23:57:57 ******/
CREATE DATABASE [PuzzleHunt] ON  PRIMARY 
( NAME = N'PuzzleHunt', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PuzzleHunt.mdf' , SIZE = 10240KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'PuzzleHunt_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PuzzleHunt_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PuzzleHunt] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PuzzleHunt].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PuzzleHunt] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [PuzzleHunt] SET ANSI_NULLS OFF
GO
ALTER DATABASE [PuzzleHunt] SET ANSI_PADDING OFF
GO
ALTER DATABASE [PuzzleHunt] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [PuzzleHunt] SET ARITHABORT OFF
GO
ALTER DATABASE [PuzzleHunt] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [PuzzleHunt] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [PuzzleHunt] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [PuzzleHunt] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [PuzzleHunt] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [PuzzleHunt] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [PuzzleHunt] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [PuzzleHunt] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [PuzzleHunt] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [PuzzleHunt] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [PuzzleHunt] SET  DISABLE_BROKER
GO
ALTER DATABASE [PuzzleHunt] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [PuzzleHunt] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [PuzzleHunt] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [PuzzleHunt] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [PuzzleHunt] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [PuzzleHunt] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [PuzzleHunt] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [PuzzleHunt] SET  READ_WRITE
GO
ALTER DATABASE [PuzzleHunt] SET RECOVERY FULL
GO
ALTER DATABASE [PuzzleHunt] SET  MULTI_USER
GO
ALTER DATABASE [PuzzleHunt] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [PuzzleHunt] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'PuzzleHunt', N'ON'
GO
USE [PuzzleHunt]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](32) NOT NULL,
	[Password] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UIX_Users] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (1, N'Admin', N'$2a$10$r3RIA8J1Vag5OODIq/64dedJn6sx4WDRGHHWSCJTThOe77EICHQEi')
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (4, N'TestUser1', N'$2a$10$r3RIA8J1Vag5OODIq/64dedJn6sx4WDRGHHWSCJTThOe77EICHQEi')
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (5, N'TestUser2', N'$2a$10$r3RIA8J1Vag5OODIq/64dedJn6sx4WDRGHHWSCJTThOe77EICHQEi')
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (6, N'TestUser3', N'$2a$10$Ur16Vx1buHuASIXEghDH9u.WpQO5vqd9rW7LDq3VY3spOTkkh3.Wy')
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (7, N'TestUser4', N'$2a$10$K5WJcg215ZK0RiMoIRnEU.x9VNCXsW1IpDw31xirDOtIMFSc766b2')
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (8, N'TestUser6', N'$2a$10$GaW5ZyNC6PN2YFurTpHJ9eLuv9gKZSRuIjYBKWvxvdvVDucUG919i')
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[Hunts]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hunts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatorId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Hunts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Hunts] ON
INSERT [dbo].[Hunts] ([Id], [CreatorId], [Name], [StartTime], [EndTime]) VALUES (1, 1, N'Elite Puzzle Hunt 2011', CAST(0x00009FAC00000000 AS DateTime), CAST(0x00009FCA00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[Hunts] OFF
/****** Object:  Table [dbo].[Teams]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HuntId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Password] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Teams] ON
INSERT [dbo].[Teams] ([Id], [HuntId], [Name], [Password]) VALUES (1, 1, N'Test Team 1', N'$2a$10$QHd.TfcgbnWhidYutYix1.BopxbNvK6xOyAflSWQYZNZiHnfCGkFC')
INSERT [dbo].[Teams] ([Id], [HuntId], [Name], [Password]) VALUES (2, 1, N'Test Team 2', N'$2a$10$8cO2NHGAVPicA/H84Ah9OeTfwCyey4mvPNRudRfdNBgIs4svxID9K')
INSERT [dbo].[Teams] ([Id], [HuntId], [Name], [Password]) VALUES (3, 1, N'Test Team 3', N'$2a$10$f5lOOGXHsQCL6mb8vSzaOe/TOWObykQ5PYXORRsRNzesg60lwRnLO')
SET IDENTITY_INSERT [dbo].[Teams] OFF
/****** Object:  Table [dbo].[Puzzles]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Puzzles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HuntId] [int] NOT NULL,
	[CreatorId] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Difficulty] [nvarchar](32) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Answer] [nvarchar](64) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Solution] [nvarchar](max) NULL,
 CONSTRAINT [PK_Puzzles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Puzzles] ON
INSERT [dbo].[Puzzles] ([Id], [HuntId], [CreatorId], [Order], [Difficulty], [Name], [Answer], [Content], [Solution]) VALUES (1, 1, 1, 3, N'Hard', N'Test Puzzle 2', N'answer2', N'This is the second test puzzle', NULL)
INSERT [dbo].[Puzzles] ([Id], [HuntId], [CreatorId], [Order], [Difficulty], [Name], [Answer], [Content], [Solution]) VALUES (2, 1, 1, 2, N'Medium', N'Puzzle 3', N'answer', N'Test Puzzle 3', NULL)
INSERT [dbo].[Puzzles] ([Id], [HuntId], [CreatorId], [Order], [Difficulty], [Name], [Answer], [Content], [Solution]) VALUES (3, 1, 1, 1, N'Easy', N'Puzzle 4', N'answer', N'Test puzzle 4', NULL)
INSERT [dbo].[Puzzles] ([Id], [HuntId], [CreatorId], [Order], [Difficulty], [Name], [Answer], [Content], [Solution]) VALUES (6, 1, 1, 5, N'Super Extreme', N'Puzzle 5', N'ANSWER', N'<b>Content</b>', N'<i>Answer</i>')
SET IDENTITY_INSERT [dbo].[Puzzles] OFF
/****** Object:  Table [dbo].[Hints]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hints](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PuzzleId] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Content] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Hints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Hints] ON
INSERT [dbo].[Hints] ([Id], [PuzzleId], [Order], [Content]) VALUES (1, 1, 1, N'First hint.')
INSERT [dbo].[Hints] ([Id], [PuzzleId], [Order], [Content]) VALUES (2, 1, 2, N'Second hint.')
INSERT [dbo].[Hints] ([Id], [PuzzleId], [Order], [Content]) VALUES (3, 1, 3, N'Third hint.')
SET IDENTITY_INSERT [dbo].[Hints] OFF
/****** Object:  Table [dbo].[TeamPuzzleResults]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamPuzzleResults](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TeamId] [int] NOT NULL,
	[PuzzleId] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
 CONSTRAINT [PK_TeamPuzzleResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TeamPuzzleResults] ON
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (1, 1, 3, CAST(0x00009FAE002F1D97 AS DateTime), NULL)
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (2, 1, 1, CAST(0x00009FAE0036461D AS DateTime), NULL)
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (3, 2, 3, CAST(0x00009FAF0005CDD3 AS DateTime), CAST(0x00009FAF002AEF68 AS DateTime))
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (4, 2, 2, CAST(0x00009FB000C11ABE AS DateTime), NULL)
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (5, 2, 1, CAST(0x00009FB100B20CF6 AS DateTime), CAST(0x00009FB100B94331 AS DateTime))
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (6, 2, 6, CAST(0x00009FB100C333FE AS DateTime), NULL)
INSERT [dbo].[TeamPuzzleResults] ([Id], [TeamId], [PuzzleId], [StartTime], [EndTime]) VALUES (7, 3, 1, CAST(0x00009FB100E4BD13 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[TeamPuzzleResults] OFF
/****** Object:  Table [dbo].[TeamMemberships]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamMemberships](
	[TeamId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_TeamMemberships] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_TeamMemberships] ON [dbo].[TeamMemberships] 
(
	[UserId] ASC,
	[TeamId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [dbo].[TeamMemberships] ([TeamId], [UserId]) VALUES (1, 4)
INSERT [dbo].[TeamMemberships] ([TeamId], [UserId]) VALUES (1, 5)
INSERT [dbo].[TeamMemberships] ([TeamId], [UserId]) VALUES (2, 6)
INSERT [dbo].[TeamMemberships] ([TeamId], [UserId]) VALUES (2, 7)
INSERT [dbo].[TeamMemberships] ([TeamId], [UserId]) VALUES (3, 8)
/****** Object:  Table [dbo].[TeamHintRequests]    Script Date: 12/06/2011 23:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamHintRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TeamId] [int] NOT NULL,
	[HintId] [int] NOT NULL,
	[RequestTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TeamHintRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TeamHintRequests] ON
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (1, 2, 1, CAST(0x00009FB100000000 AS DateTime))
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (2, 1, 1, CAST(0x00009FB100E16D00 AS DateTime))
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (5, 1, 2, CAST(0x00009FB100E333D2 AS DateTime))
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (6, 1, 3, CAST(0x00009FB100E338F2 AS DateTime))
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (7, 3, 1, CAST(0x00009FB100E55954 AS DateTime))
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (8, 3, 2, CAST(0x00009FB100E55C8D AS DateTime))
INSERT [dbo].[TeamHintRequests] ([Id], [TeamId], [HintId], [RequestTime]) VALUES (9, 3, 3, CAST(0x00009FB100E55DC4 AS DateTime))
SET IDENTITY_INSERT [dbo].[TeamHintRequests] OFF
/****** Object:  ForeignKey [FK_Hunts_Users]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[Hunts]  WITH CHECK ADD  CONSTRAINT [FK_Hunts_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Hunts] CHECK CONSTRAINT [FK_Hunts_Users]
GO
/****** Object:  ForeignKey [FK_Teams_Hunts]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_Teams_Hunts] FOREIGN KEY([HuntId])
REFERENCES [dbo].[Hunts] ([Id])
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_Teams_Hunts]
GO
/****** Object:  ForeignKey [FK_Puzzles_Hunts]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[Puzzles]  WITH CHECK ADD  CONSTRAINT [FK_Puzzles_Hunts] FOREIGN KEY([HuntId])
REFERENCES [dbo].[Hunts] ([Id])
GO
ALTER TABLE [dbo].[Puzzles] CHECK CONSTRAINT [FK_Puzzles_Hunts]
GO
/****** Object:  ForeignKey [FK_Puzzles_Users]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[Puzzles]  WITH CHECK ADD  CONSTRAINT [FK_Puzzles_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Puzzles] CHECK CONSTRAINT [FK_Puzzles_Users]
GO
/****** Object:  ForeignKey [FK_Hints_Puzzles]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[Hints]  WITH CHECK ADD  CONSTRAINT [FK_Hints_Puzzles] FOREIGN KEY([PuzzleId])
REFERENCES [dbo].[Puzzles] ([Id])
GO
ALTER TABLE [dbo].[Hints] CHECK CONSTRAINT [FK_Hints_Puzzles]
GO
/****** Object:  ForeignKey [FK_TeamPuzzleResults_Puzzles]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[TeamPuzzleResults]  WITH CHECK ADD  CONSTRAINT [FK_TeamPuzzleResults_Puzzles] FOREIGN KEY([PuzzleId])
REFERENCES [dbo].[Puzzles] ([Id])
GO
ALTER TABLE [dbo].[TeamPuzzleResults] CHECK CONSTRAINT [FK_TeamPuzzleResults_Puzzles]
GO
/****** Object:  ForeignKey [FK_TeamPuzzleResults_Teams]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[TeamPuzzleResults]  WITH CHECK ADD  CONSTRAINT [FK_TeamPuzzleResults_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamPuzzleResults] CHECK CONSTRAINT [FK_TeamPuzzleResults_Teams]
GO
/****** Object:  ForeignKey [FK_TeamMemberships_Teams]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[TeamMemberships]  WITH CHECK ADD  CONSTRAINT [FK_TeamMemberships_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamMemberships] CHECK CONSTRAINT [FK_TeamMemberships_Teams]
GO
/****** Object:  ForeignKey [FK_TeamMemberships_Users]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[TeamMemberships]  WITH CHECK ADD  CONSTRAINT [FK_TeamMemberships_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TeamMemberships] CHECK CONSTRAINT [FK_TeamMemberships_Users]
GO
/****** Object:  ForeignKey [FK_TeamHintRequests_Hints]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[TeamHintRequests]  WITH CHECK ADD  CONSTRAINT [FK_TeamHintRequests_Hints] FOREIGN KEY([HintId])
REFERENCES [dbo].[Hints] ([Id])
GO
ALTER TABLE [dbo].[TeamHintRequests] CHECK CONSTRAINT [FK_TeamHintRequests_Hints]
GO
/****** Object:  ForeignKey [FK_TeamHintRequests_Teams]    Script Date: 12/06/2011 23:57:57 ******/
ALTER TABLE [dbo].[TeamHintRequests]  WITH CHECK ADD  CONSTRAINT [FK_TeamHintRequests_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamHintRequests] CHECK CONSTRAINT [FK_TeamHintRequests_Teams]
GO
