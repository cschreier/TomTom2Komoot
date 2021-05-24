FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o /publish -r linux-arm

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /publish ./
ENV TZ=Europe/Berlin
ENV LANG de_DE.UTF-8
ENV LANGUAGE de_DE.UTF-8
ENV LC_ALL de_DE.UTF-8
RUN chmod +x TomTom2Komoot
ENTRYPOINT [ "dotnet", "TomTom2Komoot.dll" ]