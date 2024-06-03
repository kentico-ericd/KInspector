SELECT
	Count(EventUrl) AS 'Count',
	EventUrl, 
	MIN(EventTime) AS 'FirstOccurrence',
	EventUrlReferrer AS 'Referrer' 
FROM CMS_EventLog 
WHERE EventCode = 'PAGENOTFOUND' 
GROUP BY EventUrl, EventUrlReferrer 
ORDER BY Count DESC