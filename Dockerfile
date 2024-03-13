# Exmple of creating a SQL Server 2019 container image that will run as a user 'mssql' instead of root
# This is example is based on the official image from Microsoft and effectively changes the user that SQL Server runs as
# and allows for dumps to generate as a non-root user


FROM mcr.microsoft.com/mssql/server:2019-latest

EXPOSE 8080 80
