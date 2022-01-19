docker compose -f docker-compose.base.yaml -f docker-compose.dev.yaml -f docker-compose.local.yaml rm -s -f --all -v

$volumes = @(
"volumes/auth-storage/mongo/data"
"volumes/auth-storage/mongo/logs"
"volumes/articles-storage/postgres/data"
"volumes/articles-storage/postgres/logs"
"volumes/person-storage/neo4j/data"
"volumes/person-storage/neo4j/logs"
"src/articles-microservice/app/publish"
"src/auth-microservice/app/publish"
"src/person-microservice/app/publish"
)
Remove-Item $volumes -Force -Recurse
New-Item $volumes -ItemType Directory

dotnet clean
dotnet restore
