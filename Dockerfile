FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build

COPY . .

RUN dotnet publish consoleapp/devrating.consoleapp.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

RUN apk --no-cache add git

COPY --from=build out/ /app

WORKDIR /workspace

ENTRYPOINT ["/app/devrating.consoleapp"]