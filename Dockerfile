   # Use the .NET SDK image for building the application
   FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

   # Set the working directory inside the container
   WORKDIR /src

   # Copy the project file and restore dependencies
   COPY ["UrlShortener.csproj", "./"]
   RUN dotnet restore UrlShortener.csproj

   # Copy the rest of the application files
   COPY . .

   # Publish the application
   RUN dotnet publish UrlShortener.csproj -c Release -o /app/publish

   # Use the ASP.NET runtime image for the final image
   FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
   WORKDIR /app

   # Copy the published output from the build stage
   COPY --from=build /app/publish .

   # Expose the port your application listens on (e.g., 80 for HTTP)
   EXPOSE 80

   # Set the environment variable to configure Kestrel to listen on all interfaces (necessary for Docker)
   ENV ASPNETCORE_URLS=http://*:80

   # Define the entry point for the container
   ENTRYPOINT ["dotnet", "UrlShortener.dll"]