DECLARE @SiteName nvarchar(MAX) = (SELECT [SiteName] FROM [CMS_Site] WHERE SiteID = @SiteId)
DECLARE @SiteStatus nvarchar(20) = (SELECT [SiteStatus] FROM [CMS_Site] WHERE SiteID = @SiteId)
DECLARE @Result nvarchar(MAX) = CONCAT('## ', @SiteName, ' (', LOWER(@SiteStatus), ')')

-- Assigned cultures

SET @Result += '
### Cultures
'
SET @Result += COALESCE((SELECT STRING_AGG(CAST(CONCAT('- ', [CultureName]) as nvarchar(MAX)), '
')
	FROM [CMS_SiteCulture] AS SC
	JOIN [CMS_Culture] AS C ON C.[CultureID] = SC.CultureID
	WHERE SC.SiteID = @SiteId)
, '_(None)_')

-- Domains and licenses

IF OBJECT_ID('tempdb..#SiteDomains') IS NOT NULL
BEGIN
	DROP TABLE #SiteDomains
END
CREATE TABLE #SiteDomains (ID int, Name nvarchar(MAX), License nvarchar(MAX))
INSERT INTO #SiteDomains SELECT
	0,
	[SiteDomainName],
	[LicenseEdition]
	FROM [CMS_Site] LEFT JOIN [CMS_LicenseKey] ON LicenseDomain = SiteDomainName  WHERE SiteID = @SiteId
INSERT INTO #SiteDomains SELECT
	[SiteDomainAliasID],
	[SiteDomainAliasName],
	[LicenseEdition]
	FROM [CMS_SiteDomainAlias] LEFT JOIN [CMS_LicenseKey] ON LicenseDomain = SiteDomainAliasName WHERE SiteID = @SiteId
SET @Result += '
### Domains and licenses
'
SET @Result += COALESCE((SELECT (STRING_AGG(CAST(CONCAT('- [',
	CASE
		WHEN [License] = 'X' THEN 'EMS'
		WHEN [License] = 'V' THEN 'Ultimate'
		WHEN [License] = 'N' THEN 'Small business'
		WHEN [License] = 'B' THEN 'Base'
		WHEN [License] = 'F' THEN 'Free'
		WHEN [License] IS NULL THEN 'None'
		ELSE [License]
	END
, '] ', [Name]) as nvarchar(MAX)), '
')) FROM #SiteDomains)
, '_(None)_')

-- Row counts

SET @Result += '
### Row counts
'
SET @Result += CONCAT('- Nodes: ',
	(SELECT COUNT(NodeID) FROM [CMS_Tree] WHERE NodeSiteID = @SiteID), '
')
SET @Result += CONCAT('- Pages: ',
	(SELECT COUNT(DocumentID) FROM [CMS_Document] JOIN [CMS_Tree] ON [DocumentNodeID] = [NodeID]  WHERE NodeSiteID = @SiteId), '
')
SET @Result += CONCAT('- Attachments: ',
	(SELECT COUNT(AttachmentID) FROM [CMS_Attachment] WHERE AttachmentSiteID = @SiteID), '
')
SET @Result += CONCAT('- Metafiles: ',
	(SELECT COUNT(MetaFileID) FROM [CMS_MetaFile] WHERE MetaFileSiteID = @SiteID), '
')
SET @Result += CONCAT('- Page aliases: ',
	(SELECT COUNT(AliasID) FROM [CMS_DocumentAlias] WHERE AliasSiteID = @SiteID), '
')
SET @Result += CONCAT('- Users: ',
	(SELECT COUNT(UserID) FROM [CMS_UserSite] WHERE SiteID = @SiteID), '
')
SET @Result += CONCAT('- Event log: ',
	(SELECT COUNT(EventID) FROM [CMS_EventLog] WHERE SiteID = @SiteID), '
')

-- Stylesheets

SET @Result += '
### Stylesheets
'

SET @Result += COALESCE((SELECT STRING_AGG(CAST(CONCAT('- ', [StylesheetDisplayName], ' _(', [StylesheetName], ')_') as nvarchar(MAX)), '
')
	FROM [CMS_CssStylesheet] S
	JOIN CMS_CssStylesheetSite SS ON S.StylesheetID = SS.StylesheetID 
	WHERE SiteID = @SiteId)
, '_(None)_')

-- Page types

SET @Result += '
### Page types
'

SET @Result += COALESCE((SELECT STRING_AGG(CAST(CONCAT('- ', [ClassDisplayName], ' _(', [ClassName], ')_') as nvarchar(MAX)), '
')
	FROM [CMS_Class] C
	JOIN [CMS_ClassSite] CS ON C.[ClassID] = CS.[ClassID] 
	WHERE CS.SiteID = @SiteId AND ClassIsDocumentType = 1)
, '_(None)_')

-- Custom tables

SET @Result += '
### Custom tables
'

SET @Result += COALESCE((SELECT STRING_AGG(CAST(CONCAT('- ', [ClassDisplayName], ' _(', [ClassName], ')_') as nvarchar(MAX)), '
')
	FROM [CMS_Class] C
	JOIN [CMS_ClassSite] CS ON C.[ClassID] = CS.[ClassID] 
	WHERE CS.SiteID = @SiteId AND ClassIsCustomTable = 1)
, '_(None)_')

-- Forms

SET @Result += '
### Forms
'

SET @Result += COALESCE((SELECT STRING_AGG(CAST(CONCAT('- ', [FormDisplayName], ' _(', [ClassName], ')_') as nvarchar(MAX)), '
')
	FROM [CMS_Form]
	JOIN [CMS_Class] ON ClassID = FormClassID
	WHERE [FormSiteID] = @SiteId)
, '_(None)_')

-- Custom modules

SET @Result += '
### Custom modules
'

SET @Result += COALESCE((SELECT STRING_AGG(CAST(CONCAT('- ', [ResourceDisplayName], ' (', [ResourceName], ')') as nvarchar(MAX)), '
')
	FROM [CMS_Resource] R
	JOIN CMS_ResourceSite RS ON R.ResourceID = RS.ResourceID 
	WHERE SiteID = @SiteId AND ResourceIsInDevelopment = 1)
, '_(None)_')

-- Finished

SELECT @Result