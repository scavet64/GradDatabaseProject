FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 52964
EXPOSE 44373

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["kinabalu/kinabalu.csproj", "kinabalu/"]
RUN dotnet restore "kinabalu/kinabalu.csproj"
COPY . .
WORKDIR "/src/kinabalu"
RUN dotnet build "kinabalu.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "kinabalu.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "kinabalu.dll"]