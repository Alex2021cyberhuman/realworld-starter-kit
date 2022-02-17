Invoke-Expression -Command "git pull"
Set-Location -Path src
Set-Location -Path articles-microservice
Invoke-Expression -Command "git pull"
Set-Location -Path shared-core
Invoke-Expression -Command "git pull"
Set-Location -Path ../
Set-Location -Path ../

Set-Location -Path auth-microservice
Invoke-Expression -Command "git pull"
Set-Location -Path shared-core
Invoke-Expression -Command "git pull"
Set-Location -Path ../
Set-Location -Path ../

Set-Location -Path likes-microservice
Invoke-Expression -Command "git pull"
Set-Location -Path shared-core
Invoke-Expression -Command "git pull"
Set-Location -Path ../
Set-Location -Path ../

Set-Location -Path person-microservice
Invoke-Expression -Command "git pull"
Set-Location -Path shared-core
Invoke-Expression -Command "git pull"
Set-Location -Path ../
Set-Location -Path ../
Set-Location -Path ../
