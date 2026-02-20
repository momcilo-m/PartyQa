@echo off
echo Seeding PartyDatabase...

docker exec -i party-db psql -U postgres -d PartyDatabase -a -f /tmp/truncate.sql
docker exec -i party-db psql -U postgres -d PartyDatabase -a -f /tmp/users.sql
docker exec -i party-db psql -U postgres -d PartyDatabase -a -f /tmp/parties.sql
docker exec -i party-db psql -U postgres -d PartyDatabase -a -f /tmp/partyattendances.sql
docker exec -i party-db psql -U postgres -d PartyDatabase -a -f /tmp/tasks.sql

echo Done.
pause