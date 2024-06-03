SELECT
	NodeID,
	NodeAliasPath,
	NodeName,
	NodeSiteID

FROM CMS_Tree

WHERE NodeID IN
	(SELECT
		NodeParentID
		FROM CMS_Tree
		GROUP BY NodeParentID
		HAVING COUNT(NodeParentID) > 1000)