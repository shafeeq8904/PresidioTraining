-- 1. In a transaction, if I perform multiple updates and an error happens in the third statement, but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK? Will my first two updates persist?

ans:  when using  a ROLLBACK will undo all operations in that transaction, including the first two successful updates.
BEGIN;

-- First update
UPDATE tbl_bank_accounts
SET balance = balance - 100
WHERE account_name = 'Alice';

-- Second update
UPDATE tbl_bank_accounts
SET balance = balance + 100
WHERE account_name = 'Bob';

-- Third update (intentional error: table name typo)
UPDATE tbl_bank_account
SET balance = balance + 50
WHERE account_name = 'Charlie';

-- Now rollback
ROLLBACK;

-- 2. Suppose Transaction A updates Alice’s balance but does not commit. Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?
No, Transaction B cannot read the new balance updated by Transaction A if the isolation level is READ COMMITTED, and Transaction A has not committed yet


-- 3. What will happen if two concurrent transactions both execute .UPDATE tbl\_bank\_accounts SET balance = balance - 100 WHERE account\_name = 'Alice'; at the same time? Will one overwrite the other?
 In PostgreSQL, if two concurrent transactions both execute the same update statement on the same row, one of them will succeed and the other will be blocked until the first transaction is either committed or rolled back.
 -- example
 --session-a
    BEGIN;
    SELECT balance FROM tbl_bank_accounts WHERE account_name = 'Alice';
    UPDATE tbl_bank_accounts 
    SET balance = balance - 100 
    WHERE account_name = 'Alice';
--session-b
    BEGIN;
    SELECT balance FROM tbl_bank_accounts WHERE account_name = 'Alice';
    UPDATE tbl_bank_accounts 
    SET balance = balance - 100 
    WHERE account_name = 'Alice';
--it wait session-a to commit or , once i commit session-a, session-b will be able to update the balance of Alice

-- 4.If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made after the savepoint or everything?
ans : It will only undo changes made after the savepoint. Any changes made before the savepoint will remain intact.

-- 5.Which isolation level in PostgreSQL prevents phantom reads?
ans: The SERIALIZABLE isolation level prevents phantom reads in PostgreSQL. In this isolation level, transactions are executed in such a way that they appear
--example

CREATE TABLE orders (
    id SERIAL PRIMARY KEY,
    customer_name TEXT,
    amount INT
);

INSERT INTO orders (customer_name, amount) VALUES
('Alice', 900),
('Bob', 1200),
('Charlie', 1100);

-- Transaction A
BEGIN TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SELECT * FROM orders WHERE amount > 1000;
-- Transaction B
INSERT INTO orders (customer_name, amount)
VALUES ('David', 1300);
COMMIT;
--transaction A will not see the new row inserted by Transaction B until it is committed, thus preventing phantom reads.
SELECT * FROM orders WHERE amount > 1000;


--6. Can Postgres perform a dirty read (reading uncommitted data from another transaction)?
ans: No, PostgreSQL does not allow dirty reads. It uses Multi-Version Concurrency Control (MVCC) to ensure that transactions only see committed data. If a transaction tries to read data that is being modified by another uncommitted transaction, it will wait until the other transaction is either committed or rolled back.

--7 . If autocommit is ON (default in Postgres), and I execute an UPDATE, is it safe to assume the change is immediately committed?

ans: Yes, if autocommit is ON, any changes made by an UPDATE statement are immediately committed to the database. There is no need to explicitly issue a COMMIT statement after the UPDATE.
-- No BEGIN, this happens automatically
UPDATE tbl_bank_accounts
SET balance = balance - 100
WHERE account_name = 'Alice';
-- This update is automatically committed, no need for COMMIT

BEGIN;
UPDATE tbl_bank_accounts
SET balance = balance - 100
WHERE account_name = 'Alice';
-- The changes are NOT committed yet, you must manually COMMIT
COMMIT;


/*--8️ Question:
If I do this:

BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- (No COMMIT yet)
And from another session, I run:

SELECT balance FROM accounts WHERE id = 1;
Will the second session see the deducted balance?
*/
 No, the second session will not see the deducted balance. It will see the original balance because the first transaction has not been committed yet. In PostgreSQL, uncommitted changes are not visible to other transactions.
 The second session will see the original balance
 