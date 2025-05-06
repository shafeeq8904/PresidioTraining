/*
categories
id, name, status
 
country
id, name
 
state
id, name, country_id
 
City
id, name, state_id
 
area
zipcode, name, city_id
 
address
id, door_number, addressline1, zipcode
 
supplier
id, name, contact_person, phone, email, address_id, status
 
product
id, Name, unit_price, quantity, description, image
 
product_supplier
transaction_id, product_id, supplier_id, date_of_supply, quantity,
 
Customer
id, Name, Phone, age, address_id
 
order
  order_number, customer_id, Date_of_order, amount, order_status
 
order_details
  id, order_number, product_id, quantity, unit_price

*/
 
 CREATE TABLE categories (
     id INT PRIMARY KEY,
     name VARCHAR(100),
     status VARCHAR(20)
 );

 CREATE TABLE country (
     id INT PRIMARY KEY,
     name VARCHAR(100)
 );

 CREATE TABLE state (
     id INT PRIMARY KEY,
     name VARCHAR(100),
     country_id INT,
     FOREIGN KEY (country_id) REFERENCES country(id)
 );

 CREATE TABLE city (
     id INT PRIMARY KEY,
     name VARCHAR(100),
     state_id INT,
     FOREIGN KEY (state_id) REFERENCES state(id)
 );

 CREATE TABLE area (
     zipcode VARCHAR(10) PRIMARY KEY,
     name VARCHAR(100),
     city_id INT,
     FOREIGN KEY (city_id) REFERENCES city(id)
);

CREATE TABLE address (
     id INT PRIMARY KEY,
     door_number VARCHAR(20),
     addressline1 VARCHAR(100),
     zipcode VARCHAR(10),
     FOREIGN KEY (zipcode) REFERENCES area(zipcode)
 );

 CREATE TABLE supplier (
     id INT PRIMARY KEY,
     name VARCHAR(100),
     contact_person VARCHAR(100),
     phone VARCHAR(20),
     email VARCHAR(100),
     address_id INT,
     status VARCHAR(20),
     FOREIGN KEY (address_id) REFERENCES address(id)
 );

 CREATE TABLE product (
     id INT PRIMARY KEY,
     name VARCHAR(100),
     unit_price DECIMAL(10,2),
     quantity INT,
     description TEXT,
     image VARCHAR(255)
 );

 CREATE TABLE product_supplier (
     transaction_id INT PRIMARY KEY,
     product_id INT, /*Composite key if we have product_id & supplier_id as primary key */
     supplier_id INT,
     date_of_supply DATETIME,
     quantity INT,
     FOREIGN KEY (product_id) REFERENCES product(id),
     FOREIGN KEY (supplier_id) REFERENCES supplier(id)
);

CREATE TABLE customer (
     id INT PRIMARY KEY,
     name VARCHAR(100),
     phone VARCHAR(20),
     age INT,
     address_id INT,
     FOREIGN KEY (address_id) REFERENCES address(id)
 );

CREATE TABLE orders (
    order_number INT PRIMARY KEY,
    customer_id INT,
    date_of_order DATETIME,
    amount DECIMAL(10,2),
    order_status VARCHAR(20),
    FOREIGN KEY (customer_id) REFERENCES customer(id)
);

CREATE TABLE order_details (
    id INT PRIMARY KEY,
    order_number INT,
    product_id INT,
    quantity INT,
    unit_price DECIMAL(10,2),
    FOREIGN KEY (order_number) REFERENCES orders(order_number),
    FOREIGN KEY (product_id) REFERENCES product(id)
);



