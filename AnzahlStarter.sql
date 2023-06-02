SELECT c.ShortName, COUNT(DISTINCT sbm.MemberId) FROM StartboatMembers sbm
INNER JOIN Startboats sb ON sb.StartboatId = sbm.StartboatId
INNER JOIN Members m ON m.MemberId = sbm.MemberId
INNER JOIN Clubs c ON c.ClubId = m.ClubId
WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
GROUP BY c.ShortName


SELECT COUNT(DISTINCT sbm.MemberId) FROM StartboatMembers sbm
INNER JOIN Startboats sb ON sb.StartboatId = sbm.StartboatId
INNER JOIN Members m ON m.MemberId = sbm.MemberId
INNER JOIN Clubs c ON c.ClubId = m.ClubId
WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)


SELECT c.ShortName, m.LastName, m.FirstName FROM StartboatMembers sbm
INNER JOIN Startboats sb ON sb.StartboatId = sbm.StartboatId
INNER JOIN Members m ON m.MemberId = sbm.MemberId
INNER JOIN Clubs c ON c.ClubId = m.ClubId
WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
GROUP BY c.ShortName, m.LastName, m.FirstName