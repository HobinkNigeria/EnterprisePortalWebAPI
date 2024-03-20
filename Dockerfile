FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Define user properties
ARG USERNAME=alphasystems
ARG USER_UID=1000
ARG USER_GID=$USER_UID

WORKDIR /app
EXPOSE 8080
ARG environment

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=$environment

# Create a non-root user 

RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    && apt-get update \
    && apt-get install -y sudo=1.9.5p2-3+deb11u1 \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

# Set the working directory and grant permissions to the user 
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EnterprisePortalWebAPI/EnterprisePortalWebAPI.csproj", "EnterprisePortalWebAPI/"]
RUN dotnet restore "EnterprisePortalWebAPI/EnterprisePortalWebAPI.csproj"
COPY . .
WORKDIR "/src/EnterprisePortalWebAPI"
RUN dotnet build "EnterprisePortalWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnterprisePortalWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Switch to the non-root user 
USER $USERNAME
ENTRYPOINT ["dotnet", "EnterprisePortalWebAPI.dll"]