--incontri ai gironi

declare @IdTorneo int;
set @IdTorneo = 10;

select count(*) from GironiIncontri where IdTorneo >= @IdTorneo


