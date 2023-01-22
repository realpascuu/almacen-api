# Get Base Image (Full .NET Core SDK)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
LABEL Remarks="GET SDK FROM MICROSOFT"
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else
COPY . ./
# and build (in production mode)
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
#EXPOSE 5000

# copy build data (in production mode)
COPY --from=build-env /app/out .
EXPOSE 80

# in production mode
ENTRYPOINT ["dotnet", "almacenAPI.dll"]

