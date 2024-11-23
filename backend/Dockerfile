# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the backend project files
COPY backend/*.csproj ./backend/
WORKDIR /app/backend
RUN dotnet restore

# Copy the rest of the backend files and build
COPY backend/. .
RUN dotnet publish -c Release -o out

# Use the runtime-only image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/backend/out .

# Expose the port your application listens on
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "backend.dll"]


