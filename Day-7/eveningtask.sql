select * from customer;
select * from store;
select * from inventory;
select * from film;
select * from payment;

CREATE TABLE customer_summary (
    customer_id INT PRIMARY KEY,
    full_name TEXT,
    rental_count INT
);

do $$
declare
    customer_rec record;
    rental_count int;
begin
    
    for customer_rec in
        select customer_id, lower(first_name || ' ' || last_name) as full_name
        from customer
    loop
        select count(*) into rental_count
        from rental
        where customer_id = customer_rec.customer_id;

        insert into customer_summary (customer_id, full_name, rental_count)
        values (customer_rec.customer_id, customer_rec.full_name, rental_count);
    end loop;
end;
$$;

SELECT * FROM customer_summary
ORDER BY rental_count DESC
LIMIT 10;

--Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.

do $$
declare
    film_rec record;
begin
    for film_rec in
        select f.title, count(r.rental_id) as rental_count
        from film f
        join film_category fc on f.film_id = fc.film_id
        join category c on fc.category_id = c.category_id
        join inventory i on f.film_id = i.film_id
        join rental r on i.inventory_id = r.inventory_id
        where lower(c.name) = 'comedy'
        group by f.film_id, f.title
        having count(r.rental_id) > 10
    loop
        raise notice 'Title: %, Rentals: %', film_rec.title, film_rec.rental_count;
    end loop;
end;
$$;


--Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.
create table film_countReport (
    store_id int,
    store_name text,
    total_films int
);
do $$
declare
    store_rec record;
    film_count int;
begin
    for store_rec in
        select s.store_id, concat(lower(st.first_name), ' ', lower(st.last_name)) as store_name
        from store s
        join staff st on st.staff_id = s.manager_staff_id
    loop
        select count(distinct i.film_id) into film_count
        from inventory i
        where i.store_id = store_rec.store_id;

        insert into film_countReport(store_id, store_name, total_films)
        values (store_rec.store_id, store_rec.store_name, film_count);
    end loop;
end $$;

select * from film_countReport

--Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive\_customers table.


create table inactive_customers (
    customer_id int primary key,
    full_name text,
    email varchar(50),
    last_rental_date date
);
do $$
declare
    cust_rec record;
    last_rental_date date;
begin
    for cust_rec in
        select c.customer_id, c.first_name, c.last_name, c.email
        from customer c
    loop
        select max(r.rental_date)
        into last_rental_date
        from rental r
        where r.customer_id = cust_rec.customer_id;
		
        if last_rental_date is null or last_rental_date < current_date - interval '6 months' then
            insert into inactive_customers (customer_id, full_name, email, last_rental_date)
            values (
                cust_rec.customer_id,
                lower(cust_rec.first_name || ' ' || cust_rec.last_name),
                cust_rec.email,
                last_rental_date
            );
        end if;
    end loop;

    raise notice 'Inactive customers inserted successfully.';
end $$;

select * from inactive_customers order by last_rental_date ;

-----------------------------------------------------------------------------------------------
-- 1. Create a trigger to prevent inserting payments of zero or negative amount.

create or replace function prevent_invalid_payment()
returns trigger as $$
begin
    if NEW.amount <= 0 then
        raise exception 'Payment amount must be greater than zero';
    end if;
    return new;
end;
$$ language plpgsql;

create trigger trg_prevent_invalid_payment
before insert on payment
for each row
execute function prevent_invalid_payment();

------------------------------------------------------------------------------
--2.Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.

create or replace function update_film_last_update()
returns trigger as $$
begin
    if NEW.title is distinct from OLD.title or
       NEW.rental_rate is distinct from OLD.rental_rate then
        NEW.last_update := current_timestamp;
    end if;
    return new;
end;
$$ language plpgsql;

create trigger trg_update_film_last_update
before update on film
for each row
execute function update_film_last_update();

select title, rental_rate, last_update from film where film_id = 1;
update film
set rental_rate = rental_rate + 5.00
where film_id = 1;

-----------------------------------------------------------------------------------------------
--3. Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week

create table rental_log (
    log_id SERIAL primary key,
    film_id INT,
    rental_count INT,
    log_date TIMESTAMP default CURRENT_TIMESTAMP
);
create or replace function log_high_rental()
returns trigger as $$
declare
    rental_count INT;
begin
    select count(*)
    into rental_count
    from rental r
    JOIN inventory i ON r.inventory_id = i.inventory_id
    where i.film_id = NEW.inventory_id AND r.rental_date >= NOW() - INTERVAL '7 days';

    if rental_count > 3 then
        insert into rental_log (film_id, rental_count)
        values (NEW.inventory_id, rental_count);
    end if;

    return new;
end;
$$ language plpgsql;

create trigger trg_log_high_rental
after insert on rental
for each row
execute function log_high_rental();

----------------------------------------------------------------------------------------
-- transactions
-- 1) write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.
begin;
with new_customer as (
    insert into customer (store_id, first_name, last_name, email, address_id, active, create_date) 
    values (1, 'john', 'doe', 'john.doe@example.com', 5, 1, now())
    returning customer_id
),
new_rental as (
    insert into rental (rental_date, inventory_id, customer_id, staff_id) 
    select now(), 1000, customer_id, 1 from new_customer
    returning rental_id, customer_id
)

insert into payment (customer_id, staff_id, rental_id, amount, payment_date)
select customer_id, 1, rental_id, 4.99, now() from new_rental;
commit;

select * from customer order by customer_id desc;
select * from payment order by customer_id desc;
rollback;

-- 2) simulate a transaction where one update fails (e.g., invalid rental id), and ensure the entire transaction rolls back.
do $$
begin
    begin
        update customer set last_name = 'doe' where customer_id = (select max(customer_id) from customer);

        update rental set rental_date = now() where customer_id = -999;
        commit;
    exception when others then
        rollback;
        raise 'transaction rollbacked because of error';
    end;
end;
$$

-- 3) use savepoint to update multiple payment amounts. roll back only one payment update using rollback to savepoint.
begin;
savepoint payment1;
update payment set amount = amount - 5 where payment_id = 17507;

savepoint payment2;
update payment set amount = amount - 5 where payment_id = -999;
rollback to savepoint payment2;

savepoint payment3;
update payment set amount = amount + 5 where payment_id = 17508;

commit;

select * from payment where payment_id in (17507, 17508);

-- 4) perform a transaction that transfers inventory from one store to another (delete + insert) safely.
begin;
    delete from payment where rental_id in (select rental_id from rental where inventory_id = 1001);
    delete from rental where inventory_id = 1001;
with deleted_inventory as (
    delete from inventory where inventory_id = 1001
    returning film_id)

insert into inventory (film_id, store_id, last_update)
select film_id, 2, now() from deleted_inventory;

commit;
-- rollback

-- 5) create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.
begin;
delete from payment where customer_id = 524;
delete from rental where customer_id = 524;
delete from customer where customer_id = 524;
commit;


