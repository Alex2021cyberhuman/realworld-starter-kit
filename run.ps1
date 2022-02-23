param(
    [Boolean]$JustRestart = $false,
    [String[]]$Services = @(),
    [Boolean]$OnlyDotnetServices = $false,
    [Boolean]$AllServices = $false,
    [Boolean]$RestartProxy = $true
)

if ($OnlyDotnetServices)
{
    $Services = @(        
        'articles',
        'person',
        'auth',
        'likes',
        'comments'
    )
}

if ($AllServices)
{
    $Services = @(        
        'articles',
        'articles-storage',
        'person',
        'person-storage',
        'auth',
        'auth-storage',
        'likes',
        'likes-storage',
        'comments',
        'comments-storage',
        'queue'
    )
}

function RestartDocker
{
    param([String] $Service)
    docker compose -f ".\docker-compose.base.yaml" -f ".\docker-compose.dev.yaml" -f ".\docker-compose.local.yaml" up --force-recreate --no-deps -d $Service
}

if ($Services.Contains('articles') -And ! $JustRestart)
{
    ./src/articles-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/articles-microservice/    
}

if ($Services.Contains('person') -And ! $JustRestart)
{
    ./src/person-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/person-microservice/    
}

if ($Services.Contains('auth') -And ! $JustRestart)
{
    ./src/auth-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/auth-microservice/    
}

if ($Services.Contains('likes') -And ! $JustRestart)
{
    ./src/likes-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/likes-microservice/    
}

if ($Services.Contains('comments') -And ! $JustRestart)
{
    ./src/comments-microservice/prebuild.Dockerfile.ps1 -PathToRepository ./src/comments-microservice/    
}

foreach($service in $Services)
{
    RestartDocker -Service $service
}

if ($RestartProxy)
{
    RestartDocker -Service 'proxy'
}