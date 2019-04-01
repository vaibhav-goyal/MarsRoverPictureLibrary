# MarsRoverPictureLibrary

## Prerequisites
- [.NET Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- [Node and NPM](https://nodejs.org/en/download/) 

## How to Build and Run
1. From root run `dotnet restore`
2. From root run `dotnet build --no-restore`
3. Go To `.\src\MarsRover.PictureLibrary\client-app`
4. Run `npm install`
5. Run `npm build`
6. Open new command prompt and go to `.\src\MarsRover.PictureLibrary`
7. Run `dotnet run --no-build`
8. Open in browser http://localhost:5001

TODO : Publish intructions.

## Run Unit Tests
1. From root run `dotnet restore`
2. From root run `dotnet build --no-restore`
3. From root run `dotnet test --no-build`


## Run in Docker
1. Open new command prompt and go to `.\src\MarsRover.PictureLibrary`
2. Run `docker build --target "final" -t "marsrover.pl" .`
3. Run `docker run -d -p 8080:80 --name marsrover.pl marsrover.pl`


## TODO
1. Logging
2. Global Exception Handling
3. Unit Tests

