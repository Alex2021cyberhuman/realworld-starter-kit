FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ARG ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ARG CONFIG=Debug
ENV CONFIG=$CONFIG
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ARG CONFIG=Debug
ENV CONFIG=$CONFIG
WORKDIR /src
COPY ["./Conduit.Articles.PresentationLayer/Conduit.Articles.PresentationLayer.csproj", "Conduit.Articles.PresentationLayer/"]
COPY ["./Conduit.Articles.BusinessLogicLayer/Conduit.Articles.BusinessLogicLayer.csproj", "Conduit.Articles.BusinessLogicLayer/"]
COPY ["./shared-core/Conduit.Shared/Conduit.Shared.csproj", "./shared-core/Conduit.Shared/"]
COPY ["./Conduit.Articles.DomainLayer/Conduit.Articles.DomainLayer.csproj", "Conduit.Articles.DomainLayer/"]
COPY ["./Conduit.Articles.DataAccessLayer/Conduit.Articles.DataAccessLayer.csproj", "Conduit.Articles.DataAccessLayer/"]
WORKDIR "/src/Conduit.Articles.PresentationLayer/"
RUN dotnet restore "Conduit.Articles.PresentationLayer.csproj"
WORKDIR "/src/"
COPY . .
WORKDIR "/src/Conduit.Articles.PresentationLayer/"
RUN dotnet build "Conduit.Articles.PresentationLayer.csproj" -c $CONFIG -o /app/build

FROM build AS publish
RUN dotnet publish "Conduit.Articles.PresentationLayer.csproj" -c $CONFIG -o /app/publish --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conduit.Articles.PresentationLayer.dll"]
HEALTHCHECK --timeout=120s --retries=120 CMD curl --fail http://localhost/health || exit