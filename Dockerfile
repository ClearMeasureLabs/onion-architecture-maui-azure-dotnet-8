FROM mcr.microsoft.com/mssql/server:latest AS runtime
EXPOSE 8080 80
#WORKDIR /app
#COPY /built/ /app

#RUN ls -lsa /app


#USER $APP_UID
#ENTRYPOINT ["dotnet", "ProgrammingWithPalermo.ChurchBulletin.UI.Server.dll"]