--1. Create a stored procedure to encrypt a given text
--Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
 --Use pgp_sym_encrypt(text, key) from pgcrypto.
create extension if not exists pgcrypto;
create or replace function sp_encrypt_text(p_text TEXT , p_key TEXT)
returns BYTEA
language plpgsql
as $$
declare 
	encrypted BYTEA;
begin
	encrypted := pgp_sym_encrypt(p_text, p_key);
	return encrypted;
end;
$$;
SELECT sp_encrypt_text('shafeeq89@gmail.com', '12345');

--Create a stored procedure to compare two encrypted texts
--Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text
create or replace function sp_compare_encrypted (p_enc1 BYTEA , p_enc2 BYTEA , p_key TEXT)
returns boolean
language plpgsql
as $$
declare 
	decrypted1 TEXT;
    decrypted2 TEXT;
begin 
	decrypted1 := pgp_sym_decrypt(p_enc1, p_key);
    decrypted2 := pgp_sym_decrypt(p_enc2, p_key);
    return decrypted1 = decrypted2;
end;
$$;
SELECT sp_compare_encrypted(
  sp_encrypt_text('shafeeq89@gmail.com', '12345'),
  sp_encrypt_text('shafeeq89@gmail.com', '12345'),
  '12345'
);

--3. Create a stored procedure to partially mask a given text
--Task: Write a procedure sp_mask_text that:
 --Shows only the first 2 and last 2 characters of the input string
create or replace function sp_mask_text(p_text TEXT)
returns TEXT
language plpgsql
as $$
declare
    masked TEXT;
    text_length INT;
begin
    text_length := length(p_text);
    if text_length <= 4 then
        return p_text;
    end if;

    masked := substring(p_text from 1 for 2) || 
              repeat('*', text_length - 4) || 
              substring(p_text from text_length - 1 for 2);

    return masked;
end;
$$;
select sp_mask_text('shafeeq89@gmail.com');

--4. Create a procedure to insert into customer with encrypted email and masked name

CREATE TABLE customer_encrypted (
    customer_id SERIAL PRIMARY KEY,
    first_name VARCHAR(100), 
    last_name VARCHAR(50),
    email BYTEA,        
    create_date DATE DEFAULT CURRENT_DATE
);
create or replace  procedure sp_insert_customer_secure(
    p_first_name TEXT,
    p_last_name TEXT,
    p_email TEXT,
    p_key TEXT
)
language plpgsql
as $$
declare
    v_masked_name TEXT;
    v_encrypted_email BYTEA;
begin
    v_masked_name := sp_mask_text(p_first_name);
    v_encrypted_email := sp_encrypt_text(p_email, p_key);

    insert into customer_encrypted (first_name, last_name, email, create_date)
    values (v_masked_name, p_last_name, v_encrypted_email, CURRENT_DATE);

    raise notice 'Customer inserted successfully.';
end;
$$;
call sp_insert_customer_secure(
    'Shafeeq',
    'Ahmed',
    'shafeeq89@gmail.com',
    '12345'
);
select * from customer_encrypted;

--5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
--Task:
--Write sp_read_customer_masked() that:
create or replace procedure sp_read_customer_masked(p_key text)
language plpgsql
as $$
declare
    v_customer_id int;
    v_first_name text;
    v_email bytea;
    v_decrypted_email text;

    cursor_customer cursor for
        select customer_id, first_name, email from customer_encrypted;
begin
    open cursor_customer;
	loop
        fetch cursor_customer into v_customer_id, v_first_name, v_email;
        exit when not found;
        v_decrypted_email := pgp_sym_decrypt(v_email, p_key);
		raise notice 'ID: %, Name: %, Email: %', v_customer_id, v_first_name, v_decrypted_email;
    end loop;

    close cursor_customer;
end;
$$;
call sp_read_customer_masked('12345');



