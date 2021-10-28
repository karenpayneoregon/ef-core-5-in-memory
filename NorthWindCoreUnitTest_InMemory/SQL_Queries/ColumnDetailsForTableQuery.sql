DECLARE @TableName NVARCHAR(50)= 'Customers';
SELECT col.ORDINAL_POSITION AS Position, col.COLUMN_NAME AS ColumnName, prop.value AS Description
FROM INFORMATION_SCHEMA.TABLES AS tbl
     INNER JOIN INFORMATION_SCHEMA.COLUMNS AS col ON col.TABLE_NAME = tbl.TABLE_NAME
     INNER JOIN sys.columns AS sc ON sc.object_id = OBJECT_ID(tbl.TABLE_SCHEMA + '.' + tbl.TABLE_NAME) AND sc.name = col.COLUMN_NAME
     LEFT OUTER JOIN sys.extended_properties AS prop ON prop.major_id = sc.object_id AND prop.minor_id = sc.column_id AND prop.name = 'MS_Description'
WHERE tbl.TABLE_NAME = @TableName AND prop.value IS NOT NULL 
ORDER BY Position; --- or, by ColumnName