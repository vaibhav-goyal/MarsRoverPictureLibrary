FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/MarsRover.PictureLibrary/MarsRover.PictureLibrary.csproj", "src/MarsRover.PictureLibrary/"]
RUN dotnet restore "src/MarsRover.PictureLibrary/MarsRover.PictureLibrary.csproj"
COPY . .
WORKDIR "/src/src/MarsRover.PictureLibrary"
RUN dotnet build "MarsRover.PictureLibrary.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MarsRover.PictureLibrary.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MarsRover.PictureLibrary.dll"]