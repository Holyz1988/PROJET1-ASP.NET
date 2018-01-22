USE [master]
GO
/****** Object:  Database [Simpoll]    Script Date: 22/01/2018 16:53:37 ******/
CREATE DATABASE [Simpoll]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Simpoll', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Simpoll.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Simpoll_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Simpoll_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Simpoll] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Simpoll].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Simpoll] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Simpoll] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Simpoll] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Simpoll] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Simpoll] SET ARITHABORT OFF 
GO
ALTER DATABASE [Simpoll] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Simpoll] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Simpoll] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Simpoll] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Simpoll] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Simpoll] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Simpoll] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Simpoll] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Simpoll] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Simpoll] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Simpoll] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Simpoll] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Simpoll] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Simpoll] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Simpoll] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Simpoll] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Simpoll] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Simpoll] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Simpoll] SET  MULTI_USER 
GO
ALTER DATABASE [Simpoll] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Simpoll] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Simpoll] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Simpoll] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Simpoll] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Simpoll] SET QUERY_STORE = OFF
GO
USE [Simpoll]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Simpoll]
GO
/****** Object:  Table [dbo].[Createur]    Script Date: 22/01/2018 16:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Createur](
	[IdCreateur] [int] IDENTITY(1,1) NOT NULL,
	[NomCreateur] [nvarchar](50) NOT NULL,
	[PrenomCreateur] [nvarchar](50) NOT NULL,
	[EmailCreateur] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Createur_1] PRIMARY KEY CLUSTERED 
(
	[IdCreateur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reponse]    Script Date: 22/01/2018 16:53:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reponse](
	[IdReponse] [int] IDENTITY(1,1) NOT NULL,
	[IntituleReponse] [nvarchar](255) NOT NULL,
	[NbVoteReponse] [int] NOT NULL,
	[FKIdSondage] [int] NOT NULL,
 CONSTRAINT [PK_Reponse_1] PRIMARY KEY CLUSTERED 
(
	[IdReponse] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sondage]    Script Date: 22/01/2018 16:53:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sondage](
	[IdSondage] [int] IDENTITY(1,1) NOT NULL,
	[QuestionSondage] [nvarchar](255) NOT NULL,
	[ChoixMultiple] [bit] NOT NULL,
	[NbVotant] [int] NOT NULL,
	[FKIdCreateur] [int] NOT NULL,
	[UrlPartage] [nvarchar](255) NULL,
	[UrlSuppression] [nvarchar](255) NULL,
	[UrlResultat] [nvarchar](255) NULL,
 CONSTRAINT [PK_Sondage_1] PRIMARY KEY CLUSTERED 
(
	[IdSondage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Createur] ON 

INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (1, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (2, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (3, N'd', N'd', N'd')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (4, N'd', N'd', N'd')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (5, N'd', N'd', N'd')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (6, N'd', N'd', N'd')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (7, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.Fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (8, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.Fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (9, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (10, N'Meistertzheim', N'Damien', N'Meistertzheim.Damine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (11, N'Meistertzheim', N'Damien', N'Meistertzheim.Damine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (12, N'Meistertzheim', N'Damien', N'Meistertzheim.Damine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (13, N'Meistertzheim', N'Damien', N'Meistertzheim.Damine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (14, N'gfdh', N'fdgh', N'dfgh@fgdh')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (15, N'', N'', N'')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (16, N'', N'', N'')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (17, N'', N'', N'')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (18, N'hgjk', N'ghjk', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (19, N'l', N'l', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (20, N'sd', N'sd', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (21, N'kjlm', N'jklm', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (22, N'Amine', N'ZEGHAD', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (23, N'hjk', N'hgjk', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (24, N'dfgh', N'dfh', N'dfgh@fgdh')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (25, N'qsd', N'qsd', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (26, N'', N'', N'')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (27, N'jhkl', N'hjkl', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (28, N'jhkl', N'hjkl', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (29, N'jhkl', N'hjkl', N'lol@lol.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (30, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (31, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (32, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (33, N'Emilien', N'Emilien', N'Emilien@hotmail.com')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (34, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (35, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (36, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (37, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (38, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (39, N'ZEGHAD', N'Amine', N'zghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (40, N'tyuyt', N'ytutyu', N'lol@lol')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (41, N'ghjghf', N'jgfjhf', N'lol@lol')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (42, N'fdg', N'dfg', N'lol@lol')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (43, N'ZEGHAD', N'Amine', N'zghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (44, N'ZEGHAD', N'Amine', N'zeghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (45, N'ZEGHAD', N'Amine', N'zghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (46, N'ZEGHAD', N'Amine', N'zghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (47, N'ZEGHAD', N'Walid', N'zeghad.walid@yahoo.Fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (48, N'dfsg', N'sdfg', N'sdfg@lkjh')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (49, N'ZEGHAD', N'Amine', N'zghad.amine@yahoo.fr')
INSERT [dbo].[Createur] ([IdCreateur], [NomCreateur], [PrenomCreateur], [EmailCreateur]) VALUES (50, N'fgdh', N'fdgh', N'dfgh@gfdh')
SET IDENTITY_INSERT [dbo].[Createur] OFF
SET IDENTITY_INSERT [dbo].[Reponse] ON 

INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (1, N'r', 0, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (2, N'r', 0, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (3, N'r', 0, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (4, N'ghjk', 0, 9)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (5, N'gjk', 0, 9)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (6, N'ghjk', 0, 9)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (7, N'dfsg', 0, 10)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (8, N'sdfg', 0, 10)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (9, N'sdfg', 0, 10)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (10, N'dfsg', 0, 11)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (11, N'sdfg', 0, 11)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (12, N'sdfg', 0, 11)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (13, N'dfsg', 0, 12)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (14, N'sdfg', 0, 12)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (15, N'sdfg', 0, 12)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (16, N'dfsg', 0, 13)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (17, N'sdfg', 0, 13)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (18, N'sdfg', 0, 13)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (19, N'dfsg', 0, 14)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (20, N'sdfg', 0, 14)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (21, N'sdfg', 0, 14)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (22, N'15', 0, 15)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (23, N'25', 0, 15)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (24, N'48', 0, 15)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (25, N'dfg', 0, 16)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (26, N'dfg', 0, 16)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (27, N'dfg', 0, 16)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (28, N'fghf', 0, 17)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (29, N'gdhgd', 0, 17)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (30, N'fgh', 0, 17)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (31, N'15', 0, 18)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (32, N'20', 0, 18)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (33, N'12', 0, 18)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (34, N'15', 1, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (35, N'12', 21, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (37, N'12', 21, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (40, N'12', 1, 4)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (41, N'1', 0, 19)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (42, N'15', 0, 20)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (43, N'hgj', 0, 21)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (44, N'fghf', 0, 22)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (45, N'15', 0, 23)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (46, N'20', 0, 23)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (47, N'30', 0, 23)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (48, N'sdqfdsf', 0, 24)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (49, N'sqdfdsf', 0, 24)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (50, N'sdfdsqf', 0, 24)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (51, N'j', 0, 25)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (52, N'j', 0, 25)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (53, N'j', 0, 25)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (54, N'sdqf', 0, 26)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (55, N'sqdf', 0, 26)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (56, N'sdqf', 0, 26)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (57, N'qsdf', 3, 27)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (58, N'sdqf', 6, 27)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (59, N'qsdf', 1, 27)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (60, N'qsdf', 1, 28)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (61, N'sdqf', 2, 28)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (62, N'qsdf', 0, 28)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (63, N'25', 0, 29)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (64, N'29', 0, 29)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (65, N'48', 0, 29)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (66, N'Croq''Vite', 4, 30)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (67, N'Kebab', 3, 30)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (68, N'Boulangerie', 2, 30)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (69, N'grec', 1, 31)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (70, N'chinoi', 2, 31)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (71, N'turc', 2, 31)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (72, N'qsdf', 0, 32)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (73, N'j', 0, 32)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (74, N'j', 0, 32)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (75, N'Croq''Vite', 0, 33)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (76, N'chinoi', 1, 33)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (77, N'Boulangerie', 0, 33)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (78, N'Croq''Vite', 0, 34)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (79, N'chinoi', 0, 34)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (80, N'Boulangerie', 0, 34)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (81, N'fgh', 0, 35)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (82, N'dgh', 0, 35)
INSERT [dbo].[Reponse] ([IdReponse], [IntituleReponse], [NbVoteReponse], [FKIdSondage]) VALUES (83, N'fgh', 0, 35)
SET IDENTITY_INSERT [dbo].[Reponse] OFF
SET IDENTITY_INSERT [dbo].[Sondage] ON 

INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (1, N'Quel Age ?', 0, 0, 2, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (2, N'd', 0, 0, 4, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (3, N'd', 0, 0, 6, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (4, N'Bonjour', 1, 0, 8, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (9, N'ghjk', 0, 0, 27, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (10, N'dsfg', 1, 0, 29, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (11, N'dsfg', 1, 0, 29, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (12, N'dsfg', 1, 0, 29, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (13, N'dsfg', 1, 0, 29, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (14, N'dsfg', 1, 0, 29, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (15, N'Quel est vôtre âge ?', 0, 0, 30, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (16, N'dfg', 0, 0, 31, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (17, N'gfhd', 1, 1, 32, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (18, N'Age', 0, 0, 33, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (19, N'UML ?', 0, 0, 34, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (20, N'Quel est vôtre âge ?', 0, 0, 35, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (21, N'Quel est vôtre âge ?', 0, 0, 36, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (22, N'Quel est vôtre âge ?', 0, 0, 37, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (23, N'Quel est vôtre âge ?', 0, 0, 38, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (24, N'qdsfdsqf', 0, 0, 39, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (25, N'j', 0, 0, 40, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (26, N'sdqf', 0, 0, 41, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (27, N'sdqf', 0, 10, 42, NULL, NULL, NULL)
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (28, N'sdqf', 0, 3, 43, N'localhost:8870/Simpoll/Vote?idSondage=28', N'localhost:8870/Simpoll/Suppression?idSondage=28', N'localhost:8870/Simpoll/Resultat?idSondage=28')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (29, N'Quel est vôtre âge ?', 1, 2, 44, N'localhost:8870/Simpoll/Vote?idSondage=29', N'localhost:8870/Simpoll/Suppression?idSondage=29', N'localhost:8870/Simpoll/Resultat?idSondage=29')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (30, N'Quel Resto ?', 1, 6, 45, N'localhost:8870/Simpoll/Vote?idSondage=30', N'localhost:8870/Simpoll/Suppression?idSondage=30', N'localhost:8870/Simpoll/Resultat?idSondage=30')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (31, N'Quel Resto ?', 0, 4, 47, N'localhost:8870/Simpoll/Vote?idSondage=31', N'localhost:8870/Simpoll/Suppression?idSondage=31', N'localhost:8870/Simpoll/Resultat?idSondage=31')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (32, N'Quel Resto ?', 0, 0, 48, N'localhost:8870/Simpoll/Vote?idSondage=32', N'localhost:8870/Simpoll/Suppression?idSondage=32', N'localhost:8870/Simpoll/Resultat?idSondage=32')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (33, N'Quel Resto ?', 0, 1, 49, N'localhost:8870/Simpoll/Vote?idSondage=33', N'localhost:8870/Simpoll/Suppression?idSondage=33', N'localhost:8870/Simpoll/Resultat?idSondage=33')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (34, N'Quel Resto ?', 1, 0, 49, N'localhost:8870/Simpoll/Vote?idSondage=34', N'localhost:8870/Simpoll/Suppression?idSondage=34', N'localhost:8870/Simpoll/Resultat?idSondage=34')
INSERT [dbo].[Sondage] ([IdSondage], [QuestionSondage], [ChoixMultiple], [NbVotant], [FKIdCreateur], [UrlPartage], [UrlSuppression], [UrlResultat]) VALUES (35, N'fdgh', 0, 0, 50, N'localhost:8870/Simpoll/Vote?idSondage=35', N'localhost:8870/Simpoll/Suppression?idSondage=35', N'localhost:8870/Simpoll/Resultat?idSondage=35')
SET IDENTITY_INSERT [dbo].[Sondage] OFF
ALTER TABLE [dbo].[Reponse]  WITH CHECK ADD  CONSTRAINT [FK_Sondage_Reponse] FOREIGN KEY([FKIdSondage])
REFERENCES [dbo].[Sondage] ([IdSondage])
GO
ALTER TABLE [dbo].[Reponse] CHECK CONSTRAINT [FK_Sondage_Reponse]
GO
ALTER TABLE [dbo].[Sondage]  WITH CHECK ADD  CONSTRAINT [FK_Createur_Sondage] FOREIGN KEY([FKIdCreateur])
REFERENCES [dbo].[Createur] ([IdCreateur])
GO
ALTER TABLE [dbo].[Sondage] CHECK CONSTRAINT [FK_Createur_Sondage]
GO
USE [master]
GO
ALTER DATABASE [Simpoll] SET  READ_WRITE 
GO
