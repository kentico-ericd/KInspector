SELECT 
	ChannelID as Id,
	ChannelName as Name,
	ChannelGUID as Guid,
	WebsiteChannelDomain as DomainName,
    'RUNNING' as Status
FROM CMS_Channel C
JOIN CMS_WebsiteChannel W ON C.ChannelID = W.WebsiteChannelChannelID