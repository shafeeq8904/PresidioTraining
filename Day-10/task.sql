CREATE TABLE students (
    student_id SERIAL PRIMARY KEY,
    full_name TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE,
    phone_no TEXT NOT NULL
);

CREATE TABLE courses(
	course_id SERIAL PRIMARY KEY,
	course_name TEXT NOT NULL,
    category TEXT NOT NULL,
    duration_days INT NOT NULL CHECK (duration_days > 0)
);

CREATE TABLE trainers (
    trainer_id SERIAL PRIMARY KEY,
    trainer_name TEXT NOT NULL,
    expertise TEXT NOT NULL
);

CREATE TABLE enrollments (
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    enroll_date DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (student_id) REFERENCES students(student_id),
    FOREIGN KEY (course_id) REFERENCES courses(course_id),
    UNIQUE (student_id, course_id)
);

CREATE TABLE certificates (
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT UNIQUE NOT NULL,
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    serial_no UUID UNIQUE DEFAULT gen_random_uuid(),
    FOREIGN KEY (enrollment_id) REFERENCES enrollments(enrollment_id)
);

CREATE TABLE course_trainers (
    course_id INT NOT NULL,
    trainer_id INT NOT NULL,
    PRIMARY KEY (course_id, trainer_id),
    FOREIGN KEY (course_id) REFERENCES courses(course_id),
    FOREIGN KEY (trainer_id) REFERENCES trainers(trainer_id)
);

--insert some values
INSERT INTO students (full_name, email, phone_no) VALUES
('shafeeq ahmed','shafeeq89@gmail.com','1234567890'),
('harish','harish@gmail.com','0987654321');

INSERT INTO courses (course_name, category, duration_days) VALUES
('aws', 'Cloud', 30),
('c-sharp', 'programming', 45);

INSERT INTO trainers (trainer_name, expertise) VALUES
('karthick', 'aws'),
('piyush', 'c-sharp');

INSERT INTO course_trainers (course_id, trainer_id) VALUES
(1, 1), 
(2, 2); 

INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES
(1, 1, '2025-05-10'),
(2, 2, '2025-05-11');

INSERT INTO certificates (enrollment_id, issue_date) VALUES
(1, '2025-06-10'),
(2, '2025-06-15');  

--creating indexs
CREATE INDEX idx_students_student_id ON students(student_id);
CREATE INDEX idx_students_email ON students(email);
CREATE INDEX idx_courses_course_id ON courses(course_id);

--tables visualisation
select * from trainers;
select * from certificates;
select * from students;
select * from courses;
select * from enrollments;

--joins
--list the name and the course
SELECT 
    s.full_name AS student_name,
    c.course_name,
    e.enroll_date
FROM 
    enrollments e
JOIN students s ON e.student_id = s.student_id
JOIN courses c ON e.course_id = c.course_id
ORDER BY s.full_name;

-- Show students who received certificates with trainer names
SELECT 
    s.full_name AS student_name,
    c.course_name,
    t.trainer_name,
    cert.issue_date,
    cert.serial_no
FROM 
    certificates cert
JOIN enrollments e ON cert.enrollment_id = e.enrollment_id
JOIN students s ON e.student_id = s.student_id
JOIN courses c ON e.course_id = c.course_id
JOIN course_trainers ct ON c.course_id = ct.course_id
JOIN trainers t ON ct.trainer_id = t.trainer_id
ORDER BY cert.issue_date DESC;


--count the number of students per course

select c.course_name , count(e.student_id) as total_count
from courses c
left join enrollments e
on c.course_id = e.course_id
group by c.course_name;


--functions
--Create `get_certified_students(course_id INT)`
-- Returns a list of students who completed the given course and received certificates.
create or replace function get_certified_students(course_id INT)
returns table(
	student_id int,
	student_full_name TEXT,
    course_name TEXT,
    issue_date DATE,
    serial_no UUID
)
as $$
begin
	return query
	select 
		s.student_id,
        s.full_name,
        c.course_name,
        cert.issue_date,
        cert.serial_no
	from certificates cert
	join enrollments e on cert.enrollment_id = e.enrollment_id
    join students s on e.student_id = s.student_id
    join courses c on e.course_id = c.course_id
    where 
        c.course_id = get_certified_students.course_id;
end;
$$ language plpgsql;

select * from get_certified_students(2);

--stored procedure

create or replace procedure sp_enroll_student(
    p_student_id int,
    p_course_id int,
    p_is_completed boolean default false
)
language plpgsql
as $$
declare
    v_enrollment_id int;
begin
    insert into enrollments (student_id, course_id, enroll_date)
    values (p_student_id, p_course_id, current_date)
    returning enrollment_id into v_enrollment_id;

    raise notice 'Student enrolled successfully.';

    if p_is_completed then
        insert into certificates (enrollment_id, issue_date)
        values (v_enrollment_id, current_date);

        raise notice 'Certificate issued.';
    end if;
end;
$$;


call sp_enroll_student(3, 2, true);
select * from enrollments;
select * from certificates;

--Use a cursor to:
--* Loop through all students in a course
--* Print name and email of those who do not yet have certificates

create or replace procedure list_students_without_cert(course_id_input int)
language plpgsql
as $$
declare
    rec record;
    cur cursor for
        select s.student_id, s.full_name, s.email, e.enrollment_id
        from students s
        join enrollments e on s.student_id = e.student_id
        where e.course_id = course_id_input;
begin
    open cur;

    loop
        fetch cur into rec;
        exit when not found;
        if not exists (
            select 1 from certificates
            where enrollment_id = rec.enrollment_id
        ) then
            raise notice 'Name: %, Email: %', rec.full_name, rec.email;
        end if;
    end loop;

    close cur;
end;
$$;
delete from certificates
where enrollment_id = 5;

call list_students_without_cert(2);  

-- Phase 6: Security & Roles
-- 1) Create a `readonly_user` role:
--   * Can run `SELECT` on `students`, `courses`, and `certificates`
--   * Cannot `INSERT`, `UPDATE`, or `DELETE`
CREATE ROLE readonly_user LOGIN PASSWORD 'readonly_pass';
GRANT CONNECT ON DATABASE "Edtech" TO readonly_user;
GRANT USAGE ON SCHEMA public TO readonly_user;
GRANT SELECT ON students,trainers,courses,enrollments,certificates,course_trainers TO readonly_user;
REVOKE INSERT, UPDATE, DELETE ON students,trainers,courses,enrollments,certificates,course_trainers FROM readonly_user;

-- 2. Create a `data_entry_user` role:
--   * Can `INSERT` into `students`, `enrollments`
--   * Cannot modify certificates directly
CREATE ROLE data_entry_user LOGIN PASSWORD 'data_entry_pass';
GRANT CONNECT ON DATABASE "Edtech" TO data_entry_user;
GRANT USAGE ON SCHEMA public TO data_entry_user;
GRANT SELECT ON students,trainers,courses,enrollments,certificates,course_trainers TO data_entry_user;
GRANT INSERT ON students,enrollments TO data_entry_user;
REVOKE UPDATE, DELETE ON students,trainers,courses,enrollments,certificates,course_trainers FROM data_entry_user;
REVOKE ALL ON certificates FROM data_entry_user;

--Phase 7: Transactions & Atomicity
--Write a transaction block that:
--* Enrolls a student
--* Issues a certificate
--* Fails if certificate generation fails (rollback)
create or replace procedure sp_enroll_and_certify(
    p_student_id int,
    p_course_id int
)
language plpgsql
as $$
declare
    v_enrollment_id int;
begin

    insert into enrollments (student_id, course_id, enroll_date)
    values (p_student_id, p_course_id, current_date)
    returning enrollment_id into v_enrollment_id;

    insert into certificates (enrollment_id)
    values (v_enrollment_id);

exception
    when others then
        raise exception 'Transaction failed: %', sqlerrm;
end;
$$;

call sp_enroll_and_certify(1, 1);