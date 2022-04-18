param(
    [Boolean]$JustRestart = $false,
    [String[]]$Services = @(),
    [Boolean]$OnlyPrebuildableServices = $false,
    [Boolean]$AllServices = $false,
    [Boolean]$RestartProxy = $true,
    [Boolean]$Recreate = $false
)

if ($OnlyPrebuildableServices)
{
    $Services = @(
    'articles',
    'person',
    'auth',
    'likes',
    'comments',
    'image'
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
    'image',
    'image-storage',
    'queue'
    )
}

if ($RestartProxy -And ! $Services.Contains('proxy'))
{
    $Services += 'proxy'
}

function RestartDocker
{
    param([String[]] $RestartableServices)
    $files = $( "-f `".\docker-compose.base.yaml`" -f `".\docker-compose.dev.yaml`" -f `".\docker-compose.local.yaml`"" )
    $flags = $( "-d --no-deps" )
    if ($Recreate)
    {
        $flags += " --force-recreate"
    }
    $serviceString = $( $RestartableServices | Join-String -Separator ' ' )
    $command = "docker compose $( $files ) up $( $flags ) $( $serviceString )"
    Invoke-Expression $command
}



function BuildServices
{
    param([String[]] $PrebuildableServices)
    $jobs = @()
    foreach ($service in [Linq.Enumerable]::Distinct($PrebuildableServices))
    {
        $args = @($service)
        $j = Start-Job -Args $args -ScriptBlock {
            $pathToRepository = switch ($args)
            {
                'articles' {
                    './src/articles-microservice'
                }
                'person' {
                    './src/person-microservice'
                }
                'auth' {
                    './src/auth-microservice'
                }
                'likes' {
                    './src/likes-microservice'
                }
                'comments' {
                    './src/comments-microservice'
                }
                'image' {
                    './src/image-microservice'
                }
                default {
                    $false
                }
            }
            if ($pathToRepository -ne $false)
            {
                $command = "$( $pathToRepository )/prebuild.Dockerfile.ps1 -PathToRepository `"$( $pathToRepository )/`""
                Invoke-Expression $command
            }
        }
        $jobs += $j
    }
    Wait-Job $jobs
    Receive-Job $jobs
}

if (!$JustRestart)
{
    BuildServices -PrebuildableServices $Services
}

RestartDocker -RestartableServices $Services