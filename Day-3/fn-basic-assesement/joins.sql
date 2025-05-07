--How can you produce a list of the start times for bookings by members named 'David Farrell'?
select starttime from cd.bookings b
join cd.members m
on b.memid=m.memid
where m.firstname='David' and m.surname='Farrell'


--How can you produce a list of the start times for bookings for tennis courts, for the date '2012-09-21'? Return a list of start time and facility name pairings, ordered by the time.
select starttime as start,name
from cd.bookings b
join cd.facilities f
on b.facid = f.facid
WHERE name LIKE 'Tennis%' AND starttime BETWEEN '2012-09-21 00:00:00' AND '2012-09-21 23:59:59'
order by start

--How can you output a list of all members who have recommended another member? Ensure that there are no duplicates in the list, and that results are ordered by (surname, firstname).
select distinct m.firstname,m.surname
from cd.members as m
join cd.members as r
on m.memid = r.recommendedby
order by m.surname, m.firstname



