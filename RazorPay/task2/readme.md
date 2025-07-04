# docker pull mysql:latest

# step:2
docker run -d \
  --name mysql-container \
  -e MYSQL_ROOT_PASSWORD=12345 \
  -e MYSQL_DATABASE=testdb \
  -v mydbdata:/var/lib/mysql \
  -p 3306:3306 \
  mysql:latest

# step:3
  docker exec -it mysql-container mysql -uroot -p12345

# step:4

USE testdb;

CREATE TABLE greetings (
  id INT AUTO_INCREMENT PRIMARY KEY,
  message VARCHAR(100)
);

INSERT INTO greetings (message) VALUES ('Hello World');

SELECT * FROM greetings;

# step:5

docker stop mysql-container
docker rm mysql-container

# step:6
docker run -d \
  --name mysql-container \
  -e MYSQL_ROOT_PASSWORD=12345 \       
  -v mydbdata:/var/lib/mysql \
  -p 3306:3306 \
  mysql:latest

 #  step:7 
docker exec -it mysql-container mysql -uroot -p12345 