param(
[Boolean]$Restart = $true,
[Boolean]$NoBuild = $false
)

if ($NoBuild -eq $false) 
{
    ./src/articles-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/articles-microservice/
    ./src/auth-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/auth-microservice/
    ./src/person-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/person-microservice/    
}

if ($Restart) 
{
    docker compose  -f docker-compose.base.yaml -f docker-compose.dev.yaml -f docker-compose.local.yaml stop articles auth person
    docker compose  -f docker-compose.base.yaml -f docker-compose.dev.yaml -f docker-compose.local.yaml create --force-recreate --build articles auth person
    docker compose  -f docker-compose.base.yaml -f docker-compose.dev.yaml -f docker-compose.local.yaml restart articles auth person proxy
}