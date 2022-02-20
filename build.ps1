param(
    [Boolean]$RestartMyServices = $true,
    [Boolean]$NoBuild = $false,
    [Boolean]$RestartAllServices = $false
)

if ($NoBuild -eq $false)
{
    ./src/articles-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/articles-microservice/
    ./src/auth-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/auth-microservice/
    ./src/person-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/person-microservice/
    ./src/likes-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/likes-microservice/
    ./src/comments-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/comments-microservice/
}

if ($RestartAllServices)
{
    docker compose `
        -f docker-compose.base.yaml `
        -f docker-compose.dev.yaml `
        -f docker-compose.local.yaml stop
    docker compose `
        -f docker-compose.base.yaml `
        -f docker-compose.dev.yaml `
        -f docker-compose.local.yaml create
    docker compose `
        -f docker-compose.base.yaml `
        -f docker-compose.dev.yaml `
        -f docker-compose.local.yaml restart
}

if ($RestartMyServices -and -not$RestartAllServices)
{
    docker compose `
        -f docker-compose.base.yaml `
        -f docker-compose.dev.yaml `
        -f docker-compose.local.yaml `
        stop articles auth person comments likes
    docker compose `
        -f docker-compose.base.yaml `
        -f docker-compose.dev.yaml `
        -f docker-compose.local.yaml `
        create --force-recreate --build articles auth person comments likes
    docker compose `
        -f docker-compose.base.yaml `
        -f docker-compose.dev.yaml `
        -f docker-compose.local.yaml `
        restart articles auth person proxy comments likes
}