-- PostgreSQL Transactions and Concurrency Guide

-- ACID Properties
-- 1. Atomicity
-- 2. Consistency
-- 3. Isolation
-- 4. Durability

-- Why are Transactions Important?
-- Transactions help ensure reliable, consistent database changes and support concurrent access without data corruption.

-- Basic Transaction Commands:
-- BEGIN / START TRANSACTION
-- COMMIT
-- ROLLBACK
-- SAVEPOINT

-- Setup: Bank Accounts Table
CREATE TABLE tbl_bank_accounts (
    account_id SERIAL PRIMARY KEY,
    account_name VARCHAR(100),
    balance NUMERIC(10, 2)
);

-- Insert initial data
INSERT INTO tbl_bank_accounts (account_name, balance) VALUES
('Alice', 5000.00),
('Bob', 3000.00);

-- View data
SELECT * FROM tbl_bank_accounts;

-- 1. Successful Transaction
BEGIN;
UPDATE tbl_bank_accounts SET balance = balance - 500 WHERE account_name = 'Alice';
UPDATE tbl_bank_accounts SET balance = balance + 500 WHERE account_name = 'Bob';
COMMIT;
SELECT * FROM tbl_bank_accounts;

-- 2. Introduce an error and use ROLLBACK
BEGIN;
UPDATE tbl_bank_accounts SET balance = balance - 500 WHERE account_name = 'Alice';
-- Typo in table name to simulate error
UPDATE tbl_bank_account SET balance = balance + 500 WHERE account_name = 'Bob';
ROLLBACK;
SELECT * FROM tbl_bank_accounts;

-- 3. Savepoints and Partial Rollback
-- Example 1
BEGIN;
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
SAVEPOINT after_alice;
-- Typo again to simulate issue
UPDATE tbl_bank_account SET balance = balance + 100 WHERE account_name = 'Bob';
ROLLBACK TO after_alice;
UPDATE tbl_bank_accounts SET balance = balance + 100 WHERE account_name = 'Bob';
COMMIT;

-- Example 2: Auto-commit without BEGIN
UPDATE tbl_bank_accounts SET balance = balance + 100 WHERE account_name = 'Bob';

-- 4. Raise Notice and Transaction Check
BEGIN;
DO $$
DECLARE
  current_balance NUMERIC;
BEGIN
  SELECT balance INTO current_balance FROM tbl_bank_accounts WHERE account_name = 'Alice';
  IF current_balance >= 1500 THEN
    UPDATE tbl_bank_accounts SET balance = balance - 4500 WHERE account_name = 'Alice';
    UPDATE tbl_bank_accounts SET balance = balance + 4500 WHERE account_name = 'Bob';
  ELSE
    RAISE NOTICE 'Insufficient Funds!';
    ROLLBACK;
  END IF;
END;
$$;

-- 5. Auto-commit behavior
-- This update is auto-committed
UPDATE tbl_bank_accounts SET balance = balance - 4500 WHERE account_name = 'Alice';

-- Best Practices
-- 1. Keep transactions short
-- 2. Use SAVEPOINTs in complex flows
-- 3. Log failed transactions
-- 4. Avoid user inputs inside transactions
-- 5. Use try-catch with commit/rollback in production code

-- Concurrency Control in PostgreSQL
-- PostgreSQL uses MVCC (Multi-Version Concurrency Control)

-- Example: Read During Write
-- Trans A
BEGIN;
UPDATE products SET price = 500 WHERE id = 1;
-- Trans B
BEGIN;
SELECT price FROM products WHERE id = 1; -- Still sees old value

-- Isolation Levels
-- 1. Read Committed (Default)
-- 2. Repeatable Read
-- 3. Serializable

-- Dirty Read Scenario (theoretical in PostgreSQL; requires read uncommitted support)
-- Lost Update Example
-- Trans A
BEGIN;
SELECT balance FROM Accounts WHERE id = 1;
-- Trans B
BEGIN;
SELECT balance FROM Accounts WHERE id = 1;
UPDATE Accounts SET balance = 70 WHERE id = 1;
COMMIT;
-- Trans A
UPDATE Accounts SET balance = 150 WHERE id = 1;
COMMIT;

-- Avoiding Lost Updates
-- 1. SELECT ... FOR UPDATE
-- 2. Optimistic Locking using version column
-- 3. Serializable Isolation Level

-- Non-Repeatable Read
-- Trans A reads balance = 1000
-- Trans B updates balance = 800 and commits
-- Trans A reads again → balance = 800 (non-repeatable read)

-- Phantom Read Example
-- Trans A
BEGIN;
SELECT * FROM Accounts WHERE balance > 500;
-- Trans B inserts new row with balance 600 and commits
-- Trans A runs same query again and sees new row

-- Isolation Simulation Example
CREATE TABLE Accounts (
  ID INT PRIMARY KEY,
  balance INT
);
INSERT INTO Accounts VALUES (1, 1000);

-- Trans A
BEGIN;
UPDATE Accounts SET balance = 0 WHERE id = 1;
-- Trans B
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
BEGIN;
SELECT balance FROM Accounts WHERE id = 1; -- sees 0 (dirty read)
-- Trans A ROLLBACKs

-- END OF TRANSACTION AND CONCURRENCY GUIDE
