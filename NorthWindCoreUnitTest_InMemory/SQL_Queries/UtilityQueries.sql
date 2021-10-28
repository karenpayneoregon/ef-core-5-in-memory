﻿
SELECT TABLE_SCHEMA + '.' + TABLE_NAME
FROM NorthWind2020.INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
      AND TABLE_NAME <> 'sysdiagrams'
ORDER BY TABLE_NAME;


SELECT STRING_AGG(R.ColumnName, ',') 
FROM
(
    SELECT TOP 100 COLUMN_NAME AS ColumnName FROM INFORMATION_SCHEMA.TABLES AS tbl
         INNER JOIN INFORMATION_SCHEMA.COLUMNS AS col ON col.TABLE_NAME = tbl.TABLE_NAME
         INNER JOIN sys.columns AS sc ON sc.object_id = OBJECT_ID(tbl.TABLE_SCHEMA + '.' + tbl.TABLE_NAME) AND sc.name = col.COLUMN_NAME
         LEFT JOIN sys.extended_properties prop ON prop.major_id = sc.object_id AND prop.minor_id = sc.column_id AND prop.name = 'MS_Description'
    WHERE tbl.TABLE_NAME = 'Products' ORDER BY COLUMN_NAME ) AS r;



SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME <> 'sysdiagrams' ORDER BY TABLE_NAME