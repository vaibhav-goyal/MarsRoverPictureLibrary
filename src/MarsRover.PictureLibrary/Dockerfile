FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80


FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["./*.csproj", "/src"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app --no-restore

FROM node:8.11 as node-build
WORKDIR /client-app
COPY ./client-app .
RUN npm install
RUN npm run build

FROM build AS publish
RUN dotnet publish -c Release -o /app --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
WORKDIR  /app/client-app/build
COPY --from=node-build /client-app/build .
WORKDIR /app
ENTRYPOINT ["dotnet", "MarsRover.PictureLibrary.dll"]