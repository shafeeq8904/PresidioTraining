-- Transaction 1
BEGIN;

UPDATE tbl_bank_accounts
SET balance = balance - 100
WHERE account_name = 'Alice';
-- Transaction 1 has locked the row for Alice
--commit

-- Transaction 2
--it waits for transaction 1 to finish or commit
BEGIN;

UPDATE tbl_bank_accounts
SET balance = balance - 100
WHERE account_name = 'Alice';


-- 3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.

DROP TABLE IF EXISTS products;
CREATE TABLE products (
  id SERIAL PRIMARY KEY,
  name TEXT,
  price NUMERIC
);

INSERT INTO products (name, price)
VALUES ('Item1', 100), ('Item2', 200);

BEGIN;

-- Lock Row 1
UPDATE products
SET price = price + 10
WHERE id = 1;
-- Now Transaction A holds lock on row id = 1
BEGIN;
-- Lock Row 2
UPDATE products
SET price = price + 10
WHERE id = 2;
-- Now Transaction B holds lock on row id = 2
-- Try to lock row 2 (already locked by B)
UPDATE products
SET price = price + 20
WHERE id = 2;
-- Try to lock row 1 (already locked by A)
UPDATE products
SET price = price + 20
WHERE id = 1;

--5. 5. Explore about Lock Modes.

| Lock Mode                | Description                                      | Blocks            |
| ------------------------ | ------------------------------------------------ | ----------------- |
| `ACCESS SHARE`           | Default for `SELECT`                             | Nothing           |
| `ROW SHARE`              | Acquired with `SELECT ... FOR UPDATE`            | `EXCLUSIVE`, etc. |
| `ROW EXCLUSIVE`          | For `INSERT`, `UPDATE`, `DELETE`                 | Other writes      |
| `SHARE UPDATE EXCLUSIVE` | Internal use (e.g., VACUUM)                      | Some writes       |
| `SHARE`                  | For operations needing read-only but stable rows | Writes            |
| `SHARE ROW EXCLUSIVE`    | Rare; blocks reads & writes selectively          | Many              |
| `EXCLUSIVE`              | Strong; allows only SELECT                       | Most              |
| `ACCESS EXCLUSIVE`       | Strongest; blocks everything                     | All               |
