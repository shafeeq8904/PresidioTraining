-- CTE (Common Table Expression) is a temporary result set that you can reference within a SELECT, INSERT, UPDATE, or DELETE statement.`
-- CTEs are defined using the WITH clause and can be used to simplify complex queries, improve readability, and break down large queries into smaller, more manageable parts.
with cteAuthors
as
(select au_id, concat(au_fname,' ',au_lname) author_name from authors)

select * from cteAuthors