IF OBJECT_ID('tempdb..#tmpbefore') is not null drop table #tmpbefore
GO
IF OBJECT_ID('tempdb..#tmpmbr') is not null drop table #tmpmbr
GO

WITH doppelte AS (
SELECT MemberId, LastName, FirstName, ROW_NUMBER() OVER 
(PARTITION BY LastName, FirstName ORDER BY LastName) nr
FROM Members
)
SELECT LastName, FirstName 
INTO #tmpbefore
FROM doppelte
WHERE nr > 1 AND LastName <> 'GESUCHT'


SELECT MemberId, LastName, FirstName 
INTO #tmpmbr
FROM Members 
 WHERE LastName + FirstName IN
    (SELECT LastName + FirstName FROM #tmpbefore)        


SELECT * FROM #tmpmbr

DELETE FROM Members
WHERE MemberId IN 
(SELECT MemberId FROM #tmpmbr)