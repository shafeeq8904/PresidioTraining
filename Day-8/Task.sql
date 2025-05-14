C:\Windows\System32>initdb -D "C:\pri"
C:\Windows\System32>initdb -D "C:\sec"

--start server
C:\Windows\System32>pg_ctl -D "C:\pri" -o "-p 5433" -l "C:\pri\logfile" start
--creating role
C:\Windows\System32>psql -p 5433 -d postgres -c "CREATE ROLE replicator WITH REPLICATION LOGIN PASSWORD 'repl_pass';"

C:\Windows\System32>pg_basebackup -D "C:\sec" -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433
24245/24245 kB (100%), 1/1 tablespace

--start sec server
C:\Windows\System32>pg_ctl -D "C:\sec" -o "-p 5435" -l "C:\sec\logfile" start

--log in to primary server
C:\Windows\System32>psql -p 5433 -U shafeeqahmeds  -d postgres

--create table adn insert in primary server
postgres=# CREATE TABLE testrep (id INT);
CREATE TABLE
postgres=# INSERT INTO testrep VALUES (1), (2);
INSERT 0 2

--check if the table is replicated
C:\Windows\System32>psql -p 5435 -U shafeeqahmeds -d postgres

postgres=# SELECT * FROM testrep;


--Create a table on the primary:
CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    rental_time TIMESTAMP,
    customer_id INT,
    film_id INT,
    amount NUMERIC,
    logged_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

--Write a stored procedure to:
--Insert a new rental log entry

CREATE OR REPLACE PROCEDURE sp_add_rental_log(
    p_customer_id INT,
    p_film_id INT,
    p_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (rental_time, customer_id, film_id, amount)
    VALUES (CURRENT_TIMESTAMP, p_customer_id, p_film_id, p_amount);
EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error occurred: %', SQLERRM;
END;
$$;

--Call the procedure on the primary:
CALL sp_add_rental_log(1, 100, 4.99);

--On the standby (port 5435):
Run:SELECT * FROM rental_log ORDER BY log_id DESC LIMIT 1;
