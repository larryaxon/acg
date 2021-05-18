SELECT OBJECT_NAME (referencing_id) ReferenceObject, referencing_id, 
OBJECT_NAME(d.referenced_id) TableName, referenced_id,  OBJECT_DEFINITION(referencing_id) Definition,
0 HasBeenConverted
INTO LLAFluentstreamBillingConversionList
FROM  sys.sql_expression_dependencies d
WHERE OBJECT_NAME(d.referenced_id) = 'Entity'
      AND OBJECT_DEFINITION(referencing_id) like '%AlternateID%'
ORDER BY OBJECT_NAME (referencing_id) 

--select top 100 * from sys.sql_expression_dependencies

select * from DataSources where FromClause like '%alternateid%'