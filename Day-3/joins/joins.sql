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

