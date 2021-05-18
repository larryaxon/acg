truncate table cityhostedtest.dbo.entity
INSERT INTO cityhostedtest.dbo.entity
SELECT * from cityhostedprod.dbo.Entity
Truncate Table cityhostedtest.dbo.attribute
INSERT INTO cityhostedtest.dbo.attribute
select * from cityhostedprod.dbo.Attribute
