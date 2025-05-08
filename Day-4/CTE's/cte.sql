-- CTE (Common Table Expression) is a temporary result set that you can reference within a SELECT, INSERT, UPDATE, or DELETE statement.`
-- CTEs are defined using the WITH clause and can be used to simplify complex queries, improve readability, and break down large queries into smaller, more manageable parts.
with cteAuthors
as
(select au_id, concat(au_fname,' ',au_lname) author_name from authors)

select * from cteAuthors

--pagination using CTE
declare @page int =2, @pageSize int=10;
with PaginatedBooks as
( select  title_id,title, price, ROW_Number() over (order by price desc) as RowNum
  from titles
)
select * from PaginatedBooks where rowNUm between((@page-1)*@pageSize) and (@page*@pageSize)

--using stored procedure to get the page size and page number
create or alter proc proc_PaginateTitles( @page int =1, @pageSize int=10)
as
begin
with PaginatedBooks as
( select  title_id,title, price, ROW_Number() over (order by price desc) as RowNum
  from titles
)
select * from PaginatedBooks where RowNum between((@page-1)*(@pageSize+1)) and (@page*@pageSize)
end

exec proc_PaginateTitles 2,5

 select  title_id,title, price
  from titles
  order by price desc
  offset 10 rows fetch next 10 rows only

  --scalar value function 

  create function  fn_CalculateTax(@baseprice float, @tax float)
  returns float
  as
  begin
     return (@baseprice +(@baseprice*@tax/100))
  end

  select dbo.fn_CalculateTax(1000,10)

  select title,dbo.fn_CalculateTax(price,12) from titles

--Table valued function
--Table-valued functions return a table data type and can be used in the FROM clause of a query, just like a regular table.
  
  create function fn_tableSample(@minprice float)
  returns table
  as
    return select title,price from titles where price>= @minprice

	select * from dbo.fn_tableSample(10)
