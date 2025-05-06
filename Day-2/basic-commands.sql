-- Basic SQL Commands

-- 1. SELECT statement to retrieve all columns from the titles table
select title from titles ;

-- 2. Print all the titles that have been published by 1389
SELECT title FROM titles
WHERE pub_id = '1389';

-- Print the books that have price in range of 10 to 15
SELECT title FROM titles
WHERE price BETWEEN 10 AND 15;

-- Print those books that have no price
SELECT title FROM titles
WHERE price is NULL;

-- Print the book names that starts with 'The'
SELECT title FROM titles
WHERE title LIKE 'The%';

-- Print the book names that do not have 'v' in their name
SELECT title FROM titles
WHERE title NOT LIKE '%v%';

-- print the books sorted by the royalty
SELECT title, royalty 
FROM titles 
ORDER BY royalty DESC;

-- print the books sorted by publisher in descending then by types in ascending then by price in descending
SELECT title 
FROM titles 
ORDER BY pub_id DESC, type ASC, price DESC;
 
-- Print the average price of books in every type
SELECT type, AVG(price) AS average_price
FROM titles
GROUP BY type;

-- print all the types in unique
SELECT DISTINCT type
FROM titles;

-- Print the first 2 costliest books
SELECT TOP 2 title, price
FROM titles
ORDER BY price DESC;

-- Print books that are of type business and have price less than 20 which also have advance greater than 7000
SELECT title, price, advance
FROM titles
WHERE type = 'business' AND price < 20 AND advance > 7000;

-- Select those publisher id and number of books which have price between 15 to 25 and have 'It' in its name. Print only those which have count greater than 2. Also sort the result in ascending order of count
SELECT pub_id, COUNT(*) AS book_count
FROM titles
WHERE price BETWEEN 15 AND 25 AND title LIKE '%It%'
GROUP BY pub_id
HAVING COUNT(*) > 2
ORDER BY book_count ASC;

--Print the Authors who are from 'CA'
SELECT au_id, au_fname, au_lname, state
FROM authors
WHERE state = 'CA';

-- Print the count of authors from every state
SELECT state, COUNT(*) AS author_count
FROM authors
GROUP BY state;




