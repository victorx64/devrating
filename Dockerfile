FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

COPY . .

RUN dotnet publish ConsoleApp/DevRating.ConsoleApp.csproj -c Release -o out

RUN rm -rf out/runtimes/linux-arm \
    out/runtimes/linux-arm64 \
    out/runtimes/linux-armel \
    out/runtimes/linux-x64 \
    out/runtimes/linux-x86 \
    out/runtimes/osx \
    out/runtimes/osx-x64 \
    out/runtimes/win-arm \
    out/runtimes/win-arm64 \
    out/runtimes/win-x64 \
    out/runtimes/win-x86

FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine

RUN apk --no-cache add git

COPY --from=build out/ /app

WORKDIR /workspace

ENTRYPOINT ["dotnet", "/app/DevRating.ConsoleApp.dll"]