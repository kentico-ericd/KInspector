-- File parent folder path is used to group items
-- This path is calculated from FilePath field by removing the file part
-- Example: 
-- /somefolder/subfolder/filename.ext -> /somefolder/subfolder
-- /filename.ext -> '' (empty string)

SELECT LibraryDisplayName,
	COALESCE(NULLIF(SUBSTRING('/' + f.FilePath, 0, LEN('/' + f.FilePath) - CHARINDEX('/', REVERSE('/' + f.FilePath)) + 1), ''), '/') AS 'FolderName',
	SiteName,
	COUNT(*) AS 'NumberOfFiles'
FROM Media_File AS F
INNER JOIN Media_Library AS L ON F.FileLibraryID = L.LibraryID 
INNER JOIN CMS_Site AS S ON S.SiteID = L.LibrarySiteID
GROUP BY SUBSTRING('/' + F.FilePath, 0, LEN('/' + F.FilePath) - CHARINDEX('/', REVERSE('/' + F.FilePath)) + 1), LibraryDisplayName, SiteName
HAVING COUNT(*) > 100
ORDER BY LibraryDisplayName