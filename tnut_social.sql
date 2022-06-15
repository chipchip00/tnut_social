USE [master]
GO
/****** Object:  Database [tnut_social]    Script Date: 15/06/2022 9:48:06 CH ******/
CREATE DATABASE [tnut_social]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tnut_social', FILENAME = N'D:\sql server\MSSQL12.SQLEXPRESS\MSSQL\DATA\tnut_social.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'tnut_social_log', FILENAME = N'D:\sql server\MSSQL12.SQLEXPRESS\MSSQL\DATA\tnut_social_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [tnut_social] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tnut_social].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tnut_social] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [tnut_social] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [tnut_social] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [tnut_social] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [tnut_social] SET ARITHABORT OFF 
GO
ALTER DATABASE [tnut_social] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [tnut_social] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [tnut_social] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [tnut_social] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [tnut_social] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [tnut_social] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [tnut_social] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [tnut_social] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [tnut_social] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [tnut_social] SET  DISABLE_BROKER 
GO
ALTER DATABASE [tnut_social] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [tnut_social] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [tnut_social] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [tnut_social] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [tnut_social] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [tnut_social] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [tnut_social] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [tnut_social] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [tnut_social] SET  MULTI_USER 
GO
ALTER DATABASE [tnut_social] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [tnut_social] SET DB_CHAINING OFF 
GO
ALTER DATABASE [tnut_social] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [tnut_social] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [tnut_social] SET DELAYED_DURABILITY = DISABLED 
GO
USE [tnut_social]
GO
/****** Object:  Table [dbo].[account]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account](
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NULL,
 CONSTRAINT [PK_account] PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[anh]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[anh](
	[id_image] [int] IDENTITY(1,1) NOT NULL,
	[link] [nvarchar](50) NULL,
	[mo_ta] [nvarchar](500) NULL,
	[id_post] [int] NULL,
 CONSTRAINT [PK_album] PRIMARY KEY CLUSTERED 
(
	[id_image] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[category]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[id_category] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
 CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED 
(
	[id_category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[comment]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comment](
	[id_comment] [int] IDENTITY(1,1) NOT NULL,
	[uid] [nvarchar](20) NOT NULL,
	[id_post] [int] NOT NULL,
	[time] [datetime] NULL CONSTRAINT [DF_comment_time]  DEFAULT (getdate()),
	[noi_dung] [nvarchar](500) NULL,
	[id_parent] [int] NULL,
 CONSTRAINT [PK_comment_1] PRIMARY KEY CLUSTERED 
(
	[id_comment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[chat]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chat](
	[id_chat] [int] IDENTITY(1,1) NOT NULL,
	[noi_dung] [nvarchar](max) NOT NULL,
	[uid] [nvarchar](20) NOT NULL,
	[nguoi_nhan] [nvarchar](20) NOT NULL,
	[time] [datetime] NOT NULL CONSTRAINT [DF_chat_time]  DEFAULT (getdate()),
 CONSTRAINT [PK_chat] PRIMARY KEY CLUSTERED 
(
	[id_chat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[group]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[group](
	[ma_nhom] [int] IDENTITY(1,1) NOT NULL,
	[ten_nhom] [nvarchar](100) NULL,
	[mo_ta] [nvarchar](500) NULL,
	[anh] [nvarchar](100) NULL,
 CONSTRAINT [PK_group] PRIMARY KEY CLUSTERED 
(
	[ma_nhom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[khoa]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[khoa](
	[ma_khoa] [int] IDENTITY(1,1) NOT NULL,
	[ten_khoa] [nvarchar](100) NULL,
 CONSTRAINT [PK_khoa] PRIMARY KEY CLUSTERED 
(
	[ma_khoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[like]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[like](
	[uid] [nvarchar](20) NOT NULL,
	[id_post] [int] NOT NULL,
	[time] [datetime] NULL CONSTRAINT [DF_like_time]  DEFAULT (getdate()),
 CONSTRAINT [PK_like] PRIMARY KEY CLUSTERED 
(
	[uid] ASC,
	[id_post] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lop]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lop](
	[ma_lop] [nvarchar](10) NOT NULL,
	[ten_lop] [nvarchar](100) NULL,
	[ma_nganh] [int] NULL,
 CONSTRAINT [PK_lop] PRIMARY KEY CLUSTERED 
(
	[ma_lop] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mon_hoc]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mon_hoc](
	[ma_mon_hoc] [int] IDENTITY(1,1) NOT NULL,
	[ten_mon_hoc] [nvarchar](50) NULL,
 CONSTRAINT [PK_mon_hoc] PRIMARY KEY CLUSTERED 
(
	[ma_mon_hoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[nganh]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nganh](
	[ma_nganh] [int] IDENTITY(1,1) NOT NULL,
	[ten_nganh] [nvarchar](100) NULL,
	[ma_khoa] [int] NULL,
 CONSTRAINT [PK_nganh] PRIMARY KEY CLUSTERED 
(
	[ma_nganh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[post]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[post](
	[id_post] [int] IDENTITY(1,1) NOT NULL,
	[noi_dung] [nvarchar](max) NULL,
	[uid] [nvarchar](20) NULL,
	[ma_nhom] [int] NULL,
	[ngay_dang] [datetime] NULL CONSTRAINT [DF_post_ngay_dang]  DEFAULT (getdate()),
	[id_category] [int] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_post] PRIMARY KEY CLUSTERED 
(
	[id_post] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[role]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role](
	[id_role] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [nvarchar](50) NULL,
 CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED 
(
	[id_role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tai_lieu]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tai_lieu](
	[id_tai_lieu] [int] IDENTITY(1,1) NOT NULL,
	[ten] [nvarchar](100) NULL,
	[link] [nvarchar](50) NULL,
	[anh] [nvarchar](50) NULL,
	[mo_ta] [nvarchar](500) NULL,
	[ma_mon_hoc] [int] NULL,
	[uid] [nvarchar](20) NULL,
 CONSTRAINT [PK_tai_lieu] PRIMARY KEY CLUSTERED 
(
	[id_tai_lieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[uid] [nvarchar](20) NOT NULL,
	[password] [nvarchar](50) NULL,
	[ho_ten] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[dia_chi] [nvarchar](100) NULL,
	[ma_lop] [nvarchar](10) NULL,
	[role] [nvarchar](10) NULL,
	[anh] [nvarchar](50) NULL,
	[gioi_tinh] [bit] NULL,
	[gioi_thieu] [nvarchar](max) NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user_group]    Script Date: 15/06/2022 9:48:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_group](
	[uid] [nvarchar](20) NOT NULL,
	[ma_nhom] [int] NOT NULL,
	[id_role] [int] NULL CONSTRAINT [DF_user_group_id_role]  DEFAULT ((3)),
 CONSTRAINT [PK_user_group_1] PRIMARY KEY CLUSTERED 
(
	[uid] ASC,
	[ma_nhom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[anh]  WITH CHECK ADD  CONSTRAINT [FK_anh_post] FOREIGN KEY([id_post])
REFERENCES [dbo].[post] ([id_post])
GO
ALTER TABLE [dbo].[anh] CHECK CONSTRAINT [FK_anh_post]
GO
ALTER TABLE [dbo].[comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_comment] FOREIGN KEY([id_parent])
REFERENCES [dbo].[comment] ([id_comment])
GO
ALTER TABLE [dbo].[comment] CHECK CONSTRAINT [FK_comment_comment]
GO
ALTER TABLE [dbo].[comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_post] FOREIGN KEY([id_post])
REFERENCES [dbo].[post] ([id_post])
GO
ALTER TABLE [dbo].[comment] CHECK CONSTRAINT [FK_comment_post]
GO
ALTER TABLE [dbo].[comment]  WITH CHECK ADD  CONSTRAINT [FK_comment_user] FOREIGN KEY([uid])
REFERENCES [dbo].[user] ([uid])
GO
ALTER TABLE [dbo].[comment] CHECK CONSTRAINT [FK_comment_user]
GO
ALTER TABLE [dbo].[chat]  WITH CHECK ADD  CONSTRAINT [FK_chat_user1] FOREIGN KEY([uid])
REFERENCES [dbo].[user] ([uid])
GO
ALTER TABLE [dbo].[chat] CHECK CONSTRAINT [FK_chat_user1]
GO
ALTER TABLE [dbo].[like]  WITH CHECK ADD  CONSTRAINT [FK_like_post] FOREIGN KEY([id_post])
REFERENCES [dbo].[post] ([id_post])
GO
ALTER TABLE [dbo].[like] CHECK CONSTRAINT [FK_like_post]
GO
ALTER TABLE [dbo].[like]  WITH CHECK ADD  CONSTRAINT [FK_like_user] FOREIGN KEY([uid])
REFERENCES [dbo].[user] ([uid])
GO
ALTER TABLE [dbo].[like] CHECK CONSTRAINT [FK_like_user]
GO
ALTER TABLE [dbo].[lop]  WITH CHECK ADD  CONSTRAINT [FK_lop_nganh] FOREIGN KEY([ma_nganh])
REFERENCES [dbo].[nganh] ([ma_nganh])
GO
ALTER TABLE [dbo].[lop] CHECK CONSTRAINT [FK_lop_nganh]
GO
ALTER TABLE [dbo].[nganh]  WITH CHECK ADD  CONSTRAINT [FK_nganh_khoa] FOREIGN KEY([ma_khoa])
REFERENCES [dbo].[khoa] ([ma_khoa])
GO
ALTER TABLE [dbo].[nganh] CHECK CONSTRAINT [FK_nganh_khoa]
GO
ALTER TABLE [dbo].[post]  WITH CHECK ADD  CONSTRAINT [FK_post_category] FOREIGN KEY([id_category])
REFERENCES [dbo].[category] ([id_category])
GO
ALTER TABLE [dbo].[post] CHECK CONSTRAINT [FK_post_category]
GO
ALTER TABLE [dbo].[post]  WITH CHECK ADD  CONSTRAINT [FK_post_user_group] FOREIGN KEY([uid], [ma_nhom])
REFERENCES [dbo].[user_group] ([uid], [ma_nhom])
GO
ALTER TABLE [dbo].[post] CHECK CONSTRAINT [FK_post_user_group]
GO
ALTER TABLE [dbo].[tai_lieu]  WITH CHECK ADD  CONSTRAINT [FK_tai_lieu_mon_hoc] FOREIGN KEY([ma_mon_hoc])
REFERENCES [dbo].[mon_hoc] ([ma_mon_hoc])
GO
ALTER TABLE [dbo].[tai_lieu] CHECK CONSTRAINT [FK_tai_lieu_mon_hoc]
GO
ALTER TABLE [dbo].[tai_lieu]  WITH CHECK ADD  CONSTRAINT [FK_tai_lieu_user] FOREIGN KEY([uid])
REFERENCES [dbo].[user] ([uid])
GO
ALTER TABLE [dbo].[tai_lieu] CHECK CONSTRAINT [FK_tai_lieu_user]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_lop] FOREIGN KEY([ma_lop])
REFERENCES [dbo].[lop] ([ma_lop])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_lop]
GO
ALTER TABLE [dbo].[user_group]  WITH CHECK ADD  CONSTRAINT [FK_user_group_group1] FOREIGN KEY([ma_nhom])
REFERENCES [dbo].[group] ([ma_nhom])
GO
ALTER TABLE [dbo].[user_group] CHECK CONSTRAINT [FK_user_group_group1]
GO
ALTER TABLE [dbo].[user_group]  WITH CHECK ADD  CONSTRAINT [FK_user_group_role] FOREIGN KEY([id_role])
REFERENCES [dbo].[role] ([id_role])
GO
ALTER TABLE [dbo].[user_group] CHECK CONSTRAINT [FK_user_group_role]
GO
ALTER TABLE [dbo].[user_group]  WITH CHECK ADD  CONSTRAINT [FK_user_group_user1] FOREIGN KEY([uid])
REFERENCES [dbo].[user] ([uid])
GO
ALTER TABLE [dbo].[user_group] CHECK CONSTRAINT [FK_user_group_user1]
GO
USE [master]
GO
ALTER DATABASE [tnut_social] SET  READ_WRITE 
GO
