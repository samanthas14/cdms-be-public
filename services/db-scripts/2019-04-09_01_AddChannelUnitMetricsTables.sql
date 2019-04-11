CREATE TABLE [dbo].[ChannelUnitMetrics_Detail] (
    [Id] [int] NOT NULL IDENTITY,
    [ChUnitID] [int],
    [ChUnitNum] [int],
    [Tier1] [int],
    [Tier2] [int],
    [AreaTotal] [decimal](18, 2),
    [PolyArea] [decimal](18, 2),
    [TotalVol] [decimal](18, 2),
    [DpthMax] [decimal](18, 2),
    [DpthThlwgExit] [decimal](18, 2),
    [DpthResid] [decimal](18, 2),
    [CountOfLWD] [int],
    [RowId] [int] NOT NULL,
    [RowStatusId] [int] NOT NULL,
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [QAStatusId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.ChannelUnitMetrics_Detail] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[ChannelUnitMetrics_Detail]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[ChannelUnitMetrics_Detail]([ByUserId])
CREATE INDEX [IX_QAStatusId] ON [dbo].[ChannelUnitMetrics_Detail]([QAStatusId])
CREATE TABLE [dbo].[ChannelUnitMetrics_Header] (
    [Id] [int] NOT NULL IDENTITY,
    [ProgramSiteID] [int],
    [SiteName] [nvarchar](max),
    [WatershedID] [int],
    [WatershedName] [nvarchar](max),
    [SampleDate] [nvarchar](max),
    [HitchName] [nvarchar](max),
    [CrewName] [nvarchar](max),
    [VisitYear] [int],
    [IterationID] [int],
    [CategoryName] [nvarchar](max),
    [PanelName] [nvarchar](max),
    [VisitID] [int],
    [VisitDate] [int],
    [ActivityId] [int] NOT NULL,
    [ByUserId] [int] NOT NULL,
    [EffDt] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.ChannelUnitMetrics_Header] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ActivityId] ON [dbo].[ChannelUnitMetrics_Header]([ActivityId])
CREATE INDEX [IX_ByUserId] ON [dbo].[ChannelUnitMetrics_Header]([ByUserId])
ALTER TABLE [dbo].[ChannelUnitMetrics_Detail] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Detail_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Detail] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Detail_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Detail] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Detail_dbo.QAStatus_QAStatusId] FOREIGN KEY ([QAStatusId]) REFERENCES [dbo].[QAStatus] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Header] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Header_dbo.Activities_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
ALTER TABLE [dbo].[ChannelUnitMetrics_Header] ADD CONSTRAINT [FK_dbo.ChannelUnitMetrics_Header_dbo.Users_ByUserId] FOREIGN KEY ([ByUserId]) REFERENCES [dbo].[Users] ([Id])
