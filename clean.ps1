docker compose -f docker-compose.base.yaml -f docker-compose.dev.yaml -f docker-compose.local.yaml rm -s -f --all -v

$volumes = @(
"./volumes/auth-storage/mongo/data"
"./volumes/auth-storage/mongo/logs"
"./volumes/person-storage/neo4j/data"
"./volumes/person-storage/neo4j/logs"
"./volumes/articles-storage/postgres/data"
"./volumes/articles-storage/postgres/logs"
"./volumes/comments/web-api/logs"
"./volumes/comments-storage/postgres/data"
"./volumes/comments-storage/postgres/logs"
"./volumes/likes-storage/redis/data"
"./volumes/likes-storage/redis/logs"
"./volumes/queue/rabbitmq/data/"
"./volumes/queue/rabbitmq/log/"
)
Remove-Item $volumes -Force -Recurse
New-Item $volumes -ItemType Directory

dotnet clean
dotnet restore
