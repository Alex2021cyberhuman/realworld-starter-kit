param($enviroment='dev', $need_build=1);

$ErrorActionPreference = "Stop"

$docker_compose_build_command='';
$docker_compose_up_command='';

if($enviroment -eq 'dev') {
    $files="-f 'docker-compose.base.yaml' -f 'docker-compose.dev.yaml' -f 'docker-compose.local.yaml'";
    if ($need_build -eq 1) {
        $docker_compose_build_command="docker compose $files  build --progress=plain";
    }
    $docker_compose_up_command="docker compose $files  up"
}
else {
    throw "invalid enviroment: '${enviroment}'";
}

Write-Host "Executing building command: ${docker_compose_build_command}";
Invoke-Expression $docker_compose_build_command;

Write-Host "Executing upping command: ${docker_compose_up_command}";
Invoke-Expression $docker_compose_up_command;
