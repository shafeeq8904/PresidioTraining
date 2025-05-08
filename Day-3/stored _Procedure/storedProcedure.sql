--creating the stored procedure
create procedure proc_FirstProcedure
as
begin
	print 'this is the first stored procedure'
end

--executing the procedure
exec proc_FirstProcedure


--creating the table
CREATE TABLE Products (
    id int identity(1,1) constraint pk_ProductId primary key,
	name nvarchar (100) not null,
	details nvarchar (max)

	)

-- creating the procedure for inserting the data in the table
create proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end

--calling the stored procedure
proc_InsertProduct 'Laptop','{"brand":"Dell","spec":{"ram":"16GB","cpu":"i5"}}'

select * from Products

create or alter proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end

--geting the json data
select JSON_QUERY(details, '$.spec') Product_Specification from products

--update procedure
create proc proc_UpdateProductSpec(@pid int,@newvalue varchar(20))
as
begin
   update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @pid
end

--calling the update procedure
proc_UpdateProductSpec 1, '24GB'

select * from Products;

--single value extraction from json
select id,name,JSON_VALUE(details,'$.brand') Brand_Name
from Products;


--BULK INSERT  using stored procedure
 create table Posts
  (id int primary key,
  title nvarchar(100),
  user_id int,
  body nvarchar(max))

select * from Posts;

create proc proc_BulInsertPosts(@jsondata nvarchar(max))
  as
  begin
	  insert into Posts(user_id,id,title,body)
	  select userId,id,title,body from openjson(@jsondata)
	  with (userId int,id int, title varchar(100), body varchar(max))
  end

  proc_BulInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }]'

SELECT * FROM POSTS;

-- bulk insert using Declare statement

-- 1. Create the table
CREATE TABLE Posts (
  id INT PRIMARY KEY,
  title NVARCHAR(100),
  user_id INT,
  body NVARCHAR(MAX)
);

-- 2. Declare the JSON data
DECLARE @jsondata NVARCHAR(MAX) = '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }
]';

-- 3. Insert using OPENJSON directly
INSERT INTO Posts(user_id, id, title, body)
SELECT userId, id, title, body 
FROM OPENJSON(@jsondata)
WITH (
  userId INT,
  id INT,
  title VARCHAR(100),
  body VARCHAR(MAX)
);

-- 4. Verify insertion
SELECT * FROM Posts;


-- try cast
SELECT *
FROM products
WHERE TRY_CAST(JSON_VALUE(details, '$.spec.cpu') AS NVARCHAR(20)) = 'i7';


-- create aprocedure that brings posts by taking the userid as parameter

create proc proc_GetPostsByUserId(@userid int)
as 
begin
    select * from Posts
    where user_id = @userid
end

proc_GetPostsByUserId '1'

-- out parameter

create proc proc_FilterProducts(@pcpu nvarchar(20) , @pcount int out)
as 
begin
	set @pcount = (select count(*) from Products 
	where
	TRY_CAST(JSON_VALUE(details,'$.spec.cpu') as nvarchar(20))= @pcpu) 
end

begin
declare @cnt int
exec proc_FilterProducts 'i5',@cnt out
print concat ('the numbers of cpu is', @cnt)
end

--bulk insert using stored procedure
create table people(
  id int primary key,
  name nvarchar(100),
  age int,
);

create or alter proc proc_BulkInsertPeople(@filepath nvarchar(max))
as 
begin
  declare @insertQuery nvarchar(max)

  set @insertQuery = 'BULK INSERT people FROM ''' + @filepath + ''' 
                      WITH (FIRSTROW=2 ,
                      FIELDTERMINATOR = '','', 
                      ROWTERMINATOR = ''\n'')'
                      exec sp_executesql @insertQuery
end

proc_BulkInsertPeople 'C:\GensparkTraining\PresidioTraining\Day-3\stored procedure\Data(in).csv'

--exception handling in stored procedure with try catch block


create table BulkInsertLog
(LogId int identity(1,1) primary key,
FilePath nvarchar(1000),
status nvarchar(50) constraint chk_status Check(status in('Success','Failed')),
Message nvarchar(1000),
InsertedOn DateTime default GetDate())


create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
  Begin try
	   declare @insertQuery nvarchar(max)

	   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
	   with(
	   FIRSTROW =2,
	   FIELDTERMINATOR='','',
	   ROWTERMINATOR = ''\n'')'

	   exec sp_executesql @insertQuery

	   insert into BulkInsertLog(filepath,status,message)
	   values(@filepath,'Success','Bulk insert completed')
  end try
  begin catch
		 insert into BulkInsertLog(filepath,status,message)
		 values(@filepath,'Failed',Error_Message())
  END Catch
end

proc_BulkInsert 'C:\GensparkTraining\PresidioTraining\Day-3\stored procedure\Data(in).csv'
truncate table people
select * from BulkInsertLog


