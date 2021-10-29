
SELECT TABLE_SCHEMA + '.' + TABLE_NAME
FROM NorthWind2020.INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
      AND TABLE_NAME <> 'sysdiagrams'
ORDER BY TABLE_NAME;


/*--------------------------------------------------------------------------------------------------------------------------------*/

SELECT STRING_AGG(R.ColumnName, ',') 
FROM
(
    SELECT TOP 100 COLUMN_NAME AS ColumnName FROM INFORMATION_SCHEMA.TABLES AS tbl
         INNER JOIN INFORMATION_SCHEMA.COLUMNS AS col ON col.TABLE_NAME = tbl.TABLE_NAME
         INNER JOIN sys.columns AS sc ON sc.object_id = OBJECT_ID(tbl.TABLE_SCHEMA + '.' + tbl.TABLE_NAME) AND sc.name = col.COLUMN_NAME
         LEFT JOIN sys.extended_properties prop ON prop.major_id = sc.object_id AND prop.minor_id = sc.column_id AND prop.name = 'MS_Description'
    WHERE tbl.TABLE_NAME = 'Products' ORDER BY COLUMN_NAME ) AS r;



SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME <> 'sysdiagrams' ORDER BY TABLE_NAME

/*----For table get column positon, name and description----------------------------------------------------------------------*/
DECLARE @TableName NVARCHAR(50)= 'Customers';
SELECT col.ORDINAL_POSITION AS Position, col.COLUMN_NAME AS ColumnName, prop.value AS Description
FROM INFORMATION_SCHEMA.TABLES AS tbl
     INNER JOIN INFORMATION_SCHEMA.COLUMNS AS col ON col.TABLE_NAME = tbl.TABLE_NAME
     INNER JOIN sys.columns AS sc ON sc.object_id = OBJECT_ID(tbl.TABLE_SCHEMA + '.' + tbl.TABLE_NAME)
                                     AND sc.name = col.COLUMN_NAME
     LEFT OUTER JOIN sys.extended_properties AS prop ON prop.major_id = sc.object_id
                                                        AND prop.minor_id = sc.column_id
                                                        AND prop.name = 'MS_Description'
WHERE tbl.TABLE_NAME = @TableName
      AND prop.value IS NOT NULL
ORDER BY ColumnName;

/*----All tables column details---------------------------------------------------------------------------------------------------*/

SELECT	syso.name [Table],
                sysc.name [Field], 
                sysc.colorder [FieldOrder], 
                syst.name [DataType], 
                sysc.[length] [Length], 
                CASE WHEN sysc.prec IS NULL THEN 'NULL' ELSE CAST(sysc.prec AS VARCHAR) END [Precision],
        CASE WHEN sysc.scale IS null THEN '-' ELSE sysc.scale END [Scale], 
        CASE WHEN sysc.isnullable = 1 THEN 'True' ELSE 'False' END [AllowNulls], 
        CASE WHEN sysc.[status] = 128 THEN 'True' ELSE 'False' END [Identity], 
        CASE WHEN sysc.colstat = 1 THEN 'True' ELSE 'False' END [PrimaryKey],
        CASE WHEN fkc.parent_object_id is NULL THEN 'False' ELSE 'True' END [ForeignKey], 
        CASE WHEN fkc.parent_object_id is null THEN '(none)' ELSE obj.name  END [RelatedTable],
        CASE WHEN ep.value is NULL THEN '(none)' ELSE CAST(ep.value as NVARCHAR(500)) END [Description]
        FROM [sys].[sysobjects] AS syso
        JOIN [sys].[syscolumns] AS sysc on syso.id = sysc.id
        LEFT JOIN [sys].[systypes] AS syst ON sysc.xtype = syst.xtype and syst.name != 'sysname'
        LEFT JOIN [sys].[foreign_key_columns] AS fkc on syso.id = fkc.parent_object_id and 
            sysc.colid = fkc.parent_column_id    
        LEFT JOIN [sys].[objects] AS obj ON fkc.referenced_object_id = obj.[object_id]
        LEFT JOIN [sys].[extended_properties] AS ep ON syso.id = ep.major_id and sysc.colid = 
            ep.minor_id and ep.name = 'MS_Description'
        WHERE syso.type = 'U' AND  syso.name != 'sysdiagrams'
        ORDER BY [Table], FieldOrder, Field;