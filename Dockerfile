FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BlazoeToDoList/BlazoeToDoList.csproj", "BlazoeToDoList/"]
RUN dotnet restore "BlazoeToDoList/BlazoeToDoList.csproj"
COPY . .
WORKDIR "/src/BlazoeToDoList"
RUN dotnet build "BlazoeToDoList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazoeToDoList.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazoeToDoList.dll"]
