USE [UserManagement]
GO
/****** Object:  Table [dbo].[Managers]    Script Date: 5/31/2024 2:01:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Managers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[ZipCode] [nvarchar](10) NOT NULL,
	[PhoneNumber] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[IdCardPassportNumber] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](100) NOT NULL,
	[RoleType] [nvarchar](10) NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/31/2024 2:01:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[ZipCode] [nvarchar](10) NOT NULL,
	[PhoneNumber] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[IdCardPassportNumber] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](100) NOT NULL,
	[RoleType] [nvarchar](10) NOT NULL,
	[IsDeleted] [bit] NULL,
	[IsEmail] [bit] NULL,
	[OTP] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Managers] ON 

INSERT [dbo].[Managers] ([Id], [FirstName], [LastName], [DateOfBirth], [Gender], [Address], [ZipCode], [PhoneNumber], [Email], [Username], [IdCardPassportNumber], [PasswordHash], [RoleType], [IsDeleted]) VALUES (2, N'Admin', N'admin', CAST(N'2000-01-01' AS Date), N'Male', N'admin', N'admin', N'0', N'admin@gamil.com', N'admin', N'0', N'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', N'Admin', 0)
INSERT [dbo].[Managers] ([Id], [FirstName], [LastName], [DateOfBirth], [Gender], [Address], [ZipCode], [PhoneNumber], [Email], [Username], [IdCardPassportNumber], [PasswordHash], [RoleType], [IsDeleted]) VALUES (13, N'Huynh Doan', N'Duc Sieu', CAST(N'2003-05-26' AS Date), N'Male', N'ST', N'12345', N'0366230497', N'Ducsieuda2@gmail.com', N'sieustaff', N'11111111111111', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'HRM', 0)
SET IDENTITY_INSERT [dbo].[Managers] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DateOfBirth], [Gender], [Address], [ZipCode], [PhoneNumber], [Email], [Username], [IdCardPassportNumber], [PasswordHash], [RoleType], [IsDeleted], [IsEmail], [OTP]) VALUES (30, N'Huỳnh Đoàn', N'Đức Siêu 2', CAST(N'2003-05-26' AS Date), N'Male', N'ST', N'12345', N'0366230497', N'Ducsieuda2@gmail.com', N'sieu', N'2111111111111111111111111', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'User', 0, 1, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Managers] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsEmail]
GO
