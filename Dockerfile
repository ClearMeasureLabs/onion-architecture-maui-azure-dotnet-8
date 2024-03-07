FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
EXPOSE 8080 80
WORKDIR /app
COPY /built/ /app

RUN ls -lsa /app

RUN mkdir /sqlcmd
RUN apt-get update
RUN apt-get install -y --no-install-recommends curl gpg apt-transport-https wget
RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
RUN curl https://packages.microsoft.com/config/debian/12/prod.list | tee /etc/apt/sources.list.d/mssql-release.list
RUN apt-get update
#RUN apt-get install -y mssql-tools18 unixodbc-dev


RUN set -eux; \
  apt-get update; \
  apt-get install -y --no-install-recommends \
    apt-transport-https \
    wget\
    gnupg \
    wget \
    curl \
  ; \
  curl -SL --progress-bar https://packages.microsoft.com/keys/microsoft.asc | apt-key add -; \
  curl -SL --progress-bar https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list; \
  apt-get update; \
  ACCEPT_EULA=Y apt-get install -y msodbcsql18; \
  apt-get install -y unixodbc-dev; \
  apt-get install -y mssql-tools18; \
  sed -i -E 's/(CipherString\s*=\s*DEFAULT@SECLEVEL=)2/\11/' /etc/ssl/openssl.cnf; \
  rm -rf /var/lib/apt/lists/*;


USER $APP_UID
ENTRYPOINT ["dotnet", "ProgrammingWithPalermo.ChurchBulletin.UI.Server.dll"]