USE [master]
GO
/****** Object:  Database [Ajinkya_Repository]    Script Date: 17/01/2019 10:29:28 PM ******/
CREATE DATABASE [Ajinkya_Repository] ON  PRIMARY 
( NAME = N'Ajinkya_Repository', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\Ajinkya_Repository.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Ajinkya_Repository_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\Ajinkya_Repository_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Ajinkya_Repository] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Ajinkya_Repository].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Ajinkya_Repository] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET ARITHABORT OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Ajinkya_Repository] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Ajinkya_Repository] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Ajinkya_Repository] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Ajinkya_Repository] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Ajinkya_Repository] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Ajinkya_Repository] SET  MULTI_USER 
GO
ALTER DATABASE [Ajinkya_Repository] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Ajinkya_Repository] SET DB_CHAINING OFF 
GO
USE [Ajinkya_Repository]
GO
/****** Object:  Table [dbo].[Registration]    Script Date: 17/01/2019 10:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registration](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [varchar](max) NOT NULL,
	[Last_Name] [varchar](max) NOT NULL,
	[Date_Of_Birth] [date] NULL,
	[EmailID] [varchar](max) NOT NULL,
	[Mobile_Number] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[ActivationCode] [uniqueidentifier] NOT NULL,
	[ResetPasswordCode] [varchar](max) NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [First_Name]  DEFAULT ('NO_DATA') FOR [First_Name]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [Last_Name]  DEFAULT ('NO_DATA') FOR [Last_Name]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [EmailID]  DEFAULT ('NO_DATA') FOR [EmailID]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [Mobile_Number]  DEFAULT ('NO_DATA') FOR [Mobile_Number]
GO
ALTER TABLE [dbo].[Registration] ADD  CONSTRAINT [Password]  DEFAULT ('NO_DATA') FOR [Password]
GO
USE [master]
GO
ALTER DATABASE [Ajinkya_Repository] SET  READ_WRITE 
GO
