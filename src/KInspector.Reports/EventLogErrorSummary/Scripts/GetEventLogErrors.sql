SELECT Count(EventDescription) AS Count,
		EventCode,
		Source, 
		EventDescription,
		MIN(EventTime) AS 'EventFirstDate', 
		MAX(EventTime) AS 'EventLastDate' 
FROM CMS_EventLog 
WHERE EventType = 'E' 
GROUP BY Source, EventCode, EventDescription 
ORDER BY Count DESC