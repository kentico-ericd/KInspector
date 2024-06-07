SELECT KeyName AS 'Key', COALESCE(KeyValue, 'Not set') AS 'Value', COALESCE(SiteName, 'Global') AS 'Site'
FROM CMS_SettingsKey
LEFT JOIN CMS_Site ON CMS_Site.SiteID = CMS_SettingsKey.SiteID
WHERE KeyCategoryID IN
	(SELECT CategoryID FROM CMS_SettingsCategory WHERE CategoryName LIKE 'CMS.OnlineMarketing.InactiveContacts%')
	AND 
	EXISTS(SELECT SiteID FROM CMS_SettingsKey WHERE KeyName LIKE 'CMSEnableOnlineMarketing' 
			AND KeyValue = 'True'
			AND COALESCE(SiteID, 0) = COALESCE(CMS_SettingsKey.SiteID, 0))