param($docker_enviroment = 'dev');

Get-ChildItem .\ -include bin, obj -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

$ErrorActionPreference = "Stop"

$docker_compose_rm_command = '';

if ($docker_enviroment -eq 'dev')
{
    $files = "-f 'docker-compose.base.yaml' -f 'docker-compose.dev.yaml' -f 'docker-compose.local.yaml'";

    $docker_compose_rm_command = "docker compose $files rm -s -f -v";
}
else
{
    throw "invalid enviroment: '${docker_enviroment}'";
}

Write-Host "Executing romoveing command: ${docker_compose_rm_command}";
Invoke-Expression $docker_compose_rm_command;