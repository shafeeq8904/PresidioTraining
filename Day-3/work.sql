select title , pub_name
from titles  join publishers
on titles.pub_id = publishers.pub_id

select title , pub_name
from titles right outer join publishers
on titles.pub_id = publishers.pub_id

select * from authors
select * from jobs;
select * from publishers;
select * from pub_info;
select * from titles;
select * from stores;
select * from sales;
select * from titleauthor;

-- select the author_id for all books . print the author_id
select title , au_id
from titles  join titleauthor
on titles.title_id = titleauthor.title_id;

select concat(au_fname ,' ',au_lname) Author_Name , title Book_Name 
from authors a
join titleauthor ta
on a.au_id = ta.au_id
join titles t
on ta.title_id= t.title_id;


select pub_name PublisherName , title BookName ,ord_date OrderDate
from publishers p
join titles t
on p.pub_id = t.pub_id
join sales s
on t.title_id = s.title_id;
 

 -- print the publisher name and the first book sales date for all the publisher

SELECT p.pub_name Publisher_Name, MIN(s.ord_date) First_Sale_Date
FROM publishers p
LEFT JOIN titles t ON p.pub_id = t.pub_id
LEFT JOIN sales s ON t.title_id = s.title_id
GROUP BY p.pub_name
order by  2 desc

 -- print the book name and the store address

select title Book_Name, stor_address Store_Address 
from titles
JOIN sales ON titles.title_id = sales.title_id 
JOIN stores ON sales.stor_id = stores.stor_id;


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