
rm -rf ./volumes/*
mkdir volumes/
mkdir volumes/auth-storage
mkdir volumes/person-storage
mkdir volumes/auth-storage/postgres
mkdir volumes/person-storage/neo4j
mkdir volumes/auth-storage/postgres/data
mkdir volumes/auth-storage/postgres/logs
mkdir volumes/person-storage/neo4j/data
mkdir volumes/person-storage/neo4j/logs

find . -iname "bin" -print0 | xargs -0 rm -rf
find . -iname "obj" -print0 | xargs -0 rm -rf

docker compose -f docker-compose.base.yaml -f docker-compose.dev.yaml rm -f --all
