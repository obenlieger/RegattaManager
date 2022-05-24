SELECT RaceCode, Starttime FROM Races
WHERE Starttime > '2022-06-11 14:18:00' AND Starttime <= '2022-06-11 20:00:00'
ORDER BY Starttime

--UPDATE Races SET Starttime = DATEADD(minute,1,Starttime)
--WHERE Starttime > '2022-06-11 14:18:00' AND Starttime <= '2022-06-11 20:00:00'