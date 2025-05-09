select * from actor;
select * from film;
select * from film_actor;
select * from customer;
select * from rental;
select * from payment;

--1.List all films with their length and rental rate, sorted by length descending.
--Columns: title, length, rental_rate
select title,length,rental_rate from film
order by length desc;

--2.Find the top 5 customers who have rented the most films.
select c.customer_id, concat(c.first_name, c.last_name) as fullname, count(r.rental_id) AS rental_count
from customer c
join rental r 
on c.customer_id = r.customer_id
group by c.customer_id
order by rental_count desc
limit 5;

--3.Display all films that have never been rented.
select f.film_id, f.title
from film f
left join inventory i 
on f.film_id = i.film_id
left join rental r on i.inventory_id = r.inventory_id
where r.rental_id is null;

--4.List all actors who appeared in the film ‘Academy Dinosaur’.
select first_name,last_name ,title
from actor a
join film_actor fa
on a.actor_id = fa.actor_id
join film f
on fa.film_id = f.film_id
where title='Academy Dinosaur'

--5.List each customer along with the total number of rentals they made and the total amount paid.
select c.customer_id, c.first_name, c.last_name,count(r.rental_id) AS total_rentals,sum(p.amount) AS total_paid
from customer c
join rental r 
on c.customer_id = r.customer_id
join payment p 
on r.rental_id = p.rental_id
group by c.customer_id;

--6.Using a CTE, show the top 3 rented movies by number of rentals.
with RentalCounts as (
  select f.title, count(r.rental_id) as rental_count
  from film f
  join inventory i 
  on f.film_id = i.film_id
  join rental r 
  on i.inventory_id = r.inventory_id
  group by f.title
)
select * from RentalCounts
order by rental_count desc
limit 3;

--7.Find customers who have rented more than the average number of films.
--Use a CTE to compute the average rentals per customer, then filter
with CustomerRentals AS (
  select customer_id, count(*) AS rental_count
  from rental
  group by customer_id
),
AverageRentals AS (
  select avg(rental_count) as avg_rentals
  from CustomerRentals
)
select cr.customer_id, cr.rental_count
from CustomerRentals cr, AverageRentals ar
where cr.rental_count > ar.avg_rentals;

--8.Write a function that returns the total number of rentals for a given customer ID.
--Function: get_total_rentals(customer_id INT)
create OR replace function get_total_rentals(customer_id INT)
returns INT
as $$
declare
    total INT;
begin
    select count(*) into total
    from rental
    where rental.customer_id = get_total_rentals.customer_id;
    return total;
END;
$$ LANGUAGE plpgsql;

select get_total_rentals(1);

--Write a stored procedure that updates the rental rate of a film by film ID and new rate.
--Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)
create OR replace procedure update_rental_rate(film_id INT, new_rate NUMERIC)
AS $$
begin
    update film
    set rental_rate = new_rate
    where film.film_id = update_rental_rate.film_id;
end;
$$ language plpgsql;
CALL update_rental_rate(133, 5.99);

--Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
--Procedure: get_overdue_rentals() that selects relevant columns.
CREATE OR REPLACE PROCEDURE get_overdue_rentals()
LANGUAGE plpgsql
as $$
begin
	SELECT 
        r.rental_id,
        r.rental_date,
        r.customer_id,
        f.title,
        r.return_date
    FROM rental r
    JOIN inventory i ON r.inventory_id = i.inventory_id
    JOIN film f ON i.film_id = f.film_id
    WHERE r.return_date IS NULL AND r.rental_date < CURRENT_DATE - INTERVAL '7 days';
end
$$
 
CALL get_overdue_rentals();
