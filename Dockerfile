FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
EXPOSE 80
WORKDIR /app

ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/Europe/Moscow /etc/localtime && echo Europe/Moscow > /etc/timezone

COPY PersonPublish/ .

ENTRYPOINT ["dotnet", "Person.Server.dll"]