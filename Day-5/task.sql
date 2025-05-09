1.-- Write a cursor that loops through all films and prints titles longer than 120 minutes.
do $$
declare
    film_rec RECORD;
    film_cur cursor for
        select title, length from film;
begin
    open film_cur;
    loop
        fetch film_cur into film_rec;
        exit when not found;
		
        if film_rec.length > 120 then
            raise notice 'Title: %, Length: % mins', film_rec.title, film_rec.length;
        end if;
    end loop;
    close film_cur;
end $$;

2.-- Create a cursor that iterates through all customers and counts how many rentals each made.
do $$
declare
	customer_rec RECORD;
	customer_cur cursor for
		select c.customer_id,c.first_name,c.last_name,count(r.rental_id) as totalrental
		from customer c
		left join rental r
		on c.customer_id=r.customer_id
		group by c.customer_id;

begin
	open customer_cur;
	loop
		fetch customer_cur into customer_rec;
		 exit when not found;

		 raise notice 'Customer: %, Rentals: %', customer_rec.first_name || ' ' || customer_rec.last_name, customer_rec.totalrental;

		 END LOOP;

		 CLOSE customer_cur;
END $$;


3.--Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.
do $$ 
declare
    film_rec record;
    film_cur cursor for
        select f.film_id, f.rental_rate, count(r.rental_id) as rental_count
        from film f
        left join inventory i on f.film_id = i.film_id
        left join rental r on i.inventory_id = r.inventory_id
        group by f.film_id, f.rental_rate;
begin
    open film_cur;
    
    loop
        fetch film_cur into film_rec;
        exit when not found;
        
        if film_rec.rental_count < 5 then
            update film
            set rental_rate = film_rec.rental_rate + 1
            where film_id = film_rec.film_id;
            
            raise notice 'film id: %, updated rental rate: $%.', film_rec.film_id, film_rec.rental_rate + 1;
        end if;
    end loop;
    
    close film_cur;
end $$;

4.--Create a function using a cursor that collects titles of all films from a particular category.
create or replace function get_film_titles_by_category(category_name text)
returns table(title text) as $$
declare
    film_rec record;
    film_cur cursor for
        select f.title
        from film f
        join film_category fc on f.film_id = fc.film_id
        join category c on fc.category_id = c.category_id
        where c.name = category_name;
begin
    open film_cur;
    
    loop
        fetch film_cur into film_rec;
        exit when not found;
        
        title := film_rec.title;
        return next;
    end loop;
    
    close film_cur;
end;
$$ language plpgsql;

select * from get_film_titles_by_category('Comedy');

5.--Loop through all stores and count how many distinct films are available in each store using a cursor.
do $$
declare
    store_rec record;
    store_cur cursor for
        select s.store_id
        from store s;
    film_count int;
begin
    open store_cur;
    
    loop
        fetch store_cur into store_rec;
        exit when not found;
        
        select count(distinct i.film_id)
        into film_count
        from inventory i
        where i.store_id = store_rec.store_id;
        
        raise notice 'store id: %, distinct films: %', store_rec.store_id, film_count;
    end loop;
    
    close store_cur;
end $$;


6.--Write a trigger that logs whenever a new customer is inserted.
create table customer_log (
    log_id serial primary key,
    customer_id int,
    full_name text,
    log_time timestamp default current_timestamp
);

create or replace function log_new_customer()
returns trigger as $$
begin
    insert into customer_log(customer_id, full_name)
    values (new.customer_id, new.first_name || ' ' || new.last_name);
    return new;
end;
$$ language plpgsql;

create trigger trg_log_new_customer
after insert on customer
for each row
execute function log_new_customer();

insert into customer (customer_id, store_id, first_name, last_name, email, address_id, activebool, create_date, last_update)
values (600, 1, 'Shafeeq', 'Ahmed', 'shafeeqahmed@gmail.com', 10, true, current_timestamp, current_timestamp);

select * from customer_log;
select * from payment;

7.--Create a trigger that prevents inserting a payment of amount 0.
create or replace function prevent_zero_payment()
returns trigger as $$
begin
    if new.amount = 0 then
        raise exception 'Payment amount cannot be zero';
    end if;

    return new;
end;
$$ language plpgsql;

create trigger trg_prevent_zero_payment
before insert on payment
for each row
execute function prevent_zero_payment();

insert into payment (payment_id, customer_id, amount)
values (1, 1, 0);

8.-- Set up a trigger to automatically set last_update on the film table before update.
create or replace function set_last_update()
returns trigger as $$
begin
    
    new.last_update := current_timestamp;
    return new;
end;
$$ language plpgsql;

create trigger trg_set_last_update
before update on film
for each row
execute function set_last_update();

select film_id, title, last_update
from film
where film_id = 1;

update film
set title = 'mersal'
where film_id = 1;

9.-- Create a trigger to log changes in the inventory table (insert/delete).
create table inventory_logs (
    log_id serial primary key,
    inventory_id int,
    message text,
    log_time timestamp default current_timestamp
);

create or replace function fn_log_inventory_changes()
returns trigger
language plpgsql
as $$
begin
    if tg_op = 'INSERT' then
        insert into inventory_logs (inventory_id, message)
        values (new.inventory_id, 'inserted successfully');
    elseif tg_op = 'DELETE' then
        insert into inventory_logs (inventory_id, message)
        values (old.inventory_id, 'deleted successfully');
    end if;
    return new;  
end;
$$;

create trigger trg_log_inventory_changes
after insert or delete on inventory
for each row
execute function fn_log_inventory_changes();

insert into inventory (film_id, store_id) values (1, 1);
delete from inventory where inventory_id = (select max(inventory_id) from inventory);
select * from inventory_logs;

10.-- Write a trigger that ensures a rental can’t be made for a customer who owes more than $50

create or replace function fn_check_customer_pending()
returns trigger
language plpgsql
as $$
declare
    total_amount numeric;
begin
    
    select sum(amount) into total_amount from payment
    where customer_id = new.customer_id;

    
    if total_amount < 100 then
        raise exception 'Rental not allowed because customer(%) owes more than 100$', new.customer_id;
    end if;
    
    
    return new;
end;
$$;

create trigger trg_check_customer_pending
before insert on rental
for each row
execute function fn_check_customer_pending();

select customer_id, sum(amount) 
from payment 
group by customer_id
order by customer_id;

insert into rental (rental_date, inventory_id, customer_id, return_date, staff_id)
values (now(), 1, 1, null, 1);

select *  from customer
order by customer_id desc

11.-- Write a transaction that inserts a customer and an initial rental in one atomic operation.

BEGIN;


INSERT INTO customer (store_id, first_name, last_name, email, activebool, create_date, last_update)
VALUES (1, 'shafeeq', 'ahmed', 'shafeeqahmed@example.com', true, current_timestamp, current_timestamp);

INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id)
VALUES (current_timestamp, 1, (SELECT currval(pg_get_serial_sequence('customer', 'customer_id'))), NULL, 1);

COMMIT;

--12. Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.
BEGIN;

UPDATE film
SET rental_rate = 6.99
WHERE film_id = 1;

INSERT INTO inventory (film_id, store_id)
VALUES (NULL, 1); 


ROLLBACK;