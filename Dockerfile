FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY . .

RUN dotnet publish ConsoleApp/DevRating.ConsoleApp.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine

RUN apk --no-cache add git

COPY --from=build out/ /app

WORKDIR /workspace

ENTRYPOINT ["dotnet", "/app/DevRating.ConsoleApp.dll"]