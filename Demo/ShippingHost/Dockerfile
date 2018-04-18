FROM microsoft/aspnetcore:latest
COPY . /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "ShippingHost.dll"]