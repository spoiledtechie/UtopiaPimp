/* Version 1.0.2 */

/*
** Create missing tables
*/
if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Active]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Active](
		SessionID		nvarchar (24) NOT NULL ,
		BoardID			int NOT NULL ,
		UserID			int NOT NULL ,
		IP				nvarchar (15) NOT NULL ,
		Login			datetime NOT NULL ,
		LastActive		datetime NOT NULL ,
		Location		nvarchar (50) NOT NULL ,
		ForumID			int NULL ,
		TopicID			int NULL ,
		Browser			nvarchar (50) NULL ,
		Platform		nvarchar (50) NULL 
	)
go

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}BannedIP]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}BannedIP](
		ID				int IDENTITY (1, 1) NOT NULL ,
		BoardID			int NOT NULL ,
		Mask			nvarchar (15) NOT NULL ,
		Since			datetime NOT NULL 
	)
go

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Category]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Category](
		CategoryID		int IDENTITY (1, 1) NOT NULL ,
		BoardID			int NOT NULL ,
		[Name]			[nvarchar](128) NOT NULL,
		[CategoryImage] [nvarchar](255) NULL,		
		SortOrder		smallint NOT NULL 
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}CheckEmail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}CheckEmail](
		CheckEmailID	int IDENTITY (1, 1) NOT NULL ,
		UserID			int NOT NULL ,
		Email			nvarchar (50) NOT NULL ,
		Created			datetime NOT NULL ,
		Hash			nvarchar (32) NOT NULL 
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Choice]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Choice](
		ChoiceID		int IDENTITY (1, 1) NOT NULL ,
		PollID			int NOT NULL ,
		Choice			nvarchar (50) NOT NULL ,
		Votes			int NOT NULL 
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}PollVote]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	CREATE TABLE [{databaseOwner}].[{objectQualifier}PollVote] (
		[PollVoteID] [int] IDENTITY (1, 1) NOT NULL ,
		[PollID] [int] NOT NULL ,
		[UserID] [int] NULL ,
		[RemoteIP] [nvarchar] (10) NULL 
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Forum]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Forum](
		ForumID			int IDENTITY (1, 1) NOT NULL ,
		CategoryID		int NOT NULL ,
		ParentID		int NULL ,
		Name			nvarchar (50) NOT NULL ,
		Description		nvarchar (255) NOT NULL ,
		SortOrder		smallint NOT NULL ,
		LastPosted		datetime NULL ,
		LastTopicID		int NULL ,
		LastMessageID	int NULL ,
		LastUserID		int NULL ,
		LastUserName	nvarchar (50) NULL ,
		NumTopics		int NOT NULL,
		NumPosts		int NOT NULL,
		RemoteURL		nvarchar(100) null,
		Flags			int not null constraint [DF_{objectQualifier}Forum_Flags] default (0),
		ThemeURL		nvarchar(50) NULL
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}ForumAccess]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}ForumAccess](
		GroupID			int NOT NULL ,
		ForumID			int NOT NULL ,
		AccessMaskID	int NOT NULL
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Group](
		GroupID			int IDENTITY (1, 1) NOT NULL ,
		BoardID			int NOT NULL ,
		Name			nvarchar (50) NOT NULL ,
		Flags			int not null constraint DF_{objectQualifier}Group_Flags default (0)
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Mail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Mail](
		[MailID] [int] IDENTITY(1,1) NOT NULL,
		[FromUser] [nvarchar](50) NOT NULL,
		[FromUserName] [nvarchar](50) NULL,
		[ToUser] [nvarchar](50) NOT NULL,
		[ToUserName] [nvarchar](50) NULL,
		[Created] [datetime] NOT NULL,
		[Subject] [nvarchar](100) NOT NULL,
		[Body] [ntext] NOT NULL,
		[BodyHtml] [ntext] NULL,
		[SendTries] [int] NOT NULL CONSTRAINT [DF_{objectQualifier}Mail_SendTries]  DEFAULT ((0)),
		[SendAttempt] [datetime] NULL,
		[ProcessID] [int] NULL
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Message]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Message](
		MessageID		int IDENTITY (1, 1) NOT NULL ,
		TopicID			int NOT NULL ,
		ReplyTo			int NULL ,
		Position		int NOT NULL ,
		Indent			int NOT NULL ,
		UserID			int NOT NULL ,
		UserName		nvarchar (50) NULL ,
		Posted			datetime NOT NULL ,
		Message			ntext NOT NULL ,
		IP				nvarchar (15) NOT NULL ,
		Edited			datetime NULL ,
		Flags			int NOT NULL constraint [DF_{objectQualifier}Message_Flags] default (23),
		EditReason      nvarchar (100) NULL ,
		IsModeratorChanged      bit NOT NULL CONSTRAINT [DF_{objectQualifier}Message_IsModeratorChanged] DEFAULT (0),
	    DeleteReason    nvarchar (100)  NULL,
		IsDeleted		AS (CONVERT([bit],sign([Flags]&(8)),0)),
		IsApproved		AS (CONVERT([bit],sign([Flags]&(16)),(0)))
	)
GO

IF NOT EXISTS (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}MessageReported]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	CREATE TABLE [{databaseOwner}].[{objectQualifier}MessageReported](
		[MessageID] [int] NOT NULL,
		[Message] [ntext] NULL,
		[Resolved] [bit] NULL,
		[ResolvedBy] [int] NULL,
		[ResolvedDate] [datetime] NULL
	)
GO

IF NOT EXISTS (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}MessageReportedAudit]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	CREATE TABLE [{databaseOwner}].[{objectQualifier}MessageReportedAudit](
		[LogID] [int] IDENTITY(1,1) NOT NULL,
		[UserID] [int] NULL,
		[MessageID] [int] NULL,
		[Reported] [datetime] NULL
		)
GO


if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}PMessage]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}PMessage](
		PMessageID		int IDENTITY (1, 1) NOT NULL ,
		FromUserID		int NOT NULL ,
		Created			datetime NOT NULL ,
		Subject			nvarchar (100) NOT NULL ,
		Body			ntext NOT NULL,
		Flags			int NOT NULL 
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Poll]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Poll](
		PollID			int IDENTITY (1, 1) NOT NULL ,
		Question		nvarchar (50) NOT NULL,
		Closes datetime NULL 		
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Smiley]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Smiley](
		SmileyID		int IDENTITY (1, 1) NOT NULL ,
		BoardID			int NOT NULL ,
		Code			nvarchar (10) NOT NULL ,
		Icon			nvarchar (50) NOT NULL ,
		Emoticon		nvarchar (50) NULL ,
		SortOrder		tinyint	NOT NULL DEFAULT 0
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Topic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Topic](
		TopicID			int IDENTITY (1, 1) NOT NULL ,
		ForumID			int NOT NULL ,
		UserID			int NOT NULL ,
		UserName		nvarchar (50) NULL ,
		Posted			datetime NOT NULL ,
		Topic			nvarchar (100) NOT NULL ,
		Views			int NOT NULL ,
		Priority		smallint NOT NULL ,
		PollID			int NULL ,
		TopicMovedID	int NULL ,
		LastPosted		datetime NULL ,
		LastMessageID	int NULL ,
		LastUserID		int NULL ,
		LastUserName	nvarchar (50) NULL,
		NumPosts		int NOT NULL,
		Flags			int not null constraint [DF_{objectQualifier}Topic_Flags] default (0),
		IsDeleted		AS (CONVERT([bit],sign([Flags]&(8)),0))
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}User](
		UserID			int IDENTITY (1, 1) NOT NULL ,
		BoardID			int NOT NULL,
		ProviderUserKey	nvarchar(64),
		Name			nvarchar (50) NOT NULL ,
		Password		nvarchar (32) NOT NULL ,
		Email			nvarchar (50) NULL ,
		Joined			datetime NOT NULL ,
		LastVisit		datetime NOT NULL ,
		IP				nvarchar (15) NULL ,
		NumPosts		int NOT NULL ,
		TimeZone		int NOT NULL ,
		Avatar			nvarchar (255) NULL,
		Signature		ntext NULL,
		AvatarImage		image NULL,
		AvatarImageType	nvarchar (50) NULL,
		RankID			[int] NOT NULL,
		Suspended		[datetime] NULL,
		LanguageFile	nvarchar(50) NULL,
		ThemeFile		nvarchar(50) NULL,
		OverrideDefaultThemes	bit NOT NULL CONSTRAINT [DF_{objectQualifier}User_OverrideDefaultThemes] DEFAULT (0),
		[PMNotification] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}User_PMNotification] DEFAULT (1),
		[Flags] [int]	NOT NULL CONSTRAINT [DF_{objectQualifier}User_Flags] DEFAULT (0),
		[Points] [int]	NOT NULL CONSTRAINT [DF_{objectQualifier}User_Points] DEFAULT (0),		
		[IsApproved]	AS (CONVERT([bit],sign([Flags]&(2)),(0))),
		[IsActiveExcluded] AS (CONVERT([bit],sign([Flags]&(16)),(0)))		
)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}WatchForum]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}WatchForum](
		WatchForumID	int IDENTITY (1, 1) NOT NULL ,
		ForumID			int NOT NULL ,
		UserID			int NOT NULL ,
		Created			datetime NOT NULL ,
		LastMail		datetime null
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}WatchTopic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}WatchTopic](
		WatchTopicID	int IDENTITY (1, 1) NOT NULL ,
		TopicID			int NOT NULL ,
		UserID			int NOT NULL ,
		Created			datetime NOT NULL ,
		LastMail		datetime null
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Attachment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Attachment](
		AttachmentID	int IDENTITY (1, 1) not null,
		MessageID		int not null,
		FileName		nvarchar(255) not null,
		Bytes			int not null,
		FileID			int null,
		ContentType		nvarchar(50) null,
		Downloads		int not null,
		FileData		image null
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}UserGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}UserGroup](
		UserID			int NOT NULL,
		GroupID			int NOT NULL
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Rank]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}Rank](
		RankID			int IDENTITY (1, 1) NOT NULL,
		BoardID			int NOT NULL ,
		Name			nvarchar (50) NOT NULL,
		MinPosts		int NULL,
		RankImage		nvarchar (50) NULL,
		Flags			int not null constraint [DF_{objectQualifier}Rank_Flags] default (0)
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}AccessMask]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}AccessMask](
		AccessMaskID	int IDENTITY (1, 1) NOT NULL ,
		BoardID			int NOT NULL ,
		Name			nvarchar(50) NOT NULL ,
		Flags			int not null constraint [DF_{objectQualifier}AccessMask_Flags] default (0)
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}UserForum]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}UserForum](
		UserID			int NOT NULL ,
		ForumID			int NOT NULL ,
		AccessMaskID	int NOT NULL ,
		Invited			datetime NOT NULL ,
		Accepted		bit NOT NULL
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Board]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}Board](
		BoardID			int IDENTITY (1, 1) NOT NULL,
		Name			nvarchar(50) NOT NULL,
		AllowThreaded	bit NOT NULL,
		MembershipAppName nvarchar(255) NULL,
		RolesAppName nvarchar(255) NULL
	)
end
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}NntpServer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}NntpServer](
		NntpServerID	int IDENTITY (1, 1) not null,
		BoardID			int NOT NULL ,
		Name			nvarchar(50) not null,
		Address			nvarchar(100) not null,
		Port			int null,
		UserName		nvarchar(50) null,
		UserPass		nvarchar(50) null
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}NntpForum]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}NntpForum](
		NntpForumID		int IDENTITY (1, 1) not null,
		NntpServerID	int not null,
		GroupName		nvarchar(100) not null,
		ForumID			int not null,
		LastMessageNo	int not null,
		LastUpdate		datetime not null,
		Active			bit not null
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}NntpTopic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	create table [{databaseOwner}].[{objectQualifier}NntpTopic](
		NntpTopicID		int IDENTITY (1, 1) not null,
		NntpForumID		int not null,
		Thread			char(32) not null,
		TopicID			int not null
	)
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}UserPMessage]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}UserPMessage](
		UserPMessageID	int IDENTITY (1, 1) not null,
		UserID			int not null,
		PMessageID		int not null,
		[Flags]			int NOT NULL DEFAULT ((0)),
		[IsRead]		AS (CONVERT([bit],sign([Flags]&(1)),(0))),
		[IsInOutbox]	AS (CONVERT([bit],sign([Flags]&(2)),(0))),
		[IsArchived]	AS (CONVERT([bit],sign([Flags]&(4)),(0)))		
	)
end
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Replace_Words]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}Replace_Words](
		ID				int IDENTITY (1, 1) NOT NULL,
		BoardId			int NOT NULL,
		BadWord			nvarchar (255) NULL ,
		GoodWord		nvarchar (255) NULL ,
		constraint [PK_{objectQualifier}Replace_Words] primary key(ID)
	)
end
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Registry]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}Registry](
		RegistryID		int IDENTITY(1, 1) NOT NULL,
		Name			nvarchar(50) NOT NULL,
		Value			ntext,
		BoardID			int,
		CONSTRAINT [PK_{objectQualifier}Registry] PRIMARY KEY (RegistryID)
	)
end
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}EventLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}EventLog](
		EventLogID	int identity(1,1) not null,
		EventTime	datetime not null constraint [DF_{objectQualifier}EventLog_EventTime] default getdate(),
		UserID		int,
		Source		nvarchar(50) not null,
		Description	ntext not null,
		constraint [PK_{objectQualifier}EventLog] primary key(EventLogID)
	)
end
GO


if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Extension]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [{databaseOwner}].[{objectQualifier}Extension](
		ExtensionID int IDENTITY(1,1) NOT NULL,
		BoardId int NOT NULL,
		Extension nvarchar(10) NOT NULL,
		CONSTRAINT [PK_{objectQualifier}Extension] PRIMARY KEY(ExtensionID)
	)
END
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}BBCode]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}BBCode](
		[BBCodeID] [int] IDENTITY(1,1) NOT NULL,
		[BoardID] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](4000) NULL,
		[OnClickJS] [nvarchar](1000) NULL,
		[DisplayJS] [ntext] NULL,
		[EditJS] [ntext] NULL,
		[DisplayCSS] [ntext] NULL,
		[SearchRegex] [ntext] NULL,
		[ReplaceRegex] [ntext] NULL,
		[Variables] [nvarchar](1000) NULL,
		[UseModule] [bit] NULL,
		[ModuleClass] [nvarchar](255) NULL,		
		[ExecOrder] [int] NOT NULL,
		CONSTRAINT [PK_{objectQualifier}BBCode] PRIMARY KEY (BBCodeID)
	)
end
GO


if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}Medal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}Medal](
		[BoardID] [int] NOT NULL,
		[MedalID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Description] [ntext] NOT NULL,
		[Message] [nvarchar](100) NOT NULL,
		[Category] [nvarchar](50) NULL,
		[MedalURL] [nvarchar](250) NOT NULL,
		[RibbonURL] [nvarchar](250) NULL,
		[SmallMedalURL] [nvarchar](250) NOT NULL,
		[SmallRibbonURL] [nvarchar](250) NULL,
		[SmallMedalWidth] [smallint] NOT NULL,
		[SmallMedalHeight] [smallint] NOT NULL,
		[SmallRibbonWidth] [smallint] NULL,
		[SmallRibbonHeight] [smallint] NULL,
		[SortOrder] [tinyint] NOT NULL CONSTRAINT [DF_{objectQualifier}Medal_DefaultOrder]  DEFAULT ((255)),
		[Flags] [int] NOT NULL CONSTRAINT [DF_{objectQualifier}Medal_Flags]  DEFAULT ((0)),
		CONSTRAINT [PK_{objectQualifier}Medal] PRIMARY KEY CLUSTERED ([MedalID] ASC)
		)
end
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}GroupMedal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}GroupMedal](
		[GroupID] [int] NOT NULL,
		[MedalID] [int] NOT NULL,
		[Message] [nvarchar](100) NULL,
		[Hide] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}GroupMedal_Hide]  DEFAULT ((0)),
		[OnlyRibbon] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}GroupMedal_OnlyRibbon]  DEFAULT ((0)),
		[SortOrder] [tinyint] NOT NULL CONSTRAINT [DF_{objectQualifier}GroupMedal_SortOrder]  DEFAULT ((255))
		)
end
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}UserMedal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	create table [{databaseOwner}].[{objectQualifier}UserMedal](
		[UserID] [int] NOT NULL,
		[MedalID] [int] NOT NULL,
		[Message] [nvarchar](100) NULL,
		[Hide] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}UserMedal_Hide]  DEFAULT ((0)),
		[OnlyRibbon] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}UserMedal_OnlyRibbon]  DEFAULT ((0)),
		[SortOrder] [tinyint] NOT NULL CONSTRAINT [DF_{objectQualifier}UserMedal_SortOrder]  DEFAULT ((255)),
		[DateAwarded] [datetime] NOT NULL
	)
end
GO

if not exists (select 1 from sysobjects where id = object_id(N'[{databaseOwner}].[{objectQualifier}IgnoreUser]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
	CREATE TABLE [{databaseOwner}].[{objectQualifier}IgnoreUser]
	(
		[UserID] int NOT NULL,
		[IgnoredUserID] int NOT NULL
	)
end
GO

/*
** Added columns
*/

-- Mail Table
if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Mail]') and name='FromUserName')
begin
	alter table [{databaseOwner}].[{objectQualifier}Mail] add [FromUserName] [nvarchar](50) NULL
	alter table [{databaseOwner}].[{objectQualifier}Mail] add [ToUserName] [nvarchar](50) NULL
	alter table [{databaseOwner}].[{objectQualifier}Mail] add [BodyHtml] [ntext] NULL		
	alter table [{databaseOwner}].[{objectQualifier}Mail] add [SendTries] [int] NOT NULL CONSTRAINT [DF_{objectQualifier}Mail_SendTries]  DEFAULT ((0))		
	alter table [{databaseOwner}].[{objectQualifier}Mail] add [SendAttempt] [datetime] NULL
	alter table [{databaseOwner}].[{objectQualifier}Mail] add [ProcessID] [int] NULL
end
GO

-- Board Table
if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Board]') and name='MembershipAppName')
begin
	alter table [{databaseOwner}].[{objectQualifier}Board] add MembershipAppName nvarchar(255)
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Board]') and name='RolesAppName')
begin
	alter table [{databaseOwner}].[{objectQualifier}Board] add RolesAppName nvarchar(255)
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Board]') and name='MembershipAppName')
begin
	alter table [{databaseOwner}].[{objectQualifier}Board] add MembershipAppName nvarchar(255)
end
GO

-- UserPMessage Table
if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}UserPMessage]') and name='Flags')
begin
	-- add new "Flags" field to UserPMessage
	alter table [{databaseOwner}].[{objectQualifier}UserPMessage] add Flags int not null DEFAULT ((0))
end
GO

if exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}UserPMessage]') and name='IsRead')
BEGIN
	if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}UserPMessage]') and name='IsArchived')
	BEGIN	
		-- Copy "IsRead" value over
		grant update on [{databaseOwner}].[{objectQualifier}UserPMessage] to public
		exec('update [{databaseOwner}].[{objectQualifier}UserPMessage] set Flags = IsRead')
		revoke update on [{databaseOwner}].[{objectQualifier}UserPMessage] from public
		
		-- drop the old column
		alter table [{databaseOwner}].[{objectQualifier}UserPMessage] drop column IsRead
		
		-- Verify flags isn't NULL
		grant update on [{databaseOwner}].[{objectQualifier}UserPMessage] to public
		exec('update [{databaseOwner}].[{objectQualifier}UserPMessage] set Flags = 1 WHERE Flags IS NULL')
		revoke update on [{databaseOwner}].[{objectQualifier}UserPMessage] from public
		
		-- add new calculated columns	
		alter table [{databaseOwner}].[{objectQualifier}UserPMessage] ADD [IsRead] AS (CONVERT([bit],sign([Flags]&(1)),(0)))
		alter table [{databaseOwner}].[{objectQualifier}UserPMessage] ADD [IsInOutbox] AS (CONVERT([bit],sign([Flags]&(2)),(0)))
		alter table [{databaseOwner}].[{objectQualifier}UserPMessage] ADD [IsArchived] AS (CONVERT([bit],sign([Flags]&(4)),(0)))
	END
END
GO

-- User Table
if exists(select 1 from [{databaseOwner}].[{objectQualifier}Group] where (Flags & 2)=2)
begin
	update [{databaseOwner}].[{objectQualifier}User] set Flags = Flags | 4 where UserID in(select distinct UserID from [{databaseOwner}].[{objectQualifier}UserGroup] a join [{databaseOwner}].[{objectQualifier}Group] b on b.GroupID=a.GroupID and (b.Flags & 2)=2)
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}User]') and name='IsApproved')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] ADD [IsApproved] AS (CONVERT([bit],sign([Flags]&(2)),(0)))
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}User]') and name='IsActiveExcluded')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] ADD [IsActiveExcluded] AS (CONVERT([bit],sign([Flags]&(16)),(0)))
end
GO

if exists(select 1 from dbo.syscolumns where id = object_id(N'[{databaseOwner}].[{objectQualifier}User]') and name=N'Signature' and xtype<>99)
	alter table [{databaseOwner}].[{objectQualifier}User] alter column Signature ntext null
go

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Flags')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] add Flags int not null constraint DF_{objectQualifier}User_Flags default (0)
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='IsHostAdmin')
begin
	grant update on [{databaseOwner}].[{objectQualifier}User] to public
	exec('update [{databaseOwner}].[{objectQualifier}User] set Flags = Flags | 1 where IsHostAdmin<>0')
	revoke update on [{databaseOwner}].[{objectQualifier}User] from public
	alter table [{databaseOwner}].[{objectQualifier}User] drop column IsHostAdmin
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Approved')
begin
	grant update on [{databaseOwner}].[{objectQualifier}User] to public
	exec('update [{databaseOwner}].[{objectQualifier}User] set Flags = Flags | 2 where Approved<>0')
	revoke update on [{databaseOwner}].[{objectQualifier}User] from public
	alter table [{databaseOwner}].[{objectQualifier}User] drop column Approved
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='ProviderUserKey')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] add ProviderUserKey nvarchar(64)
end
GO

-- convert uniqueidentifier to nvarchar(64)
if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='ProviderUserKey' and xtype='36')
begin
	-- drop the provider user key index if it exists...
	if exists(select 1 from dbo.sysindexes where name=N'IX_{objectQualifier}User_ProviderUserKey' and id=object_id(N'[{databaseOwner}].[{objectQualifier}User]'))
	begin
		DROP INDEX [{databaseOwner}].[{objectQualifier}User].[IX_{objectQualifier}User_ProviderUserKey]
	end
	-- alter the column
	ALTER TABLE [{databaseOwner}].[{objectQualifier}User] ALTER COLUMN ProviderUserKey nvarchar(64)
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='PMNotification')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] add [PMNotification] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}User_PMNotification] DEFAULT (1)
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='OverrideDefaultThemes')
begin
alter table [{databaseOwner}].[{objectQualifier}User] add  [OverrideDefaultThemes]	bit NOT NULL CONSTRAINT [DF_{objectQualifier}User_OverrideDefaultThemes] DEFAULT (0)
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Points')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] add [Points] [int] NOT NULL CONSTRAINT [DF_{objectQualifier}User_Points] DEFAULT (0)
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='AvatarImageType')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] add [AvatarImageType] nvarchar(50) NULL
end
GO

-- make sure the gender column is nullable
if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Gender')
begin
	alter table [{databaseOwner}].[{objectQualifier}User] alter column Gender tinyint NULL
end
GO

-- Only remove User table columns if version is 30+
IF EXISTS (SELECT ver FROM (SELECT CAST(CAST(value as nvarchar(255)) as int) as ver FROM [{databaseOwner}].[{objectQualifier}Registry] WHERE name = 'version') reg WHERE ver > 30)
BEGIN
	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Gender')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column Gender
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Location')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column Location
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='HomePage')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column HomePage
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='MSN')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column MSN
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='YIM')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column YIM
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='AIM')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column AIM
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='ICQ')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column ICQ
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='RealName')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column RealName
	end

	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Occupation')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column Occupation
	end
	
	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Interests')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column Interests
	end
	
	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='Weblog')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column Weblog
	end
	
	if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}User]') and name='WeblogUrl')
	begin
		alter table [{databaseOwner}].[{objectQualifier}User] drop column WeblogUrl
		alter table [{databaseOwner}].[{objectQualifier}User] drop column WeblogUsername
		alter table [{databaseOwner}].[{objectQualifier}User] drop column WeblogID
	end
END
GO

-- Forum Table
if not exists(select * from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='RemoteURL')
	alter table [{databaseOwner}].[{objectQualifier}Forum] add RemoteURL nvarchar(100) null
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='Flags')
	alter table [{databaseOwner}].[{objectQualifier}Forum] add Flags int not null constraint DF_{objectQualifier}Forum_Flags default (0)
GO

if not exists(select * from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='ThemeURL')
	alter table [{databaseOwner}].[{objectQualifier}Forum] add ThemeURL nvarchar(50) NULL
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='Locked')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Forum] set Flags = Flags | 1 where Locked<>0')
	alter table [{databaseOwner}].[{objectQualifier}Forum] drop column Locked
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='Hidden')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Forum] set Flags = Flags | 2 where Hidden<>0')
	alter table [{databaseOwner}].[{objectQualifier}Forum] drop column Hidden
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='IsTest')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Forum] set Flags = Flags | 4 where IsTest<>0')
	alter table [{databaseOwner}].[{objectQualifier}Forum] drop column IsTest
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Forum]') and name='Moderated')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Forum] set Flags = Flags | 8 where Moderated<>0')
	alter table [{databaseOwner}].[{objectQualifier}Forum] drop column Moderated
end
GO

-- Group Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Group]') and name='Flags')
begin
	alter table [{databaseOwner}].[{objectQualifier}Group] add Flags int not null constraint [DF_{objectQualifier}Group_Flags] default (0)
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Group]') and name='IsAdmin')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Group] set Flags = Flags | 1 where IsAdmin<>0')
	alter table [{databaseOwner}].[{objectQualifier}Group] drop column IsAdmin
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Group]') and name='IsGuest')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Group] set Flags = Flags | 2 where IsGuest<>0')
	alter table [{databaseOwner}].[{objectQualifier}Group] drop column IsGuest
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Group]') and name='IsStart')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Group] set Flags = Flags | 4 where IsStart<>0')
	alter table [{databaseOwner}].[{objectQualifier}Group] drop column IsStart
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Group]') and name='IsModerator')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Group] set Flags = Flags | 8 where IsModerator<>0')
	alter table [{databaseOwner}].[{objectQualifier}Group] drop column IsModerator
end
GO

-- AccessMask Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='Flags')
begin
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] add Flags int not null constraint [DF_{objectQualifier}AccessMask_Flags] default (0)
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='ReadAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 1 where ReadAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column ReadAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='PostAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 2 where PostAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column PostAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='ReplyAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 4 where ReplyAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column ReplyAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='PriorityAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 8 where PriorityAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column PriorityAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='PollAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 16 where PollAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column PollAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='VoteAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 32 where VoteAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column VoteAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='ModeratorAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 64 where ModeratorAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column ModeratorAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='EditAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 128 where EditAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column EditAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='DeleteAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 256 where DeleteAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column DeleteAccess
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}AccessMask]') and name='UploadAccess')
begin
	exec('update [{databaseOwner}].[{objectQualifier}AccessMask] set Flags = Flags | 512 where UploadAccess<>0')
	alter table [{databaseOwner}].[{objectQualifier}AccessMask] drop column UploadAccess
end
GO

-- NntpForum Table

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}NntpForum]') and name='Active')
begin
	alter table [{databaseOwner}].[{objectQualifier}NntpForum] add Active bit null
	exec('update [{databaseOwner}].[{objectQualifier}NntpForum] set Active=1 where Active is null')
	alter table [{databaseOwner}].[{objectQualifier}NntpForum] alter column Active bit not null
end
GO

-- ReplaceWords Table
if exists (select * from dbo.syscolumns where id = object_id(N'[{databaseOwner}].[{objectQualifier}Replace_Words]') and name='badword' and prec < 255)
 	alter table [{databaseOwner}].[{objectQualifier}Replace_Words] alter column badword nvarchar(255) NULL
GO

if exists (select * from dbo.syscolumns where id = object_id(N'[{databaseOwner}].[{objectQualifier}Replace_Words]') and name='goodword' and prec < 255)
	alter table [{databaseOwner}].[{objectQualifier}Replace_Words] alter column goodword nvarchar(255) NULL
GO	

if not exists(select 1 from syscolumns where id=object_id(N'[{databaseOwner}].[{objectQualifier}Replace_Words]') and name='BoardID')
begin
	alter table [{databaseOwner}].[{objectQualifier}Replace_Words] add BoardID int not null constraint [DF_{objectQualifier}Replace_Words_BoardID] default (1)
end
GO

-- BBCode Table
if not exists(select 1 from syscolumns where id=object_id(N'[{databaseOwner}].[{objectQualifier}BBCode]') and name='UseModule')
begin
	alter table [{databaseOwner}].[{objectQualifier}BBCode] add UseModule bit null
	alter table [{databaseOwner}].[{objectQualifier}BBCode] add ModuleClass nvarchar(255) null
end
GO

-- Registry Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Registry]') and name='BoardID')
	alter table [{databaseOwner}].[{objectQualifier}Registry] add BoardID int
GO

if exists(select 1 from syscolumns where id=object_id(N'[{databaseOwner}].[{objectQualifier}Registry]') and name=N'Value' and xtype<>99)
	alter table [{databaseOwner}].[{objectQualifier}Registry] alter column Value ntext null
GO

-- PMessage Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}PMessage]') and name='Flags')
begin
	alter table [{databaseOwner}].[{objectQualifier}PMessage] add Flags int not null constraint [DF_{objectQualifier}Message_Flags] default (23)
end
GO

-- Message Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Topic]') and name='Flags')
begin
	alter table [{databaseOwner}].[{objectQualifier}Topic] add Flags int not null constraint [DF_{objectQualifier}Topic_Flags] default (0)
	update [{databaseOwner}].[{objectQualifier}Message] set Flags = Flags & 7
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='Approved')
begin
	exec('update [{databaseOwner}].[{objectQualifier}Message] set Flags = Flags | 16 where Approved<>0')
	alter table [{databaseOwner}].[{objectQualifier}Message] drop column Approved
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='BlogPostID')
begin
	alter table [{databaseOwner}].[{objectQualifier}Message] add BlogPostID nvarchar(50)
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='IsDeleted')
begin
	alter table [{databaseOwner}].[{objectQualifier}Message] ADD [IsDeleted] AS (CONVERT([bit],sign([Flags]&(8)),(0)))
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='IsApproved')
begin
	alter table [{databaseOwner}].[{objectQualifier}Message] ADD [IsApproved] AS (CONVERT([bit],sign([Flags]&(16)),(0)))
end
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='EditReason')
	alter table [{databaseOwner}].[{objectQualifier}Message] add EditReason nvarchar (100) NULL
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='IsModeratorChanged')
	alter table [{databaseOwner}].[{objectQualifier}Message] add 	IsModeratorChanged      bit NOT NULL CONSTRAINT [DF_{objectQualifier}Message_IsModeratorChanged] DEFAULT (0)
GO

if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Message]') and name='DeleteReason')
	alter table [{databaseOwner}].[{objectQualifier}Message] add DeleteReason            nvarchar (100)  NULL
GO

-- Topic Table
if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Topic]') and name='IsLocked')
begin
	grant update on [{databaseOwner}].[{objectQualifier}Topic] to public
	exec('update [{databaseOwner}].[{objectQualifier}Topic] set Flags = Flags | 1 where IsLocked<>0')
	revoke update on [{databaseOwner}].[{objectQualifier}Topic] from public
	alter table [{databaseOwner}].[{objectQualifier}Topic] drop column IsLocked
end
GO

if not exists (select 1 from dbo.syscolumns where id = object_id('[{databaseOwner}].[{objectQualifier}Topic]') and name='IsDeleted')
begin
	alter table [{databaseOwner}].[{objectQualifier}Topic] ADD [IsDeleted] AS (CONVERT([bit],sign([Flags]&(8)),(0)))
end
GO

-- Rank Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Rank]') and name='Flags')
begin
	alter table [{databaseOwner}].[{objectQualifier}Rank] add Flags int not null constraint [DF_{objectQualifier}Rank_Flags] default (0)
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Rank]') and name='IsStart')
begin
	grant update on [{databaseOwner}].[{objectQualifier}Rank] to public
	exec('update [{databaseOwner}].[{objectQualifier}Rank] set Flags = Flags | 1 where IsStart<>0')
	revoke update on [{databaseOwner}].[{objectQualifier}Rank] from public
	alter table [{databaseOwner}].[{objectQualifier}Rank] drop column IsStart
end
GO

if exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Rank]') and name='IsLadder')
begin
	grant update on [{databaseOwner}].[{objectQualifier}Rank] to public
	exec('update [{databaseOwner}].[{objectQualifier}Rank] set Flags = Flags | 2 where IsLadder<>0')
	revoke update on [{databaseOwner}].[{objectQualifier}Rank] from public
	alter table [{databaseOwner}].[{objectQualifier}Rank] drop column IsLadder
end
GO

-- Poll Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Poll]') and name='Closes')
begin
	alter table [{databaseOwner}].[{objectQualifier}Poll] add Closes datetime null
end
GO

-- EventLog Table
if not exists(select 1 from dbo.syscolumns where id = object_id(N'[{databaseOwner}].[{objectQualifier}EventLog]') and name=N'Type')
begin
	alter table [{databaseOwner}].[{objectQualifier}EventLog] add Type int not null constraint [DF_{objectQualifier}EventLog_Type] default (0)
	exec('update [{databaseOwner}].[{objectQualifier}EventLog] set Type = 0')
end
GO

if exists(select 1 from dbo.syscolumns where id = object_id(N'[{databaseOwner}].[{objectQualifier}EventLog]') and name=N'UserID' and isnullable=0)
	alter table [{databaseOwner}].[{objectQualifier}EventLog] alter column UserID int null
GO

-- Smiley Table
if not exists(select 1 from syscolumns where id=object_id('[{databaseOwner}].[{objectQualifier}Smiley]') and name='SortOrder')
begin
	alter table [{databaseOwner}].[{objectQualifier}Smiley] add SortOrder tinyint NOT NULL DEFAULT 0
end
GO

-- Category Table
IF NOT EXISTS (SELECT 1 FROM dbo.syscolumns WHERE id = Object_id(N'[{databaseOwner}].[{objectQualifier}Category]') AND name = N'CategoryImage')
BEGIN
    ALTER TABLE [{databaseOwner}].[{objectQualifier}Category] ADD [CategoryImage] [nvarchar](255) NULL
END
GO
