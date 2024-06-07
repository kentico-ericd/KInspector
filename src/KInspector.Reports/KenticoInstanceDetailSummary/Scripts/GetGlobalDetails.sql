DECLARE @Result nvarchar(MAX) = CONCAT('## ', '<u>Global information</u>')

-- Functions

IF OBJECT_ID (N'KInspector_GetTableRowsCount', N'FN') IS NOT NULL
DROP FUNCTION KInspector_GetTableRowsCount;

EXEC('CREATE FUNCTION KInspector_GetTableRowsCount (@TableName nvarchar(MAX))
RETURNS int
WITH EXECUTE AS CALLER
AS
BEGIN
RETURN(SELECT TOP 1 p.rows FROM
sys.tables t
INNER JOIN
sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN
sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
WHERE
t.is_ms_shipped = 0 AND t.name = @TableName
);
END;
')

-- Row counts

SET @Result += '
### Row counts
'
SET @Result += CONCAT('- Metafiles (global): ',
	(SELECT COUNT(MetaFileID) FROM [CMS_MetaFile] WHERE MetaFileSiteID IS NULL), '
')
SET @Result += CONCAT('- Event log (global): ',
	(SELECT COUNT(EventID) FROM [CMS_EventLog] WHERE SiteID IS NULL), '
')

-- Custom tables

DECLARE @CustomTables TABLE (ID int, Name nvarchar(MAX), TableName nvarchar(MAX))
INSERT INTO @CustomTables SELECT ClassID, ClassName, ClassTableName FROM [CMS_Class] WHERE ClassIsCustomTable = 1

SET @Result += '
### Custom tables
'

DECLARE @CustomTableID int
SELECT @CustomTableID = MIN(ID) FROM @CustomTables
WHILE @CustomTableID is not null
BEGIN
	DECLARE @CTName nvarchar(MAX)
	DECLARE @CTTable nvarchar(MAX)
	SELECT @CTName = Name, @CTTable = TableName FROM @CustomTables WHERE ID = @CustomTableID
	DECLARE @CTRows int;
	EXEC @CTRows = KInspector_GetTableRowsCount @TableName = @CTTable
	SET @Result += CONCAT('- ', @CTTable, ' (', @CTRows, ')
')

	SELECT @CustomTableID = MIN(ID) FROM @CustomTables where ID > @CustomTableID
END

-- Custom modules (classes, settings, UI elements)

IF OBJECT_ID('tempdb..#CustomModules') IS NOT NULL
BEGIN
	DROP TABLE CustomModules
END

CREATE TABLE #CustomModules (ID int, Name nvarchar(MAX), DisplayName nvarchar(MAX))
INSERT INTO #CustomModules SELECT
	ResourceID
	,ResourceName
	,ResourceDisplayName
	FROM [CMS_Resource] WHERE ResourceIsInDevelopment = 1

SET @Result += CONCAT('
### Custom modules (', (SELECT COUNT(ID) FROM #CustomModules), ')',  '
')

DECLARE @CustomModuleID int
SELECT @CustomModuleID = MIN(ID) FROM #CustomModules
WHILE @CustomModuleID is not null
BEGIN
	DECLARE @CMName nvarchar(MAX)
	DECLARE @CMDisplayName nvarchar(MAX)
	DECLARE @CMID int
	SELECT @CMName = Name, @CMID = ID, @CMDisplayName = DisplayName FROM #CustomModules WHERE ID = @CustomModuleID
	-- Header
	SET @Result += CONCAT('- ', @CMDisplayName, ' _(', @CMName, ')_
')
	-- Class count
	SET @Result += CONCAT('  - Classes: ', (SELECT COUNT(ClassID) FROM [CMS_Class] WHERE ClassResourceID = @CMID), '
')
	-- Settings key count
	SET @Result += CONCAT('  - Settings: ', (SELECT COUNT(KeyID) FROM [CMS_SettingsKey] JOIN [CMS_SettingsCategory] ON KeyCategoryID = CategoryID WHERE SiteID IS NULL AND CategoryResourceID = @CMID), '
')
	-- UI element count
	SET @Result += CONCAT('  - UI elements: ', (SELECT COUNT(ElementID) FROM [CMS_UIElement] WHERE ElementResourceID = @CMID), '
')

	SELECT @CustomModuleID = MIN(ID) FROM #CustomModules WHERE ID > @CustomModuleID
END
DROP TABLE #CustomModules

-- Web farms

SET @Result += '
### Web farms
- Mode: '
SET @Result += (SELECT CASE
		WHEN [KeyValue] = 0 THEN 'Disabled'
		WHEN [KeyValue] = 1 THEN 'Automatic'
		WHEN [KeyValue] = 2 THEN 'Manual'
	END
	FROM [CMS_SettingsKey] WHERE KeyName = 'CMSWebFarmMode')

SET @Result += CONCAT('
- Servers (', (SELECT COUNT(ServerID) FROM CMS_WebfarmServer), ')
')

DECLARE @WebFarmServers TABLE (ID int, Name nvarchar(MAX), Ping datetime2, IsEnabled bit)
INSERT INTO @WebFarmServers (ID, Name, Ping, IsEnabled) SELECT
	S.ServerID,
	ServerName,
	MAX(ServerPing),
	ServerEnabled
	FROM [CMS_WebFarmServer] S
	JOIN [CMS_WebFarmServerMonitoring] SM ON S.ServerID = SM.ServerID
	GROUP BY [ServerName], S.ServerID, ServerEnabled

DECLARE @ID int
SELECT @ID = MIN(ID) FROM @WebFarmServers
WHILE @ID is not null
BEGIN
	DECLARE @Name nvarchar(MAX)
	DECLARE @Ping datetime2
	DECLARE @Enabled bit
	SELECT @Name = Name, @Ping = Ping, @Enabled = IsEnabled FROM @WebFarmServers WHERE ID = @ID
	-- ID, Name
	SET @Result += CONCAT('  - [', @ID, '] ', @Name, '
')
	-- Status
	SET @Result += CONCAT('    - Status: ', CASE
			WHEN @Enabled = 1 THEN 'Enabled'
			WHEN @Enabled = 0 THEN 'Disabled'
			ELSE 'Unknown'
		END, '
')
	-- Last ping
	SET @Result += CONCAT('    - Last ping: ', FORMAT(@Ping, 'G'), '
')

	SELECT @ID = MIN(ID) FROM @WebFarmServers WHERE ID > @ID
END

-- Users

SET @Result += CONCAT('
### Users (', (SELECT COUNT(UserID) FROM CMS_User) , ')
')
SET @Result += CONCAT('- Global admins: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserPrivilegeLevel = 3), '
')
SET @Result += CONCAT('- Administrators: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserPrivilegeLevel = 2), '
')
SET @Result += CONCAT('- Editors: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserPrivilegeLevel = 1), '
')
SET @Result += CONCAT('- Live site only: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserPrivilegeLevel = 0), '
')
SET @Result += CONCAT('- Disabled: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserEnabled = 0), '
')
SET @Result += CONCAT('- Hidden: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserIsHidden = 1), '
')
SET @Result += CONCAT('- External: ',
	(SELECT COUNT(UserID) FROM [CMS_User] WHERE UserIsExternal = 1), '
')

-- Finished

SELECT @Result