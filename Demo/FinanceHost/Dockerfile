FROM microsoft/aspnetcore:latest
COPY . /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "FinanceHost.dll"]