$volumes =  @(
"volumes/auth-storage/mongo/data"
"volumes/auth-storage/mongo/logs"
"volumes/person-storage/neo4j/data"
"volumes/person-storage/neo4j/logs"
)
rm $volumes -Force -Recurse
ni $volumes -ItemType Directory

ls -Include bin,obj -Recurse | foreach ($_) { rm $_.FullName -Force -Recurse; echo "removed $($_.FullName)" }

iex "docker compose -f docker-compose.base.yaml -f docker-compose.dev.yaml -f docker-compose.local.yaml rm -f --all"

