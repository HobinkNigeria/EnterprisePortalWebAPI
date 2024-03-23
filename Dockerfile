# Use the specified version of ASP.NET Core as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Define user properties
# ARG USERNAME=alphasystems
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Set the working directory and grant permissions to the user 
WORKDIR /app
EXPOSE 5000
ARG environment

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=$environment

# Switch to the non-root user 
USER $USERNAME

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EnterprisePortalWebAPI/EnterprisePortalWebAPI.csproj", "EnterprisePortalWebAPI/"]
RUN dotnet restore "EnterprisePortalWebAPI/EnterprisePortalWebAPI.csproj"
COPY . .
WORKDIR "/src/EnterprisePortalWebAPI"
RUN dotnet build "EnterprisePortalWebAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "EnterprisePortalWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "EnterprisePortalWebAPI.dll"]